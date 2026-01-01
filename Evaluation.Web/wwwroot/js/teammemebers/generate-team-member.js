window.teamMembersUtility = window.teamMembersUtility || {};
const teamMembersUtility = window.teamMembersUtility;

(function (ns) {
    'use strict';

    // ================== ID HELPER ==================
    function tid(fieldId, name) {
        return `${fieldId}_${name}`;
    }

    // ================== PUBLIC ==================

    ns.generateTeamMembersHTML = function (fieldId) {
        let html = '';
        html += generateMembersCard(fieldId);
        html += generateSelectedTeamCard(fieldId);
        return `
            <main class="main-content">
                ${html}
            </main>
        `;
    };

    ns.generateTeamMembers = function (fieldId) {
        const html = ns.generateTeamMembersHTML(fieldId);

        const mainContent =
            document.querySelector('.main-content') ||
            document.querySelector('main') ||
            document.body;

        const wrapper = document.createElement('div');
        wrapper.id = tid(fieldId, 'wrapper');
        wrapper.innerHTML = html;

        mainContent.appendChild(wrapper);
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
                    <h4>الأعضاء</h4>
                </div>
                <div class="col-xl-8">
                    <div class="row">
                        <div class="col-md-4 mb-2">
                            ${generateTeamFilter(fieldId)}
                        </div>
                        <div class="col-md-4 mb-2">
                            ${generateSearchBox(fieldId)}
                        </div>
                        <div class="col-md-4 mb-2">
                            ${generateFilterButton(fieldId)}
                        </div>
                    </div>
                </div>
            </div>
        `;
    }

    function generateTeamFilter(fieldId) {
        return `
            <select id="${tid(fieldId, 'teamFilter')}"
                    class="form-select form-select-lg">
                <option value="">جميع الفرق</option>
                <!-- سيتم تعبئة الخيارات من JavaScript -->
            </select>
        `;
    }

    function generateSearchBox(fieldId) {
        return `
            <div class="input-group">
                <span class="input-group-text bg-transparent">
                    <i class="las la-search"></i>
                </span>
                <input type="text"
                       id="${tid(fieldId, 'customSearch')}"
                       class="form-control"
                       placeholder="ابحث هنا...">
            </div>
        `;
    }

    function generateFilterButton(fieldId) {
        return `
            <button class="btn filterbtn"
                    data-bs-toggle="offcanvas"
                    data-bs-target="#${tid(fieldId, 'filterOffcanvas')}">
                <i class="la la-filter"></i> تصفية
            </button>
        `;
    }

    function generateMembersTable(fieldId) {
        return `
            <div class="table-responsive">
                <table id="${tid(fieldId, 'userTable')}"
                       class="table table-bordered table-hover w-100">
                    <thead class="table-light">
                        <tr>
                            <th style="width:50px;">
                                ${generateCheckbox(tid(fieldId, 'selectAllMembers'))}
                            </th>
                            <th>اسم العضو</th>
                            <th>المسمى الوظيفي</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="3" class="text-center py-4">
                                جاري التحميل...
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
                    ${generateSelectedTeamActions(fieldId)}
                </div>
            </div>
        `;
    }

    function generateSelectedTeamHeader(fieldId) {
        return `
            <div class="row align-items-center mb-3">
                <div class="col-md-8">
                    <h4>فريق الزيارة المحدد</h4>
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
                تم إضافة الأعضاء بنجاح
            </div>
        `;
    }

    function generateSelectedTeamTable(fieldId) {
        return `
            <div class="table-responsive">
                <table id="${tid(fieldId, 'selectedTeamTable')}"
                       class="table table-bordered table-hover w-100">
                    <thead class="table-light">
                        <tr>
                            <th style="width:50px;">
                                ${generateCheckbox(tid(fieldId, 'selectAllSelected'))}
                            </th>
                            <th>اسم العضو</th>
                            <th>NDA</th>
                            <th>المجال</th>
                            <th>قائد الفريق</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="5"
                                class="text-center py-4 text-muted">
                                لا يوجد أعضاء محددين
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
                        <i class="las la-save"></i> حفظ الفريق
                    </button>

                    <button id="${tid(fieldId, 'deleteSelectedBtn')}"
                            class="btn btn-outline-danger">
                        <i class="las la-trash"></i> حذف المحدد
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
                <span class="checkmark"></span>
            </label>
        `;
    }

})(teamMembersUtility);