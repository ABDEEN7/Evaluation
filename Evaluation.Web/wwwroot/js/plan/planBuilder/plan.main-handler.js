(function (global) {
    'use strict';

    function t(key, fallback = '') {
        const text = uiControlsSetup()?.GetUiControlText(key);
        return text || key;
    }

    const ns = global.planUtility;
    const {
        API_ENDPOINTS,
        PLAN_TYPE_BACKEND,
        TABLE_CONFIG,
        FILTER_FIELDS
    } = global.PlanConstants;

    /* ===================== jqClient ===================== */
    if (typeof window.jqClient !== 'function') {
        window.jqClient = function () {
            return {
                Get: url => $.ajax({ url, method: 'GET', dataType: 'json' }),
                Post: (url, data) => $.ajax({
                    url, method: 'POST',
                    data: JSON.stringify(data),
                    contentType: 'application/json',
                    dataType: 'json'
                })
            };
        };
    }

    /* ===================== INSTANCES MAP ===================== */
    const instances = new Map();

    /* ===================== INSTANCE STATE ===================== */
    function createInstanceState(fieldId) {
        return {
            fieldId: fieldId,
            planId: null,
            isReadOnly: false,
            pageSize: TABLE_CONFIG.pageSize || 10,
            currentPage: 1,
            totalRecords: 0,
            filters: {},
            searchTerm: '',
            selectedSchools: [],
            selectedSchoolsMap: null
        };
    }

    /* ===================== HELPERS ===================== */
    const pid = (fieldId, id) => `#${fieldId}_${id}`;
    const pidRaw = (fieldId, id) => `${fieldId}_${id}`;
    const $p = (fieldId, id) => window.jQuery(pid(fieldId, id));

    /* ===================== INIT ===================== */
    const init = async (isReadOnly, fieldId = null, planObject = null, element) => {
        if (!fieldId) { console.error('[PlanHandler] fieldId is required!'); return; }
        if (planObject) planObject = toCamelCaseKeys(planObject);

        const state = createInstanceState(fieldId);
        state.isReadOnly = isReadOnly;
        if (planObject && planObject.id) state.planId = planObject.id;
        instances.set(fieldId, state);

        try {
            if (!ns.planTypes || ns.planTypes.length === 0) {
                await Promise.all([
                    loadPlanTypes(),
                    loadSemesters(),
                    loadVisitTypes(),
                    loadVacationDays(),
                    loadParentOrgTree(),
                    // ✅ NEW: تحميل المرحلة والجنس والشعبة
                    loadSchoolLevels(),
                    loadGenders(),
                    loadDepartments()
                ]);
            }

            if (planObject) {
                renderPlanWithData(fieldId, planObject, element);
            } else {
                renderNewPlan(fieldId);
                populatePlanTypes(fieldId, element);
                populateSemesters(fieldId, element);
            }

            bindEvents(fieldId);
            initializeFilterDatePickers(fieldId);
            populateFilterVisitTypes(fieldId);
            populateFilterParentOrgTree(fieldId);
            // ✅ NEW
            populateFilterSchoolLevels(fieldId);
            populateFilterGenders(fieldId);
            populateFilterDepartments(fieldId);

        } catch (e) {
            console.error(`[PlanHandler] Init failed for ${fieldId}`, e);
            alert('حدث خطأ أثناء التحميل');
        }
    };

    /* ===================== LOAD DATA ===================== */
    const loadPlanTypes = () => jqClient().Get(API_ENDPOINTS.GET_PLAN_TYPES).then(r => ns.planTypes = r?.result || []);
    const loadSemesters = () => jqClient().Get(API_ENDPOINTS.GET_SEMESTERS).then(r => ns.semesters = r?.result || []);
    const loadVisitTypes = () => jqClient().Get(API_ENDPOINTS.GET_VISITS).then(r => ns.visitTypes = r?.result || []);
    const loadParentOrgTree = () => jqClient().Get(API_ENDPOINTS.GET_PARNT_ORGTREE).then(r => ns.parentSchool = r?.result || []);

    // ✅ NEW: تحميل بيانات الفلاتر الجديدة
    const loadSchoolLevels = () => jqClient().Get(API_ENDPOINTS.GET_SCHOOL_LEVELS).then(r => ns.schoolLevels = r?.result || []);
    const loadGenders = () => jqClient().Get(API_ENDPOINTS.GET_GENDERS).then(r => ns.genders = r?.result || []);
    const loadDepartments = () => jqClient().Get(API_ENDPOINTS.GET_DEPARTMENTS).then(r => ns.departments = r?.result || []);

    const loadVacationDays = () =>
        jqClient().Get(API_ENDPOINTS.GET_VACATION_DATES)
            .then(r => ns.holidays = (r || []).map(x => ({
                start: new Date(x.startDate),
                end: new Date(x.endDate),
                isCron: x.isCronExpression,
                cron: x.cronExpression
            })));

    const loadPlanData = async (fieldId, planId) => {
        const r = await jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`);
        renderPlanWithData(fieldId, r.result);
    };

    /* ===================== POPULATE ===================== */
    const populatePlanTypes = (fieldId, elementId = null) => {
        const parentElement = elementId ? $(`#${elementId}`) : null;
        const $s = $(`#${fieldId ? fieldId + '_' : ''}ddlPlanType`);
        if (!$s.length) { console.warn('ddlPlanType not found'); return; }
        if ($s.data('select2')) { try { $s.select2('destroy'); } catch { } }
        $s.empty().append(`<option value="">${t('lblChoosePlanType')}</option>`);
        (ns.planTypes || []).forEach(pt =>
            $s.append(`<option value="${pt.id}" data-backendname="${pt.backendName}">${pt.name}</option>`)
        );
        const $modal = $s.closest('.modal');
        const dropdownParent = (parentElement?.length) ? parentElement : ($modal.length ? $modal : $(document.body));
        $s.select2({ width: '100%', allowClear: true, dropdownParent });
    };

    const populateSemesters = (fieldId, elementId = null) => {
        const parentElement = elementId ? $(`#${elementId}`) : null;
        const $s = $p(fieldId, 'ddlSemester');
        if ($s.hasClass('select2-hidden-accessible')) $s.select2('destroy');
        $s.empty().append(`<option value="">${t('lblChooseSemester')}</option>`);
        ns.semesters.forEach(s =>
            $s.append(`<option value="${s.id}" data-startdate="${s.startDate}" data-enddate="${s.endDate}">${s.name}</option>`)
        );
        const $modal = $s.closest('.modal');
        const dropdownParent = (parentElement?.length) ? parentElement : ($modal.length ? $modal : $(document.body));
        $s.select2({ width: '100%', allowClear: true, dropdownParent });
    };

    const populateFilterVisitTypes = (fieldId) => {
        const $select = $p(fieldId, 'filterVisitType');
        ns.visitTypes.forEach(type => $select.append(`<option value="${type.id}">${type.name}</option>`));
    };

    const populateFilterParentOrgTree = (fieldId) => {
        const $select = $p(fieldId, 'filterParentOrgTree');
        ns.parentSchool.forEach(p => $select.append(`<option value="${p.id}">${p.nameEn}</option>`));
    };

    // ✅ NEW: populate المرحلة والجنس والشعبة
    const populateFilterSchoolLevels = (fieldId) => {
        const $select = $p(fieldId, 'filterSchoolLevel');
        (ns.schoolLevels || []).forEach(lvl =>
            $select.append(`<option value="${lvl.id}">${lvl.name}</option>`)
        );
    };

    const populateFilterGenders = (fieldId) => {
        const $select = $p(fieldId, 'filterGender');
        (ns.genders || []).forEach(g =>
            $select.append(`<option value="${g.id}">${g.name}</option>`)
        );
    };

    const populateFilterDepartments = (fieldId) => {
        const $select = $p(fieldId, 'filterDepartment');
        (ns.departments || []).forEach(d =>
            $select.append(`<option value="${d.id}">${d.name}</option>`)
        );
    };

    const initializeFilterDatePickers = (fieldId) => {
        const dateFields = ['filterLastEvalDate', 'filterCreatedDate', 'filterNextEvalDate'];
        dateFields.forEach(field => {
            const $input = $p(fieldId, field);
            if ($input.length && typeof flatpickr !== 'undefined') {
                flatpickr($input[0], { locale: 'en', dateFormat: 'Y-m-d', allowInput: true });
            }
        });
    };

    /* ===================== RENDER ===================== */
    const renderNewPlan = (fieldId) => {
        const state = instances.get(fieldId);
        const form = ns.renderPlanForm(fieldId, null, state.isReadOnly);
        $p(fieldId, 'planFormContainer').find('.form-container').html(form);
        loadSchools(fieldId, 1);
        initCustomMode(fieldId);
    };

    const renderPlanWithData = (fieldId, plan, element) => {
        const state = instances.get(fieldId);
        if (plan && plan.id) state.planId = plan.id;

        const vm = {
            title: plan.name || '',
            planTypeId: plan.planTypeDepId || plan.PlanTypeDepId,
            semesterId: plan.semesterId,
            dateRange: toDateOnly(plan.startDate) && plan.endDate
                ? `${toDateOnly(plan.startDate)} to ${toDateOnly(plan.endDate)}` : '',
            schools: plan.schools || []
        };

        const form = ns.renderPlanForm(fieldId, vm, state.isReadOnly);
        $p(fieldId, 'planFormContainer').find('.form-container').html(form);

        populatePlanTypes(fieldId, element);
        populateSemesters(fieldId, element);
        $p(fieldId, 'semesterContainer').hide();

        $p(fieldId, 'planTitle').val(vm.title);
        $p(fieldId, 'ddlPlanType').val(vm.planTypeId).trigger('change');
        if (vm.semesterId) $p(fieldId, 'ddlSemester').val(vm.semesterId).trigger('change');
        $p(fieldId, 'parentDate').val(vm.dateRange);

        const selectedSchoolsMap = new Map();
        const selectedSchoolIds = [];

        vm.schools.forEach(s => {
            const schoolId = s.id || s.schoolId;
            selectedSchoolIds.push(schoolId);
            selectedSchoolsMap.set(schoolId, {
                id: schoolId,
                visitDate: s.visitDate || (s.startEvaluationDate && s.endEvaluationDate
                    ? `${s.startEvaluationDate} to ${s.endEvaluationDate}` : ''),
                visitTypeId: s.visitTypeId,
                startEvaluationDate: s.startEvaluationDate,
                endEvaluationDate: s.endEvaluationDate
            });
        });

        state.selectedSchoolsMap = selectedSchoolsMap;

        if (state.isReadOnly && selectedSchoolIds.length > 0) {
            loadSelectedSchoolsOnly(fieldId, selectedSchoolIds, selectedSchoolsMap);
        } else {
            loadSchoolsWithSelection(fieldId, 1, {}, selectedSchoolsMap);
        }

        initializeDatePickers(fieldId, vm);
    };

    /* ===================== SCHOOLS ===================== */
    const loadSchoolsWithSelection = (fieldId, page = 1, filters = {}, selectedSchoolsMap = null) => {
        const state = instances.get(fieldId);
        state.currentPage = page;
        state.filters = filters;

        const params = new URLSearchParams({ page, pageSize: state.pageSize });
        if (state.searchTerm) params.append('search', state.searchTerm);
        Object.keys(filters).forEach(k => { if (filters[k]) params.append(k, filters[k]); });

        showLoadingState(fieldId);

        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                const schools = r.items || [];
                state.totalRecords = r.totalCount || 0;
                const tbody = ns.renderSchoolTable(fieldId, schools, state.isReadOnly, selectedSchoolsMap || state.selectedSchoolsMap);
                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);
                renderPagination(fieldId);
                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);
            })
            .fail(() => showErrorState(fieldId));
    };

    const loadSelectedSchoolsOnly = (fieldId, schoolIds, selectedSchoolsMap) => {
        const state = instances.get(fieldId);
        const params = new URLSearchParams({ schoolIds: schoolIds.join(','), page: 1, pageSize: schoolIds.length });
        showLoadingState(fieldId);

        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                const schools = r.items || [];
                state.totalRecords = schools.length;
                state.currentPage = 1;
                const tbody = ns.renderSchoolTable(fieldId, schools, state.isReadOnly, selectedSchoolsMap);
                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);
                renderPagination(fieldId);
                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);
            })
            .fail(() => showErrorState(fieldId));
    };

    const loadSchools = (fieldId, page = 1, filters = {}) => {
        const state = instances.get(fieldId);
        state.currentPage = page;
        state.filters = filters;

        const params = new URLSearchParams({ page, pageSize: state.pageSize });
        if (state.searchTerm) params.append('search', state.searchTerm);
        Object.keys(filters).forEach(k => { if (filters[k]) params.append(k, filters[k]); });

        showLoadingState(fieldId);

        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                const schools = r.items || [];
                state.totalRecords = r.totalCount || 0;
                const tbody = ns.renderSchoolTable(fieldId, schools, state.isReadOnly, state.selectedSchoolsMap);
                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);
                renderPagination(fieldId);
                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);
            })
            .fail(() => showErrorState(fieldId));
    };

    /* ===================== PAGINATION ===================== */
    const renderPagination = (fieldId) => {
        const state = instances.get(fieldId);
        const totalPages = Math.ceil(state.totalRecords / state.pageSize);

        if (totalPages <= 1) { $p(fieldId, 'dtPagination').empty(); return; }

        const pagination = $('<ul>').addClass('pagination pagination-sm mb-0');

        if (state.currentPage > 1) {
            pagination.append($('<li>').addClass('page-item').append(
                $('<a>').addClass('page-link').attr('href', '#').text(t('lblPrevious'))
                    .on('click', e => { e.preventDefault(); loadSchools(fieldId, state.currentPage - 1, state.filters); })
            ));
        }

        const startPage = Math.max(1, state.currentPage - 2);
        const endPage = Math.min(totalPages, state.currentPage + 2);

        if (startPage > 1) {
            pagination.append(createPageItem(fieldId, 1, state.currentPage === 1, state.filters));
            if (startPage > 2) pagination.append($('<li>').addClass('page-item disabled').append($('<span>').addClass('page-link').text('...')));
        }
        for (let i = startPage; i <= endPage; i++) {
            pagination.append(createPageItem(fieldId, i, state.currentPage === i, state.filters));
        }
        if (endPage < totalPages) {
            if (endPage < totalPages - 1) pagination.append($('<li>').addClass('page-item disabled').append($('<span>').addClass('page-link').text('...')));
            pagination.append(createPageItem(fieldId, totalPages, state.currentPage === totalPages, state.filters));
        }

        if (state.currentPage < totalPages) {
            pagination.append($('<li>').addClass('page-item').append(
                $('<a>').addClass('page-link').attr('href', '#').text(t('lblNext'))
                    .on('click', e => { e.preventDefault(); loadSchools(fieldId, state.currentPage + 1, state.filters); })
            ));
        }

        $p(fieldId, 'dtPagination').html(pagination);
    };

    const createPageItem = (fieldId, pageNum, isActive, filters) => {
        const li = $('<li>').addClass('page-item' + (isActive ? ' active' : ''));
        li.append(
            $('<a>').addClass('page-link').attr('href', '#').text(pageNum)
                .on('click', e => { e.preventDefault(); if (!isActive) loadSchools(fieldId, pageNum, filters); })
        );
        return li;
    };

    /* ===================== EVENTS ===================== */
    const bindEvents = (fieldId) => {
        const $wrapper = $p(fieldId, 'wrapper');
        if (!$wrapper.length) { console.error(`[PlanHandler] Wrapper not found for ${fieldId}`); return; }

        $wrapper
            .off('change', pid(fieldId, 'ddlPlanType'))
            .on('change', pid(fieldId, 'ddlPlanType'), function () { onPlanTypeChange(fieldId, this); })

            .off('change', pid(fieldId, 'ddlSemester'))
            .on('change', pid(fieldId, 'ddlSemester'), function () { onSemesterChange(fieldId, this); })

            .off('input', pid(fieldId, 'customSearch'))
            .on('input', pid(fieldId, 'customSearch'), function () { onSearch(fieldId, this); })

            .off('submit', pid(fieldId, 'filterForm'))
            .on('submit', pid(fieldId, 'filterForm'), function (e) { onFilter(fieldId, e); })

            .off('click', pid(fieldId, 'clearFiltersBtn'))
            .on('click', pid(fieldId, 'clearFiltersBtn'), function () { clearFilters(fieldId); });
    };

    const attachRowEvents = (fieldId) => {
        const $table = $p(fieldId, 'planTable');
        $table.find('.selectRow').off('change').on('change', function () { updateSelectedSchools(fieldId); });
        $table.find('.childDate').off('change').on('change', function () { updateSelectedSchools(fieldId); });
        $table.find('.visitTypeSelect').off('change').on('change', function () { updateSelectedSchools(fieldId); });
    };

    /* ===================== HANDLERS ===================== */
    const onPlanTypeChange = (fieldId, element) => {
        const backend = $(element).find(':selected').data('backendname');
        $p(fieldId, 'semesterContainer').hide();
        ns.destroyChildPicker();
        switch (backend) {
            case PLAN_TYPE_BACKEND.YEAR: return initYearMode(fieldId);
            case PLAN_TYPE_BACKEND.MONTH: return initMonthMode(fieldId);
            case PLAN_TYPE_BACKEND.SEMESTER: return initSemesterMode(fieldId);
            default: return initCustomMode(fieldId);
        }
    };

    const onSemesterChange = (fieldId, element) => {
        const opt = $(element).find(':selected');
        const start = new Date(opt.data('startdate'));
        const end = new Date(opt.data('enddate'));
        $p(fieldId, 'parentDate').val(`${ns.formatDateISO(start)} to ${ns.formatDateISO(end)}`).prop('disabled', true);
        ns.initChildPicker(start, end);
    };

    const initYearMode = (fieldId) => {
        const y = new Date().getFullYear();
        $p(fieldId, 'parentDate').val(`${y}-01-01 to ${y}-12-31`).prop('disabled', true);
        ns.initChildPicker(new Date(`${y}-01-01`), new Date(`${y}-12-31`));
    };
    const initMonthMode = (fieldId) => { $p(fieldId, 'parentDate').val('').prop('disabled', false); ns.initParentPicker(fieldId, 'month', null, null, null); };
    const initSemesterMode = (fieldId) => { $p(fieldId, 'semesterContainer').show(); $p(fieldId, 'parentDate').val('').prop('disabled', true); };
    const initCustomMode = (fieldId) => { $p(fieldId, 'parentDate').val('').prop('disabled', false); ns.initParentPicker(fieldId, 'custom', null, null, null); };

    const initializeDatePickers = (fieldId, plan) => {
        if (!plan.dateRange) return;
        const parts = plan.dateRange.split(' to ');
        if (parts.length === 2) ns.initChildPicker(new Date(parts[0].trim()), new Date(parts[1].trim()));
    };

    const initChildPickerForTable = (fieldId) => {
        const dateRange = $p(fieldId, 'parentDate').val();
        if (dateRange && dateRange.includes(' to ')) {
            const [s, e] = dateRange.split(' to ');
            ns.initChildPicker(new Date(s.trim()), new Date(e.trim()));
        }
    };

    /* ===================== SEARCH & FILTER ===================== */
    const onSearch = (fieldId, element) => {
        const state = instances.get(fieldId);
        state.searchTerm = $(element).val().trim();
        clearTimeout(state.searchTimeout);
        state.searchTimeout = setTimeout(() => loadSchools(fieldId, 1, state.filters), 300);
    };

    const onFilter = (fieldId, e) => {
        e.preventDefault();
        const state = instances.get(fieldId);

        // ✅ UPDATED: إضافة المرحلة والجنس والشعبة إلى الفلاتر
        state.filters = {
            name: $p(fieldId, 'filterSchoolName').val(),
            schoolLevelId: $p(fieldId, 'filterSchoolLevel').val(),   // ✅ NEW
            genderId: $p(fieldId, 'filterGender').val(),         // ✅ NEW
            departmentId: $p(fieldId, 'filterDepartment').val(),     // ✅ NEW
            lastEvalDate: $p(fieldId, 'filterLastEvalDate').val(),
            establishmentDate: $p(fieldId, 'filterCreatedDate').val(),
            nextEvalDate: $p(fieldId, 'filterNextEvalDate').val(),
            previousResult: $p(fieldId, 'filterPreviousResult').val(),
            visitType: $p(fieldId, 'filterVisitType').val()
        };

        // حذف القيم الفارغة
        Object.keys(state.filters).forEach(key => {
            if (!state.filters[key]) delete state.filters[key];
        });

        loadSchools(fieldId, 1, state.filters);

        const offcanvas = bootstrap.Offcanvas.getInstance($p(fieldId, 'filterOffcanvas')[0]);
        if (offcanvas) offcanvas.hide();
    };

    const clearFilters = (fieldId) => {
        const state = instances.get(fieldId);
        state.filters = {};
        state.searchTerm = '';

        // ✅ UPDATED: مسح الحقول الثلاثة الجديدة أيضاً
        $p(fieldId, 'filterSchoolName').val('');
        $p(fieldId, 'filterSchoolLevel').val('');   // ✅ NEW
        $p(fieldId, 'filterGender').val('');         // ✅ NEW
        $p(fieldId, 'filterDepartment').val('');     // ✅ NEW
        $p(fieldId, 'filterLastEvalDate').val('');
        $p(fieldId, 'filterCreatedDate').val('');
        $p(fieldId, 'filterNextEvalDate').val('');
        $p(fieldId, 'filterPreviousResult').val('');
        $p(fieldId, 'filterVisitType').val('');
        $p(fieldId, 'customSearch').val('');

        loadSchools(fieldId, 1);
    };

    /* ===================== SAVE ===================== */
    const collect = (fieldId) => {
        const state = instances.get(fieldId);
        const $planType = $p(fieldId, 'ddlPlanType');

        const evaluationData = {
            fieldId: fieldId,
            title: $p(fieldId, 'planTitle').val(),
            planTypeId: $planType.val(),
            planTypeDepId: $planType.val(),
            semesterId: $p(fieldId, 'ddlSemester').val(),
            dateRange: $p(fieldId, 'parentDate').val(),
            schools: state.selectedSchools.map(s => ({
                schoolId: s.id,
                visitDate: s.visitDate,
                visitTypeId: s.visitTypeId
            }))
        };
        if (state.planId) evaluationData.id = state.planId;
        return evaluationData;
    };

    const onSaveClick = (fieldId, e) => {
        e.preventDefault();
        collect(fieldId);
    };

    /* ===================== HELPERS ===================== */
    const updateSelectedSchools = (fieldId) => {
        const state = instances.get(fieldId);
        state.selectedSchools = [];
        $p(fieldId, 'planTable').find('.selectRow:checked').each(function () {
            const $cb = $(this);
            const schoolId = $cb.data('school-id');
            state.selectedSchools.push({
                id: schoolId,
                name: $cb.data('name'),
                visitDate: $p(fieldId, 'planTable').find(`.childDate[data-school-id="${schoolId}"]`).val(),
                visitTypeId: $p(fieldId, 'planTable').find(`.visitTypeSelect[data-school-id="${schoolId}"]`).val()
            });
        });
    };

    const showLoadingState = (fieldId) => {
        $p(fieldId, 'planTable').find('tbody').html(`
            <tr><td colspan="7" class="text-center py-5">
                <div class="spinner-border text-primary"></div>
                <p class="mt-2 text-muted">جاري التحميل...</p>
            </td></tr>`);
    };

    const showErrorState = (fieldId) => {
        $p(fieldId, 'planTable').find('tbody').html(`
            <tr><td colspan="7" class="text-center text-danger py-5">
                <i class="la la-exclamation-triangle la-3x mb-2"></i>
                <p>حدث خطأ أثناء تحميل البيانات</p>
                <button class="btn btn-sm btn-primary" onclick="location.reload()">إعادة المحاولة</button>
            </td></tr>`);
    };

    /* ===================== EXPORT ===================== */
    global.PlanHandler = Object.freeze({
        init,
        collect,
        getInstance: (fieldId) => instances.get(fieldId)
    });

    /* ===================== UTILS ===================== */
    const toCamelCaseKeys = (obj) => {
        if (Array.isArray(obj)) return obj.map(toCamelCaseKeys);
        if (obj !== null && typeof obj === 'object') {
            return Object.keys(obj).reduce((acc, key) => {
                acc[key.charAt(0).toLowerCase() + key.slice(1)] = toCamelCaseKeys(obj[key]);
                return acc;
            }, {});
        }
        return obj;
    };

    function toDateOnly(d) {
        if (!d) return null;
        return new Date(d).toISOString().split('T')[0];
    }

})(window);