(function (global) {
    'use strict';
    // =================== LOCALIZATION Helper ===================
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
                    url,
                    method: 'POST',
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
            selectedSchoolsMap: null // Map to track selected schools when editing
        };
    }

    /* ===================== PREFIX HELPERS ===================== */

    const pid = (fieldId, id) => `#${fieldId}_${id}`;
    const pidRaw = (fieldId, id) => `${fieldId}_${id}`;
    const $p = (fieldId, id) => window.jQuery(pid(fieldId, id));

    /* ===================== INIT ===================== */

    const init = async (isReadOnly, fieldId = null, planObject = null, element) => {
        if (!fieldId) {
            console.error('[PlanHandler] fieldId is required!');
            return;
        }
        if (planObject) {
            planObject = toCamelCaseKeys(planObject);
        }
        const state = createInstanceState(fieldId);
        state.isReadOnly = isReadOnly;
        //Store plan Id
        if (planObject && planObject.id) {
            state.planId = planObject.id;
        }
        instances.set(fieldId, state);

        try {
            if (!ns.planTypes || ns.planTypes.length === 0) {
                await Promise.all([
                    loadPlanTypes(),
                    loadSemesters(),
                    loadVisitTypes(),
                    loadVacationDays(),
                    loadParentOrgTree(),
                    loadFomrEvalMatrixValue()
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
            populateFilterPreviousResult(fieldId);
        } catch (e) {
            console.error(`[PlanHandler] Init failed for ${fieldId}`, e);
            alert('حدث خطأ أثناء التحميل');
        }
    };

    /* ===================== LOAD DATA ===================== */

    const loadPlanTypes = () =>
        jqClient().Get(API_ENDPOINTS.GET_PLAN_TYPES)
            .then(r => ns.planTypes = r?.result || []);

    const loadSemesters = () =>
        jqClient().Get(API_ENDPOINTS.GET_SEMESTERS)
            .then(r => ns.semesters = r?.result || []);

    const loadVisitTypes = () =>
        jqClient().Get(API_ENDPOINTS.GET_VISITS)
            .then(r => ns.visitTypes = r?.result || []);

    const loadVacationDays = () =>
        jqClient().Get(API_ENDPOINTS.GET_VACATION_DATES)
            .then(r => ns.holidays = (r || []).map(x =>
            ({
                start: new Date(x.startDate),
                end: new Date(x.endDate),
                isCron: x.isCronExpression,
                cron: x.cronExpression
            })));
    const loadParentOrgTree = () =>
        jqClient().Get(API_ENDPOINTS.GET_PARNT_ORGTREE)
            .then(r => ns.parentSchool = r?.result || []);

    const loadPlanData = async (fieldId, planId) => {
        const r = await jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`);
        renderPlanWithData(fieldId, r.result);
    };
    const loadFomrEvalMatrixValue = () =>
        jqClient().Get(API_ENDPOINTS.GetFomrEvalMatrixValue)
            .then(r => ns.fomrEvalMatrixValue = r?.result || []);

    /* ===================== POPULATE ===================== */

    //const populatePlanTypes = (fieldId) => {
    //    const $s = $p(fieldId, 'ddlPlanType');

    //    if ($s.hasClass("select2-hidden-accessible")) {
    //        $s.select2('destroy');
    //    }
    //    $s.empty().append(`<option value="">${t('lblChoosePlanType')}</option>`);

    //    ns.planTypes.forEach(t =>
    //        $s.append(`<option value="${t.id}" data-backendname="${t.backendName}">${t.name}</option>`)
    //    );

    //    $s.select2({ width: '100%', allowClear: true });
    //};
    const populatePlanTypes = (fieldId, elementId = null) => {
        const parentElement = elementId ? $(`#${elementId}`) : null;
        const idPrefix = fieldId ? `${fieldId}_` : '';
        const $s = $(`#${idPrefix}ddlPlanType`);

        if (!$s.length) {
            console.warn('ddlPlanType not found:', `#${idPrefix}ddlPlanType`);
            return;
        }

        if ($s.data('select2')) {
            try { $s.select2('destroy'); } catch { }
        }

        $s.empty().append(`<option value="">${t('lblChoosePlanType')}</option>`);

        (ns.planTypes || []).forEach(pt => {
            $s.append(`<option value="${pt.id}" data-backendname="${pt.backendName}">${pt.name}</option>`);
        });

        const $modal = $s.closest('.modal');

        const dropdownParent =
            (parentElement && parentElement.length) ? parentElement :
                ($modal.length ? $modal : $(document.body));

        $s.select2({
            width: '100%',
            allowClear: true,
            dropdownParent: dropdownParent
        });
    };


    const populateSemesters = (fieldId, elementId = null) => {
        const parentElement = elementId ? $(`#${elementId}`) : null;

        const $s = $p(fieldId, 'ddlSemester');

        if ($s.hasClass("select2-hidden-accessible")) {
            $s.select2('destroy');
        }

        $s.empty().append(`<option value="">${t('lblChooseSemester')}</option>`);

        ns.semesters.forEach(s =>
            $s.append(
                `<option value="${s.id}"
                         data-startdate="${s.startDate}"
                         data-enddate="${s.endDate}">
                    ${s.name}
                 </option>`
            )
        );
        const $modal = $s.closest('.modal');
        const dropdownParent =
            (parentElement && parentElement.length) ? parentElement :
                ($modal.length ? $modal : $(document.body));
        $s.select2({ width: '100%', allowClear: true, dropdownParent: dropdownParent });
    };

    const populateFilterVisitTypes = (fieldId) => {
        const $select = $p(fieldId, 'filterVisitType');

        ns.visitTypes.forEach(type => {
            $select.append(`<option value="${type.id}">${type.name}</option>`);
        });
    };
    const populateFilterParentOrgTree = (fieldId) => {
        const $select = $p(fieldId, 'filterParentOrgTree');
        ns.parentSchool.forEach(parent => {
            $select.append(`<option value="${parent.id}">${parent.nameEn}</option>`)
        });
    }
    const populateFilterPreviousResult = (fieldId) => {
        const $select = $p(fieldId, 'filterPreviousResult');
        ns.fomrEvalMatrixValue.forEach(fromEval => {
            $select.append(`<option value="${fromEval.id}">${fromEval.name}</option>`);
        });
    };
    const initializeFilterDatePickers = (fieldId) => {
        const dateFields = ['filterLastEvalDate', 'filterCreatedDate', 'filterNextEvalDate'];

        dateFields.forEach(field => {
            const $input = $p(fieldId, field);

            if ($input.length && typeof flatpickr !== 'undefined') {
                flatpickr($input[0], {
                    locale: "en",
                    dateFormat: "Y-m-d",
                    allowInput: true
                });
            }
        });
    };

    /* ===================== RENDER ===================== */

    const renderNewPlan = (fieldId) => {
        const state = instances.get(fieldId);

        const form = ns.renderPlanForm(fieldId, null, state.isReadOnly);
        $p(fieldId, 'planFormContainer').find('.form-container').html(form);

        // ✅ تحميل المدارس من Backend
        loadSchools(fieldId, 1);
        initCustomMode(fieldId);
    };


    const renderPlanWithData = (fieldId, plan, element) => {
        const state = instances.get(fieldId);

        if (plan && plan.id) {
            state.planId = plan.id;
        }

        const vm = {
            title: plan.name || '',
            planTypeId: plan.planTypeId || plan.planTypeDepId || plan.PlanTypeDepId,
            semesterId: plan.semesterId,
            dateRange: toDateOnly(plan.startDate) && (plan.endDate) ?
                `${toDateOnly(plan.startDate)} to ${toDateOnly(plan.endDate)}` : '',
            schools: plan.schools || []
        };

        const form = ns.renderPlanForm(fieldId, vm, state.isReadOnly);
        $p(fieldId, 'planFormContainer').find('.form-container').html(form);

        populatePlanTypes(fieldId, element);
        populateSemesters(fieldId, element);
        $p(fieldId, 'semesterContainer').hide();

        $p(fieldId, 'planTitle').val(vm.title);
        $p(fieldId, 'ddlPlanType').val(vm.planTypeId).trigger('change');
        if (vm.semesterId) {
            $p(fieldId, 'ddlSemester').val(vm.semesterId).trigger('change');
        }
        $p(fieldId, 'parentDate').val(vm.dateRange);

        // ✅ MODIFIED: Create a Map of selected schools with their data AND extract IDs
        const selectedSchoolsMap = new Map();
        const selectedSchoolIds = []; // NEW: Array to store school IDs

        vm.schools.forEach(s => {
            const schoolId = s.id || s.schoolId || s.id;
            selectedSchoolIds.push(schoolId); // NEW: Collect school IDs
            selectedSchoolsMap.set(schoolId, {
                id: schoolId,
                visitDate: s.visitDate || (s.startEvaluationDate && s.endEvaluationDate ?
                    `${s.startEvaluationDate} to ${s.endEvaluationDate}` : ''),
                visitTypeId: s.visitTypeId,
                startEvaluationDate: s.startEvaluationDate,
                endEvaluationDate: s.endEvaluationDate
            });
        });

        // ✅ Store selected schools map in state
        state.selectedSchoolsMap = selectedSchoolsMap;

        // ✅ MODIFIED: If readonly, load ONLY selected schools. Otherwise, load all schools
        if (state.isReadOnly && selectedSchoolIds.length > 0) {
            loadSelectedSchoolsOnly(fieldId, selectedSchoolIds, selectedSchoolsMap);
        } else {
            loadSchoolsWithSelection(fieldId, 1, {}, selectedSchoolsMap);
        }

        initializeDatePickers(fieldId, vm);
    };

    /* ===================== SCHOOLS (Backend Only) ===================== */

    const loadSchoolsWithSelection = (fieldId, page = 1, filters = {}, selectedSchoolsMap = null) => {
        const state = instances.get(fieldId);
        state.currentPage = page;
        state.filters = filters;

        // Build query parameters
        const params = new URLSearchParams({
            pageNumber: page,
            pageSize: state.pageSize
        });

        // Add search term
        if (state.searchTerm) {
            params.append('search', state.searchTerm);
        }

        // Add filters
        Object.keys(filters).forEach(k => {
            if (filters[k]) params.append(k, filters[k]);
        });

        showLoadingState(fieldId);

        // Call API to get all schools
        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                const schools = r.items || [];
                state.totalRecords = r.totalCount || 0;

                // Render table with selected schools map
                const tbody = ns.renderSchoolTable(
                    fieldId,
                    schools,
                    state.isReadOnly,
                    selectedSchoolsMap || state.selectedSchoolsMap
                );

                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);

                // Render pagination
                renderPagination(fieldId);

                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load schools`, err);
                showErrorState(fieldId);
            });
    };

    // ✅ NEW FUNCTION: Load only selected schools by IDs (for readonly mode)
    const loadSelectedSchoolsOnly = (fieldId, schoolIds, selectedSchoolsMap) => {
        const state = instances.get(fieldId);

        // Build query parameters with school IDs
        const params = new URLSearchParams();

        schoolIds.forEach(id => {
            params.append('schoolIds', id);
        });

        params.append('page', 1);
        params.append('pageSize', schoolIds.length);

        showLoadingState(fieldId);

        // Call API to get only selected schools
        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                const schools = r.items || [];
                state.totalRecords = schools.length; // Set total to the number of selected schools
                state.currentPage = 1;

                // Render table with all schools pre-selected
                const tbody = ns.renderSchoolTable(
                    fieldId,
                    schools,
                    state.isReadOnly,
                    selectedSchoolsMap
                );

                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);

                // Render pagination (will show 1 page with all selected schools)
                renderPagination(fieldId);

                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load selected schools`, err);
                showErrorState(fieldId);
            });
    };

    const loadSchools = (fieldId, page = 1, filters = {}) => {
        const state = instances.get(fieldId);
        state.currentPage = page;
        state.filters = filters;

        // ✅ بناء الـ query parameters
        const params = new URLSearchParams({
            pageNumber: page,
            pageSize: state.pageSize
        });

        // ✅ إضافة البحث
        if (state.searchTerm) {
            params.append('search', state.searchTerm);
        }

        // ✅ إضافة الفلاتر
        Object.keys(filters).forEach(k => {
            if (filters[k]) params.append(k, filters[k]);
        });

        showLoadingState(fieldId);
        // ✅ استدعاء API
        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                const schools = r.items || [];
                state.totalRecords = r.totalCount || 0;
                const tbody = ns.renderSchoolTable(
                    fieldId,
                    schools,
                    state.isReadOnly,
                    state.selectedSchoolsMap // Pass the selection map if it exists
                );

                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);

                // ✅ عرض Pagination
                renderPagination(fieldId);

                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load schools`, err);
                showErrorState(fieldId);
            });
    };

    /* ===================== PAGINATION ===================== */

    const renderPagination = (fieldId) => {
        const state = instances.get(fieldId);
        const totalPages = Math.ceil(state.totalRecords / state.pageSize);

        if (totalPages <= 1) {
            $p(fieldId, 'dtPagination').empty();
            return;
        }

        const pagination = $('<ul>').addClass('pagination pagination-sm mb-0');

        // Previous button
        if (state.currentPage > 1) {
            const prevItem = $('<li>').addClass('page-item');
            const prevLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .text(`${t('lblPrevious')}`)
                .on('click', function (e) {
                    e.preventDefault();
                    loadSchools(fieldId, state.currentPage - 1, state.filters);
                });
            prevItem.append(prevLink);
            pagination.append(prevItem);
        }

        // Page numbers (محدودة لـ 5 صفحات فقط للعرض)
        const startPage = Math.max(1, state.currentPage - 2);
        const endPage = Math.min(totalPages, state.currentPage + 2);

        if (startPage > 1) {
            pagination.append(createPageItem(fieldId, 1, state.currentPage === 1, state.filters));
            if (startPage > 2) {
                pagination.append($('<li>').addClass('page-item disabled').append(
                    $('<span>').addClass('page-link').text('...')
                ));
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            pagination.append(createPageItem(fieldId, i, state.currentPage === i, state.filters));
        }

        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                pagination.append($('<li>').addClass('page-item disabled').append(
                    $('<span>').addClass('page-link').text('...')
                ));
            }
            pagination.append(createPageItem(fieldId, totalPages, state.currentPage === totalPages, state.filters));
        }

        // Next button
        if (state.currentPage < totalPages) {
            const nextItem = $('<li>').addClass('page-item');
            const nextLink = $('<a>')
                .addClass('page-link')
                .attr('href', '#')
                .text(`${t('lblNext')}`)
                .on('click', function (e) {
                    e.preventDefault();
                    loadSchools(fieldId, state.currentPage + 1, state.filters);
                });
            nextItem.append(nextLink);
            pagination.append(nextItem);
        }

        $p(fieldId, 'dtPagination').html(pagination);
    };

    const createPageItem = (fieldId, pageNum, isActive, filters) => {
        const pageItem = $('<li>').addClass('page-item');
        if (isActive) {
            pageItem.addClass('active');
        }

        const pageLink = $('<a>')
            .addClass('page-link')
            .attr('href', '#')
            .text(pageNum)
            .on('click', function (e) {
                e.preventDefault();
                if (!isActive) {
                    loadSchools(fieldId, pageNum, filters);
                }
            });

        pageItem.append(pageLink);
        return pageItem;
    };

    /* ===================== EVENTS ===================== */

    const bindEvents = (fieldId) => {
        const $wrapper = $p(fieldId, 'wrapper');

        if (!$wrapper.length) {
            console.error(`[PlanHandler] Wrapper not found for ${fieldId}`);
            return;
        }

        $wrapper.off('change', pid(fieldId, 'ddlPlanType'))
            .on('change', pid(fieldId, 'ddlPlanType'), function () {
                onPlanTypeChange(fieldId, this);
            });

        $wrapper.off('change', pid(fieldId, 'ddlSemester'))
            .on('change', pid(fieldId, 'ddlSemester'), function () {
                onSemesterChange(fieldId, this);
            });

        // ✅ البحث: استدعاء API بعد 300ms من التوقف عن الكتابة
        $wrapper.off('input', pid(fieldId, 'customSearch'))
            .on('input', pid(fieldId, 'customSearch'), function () {
                onSearch(fieldId, this);
            });

        // ✅ الفلتر: استدعاء API مع الفلاتر
        $wrapper.off('submit', pid(fieldId, 'filterForm'))
            .on('submit', pid(fieldId, 'filterForm'), function (e) {
                onFilter(fieldId, e);
            });

        $wrapper.off('click', pid(fieldId, 'clearFiltersBtn'))
            .on('click', pid(fieldId, 'clearFiltersBtn'), function () {
                clearFilters(fieldId);
            });
    };

    const attachRowEvents = (fieldId) => {
        const $table = $p(fieldId, 'planTable');
        const state = instances.get(fieldId);

        $table.find('.selectRow').off('change').on('change', function () {
            updateSelectedSchools(fieldId);
        });

        $table.find('.childDate').off('change').on('change', function () {
            const schoolId = $(this).data('school-id');
            const newDate = $(this).val();
            if (state.selectedSchoolsMap && state.selectedSchoolsMap.has(schoolId)) {
                state.selectedSchoolsMap.get(schoolId).visitDate = newDate;
                state.selectedSchools = Array.from(state.selectedSchoolsMap.values());
            }
        });

        $table.find('.visitTypeSelect').off('change').on('change', function () {
            const schoolId = $(this).data('school-id');
            const newType = $(this).val();
            if (state.selectedSchoolsMap && state.selectedSchoolsMap.has(schoolId)) {
                state.selectedSchoolsMap.get(schoolId).visitTypeId = newType;
                state.selectedSchools = Array.from(state.selectedSchoolsMap.values());
            }
        });
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

        $p(fieldId, 'parentDate')
            .val(`${ns.formatDateISO(start)} to ${ns.formatDateISO(end)}`)
            .prop('disabled', true);

        ns.initChildPicker(start, end);
    };

    const initYearMode = (fieldId) => {
        const y = new Date().getFullYear();
        const start = `${y}-01-01`;
        const end = `${y}-12-31`;

        $p(fieldId, 'parentDate').val(`${start} to ${end}`).prop('disabled', true);
        ns.initChildPicker(new Date(start), new Date(end));
    };

    const initMonthMode = (fieldId) => {
        $p(fieldId, 'parentDate').val('').prop('disabled', false);
        ns.initParentPicker(fieldId, 'month', null, null, null);
    };

    const initSemesterMode = (fieldId) => {
        $p(fieldId, 'semesterContainer').show();
        $p(fieldId, 'parentDate').val('').prop('disabled', true);
    };

    const initCustomMode = (fieldId) => {
        $p(fieldId, 'parentDate').val('').prop('disabled', false);
        ns.initParentPicker(fieldId, 'custom', null, null, null);
    };

    const initializeDatePickers = (fieldId, plan) => {
        if (!plan.dateRange) return;

        const parts = plan.dateRange.split(' to ');
        if (parts.length === 2) {
            const startDate = new Date(parts[0].trim());
            const endDate = new Date(parts[1].trim());
            ns.initChildPicker(startDate, endDate);
        }
    };

    const initChildPickerForTable = (fieldId) => {
        const dateRange = $p(fieldId, 'parentDate').val();
        if (dateRange && dateRange.includes(' to ')) {
            const parts = dateRange.split(' to ');
            if (parts.length === 2) {
                const startDate = new Date(parts[0].trim());
                const endDate = new Date(parts[1].trim());
                ns.initChildPicker(startDate, endDate);
            }
        }
    };
    const updateSelectionCounter = (fieldId) => {
        const state = instances.get(fieldId);
        const count = state.selectedSchoolsMap ? state.selectedSchoolsMap.size : 0;
        const $counter = $p(fieldId, 'selectedSchoolsCounter');
        if ($counter.length) {
            $counter.text(count);
            $counter.closest('.selection-counter-badge')
                .toggleClass('badge-active', count > 0);
        }
    };

    /* ===================== SEARCH & FILTER (Backend) ===================== */

    const onSearch = (fieldId, element) => {
        const state = instances.get(fieldId);
        state.searchTerm = $(element).val().trim();

        clearTimeout(state.searchTimeout);
        state.searchTimeout = setTimeout(() => {
            // ✅ استدعاء API مع البحث
            loadSchools(fieldId, 1, state.filters);
        }, 300);
    };

    const onFilter = (fieldId, e) => {
        e.preventDefault();
        const state = instances.get(fieldId);

        // ✅ جمع قيم الفلاتر
        state.filters = {
            name: $p(fieldId, 'filterSchoolName').val(),
            lastEvalDate: $p(fieldId, 'filterLastEvalDate').val(),
            establishmentDate: $p(fieldId, 'filterCreatedDate').val(),
            nextEvalDate: $p(fieldId, 'filterNextEvalDate').val(),
            fomrEvalMatrixValueId: $p(fieldId, 'filterPreviousResult').val(),
            visitType: $p(fieldId, 'filterVisitType').val()
        };

        // حذف القيم الفارغة
        Object.keys(state.filters).forEach(key => {
            if (!state.filters[key]) delete state.filters[key];
        });
        // ✅ استدعاء API مع الفلاتر
        loadSchools(fieldId, 1, state.filters);

        // إغلاق الـ offcanvas
        const offcanvas = bootstrap.Offcanvas.getInstance($p(fieldId, 'filterOffcanvas')[0]);
        if (offcanvas) offcanvas.hide();
    };

    const clearFilters = (fieldId) => {
        const state = instances.get(fieldId);

        // مسح state
        state.filters = {};
        state.searchTerm = '';

        // مسح الحقول من UI
        $p(fieldId, 'filterSchoolName').val('');
        $p(fieldId, 'filterLastEvalDate').val('');
        $p(fieldId, 'filterCreatedDate').val('');
        $p(fieldId, 'filterNextEvalDate').val('');
        $p(fieldId, 'filterPreviousResult').val('');
        $p(fieldId, 'filterVisitType').val('');
        $p(fieldId, 'customSearch').val('');
        // ✅ إعادة تحميل كل المدارس
        loadSchools(fieldId, 1);
    };

    /* ===================== SAVE ===================== */

    const collect = (fieldId) => {
        const state = instances.get(fieldId);
        const $planType = $p(fieldId, 'ddlPlanType');

        const evaluationData = {
            fieldId: fieldId,
            title: $p(fieldId, 'planTitle').val(),
            planTypeId: $p(fieldId, 'ddlPlanType').val(),
            planTypeDepId: $planType.val(),
            semesterId: $p(fieldId, 'ddlSemester').val(),
            dateRange: $p(fieldId, 'parentDate').val(),
            schools: state.selectedSchools.map(s => ({
                schoolId: s.id,
                visitDate: s.visitDate,
                visitTypeId: s.visitTypeId
            }))
        };
        if (state.planId) {
            evaluationData.id = state.planId;
        }
        return evaluationData;
    };

    const onSaveClick = (fieldId, e) => {
        e.preventDefault();
        const payload = collect(fieldId);
    };

    /* ===================== HELPERS ===================== */

    const updateSelectedSchools = (fieldId) => {
        const state = instances.get(fieldId);

        if (!state.selectedSchoolsMap) {
            state.selectedSchoolsMap = new Map();
        }

        $p(fieldId, 'planTable').find('.selectRow').each(function () {
            const schoolId = $(this).data('school-id');
            if (!$(this).is(':checked')) {
                state.selectedSchoolsMap.delete(schoolId);
            }
        });
        $p(fieldId, 'planTable').find('.selectRow:checked').each(function () {
            const $checkbox = $(this);
            const schoolId = $checkbox.data('school-id');
            const schoolName = $checkbox.data('name');
            const visitDate = $p(fieldId, 'planTable')
                .find(`.childDate[data-school-id="${schoolId}"]`).val();
            const visitTypeId = $p(fieldId, 'planTable')
                .find(`.visitTypeSelect[data-school-id="${schoolId}"]`).val();

            state.selectedSchoolsMap.set(schoolId, {
                id: schoolId,
                name: schoolName,
                visitDate: visitDate,
                visitTypeId: visitTypeId
            });
        });

        state.selectedSchools = Array.from(state.selectedSchoolsMap.values());

        updateSelectionCounter(fieldId);
    };

    const showLoadingState = (fieldId) => {
        $p(fieldId, 'planTable').find('tbody').html(`
            <tr>
                <td colspan="7" class="text-center py-5">
                    <div class="spinner-border text-primary"></div>
                    <p class="mt-2 text-muted">جاري التحميل...</p>
                </td>
            </tr>
        `);
    };

    const showErrorState = (fieldId) => {
        $p(fieldId, 'planTable').find('tbody').html(`
            <tr>
                <td colspan="7" class="text-center text-danger py-5">
                    <i class="la la-exclamation-triangle la-3x mb-2"></i>
                    <p>حدث خطأ أثناء تحميل البيانات</p>
                    <button class="btn btn-sm btn-primary" onclick="location.reload()">
                        إعادة المحاولة
                    </button>
                </td>
            </tr>
        `);
    };

    /* ===================== EXPORT ===================== */

    global.PlanHandler = Object.freeze({
        init,
        collect,
        getInstance: (fieldId) => instances.get(fieldId)
    });
    /*=================== Convert to small letters ===================*/
    const toCamelCaseKeys = (obj) => {
        if (Array.isArray(obj)) {
            return obj.map(toCamelCaseKeys);
        }

        if (obj !== null && typeof obj === "object") {
            return Object.keys(obj).reduce((acc, key) => {
                const camelKey = key.charAt(0).toLowerCase() + key.slice(1);
                acc[camelKey] = toCamelCaseKeys(obj[key]);
                return acc;
            }, {});
        }

        return obj;
    };
    function toDateOnly(dateString) {
        if (!dateString) return null;

        return new Date(dateString).toISOString().split('T')[0];
    }

})(window);