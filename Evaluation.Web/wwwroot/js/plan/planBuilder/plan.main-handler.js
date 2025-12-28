(function (global) {
    'use strict';

    const ns = global.planUtility;
    const {
        RENDER_TYPE,
        ACTION_TYPE,
        API_ENDPOINTS,
        PLAN_TYPE_BACKEND,
        TABLE_CONFIG
    } = global.PlanConstants;

    /* ===================== jqClient DEFINITION ===================== */

    if (typeof window.jqClient !== 'function') {
        window.jqClient = function () {
            return {
                Get: function (url) {
                    return $.ajax({
                        url: url,
                        method: 'GET',
                        dataType: 'json'
                    });
                },
                Post: function (url, data) {
                    return $.ajax({
                        url: url,
                        method: 'POST',
                        data: JSON.stringify(data),
                        contentType: 'application/json',
                        dataType: 'json'
                    });
                }
            };
        };
    }

    /* ===================== STATE ===================== */

    const state = {
        actionType: ACTION_TYPE.CREATE,
        planId: null,
        isReadOnly: false,
        pageSize: TABLE_CONFIG.pageSize || 10,
        currentPage: 1,
        filters: {},  // تهيئة الفلاتر
        searchTerm: '' // إضافة البحث
    };

    /* ===================== INIT ===================== */

    const init = async (actionType, isReadOnly, planId = null) => {
        state.actionType = actionType;
        state.planId = planId;
        state.isReadOnly = isReadOnly;
        console.log('[PlanHandler] Initializing...', state);

        try {
            // Load all required data
            await Promise.all([
                loadPlanTypes(),
                loadSemesters(),
                loadVisitTypes(),
                loadVacationDays()
            ]);

            // Populate plan types dropdown
            populatePlanTypes();

            // Populate semesters dropdown
            populateSemesters();

            if (state.planId) {
                await loadPlanData(state.planId);
            } else {
                renderNewPlan();
            }

            bindEvents();

            console.log('[PlanHandler] Initialization complete');

        } catch (err) {
            console.error('[PlanHandler] Initialization failed', err);
            alert('حدث خطأ أثناء تحميل الصفحة');
        }
    };

    /* ===================== DATA ===================== */

    const loadPlanTypes = () =>
        jqClient().Get(API_ENDPOINTS.GET_PLAN_TYPES)
            .then(r => {
                ns.planTypes = r?.result || [];
                console.log('[PlanHandler] Plan types loaded:', ns.planTypes);
            });

    const loadSemesters = () =>
        jqClient().Get(API_ENDPOINTS.GET_SEMESTERS)
            .then(r => {
                ns.semesters = r?.result || [];
                console.log('[PlanHandler] Semesters loaded:', ns.semesters);
            });

    const loadVisitTypes = () =>
        jqClient().Get(API_ENDPOINTS.GET_VISITS)
            .then(r => {
                ns.visitTypes = r?.result || [];
                console.log('[PlanHandler] Visit types loaded:', ns.visitTypes);
            });

    const loadVacationDays = () =>
        jqClient().Get(API_ENDPOINTS.GET_VACATION_DATES)
            .then(r => {
                const data = r?.result || [];
                ns.holidays = data.map(x => ({ date: x.date }));
                console.log('[PlanHandler] Holidays loaded:', ns.holidays);
            });

    const loadPlanData = async (planId) => {
        console.log('[PlanHandler] Loading plan data for ID:', planId);
        const res = await jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`);
        if (!res?.result) throw new Error('Invalid plan data');
        renderPlanWithData(res.result);
    };

    /* ===================== POPULATE DROPDOWNS ===================== */

    const populatePlanTypes = () => {
        const $select = $('#ddlPlanType');
        $select.empty().append($('<option>').val('').text('اختر نوع الخطة'));

        ns.planTypes.forEach(type => {
            $select.append(
                $('<option>')
                    .val(type.id)
                    .text(type.name)
                    .attr('data-backendname', type.backendName)
            );
        });

        if ($select.hasClass("select2-hidden-accessible")) {
            $select.select2('destroy');
        }
        $select.select2({
            placeholder: 'اختر نوع الخطة',
            width: '100%',
            allowClear: true
        });
    };

    const populateSemesters = () => {
        const $select = $('#ddlSemester');
        $select.empty().append($('<option>').val('').text('اختر الفصل الدراسي'));

        ns.semesters.forEach(semester => {
            $select.append(
                $('<option>')
                    .val(semester.id)
                    .text(semester.name)
                    .data('startDate', semester.startDate)
                    .data('endDate', semester.endDate)
            );
        });

        if ($select.hasClass("select2-hidden-accessible")) {
            $select.select2('destroy');
        }
        $select.select2({
            placeholder: 'اختر الفصل الدراسي',
            width: '100%',
            allowClear: true
        });
    };

    /* ===================== SCHOOLS ===================== */

    const loadSchoolsData = (page = 1, filters = {}) => {
        state.currentPage = page;
        state.filters = filters; // حفظ الفلاتر في الحالة

        const params = new URLSearchParams({
            page,
            pageSize: state.pageSize
        });

        // إضافة الفلاتر
        Object.keys(filters).forEach(k => {
            if (filters[k]) params.append(k, filters[k]);
        });

        // إضافة البحث
        if (state.searchTerm) {
            params.append('search', state.searchTerm);
        }

        showLoadingState();

        return jqClient()
            .Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params.toString()}`)
            .done(result => {
                const data = result?.items || [];

                ns.allSchools = data;
                ns.filteredSchools = data;

                const tbody = ns.renderSchoolTable(
                    ns.filteredSchools,
                    state.isReadOnly,
                    state.actionType
                );

                $('#planTable tbody').replaceWith(tbody);

                const total = result.totalCount || data.length;
                $('#dtPagination').html(ns.renderPagination(total));

                // تحديث عداد الفلاتر
                updateFilterBadge();

                reinitializeDatePickers();
                attachRowEvents();

                console.log('[PlanHandler] Schools loaded:', data.length);
            })
            .fail(err => {
                console.error('[PlanHandler] Failed to load schools', err);
                showErrorState();
            });
    };

    /* ===================== RENDER ===================== */

    const renderNewPlan = () => {
        console.log('[PlanHandler] Rendering new plan form');

        $('#planFormContainer .form-container').html(
            ns.renderPlanForm(null, state.isReadOnly, state.actionType)
        );

        initSelects();
        loadSchoolsData(1);
        initCustomMode();
    };

    const renderPlanWithData = (plan) => {
        console.log('[PlanHandler] Rendering plan with data:', plan);

        const vm = {
            title: plan.name || '',
            planTypeId: plan.planTypeId,
            semesterId: plan.semesterId,
            dateRange: formatRange(plan.startDate, plan.endDate),
            schools: plan.schools || []
        };

        $('#planFormContainer .form-container').html(
            ns.renderPlanForm(vm, state.actionType)
        );

        initSelects(vm);

        ns.selectedSchools = vm.schools.map(s => ({
            ...s,
            visitDate: s.visitDate || '',
            visitTypeId: s.visitTypeId || ''
        }));

        const tbody = ns.renderSchoolTable(
            vm.schools,
            state.isReadOnly,
            state.actionType
        );

        $('#planTable tbody').replaceWith(tbody);

        initializeDatePickersForPlan(vm);
        attachRowEvents();
    };

    /* ===================== UI ===================== */

    const initSelects = (data = {}) => {
        if ($('#ddlPlanType').hasClass("select2-hidden-accessible")) {
            $('#ddlPlanType').select2('destroy');
        }
        $('#ddlPlanType').select2({ width: '100%', allowClear: true });
        if (data.planTypeId) {
            $('#ddlPlanType').val(data.planTypeId).trigger('change');
        }

        if ($('#ddlSemester').hasClass("select2-hidden-accessible")) {
            $('#ddlSemester').select2('destroy');
        }
        $('#ddlSemester').select2({ width: '100%', allowClear: true });
        if (data.semesterId) {
            $('#ddlSemester').val(data.semesterId).trigger('change');
        }
    };

    /* ===================== EVENTS ===================== */

    const bindEvents = () => {
        console.log('[PlanHandler] Binding events');

        $(document)
            .off('change', '#ddlPlanType').on('change', '#ddlPlanType', onPlanTypeChange)
            .off('change', '#ddlSemester').on('change', '#ddlSemester', onSemesterChange)
            .off('click', '#btn-save-plan').on('click', '#btn-save-plan', onSaveClick)
            .off('click', '#btn-submit').on('click', '#btn-submit', onSubmit)
            .off('change', '.selectRow').on('change', '.selectRow', updateSelectedSchools)
            .off('change', '.childDate').on('change', '.childDate', onVisitDateChange)
            .off('change', '.visitTypeSelect').on('change', '.visitTypeSelect', onVisitTypeChange)
            .off('click', '#selectAll').on('click', '#selectAll', onSelectAll)
            .off('click', '.page-link').on('click', '.page-link', onPaginationClick)
            // إضافة أحداث الفلاتر
            .off('submit', '#filterForm').on('submit', '#filterForm', handleFilterSubmit)
            .off('click', '#clearFiltersBtn').on('click', '#clearFiltersBtn', handleClearFilters)
            .off('input', '#customSearch').on('input', '#customSearch', handleSearch);
    };

    const attachRowEvents = () => {
        $('.selectRow').off('change').on('change', updateSelectedSchools);
        $('.childDate').off('change').on('change', onVisitDateChange);
        $('.visitTypeSelect').off('change').on('change', onVisitTypeChange);
    };

    /* ===================== HANDLERS ===================== */

    const onPlanTypeChange = function () {
        const backend = $(this).find(':selected').data('backendname');
        console.log('[PlanHandler] Plan type changed:', backend);

        $('#semesterContainer').hide();
        ns.destroyChildPicker();

        switch (backend) {
            case PLAN_TYPE_BACKEND.YEAR: return initYearMode();
            case PLAN_TYPE_BACKEND.MONTH: return initMonthMode();
            case PLAN_TYPE_BACKEND.SEMESTER: return initSemesterMode();
            default: return initCustomMode();
        }
    };

    const onSemesterChange = function () {
        const opt = $(this).find(':selected');
        if (!opt.length) return;

        const start = new Date(opt.data('startdate'));
        const end = new Date(opt.data('enddate'));

        $('#parentDate')
            .val(`${ns.formatDateISO(start)} to ${ns.formatDateISO(end)}`)
            .prop('disabled', true);

        ns.initChildPicker(start, end);
        console.log('[PlanHandler] Semester changed, date range set');
    };

    const onVisitDateChange = function () {
        const id = $(this).data('school-id');
        const school = ns.selectedSchools.find(s => s.id === id);
        if (school) {
            school.visitDate = $(this).val();
            console.log('[PlanHandler] Visit date updated for school:', id);
        }
    };

    const onVisitTypeChange = function () {
        const id = $(this).data('school-id');
        const school = ns.selectedSchools.find(s => s.id === id);
        if (school) {
            school.visitTypeId = $(this).val();
            console.log('[PlanHandler] Visit type updated for school:', id);
        }
    };

    const onSelectAll = function () {
        const isChecked = $(this).prop('checked');
        $('.selectRow').prop('checked', isChecked);
        updateSelectedSchools();
    };

    const onPaginationClick = function (e) {
        e.preventDefault();
        const page = parseInt($(this).data('page'));
        if (page) {
            loadSchoolsData(page, state.filters);
        }
    };

    const onSaveClick = function (e) {
        e.preventDefault();
        if (!validate()) return;

        updateSelectedSchools();
        $('#confirmationMessage').text(`تم تحديد (${ns.selectedSchools.length}) مدرسة للإضافة للخطة`);

        const modal = new bootstrap.Modal(document.getElementById('confirmation-modal'));
        modal.show();
    };

    const onSubmit = () => {
        if (!validate()) return;
        save(collect());
    };

    /* ===================== FILTER HANDLERS ===================== */

    const handleFilterSubmit = function (e) {
        e.preventDefault();
        console.log('[PlanHandler] Applying filters');

        // جمع قيم الفلاتر
        const filters = {
            Name: $('#filterSchoolName').val()?.trim(),
            lastEvalDate: $('#filterLastEvalDate').val(),
            establishmentDate: $('#filterCreatedDate').val(),
            nextEvalDate: $('#filterNextEvalDate').val(),
            previousResult: $('#filterPreviousResult').val(),
            visitType: $('#filterVisitType').val()
        };

        // إزالة القيم الفارغة
        Object.keys(filters).forEach(key => {
            if (!filters[key]) delete filters[key];
        });

        // تطبيق الفلاتر
        state.filters = filters;
        state.currentPage = 1;
        loadSchoolsData(1, filters);

        // إغلاق القائمة الجانبية
        const offcanvas = bootstrap.Offcanvas.getInstance(document.getElementById('filterOffcanvas'));
        if (offcanvas) offcanvas.hide();
    };

    const handleClearFilters = function (e) {
        e.preventDefault();
        console.log('[PlanHandler] Clearing filters');

        // مسح النموذج
        $('#filterForm')[0].reset();

        // مسح الفلاتر
        state.filters = {};
        state.currentPage = 1;

        // تحديث عداد الفلاتر
        updateFilterBadge();

        // إعادة تحميل البيانات
        loadSchoolsData(1, {});
    };

    const handleSearch = function () {
        const term = $(this).val()?.trim() || '';
        console.log('[PlanHandler] Searching:', term);

        // تأخير البحث
        clearTimeout(state.searchTimeout);
        state.searchTimeout = setTimeout(() => {
            state.searchTerm = term;
            state.currentPage = 1;
            loadSchoolsData(1, state.filters);
        }, 300);
    };

    const updateFilterBadge = () => {
        const count = Object.keys(state.filters).length;
        const $badge = $('.filterbtn .badge');

        if (count > 0) {
            $badge.text(count).show();
        } else {
            $badge.hide();
        }

        console.log('[PlanHandler] Filter badge updated:', count);
    };

    /* ===================== DATE MODES ===================== */

    const initYearMode = () => {
        const y = new Date().getFullYear();
        const start = `${y}-01-01`;
        const end = `${y}-12-31`;

        $('#parentDate').val(`${start} to ${end}`).prop('disabled', true);
        ns.initChildPicker(new Date(start), new Date(end));
        console.log('[PlanHandler] Year mode initialized');
    };

    const initMonthMode = () => {
        $('#parentDate').val('').prop('disabled', false);
        ns.initParentPicker('month');
        console.log('[PlanHandler] Month mode initialized');
    };

    const initSemesterMode = () => {
        $('#semesterContainer').show();
        $('#parentDate').val('').prop('disabled', true);
        console.log('[PlanHandler] Semester mode initialized');
    };

    const initCustomMode = () => {
        $('#parentDate').val('').prop('disabled', false);
        ns.initParentPicker('custom');
        console.log('[PlanHandler] Custom mode initialized');
    };

    const initializeDatePickersForPlan = (plan) => {
        if (!plan.dateRange) return;
        const range = getDateRangeFromInput();
        if (range) {
            ns.initChildPicker(range.startDateObj, range.endDateObj);
        }
    };

    const reinitializeDatePickers = () => {
        const range = getDateRangeFromInput();
        if (range) {
            ns.initChildPicker(range.startDateObj, range.endDateObj);
        }
    };

    /* ===================== SAVE ===================== */

    const collect = () => {
        const range = getDateRangeFromInput();

        return {
            id: state.planId,
            title: $('#planTitle').val(),
            planTypeId: $('#ddlPlanType').val(),
            semesterId: $('#ddlSemester').val() || null,
            startDate: range?.startDate,
            endDate: range?.endDate,
            schools: ns.selectedSchools.map(s => ({
                schoolId: s.id,
                visitDate: s.visitDate,
                visitTypeId: s.visitTypeId
            }))
        };
    };

    const save = (payload) => {
        console.log('[PlanHandler] Saving plan:', payload);

        const url = payload.id
            ? API_ENDPOINTS.UPDATE_PLAN
            : API_ENDPOINTS.INSERTORUPUDATEPLAN;

        jqClient().Post(url, payload)
            .done(() => {
                console.log('[PlanHandler] Plan saved successfully');
                alert('تم حفظ الخطة بنجاح');
                location.href = '/Plan/Index';
            })
            .fail(err => {
                console.error('[PlanHandler] Save failed', err);
                alert('حدث خطأ أثناء الحفظ');
            });
    };

    /* ===================== VALIDATION ===================== */

    const validate = () => {
        if (!$('#planTitle').val()) {
            alert('يرجى إدخال عنوان الخطة');
            return false;
        }

        if (!$('#ddlPlanType').val()) {
            alert('يرجى اختيار نوع الخطة');
            return false;
        }

        const range = getDateRangeFromInput();
        if (!range) {
            alert('يرجى اختيار الفترة الزمنية');
            return false;
        }

        updateSelectedSchools();

        if (!ns.selectedSchools.length) {
            alert('يرجى اختيار مدرسة واحدة على الأقل');
            return false;
        }

        for (let school of ns.selectedSchools) {
            if (!school.visitDate) {
                alert(`يرجى تحديد تاريخ الزيارة للمدرسة: ${school.name}`);
                return false;
            }
            if (!school.visitTypeId) {
                alert(`يرجى تحديد نوع الزيارة للمدرسة: ${school.name}`);
                return false;
            }
        }

        return true;
    };

    const getDateRangeFromInput = () => {
        const dateRangeStr = $('#parentDate').val();

        if (!dateRangeStr || dateRangeStr.trim() === '') {
            return null;
        }

        let parts = dateRangeStr.split(' إلى ');
        if (parts.length !== 2) {
            parts = dateRangeStr.split(' to ');
        }

        if (parts.length === 2) {
            const startStr = parts[0].trim();
            const endStr = parts[1].trim();

            const storedStart = $('#parentDate').data('startDate');
            const storedEnd = $('#parentDate').data('endDate');

            return {
                startDate: storedStart || startStr,
                endDate: storedEnd || endStr,
                startDateObj: new Date(storedStart || startStr),
                endDateObj: new Date(storedEnd || endStr)
            };
        }

        return null;
    };

    const updateSelectedSchools = () => {
        ns.selectedSchools = [];

        $('.selectRow:checked').each(function () {
            const id = $(this).data('id');
            const school = ns.allSchools.find(s => s.id === id);
            if (!school) return;

            ns.selectedSchools.push({
                ...school,
                visitDate: $(`.childDate[data-school-id="${id}"]`).val(),
                visitTypeId: $(`.visitTypeSelect[data-school-id="${id}"]`).val()
            });
        });

        console.log('[PlanHandler] Selected schools updated:', ns.selectedSchools.length);
    };

    /* ===================== UI STATES ===================== */

    const showLoadingState = () => {
        $('#planTable tbody').html(`
            <tr>
                <td colspan="7" class="text-center py-5">
                    <div class="spinner-border text-primary"></div>
                    <p class="mt-2 text-muted">جاري تحميل البيانات...</p>
                </td>
            </tr>
        `);
    };

    const showErrorState = () => {
        $('#planTable tbody').html(`
            <tr>
                <td colspan="7" class="text-center text-danger py-5">
                    حدث خطأ أثناء تحميل البيانات
                </td>
            </tr>
        `);
    };

    const formatRange = (s, e) =>
        s && e ? `${ns.formatDateISO(new Date(s))} to ${ns.formatDateISO(new Date(e))}` : '';

    /* ===================== EXPORT ===================== */

    global.PlanHandler = Object.freeze({
        init,
        loadSchools: loadSchoolsData,
        applyFilters: (filters) => loadSchoolsData(1, filters),
        clearFilters: handleClearFilters,
        getState: () => ({ ...state })
    });

})(window);