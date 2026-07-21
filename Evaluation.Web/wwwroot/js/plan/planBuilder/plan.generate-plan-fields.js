window.planUtility = window.planUtility || {};
const planUtility = window.planUtility;

(function (ns) {
    'use strict';

    // ================== LOCALIZATION Helper ==================
  

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
        html += `<div id="${pid(fieldId, 'filterOffcanvasPlaceholder')}"></div>`;
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

    ns.generateFilterOffcanvasHTML = generateFilterOffcanvas;
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
                                ${localization('lblPlanTitle')} <span class="text-danger">*</span>
                            </label>
                            <input type="text"
                                   id="${pid(fieldId, 'planTitle')}"
                                   name="PlanTitle"
                                   class="form-control"
                                   placeholder="${localization('plhEnterPlanTitle')}"
                                   ${VALIDATION_RULES?.TITLE?.required ? 'required' : ''}>
                        </div>
                            <div id="${pid(fieldId, 'planTitle_error')}" class="error-message text-danger"></div>

                    <!-- Plan Type -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'ddlPlanType')}" class="form-label">
                                ${localization('lblPlanType')} <span class="text-danger">*</span>
                            </label>
                            <select id="${pid(fieldId, 'ddlPlanType')}"
                                    name="PlanTypeId"
                                    class="form-control"
                                    ${VALIDATION_RULES?.PLAN_TYPE?.required ? 'required' : ''}>
                                <option value="">${localization('lblChoosePlanType')}</option>
                            </select>
                        </div>
                            <div id="${pid(fieldId, 'ddlPlanType_error')}" class="error-message text-danger"></div>
                    </div>

                    <!-- Semester -->
                    <div id="${pid(fieldId, 'semesterContainer')}" class="col-md-4" style="display:none;">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'ddlSemester')}" class="form-label">
                                ${localization('lblSemester')}
                            </label>
                            <select id="${pid(fieldId, 'ddlSemester')}"
                                    name="SemesterId"
                                    class="form-control">
                                <option value="">${localization('lblChooseSemester')}</option>
                            </select>
                            <div id="${pid(fieldId, 'ddlSemester_error')}" class="error-message text-danger"></div>
                        </div>
                    </div>

                    <!-- Date -->
                    <div class="col-md-4">
                        <div class="mb-4">
                            <label for="${pid(fieldId, 'parentDate')}" class="form-label">
                                ${localization('lblTimePeriod')} <span class="text-danger">*</span>
                            </label>
                            <div class="input-group datetime">
                                <input type="text"
                                       id="${pid(fieldId, 'parentDate')}"
                                       name="dateRange"
                                       class="form-control datePicker"
                                       placeholder="${localization('plhChooseStartEndDate')}">
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
    <div class="col-md-10 d-flex align-items-center gap-3">
        <h4 class="mb-0">${localization('lblSelectSchools')}</h4>

        <div class="selection-counter-badge d-flex align-items-center gap-1 
                     p-2 rounded-pill border custom-badge-count">
            
            <span>${localization('lblSelectedSchools') || 'المدارس المحددة'}:</span>
            <span id="${pid(fieldId, 'selectedSchoolsCounter')}">0</span>
        </div>
        <div class="form-check form-switch ms-2">
    <input class="form-check-input" type="checkbox"
           id="${pid(fieldId, 'showSelectedOnly')}" >
    <label class="form-check-label" for="${pid(fieldId, 'showSelectedOnly')}">
        ${localization('lblShowSelectedOnly') || 'المحدد فقط'}
    </label>
</div>
    </div>

    <div class="col-md-2">
                           
                              
                                    <button type="button" class="btn filterbtn"
                                            data-bs-toggle="offcanvas"
                                            data-bs-target="#${pid(fieldId, 'filterOffcanvas')}">
                                        <i class="la la-filter"></i> ${localization('btnFilter')}
                                    </button>
                              
                           
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
                            <th>${localization('lblSchoolName')}</th>
                            <th>${localization('lblVisitDate')}</th>
                            <th>${localization('lblEstablishmentDate')}</th>
                            <th>${localization('lblVisitType')}</th>
                            <th>${localization('lblAcademicYear')}</th>
                            <th>${localization('lblActions')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="7" class="text-center py-5">
                                ${localization('msgLoading')}
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
        const cfg = window.planUtility?.depConfig || {};
        const enabled = (key) => cfg[key] === true;

        const blocks = [];

        if (enabled('schoolName')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblSchoolName')}</label>
                <input type="text"
                       id="${pid(fieldId, 'filterSchoolName')}"
                       name="schoolName"
                       class="form-control"
                       placeholder="${localization('plhWriteHere')}">
            </div>
            <hr>`);
        }

        if (enabled('lastEvalDate')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblLastEvalDate')}</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="la la-calendar"></i></span>
                    <input type="text"
                           id="${pid(fieldId, 'filterLastEvalDate')}"
                           name="lastEvalDate"
                           class="form-control filter-date-picker"
                           placeholder="${localization('plhChooseDate')}">
                </div>
            </div>
            <hr>`);
        }

        if (enabled('createdDate')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblCreatedDate')}</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="la la-calendar"></i></span>
                    <input type="text"
                           id="${pid(fieldId, 'filterCreatedDate')}"
                           name="createdDate"
                           class="form-control filter-date-picker"
                           placeholder="${localization('plhChooseDate')}">
                </div>
            </div>
            <hr>`);
        }

        if (enabled('createdToDate')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblCreatedDateTo')}</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="la la-calendar"></i></span>
                    <input type="text"
                           id="${pid(fieldId, 'filterToCreatedDate')}"
                           name="toCreatedDate"
                           class="form-control filter-date-picker"
                           placeholder="${localization('plhChooseDate')}">
                </div>
            </div>
            <hr>`);
        }

        if (enabled('nextEvalDate')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblNextEvalDate')}</label>
                <div class="input-group">
                    <span class="input-group-text"><i class="la la-calendar"></i></span>
                    <input type="text"
                           id="${pid(fieldId, 'filterNextEvalDate')}"
                           name="nextEvalDate"
                           class="form-control filter-date-picker"
                           placeholder="${localization('plhChooseDate')}">
                </div>
            </div>
            <hr>`);
        }

        if (enabled('previousResult')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblPreviousResult')}</label>
                <select id="${pid(fieldId, 'filterPreviousResult')}"
                        name="previousResult"
                        class="form-control">
                    <option value="">${localization('lblAll')}</option>
                    <option value="Perfect">${localization('lblPerfect')}</option>
                    <option value="VeryGood">${localization('lblVeryGood')}</option>
                    <option value="Good">${localization('lblGood')}</option>
                    <option value="Acceptable">${localization('lblAcceptable')}</option>
                    <option value="Week">${localization('lblWeak')}</option>
                </select>
            </div>
            <hr>`);
        }

        if (enabled('visitType')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblVisitType')}</label>
                <select id="${pid(fieldId, 'filterVisitType')}"
                        name="visitType"
                        class="form-control">
                    <option value="">${localization('lblAll')}</option>
                </select>
            </div>
            <hr>`);
        }

        if (enabled('parentOrgTree')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblParentsSchool')}</label>
                <select id="${pid(fieldId, 'filterParentOrgTree')}"
                        name="parentOrgTree"
                        class="form-control">
                    <option value="">${localization('lblAll')}</option>
                </select>
            </div>
            <hr>`);
        }

        if (enabled('schoolLevel')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblSchoolLevel')}</label>
                <select id="${pid(fieldId, 'filterSchoolLevel')}"
                        name="schoolLevel"
                        class="form-control">
                    <option value="">${localization('lblAll')}</option>
                </select>
            </div>
            <hr>`);
        }

        if (enabled('gender')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblGender')}</label>
                <select id="${pid(fieldId, 'filterGender')}"
                        name="gender"
                        class="form-control">
                    <option value="">${localization('lblAll')}</option>
                </select>
            </div>
            <hr>`);
        }

        if (enabled('grade')) {
            blocks.push(`
            <div class="mb-3">
                <label class="form-label">${localization('lblGrade')}</label>
                <select id="${pid(fieldId, 'filterGrade')}"
                        name="grade"
                        class="form-control">
                    <option value="">${localization('lblAll')}</option>
                </select>
            </div>`);
        }

        return `
        <div class="offcanvas offcanvas-end" tabindex="-1" id="${pid(fieldId, 'filterOffcanvas')}">
            <div class="offcanvas-header justify-content-between p-4">
                <h5 class="offcanvas-title border d-flex align-items-center w-75 justify-content-between px-3 py-2 rounded">
                    <span>${localization('lblFilterResults')}</span>
                    <i class="la la-filter"></i>
                </h5>
                <button type="button" class="btn btn-lg border d-flex align-items-center h-100" data-bs-dismiss="offcanvas">
                    <i class="la la-angle-right"></i>
                </button>
            </div>
            <div class="offcanvas-body p-4">
                <form id="${pid(fieldId, 'filterForm')}">
                    ${blocks.join('')}
                    <div class="d-flex gap-2 mt-4">
                        <button type="submit" class="btn btn-primary w-100">${localization('btnApply')}</button>
                        <button type="button" class="btn btn-outline-primary w-100" id="${pid(fieldId, 'clearFiltersBtn')}">
                            ${localization('btnClear')}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    `;
    }


})(planUtility);