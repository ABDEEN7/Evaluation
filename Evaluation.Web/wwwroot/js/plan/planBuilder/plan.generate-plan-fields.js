window.planUtility = window.planUtility || {};
const planUtility = window.planUtility;

(function (ns) {
    'use strict';

    // ================== CONSTANTS ==================
    const {
        PLAN_FIELD_TYPE,
        SCHOOL_FIELD_TYPE,
        VALIDATION_RULES,
        TABLE_CONFIG,
        FILTER_FIELDS,
        RATING_CLASSES,
        PLAN_TYPE_BACKEND,
        API_ENDPOINTS,
    } = window.PlanConstants || {};

    // ================== ID HELPER ==================
    function pid(fieldId, name) {
        return `${fieldId}_${name}`;
    }

    // ================== PUBLIC ==================
    ns.generatePlanFieldsHTML = function (fieldId) {
        let html = '';
        html += generatePlanFormContainer(fieldId);
        html += generateSchoolsTableSection(fieldId);
        html += generateFilterOffcanvas(fieldId);
        html += generateConfirmationModal(fieldId);
        return html;
    };

    ns.generatePlanFields = function (fieldId) {
        const html = ns.generatePlanFieldsHTML(fieldId);

        const mainContent =
            document.querySelector('.main-content') ||
            document.querySelector('main') ||
            document.body;

        const wrapper = document.createElement('div');
        wrapper.id = pid(fieldId, 'wrapper');
        wrapper.innerHTML = html;

        mainContent.appendChild(wrapper);
    };

    // ================== FORM ==================
    function generatePlanFormContainer(fieldId) {
        return `
            <div id="${pid(fieldId, 'planFormContainer')}" class="card-table mb-4">
                <div class="card-body p-0">
                    <div class="form-container pb-0">
                        ${generateFormFields(fieldId)}
                    </div>
                </div>
            </div>
        `;
    }

    function generateFormFields(fieldId) {
        return `
            <form id="${pid(fieldId, 'planForm')}">
                <div class="row">

                    <!-- Title -->
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'planTitle')}" class="form-label">
                                عنوان الخطة <span class="text-danger">*</span>
                            </label>
                            <input type="text"
                                   id="${pid(fieldId, 'planTitle')}"
                                   name="PlanTitle"
                                   class="form-control"
                                   placeholder="أدخل عنوان الخطة"
                                   ${VALIDATION_RULES?.TITLE?.required ? 'required' : ''}>
                        </div>

                    <!-- Plan Type -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'ddlPlanType')}" class="form-label">
                                نوع الخطة <span class="text-danger">*</span>
                            </label>
                            <select id="${pid(fieldId, 'ddlPlanType')}"
                                    name="PlanTypeId"
                                    class="form-control"
                                    ${VALIDATION_RULES?.PLAN_TYPE?.required ? 'required' : ''}>
                                <option value="">اختر نوع الخطة</option>
                            </select>
                        </div>
                    </div>

                    <!-- Semester -->
                    <div id="${pid(fieldId, 'semesterContainer')}" class="col-md-4" style="display:none;">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'ddlSemester')}" class="form-label">
                                الفصل الدراسي
                            </label>
                            <select id="${pid(fieldId, 'ddlSemester')}"
                                    name="SemesterId"
                                    class="form-control">
                                <option value="">اختر الفصل الدراسي</option>
                            </select>
                        </div>
                    </div>

                    <!-- Date -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'parentDate')}" class="form-label">
                                الفترة الزمنية <span class="text-danger">*</span>
                            </label>
                            <div class="input-group datetime">
                                <input type="text"
                                       id="${pid(fieldId, 'parentDate')}"
                                       name="dateRange"
                                       class="form-control"
                                       placeholder="اختر تاريخ البداية والنهاية">
                                <span class="input-group-text">
                                    <i class="la la-calendar"></i>
                                </span>
                            </div>
                        </div>
                    </div>

                </div>
            </form>
        `;
    }

    // ================== TABLE ==================
    function generateSchoolsTableSection(fieldId) {
        return `
            <div class="card card-table">
                <div class="card-body">

                    <div class="row align-items-center mb-3">
                        <div class="col-xl-7">
                            <h4>تحديد المدارس</h4>
                        </div>

                        <div class="col-xl-5">
                            <div class="row">
                                <div class="col-md-8 mb-3">
                                    <input type="text"
                                           id="${pid(fieldId, 'customSearch')}"
                                           class="form-control"
                                           placeholder="ابحث هنا...">
                                </div>
                                <div class="col-md-4 mb-3">
                                    <button class="btn filterbtn"
                                            data-bs-toggle="offcanvas"
                                            data-bs-target="#${pid(fieldId, 'filterOffcanvas')}">
                                        <i class="la la-filter"></i> تصفية
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    ${generateSchoolsTable(fieldId)}

                    <div class="row mt-3">
                        <div class="col-md-6">
                            <button class="btn btn-primary"
                                    id="${pid(fieldId, 'btnSavePlan')}">
                                <i class="la la-save"></i> حفظ الخطة
                            </button>
                        </div>
                        <div class="col-md-6 text-end">
                            <div id="${pid(fieldId, 'dtPagination')}"></div>
                        </div>
                    </div>

                </div>
            </div>
        `;
    }

    function generateSchoolsTable(fieldId) {
        return `
            <div class="table-responsive">
                <table id="${pid(fieldId, 'planTable')}"
                       class="table table-bordered table-hover w-100">
                    <thead class="table-light">
                        <tr>
                            <th style="width:50px;">
                                <input type="checkbox" id="${pid(fieldId, 'selectAll')}">
                            </th>
                            <th>اسم المدرسة</th>
                            <th>تاريخ الزيارة</th>
                            <th>آخر تقييم</th>
                            <th>نوع الزيارة</th>
                            <th>العام الأكاديمي</th>
                            <th>الإجراءات</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="7" class="text-center py-5">
                                جاري التحميل...
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        `;
    }

    // ================== FILTER ==================
    function generateFilterOffcanvas(fieldId) {
        return `
            <div class="offcanvas offcanvas-end"
                 tabindex="-1"
                 id="${pid(fieldId, 'filterOffcanvas')}">

                <div class="offcanvas-header">
                    <h5>تصفية النتائج</h5>
                    <button type="button" class="btn-close"
                            data-bs-dismiss="offcanvas"></button>
                </div>

                <div class="offcanvas-body">
                    <form id="${pid(fieldId, 'filterForm')}">

                        <div class="mb-3">
                            <label>اسم المدرسة</label>
                            <input type="text"
                                   id="${pid(fieldId, 'filterSchoolName')}"
                                   class="form-control">
                        </div>

                        <div class="mb-3">
                            <label>آخر تقييم</label>
                            <input type="text"
                                   id="${pid(fieldId, 'filterLastEvalDate')}"
                                   class="form-control">
                        </div>

                        <div class="d-flex gap-2">
                            <button class="btn btn-primary w-100">
                                تطبيق
                            </button>
                            <button type="button"
                                    class="btn btn-outline-primary w-100"
                                    id="${pid(fieldId, 'clearFiltersBtn')}">
                                مسح
                            </button>
                        </div>

                    </form>
                </div>
            </div>
        `;
    }

    // ================== MODAL ==================
    function generateConfirmationModal(fieldId) {
        return `
            <div class="modal fade"
                 id="${pid(fieldId, 'confirmationModal')}"
                 tabindex="-1">

                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">

                        <div class="modal-body text-center">
                            <h5 id="${pid(fieldId, 'confirmationMessage')}">
                                هل ترغب في حفظ الخطة؟
                            </h5>

                            <div class="d-flex gap-2 justify-content-center mt-3">
                                <button class="btn btn-primary"
                                        id="${pid(fieldId, 'btnSubmit')}">
                                    تأكيد
                                </button>
                                <button class="btn btn-outline-primary"
                                        data-bs-dismiss="modal">
                                    إلغاء
                                </button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        `;
    }

})(planUtility);
