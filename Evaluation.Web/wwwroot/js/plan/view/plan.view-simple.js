// ============= SIMPLIFIED VIEW-ONLY PLAN HANDLER =============
// This handler only needs the plan ID and fetches all data from the server

(function () {
    // Constants
    const API_ENDPOINTS = {
        GET_PLAN_DETAILS: '/Plan/GetPlanDetails',
        GET_PLAN_TYPES: '/PlanType/GetPlanTypes',
        GET_SEMESTERS: '/Plan/GetSemesters',
        GET_VISITS: '/School/GetVisits'
    };

    const RATING_CLASSES = {
        'Perfect': 'bg-success',
        'VeryGood': 'bg-info',
        'Good': 'bg-primary',
        'Acceptable': 'bg-secondary',
        'Week': 'bg-danger',
        'ممتاز': 'bg-success',
        'جيد جداً': 'bg-info',
        'جيد': 'bg-primary',
        'مقبول': 'bg-secondary',
        'ضعيف': 'bg-danger'
    };

    // State
    let planTypes = [];
    let semesters = [];
    let visitTypes = [];
    let currentPlan = null;

    // ================== INITIALIZATION ==================

    const initialize = (planId) => {
        // Validate plan ID (can be Guid string or empty)
        if (!planId || planId === '' || planId === '00000000-0000-0000-0000-000000000000') {
            showError('معرف الخطة مفقود');
            return;
        }

        console.log('Initializing view page with plan ID:', planId);

        // Load all required data
        Promise.all([
            loadPlanTypes(),
            loadSemesters(),
            loadVisitTypes(),
            loadPlanData(planId)
        ]).then(() => {
            console.log('All data loaded successfully');
            renderPlanView();
        }).catch(error => {
            console.error('Failed to load data:', error);
            showError('فشل تحميل البيانات');
        });
    };

    // ================== DATA LOADING ==================

    const loadPlanTypes = () => {
        return jqClient().Get(API_ENDPOINTS.GET_PLAN_TYPES)
            .done(response => {
                planTypes = response?.result || [];
                console.log('Plan types loaded:', planTypes.length);
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Failed to load plan types:', textStatus, err);
            });
    };

    const loadSemesters = () => {
        return jqClient().Get(API_ENDPOINTS.GET_SEMESTERS)
            .done(response => {
                semesters = response?.result || [];
                console.log('Semesters loaded:', semesters.length);
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Failed to load semesters:', textStatus, err);
            });
    };

    const loadVisitTypes = () => {
        return jqClient().Get(API_ENDPOINTS.GET_VISITS)
            .done(response => {
                visitTypes = response?.result || [];
                console.log('Visit types loaded:', visitTypes.length);
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Failed to load visit types:', textStatus, err);
            });
    };

    const loadPlanData = (planId) => {
        //showLoadingState();

        return jqClient().Get(`${API_ENDPOINTS.GET_PLAN_DETAILS}/${planId}`)
            .done(response => {
                currentPlan = response?.result;
                console.log('Plan data loaded:', currentPlan);

                if (!currentPlan) {
                    throw new Error('Plan data is null');
                }
            })
            .fail((jqXHR, textStatus, err) => {
                console.error('Failed to load plan data:', textStatus, err);
                throw err;
            });
    };

    // ================== RENDERING ==================

    const renderPlanView = () => {
        if (!currentPlan) {
            showError('لم يتم العثور على بيانات الخطة');
            return;
        }

        // Hide loading skeleton
        $('#loadingSkeleton').hide();

        // Render status box
        renderStatusBox();

        // Render plan details
        renderPlanDetails();

        // Render schools
        renderSchools();

        // Show/hide edit button based on status
        if (currentPlan.status !== 'مكتملة' && currentPlan.status !== 'Completed') {
            $('#btnEditPlan').show().attr('href', `/Plan/Edit?id=${currentPlan.id}`);
        }
    };

    const renderStatusBox = () => {
        const statusClass = getStatusBadgeClass(currentPlan.status);

        const html = `
            <div class="alert alert-info mb-4">
                <div class="row g-3">
                    <div class="col-md-3">
                        <strong>حالة الخطة:</strong> 
                        <span class="badge ${statusClass} status-badge">${currentPlan.status || '-'}</span>
                    </div>
                    <div class="col-md-3">
                        <strong>تاريخ الإنشاء:</strong> 
                        ${formatDate(currentPlan.createdDate)}
                    </div>
                    <div class="col-md-3">
                        <strong>المنشئ:</strong> 
                        ${currentPlan.createdBy || '-'}
                    </div>
                    <div class="col-md-3">
                        <strong>عدد المدارس:</strong> 
                        <span class="badge bg-primary status-badge">${currentPlan.schools?.length || 0}</span>
                    </div>
                </div>
            </div>
        `;

        $('#planStatusBox').html(html).show();
    };

    const renderPlanDetails = () => {
        const planType = planTypes.find(t => t.id === currentPlan.planTypeId);
        const semester = semesters.find(s => s.id === currentPlan.semesterId);

        const html = `
            <div class="card">
                <div class="card-body">
                    <h4 class="mb-4 text-primary">تفاصيل الخطة</h4>
                    
                    <div class="row g-3">
                        <!-- Plan Title -->
                        <div class="col-md-12">
                            <div class="info-card">
                                <div class="info-label">عنوان الخطة</div>
                                <div class="info-value">${escapeHtml(currentPlan.title) || '-'}</div>
                            </div>
                        </div>

                        <!-- Plan Type -->
                        <div class="col-md-${currentPlan.semesterId ? '4' : '6'}">
                            <div class="info-card">
                                <div class="info-label">نوع الخطة</div>
                                <div class="info-value">${planType ? escapeHtml(planType.name) : '-'}</div>
                            </div>
                        </div>

                        ${currentPlan.semesterId ? `
                        <!-- Semester -->
                        <div class="col-md-4">
                            <div class="info-card">
                                <div class="info-label">الفصل الدراسي</div>
                                <div class="info-value">${semester ? escapeHtml(semester.name) : '-'}</div>
                            </div>
                        </div>
                        ` : ''}

                        <!-- Date Range -->
                        <div class="col-md-${currentPlan.semesterId ? '4' : '6'}">
                            <div class="info-card">
                                <div class="info-label">الفترة الزمنية</div>
                                <div class="info-value">
                                    ${formatDate(currentPlan.startDate)} 
                                    <span class="text-muted">إلى</span> 
                                    ${formatDate(currentPlan.endDate)}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;

        $('#planDetailsContainer').html(html);
    };

    const renderSchools = () => {
        if (!currentPlan.schools || currentPlan.schools.length === 0) {
            const html = `
                <div class="card">
                    <div class="card-body text-center py-5">
                        <i class="la la-info-circle" style="font-size: 4rem; color: #6c757d;"></i>
                        <p class="mt-3 text-muted fs-5">لا توجد مدارس مدرجة في هذه الخطة</p>
                    </div>
                </div>
            `;
            $('#schoolsContainer').html(html);
            return;
        }

        let html = `
            <div class="card">
                <div class="card-body">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="mb-0 text-primary">المدارس المدرجة في الخطة</h4>
                        <span class="badge bg-primary status-badge">
                            ${currentPlan.schools.length} مدرسة
                        </span>
                    </div>
                    
                    <div class="row g-3">
        `;

        currentPlan.schools.forEach((school, index) => {
            html += renderSchoolCard(school, index + 1);
        });

        html += `
                    </div>
                </div>
            </div>
        `;

        $('#schoolsContainer').html(html);
    };

    const renderSchoolCard = (school, number) => {
        const ratingClass = RATING_CLASSES[school.rating] || 'bg-secondary';
        const visitType = visitTypes.find(v => v.id === school.visitTypeId);

        return `
            <div class="col-md-6">
                <div class="school-card">
                    <div class="d-flex gap-3">
                        <!-- School Number -->
                        <div class="school-number flex-shrink-0">
                            ${number}
                        </div>
                        
                        <!-- School Details -->
                        <div class="flex-grow-1">
                            <!-- School Name and Rating -->
                            <div class="d-flex justify-content-between align-items-start mb-3">
                                <div>
                                    <h5 class="school-name-header mb-1">${escapeHtml(school.name) || '-'}</h5>
                                    ${school.schoolLevel && school.schoolLevel.length > 0 ? `
                                    <div class="text-muted small">
                                        <i class="la la-graduation-cap"></i>
                                        ${school.schoolLevel.map(l => escapeHtml(l.name)).join(', ')}
                                    </div>
                                    ` : ''}
                                </div>
                                <span class="badge ${ratingClass}">
                                    ${escapeHtml(school.rating) || '-'}
                                </span>
                            </div>

                            <!-- Details Grid -->
                            <div>
                                <div class="detail-row">
                                    <span class="detail-label">
                                        <i class="la la-calendar"></i> تاريخ الزيارة
                                    </span>
                                    <span class="detail-value">
                                        ${formatVisitDate(school.visitDate)}
                                    </span>
                                </div>

                                <div class="detail-row">
                                    <span class="detail-label">
                                        <i class="la la-clipboard-list"></i> نوع الزيارة
                                    </span>
                                    <span class="detail-value">
                                        ${visitType ? escapeHtml(visitType.name) : '-'}
                                    </span>
                                </div>

                                <div class="detail-row">
                                    <span class="detail-label">
                                        <i class="la la-history"></i> آخر تقييم
                                    </span>
                                    <span class="detail-value">
                                        ${formatDate(school.lastEvalDate)}
                                    </span>
                                </div>

                                ${school.academicYear ? `
                                <div class="detail-row">
                                    <span class="detail-label">
                                        <i class="la la-book"></i> العام الأكاديمي
                                    </span>
                                    <span class="detail-value">
                                        ${escapeHtml(school.academicYear)}
                                    </span>
                                </div>
                                ` : ''}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    };

    // ================== HELPER FUNCTIONS ==================

    const formatDate = (dateString) => {
        if (!dateString) return '-';

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

    const formatVisitDate = (dateString) => {
        if (!dateString) return '-';

        // Handle date range format (e.g., "2024-01-01 to 2024-01-31")
        if (dateString.includes(' to ')) {
            const [start, end] = dateString.split(' to ').map(d => d.trim());
            return `${formatDate(start)} - ${formatDate(end)}`;
        }

        return formatDate(dateString);
    };

    const getStatusBadgeClass = (status) => {
        const statusLower = (status || '').toLowerCase();

        if (statusLower.includes('مكتمل') || statusLower.includes('completed')) {
            return 'bg-success';
        } else if (statusLower.includes('قيد') || statusLower.includes('pending')) {
            return 'bg-warning';
        } else if (statusLower.includes('مرفوض') || statusLower.includes('rejected')) {
            return 'bg-danger';
        } else if (statusLower.includes('مسودة') || statusLower.includes('draft')) {
            return 'bg-secondary';
        }

        return 'bg-primary';
    };

    const escapeHtml = (text) => {
        if (!text) return '';
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    };

    const showError = (message) => {
        $('#loadingSkeleton').hide();

        const errorHtml = `
            <div class="alert alert-danger text-center" role="alert">
                <i class="la la-exclamation-triangle" style="font-size: 3rem;"></i>
                <p class="mt-3 mb-0 fs-5">${escapeHtml(message)}</p>
            </div>
        `;

        $('#planDetailsContainer').html(errorHtml);
        $('#schoolsContainer').html('');
    };

    // ================== PUBLIC API ==================

    window.PlanViewSimple = {
        initialize: initialize
    };

})();

// ================== DOCUMENT READY ==================

$(document).ready(function () {
    console.log('Document ready, initializing view page...');

    // Get plan ID from config
    const config = window.PLAN_PAGE_CONFIG;

    if (!config || !config.planId) {
        console.error('Plan configuration is missing');
        alert('معرف الخطة مفقود');
        return;
    }

    console.log('Plan ID from config:', config.planId);

    // Initialize view page
    window.PlanViewSimple.initialize(config.planId);
});
