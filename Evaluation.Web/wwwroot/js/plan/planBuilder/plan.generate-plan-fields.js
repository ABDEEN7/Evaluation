window.planUtility = window.planUtility || {};
const planUtility = window.planUtility;

(function (ns) {
    'use strict';

    // ================== CONSTANTS ==================
    const {
        ACTION_TYPE,
        ReadOnly_ACTION_TYPES,
        PLAN_FIELD_TYPE,
        SCHOOL_FIELD_TYPE,
        VALIDATION_RULES,
        TABLE_CONFIG,
        FILTER_FIELDS,
        RATING_CLASSES,
        PLAN_TYPE_BACKEND,
        API_ENDPOINTS,
        
    } = window.PlanConstants || {};

    ns.generatePlanFieldsHTML = function () {
        let html = '';
        html += generateBreadcrumbs();
        html += generatePlanFormContainer();
        html += generateSchoolsTableSection();
        html += generateFilterOffcanvas();
        html += generateConfirmationModal();
        return html;
    };

    function generateBreadcrumbs() {
        return `
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item"><a href="/Home/Index">الرئيسية</a></li>
                    <li class="breadcrumb-item"><a href="/Plan/Index">الخطط</a></li>
                    <li class="breadcrumb-item active" aria-current="page">إنشاء خطة</li>
                </ol>
            </nav>
        `;
    }

    function generatePlanFormContainer() {
        let html = `
            <div id="planFormContainer" class="card-table mb-4">
                <div class="card-body p-0">
                    <div class="form-container pb-0">
                        <h4 class="mb-4 text-primary">إنشاء الخطة</h4>
        `;
        html += generateFormFields();
        html += `
                    </div>
                </div>
            </div>
        `;
        return html;
    }

    function generateFormFields() {
        return `
            <form id="planForm">
                <div class="row">
                    <!-- Title Field -->
                    <div class="col-md-12">
                        <div class="mb-4">
                            <label for="planTitle" class="form-label">
                                عنوان الخطة <span class="text-danger">*</span>
                            </label>
                            <input type="text" 
                                   id="planTitle" 
                                   name="PlanTitle" 
                                   class="form-control" 
                                   placeholder="أدخل عنوان الخطة"
                                   value=""
                                   ${VALIDATION_RULES.TITLE.required ? 'required' : ''}>
                            <div class="invalid-feedback">يرجى إدخال عنوان الخطة</div>
                        </div>
                    </div>

                    <!-- Plan Type Field -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="ddlPlanType" class="form-label">
                                نوع الخطة <span class="text-danger">*</span>
                            </label>
                            <select id="ddlPlanType" 
                                    name="PlanTypeId" 
                                    class="form-control"
                                    ${VALIDATION_RULES.PLAN_TYPE.required ? 'required' : ''}>
                                <option value="">اختر نوع الخطة</option>
                            </select>
                            <div class="invalid-feedback">يرجى اختيار نوع الخطة</div>
                        </div>
                    </div>

                    <!-- Semester Field -->
                    <div id="semesterContainer" class="col-md-4" style="display: none;">
                        <div class="mb-4">
                            <label for="ddlSemester" class="form-label">
                                الفصل الدراسي <span class="text-danger">*</span>
                            </label>
                            <select id="ddlSemester" 
                                    name="SemesterId" 
                                    class="form-control">
                                <option value="">اختر الفصل الدراسي</option>
                            </select>
                            <div class="invalid-feedback">يرجى اختيار الفصل الدراسي</div>
                        </div>
                    </div>

                    <!-- Date Range Field -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="parentDate" class="form-label">
                                الفترة الزمنية <span class="text-danger">*</span>
                            </label>
                            <div class="input-group datetime">
                                <input type="text" 
                                       id="parentDate" 
                                       name="dateRange" 
                                       class="form-control" 
                                       placeholder="اختر تاريخ بداية ونهاية الخطة"
                                       value="">
                                <span class="input-group-text">
                                    <i class="la la-calendar"></i>
                                </span>
                            </div>
                            <div class="invalid-feedback">يرجى اختيار الفترة الزمنية</div>
                        </div>
                    </div>
                </div>
            </form>
        `;
    }

    function generateSchoolsTableSection() {
        return `
            <div class="card card-table">
                <div class="card-body">
                    <!-- Search and Filter Row -->
                    <div class="row align-items-center mb-3">
                        <div class="col-xl-7 col-lg-5">
                            <h4 class="mb-3 mb-lg-0">تحديد المدارس</h4>
                        </div>
                        <div class="col-xl-5 col-lg-7">
                            <div class="row">
                                <div class="col-xl-8 col-lg-8 col-md-8 mb-3">
                                    <div class="d-flex justify-content-end custom-search">
                                        <div class="input-group">
                                            <span class="input-group-text border-end-0 bg-transparent">
                                                <i class="las la-search"></i>
                                            </span>
                                            <input type="text"
                                                   id="customSearch"
                                                   class="form-control form-control-lg border-start-0 ps-0"
                                                   placeholder="ابحث هنا...">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xl-4 col-lg-4 col-md-4 mb-3">
                                    <button class="btn filterbtn"
                                            data-bs-toggle="offcanvas"
                                            data-bs-target="#filterOffcanvas">
                                        <i class="la la-filter"></i> تصفية
                                        <span class="badge bg-primary rounded-pill mx-2">0</span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Schools Table -->
                    ${generateSchoolsTable()}

                    <!-- Action Buttons and Pagination -->
                    <div class="row mt-3">
                        <div class="col-md-6 mt-3">
                            <div class="d-flex gap-2 align-items-center flex-wrap">
                                <a href="#" class="btn btn-primary" id="btn-save-plan">
                                    <i class="la la-save"></i> حفظ الخطة
                                </a>
                                <a href="/Plan/Index" class="btn btn-outline-primary">
                                    عرض الخطط
                                </a>
                            </div>
                        </div>
                        <div class="col-md-6 text-end">
                            <div class="d-flex justify-content-end align-items-center">
                                <div id="dtPagination"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    function generateSchoolsTable() {
        return `
            <div class="table-card rounded overflow-hidden">
                <div class="table-responsive">
                    <table id="planTable" class="table table-bordered table-hover align-middle w-100">
                        <thead class="table-light">
                            <tr>
                                <th style="width: 50px;">
                                    <label class="custom-checkbox">
                                        <input type="checkbox" id="selectAll">
                                        <span class="checkmark"></span>
                                    </label>
                                </th>
                                <th>اسم المدرسة</th>
                                <th style="width: 250px;">تاريخ الزيارة</th>
                                <th style="width: 120px;">تاريخ آخر تقييم</th>
                                <th style="width: 150px;">نوع الزيارة</th>
                                <th style="width: 120px;">العام الأكاديمي</th>
                                <th style="width: 80px;">الإجراءات</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="7" class="text-center py-5">
                                    <div class="spinner-border text-primary" role="status">
                                        <span class="visually-hidden">جاري التحميل...</span>
                                    </div>
                                    <p class="mt-2 text-muted">جاري تحميل البيانات...</p>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        `;
    }

    function generateFilterOffcanvas() {
        return `
            <div class="offcanvas offcanvas-end" tabindex="-1" id="filterOffcanvas">
                <div class="offcanvas-header justify-content-between p-4">
                    <h5 class="offcanvas-title border d-flex align-items-center w-75 justify-content-between px-3 py-2 rounded">
                        <span>تصفية النتائج</span>
                        <i class="la la-filter"></i>
                    </h5>
                    <button type="button"
                            class="btn btn-lg border d-flex align-items-center h-100"
                            data-bs-dismiss="offcanvas">
                        <i class="la la-angle-right"></i>
                    </button>
                </div>
                <div class="offcanvas-body p-4">
                    <form id="filterForm">
                        <div class="mb-3">
                            <label class="form-label">اسم المدرسة</label>
                            <input type="text"
                                   class="form-control"
                                   id="filterSchoolName"
                                   name="SchoolName"
                                   placeholder="أكتب هنا...">
                        </div>
                        <hr>
                        <div class="mb-3">
                            <label class="form-label">تاريخ آخر تقييم</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       class="form-control datepicker-single"
                                       id="filterLastEvalDate"
                                       name="LastEvalDate"
                                       placeholder="اختر التاريخ">
                            </div>
                        </div>
                        <hr>
                        <div class="mb-3">
                            <label class="form-label">تاريخ الإنشاء</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       class="form-control datepicker-any"
                                       id="filterCreatedDate"
                                       name="CreatedDate"
                                       placeholder="اختر التاريخ">
                            </div>
                        </div>
                        <hr>
                        <div class="mb-3">
                            <label class="form-label">تاريخ التقييم القادم</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       class="form-control datepicker-single"
                                       id="filterNextEvalDate"
                                       name="NextEvalDate"
                                       placeholder="اختر التاريخ">
                            </div>
                        </div>
                        <hr>
                        <div class="d-flex gap-2 mt-4">
                            <button type="submit" class="btn btn-primary w-100">
                                <i class="la la-check"></i> تطبيق
                            </button>
                            <button type="button" class="btn btn-outline-primary w-100" id="clearFiltersBtn">
                                <i class="la la-trash"></i> مسح
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        `;
    }

    function generateConfirmationModal() {
        return `
            <div class="modal fade" id="confirmation-modal" tabindex="-1" aria-labelledby="confirmation-modalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-body py-0">
                            <div class="row">
                                <div class="col-md-12 py-4 text-center">
                                    <div class="social-icon bg-primary text-white fw-normal m-auto mb-3">
                                        <i class="la la-city"></i>
                                    </div>
                                    <h5 class="fw-bold text-primary mb-3" id="confirmationMessage">
                                        تم تحديد (00) مدرسة للإضافة للخطة
                                    </h5>
                                    <p>هل ترغب في حفظ الخطة؟</p>
                                    <div class="d-flex gap-2 justify-content-center">
                                        <button type="button"
                                                class="btn btn-primary mw-200"
                                                id="btn-submit">
                                            <i class="la la-bookmark"></i> تأكيد الحفظ
                                        </button>
                                        <button type="button"
                                                class="btn btn-outline-primary mw-200"
                                                data-bs-dismiss="modal">
                                            <i class="la la-window-close"></i> إلغاء
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    /**
     * Initialize plugins after rendering HTML
     */
  
    /**
     * Render the complete plan page
     */
    ns.generatePlanFields = function () {
        // Generate HTML
        const html = ns.generatePlanFieldsHTML();

        // Find main content container
        const mainContent = document.querySelector('.main-content') ||
            document.querySelector('main') ||
            document.querySelector('.card-table') ||
            document.body;

        // Clear and inject
        mainContent.innerHTML = html;

        // Initialize plugins

        console.log('[PlanFields] Page rendered successfully');
    };

    // Export for direct use

})(planUtility);