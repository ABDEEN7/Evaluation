$(document).ready(function () {

    function getEvaluationRequestFilter() {
        return {
            RequestNo: $('#evaluationRequestNoFilter').val(),
            PlanId: $('#evaluationPlanIdFilter').val(),
            OrgTreeId: $('#evaluationOrgTreeFilter').val(),
            StatusesList: $('#evaluationRequestStatusFilter').val(),
            RequestDateFrom: $('#evaluationRequestDateFrom').val(),
            RequestDateTo: $('#evaluationRequestDateTo').val()
        };
    }

    const evaluationRequestsListing = evaluationListing.createListing({
        tableId: 'evaluationRequestTable',
        ajaxUrl: '/ServiceRequest/GetEvaluationRequests',
        getFilterInput: getEvaluationRequestFilter,

        filterFormId: 'evaluation-request-filter-form-id',
        filterBtnId: 'filterEvaluationRequestBtnId',
        clearFilterBtnId: 'clearFilterEvaluationRequestBtnId',

        enableCardView: true,
        cardViewBtnId: 'cardViewEvaluationRequest',
        tableViewBtnId: 'tblViewEvaluationRequest',
        rowClass: 'evaluation-request-card',

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
                data: "status",
                title: uiControlsSetup().GetUiControlText("lblRequestStatus"),
                className: "header-right",
                render: function (data, type, row) {
                    const statusColor = row.StatusColor || "#cccccc";
                    const textColor = "#000";// getContrastingTextColor(statusColor);
                    return `<span class="request-status m-0" style="background-color:${statusColor};color:${textColor};">${data || ""}</span>`;
                }
            },
            {
                data: "orgTreeName",
                title: uiControlsSetup().GetUiControlText("lblSchoolName"),
                className: "td-left name",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
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
            openEvaluationRequestDetails(rowData.Id); 
        }
    });

    function openEvaluationRequestDetails(requestId) {
        const options = {
            success: function (response) {
                formUtility.attachments = response.attachments || [];
                formUtility.renderPreviewView(
                    'evaluation-request-details-container',
                    response.statusGroup,
                    response.formGroups,
                    response.actions,
                    response.actionTransactions,
                    response.attachments
                );

                $('#evaluationRequestModalLabel').text(response.status || '');
                $('#evaluationRequestNoText').text(response.requestNumber || '');
                $('#evaluationRequestModal').modal('show');
            }
        };

        jqClient(options).Get(`/EvaluationPlanRequest/GetEvaluationDetails?requestId=${requestId}`);
    }

    evaluationRequestsListing.reload();
});
