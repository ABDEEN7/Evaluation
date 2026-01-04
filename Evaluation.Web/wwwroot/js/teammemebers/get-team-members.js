window.teamMembersLogic = window.teamMembersLogic || {};

(function (ns, $) {
    'use strict';

    // ================== STATE ==================
    const state = {
        fieldId: null,
        teams: [],
        members: [],
        allMembers: [],
        selectedTeamMembers: [],
        scopes: [],
        pendingRemoval: new Set(),
        teamLeaderId: null
    };

    // ================== ID HELPER ==================
    function id(name) {
        return `#${state.fieldId}_${name}`;
    }

    // ================== API ==================
    const TeamApi = {
        getTeams() {
            return jqClient().Get(API_ENDPOINTS.GET_TEAMS);
        },
        getMembersByTeam(teamId) {
            return jqClient().Get(
                `${API_ENDPOINTS.GET_MEMBERS_BY_TEAM}?teamId=${teamId ?? ''}`
            );
        },
        getScopes() {
            return jqClient().Get(API_ENDPOINTS.GET_SCOPES);
        }
    };

    // ================== LOADERS ==================

    async function loadTeams() {
        try {
            const res = await TeamApi.getTeams();
            state.teams = res?.value ?? [];
            populateTeamDropdown();
            return state.teams.length > 0;
        } catch {
            showError('حدث خطأ في تحميل الفرق');
            return false;
        }
    }

    async function loadAllMembers() {
        showMembersLoading();
        try {
            const res = await TeamApi.getMembersByTeam(null);
            state.allMembers = res?.value ?? [];
            state.members = state.allMembers;
            renderMembersTable();
        } catch {
            showError('خطأ في تحميل الأعضاء');
        }
    }

    async function loadMembersByTeam(teamId) {
        showMembersLoading();
        try {
            const res = await TeamApi.getMembersByTeam(teamId);
            state.members = res?.value ?? [];

            // Add to allMembers if not exists
            state.members.forEach(member => {
                if (!state.allMembers.find(m => m.id === member.id)) {
                    state.allMembers.push(member);
                }
            });
            renderMembersTable();
        } catch {
            showError('خطأ في تحميل أعضاء الفريق');
        }
    }

    async function loadScopes() {
        try {
            const res = await TeamApi.getScopes();
            state.scopes = res?.value ?? [];
        } catch {
            showError('خطأ في تحميل المجالات');
        }
    }

    // ================== UI POPULATION ==================

    function populateTeamDropdown() {
        const $select = $(id('teamFilter'));

        if (!$select.length) {
            return;
        }

        if (!state.teams.length) {
            return;
        }

        const options = state.teams.map(t =>
            `<option value="${t.id}">${t.name}</option>`
        ).join('');

        $select.append(options);

        $select.off('change').on('change', function () {
            const teamId = $(this).val();
            if (teamId) {
                const team = state.teams.find(t => t.id === teamId);
                loadMembersByTeam(teamId);
            } else {
                loadAllMembers();
            }
        });
    }

    // ================== RENDER ==================

    function renderMembersTable() {
        const $tbody = $(id('userTable')).find('tbody');

        if (!$tbody.length) {
            return;
        }

        if (!state.members.length) {
            $tbody.html(`
                <tr>
                    <td colspan="3" class="text-center text-muted py-4">
                        لا يوجد أعضاء
                    </td>
                </tr>
            `);
            updateSelectAllCheckbox();
            return;
        }

        const rows = state.members.map(m => {
            const checked = state.selectedTeamMembers.some(x => x.id === m.id);
            const memberName = m.name || m.fullName || m.memberName || 'غير محدد';
            const memberPosition = m.position || m.jobTitle || m.title || 'غير محدد';

            return `
                <tr data-id="${m.id}">
                    <td>
                        <label class="custom-checkbox1 plus">
                            <input type="checkbox" class="row-select"
                                   ${checked ? 'checked' : ''}>
                            <span class="checkmark"></span>
                        </label>
                    </td>
                    <td><h6>${memberName}</h6></td>
                    <td><h6>${memberPosition}</h6></td>
                </tr>
            `;
        }).join('');

        $tbody.html(rows);
        updateSelectAllCheckbox();
    }

    function renderSelectedTeamTable() {
        const $tbody = $(id('selectedTeamTable')).find('tbody');

        if (!$tbody.length) {
            return;
        }

        if (!state.selectedTeamMembers.length) {
            $tbody.html(`
                <tr>
                    <td colspan="5" class="text-center text-muted py-4">
                        لا يوجد أعضاء محددين
                    </td>
                </tr>
            `);
            updateSelectedCheckboxHeader();
            return;
        }

        const rows = state.selectedTeamMembers.map((member, index) => {
            const memberName = member.name || member.fullName || member.memberName || 'غير محدد';
            const memberPosition = member.position || member.jobTitle || member.title || 'غير محدد';
            const isPending = state.pendingRemoval.has(member.id);

            return `
                <tr data-selected-id="${member.id}">
                    <td>
                        <label class="custom-checkbox1 ${isPending ? 'minus' : 'plus'}">
                            <input type="checkbox" class="selected-row-checkbox" ${isPending ? 'checked' : ''}>
                            <span class="checkmark"></span>
                        </label>
                    </td>
                    <td><h6>${memberName}</h6></td>
                    <td>
                        ${member.nda ? `
                            <div class="d-flex align-items-center justify-content-between">
                                <div>
                                    <h6>${member.nda.status}</h6>
                                    <div class="square-bullet">
                                        <div><i class="la la-calendar"></i> ${member.nda.date}</div>
                                        <div><i class="la la-clock"></i> ${member.nda.time}</div>
                                    </div>
                                </div>
                                ${member.nda.hasConflict ? `
                                    <span class="btn btn-outline-primary btn-sm d-flex gap-2">
                                        <i class="la la-info-circle"></i>ضعيف
                                    </span>
                                ` : ''}
                            </div>
                        ` : `<h6>${memberPosition}</h6>`}
                    </td>
                    <td>
                        <div class="mb-3 w-100">
                            <select multiple class="form-control multiCheckSelect-dynamic" data-member-id="${member.id}">
                                ${state.scopes.map(scope => `
                                    <option value="${scope.id}">${scope.name}</option>
                                `).join('')}
                            </select>
                        </div>
                    </td>
                    <td>
              <label class="custom-checkbox1 radio">
                <input type="radio"
                        class="team-leader-radio"
                        name="teamLeader"
                        value="${member.id}"
                        ${state.teamLeaderId == member.id ? 'checked' : ''}>
                <span class="checkmark"></span>
            </label>

                    </td>
                </tr>
            `;
        }).join('');

        $tbody.html(rows);
        setTimeout(() => {
            initializeSelect2();
        }, 100);

        updateSelectedCheckboxHeader();
    }

    // ================== SELECT2 INITIALIZATION ==================

    function initializeSelect2() {
        if ($(".multiCheckSelect-dynamic").hasClass("select2-hidden-accessible")) {
            $(".multiCheckSelect-dynamic").select2('destroy');
        }

        function formatCheckbox(option) {
            if (!option.id) return option.text;

            return $(`
                <div class="checkbox-item">
                    <input type="checkbox" class="chk" data-id="${option.id}">
                    <label>${option.text}</label>
                </div>
            `);
        }

        $(".multiCheckSelect-dynamic").select2({
            closeOnSelect: false,
            templateResult: formatCheckbox,
            templateSelection: (item) => item.text,
            width: '100%',
            placeholder: 'اختر المجالات',
            dir: 'rtl'
        });

        $(".multiCheckSelect-dynamic").on("select2:open", function () {
            const memberId = $(this).data('member-id');
            const member = state.selectedTeamMembers.find(m => m.id === memberId);
            const selected = member?.scopes || [];

            setTimeout(() => {
                $(".chk").each(function () {
                    const id = parseInt($(this).data("id"));
                    $(this).prop("checked", selected.includes(id));
                });

                $(".chk").off("change").on("change", function () {
                    const id = parseInt($(this).data("id"));
                    const selectElement = $(`.multiCheckSelect-dynamic[data-member-id='${memberId}']`);
                    let current = selectElement.val() || [];
                    current = current.map(v => parseInt(v));

                    if (this.checked) {
                        if (!current.includes(id)) {
                            current.push(id);
                        }
                    } else {
                        current = current.filter(v => v !== id);
                    }

                    selectElement.val(current.map(String)).trigger("change");

                    if (member) {
                        member.scopes = current;
                    }
                });
            }, 50);
        });
    }

    // ================== UPDATE CHECKBOXES ==================

    function updateSelectAllCheckbox() {
        const total = $(`${id('userTable')} .row-select`).length;
        const checked = $(`${id('userTable')} .row-select:checked`).length;

        $(id('selectAllMembers')).prop('checked', total > 0 && total === checked);
    }

    function updateSelectedCheckboxHeader() {
        const total = $('.selected-row-checkbox').length;
        const checked = $('.selected-row-checkbox:checked').length;

        $(id('selectAllSelected')).prop('checked', total > 0 && total === checked);
    }

    // ================== EVENT HANDLERS ==================

    function initEventListeners() {
        //Added event to Radio button
        $(document).off('change', '.team-leader-radio')
            .on('change', '.team-leader-radio', function () {
                const leaderId = $(this).val();
                state.teamLeaderId = leaderId;

                console.log('👑 قائد الفريق:', leaderId);
            });
        // Select All في جدول الأعضاء
        $(document).off('change', id('selectAllMembers')).on('change', id('selectAllMembers'), function () {
            const isChecked = this.checked;
            $(`${id('userTable')} .row-select`).each(function () {
                if ($(this).prop('checked') !== isChecked) {
                    $(this).prop('checked', isChecked).trigger('change');
                }
            });
        });

        // إضافة عضو للفريق المحدد
        $(document).off('change', `${id('userTable')} .row-select`).on('change', `${id('userTable')} .row-select`, function () {
            // منع الإزالة من الجدول الأعلى - الأعلى إضافة فقط
            if (!this.checked) {
                this.checked = true;
                return;
            }

            const row = $(this).closest('tr');
            const memberId = row.data('id');
            const member = state.members.find(m => m.id === memberId);

            if (!member) return;

            if (!state.selectedTeamMembers.find(m => m.id === memberId)) {
                state.selectedTeamMembers.push({
                    ...member,
                    scopes: [],
                    nda: null
                });
                if (!state.teamLeaderId) {
                    state.teamLeaderId = memberId;
                }
                const memberName = member.name || member.fullName || member.memberName;
                showSuccess('تم إضافة ' + memberName);
                renderSelectedTeamTable();
            }

            updateSelectAllCheckbox();
        });

        // التعامل مع checkbox في الجدول السفلي - الضغطة الأولى: minus، الثانية: حذف
        $(document).off('change', '.selected-row-checkbox').on('change', '.selected-row-checkbox', function () {
            const $checkbox = $(this);
            const $row = $checkbox.closest('tr');
            const $label = $checkbox.closest('.custom-checkbox1');
            const memberId = $row.data('selected-id');
            const isChecked = $checkbox.prop('checked');

            if (isChecked) {
                // الضغطة الأولى: إضافة للحذف وتغيير إلى minus
                state.pendingRemoval.add(memberId);
                $label.removeClass('plus').addClass('minus');
            } else {
                // الضغطة الثانية: تنفيذ الحذف الفعلي
                state.pendingRemoval.delete(memberId);

                // إزالة من الفريق
                const member = state.selectedTeamMembers.find(m => m.id === memberId);
                state.selectedTeamMembers = state.selectedTeamMembers.filter(m => m.id !== memberId);

                // إعادة تفعيل العضو في الجدول الأعلى
                $(`${id('userTable')} tr[data-id="${memberId}"] .row-select`).prop('checked', false);

                const memberName = member?.name || member?.fullName || member?.memberName;
                showSuccess('تم حذف ' + memberName);

                // إعادة رسم الجدول
                renderSelectedTeamTable();
            }

            updateSelectAllCheckbox();
            updateSelectedCheckboxHeader();
        });

        // Select All في جدول الفريق المحدد
        $(document).off('change', id('selectAllSelected')).on('change', id('selectAllSelected'), function () {
            const isChecked = this.checked;

            $('.selected-row-checkbox').each(function () {
                const $checkbox = $(this);
                const $row = $checkbox.closest('tr');
                const $label = $checkbox.closest('.custom-checkbox1');
                const memberId = $row.data('selected-id');

                if (isChecked) {
                    // تحديد الكل: إضافة شعار minus
                    $checkbox.prop('checked', true);
                    state.pendingRemoval.add(memberId);
                    $label.removeClass('plus').addClass('minus');
                } else {
                    // إلغاء التحديد: إزالة من pending
                    $checkbox.prop('checked', false);
                    state.pendingRemoval.delete(memberId);
                    $label.removeClass('minus').addClass('plus');
                }
            });
        });

        // البحث
        $(document).off('keyup', id('customSearch')).on('keyup', id('customSearch'), function () {
            const searchTerm = $(this).val().toLowerCase();

            $(`${id('userTable')} tbody tr`).each(function () {
                const name = $(this).find('td:eq(1)').text().toLowerCase();
                const position = $(this).find('td:eq(2)').text().toLowerCase();

                if (name.includes(searchTerm) || position.includes(searchTerm)) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        });

        // زر حذف المحدد
        $(document).off('click', id('deleteSelectedBtn')).on('click', id('deleteSelectedBtn'), function (e) {
            e.preventDefault();

            if (state.pendingRemoval.size === 0) {
                alert('الرجاء تحديد الأعضاء المراد حذفهم');
                return;
            }

            if (!confirm(`هل أنت متأكد من حذف ${state.pendingRemoval.size} عضو؟`)) {
                return;
            }

            // حذف الأعضاء المحددين
            state.pendingRemoval.forEach(memberId => {
                state.selectedTeamMembers = state.selectedTeamMembers.filter(m => m.id !== memberId);

                // إعادة تفعيل العضو في الجدول الأعلى
                $(`${id('userTable')} tr[data-id="${memberId}"] .row-select`).prop('checked', false);
            });

            // مسح قائمة الانتظار
            state.pendingRemoval.clear();

            renderSelectedTeamTable();
            renderMembersTable();
            showSuccess('تم حذف الأعضاء المحددين بنجاح');
        });

        // زر حفظ الفريق
        $(document).off('click', id('saveTeamBtn')).on('click', id('saveTeamBtn'), function (e) {
            e.preventDefault();

            if (state.selectedTeamMembers.length === 0) {
                alert('الرجاء تحديد أعضاء الفريق');
                return;
            }

            // استخدام الدالة الخارجية للحصول على البيانات
            const teamData = window.getSelectedTeamData(`${state.fieldId}_selectedTeamTable`);

            if (!teamData) {
                showError('خطأ في استخراج بيانات الفريق');
                return;
            }

            console.log('📦 بيانات الفريق:', teamData);
           
            showSuccess(`تم حفظ الفريق بنجاح (${teamData.totalMembers} أعضاء)`);
        });
    }

    // ================== HELPERS ==================

    function showMembersLoading() {
        const $tbody = $(id('userTable')).find('tbody');

        if (!$tbody.length) {
            return;
        }

        $tbody.html(`
            <tr>
                <td colspan="3" class="text-center py-4">
                    <i class="la la-spinner la-spin"></i> جاري التحميل...
                </td>
            </tr>
        `);
    }

    function showSuccess(msg) {
        const $alert = $(id('successAlert'));
        if (!$alert.length) {
            // Try alternative success alert
            const $bgSuccess = $('.bg-success-light');
            if ($bgSuccess.length) {
                $bgSuccess.find('p').text(msg);
                $bgSuccess.show();
                setTimeout(() => $bgSuccess.fadeOut(), 3000);
            } else {
                console.log('✅', msg);
            }
            return;
        }

        $alert.text(msg).removeClass('d-none');
        setTimeout(() => $alert.addClass('d-none'), 3000);
    }

    function showError(msg) {
        console.error('❌', msg);
        alert(msg);
    }

    // ================== INIT ==================

    ns.init = async function (fieldId) {
        console.log('========================================');
        console.log('🚀 Initializing Team Management System');
        console.log('Field ID:', fieldId);
        console.log('========================================');

        state.fieldId = fieldId;

        // التحقق من وجود العناصر الأساسية
        const $teamFilter = $(id('teamFilter'));
        const $userTable = $(id('userTable'));
        const $selectedTeamTable = $(id('selectedTeamTable'));

        if (!$teamFilter.length) {
            console.error('❌ عنصر teamFilter غير موجود');
        }

        if (!$userTable.length) {
            console.error('❌ عنصر userTable غير موجود');
        }

        if (!$selectedTeamTable.length) {
            console.error('❌ عنصر selectedTeamTable غير موجود');
        }

        // إخفاء تنبيه النجاح
        $('.bg-success-light').hide();

        // تحميل البيانات
        console.log('📥 جاري تحميل الفرق...');
        const teamsLoaded = await loadTeams();

        if (teamsLoaded) {
            console.log('📥 جاري تحميل الأعضاء...');
            await loadAllMembers();
        } else {
            console.error('❌ Failed to initialize: No teams loaded');
            showError('فشل تحميل البيانات الأساسية');
        }

        console.log('📥 جاري تحميل المجالات...');
        await loadScopes();

        // تهيئة معالجات الأحداث
        initEventListeners();

        console.log('✅ اكتمل التهيئة بنجاح');
        console.log('========================================');
    };

})(window.teamMembersLogic, jQuery);