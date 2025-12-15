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
        rowClass: 'plan-request-card',

        columns: [
           
            {
                data: "Status",
                title: uiControlsSetup().GetUiControlText("lblRequestStatus"),
                className: "header-left status",
                render: function (data, type, row) {

                    if (row.StatusISOPen === false) {
                        return ` <span class="badge bg-success-light fw-semibold br-0">
                                    <i class="la la-check fs-14"></i>
                                    مكتمل
                                </span>`;
                    }
                    else
                    return `<span class="badge bg-danger-light fw-semibold br-0">
                            <i class="las la-times fs-14"></i>
                            غير مكتمل
                        </span>`;
                }
            }
            ,
            {
                data: "evaluationType",
                title: uiControlsSetup().GetUiControlText("lblEvaluationPlan"),
                className: "header-right",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            {
                data: "orgTreeName",
                title: uiControlsSetup().GetUiControlText("lblSchoolName"),
                className: "td-full",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            {
                data: "status",
                title: uiControlsSetup().GetUiControlText("lblRequestNo"),
                className: "td-full"
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
            },

            {
                data: null,
                title: uiControlsSetup().GetUiControlText("lblActions") || "",
                orderable: false,
                searchable: false,
                className: "td-full",
                render: function (data, type, row) {

                    return `
            <div class="d-flex justify-content-center gap-1">

                <button class="btn btn-outline-primary btn-sm"
                        title="تصفح عملية التقييم"
                        onclick="openEvaluation('${row.Id}')">
                    <i class="la la-arrow-left"></i>
                </button>

            </div>
        `;
                }
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
