(function (global) {
    'use strict';

    const ns = global.planUtility;
    const {
        API_ENDPOINTS,
        PLAN_TYPE_BACKEND,
        TABLE_CONFIG
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
    // ✅ تخزين state منفصل لكل instance
    const instances = new Map();

    /* ===================== INSTANCE STATE ===================== */
    function createInstanceState(fieldId) {
        return {
            fieldId: fieldId,
            isReadOnly: false,
            pageSize: TABLE_CONFIG.pageSize || 10,
            currentPage: 1,
            filters: {},
            searchTerm: '',
            selectedSchools: [],
            allSchools: []
        };
    }

    /* ===================== PREFIX HELPERS ===================== */

    const pid = (fieldId, id) => `#${fieldId}_${id}`;
    const pidRaw = (fieldId, id) => `${fieldId}_${id}`;
    const $p = (fieldId, id) => window.jQuery(pid(fieldId, id));

    /* ===================== INIT ===================== */

    const init = async (isReadOnly, fieldId = null, planObject = null) => {
        if (!fieldId) {
            console.error('[PlanHandler] fieldId is required!');
            return;
        }
        const state = createInstanceState(fieldId);
        state.isReadOnly = isReadOnly;

        instances.set(fieldId, state);

        try {
            // ✅ تحميل البيانات المشتركة (مرة واحدة فقط)
            if (!ns.planTypes || ns.planTypes.length === 0) {
                await Promise.all([
                    loadPlanTypes(),
                    loadSemesters(),
                    loadVisitTypes(),
                    loadVacationDays()
                ]);
            }

            populatePlanTypes(fieldId);
            populateSemesters(fieldId);

            if (planObject) {
                renderPlanWithData(fieldId, planObject);
            //}
            //else if (planId) {
            //    await loadPlanData(fieldId, planId);
            } else {
                renderNewPlan(fieldId);
            }

            bindEvents(fieldId);

            console.log(`[PlanHandler] Instance ${fieldId} initialized successfully`);

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
            .then(r => ns.holidays = (r?.result || []).map(x => ({ date: x.date })));

    const loadPlanData = async (fieldId, planId) => {
        const r = await jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`);
        renderPlanWithData(fieldId, r.result);
    };

    /* ===================== POPULATE ===================== */

    const populatePlanTypes = (fieldId) => {
        const $s = $p(fieldId, 'ddlPlanType');

        // Destroy existing select2
        if ($s.hasClass("select2-hidden-accessible")) {
            $s.select2('destroy');
        }

        $s.empty().append('<option value="">اختر نوع الخطة</option>');

        ns.planTypes.forEach(t =>
            $s.append(`<option value="${t.id}" data-backendname="${t.backendName}">${t.name}</option>`)
        );

        $s.select2({ width: '100%', allowClear: true });
    };

    const populateSemesters = (fieldId) => {
        const $s = $p(fieldId, 'ddlSemester');

        // Destroy existing select2
        if ($s.hasClass("select2-hidden-accessible")) {
            $s.select2('destroy');
        }

        $s.empty().append('<option value="">اختر الفصل الدراسي</option>');

        ns.semesters.forEach(s =>
            $s.append(
                `<option value="${s.id}"
                         data-startdate="${s.startDate}"
                         data-enddate="${s.endDate}">
                    ${s.name}
                 </option>`
            )
        );

        $s.select2({ width: '100%', allowClear: true });
    };

    /* ===================== RENDER ===================== */

    const renderNewPlan = (fieldId) => {
        const state = instances.get(fieldId);

        // ✅ Render form with fieldId
        const form = ns.renderPlanForm(fieldId, null, state.isReadOnly);
        $p(fieldId, 'planFormContainer').find('.form-container').html(form);

        loadSchools(fieldId, 1);
        initCustomMode(fieldId);
    };

    const renderPlanWithData = (fieldId, plan) => {
        const state = instances.get(fieldId);

        const vm = {
            title: plan.name || '',
            planTypeId: plan.planTypeDepId || plan.PlanTypeDepId,
            semesterId: plan.semesterId,
            dateRange: plan.startDate && plan.endDate ?
                `${plan.startDate} to ${plan.endDate}` : '',
            schools: plan.schools || []
        };

        // ✅ Render form with fieldId
        const form = ns.renderPlanForm(fieldId, vm, state.isReadOnly);
        $p(fieldId, 'planFormContainer').find('.form-container').html(form);

        // Re-initialize selects after rendering
        populatePlanTypes(fieldId);
        populateSemesters(fieldId);

        // Set values
        $p(fieldId, 'planTitle').val(vm.title);
        $p(fieldId, 'ddlPlanType').val(vm.planTypeId).trigger('change');
        if (vm.semesterId) {
            $p(fieldId, 'ddlSemester').val(vm.semesterId).trigger('change');
        }
        $p(fieldId, 'parentDate').val(vm.dateRange);

        // ✅ حفظ المدارس المحددة
        state.selectedSchools = vm.schools.map(s => ({
            ...s,
            id: s.id || s.schoolId,
            visitDate: s.visitDate || (s.startEvaluationDate && s.endEvaluationDate ?
                `${s.startEvaluationDate} to ${s.endEvaluationDate}` : ''),
            visitTypeId: s.visitTypeId
        }));

        // ✅ Render schools table
        const tbody = ns.renderSchoolTable(fieldId, vm.schools, state.isReadOnly);
        $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);

        attachRowEvents(fieldId);
        initializeDatePickers(fieldId, vm);
    };

    /* ===================== SCHOOLS ===================== */

    const loadSchools = (fieldId, page = 1, filters = {}) => {
        const state = instances.get(fieldId);
        state.currentPage = page;
        state.filters = filters;

        const params = new URLSearchParams({
            page,
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

                const tbody = ns.renderSchoolTable(
                    fieldId,
                    state.allSchools,
                    state.isReadOnly
                );

                $p(fieldId, 'planTable').find('tbody').replaceWith(tbody);

                attachRowEvents(fieldId);
                initChildPickerForTable(fieldId);

                console.log(`[PlanHandler] Schools loaded for ${fieldId}:`, state.allSchools.length);
            })
            .fail(err => {
                console.error(`[PlanHandler] Failed to load schools for ${fieldId}`, err);
                showErrorState(fieldId);
            });
    };

    /* ===================== EVENTS ===================== */

    const bindEvents = (fieldId) => {
        console.log(`[PlanHandler] Binding events for ${fieldId}`);

        // ✅ استخدام event delegation على الـ wrapper
        const $wrapper = $p(fieldId, 'wrapper');

        if (!$wrapper.length) {
            console.error(`[PlanHandler] Wrapper not found for ${fieldId}`);
            return;
        }

        // Form events
        $wrapper.off('change', pid(fieldId, 'ddlPlanType'))
            .on('change', pid(fieldId, 'ddlPlanType'), function () {
                onPlanTypeChange(fieldId, this);
            });

        $wrapper.off('change', pid(fieldId, 'ddlSemester'))
            .on('change', pid(fieldId, 'ddlSemester'), function () {
                onSemesterChange(fieldId, this);
            });

        $wrapper.off('click', pid(fieldId, 'btnSavePlan'))
            .on('click', pid(fieldId, 'btnSavePlan'), function (e) {
                onSaveClick(fieldId, e);
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

        console.log(`[PlanHandler] Events bound for ${fieldId}`);
    };

    const attachRowEvents = (fieldId) => {
        const $table = $p(fieldId, 'planTable');

        $table.find('.selectRow').off('change').on('change', function () {
            updateSelectedSchools(fieldId);
        });

        $table.find('.childDate').off('change').on('change', function () {
            updateSelectedSchools(fieldId);
        });

        $table.find('.visitTypeSelect').off('change').on('change', function () {
            updateSelectedSchools(fieldId);
        });
    };

    /* ===================== HANDLERS ===================== */

    const onPlanTypeChange = (fieldId, element) => {
        const backend = $(element).find(':selected').data('backendname');

        console.log(`[PlanHandler] Plan type changed for ${fieldId}:`, backend);

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

    /* ===================== SAVE ===================== */

    const collect = (fieldId) => {
        const state = instances.get(fieldId);

        return {
            fieldId: fieldId,
            title: $p(fieldId, 'planTitle').val(),
            planTypeId: $p(fieldId, 'ddlPlanType').val(),
            semesterId: $p(fieldId, 'ddlSemester').val(),
            dateRange: $p(fieldId, 'parentDate').val(),
            schools: state.selectedSchools.map(s => ({
                schoolId: s.id,
                visitDate: s.visitDate,
                visitTypeId: s.visitTypeId
            }))
        };
    };

    /* ===================== HELPERS ===================== */

    const updateSelectedSchools = (fieldId) => {
        const state = instances.get(fieldId);
        state.selectedSchools = [];

        $p(fieldId, 'planTable').find('.selectRow:checked').each(function () {
            const schoolId = $(this).data('school-id');
            const school = state.allSchools.find(s => s.id === schoolId);
            if (!school) return;

            state.selectedSchools.push({
                ...school,
                visitDate: $p(fieldId, 'planTable').find(`.childDate[data-school-id="${schoolId}"]`).val(),
                visitTypeId: $p(fieldId, 'planTable').find(`.visitTypeSelect[data-school-id="${schoolId}"]`).val()
            });
        });

        console.log(`[PlanHandler] Selected schools updated for ${fieldId}:`, state.selectedSchools.length);
    };

    const onSaveClick = (fieldId, e) => {
        e.preventDefault();
        const payload = collect(fieldId);
        console.log(`[PlanHandler] SAVE PAYLOAD for ${fieldId}:`, payload);
    };

    const onSearch = (fieldId, element) => {
        const state = instances.get(fieldId);
        state.searchTerm = $(element).val();

        clearTimeout(state.searchTimeout);
        state.searchTimeout = setTimeout(() => {
            loadSchools(fieldId, 1, state.filters);
        }, 300);
    };

    const onFilter = (fieldId, e) => {
        e.preventDefault();
        const state = instances.get(fieldId);
        loadSchools(fieldId, 1, state.filters);
    };

    const clearFilters = (fieldId) => {
        const state = instances.get(fieldId);
        state.filters = {};
        loadSchools(fieldId, 1);
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
                    حدث خطأ أثناء تحميل البيانات
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

})(window);