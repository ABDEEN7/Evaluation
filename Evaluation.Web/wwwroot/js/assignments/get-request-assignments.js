window.assignmentsLogic = window.assignmentsLogic || {};

(function (ns, $) {
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

    // ================== STATE ==================
    const state = {
        fieldId: null,
        evaluationRequestId: null,
        teams: [],
        members: [],
        allMembers: [],
        selectedAssignments: [],
        scopes: [],
        pendingRemoval: new Set(),
        isNDA: false,
        teamLeaderId: null,
        ndaStatus: {},
        existingAssignments: [],
        $root: null
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
        },
        getPendingStatus() {
            return jqClient().Get(API_ENDPOINTS.GET_PENDING_STATUS);
        },
        getAssignmentsByEvaluationRequest(evaluationRequestId) {
            return jqClient().Get(
                `${API_ENDPOINTS.GET_TEAM_MEMBERS_BY_EVALUATION_REQUEST}?evaluationRequestId=${evaluationRequestId}`
            );
        }
    };

    // ================== LOADERS ==================

    async function loadTeams() {
        try {
            const res = await TeamApi.getTeams();
            state.teams = res?.value?.data ?? [];
            state.isNDA = res?.value?.isNDA;

            populateTeamDropdown();
            return state.teams.length > 0;
        } catch {
            showError('حدث خطأ في تحميل الفرق');
            return false;
        }
    }

    async function loadPendingNDA() {
        const res = await TeamApi.getPendingStatus();
        state.ndaStatus = {
            id: res.value.id,
            name: res.value.name
        };
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

    /**
     * Load existing team members from evaluation request
     */
    async function loadExistingAssignments(evaluationRequestId) {
        if (!evaluationRequestId) {
            return;
        }

        try {
            const res = await TeamApi.getAssignmentsByEvaluationRequest(evaluationRequestId);

            if (!res?.isSuccess || !res?.value) {
                console.warn('⚠️ No existing team members found');
                return;
            }

            const assignments = res.value;
            state.existingAssignments = assignments;

            for (const assignment of assignments) {
                const memberId = assignment.ministryUserId;

                let member = state.allMembers.find(m => m.id === memberId);

                if (!member) {
                    member = state.members.find(m => m.id === memberId);

                    if (!member) {
                        console.warn(`⚠️ Member ${memberId} not found in available members`);
                        continue;
                    }
                }

                if (state.selectedAssignments.find(m => m.id === memberId && m.partyTypeId === assignment.partyTypeId)) {
                    continue;
                }

                // Map scopes from API format to array of IDs
                const scopes = (assignment.evalRequestAssignmentScopies || [])
                    .map(scope => scope.id)
                    .filter(Boolean);

                state.selectedAssignments.push({
                    ...member,
                    id: memberId,
                    partyTypeId: assignment.partyTypeId,
                    scopes: scopes,
                    nda: assignment.isNDA ? assignment.ndaStatusId : null,
                    isLeader: assignment.isLeader,
                    assignmentData: assignment
                });

                if (assignment.isLeader) {
                    state.teamLeaderId = memberId;
                }

                $(`${id('userTable')} tr[data-id="${memberId}"] .team-add-btn`)
                    .addClass('selected')
                    .html('');
            }

            renderSelectedTeamTable();
            updateSelectAllCheckbox();

            console.log('✅ Loaded existing team members:', state.selectedAssignments.length);
        } catch (error) {
            console.error('❌ Error loading existing team members:', error);
            showError('خطأ في تحميل أعضاء الفريق المحفوظين');
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
                loadMembersByTeam(teamId);
            } else {
                loadAllMembers();
            }
        });
    }

    // ================== UPDATE TABLE HEADERS ==================

    function updateSelectedTeamTableHeader() {
        const $thead = $(id('selectedTeamTable')).find('thead tr');

        if (!$thead.length) {
            return;
        }

        let headerHTML = `
            <th style="width:50px;" class="text-center">
    <button type="button"
            id="${state.fieldId}_removeAllMembers"
            class="team-remove-btn bulk-remove-btn"
            title="Remove all">
        <i class="las la-times"></i>
    </button>
</th>
            <th>${t('lblMemberName')}</th>
        `;

        if (state.isNDA) {
            headerHTML += `<th>NDA</th>`;
        }

        headerHTML += `
            <th>${t('lblDomain')}</th>
            <th>${t('lblPartyType')}</th>
            <th>${t('lblTeamLeader')}</th>
        `;

        $thead.html(headerHTML);
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
                        ${t('lblNoMembers')}
                    </td>
                </tr>
            `);
            updateSelectAllCheckbox();
            return;
        }

        const rows = state.members.map(m => {
            const checked = state.selectedAssignments.some(x => x.id === m.id);
            const memberName = m.name || m.fullName || m.memberName || 'غير محدد';
            const memberPosition = m.position || m.jobTitle || m.title || 'غير محدد';

            return `
                <tr data-id="${m.id}" class="team-checkbox-container">
                    <td>
                        <button type="button"
                                class="team-add-btn ${checked ? 'selected' : ''}">
                       ${checked ? '' : '<i class="las la-plus"></i>'}
                        </button>
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

        if (!state.selectedAssignments.length) {
            const colspan = state.isNDA ? 6 : 5;
            $tbody.html(`
                <tr>
                    <td colspan="${colspan}" class="text-center text-muted py-4">
                        ${t('lblNoSelectedMembers')}
                    </td>
                </tr>
            `);
            updateSelectedCheckboxHeader();
            return;
        }

        const rows = state.selectedAssignments.map((member, index) => {
            const memberName = member.name || member.fullName || member.memberName || 'غير محدد';
            const memberPosition = member.position || member.jobTitle || member.title || 'غير محدد';
            const isPending = state.pendingRemoval.has(member.id);

            const userPartyTypes = member.userPartyTypes || [];

            // Use member.partyTypeId if it exists (from loaded data), otherwise auto-select
            const defaultPartyTypeId = member.partyTypeId ||
                (userPartyTypes.length === 1 ? userPartyTypes[0].partyType.id : null);

            // Build party type options
            const partyTypeOptionsHTML = userPartyTypes.length === 1
                ? `<option value="${userPartyTypes[0].partyType.id}" selected>
                       ${userPartyTypes[0].partyType.name}
                   </option>`
                : `<option value="">اختر نوع الطرف</option>
                   ${userPartyTypes.map(upt => `
                       <option value="${upt.partyType.id}"
                           ${defaultPartyTypeId == upt.partyType.id ? 'selected' : ''}>
                           ${upt.partyType.name}
                       </option>
                   `).join('')}`;

            // Build NDA cell if enabled
            let ndaCellHTML = '';
            if (state.isNDA) {
                const ndaChecked = member.nda === state.ndaStatus.id ? 'checked' : '';
                ndaCellHTML = `
                    <td data-nda-id="${member.nda || ''}">
                        <label class="custom-checkbox1">
                            <input type="checkbox" class="nda-checkbox" 
                                   data-member-id="${member.id}" 
                                   ${ndaChecked}>
                           
                        </label>
                    </td>
                `;
            }

            // Check if this member is team leader
            const isLeaderChecked = member.id === state.teamLeaderId || member.isLeader ? 'checked' : '';

            return `
    <tr data-selected-id="${member.id}">
      <td>
    <button type="button"
            class="team-remove-btn ${isPending ? 'selected' : ''}">
        <i class="las ${isPending ? 'la-minus' : 'la-times'}"></i>
    </button>
</td>
        <td>
            <h6>${memberName}</h6>
            <small class="text-muted">${memberPosition}</small>
        </td>
        ${ndaCellHTML}
        <td>
            <div class="w-100">
                <select multiple class="form-control multiCheckSelect-dynamic" 
                        data-member-id="${member.id}">
                    ${state.scopes.map(scope => `
                        <option value="${scope.id}">${scope.name}</option>
                    `).join('')}
                </select>
            </div>
        </td>
        <td>
            <select class="form-select party-type-select mt-0" data-member-id="${member.id}">
                ${partyTypeOptionsHTML}
            </select>
        </td>
        <td>
            <label class="custom-checkbox1 radio">
                <input type="radio"
                       class="team-leader-radio"
                       name="${state.fieldId}_teamLeader"
                       value="${member.id}"
                       data-member-id="${member.id}"
                       ${isLeaderChecked}>
             
            </label>
        </td>
    </tr>
`;
        }).join('');

        $tbody.html(rows);

        // Initialize Select2 after rendering
        setTimeout(() => {
            initializeSelect2();
        }, 100);

        updateSelectedCheckboxHeader();
    }

    // ================== SELECT2 INITIALIZATION ==================
    function initializeSelect2() {
        const $root = state.$root && state.$root.length ? state.$root : $(document);

        const $selects = $root.find(".multiCheckSelect-dynamic");

        // Destroy only inside this root
        $selects.each(function () {
            const $s = $(this);
            if ($s.hasClass("select2-hidden-accessible")) {
                $s.select2("destroy");
            }
        });

        function formatCheckbox(option) {
            if (!option.id) return option.text;
            return $(`
      <div class="checkbox-item">
        <input type="checkbox" class="chk" data-id="${option.id}">
        <label>${option.text}</label>
      </div>
    `);
        }

        $selects.each(function () {
            const $s = $(this);

            // ✅ dropdownParent مهم لو داخل modal
            const $dropdownParent = $s.closest(".modal").length ? $s.closest(".modal") : $root;

            $s.select2({
                closeOnSelect: false,
                templateResult: formatCheckbox,
                templateSelection: (item) => item.text,
                width: "100%",
                placeholder: "اختر المجالات",
                dir: "rtl",
                dropdownParent: $dropdownParent
            });

            // ✅ حدث open لكل select لوحده (بدون .chk global)
            $s.off("select2:open.assignScopes").on("select2:open.assignScopes", function () {
                const memberId = $s.data("member-id");
                const member = state.selectedAssignments.find(m => m.id === memberId);
                const selected = (member?.scopes || []).map(String);

                setTimeout(() => {
                    // ✅ خُد نتائج الـ dropdown الخاصة بالـ select ده فقط
                    const $results = $dropdownParent.find(".select2-results");

                    $results.find(".chk").each(function () {
                        const scopeId = String($(this).data("id"));
                        $(this).prop("checked", selected.includes(scopeId));
                    });

                    $results.find(".chk").off("change.assignScopes").on("change.assignScopes", function () {
                        const scopeId = String($(this).data("id"));
                        let current = ($s.val() || []).map(String);

                        if (this.checked) {
                            if (!current.includes(scopeId)) current.push(scopeId);
                        } else {
                            current = current.filter(v => v !== scopeId);
                        }

                        $s.val(current).trigger("change");

                        if (member) member.scopes = current;
                    });
                }, 0);
            });
        });

        // Pre-select values for loaded data
        state.selectedAssignments.forEach(member => {
            if (member.scopes?.length) {
                const $s = $root.find(`.multiCheckSelect-dynamic[data-member-id='${member.id}']`);
                if ($s.length) $s.val(member.scopes.map(String)).trigger("change");
            }
        });
    }

    //function initializeSelect2() {
    //    // Destroy existing Select2 instances
    //    if ($(".multiCheckSelect-dynamic").hasClass("select2-hidden-accessible")) {
    //        $(".multiCheckSelect-dynamic").select2('destroy');
    //    }

    //    function formatCheckbox(option) {
    //        if (!option.id) return option.text;

    //        return $(`
    //            <div class="checkbox-item">
    //                <input type="checkbox" class="chk" data-id="${option.id}">
    //                <label>${option.text}</label>
    //            </div>
    //        `);
    //    }

    //    $(".multiCheckSelect-dynamic").select2({
    //        closeOnSelect: false,
    //        templateResult: formatCheckbox,
    //        templateSelection: (item) => item.text,
    //        width: '100%',
    //        placeholder: 'اختر المجالات',
    //        dir: 'rtl'
    //    });

    //    // Handle Select2 opening and pre-selecting checkboxes
    //    $(".multiCheckSelect-dynamic").on("select2:open", function () {
    //        const memberId = $(this).data('member-id');
    //        const member = state.selectedAssignments.find(m => m.id === memberId);
    //        const selected = member?.scopes || [];

    //        setTimeout(() => {
    //            // Pre-check the checkboxes based on member's scopes
    //            $(".chk").each(function () {
    //                const scopeId = $(this).data("id");
    //                const isSelected = selected.includes(scopeId);
    //                $(this).prop("checked", isSelected);
    //            });

    //            // Handle checkbox change
    //            $(".chk").off("change").on("change", function () {
    //                const scopeId = $(this).data("id");
    //                const selectElement = $(`.multiCheckSelect-dynamic[data-member-id='${memberId}']`);
    //                let current = selectElement.val() || [];

    //                // Convert to proper type (string IDs)
    //                current = current.map(v => String(v));

    //                if (this.checked) {
    //                    if (!current.includes(String(scopeId))) {
    //                        current.push(String(scopeId));
    //                    }
    //                } else {
    //                    current = current.filter(v => v !== String(scopeId));
    //                }

    //                selectElement.val(current).trigger("change");

    //                // Update state
    //                if (member) {
    //                    member.scopes = current.map(id => String(id));
    //                }
    //            });
    //        }, 50);
    //    });

    //    // Pre-select values for loaded data
    //    state.selectedAssignments.forEach(member => {
    //        if (member.scopes && member.scopes.length > 0) {
    //            const $select = $(`.multiCheckSelect-dynamic[data-member-id='${member.id}']`);
    //            if ($select.length) {
    //                $select.val(member.scopes.map(String)).trigger('change');
    //            }
    //        }
    //    });
    //}

    // ================== UPDATE CHECKBOXES ==================

    function updateSelectAllCheckbox() {
        const $addAllBtn = $(id('addAllMembers'));
        if (!$addAllBtn.length) return;

        const hasMembersToAdd =
            $(`${id('userTable')} tbody tr[data-id] .team-add-btn:not(.selected)`).length > 0;

        if (hasMembersToAdd) {
            $addAllBtn.html('<i class="las la-plus"></i>');
        } else {
            $addAllBtn.html('');
        }
    }

    function updateSelectedCheckboxHeader() {
        const $removeAllBtn = $(id('removeAllMembers'));
        if (!$removeAllBtn.length) return;

        const hasSelectedMembers = state.selectedAssignments.length > 0;

        $removeAllBtn.html(
            hasSelectedMembers ? '<i class="las la-times"></i>' : ''
        );
    }

    // ================== EVENT HANDLERS ==================

    function initEventListeners() {
        // Team Leader Radio
        $(document).off('change', '.team-leader-radio')
            .on('change', '.team-leader-radio', function () {
                const leaderId = $(this).val();
                state.teamLeaderId = leaderId;

                // Update isLeader flag for all members
                state.selectedAssignments.forEach(member => {
                    member.isLeader = member.id === leaderId;
                });
            });

        // Party Type Selection
        $(document).off('change', '.party-type-select')
            .on('change', '.party-type-select', function () {
                const memberId = $(this).data('member-id');
                const partyTypeId = $(this).val();

                const member = state.selectedAssignments.find(m => m.id === memberId);
                if (member) {
                    member.partyTypeId = partyTypeId;
                    console.log('🏢 نوع الطرف للعضو', memberId, ':', partyTypeId);
                }
            });

        // NDA Checkbox
        $(document).off('change', '.nda-checkbox')
            .on('change', '.nda-checkbox', function () {
                const $checkbox = $(this);
                const memberId = $checkbox.data('member-id');
                const $cell = $checkbox.closest('td');

                const member = state.selectedAssignments.find(m => m.id === memberId);

                if (!member) return;

                if ($checkbox.is(':checked')) {
                    member.nda = state.ndaStatus.id;
                    $cell.attr('data-nda-id', state.ndaStatus.id);
                } else {
                    member.nda = null;
                    $cell.attr('data-nda-id', '');
                }
            });

        // Select All في جدول الأعضاء
        // Select All Members
        $(document).off('click', id('addAllMembers'))
            .on('click', id('addAllMembers'), function () {

                const $availableRows = $(`${id('userTable')} tbody tr:visible[data-id]`)
                    .filter(function () {
                        return !$(this).find('.team-add-btn').hasClass('selected');
                    });

                $availableRows.each(function () {
                    const $row = $(this);
                    const memberId = $row.data('id');
                    const member = state.members.find(m => m.id === memberId);

                    if (!member) return;

                    const userPartyTypes = member.userPartyTypes || [];
                    if (userPartyTypes.length === 0) return;

                    const autoSelectedPartyTypeId =
                        userPartyTypes.length === 1
                            ? userPartyTypes[0].partyType.id
                            : null;

                    state.selectedAssignments.push({
                        ...member,
                        scopes: (member.scopeIds || []).map(String),
                        nda: null,
                        partyTypeId: autoSelectedPartyTypeId,
                        isLeader: false
                    });

                    if (!state.teamLeaderId)
                        state.teamLeaderId = memberId;

                    $row.find('.team-add-btn')
                        .addClass('selected')
                        .html('');
                });

                renderSelectedTeamTable();
                updateSelectAllCheckbox();
            });
        
        $(document).off('click', `${id('userTable')} .team-add-btn`)
            .on('click', `${id('userTable')} .team-add-btn`, function () {

                const $btn = $(this);
                const row = $btn.closest('tr');
                const memberId = row.data('id');
                const member = state.members.find(m => m.id === memberId);

                if (!member) return;

                if (state.selectedAssignments.find(m => m.id === memberId)) {
                    return;
                }

                const userPartyTypes = member.userPartyTypes || [];

                if (userPartyTypes.length === 0) {
                    showError(`لا يمكن إضافة المستخدم لأنه لا يملك أي نوع طرف`);
                    return;
                }

                const autoSelectedPartyTypeId = userPartyTypes.length === 1
                    ? userPartyTypes[0].partyType.id
                    : null;

                const defaultScopes = (member.scopeIds || []).map(String);

                state.selectedAssignments.push({
                    ...member,
                    scopes: defaultScopes,
                    nda: null,
                    partyTypeId: autoSelectedPartyTypeId,
                    isLeader: false
                });

                if (!state.teamLeaderId) {
                    state.teamLeaderId = memberId;
                }

                $btn.addClass('selected');
                $btn.html('');

                const memberName = member.name || member.fullName || member.memberName;
                showSuccess('تم إضافة ' + memberName);

                renderSelectedTeamTable();
                updateSelectAllCheckbox();
            });

        //down table
        $(document).off('click', '.team-remove-btn')
            .on('click', '.team-remove-btn', function () {

                const $btn = $(this);
                const $row = $btn.closest('tr');
                const memberId = $row.data('selected-id');

                const member = state.selectedAssignments.find(m => m.id === memberId);

                state.selectedAssignments = state.selectedAssignments.filter(m => m.id !== memberId);
                state.pendingRemoval.delete(memberId);

                // Restore the + button in the top table
                $(`${id('userTable')} tr[data-id="${memberId}"] .team-add-btn`)
                    .removeClass('selected')
                    .html('<i class="las la-plus"></i>');

                const memberName = member?.name || member?.fullName || member?.memberName;
                showSuccess('تم حذف ' + memberName);

                renderSelectedTeamTable();
                updateSelectAllCheckbox();
            });
        // Select All memeber seleced
        $(document).off('click', id('removeAllMembers'))
            .on('click', id('removeAllMembers'), function () {

                state.selectedAssignments.forEach(member => {

                    $(`${id('userTable')} tr[data-id="${member.id}"] .team-add-btn`)
                        .removeClass('selected')
                        .html('<i class="las la-plus"></i>');
                });

                state.selectedAssignments = [];
                state.pendingRemoval.clear();
                state.teamLeaderId = null;

                renderSelectedTeamTable();
                renderMembersTable();

                showSuccess('تم حذف جميع الأعضاء');
            });

        // search
        $(document).off('keyup', id('customSearch'))
            .on('keyup', id('customSearch'), function () {
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
        $(document).off('click', id('deleteSelectedBtn'))
            .on('click', id('deleteSelectedBtn'), function (e) {
                e.preventDefault();

                if (state.pendingRemoval.size === 0) {
                    alert('الرجاء تحديد الأعضاء المراد حذفهم');
                    return;
                }

                if (!confirm(`هل أنت متأكد من حذف ${state.pendingRemoval.size} عضو؟`)) {
                    return;
                }

                state.pendingRemoval.forEach(memberId => {
                    state.selectedAssignments = state.selectedAssignments.filter(m => m.id !== memberId);
                    $(`${id('userTable')} tr[data-id="${memberId}"] .row-select`).prop('checked', false);
                });

                state.pendingRemoval.clear();

                renderSelectedTeamTable();
                renderMembersTable();
                showSuccess('تم حذف الأعضاء المحددين بنجاح');
            });
        $(document).off('click', '.send-mail-btn')
            .on('click', '.send-mail-btn', function () {
                const memberId = $(this).data('member-id');
                notificationUtil.confirmation({
                    title: sharedFn().GetUiControlText("WEB_WARNING_CONFIRM"),
                    okText: sharedFn().GetUiControlText("WEB_CONFIRM_BUTTON"),
                    cancelText: sharedFn().GetUiControlText('WEB_CANCEL')
                },
                    result => {

                        const response = jqClient().Post(API_ENDPOINTS.SEND_MAIL_NOTIFICATION,
                            { userId: memberId, evaluationRequestId: state.evaluationRequestId });

                        if (response.success || response.isSuccess) {
                            showSuccess(t('msgMailSentSuccess', 'تم إرسال البريد بنجاح'));
                        } else {
                            throw new Error(response.message || 'فشل في إرسال البريد');
                        }
                    });
            });
    }
    window.sendAssignmentEmail = async function (ministryUserId, evaluationRequestId, ministryUser) {

        const confirmMessage =
            uiControlsSetup().GetUiControlText("WEB_CONFIRM_SEND_EMAIL")
                .replace("{0}", ` <span style="font-weight:bold;color:maroon;">${ministryUser}</span> `);

        notificationUtil.confirmation({
            title: confirmMessage,
            okText: uiControlsSetup().GetUiControlText("WEB_CONFIRM_BUTTON"),
            cancelText: uiControlsSetup().GetUiControlText("WEB_CANCEL")
        }, async function (confirmed) {

            if (!confirmed) return;

            try {

                const response = await jqClient().Post(
                    API_ENDPOINTS.SEND_MAIL_NOTIFICATION,
                    {
                        userId: ministryUserId,
                        evaluationRequestId: evaluationRequestId
                    });

                if (response?.isSuccess || response?.success) {
                    notificationUtil.success(
                        uiControlsSetup().GetUiControlText("msgMailSentSuccess")
                    );
                } else {
                    notificationUtil.error(
                        response?.message ||
                        uiControlsSetup().GetUiControlText("msgMailSentFailed")
                    );
                }

            } catch (e) {
                console.error(e);

                notificationUtil.error(
                    sharedFn().GetUiControlText("msgMailSentFailed")
                );
            }
        });
    };
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

    ns.init = async function (fieldId, evaluationRequestId = null, elementId = null) {
        // ====== RESET STATE BEFORE EACH INIT ======
        // هذا يضمن أن كل مرة يتم استدعاء init يتم إعادة ضبط كل شيء
        resetStateInternal();

        // ====== SET NEW STATE ======
        state.fieldId = fieldId;
        state.evaluationRequestId = evaluationRequestId;

        state.$root = elementId
            ? (elementId instanceof jQuery ? elementId : $(elementId))
            : $(document);

        // ====== CLEAN UP OLD DOM ELEMENTS ======
        // إزالة أي Select2 instances قديمة
        if ($.fn.select2) {
            // تنظيف على مستوى المستند
            $(document).find('.multiCheckSelect-dynamic').each(function () {
                if ($(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2('destroy');
                }
            });

            // تنظيف داخل الـ root الجديد إن وجد
            if (state.$root && state.$root.length) {
                state.$root.find('.multiCheckSelect-dynamic').each(function () {
                    if ($(this).hasClass('select2-hidden-accessible')) {
                        $(this).select2('destroy');
                    }
                });
            }
        }

        // ====== REMOVE OLD EVENT LISTENERS ======
        $(document).off('change', '.team-leader-radio');
        $(document).off('change', '.party-type-select');
        $(document).off('change', '.nda-checkbox');
        $(document).off('change', id('selectAllMembers'));
        $(document).off('change', `${id('userTable')} .row-select`);
        $(document).off('change', '.selected-row-checkbox');
        $(document).off('change', id('selectAllSelected'));
        $(document).off('keyup', id('customSearch'));
        $(document).off('click', id('deleteSelectedBtn'));
        $(document).off('click', '.send-mail-btn');

        // ====== CHECK DOM ELEMENTS ======
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

        // ====== HIDE ALERTS ======
        $('.bg-success-light').hide();
        $(id('successAlert')).addClass('d-none');

        // ====== LOAD DATA ======
        const teamsLoaded = await loadTeams();

        updateSelectedTeamTableHeader();

        if (teamsLoaded) {
            await loadAllMembers();
        } else {
            showError('فشل تحميل البيانات الأساسية');
        }

        if (state.isNDA) {
            await loadPendingNDA();
        }

        await loadScopes();

        // Load existing team members if evaluationRequestId is provided
        if (evaluationRequestId) {
            await loadExistingAssignments(evaluationRequestId);
        }

        // ====== INIT EVENT LISTENERS ======
        initEventListeners();

        console.log('✅ Assignments initialized with fresh state for fieldId:', fieldId);
    };

    // ================== INTERNAL RESET FUNCTION ==================

    function resetStateInternal() {
        state.fieldId = null;
        state.evaluationRequestId = null;
        state.teams = [];
        state.members = [];
        state.allMembers = [];
        state.selectedAssignments = [];
        state.scopes = [];
        state.pendingRemoval = new Set();
        state.isNDA = false;
        state.teamLeaderId = null;
        state.ndaStatus = {};
        state.existingAssignments = [];
        state.$root = null;

        console.log('🔄 State reset for new initialization');
    }

    // ================== PUBLIC API ==================
    ns.getState = function () {
        return state;
    };

    ns.getSelectedAssignments = function () {
        return state.selectedAssignments;
    };
    // ================== PUBLIC RESET ==================

    ns.resetState = function () {
        resetStateInternal();

        // Clean up Select2 instances
        if ($.fn.select2) {
            $(document).find('.multiCheckSelect-dynamic').each(function () {
                if ($(this).hasClass('select2-hidden-accessible')) {
                    $(this).select2('destroy');
                }
            });
        }

        // Remove event listeners
        $(document).off('change', '.team-leader-radio');
        $(document).off('change', '.party-type-select');
        $(document).off('change', '.nda-checkbox');
        $(document).off('change', '#selectAllMembers');
        $(document).off('change', '#userTable .row-select');
        $(document).off('change', '.selected-row-checkbox');
        $(document).off('change', '#selectAllSelected');
        $(document).off('keyup', '#customSearch');
        $(document).off('click', '#deleteSelectedBtn');
        $(document).off('click', '.send-mail-btn');

        console.log('🗑️ Assignments state and listeners cleaned up');
    };
   
})(window.assignmentsLogic, jQuery);

