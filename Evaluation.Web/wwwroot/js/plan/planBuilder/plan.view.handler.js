// ============= VIEW-ONLY PLAN HANDLER =============
// This handler is specifically for displaying plans in read-only mode

(function () {
    const {
        RENDER_TYPE,
        ACTION_TYPE,
        API_ENDPOINTS,
        RATING_CLASSES
    } = window.PlanConstants || {};

    const ns = window.planUtility;

    // ================== INITIALIZATION ==================

    const initializeViewPage = (planId) => {
        if (!planId) {
            console.error('Plan ID is required for view mode');
            alert('معرف الخطة مفقود');
            return;
        }

        // Store current mode
        ns.currentRenderType = RENDER_TYPE.PREVIEW;
        ns.currentActionType = ACTION_TYPE.VIEW;
        ns.currentPlanId = planId;

        // Load initial data
        Promise.all([
            loadPlanTypes(),
            loadSemesters(),
            loadVisitTypes()
        ]).then(() => {
            // Load plan data
            loadPlanData(planId);
        }).catch(error => {
            console.error('Failed to load initial data:', error);
            showErrorState('فشل تحميل البيانات الأولية');
        });
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

    const loadPlanData = (planId) => {
        showLoadingState();

        return jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}?planId=${planId}`)
            .done(result => {
                const planData = result?.result;
                if (planData) {
                    renderPlanView(planData);
                } else {
                    showErrorState('لم يتم العثور على بيانات الخطة');
                }
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Load plan data failed', textStatus, err);
                showErrorState('فشل تحميل بيانات الخطة');
            });
    };

    // ================== RENDERING ==================

    const renderPlanView = (planData) => {
        // Render plan details as read-only cards
        const planDetailsHtml = generatePlanDetailsCards(planData);
        $('#planFormContainer .form-container').html(planDetailsHtml);

        // Render schools table
        if (planData.schools && planData.schools.length > 0) {
            ns.selectedSchools = planData.schools;
            const tbody = generateViewOnlySchoolsTable(planData.schools);
            $('#planTable tbody').replaceWith(tbody);

            // Update schools count
            $('#schoolsCount').text(`${planData.schools.length} مدرسة`);
        } else {
            showNoSchoolsState();
        }
    };

    const generatePlanDetailsCards = (planData) => {
        const planType = ns.planTypes.find(t => t.id === planData.planTypeId);
        const semester = ns.semesters.find(s => s.id === planData.semesterId);

        return `
            <div class="row g-3">
                <!-- Plan Title -->
                <div class="col-md-12">
                    <div class="info-card">
                        <div class="info-label">عنوان الخطة</div>
                        <div class="info-value">${planData.title || '-'}</div>
                    </div>
                </div>

                <!-- Plan Type -->
                <div class="col-md-6">
                    <div class="info-card">
                        <div class="info-label">نوع الخطة</div>
                        <div class="info-value">${planType ? planType.name : '-'}</div>
                    </div>
                </div>

                <!-- Date Range -->
                <div class="col-md-6">
                    <div class="info-card">
                        <div class="info-label">الفترة الزمنية</div>
                        <div class="info-value">
                            ${planData.startDate ? formatDisplayDate(planData.startDate) : '-'} 
                            إلى 
                            ${planData.endDate ? formatDisplayDate(planData.endDate) : '-'}
                        </div>
                    </div>
                </div>

                ${planData.semesterId ? `
                <!-- Semester -->
                <div class="col-md-6">
                    <div class="info-card">
                        <div class="info-label">الفصل الدراسي</div>
                        <div class="info-value">${semester ? semester.name : '-'}</div>
                    </div>
                </div>
                ` : ''}

                <!-- Total Schools -->
                <div class="col-md-6">
                    <div class="info-card">
                        <div class="info-label">عدد المدارس</div>
                        <div class="info-value">
                            <span class="badge bg-primary fs-6">${planData.schools ? planData.schools.length : 0}</span>
                        </div>
                    </div>
                </div>
            </div>
        `;
    };

    const generateViewOnlySchoolsTable = (schools) => {
        const tbody = $('<tbody>');

        schools.forEach((school, index) => {
            const row = generateViewOnlySchoolRow(school, index + 1);
            tbody.append(row);
        });

        return tbody;
    };

    const generateViewOnlySchoolRow = (school, rowNumber) => {
        const tr = $('<tr>');

        // Row Number
        tr.append($('<td>').addClass('text-center').text(rowNumber));

        // School Name with Rating
        const schoolNameCell = $('<td>');
        const ratingClass = RATING_CLASSES[school.rating] || 'bg-light';

        const nameContainer = $('<div>').addClass('d-flex align-items-center justify-content-between');

        const infoDiv = $('<div>');
        infoDiv.append($('<h6>').addClass('mb-1').text(school.name || '-'));

        if (school.schoolLevel && school.schoolLevel.length > 0) {
            const levelBadge = $('<div>').addClass('square-bullet');
            const levelText = school.schoolLevel.map(l => l.name).join(', ');
            levelBadge.append($('<div>').text(levelText));
            infoDiv.append(levelBadge);
        }

        const ratingBadge = $('<span>')
            .addClass(`badge ${ratingClass}`)
            .text(school.rating || '-');

        nameContainer.append(infoDiv, ratingBadge);
        schoolNameCell.append(nameContainer);
        tr.append(schoolNameCell);

        // Visit Date
        const visitDateCell = $('<td>');
        if (school.visitDate) {
            const dateText = formatDisplayDate(school.visitDate);
            visitDateCell.text(dateText);
        } else {
            visitDateCell.text('-');
        }
        tr.append(visitDateCell);

        // Last Evaluation Date
        const lastEvalCell = $('<td>').addClass('text-center');
        if (school.lastEvalDate) {
            lastEvalCell.text(formatDisplayDate(school.lastEvalDate));
        } else {
            lastEvalCell.text('-');
        }
        tr.append(lastEvalCell);

        // Visit Type
        const visitTypeCell = $('<td>');
        const visitType = ns.visitTypes.find(v => v.id === school.visitTypeId);
        if (visitType) {
            const badge = $('<span>')
                .addClass('badge bg-info')
                .text(visitType.name);
            visitTypeCell.append(badge);
        } else {
            visitTypeCell.text('-');
        }
        tr.append(visitTypeCell);

        // Academic Year
        const academicYearCell = $('<td>').addClass('text-center');
        academicYearCell.text(school.academicYear || '-');
        tr.append(academicYearCell);

        return tr;
    };

    // ================== HELPER FUNCTIONS ==================

    const formatDisplayDate = (dateString) => {
        if (!dateString) return '-';

        // Handle date range format (e.g., "2024-01-01 to 2024-01-31")
        if (dateString.includes(' to ')) {
            const [start, end] = dateString.split(' to ').map(d => d.trim());
            return `${formatSingleDate(start)} إلى ${formatSingleDate(end)}`;
        }

        return formatSingleDate(dateString);
    };

    const formatSingleDate = (dateString) => {
        try {
            const date = new Date(dateString);
            if (isNaN(date.getTime())) return dateString;

            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        } catch (e) {
            return dateString;
        }
    };

    // ================== STATE MANAGEMENT ==================

    const showLoadingState = () => {
        $('#planFormContainer .form-container').html(`
            <div class="text-center py-5">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">جاري التحميل...</span>
                </div>
                <p class="mt-2 text-muted">جاري تحميل بيانات الخطة...</p>
            </div>
        `);
    };

    const showErrorState = (message) => {
        $('#planFormContainer .form-container').html(`
            <div class="alert alert-danger text-center" role="alert">
                <i class="la la-exclamation-triangle fs-1 mb-2"></i>
                <p class="mb-0">${message}</p>
            </div>
        `);

        $('#planTable tbody').html(`
            <tr>
                <td colspan="6" class="text-center py-4">
                    <div class="alert alert-danger mb-0">
                        <i class="la la-exclamation-triangle"></i> ${message}
                    </div>
                </td>
            </tr>
        `);
    };

    const showNoSchoolsState = () => {
        $('#planTable tbody').html(`
            <tr>
                <td colspan="6" class="text-center py-5">
                    <i class="la la-info-circle fs-1 text-muted"></i>
                    <p class="mt-2 text-muted">لا توجد مدارس مدرجة في هذه الخطة</p>
                </td>
            </tr>
        `);
        $('#schoolsCount').text('0 مدرسة');
    };

    // ================== PUBLIC API ==================

    window.PlanViewHandler = {
        initialize: initializeViewPage
    };

})();

// ================== DOCUMENT READY ==================

$(document).ready(function () {
    // Get plan ID from config
    const config = window.PLAN_PAGE_CONFIG;

    if (!config || !config.planId) {
        console.error('Plan configuration is missing');
        alert('معرف الخطة مفقود');
        return;
    }

    // Initialize view-only mode
    window.PlanViewHandler.initialize(config.planId);
});
