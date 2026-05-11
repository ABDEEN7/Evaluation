window.planUtility = window.planUtility || {};
const planUtility = window.planUtility;

(function (ns) {
    'use strict';

    // ================== LOCALIZATION Helper ==================
    function t(key, fallback = '') {
        const text = uiControlsSetup()?.GetUiControlText(key);
        return text || key;
    }

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
                                ${t('lblPlanTitle')} <span class="text-danger">*</span>
                            </label>
                            <input type="text"
                                   id="${pid(fieldId, 'planTitle')}"
                                   name="PlanTitle"
                                   class="form-control"
                                   placeholder="${t('plhEnterPlanTitle')}"
                                   ${VALIDATION_RULES?.TITLE?.required ? 'required' : ''}>
                        </div>
                            <div id="${pid(fieldId, 'planTitle_error')}" class="error-message text-danger"></div>

                    <!-- Plan Type -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'ddlPlanType')}" class="form-label">
                                ${t('lblPlanType')} <span class="text-danger">*</span>
                            </label>
                            <select id="${pid(fieldId, 'ddlPlanType')}"
                                    name="PlanTypeId"
                                    class="form-control"
                                    ${VALIDATION_RULES?.PLAN_TYPE?.required ? 'required' : ''}>
                                <option value="">${t('lblChoosePlanType')}</option>
                            </select>
                        </div>
                            <div id="${pid(fieldId, 'ddlPlanType_error')}" class="error-message text-danger"></div>
                    </div>

                    <!-- Semester -->
                    <div id="${pid(fieldId, 'semesterContainer')}" class="col-md-4" style="display:none;">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'ddlSemester')}" class="form-label">
                                ${t('lblSemester')}
                            </label>
                            <select id="${pid(fieldId, 'ddlSemester')}"
                                    name="SemesterId"
                                    class="form-control">
                                <option value="">${t('lblChooseSemester')}</option>
                            </select>
                            <div id="${pid(fieldId, 'ddlSemester_error')}" class="error-message text-danger"></div>
                        </div>
                    </div>

                    <!-- Date -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'parentDate')}" class="form-label">
                                ${t('lblTimePeriod')} <span class="text-danger">*</span>
                            </label>
                            <div class="input-group datetime">
                                <input type="text"
                                       id="${pid(fieldId, 'parentDate')}"
                                       name="dateRange"
                                       class="form-control"
                                       placeholder="${t('plhChooseStartEndDate')}">
                                <span class="input-group-text">
                                    <i class="la la-calendar"></i>
                                </span>
                            </div>
                            <div id="${pid(fieldId, 'parentDate_error')}" class="error-message text-danger"></div>
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
                            <h4>${t('lblSelectSchools')}</h4>
                        </div>

                        <div class="col-xl-5">
                            <div class="row">
                                <div class="col-md-8 mb-3">
                                    <input type="text"
                                           id="${pid(fieldId, 'customSearch')}"
                                           class="form-control"
                                           placeholder="${t('plhSearchHere')}">
                                </div>
                                <div class="col-md-4 mb-3">
                                    <button type="button" class="btn filterbtn"
                                            data-bs-toggle="offcanvas"
                                            data-bs-target="#${pid(fieldId, 'filterOffcanvas')}">
                                        <i class="la la-filter"></i> ${t('btnFilter')}
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
                            <th>${t('lblSchoolName')}</th>
                            <th>${t('lblVisitDate')}</th>
                            <th>${t('lblLastEvaluation')}</th>
                            <th>${t('lblVisitType')}</th>
                            <th>${t('lblAcademicYear')}</th>
                            <th>${t('lblActions')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="7" class="text-center py-5">
                                ${t('msgLoading')}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
                <div id="${pid(fieldId, 'selectAll_error')}" class="error-message text-danger"></div>

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
                        <span>${t('lblFilterResults')}</span>
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
                            <label class="form-label">${t('lblSchoolName')}</label>
                            <input type="text"
                                   id="${pid(fieldId, 'filterSchoolName')}"
                                   name="schoolName"
                                   class="form-control"
                                   placeholder="${t('plhWriteHere')}">
                        </div>
                        <hr>

                        <!-- تاريخ آخر تقييم -->
                        <div class="mb-3">
                            <label class="form-label">${t('lblLastEvalDate')}</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       id="${pid(fieldId, 'filterLastEvalDate')}"
                                       name="lastEvalDate"
                                       class="form-control filter-date-picker"
                                       placeholder="${t('plhChooseDate')}">
                            </div>
                        </div>
                        <hr>

                        <!-- تاريخ الإنشاء -->
                        <div class="mb-3">
                            <label class="form-label">${t('lblCreatedDate')}</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       id="${pid(fieldId, 'filterCreatedDate')}"
                                       name="createdDate"
                                       class="form-control filter-date-picker"
                                       placeholder="${t('plhChooseDate')}">
                            </div>
                        </div>
                        <hr>

                        <!-- تاريخ التقييم القادم -->
                        <div class="mb-3">
                            <label class="form-label">${t('lblNextEvalDate')}</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="la la-calendar"></i></span>
                                <input type="text"
                                       id="${pid(fieldId, 'filterNextEvalDate')}"
                                       name="nextEvalDate"
                                       class="form-control filter-date-picker"
                                       placeholder="${t('plhChooseDate')}">
                            </div>
                        </div>
                        <hr>

                        <!-- النتيجة السابقة -->
                        <div class="mb-3">
                            <label class="form-label">${t('lblPreviousResult')}</label>
                            <select id="${pid(fieldId, 'filterPreviousResult')}"
                                    name="previousResult"
                                    class="form-control">
                                    <option value="">${t('lblAll')}</option>
                            </select>
                        </div>
                        <hr>

                        <!-- نوع الزيارة -->
                        <div class="mb-3">
                            <label class="form-label">${t('lblVisitType')}</label>
                            <select id="${pid(fieldId, 'filterVisitType')}"
                                    name="visitType"
                                    class="form-control">
                                <option value="">${t('lblAll')}</option>
                                <!-- Will be populated dynamically -->
                            </select>
                        </div>
                        <hr>

                        <!-- المدرسة الأم - FIXED ID -->
                        <div class="mb-3">
                            <label class="form-label">${t('lblParentsSchool')}</label>
                            <select id="${pid(fieldId, 'filterParentOrgTree')}"
                                    name="parentOrgTree"
                                    class="form-control">
                                <option value="">${t('lblAll')}</option>
                                <!-- Will be populated dynamically -->
                            </select>
                        </div>

                        <div class="d-flex gap-2 mt-4">
                            <button type="submit" class="btn btn-primary w-100">
                                ${t('btnApply')}
                            </button>
                            <button type="button"
                                    class="btn btn-outline-primary w-100"
                                    id="${pid(fieldId, 'clearFiltersBtn')}">
                                ${t('btnClear')}
                            </button>
                        </div>

                    </form>
                </div>
            </div>
        `;
    }



})(planUtility);