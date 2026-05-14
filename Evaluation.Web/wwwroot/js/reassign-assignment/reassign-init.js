$(async function () {
    await fetchDropLists(null);
});

async function fetchDropLists(excludeUserId) {
    try {
        const url = excludeUserId
            ? `${API.dropList}?userId=${excludeUserId}`
            : API.dropList;

        const json = await jqClient().SyncGet(url);

        // ── handle all wrapper shapes: { data:{} } / { result:{} } / raw object
        const data = json.data ?? json.result ?? json;

        populateSelect('fromUser', data.FormUserList ?? data.formUserList, getFromUserId());
        populateSelect('toUser', data.ToUserList ?? data.toUserList, getToUserId());

        updateLoadBtn();
    } catch (e) {
        showToast('فشل تحميل قوائم المستخدمين', 'danger');
    }
}

function populateSelect(id, list, keepValue) {
    const $sel = $(`#${id}`);
    $sel.empty().append('<option value="">— اختر المستخدم —</option>');
    (list || []).forEach(item => {
        $sel.append(
            $('<option>', { value: item.id, text: item.name })
                .prop('selected', item.id === keepValue)
        );
    });
}

// ══════════════════════════════════════════════════════
//  EVENTS
// ══════════════════════════════════════════════════════
async function onFromUserChange() {
    clearTable();
    const fromId = getFromUserId();
    if (fromId) await fetchDropLists(fromId);
    updateLoadBtn();
}

function updateLoadBtn() {
    $('#btnLoad').prop('disabled', !(getFromUserId() && getToUserId()));
}

// ══════════════════════════════════════════════════════
//  LOAD ASSIGNMENTS
// ══════════════════════════════════════════════════════
async function loadAssignments() {
    const userId = getFromUserId();
    if (!userId) return;

    setLoading('btnLoad', 'btnLoadText', 'loadSpinner', true);
    clearTable();

    try {
        const json = await jqClient().SyncGet(`${API.load}?userId=${userId}`);
        const list = json.data ?? json.result ?? json;

        renderTable(Array.isArray(list) ? list : []);
        $('#tableCard').removeClass('d-none');
        $('#tableCard')[0].scrollIntoView({ behavior: 'smooth', block: 'start' });
    } catch (e) {
        showToast('فشل تحميل طلبات التقييم', 'danger');
    } finally {
        setLoading('btnLoad', 'btnLoadText', 'loadSpinner', false);
    }
}

// ══════════════════════════════════════════════════════
//  RENDER TABLE
// ══════════════════════════════════════════════════════
function renderTable(list) {
    const $tbody = $('#assignmentsBody');
    $('#totalCount, #badgeCount').text(list.length);

    if (!list.length) {
        $tbody.html(`
            <tr>
                <td colspan="5" class="text-center text-muted py-5">
                    <i class="fas fa-inbox fa-2x mb-2 d-block opacity-50"></i>
                    لا توجد طلبات تقييم معيّنة لهذا المستخدم
                </td>
            </tr>`);
        $('#btnSave').prop('disabled', true);
        return;
    }

    $tbody.html(list.map((item, i) => `
        <tr>
            <td class="text-center">
                <input type="checkbox" class="form-check-input row-check"
                       value="${item.evaluationRequestId}"
                       onchange="updateCount()">
            </td>
            <td>${i + 1}</td>
            <td><strong>${item.requestNumber ?? '—'}</strong></td>
            <td>${item.serviceNameAr ?? '—'}</td>
            <td>${item.serviceNameEn ?? '—'}</td>
        </tr>
    `).join(''));

    updateCount();
}

// ══════════════════════════════════════════════════════
//  CHECKBOX LOGIC
// ══════════════════════════════════════════════════════
function toggleAll(master) {
    $('.row-check').prop('checked', master.checked);
    updateCount();
}
function selectAll() {
    $('.row-check, #checkAll').prop('checked', true);
    updateCount();
}
function clearAll() {
    $('.row-check, #checkAll').prop('checked', false);
    updateCount();
}
function updateCount() {
    const checked = $('.row-check:checked').length;
    const total = $('.row-check').length;
    $('#selectedCount').text(checked);
    $('#btnSave').prop('disabled', checked === 0 || !getToUserId());

    const master = document.getElementById('checkAll');
    master.indeterminate = checked > 0 && checked < total;
    master.checked = total > 0 && checked === total;
}