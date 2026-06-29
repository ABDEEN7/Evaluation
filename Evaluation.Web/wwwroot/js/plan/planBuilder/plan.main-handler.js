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
            allSchools: [],
            filters: {},
            searchTerm: '',
            selectedSchools: [],
            selectedSchoolsMap: null
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
                    loadFomrEvalMatrixValue(),
                    loadCurrentAcademicYear(),
                    loadSchoolLevels(),
                    loadSchoolGenders()
                ]);
            }

            if (!ns.depConfig) {
                await loadDepartmentConfig();
            }
            const $placeholder = $(`#${fieldId}_filterOffcanvasPlaceholder`);
            if ($placeholder.length) {
                $placeholder.replaceWith(ns.generateFilterOffcanvasHTML(fieldId));
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
            populateFilterSchoolLevels(fieldId);
            populateFilterGenders(fieldId);
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
        if (!ns.depConfig) {
            await loadDepartmentConfig();
        }
        const r = await jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`);
        renderPlanWithData(fieldId, r.result);
    };
    const loadFomrEvalMatrixValue = () =>
        jqClient().Get(API_ENDPOINTS.GetFomrEvalMatrixValue)
            .then(r => ns.fomrEvalMatrixValue = r?.result || []);

    const loadCurrentAcademicYear = () =>
        jqClient().Get(API_ENDPOINTS.GET_CurrentAcademicYear)
            .then(r => {
                ns.currentAcademicYear = r
                    ? { start: new Date(r.startDate), end: new Date(r.endDate) }
                    : null;
            });

    /* ===================== POPULATE ===================== */

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
        if (!$select.length) return;
        ns.visitTypes?.forEach(v => $select.append($('<option>').val(v.backendName).text(v.name)));
    };

    const populateFilterParentOrgTree = (fieldId) => {
        const $select = $p(fieldId, 'filterParentOrgTree');
        if (!$select.length) return;
        ns.parentSchool.forEach(parent => {
            $select.append(`<option value="${parent.id}">${parent.nameEn}</option>`);
        });
    };
    const populateFilterPreviousResult = (fieldId) => {
        const $select = $p(fieldId, 'filterPreviousResult');
        if (!$select.length) return;
        ns.fomrEvalMatrixValue.forEach(fromEval => {
            $select.append(`<option value="${fromEval.id}">${fromEval.name}</option>`);
        });
    };
    const initializeFilterDatePickers = (fieldId) => {
        const dateFields = ['filterLastEvalDate', 'filterCreatedDate', 'filterNextEvalDate'];
        const isAr = document.documentElement.lang.toLowerCase().startsWith('ar');

        dateFields.forEach(field => {
            const $input = $p(fieldId, field);
            if ($input.length && typeof flatpickr !== 'undefined') {
                flatpickr($input[0], {
                    locale: isAr ? "ar" : "default",
                    dateFormat: "Y-m-d",
                    allowInput: true,
                    disableMobile: true
                });
            }
        });
    };
    const populateFilterSchoolLevels = (fieldId) => {
        const $select = $p(fieldId, 'filterSchoolLevel');
        if (!$select.length) return;
        ns.schoolLevels?.forEach(level => {
            $select.append($('<option>').val(level.id).text(level.name));
        });
    };

    const populateFilterGenders = (fieldId) => {
        const $select = $p(fieldId, 'filterGender');
        if (!$select.length) return;
        ns.schoolGenders?.forEach(g => {
            $select.append($('<option>').val(g.backendName).text(g.name));
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

        const selectedSchoolsMap = new Map();
        const selectedSchoolIds = [];

        vm.schools.forEach(s => {
            const schoolId = s.id || s.schoolId;
            selectedSchoolIds.push(schoolId);
            selectedSchoolsMap.set(schoolId, {
                id: schoolId,
                visitDate: s.visitDate || (s.startEvaluationDate && s.endEvaluationDate ?
                    `${s.startEvaluationDate} to ${s.endEvaluationDate}` : ''),
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
        state.filters = filters;

        if (state.allSchools.length > 0 && page !== 1) {
            state.currentPage = page;
            renderPageFromCache(fieldId);
            return;
        }

        state.currentPage = 1;

        const params = new URLSearchParams({
            pageNumber: page,
            pageSize: state.pageSize 
        });

        if (state.searchTerm) {
            params.append('search', state.searchTerm);
        }

        Object.keys(filters).forEach(k => {
            if (filters[k]) params.append(k, filters[k]);
        });

        if (selectedSchoolsMap || state.selectedSchoolsMap) {
            const map = selectedSchoolsMap || state.selectedSchoolsMap;
            map.forEach((_, id) => params.append('prioritizeSchoolIds', id));
        }

        showLoadingState(fieldId);

        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                state.allSchools = r.items || [];
                state.totalRecords = state.allSchools.length;

                if (selectedSchoolsMap) {
                    state.selectedSchoolsMap = selectedSchoolsMap;
                }

                renderPageFromCache(fieldId);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load schools`, err);
                showErrorState(fieldId);
            });
    };

    
    const loadSelectedSchoolsOnly = (fieldId, schoolIds, selectedSchoolsMap) => {
        const state = instances.get(fieldId);
        state.currentPage = 1;

        const params = new URLSearchParams();
        schoolIds.forEach(id => params.append('schoolIds', id));
        params.append('pageNumber', 1);
        params.append('pageSize', state.pageSize);

        showLoadingState(fieldId);

        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                state.allSchools = r.items || [];
                state.totalRecords = state.allSchools.length;
                state.selectedSchoolsMap = selectedSchoolsMap;

                renderPageFromCache(fieldId);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load selected schools`, err);
                showErrorState(fieldId);
            });
    };

    const loadSchools = (fieldId, page = 1, filters = {}) => {
        const state = instances.get(fieldId);
        state.filters = filters;
        if (state.allSchools.length > 0 && page !== 1) {
            state.currentPage = page;
            renderPageFromCache(fieldId);
            return;
        }

        state.currentPage = 1;

        const params = new URLSearchParams({
            pageNumber: 1,
            pageSize: state.pageSize 
        });

        if (state.searchTerm) {
            params.append('search', state.searchTerm);
        }

        Object.keys(filters).forEach(k => {
            if (filters[k]) params.append(k, filters[k]);
        });

        showLoadingState(fieldId);

        jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params}`)
            .done(r => {
                state.allSchools = r.items || [];
                state.totalRecords = state.allSchools.length;

                renderPageFromCache(fieldId);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load schools`, err);
                showErrorState(fieldId);
            });
    };
    const renderPageFromCache = (fieldId) => {
        const state = instances.get(fieldId);
        const start = (state.currentPage - 1) * state.pageSize;
        const end = start + state.pageSize;
        const pageSchools = state.allSchools.slice(start, end);

        const tbody = ns.renderSchoolTable(
            fieldId,
            pageSchools,
            state.isReadOnly,
            state.selectedSchoolsMap
        );

        $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);
        renderPagination(fieldId);
        attachRowEvents(fieldId);
        initChildPickerForTable(fieldId);
    };

    const loadSchoolLevels = () =>
        jqClient().Get(API_ENDPOINTS.GETEDUCATION_LEVEL)
            .then(r => ns.schoolLevels = r?.result || []);

    const loadSchoolGenders = () =>
        jqClient().Get(API_ENDPOINTS.GET_SCHOOL_GENDER)
            .then(r => ns.schoolGenders = r?.result || []);

    const loadDepartmentConfig = () =>
        jqClient().Get(API_ENDPOINTS.GET_DEPARTMENT_CONFIG)
            .then(r => {
                try {
                    ns.depConfig = JSON.parse(r?.result || '{}');
                } catch (e) {
                    console.error('[PlanHandler] Failed to parse DepConfig', e);
                    ns.depConfig = {};
                }
            });

    /* ===================== PAGINATION (Client-Side) ===================== */

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
                    goToPage(fieldId, state.currentPage - 1);
                });
            prevItem.append(prevLink);
            pagination.append(prevItem);
        }

        const startPage = Math.max(1, state.currentPage - 2);
        const endPage = Math.min(totalPages, state.currentPage + 2);

        if (startPage > 1) {
            pagination.append(createPageItem(fieldId, 1, state.currentPage === 1));
            if (startPage > 2) {
                pagination.append($('<li>').addClass('page-item disabled').append(
                    $('<span>').addClass('page-link').text('...')
                ));
            }
        }

        for (let i = startPage; i <= endPage; i++) {
            pagination.append(createPageItem(fieldId, i, state.currentPage === i));
        }

        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                pagination.append($('<li>').addClass('page-item disabled').append(
                    $('<span>').addClass('page-link').text('...')
                ));
            }
            pagination.append(createPageItem(fieldId, totalPages, state.currentPage === totalPages));
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
                    goToPage(fieldId, state.currentPage + 1);
                });
            nextItem.append(nextLink);
            pagination.append(nextItem);
        }

        $p(fieldId, 'dtPagination').html(pagination);
    };

    
    const goToPage = (fieldId, pageNum) => {
        const state = instances.get(fieldId);
        state.currentPage = pageNum;
        renderPageFromCache(fieldId);
    };

    const createPageItem = (fieldId, pageNum, isActive) => {
        const pageItem = $('<li>').addClass('page-item');
        if (isActive) pageItem.addClass('active');

        const pageLink = $('<a>')
            .addClass('page-link')
            .attr('href', '#')
            .text(pageNum)
            .on('click', function (e) {
                e.preventDefault();
                if (!isActive) {
                    goToPage(fieldId, pageNum);
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
        $wrapper.off('change', pid(fieldId, 'showSelectedOnly'))
            .on('change', pid(fieldId, 'showSelectedOnly'), function () {
                onShowSelectedOnly(fieldId, this);
            });
        $wrapper.off('change', pid(fieldId, 'ddlPlanType'))
            .on('change', pid(fieldId, 'ddlPlanType'), function () {
                onPlanTypeChange(fieldId, this);
            });

        $wrapper.off('change', pid(fieldId, 'ddlSemester'))
            .on('change', pid(fieldId, 'ddlSemester'), function () {
                onSemesterChange(fieldId, this);
            });

        $wrapper.off('input', pid(fieldId, 'customSearch'))
            .on('input', pid(fieldId, 'customSearch'), function () {
                onSearch(fieldId, this);
            });

        $wrapper.off('submit', pid(fieldId, 'filterForm'))
            .on('submit', pid(fieldId, 'filterForm'), function (e) {
                onFilter(fieldId, e);
            });

        $wrapper.off('click', pid(fieldId, 'clearFiltersBtn'))
            .on('click', pid(fieldId, 'clearFiltersBtn'), function () {
                clearFilters(fieldId);
            });

        $wrapper.off('change', `#${pidRaw(fieldId, 'filterSchoolLevel')}`)
            .on('change', `#${pidRaw(fieldId, 'filterSchoolLevel')}`, function () {
                populateFilterGrades(fieldId, $(this).val());
            });
    };
    const onShowSelectedOnly = (fieldId, element) => {
        const state = instances.get(fieldId);
        const showOnly = $(element).is(':checked');

        if (showOnly) {

            const selectedIds = new Set(state.selectedSchoolsMap?.keys() || []);
            const filtered = state.allSchools.filter(s => selectedIds.has(s.id));

            
            state._allSchoolsBackup = state.allSchools;
            state.allSchools = filtered;
            state.totalRecords = filtered.length;
            state.currentPage = 1;
        } else {
            
            if (state._allSchoolsBackup) {
                state.allSchools = state._allSchoolsBackup;
                state.totalRecords = state.allSchools.length;
                state._allSchoolsBackup = null;
            }
            state.currentPage = 1;
        }

        renderPageFromCache(fieldId);
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

            if (newType) {
                const $checkbox = $table.find(`.selectRow[data-school-id="${schoolId}"]`);
                if (!$checkbox.is(':checked')) {
                    $checkbox.prop('checked', true);
                }

                if (!state.selectedSchoolsMap.has(schoolId)) {
                    const schoolName = $checkbox.data('name');
                    state.selectedSchoolsMap.set(schoolId, {
                        id: schoolId,
                        name: schoolName,
                        visitDate: $table.find(`.childDate[data-school-id="${schoolId}"]`).val() || '',
                        visitTypeId: newType
                    });
                } else {
                    state.selectedSchoolsMap.get(schoolId).visitTypeId = newType;
                }
                state.selectedSchools = Array.from(state.selectedSchoolsMap.values());
                updateSelectionCounter(fieldId);
            } else if (state.selectedSchoolsMap.has(schoolId)) {
                state.selectedSchoolsMap.get(schoolId).visitTypeId = newType;
                state.selectedSchools = Array.from(state.selectedSchoolsMap.values());
            }
        });

        const cfg = window.planUtility?.depConfig || {};
        if (fieldId === 'resendemail' && cfg.resendEmail === true) {
            $table.find('[data-action="resend-email"]').off('click').on('click', function (e) {
                e.preventDefault();
                const schoolId = $(this).data('id');
                const planId = state.planId;
                const $btn = $(this);

                $btn.addClass('disabled');

                jqClient().Post(API_ENDPOINTS.RESEND_EMAIL_SCHOOLS, { planId, schoolId })
                    .done(() => {
                        toastr?.success(t('ResendEmailSuccess'));
                    })
                    .fail(() => {
                        toastr?.error(t('ResendEmailFailed'));
                    })
                    .always(() => {
                        $btn.removeClass('disabled');
                    });
            });
        }
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
        const ay = ns.currentAcademicYear;
        if (ay?.start && ay?.end) {
            const start = ns.formatDateISO(ay.start);
            const end = ns.formatDateISO(ay.end);
            $p(fieldId, 'parentDate').val(`${start} to ${end}`).prop('disabled', true);
            ns.initChildPicker(ay.start, ay.end);
        } else {
            const y = new Date().getFullYear();
            const start = `${y}-01-01`;
            const end = `${y}-12-31`;
            $p(fieldId, 'parentDate').val(`${start} to ${end}`).prop('disabled', true);
            ns.initChildPicker(new Date(start), new Date(end));
        }
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

    /* ===================== SEARCH & FILTER ===================== */

    const onSearch = (fieldId, element) => {
        const state = instances.get(fieldId);
        state.searchTerm = $(element).val().trim();
        state.allSchools = [];

        clearTimeout(state.searchTimeout);
        state.searchTimeout = setTimeout(() => {
            loadSchools(fieldId, 1, state.filters);
        }, 300);
    };

    const onFilter = (fieldId, e) => {
        e.preventDefault();
        const state = instances.get(fieldId);

        const safeVal = (fieldId, name) => {
            const $el = $p(fieldId, name);
            return $el.length ? $el.val() : undefined;
        };

        state.filters = {
            name: safeVal(fieldId, 'filterSchoolName'),
            lastEvalDate: safeVal(fieldId, 'filterLastEvalDate'),
            establishmentDate: safeVal(fieldId, 'filterCreatedDate'),
            establishmentDateTo: safeVal(fieldId, 'filterToCreatedDate'),
            nextEvalDate: safeVal(fieldId, 'filterNextEvalDate'),
            fomrEvalMatrixValueId: safeVal(fieldId, 'filterPreviousResult'),
            visitType: safeVal(fieldId, 'filterVisitType'),
            schoolLevel: safeVal(fieldId, 'filterSchoolLevel'),
            gender: safeVal(fieldId, 'filterGender'),
            grade: safeVal(fieldId, 'filterGrade')
        };

        Object.keys(state.filters).forEach(k => state.filters[k] === undefined && delete state.filters[k]);

        state.allSchools = [];
        loadSchools(fieldId, 1, state.filters);

        const offcanvas = bootstrap.Offcanvas.getInstance($p(fieldId, 'filterOffcanvas')[0]);
        if (offcanvas) offcanvas.hide();
    };

    const clearFilters = (fieldId) => {
        ['filterSchoolName', 'filterLastEvalDate', 'filterCreatedDate', 'filterToCreatedDate',
            'filterNextEvalDate', 'filterPreviousResult', 'filterVisitType', 'filterParentOrgTree',
            'filterSchoolLevel', 'filterGender', 'filterGrade'].forEach(name => {
                const $el = $p(fieldId, name);
                if ($el.length) $el.val('');
            });
        if ($p(fieldId, 'filterGrade').length) populateFilterGrades(fieldId, '');
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