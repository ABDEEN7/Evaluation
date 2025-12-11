// ============= MAIN PLAN HANDLER =============

(function () {
    const {
        RENDER_TYPE,
        ACTION_TYPE,
        API_ENDPOINTS,
        PLAN_TYPE_BACKEND,
        TABLE_CONFIG
    } = window.PlanConstants || {};

    const ns = window.planUtility;

    // ================== INITIALIZATION ==================
    jsPlan(ns);
    function jsPlan(ns) {
        return ns;
    }
    const initializePage = (options = {}) => {
        const {
            renderType = RENDER_TYPE.ACTION,
            actionType = ACTION_TYPE.CREATE,
            planId = null,
            oldPlanId = null
        } = options;

        // Store current mode
        ns.currentRenderType = renderType;
        ns.currentActionType = actionType;
        ns.currentPlanId = planId;

        // Load initial data
        Promise.all([
            loadPlanTypes(),
            loadSemesters(),
            loadVisitTypes(),
            loadVacationDays()
        ]).then(() => {
            if (renderType === RENDER_TYPE.COMPARISON && oldPlanId && planId) {
                // Load both plans for comparison
                loadComparisonView(oldPlanId, planId);
            } else if (planId) {
                // Load existing plan
                loadPlanData(planId, renderType, actionType);
            } else {
                // New plan
                renderNewPlan(renderType, actionType);
            }
        });

        // Initialize event handlers
        initializeEventHandlers();
    };

    // ================== DATA LOADING ==================

    const loadPlanTypes = () => {
        return jqClient().Get(API_ENDPOINTS.GET_PLAN_TYPES)
            .done(result => {
                ns.planTypes = result?.result || [];
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load plan types failed', textStatus, err);
            });
    };

    const loadSemesters = () => {
        return jqClient().Get(API_ENDPOINTS.GET_SEMESTERS)
            .done(result => {
                ns.semesters = result?.result || [];
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load semesters failed', textStatus, err);
            });
    };

    const loadVisitTypes = () => {
        return jqClient().Get(API_ENDPOINTS.GET_VISITS)
            .done(result => {
                ns.visitTypes = result?.result || [];
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load visit types failed', textStatus, err);
            });
    };

    const loadVacationDays = () => {
        return jqClient().Get(API_ENDPOINTS.GET_VACATION_DATES)
            .done(result => {
                const data = result?.result || [];
                ns.holidays = data.map(item => ({ date: item.date }));
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load vacation days failed', textStatus, err);
            });
    };

    const loadSchoolsData = (page = 1, filters = {}) => {
        const params = new URLSearchParams({
            page: page,
            pageSize: ns.pageSize || TABLE_CONFIG.pageSize
        });

        // Add filters
        Object.keys(filters).forEach(key => {
            if (filters[key]) {
                params.append(key, filters[key]);
            }
        });

        showLoadingState();

        return jqClient().Get(`${API_ENDPOINTS.GET_SCHOOLS}?${params.toString()}`)
            .done(result => {
                const data = result?.items || [];
                ns.allSchools = data;
                ns.filteredSchools = data;

                // Render table
                const tbody = ns.renderSchoolTable(
                    ns.filteredSchools,
                    ns.currentRenderType,
                    ns.currentActionType
                );
                $('#planTable tbody').replaceWith(tbody);

                // Render pagination
                const totalRecords = result.totalCount || ns.filteredSchools.length;
                const pagination = ns.renderPagination(totalRecords);
                $('#dtPagination').html(pagination);

                // Re-initialize date pickers
                reinitializeDatePickers();

                // Attach event handlers to new rows
                attachRowEventHandlers();
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load schools failed', textStatus, err);
                showErrorState();
            });
    };

    const loadPlanData = (planId, renderType, actionType) => {
        return jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}?planId=${planId}`)
            .done(result => {
                const planData = result?.result;
                renderPlanWithData(planData, renderType, actionType);
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load plan data failed', textStatus, err);
            });
    };

    const loadComparisonView = (oldPlanId, newPlanId) => {
        Promise.all([
            jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}?planId=${oldPlanId}`),
            jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}?planId=${newPlanId}`)
        ]).then(([oldResult, newResult]) => {
            const oldPlan = oldResult?.result;
            const newPlan = newResult?.result;

            const comparisonView = ns.generateComparisonView(oldPlan, newPlan);
            $('#planFormContainer').html(comparisonView);
        }).catch(err => {
            console.error('Load comparison failed', err);
        });
    };

    // ================== RENDERING ==================

    const renderNewPlan = (renderType, actionType) => {
        // Render empty plan form
        const planForm = ns.renderPlanForm(null, renderType, actionType);
        $('#planFormContainer').html(planForm);

        // Initialize Select2 for dropdowns
        $('#ddlPlanType').select2({
            placeholder: "اختر نوع الخطة",
            allowClear: true,
            width: '100%'
        });

        $('#ddlSemester').select2({
            placeholder: "اختر الفصل الدراسي",
            allowClear: true,
            width: '100%'
        });

        // Load schools
        loadSchoolsData(1);

        // Initialize date picker in custom mode by default
        initCustomMode();
    };

    const renderPlanWithData = (planData, renderType, actionType) => {
        // Render plan form with data
        const planForm = ns.renderPlanForm(planData, renderType, actionType);
        $('#planFormContainer').html(planForm);

        // Initialize Select2
        $('#ddlPlanType').select2({
            placeholder: "اختر نوع الخطة",
            allowClear: true,
            width: '100%'
        });

        $('#ddlSemester').select2({
            placeholder: "اختر الفصل الدراسي",
            allowClear: true,
            width: '100%'
        });

        // Set selected schools
        ns.selectedSchools = planData.schools || [];

        // Render schools table with data
        const tbody = ns.renderSchoolTable(
            planData.schools || [],
            renderType,
            actionType
        );
        $('#planTable tbody').replaceWith(tbody);

        // Initialize date pickers based on plan type
        initializeDatePickersForPlanType(planData);

        // Attach event handlers
        attachRowEventHandlers();
    };

    const showLoadingState = () => {
        const tbody = $('#planTable tbody');
        tbody.html(`
            <tr>
                <td colspan="7" class="text-center py-5">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">جاري التحميل...</span>
                    </div>
                    <p class="mt-2 text-muted">جاري تحميل البيانات...</p>
                </td>
            </tr>
        `);
    };

    const showErrorState = () => {
        const tbody = $('#planTable tbody');
        tbody.html(`
            <tr>
                <td colspan="7" class="text-center py-5 text-danger">
                    <i class="la la-exclamation-triangle fs-1"></i>
                    <p class="mt-2">حدث خطأ أثناء تحميل البيانات</p>
                </td>
            </tr>
        `);
    };

    // ================== EVENT HANDLERS ==================

    const initializeEventHandlers = () => {
        // Plan type change
        $(document).on('change', '#ddlPlanType', handlePlanTypeChange);

        // Semester change
        $(document).on('change', '#ddlSemester', handleSemesterChange);

        // Select all checkbox
        $(document).on('change', '#selectAll', handleSelectAll);

        // Search
        let searchTimeout;
        $(document).on('input', '#customSearch', function () {
            clearTimeout(searchTimeout);
            const term = $(this).val().trim();
            searchTimeout = setTimeout(() => performSearch(term), 300);
        });

        // Filter form
        $(document).on('submit', '#filterForm', handleFilterSubmit);
        $(document).on('click', '#clearFiltersBtn', handleClearFilters);

        // Pagination
        $(document).on('click', '#dtPagination .page-link', handlePaginationClick);

        // Save button
        //$(document).on('click', '#btn-submit', handleSavePlan);

        // Open confirmation modal
        $(document).on('click', '[data-bs-target="#confirmation-modal"]', handleOpenConfirmation);
    };

    const handlePlanTypeChange = function () {
        const selectedOption = $(this).select2('data')[0];

        if (!selectedOption) {
            $('#semesterContainer').hide();
            initCustomMode();
            return;
        }

        const backendName = selectedOption.element?.dataset?.backendname || selectedOption.backendName;

        // Hide semester by default
        $('#semesterContainer').hide();
        ns.destroyChildPicker();

        switch (backendName) {
            case PLAN_TYPE_BACKEND.YEAR:
                initYearMode();
                break;
            case PLAN_TYPE_BACKEND.MONTH:
                initMonthMode();
                break;
            case PLAN_TYPE_BACKEND.SEMESTER:
                initSemesterMode();
                break;
            default:
                initCustomMode();
                break;
        }
    };

    const handleSemesterChange = function () {
        const selectedOption = $(this).select2('data')[0];

        if (!selectedOption) {
            ns.destroyChildPicker();
            $('#parentDate').val('');
            return;
        }

        const startDate = selectedOption.element?.dataset?.startdate || selectedOption.startDate;
        const endDate = selectedOption.element?.dataset?.enddate || selectedOption.endDate;

        if (startDate && endDate) {
            const start = new Date(startDate);
            const end = new Date(endDate);

            const startStr = ns.formatDateISO(start);
            const endStr = ns.formatDateISO(end);

            $('#parentDate').val(`${startStr} to ${endStr}`).prop('disabled', true);

            // Store dates in data attributes
            $('#parentDate').data('startDate', startStr);
            $('#parentDate').data('endDate', endStr);

            ns.initChildPicker(start, end);
        }
    };

    const handleSelectAll = function () {
        const isChecked = $(this).is(':checked');
        $('.selectRow').not(':disabled').prop('checked', isChecked);
        updateSelectedSchools();
    };

    const handleFilterSubmit = function (e) {
        e.preventDefault();

        ns.currentFilters = {
            Name: $('#filterSchoolName').val().trim(),
            lastEvalDate: $('#filterLastEvalDate').val(),
            establishmentDate: $('#filterCreatedDate').val(),
            nextEvalDate: $('#filterNextEvalDate').val(),
            previousResult: $('#filterPreviousResult').val(),
            visitType: $('#filterVisitType').val()
        };

        // Remove empty filters
        Object.keys(ns.currentFilters).forEach(key => {
            if (!ns.currentFilters[key]) delete ns.currentFilters[key];
        });

        updateFilterBadge();
        ns.currentPage = 1;
        loadSchoolsData(1, ns.currentFilters);

        // Close offcanvas
        const offcanvas = bootstrap.Offcanvas.getInstance(document.getElementById('filterOffcanvas'));
        if (offcanvas) offcanvas.hide();
    };

    const handleClearFilters = function () {
        $('#filterForm')[0].reset();
        ns.currentFilters = {};
        updateFilterBadge();
        ns.currentPage = 1;
        loadSchoolsData(1);
    };

    const handlePaginationClick = function (e) {
        e.preventDefault();
        const page = parseInt($(this).data('page'));
        ns.currentPage = page;
        loadSchoolsData(page, ns.currentFilters);
    };

    const handleOpenConfirmation = function (e) {
        e.preventDefault();
        updateSelectedSchools();
        const count = ns.selectedSchools.length;
        $('#confirmationMessage').html(`تم تحديد (${count.toString().padStart(2, '0')}) مدرسة للإضافة للخطة`);
    };

    const handleSavePlan = function () {
        if (validatePlan()) {
            const planData = collectPlanData();
            savePlan(planData);
        }
    };

    const attachRowEventHandlers = () => {
        // Checkbox change
        $('.selectRow').off('change').on('change', function () {
            updateSelectedSchools();
        });

        // Visit date change
        $('.childDate').off('change').on('change', function () {
            const schoolId = $(this).data('school-id');
            const dateRange = $(this).val();
            updateSchoolVisitDate(schoolId, dateRange);
        });

        // Visit type change
        $('.visitTypeSelect').off('change').on('change', function () {
            const schoolId = $(this).data('school-id');
            const visitTypeId = $(this).val();
            updateSchoolVisitType(schoolId, visitTypeId);
        });
    };

    // ================== HELPER FUNCTIONS ==================

    const initYearMode = () => {
        const currentYear = new Date().getFullYear();
        const startDate = `${currentYear}-01-01`;
        const endDate = `${currentYear}-12-31`;

        $('#parentDate').val(`${startDate} to ${endDate}`).prop('disabled', true);

        // Store dates in data attributes
        $('#parentDate').data('startDate', startDate);
        $('#parentDate').data('endDate', endDate);

        const minDate = new Date(currentYear, 0, 1);
        const maxDate = new Date(currentYear, 11, 31);
        ns.initChildPicker(minDate, maxDate);
    };

    const initMonthMode = () => {
        // Enable parent date picker in month selection mode
        $('#parentDate').val('').prop('disabled', false).attr('placeholder', 'اختر الشهر');
        ns.initParentPicker('month');
        ns.destroyChildPicker();
    };

    const initSemesterMode = () => {
        $('#semesterContainer').show();
        $('#parentDate').val('').prop('disabled', true);
        ns.destroyChildPicker();
    };

    const initCustomMode = () => {
        $('#parentDate').val('').prop('disabled', false).attr('placeholder', 'اختر تاريخ بداية ونهاية الخطة');
        ns.initParentPicker('custom');
        ns.destroyChildPicker();
    };

    const initializeDatePickersForPlanType = (planData) => {
        const planType = ns.planTypes.find(t => t.id === planData.planTypeId);

        if (!planType) {
            initCustomMode();
            if (planData.dateRange) {
                $('#parentDate').val(planData.dateRange);
                const range = getDateRangeFromInput();
                if (range) {
                    ns.initChildPicker(range.startDateObj, range.endDateObj);
                }
            }
            return;
        }

        switch (planType.backendName) {
            case PLAN_TYPE_BACKEND.YEAR:
                initYearMode();
                break;

            case PLAN_TYPE_BACKEND.MONTH:
                // For existing month plan, show the selected month
                if (planData.dateRange) {
                    $('#parentDate').val(planData.dateRange).prop('disabled', false);
                    const range = getDateRangeFromInput();
                    if (range) {
                        // Store in data attributes
                        $('#parentDate').data('startDate', range.startDate);
                        $('#parentDate').data('endDate', range.endDate);
                        ns.initParentPicker('month');
                        ns.initChildPicker(range.startDateObj, range.endDateObj);
                    }
                } else {
                    initMonthMode();
                }
                break;

            case PLAN_TYPE_BACKEND.SEMESTER:
                $('#semesterContainer').show();
                if (planData.dateRange) {
                    $('#parentDate').val(planData.dateRange).prop('disabled', true);
                    const range = getDateRangeFromInput();
                    if (range) {
                        $('#parentDate').data('startDate', range.startDate);
                        $('#parentDate').data('endDate', range.endDate);
                        ns.initChildPicker(range.startDateObj, range.endDateObj);
                    }
                }
                break;

            default:
                if (planData.dateRange) {
                    $('#parentDate').val(planData.dateRange);
                    const range = getDateRangeFromInput();
                    if (range) {
                        ns.initParentPicker('custom');
                        ns.initChildPicker(range.startDateObj, range.endDateObj);
                    }
                } else {
                    initCustomMode();
                }
                break;
        }
    };

    const reinitializeDatePickers = () => {
        const parentDate = $('#parentDate').val();
        if (parentDate) {
            const range = getDateRangeFromInput();
            if (range) {
                ns.initChildPicker(range.startDateObj, range.endDateObj);
            }
        }
    };

    const getDateRangeFromInput = () => {
        const dateRangeStr = $('#parentDate').val();

        if (!dateRangeStr || dateRangeStr.trim() === '') {
            return null;
        }

        // Try different separators
        let parts = dateRangeStr.split(' إلى ');
        if (parts.length !== 2) {
            parts = dateRangeStr.split(' to ');
        }

        if (parts.length === 2) {
            const startStr = parts[0].trim();
            const endStr = parts[1].trim();

            // Check if we have data attributes (from month selection)
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
            const schoolId = $(this).data('id');
            const school = ns.allSchools.find(s => s.id === schoolId);
            if (school) {
                const visitDate = $(`.childDate[data-school-id="${schoolId}"]`).val();
                const visitTypeId = $(`.visitTypeSelect[data-school-id="${schoolId}"]`).val();

                ns.selectedSchools.push({
                    ...school,
                    visitDate: visitDate,
                    visitTypeId: visitTypeId
                });
            }
        });
    };

    const updateSchoolVisitDate = (schoolId, dateRange) => {
        const school = ns.selectedSchools.find(s => s.id === schoolId);
        if (school) {
            school.visitDate = dateRange;
        }
    };

    const updateSchoolVisitType = (schoolId, visitTypeId) => {
        const school = ns.selectedSchools.find(s => s.id === schoolId);
        if (school) {
            school.visitTypeId = visitTypeId;
        }
    };

    const updateFilterBadge = () => {
        const filterCount = Object.keys(ns.currentFilters).length;
        $('.filterbtn .badge').text(filterCount);
    };

    const performSearch = (term) => {
        if (!term) {
            ns.filteredSchools = ns.allSchools;
        } else {
            ns.filteredSchools = ns.allSchools.filter(s =>
                (s.name && s.name.includes(term)) ||
                (s.type && s.type.includes(term))
            );
        }

        ns.currentPage = 1;
        const tbody = ns.renderSchoolTable(ns.filteredSchools, ns.currentRenderType, ns.currentActionType);
        $('#planTable tbody').replaceWith(tbody);

        const pagination = ns.renderPagination(ns.filteredSchools.length);
        $('#dtPagination').html(pagination);

        reinitializeDatePickers();
        attachRowEventHandlers();
    };

    // ================== VALIDATION ==================

    const validatePlan = () => {
        let isValid = true;
        clearErrors();

        // Validate title
        const title = $('#planTitle').val().trim();
        if (!title) {
            showError('planTitle', 'يرجى إدخال عنوان الخطة');
            isValid = false;
        }

        // Validate plan type
        const planTypeId = $('#ddlPlanType').val();
        if (!planTypeId) {
            showError('ddlPlanType', 'يرجى اختيار نوع الخطة');
            isValid = false;
        }

        // Validate date range
        const dateRange = $('#parentDate').val();
        if (!dateRange || dateRange.trim() === '') {
            showError('parentDate', 'يرجى اختيار الفترة الزمنية');
            isValid = false;
        } else {
            // Validate date range format
            const range = getDateRangeFromInput();
            if (!range || !range.startDateObj || !range.endDateObj ||
                isNaN(range.startDateObj.getTime()) || isNaN(range.endDateObj.getTime())) {
                showError('parentDate', 'تاريخ غير صحيح');
                isValid = false;
            }
        }

        // Validate semester if visible
        if ($('#semesterContainer').is(':visible')) {
            const semesterId = $('#ddlSemester').val();
            if (!semesterId) {
                showError('ddlSemester', 'يرجى اختيار الفصل الدراسي');
                isValid = false;
            }
        }

        // Validate schools
        updateSelectedSchools();
        if (ns.selectedSchools.length === 0) {
            alert('يرجى تحديد مدرسة واحدة على الأقل');
            isValid = false;
        }

        // Validate each school has visit date and type
        for (const school of ns.selectedSchools) {
            if (!school.visitDate) {
                alert(`يرجى تحديد تاريخ الزيارة للمدرسة: ${school.name}`);
                isValid = false;
                break;
            }
            if (!school.visitTypeId) {
                alert(`يرجى تحديد نوع الزيارة للمدرسة: ${school.name}`);
                isValid = false;
                break;
            }
        }

        return isValid;
    };

    const showError = (fieldId, message) => {
        $(`#${fieldId}`).addClass('is-invalid');
        $(`#${fieldId}`).siblings('.invalid-feedback').text(message).show();
    };

    const clearErrors = () => {
        $('.is-invalid').removeClass('is-invalid');
        $('.invalid-feedback').hide();
    };

    // ================== DATA COLLECTION & SAVE ==================

    const collectPlanData = () => {
        const dateRange = getDateRangeFromInput();

        return {
            id: ns.currentPlanId,
            title: $('#planTitle').val().trim(),
            planTypeId: $('#ddlPlanType').val(),
            semesterId: $('#ddlSemester').val() || null,
            startDate: dateRange ? dateRange.startDate : null,
            endDate: dateRange ? dateRange.endDate : null,
            dateRange: $('#parentDate').val(), // Keep the display format
            schools: ns.selectedSchools.map(school => ({
                schoolId: school.id,
                visitDate: school.visitDate,
                visitTypeId: school.visitTypeId
            }))
        };
    };

    const savePlan = (planData) => {
        const endpoint = planData.id ? API_ENDPOINTS.UPDATE_PLAN : API_ENDPOINTS.APPROVE_PLAN;

        jqClient().Post(endpoint, planData)
            .done(result => {
                if (result.success) {
                    alert('تم حفظ الخطة بنجاح');
                    // Redirect or refresh
                    window.location.href = '/Plan/Index';
                } else {
                    alert('حدث خطأ أثناء حفظ الخطة');
                }
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Save plan failed', textStatus, err);
                alert('حدث خطأ أثناء حفظ الخطة');
            })
            .always(() => {
                $('#confirmation-modal').modal('hide');
            });
    };

    // ================== PUBLIC API ==================

    window.PlanHandler = {
        initialize: initializePage,
        loadSchools: loadSchoolsData,
        savePlan: handleSavePlan,
        getRenderType: () => ns.currentRenderType,
        getActionType: () => ns.currentActionType,
        getSelectedSchools: () => ns.selectedSchools
    };

})();

// ================== DOCUMENT READY ==================

$(document).ready(function () {
    // Initialize with default mode (CREATE)
    // For edit mode, pass planId
    // For comparison mode, pass oldPlanId and planId with renderType: RENDER_TYPE.COMPARISON

    const urlParams = new URLSearchParams(window.location.search);
    const planId = urlParams.get('planId');
    const oldPlanId = urlParams.get('oldPlanId');
    const mode = urlParams.get('mode'); // 'edit', 'view', 'approve', 'comparison'

    let renderType, actionType;

    if (mode === 'comparison' && oldPlanId && planId) {
        renderType = window.PlanConstants.RENDER_TYPE.COMPARISON;
        actionType = window.PlanConstants.ACTION_TYPE.APPROVE_WITH_CHANGES;
    } else if (mode === 'view' || mode === 'approve') {
        renderType = window.PlanConstants.RENDER_TYPE.PREVIEW;
        actionType = mode === 'approve'
            ? window.PlanConstants.ACTION_TYPE.APPROVE
            : window.PlanConstants.ACTION_TYPE.VIEW;
    } else if (planId) {
        renderType = window.PlanConstants.RENDER_TYPE.ACTION;
        actionType = window.PlanConstants.ACTION_TYPE.EDIT;
    } else {
        renderType = window.PlanConstants.RENDER_TYPE.ACTION;
        actionType = window.PlanConstants.ACTION_TYPE.CREATE;
    }

    window.PlanHandler.initialize({
        renderType: renderType,
        actionType: actionType,
        planId: planId,
        oldPlanId: oldPlanId
    });
});