window.planUtility = window.planUtility || {};
const planUtility = window.planUtility;

(function (ns) {
    'use strict';

    // ================== CONSTANTS ==================
    const {
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
                                    <button type="button" class="btn filterbtn"
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

                <div class="offcanvas-header justify-content-between p-4">
                    <h5 class="offcanvas-title border d-flex align-items-center w-75 justify-content-between px-3 py-2 rounded">
                        <span>تصفية النتائج</span>
                        <i class="la la-filter"></i>
                    </h5>
                    <button type="button" class="btn btn-lg border d-flex align-items-center h-100"
                            data-bs-dismiss="offcanvas">
                        <i class="la la-angle-right"></i>
                    </button>
                </div>

                <div class="offcanvas-body p-4">
                    <form id="${pid(fieldId, 'filterForm')}">

                        <div class="mb-3">
                            <label class="form-label">اسم المدرسة</label>
                            <input type="text"
                                   id="${pid(fieldId, 'filterSchoolName')}"
                                   name="schoolName"
                                   class="form-control"
                                   placeholder="أكتب هنا...">
                        </div>
                        <hr>

                        <!-- تاريخ آخر تقييم -->
                        <div class="mb-3">
                            <label class="form-label">تاريخ آخر تقييم</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       id="${pid(fieldId, 'filterLastEvalDate')}"
                                       name="lastEvalDate"
                                       class="form-control filter-date-picker"
                                       placeholder="اختر التاريخ">
                            </div>
                        </div>
                        <hr>

                        <!-- تاريخ الإنشاء -->
                        <div class="mb-3">
                            <label class="form-label">تاريخ الإنشاء</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       id="${pid(fieldId, 'filterCreatedDate')}"
                                       name="createdDate"
                                       class="form-control filter-date-picker"
                                       placeholder="اختر التاريخ">
                            </div>
                        </div>
                        <hr>

                        <!-- تاريخ التقييم القادم -->
                        <div class="mb-3">
                            <label class="form-label">تاريخ التقييم القادم</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       id="${pid(fieldId, 'filterNextEvalDate')}"
                                       name="nextEvalDate"
                                       class="form-control filter-date-picker"
                                       placeholder="اختر التاريخ">
                            </div>
                        </div>
                        <hr>

                        <!-- النتيجة السابقة -->
                        <div class="mb-3">
                            <label class="form-label">النتيجة السابقة</label>
                            <select id="${pid(fieldId, 'filterPreviousResult')}"
                                    name="previousResult"
                                    class="form-control">
                                <option value="">الكل</option>
                                <option value="Perfect">ممتاز</option>
                                <option value="VeryGood">جيد جداً</option>
                                <option value="Good">جيد</option>
                                <option value="Acceptable">مقبول</option>
                                <option value="Week">ضعيف</option>
                            </select>
                        </div>
                        <hr>

                        <!-- نوع الزيارة -->
                        <div class="mb-3">
                            <label class="form-label">نوع الزيارة</label>
                            <select id="${pid(fieldId, 'filterVisitType')}"
                                    name="visitType"
                                    class="form-control">
                                <option value="">الكل</option>
                                <!-- Will be populated dynamically -->
                            </select>
                        </div>

                        <div class="d-flex gap-2 mt-4">
                            <button type="submit" class="btn btn-primary w-100">
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



})(planUtility);