let plansDatatable = null;
const plansPageSize = 10; 

const getPlansFilterInput = () => {
    const searchText = $('#planSearchInput').val() || '';
    const statusId = $('#statusIdForPlansFilter').val() || null;
    const startDateFrom = $('#startDateFromForPlansFilter').val() || null;
    const startDateTo = $('#startDateToForPlansFilter').val() || null;

    return {
        SearchText: searchText,
        StatusId: statusId,
        StartDateFrom: startDateFrom,
        StartDateTo: startDateTo
    };
};

const initPlansTable = () => {
    const $tbl = $('#plansTable');

    if ($.fn.DataTable.isDataTable($tbl)) {
        plansDatatable = $tbl.DataTable();
        plansDatatable.page('first').draw(false);
        plansDatatable.ajax.reload(null, false);
        return;
    }

    plansDatatable = $tbl.DataTable({
        processing: true,
        serverSide: true,
        paging: true,
        pageLength: plansPageSize,
        lengthChange: false,
        searching: false,       
        autoWidth: false,
        deferRender: true,
        dom: 'ftipr',
        order: [],              
        language: {
            info: uiControlsSetup().GetUiControlText("lblShowingEntries"),
            paginate: {
                previous: `${uiControlsSetup().GetUiControlText("lblprevious")} <i class="fas fa-angle-left"></i>`,
                next: `${uiControlsSetup().GetUiControlText("lblnext")} <i class="fas fa-angle-right"></i>`
            }
        },

        ajax: function (dt, callback /*, settings */) {
            const filters = getPlansFilterInput();

            const size = dt.length || plansPageSize;
            const page = Math.floor((dt.start || 0) / size) + 1;

            const payload = {
                SearchText: filters.SearchText,
                StatusId: filters.StatusId,
                StartDateFrom: filters.StartDateFrom,
                StartDateTo: filters.StartDateTo,

                PageNumber: page,
                PageSize: size,
                Draw: dt.draw
            };

            const options = {
                success: function (resp) {
                    const total = resp.totalDataCount || 0;
                    const rows = Array.isArray(resp.data) ? resp.data : [];

                    callback({
                        draw: dt.draw,
                        recordsTotal: total,
                        recordsFiltered: total,
                        data: rows
                    });
                },
                error: function () {
                    callback({
                        draw: dt.draw,
                        recordsTotal: 0,
                        recordsFiltered: 0,
                        data: []
                    });
                    setTimeout(() => sharedUtility().RedirectUnauthorized?.(), 5000);
                }
            };

            jqClient(options).Post("/EvaluationPlan/GetPlans", payload);
        },

        columns: [
            {
                data: null,
                orderable: false,
                className: "text-center align-middle",
                width: "60px",
                title: uiControlsSetup().GetUiControlText("lblActions") || "الإجراءات",
                render: function () {
                    return `
                        <div class="dropdown">
                            <button class="btn btn-light btn-sm" type="button" data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="fa-solid fa-ellipsis-vertical"></i>
                            </button>
                            <ul class="dropdown-menu dropdown-menu-end">
                                <li><a class="dropdown-item plan-view" href="#">${uiControlsSetup().GetUiControlText("lblView") || "عرض"}</a></li>
                                <li><a class="dropdown-item plan-edit" href="#">${uiControlsSetup().GetUiControlText("lblEdit") || "تعديل"}</a></li>
                                <li><hr class="dropdown-divider"></li>
                                <li><a class="dropdown-item text-danger plan-delete" href="#">${uiControlsSetup().GetUiControlText("lblDelete") || "حذف"}</a></li>
                            </ul>
                        </div>`;
                }
            },
            {
                data: "endDate",
                title: uiControlsSetup().GetUiControlText("lblPlanEndDate") || "نهاية الخطة",
                className: "align-middle text-center",
                render: d => d || ""
            },
            {
                data: "startDate",
                title: uiControlsSetup().GetUiControlText("lblPlanStartDate") || "بداية الخطة",
                className: "align-middle text-center",
                render: d => d || ""
            },
            {
                data: "createdOn",
                title: uiControlsSetup().GetUiControlText("lblPlanCreatedOn") || "تاريخ الإنشاء",
                className: "align-middle text-center",
                render: d => d || ""
            },
            {
                data: "schoolsCount",
                title: uiControlsSetup().GetUiControlText("lblPlanSchoolsCount") || "عدد المدارس",
                className: "align-middle text-center",
                render: d => d ?? ""
            },
            {
                data: "statusName",
                title: uiControlsSetup().GetUiControlText("lblPlanStatus") || "حالة الخطة",
                className: "align-middle text-center",
                render: function (data /*, type, row */) {
                    return `<span class="badge plan-status-badge">${data || ""}</span>`;
                }
            },
            {
                data: "name",
                title: uiControlsSetup().GetUiControlText("lblPlanName") || "اسم الخطة",
                className: "align-middle text-start text-truncate",
                render: function (data, type, row) {
                    return `
                        <div class="fw-semibold text-truncate" title="${data || ""}">
                            ${data || ""}
                        </div>
                        <div class="text-muted small text-truncate">
                            ${row.description || ""}
                        </div>`;
                }
            }
        ],

        createdRow: function (row) {
            $(row).addClass('plans-row');
        },

        drawCallback: function () {
            const $tbody = $('#plansTable tbody');

            $tbody.off('click', 'tr').on('click', 'tr', function (event) {
                if ($(event.target).closest('.dropdown-menu, .dropdown-toggle').length) return;

                const rowData = plansDatatable.row(this).data();
                if (!rowData) return;

                openPlanDetails(rowData.id);
            });

            $tbody.off('click', '.plan-view')
                .on('click', '.plan-view', function (e) {
                    e.preventDefault();
                    const rowData = plansDatatable.row($(this).closest('tr')).data();
                    if (rowData) openPlanDetails(rowData.id);
                });

            $tbody.off('click', '.plan-edit')
                .on('click', '.plan-edit', function (e) {
                    e.preventDefault();
                    const rowData = plansDatatable.row($(this).closest('tr')).data();
                    if (rowData) editPlan(rowData.id);
                });

            $tbody.off('click', '.plan-delete')
                .on('click', '.plan-delete', function (e) {
                    e.preventDefault();
                    const rowData = plansDatatable.row($(this).closest('tr')).data();
                    if (rowData) deletePlan(rowData.id, rowData.name);
                });
        }
    });
};

function openPlanDetails(planId) {
    window.location.href = `/EvaluationPlan/Details?id=${planId}`;
}

function editPlan(planId) {
    window.location.href = `/EvaluationPlan/Edit?id=${planId}`;
}

function deletePlan(planId, planName) {
    notificationUtil.confirmation(
        {
            title: uiControlsSetup().GetUiControlText('lblConfirmDelete') || 'تأكيد الحذف',
            body: (uiControlsSetup().GetUiControlText('lblDeletePlanConfirm') || 'هل تريد حذف الخطة') + ` (${planName})؟`,
            okText: uiControlsSetup().GetUiControlText('lblOk'),
            cancelText: uiControlsSetup().GetUiControlText('lblCancel')
        },
        function () {
            let options = {
                success: function () {
                    plansDatatable.ajax.reload(null, false);
                }
            };
            jqClient(options).Post(`/EvaluationPlan/Delete?id=${planId}`, {});
        }
    );
}

$('#planSearchInput').on('keypress', function (e) {
    if (e.keyCode === 13) {         // Enter
        e.preventDefault();
        if (plansDatatable) plansDatatable.ajax.reload();
    }
});

$('#plansFilterApplyBtn').on('click', function () {
    if (plansDatatable) plansDatatable.ajax.reload();
});

$('#plansFilterClearBtn').on('click', function () {
    $('#planSearchInput').val('');
    $('#statusIdForPlansFilter').val(null).trigger('change');
    $('#startDateFromForPlansFilter').val('');
    $('#startDateToForPlansFilter').val('');
    if (plansDatatable) plansDatatable.ajax.reload();
});

$(document).ready(function () {
    initPlansTable();
});
