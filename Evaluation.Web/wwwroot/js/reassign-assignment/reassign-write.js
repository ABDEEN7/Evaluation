async function saveReassign() {
    const fromId = getFromUserId();
    const toId = getToUserId();

    if (!fromId || !toId) {
        showToast('يرجى تحديد المستخدم المصدر والهدف', 'warning');
        return;
    }

    const selectedIds = $('.row-check:checked').map(function () {
        return $(this).val();
    }).get();

    if (!selectedIds.length) {
        showToast('يرجى تحديد طلب واحد على الأقل', 'warning');
        return;
    }

    setLoading('btnSave', 'btnSaveText', 'saveSpinner', true);

    try {
        await jqClient().Post(API.save, {
            userId: fromId,
            toUserId: toId,
            evaluationRequestIds: selectedIds
        });

        showToast(`تمت إعادة تعيين ${selectedIds.length} طلب بنجاح ✓`, 'success');
        clearTable();
        await loadAssignments();

    } catch (e) {
        showToast('حدث خطأ أثناء الحفظ، يرجى المحاولة مجدداً', 'danger');
    } finally {
        setLoading('btnSave', 'btnSaveText', 'saveSpinner', false);
    }
}

// ══════════════════════════════════════════════════════
//  HELPERS
// ══════════════════════════════════════════════════════
const getFromUserId = () => $('#fromUser').val() || null;
const getToUserId = () => $('#toUser').val() || null;

function clearTable() {
    $('#assignmentsBody').empty();
    $('#tableCard').addClass('d-none');
    $('#selectedCount').text('0');
    $('#totalCount, #badgeCount').text('0');
    $('#checkAll').prop('checked', false).prop('indeterminate', false);
    $('#btnSave').prop('disabled', true);
}

function setLoading(btnId, textId, spinnerId, loading) {
    $(`#${btnId}`).prop('disabled', loading);
    $(`#${textId}`).toggleClass('d-none', loading);
    $(`#${spinnerId}`).toggleClass('d-none', !loading);
}

function showToast(msg, type = 'primary') {
    $('#liveToast').remove();

    const icons = {
        success: 'fas fa-check-circle',
        danger: 'fas fa-times-circle',
        warning: 'fas fa-exclamation-triangle',
        primary: 'fas fa-info-circle'
    };

    const $toast = $(`
                        <div id="liveToast"
                             class="toast align-items-center text-bg-${type} border-0
                                    position-fixed bottom-0 start-50 translate-middle-x mb-3"
                             role="alert" style="z-index:9999;min-width:300px">
                            <div class="d-flex">
                                <div class="toast-body d-flex align-items-center gap-2">
                                    <i class="${icons[type] ?? icons.primary}"></i> ${msg}
                                </div>
                                <button type="button" class="btn-close btn-close-white me-2 m-auto"
                                        data-bs-dismiss="toast"></button>
                            </div>
                        </div>`);

    $('body').append($toast);
    new bootstrap.Toast($toast[0], { delay: 3500 }).show();
}
