window.assignmentsUtility = window.assignmentsUtility || {};
const assignmentsUtility = window.assignmentsUtility;

(function (ns) {
    'use strict';

    // ================== LOCALIZATION HELPER ==================
    function t(key, fallback = '') {
        try {
            const text = uiControlsSetup()?.GetUiControlText(key);
            return text || fallback || key;
        } catch {
            return fallback || key;
        }
    }

    // ================== ID HELPER ==================
    function tid(fieldId, name) {
        return `${fieldId}_${name}`;
    }

    // ================== PUBLIC ==================

    ns.generateAssignmentsHTML = function (fieldId) {
        let html = '';
        html += generateMembersCard(fieldId);
        html += generateSelectedTeamCard(fieldId);

        return `
            <main class="main-content">
                ${html}
            </main>
        `;
    };

    ns.generateAssignments = function (fieldId) {
        $(document).ready(async function () {
            await webAppConfigsSetup().Init({
                pageNames: [
                    'Assignment'
                ]
            }).then(() => {
                const html = ns.generateAssignmentsHTML(fieldId);

                const mainContent =
                    document.querySelector('.main-content') ||
                    document.querySelector('main') ||
                    document.body;

                const wrapper = document.createElement('div');
                wrapper.id = tid(fieldId, 'wrapper');
                wrapper.innerHTML = html;

                mainContent.appendChild(wrapper);
            });
        }); 
    };

    // ================== MEMBERS CARD ==================

    function generateMembersCard(fieldId) {
        return `
            <div class="card card-table mb-4" id="${tid(fieldId, 'membersCard')}">
                <div class="card-body">
                    ${generateMembersHeader(fieldId)}
                    ${generateMembersTable(fieldId)}
                </div>
            </div>
        `;
    }

    function generateMembersHeader(fieldId) {
        return `
        <div class="row align-items-center mb-3">
            <div class="col-xl-4">
                <h5>${t('lblMembers')}</h5>
            </div>

            <div class="col-xl-8">
                <div class="d-flex justify-content-end gap-2">
                    <div style="width: 220px;">
                        ${generateTeamFilter(fieldId)}
                    </div>

                    <div style="width: 220px;">
                        ${generateSearchBox(fieldId)}
                    </div>
                </div>
            </div>
        </div>
    `;
    }

    function generateTeamFilter(fieldId) {
        return `
            <select id="${tid(fieldId, 'teamFilter')}"
                    class="form-select form-select-md">
                <option value="">
                    ${t('lblAllTeams')}
                </option>
            </select>
        `;
    }

    function generateSearchBox(fieldId) {
        return `
            <div class="input-group m-0 p-0">
                <input type="text"
                       id="${tid(fieldId, 'customSearch')}"
                       class="form-control custom-search"
                       placeholder="${t('phSearchHere')}">
            </div>
        `;
    }

    function generateFilterButton(fieldId) {
        return `
            <button class="btn filterbtn"
                    data-bs-toggle="offcanvas"
                    data-bs-target="#${tid(fieldId, 'filterOffcanvas')}">
                <i class="la la-filter"></i> ${t('btnFilter')}
            </button>
        `;
    }

    function generateMembersTable(fieldId) {
        return `
            <div class="table-custom">
                <table id="${tid(fieldId, 'userTable')}"
                       class="table table-bordered table-hover w-100">
                    <thead class="table-light">
                        <tr>
                          <th style="width:50px;" class="text-center">
    <button type="button"
            id="${tid(fieldId, 'addAllMembers')}"
            class="team-add-btn bulk-add-btn"
            title="Add all">
        <i class="las la-plus"></i>
    </button>
</th>
                            <th>${t('lblMemberName')}</th>
                            <th>${t('lblJobTitle')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="3" class="text-center py-4">
                                ${t('lblLoading')}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        `;
    }

    // ================== SELECTED TEAM ==================

    function generateSelectedTeamCard(fieldId) {
        return `
            <div class="card card-table" id="${tid(fieldId, 'selectedTeamCard')}">
                <div class="card-body">
                    ${generateSelectedTeamHeader(fieldId)}
                    ${generateSelectedTeamTable(fieldId)}
                </div>
            </div>
        `;
    }

    function generateSelectedTeamHeader(fieldId) {
        return `
            <div class="row align-items-center mb-3">
                <div class="col-md-8">
                    <h4>${t('lblSelectedVisitTeam')}</h4>
                </div>
                <div class="col-md-4">
                    ${generateSuccessAlert(fieldId)}
                </div>
            </div>
        `;
    }

    function generateSuccessAlert(fieldId) {
        return `
            <div id="${tid(fieldId, 'successAlert')}"
                 class="alert alert-success d-none">
                ${t('msgMembersAddedSuccessfully')}
            </div>
        `;
    }

    function generateSelectedTeamTable(fieldId) {
        return `
            <div class="table-custom">
                <table id="${tid(fieldId, 'selectedTeamTable')}"
                       class="table table-bordered table-hover w-100">
                    <thead class="table-light">
                        <tr>
                            <th style="width:50px;">
                                ${generateCheckbox(tid(fieldId, 'selectAllSelected'))}
                            </th>
                            <th>${t('lblMemberName')}</th>
                            <th>${t('lblNDA')}</th>
                            <th>${t('lblDomain')}</th>
                            <th>${t('lblTeamLeader')}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="5"
                                class="text-center py-4 text-muted">
                                ${t('lblNoSelectedMembers')}
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        `;
    }

    function generateSelectedTeamActions(fieldId) {
        return `
            <div class="row mt-3">
                <div class="col-md-6 d-flex gap-2">
                    <button id="${tid(fieldId, 'saveTeamBtn')}"
                            class="btn btn-primary">
                        <i class="las la-save"></i> ${t('btnSaveTeam')}
                    </button>

                    <button id="${tid(fieldId, 'deleteSelectedBtn')}"
                            class="btn btn-outline-danger">
                        <i class="las la-trash"></i> ${t('btnDeleteSelected')}
                    </button>
                </div>
            </div>
        `;
    }

    // ================== HELPERS ==================

    function generateCheckbox(id) {
        return `
            <label class="custom-checkbox1">
                <input type="checkbox" id="${id}">
               
            </label>
        `;
    }

    function generateCheckBoxFormBuilder(id) {
        const label = $('<label>', { class: 'custom-checkbox1' });
        const input = $('<input>', { type: 'checkbox', id });
       

        label.append(input, span);
        return label;
    }

})(assignmentsUtility);