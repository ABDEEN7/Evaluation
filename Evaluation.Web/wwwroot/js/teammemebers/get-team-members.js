// API Endpoints

$(document).ready(function () {
    // Variables to store API data
    let teams = [];
    let members = [];
    let allMembers = [];
    let selectedTeamMembers = [];

    const TeamState = {
        scopes: []
    }

    // Load teams from API
    async function loadTeams() {
        try {
            const response = await TeamApi.getTeams();

            if (response && response.value) {
                teams = response.value;
                console.log('✅ Teams loaded:', teams.length, 'teams');
                console.log('Teams:', teams);
                initTeamDropdown();
                return true;
            } else {
                console.error('❌ No teams data received');
                showErrorAlert('لم يتم العثور على فرق');
                return false;
            }
        } catch (error) {
            console.error('❌ Error loading teams:', error);
            showErrorAlert('حدث خطأ في تحميل الفرق');
            return false;
        }
    }

    // Load members by team ID from API
    async function loadMembersByTeam(teamId) {

        showMembersLoading();
        try {
            const response = await TeamApi.getMembersByTeam(teamId);

            if (response && response.value) {
                members = response.value;

                // Add to allMembers if not exists
                members.forEach(member => {
                    if (!allMembers.find(m => m.id === member.id)) {
                        allMembers.push(member);
                    }
                });

                renderMembersTable();
                return true;
            } else {
                $('#userTable tbody').html(`
                    <tr>
                        <td colspan="3" class="text-center text-muted py-4">لا يوجد أعضاء في هذا الفريق</td>
                    </tr>
                `);
                return false;
            }
        } catch (error) {
            showErrorAlert('حدث خطأ في تحميل الأعضاء');
            $('#userTable tbody').html(`
                <tr>
                    <td colspan="3" class="text-center text-danger py-4">حدث خطأ في تحميل البيانات</td>
                </tr>
            `);
            return false;
        }
    }
    //Load Scope From API
    async function LoadScopes() {
        try {
            const res = await TeamApi.getScopes();
            TeamState.scopes = res?.value ?? [];

        } catch {
            showErrorAlert('خطأ في تحميل المجالات');
        }
    }

    // Load all members (for "all teams" option)
    async function loadAllMembers() {
        try {
            showMembersLoading();
            if (teams.length === 0) {
                $('#userTable tbody').html(`
                    <tr>
                        <td colspan="3" class="text-center text-muted py-4">لا توجد فرق متاحة</td>
                    </tr>
                `);
                return false;
            }

            // Load members for each team
            const promises = await TeamApi.getMembersByTeam(null);
            const responses = promises.value;
            allMembers = [];

            responses.forEach(member => {
                if (!allMembers.find(m => m.id === member.id)) {
                    allMembers.push(member);
                }
            });
            members = allMembers;
            console.log('✅ All members loaded:', allMembers.length, 'total members');
            renderMembersTable();
            return true;
        } catch (error) {
            console.error('❌ Error loading all members:', error);
            showErrorAlert('حدث خطأ في تحميل الأعضاء');
            $('#userTable tbody').html(`
                <tr>
                    <td colspan="3" class="text-center text-danger py-4">حدث خطأ في تحميل البيانات</td>
                </tr>
            `);
            return false;
        }
    }

    // Initialize team dropdown
    function initTeamDropdown() {
        if ($('#teamFilter').length > 0) {
            console.log('ℹ️ Team dropdown already exists');
            return;
        }

        if (teams.length === 0) {
            console.warn('⚠️ No teams to display in dropdown');
            return;
        }

        const dropdown = `
            <div class="col-xl-4 col-lg-4 col-md-4 mb-3">
                <select id="teamFilter" class="form-select form-select-lg">
                    <option value="">جميع الفرق</option>
                    ${teams.map(team => `<option value="${team.id}">${team.name}</option>`).join('')}
                </select>
            </div>
        `;

        const parentRow = $('.col-xl-8.col-lg-8.col-md-8.mb-3').first().parent();
        if (parentRow.length) {
            parentRow.prepend(dropdown);
            console.log('✅ Team dropdown added successfully with', teams.length, 'teams');
        } else {
            console.error('❌ Parent row not found for team dropdown');
        }
    }

    // Render members table
    function renderMembersTable() {
        const { scopes } = TeamState;
        if (!members || members.length === 0) {
            $('#userTable tbody').html(`
                <tr>
                    <td colspan="3" class="text-center text-muted py-4">لا يوجد أعضاء</td>
                </tr>
            `);
            updateSelectAllCheckbox();
            return;
        }

        const tbody = members.map(member => {
            const isSelected = selectedTeamMembers.find(m => m.id === member.id);
            const memberName = member.name || member.fullName || member.memberName || 'غير محدد';
            const memberPosition = member.position || member.jobTitle || member.title || 'غير محدد';

            return `
                <tr data-member-id="${member.id}">
                    <td>
                        <label class="custom-checkbox1 plus">
                            <input type="checkbox" class="row-select" ${isSelected ? 'checked' : ''}>
                            <span class="checkmark"></span>
                        </label>
                    </td>
                    <td><h6>${memberName}</h6></td>
                    <td><h6>${memberPosition}</h6></td>
                </tr>
            `;
        }).join('');

        $('#userTable tbody').html(tbody);
        updateSelectAllCheckbox();
        console.log('✅ Members table rendered:', members.length, 'members');
    }

    // Render selected team table
    function renderSelectedTeamTable() {
        const selectedTable = $('.card-table').eq(1).find('tbody');

        if (!selectedTable.length) {
            console.error('❌ Selected team table not found!');
            return;
        }

        if (selectedTeamMembers.length === 0) {
            selectedTable.html(`
                <tr>
                    <td colspan="5" class="text-center text-muted py-4">لا يوجد أعضاء محددين</td>
                </tr>
            `);
            console.log('ℹ️ Selected team table cleared');
            return;
        }

        const tbody = selectedTeamMembers.map((member, index) => {
            const { scopes } = TeamState;
            const memberName = member.name || member.fullName || member.memberName || 'غير محدد';
            const memberPosition = member.position || member.jobTitle || member.title || 'غير محدد';

            return `
                <tr data-selected-id="${member.id}">
                    <td>
                        <label class="custom-checkbox1 minus">
                            <input type="checkbox" class="selected-row-checkbox">
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
                                ${scopes.map(scope => `
                                    <option value="${scope.id}">${scope.name}</option>
                                `).join('')}
                            </select>
                        </div>
                    </td>
                    <td>
                        <div class="form-check custom-radio">
                            <input class="form-check-input team-leader-radio" type="radio" 
                                   name="example" value="${member.id}" 
                                   ${index === 0 ? 'checked' : ''}>
                        </div>
                    </td>
                </tr>
            `;
        }).join('');

        selectedTable.html(tbody);
        console.log('✅ Selected team table rendered:', selectedTeamMembers.length, 'members');

        setTimeout(() => {
            initializeSelect2();
        }, 100);

        updateSelectedCheckboxHeader();
    }

    // Initialize Select2 for scopeselection
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
            const member = selectedTeamMembers.find(m => m.id === memberId);
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

    // Update "Select All" checkbox state
    function updateSelectAllCheckbox() {
        const totalCheckboxes = $('#userTable .row-select').length;
        const checkedCheckboxes = $('#userTable .row-select:checked').length;

        const selectAllCheckbox = $('#selectAll');
        if (selectAllCheckbox.length) {
            selectAllCheckbox.prop('checked', totalCheckboxes > 0 && totalCheckboxes === checkedCheckboxes);
        }
    }

    // Update selected team checkbox header
    function updateSelectedCheckboxHeader() {
        const totalCheckboxes = $('.selected-row-checkbox').length;
        const checkedCheckboxes = $('.selected-row-checkbox:checked').length;

        $('.card-table').eq(1).find('thead .custom-checkbox1.minus input[type="checkbox"]')
            .prop('checked', totalCheckboxes > 0 && totalCheckboxes === checkedCheckboxes);
    }

    // Handle member selection (add to team)
    $(document).on('change', '#userTable .row-select', function () {
        const row = $(this).closest('tr');
        const memberId = row.data('member-id');
        const member = members.find(m => m.id === memberId);

        if (!member) {
            console.error('❌ Member not found:', memberId);
            return;
        }

        const memberName = member.name || member.fullName || member.memberName || 'غير محدد';

        if (this.checked) {
            if (!selectedTeamMembers.find(m => m.id === memberId)) {
                selectedTeamMembers.push({
                    ...member,
                    scopes: [],
                    nda: null
                });
                console.log('✅ Added member:', memberName);
                showSuccessAlert('تم إضافة ' + memberName + ' بنجاح');
            }
        } else {
            selectedTeamMembers = selectedTeamMembers.filter(m => m.id !== memberId);
            console.log('❌ Removed member:', memberName);
        }

        console.log('📊 Total selected members:', selectedTeamMembers.length);
        renderSelectedTeamTable();
        updateSelectAllCheckbox();
    });

    // Handle "Select All" in members table
    $(document).on('change', '#selectAll', function () {
        const isChecked = this.checked;
        console.log('🔄 Select All clicked:', isChecked);

        $('#userTable .row-select').each(function () {
            if ($(this).prop('checked') !== isChecked) {
                $(this).prop('checked', isChecked).trigger('change');
            }
        });
    });

    // Handle removal from selected team
    $(document).on('change', '.selected-row-checkbox', function () {
        const row = $(this).closest('tr');

        if (this.checked) {
            row.addClass('marked-for-removal');
            row.css('background-color', '#ffebee');
        } else {
            row.removeClass('marked-for-removal');
            row.css('background-color', '');
        }

        updateSelectedCheckboxHeader();
    });

    // Handle "Select All" in selected team table
    $(document).on('change', '.card-table:eq(1) thead .custom-checkbox1.minus input[type="checkbox"]', function () {
        const isChecked = this.checked;
        $('.selected-row-checkbox').prop('checked', isChecked).trigger('change');
    });

    // Remove selected members from team
    $(document).on('click', '.btn-outline-danger', function (e) {
        e.preventDefault();
        const markedRows = $('.marked-for-removal');

        if (markedRows.length === 0) {
            alert('الرجاء تحديد الأعضاء المراد حذفهم');
            return;
        }

        if (!confirm('هل أنت متأكد من حذف ' + markedRows.length + ' عضو؟')) {
            return;
        }

        markedRows.each(function () {
            const memberId = $(this).data('selected-id');
            selectedTeamMembers = selectedTeamMembers.filter(m => m.id !== memberId);
        });

        renderSelectedTeamTable();
        renderMembersTable();
        showSuccessAlert('تم حذف الأعضاء المحددين بنجاح');
    });

    // Team filter change
    $(document).on('change', '#teamFilter', async function () {
        const teamId = $(this).val();

        if (teamId) {
            const team = teams.find(t => t.id === teamId);
            console.log('🔄 Team filter changed to:', team?.name);
            await loadMembersByTeam(teamId);
        } else {
            console.log('🔄 Team filter changed to: All Teams');
            await loadAllMembers();
        }
    });

    // Search functionality
    $(document).on('keyup', '#customSearch', function () {
        const searchTerm = $(this).val().toLowerCase();

        $('#userTable tbody tr').each(function () {
            const name = $(this).find('td:eq(1)').text().toLowerCase();
            const position = $(this).find('td:eq(2)').text().toLowerCase();

            if (name.includes(searchTerm) || position.includes(searchTerm)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    // Save team button
    $(document).on('click', '.btn-primary', function (e) {
        const buttonText = $(this).text().trim();

        if (buttonText.includes('حفظ الفريق') || buttonText.includes('حفظ')) {
            e.preventDefault();

            if (selectedTeamMembers.length === 0) {
                alert('الرجاء تحديد أعضاء الفريق');
                return;
            }

            const leaderId = $('.team-leader-radio:checked').val();
            const leader = selectedTeamMembers.find(m => m.id === leaderId);
            const leaderName = leader?.name || leader?.fullName || leader?.memberName;

            const teamData = {
                members: selectedTeamMembers.map(m => ({
                    id: m.id,
                    name: m.name || m.fullName || m.memberName,
                    position: m.position || m.jobTitle || m.title,
                    scopes: m.scopes || [],
                    isLeader: m.id === leaderId
                })),
                leaderId: leaderId,
                leaderName: leaderName,
                totalMembers: selectedTeamMembers.length,
                timestamp: new Date().toISOString()
            };

            console.log('========== SAVING TEAM ==========');
            console.log('Team Leader:', leaderName);
            console.log('Total Members:', selectedTeamMembers.length);
            console.log('Team Data:', teamData);
            console.log('=================================');

            // TODO: Add your save API call here
            // Example:
            // jqClient().Post(`${API_ENDPOINTS.SAVE_TEAM}`, teamData)
            //     .then(response => {
            //         showSuccessAlert('تم حفظ الفريق بنجاح');
            //     })
            //     .catch(error => {
            //         showErrorAlert('حدث خطأ في حفظ الفريق');
            //     });

            showSuccessAlert('تم حفظ الفريق بنجاح (' + selectedTeamMembers.length + ' أعضاء)');
        }
    });

    // Show success alert
    function showSuccessAlert(message = 'تم العملية بنجاح') {
        const alert = $('.bg-success-light');

        if (alert.length) {
            alert.find('p').text(message);
            alert.show();

            setTimeout(() => {
                alert.fadeOut();
            }, 3000);
        }
    }

    // Show error alert
    function showErrorAlert(message = 'حدث خطأ') {
        alert(message);
        console.error('❌', message);
    }

    // Initialize system
    async function initializeSystem() {
        console.log('========================================');
        console.log('🚀 Initializing Team Management System');
        console.log('========================================');

        $('.bg-success-light').hide();

        // Load teams first
        const teamsLoaded = await loadTeams();

        if (teamsLoaded) {
            // Load all members by default
            await loadAllMembers();
        } else {
            console.error('❌ Failed to initialize: No teams loaded');
            showErrorAlert('فشل تحميل البيانات الأساسية');
        }
        await LoadScopes();
        console.log('========================================');
    }
    const TeamApi = {
        getTeams() {
            return jqClient().Get(API_ENDPOINTS.GET_TEAMS);
        },

        getMembersByTeam(teamId) {
            return jqClient().Get(`${API_ENDPOINTS.GET_MEMBERS_BY_TEAM}?teamId=${teamId}`);
        },

        getScopes() {
            return jqClient().Get(API_ENDPOINTS.GET_SCOPES);
        }
    };
    function showMembersLoading() {
        $('#userTable tbody').html(`
                <tr>
                    <td colspan="3" class="text-center text-muted py-4">
                        <i class="la la-spinner la-spin"></i> جاري التحميل...
                    </td>
                </tr>
            `);
    }

    // Start initialization
    initializeSystem();
});