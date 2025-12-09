$(document).ready(function () {
    function getOperationsFilter() {
        return {
            OperationNo: $('#operationNoFilter').val(),
            PlanId: $('#operationPlanFilter').val(),
            SchoolId: $('#operationSchoolFilter').val(),
            StatusId: $('#operationStatusFilter').val(),
            OperationDateFrom: $('#operationDateFrom').val(),
            OperationDateTo: $('#operationDateTo').val()
        };
    }

    const operationsListing = evaluationListing.createListing({
        tableId: 'operationTable',
        ajaxUrl: '/EvaluationOperation/GetOperations',
        getFilterInput: getOperationsFilter,
        filterFormId: 'operation-filter-form-id',
        filterBtnId: 'filterOperationBtnId',
        clearFilterBtnId: 'clearFilterOperationBtnId',
        tabLabelSelector: '#tabOperationsAnchorTag',
        tabLabelKey: 'lblEvaluationOperations',
        enableCardView: false,
        cardViewBtnId: 'cardViewOperation',
        tableViewBtnId: 'tblViewOperation',
        rowClass: 'operation-row',
        columns: [
            {
                data: "operationNo",
                title: uiControlsSetup().GetUiControlText("lblOperationNo"),
                className: "td-left"
            },
            {
                data: "planName",
                title: uiControlsSetup().GetUiControlText("lblEvaluationPlan"),
                className: "td-left"
            },
            {
                data: "schoolName",
                title: uiControlsSetup().GetUiControlText("lblSchoolName"),
                className: "td-left name",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            {
                data: "operationDate",
                title: uiControlsSetup().GetUiControlText("lblOperationDate"),
                className: "td-left bg-grey"
            },
            {
                data: "status",
                title: uiControlsSetup().GetUiControlText("lblOperationStatus"),
                className: "td-right",
                render: function (data, type, row) {
                    const color = row.statusColor || "#cccccc";
                    const textColor = getContrastingTextColor(color);
                    return `<span class="request-status m-0" style="background-color:${color};color:${textColor};">${data || ""}</span>`;
                }
            }
        ],
        onRowClick: function (rowData) {
            openOperationDetails(rowData.id);
        }
    });

    function openOperationDetails(operationId) {
        const options = {
            success: function (response) {
                $('#operationDetailsModalLabel').text(response.operationNo);
                $('#operationDetailsModalBody').html(response.htmlContent || '');
                $('#operationDetailsModal').modal('show');
            }
        };
        jqClient(options).Get(`/EvaluationOperation/Details?operationId=${operationId}`);
    }

    operationsListing.reload();
});
