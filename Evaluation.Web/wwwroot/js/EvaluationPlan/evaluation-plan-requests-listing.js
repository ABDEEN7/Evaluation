$(document).ready(function () {
    function getPlanRequestFilter() {
        return {
            RequestNo: $('#planRequestNoFilter').val(),
            PlanId: $('#planIdFilter').val(),
            SchoolId: $('#planRequestSchoolFilter').val(),
            StatusesList: $('#planRequestStatusFilter').val(),
            RequestDateFrom: $('#planRequestDateFrom').val(),
            RequestDateTo: $('#planRequestDateTo').val()
        };
    }

    const planRequestsListing = evaluationListing.createListing({
        tableId: 'planRequestTable',
        ajaxUrl: '/ServiceRequest/GetPlanRequests',
        getFilterInput: getPlanRequestFilter,
        filterFormId: 'plan-request-filter-form-id',
        filterBtnId: 'filterPlanRequestBtnId',
        clearFilterBtnId: 'clearFilterPlanRequestBtnId',
        tabLabelSelector: '#tabPlanRequestsAnchorTag',
        tabLabelKey: 'lblEvaluationPlanRequests',
        enableCardView: true,
        cardViewBtnId: 'cardViewPlanRequest',
        tableViewBtnId: 'tblViewPlanRequest',
        rowClass: 'plan-request-card',
        columns: [
            {
                data: "planName",
                title: uiControlsSetup().GetUiControlText("lblEvaluationPlan"),
                className: "header-left status",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            {
                data: "service",
                title: uiControlsSetup().GetUiControlText("lblRequestService"),
                className: "td-left name",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            {
                data: "status",
                title: uiControlsSetup().GetUiControlText("lblRequestStatus"),
                className: "header-right",
                render: function (data, type, row) {
                    const statusColor = row.statusColor || "#cccccc";
                    const textColor = "#000";// getContrastingTextColor(statusColor);
                    return `<span class="request-status m-0" style="background-color:${statusColor};color:${textColor};">${data || ""}</span>`;
                }
            },
           
            {
                data: "requestNumber",
                title: uiControlsSetup().GetUiControlText("lblRequestNo"),
                className: "td-right"
            },
            {
                data: "createOn",
                title: uiControlsSetup().GetUiControlText("lblRequestCreatedDate"),
                className: "td-left bg-grey"
            },
            {
                data: "createOnTime",
                title: uiControlsSetup().GetUiControlText("lblRequestCreatedTime"),
                className: "td-right bg-grey justify-content-end"
            }
        ],
        onRowClick: function (rowData) {
            openPlanRequestDetails(rowData.id);
        }
    });

    function openPlanRequestDetails(requestId) {
        const options = {
            success: function (response) {
                formUtility.attachments = response.attachments || [];
                formUtility.renderPreviewView('plan-request-details-container', response.statusGroup, response.formGroups, response.actions, response.actionTransactions, response.attachments);
                $('#planRequestModalLabel').text(response.status);
                $('#planRequestNoText').text(response.requestNumber);
                $('#planRequestModal').modal('show');
                $('#planRequestModal').off('shown.bs.modal.redraw').on('shown.bs.modal.redraw', function () {
                    Tabulator.findTable("#planRequestModal .tabulator").forEach(t => t.redraw(true));
                });
            }
        };
        jqClient(options).Get(`/EvaluationPlanRequest/GetDetails?requestId=${requestId}`);
    }

    $('#btnAddEvaluationPlanRequest').on('click', function () {
        $("#CreatePlanModal").modal("show");
        InitializeCreatePlanRequest();
    });

    planRequestsListing.reload();
});
