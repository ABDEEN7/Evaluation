$(document).ready(function () {

    var DepartmentRouting = sharedUtility().extractDepartmentName();
    const API_ENDPOINTS = {
        GET_ACADEMIC_YEARS: `/AcademicYear/${DepartmentRouting}/GetAcademicYearByDepartment`,
        GET_Service_Status: `/ServiceRequest/${DepartmentRouting}/GetServiceStatus`
    };
   
    function getPlanRequestFilter() {
        return {
            RequestNo: $('#planRequestNoFilter').val(),
            //PlanId: $('#planIdFilter').val() || null,
            SchoolId: $('#planRequestSchoolFilter').val() || null,
            StatusesList: $('#planRequestStatusFilter').val() || null,
            RequestDateFrom: $('#planRequestDateFrom').val(),
            RequestDateTo: $('#planRequestDateTo').val()
        };
    }

    const planRequestsListing = evaluationListing.createListing({
        tableId: 'planRequestTable',
        ajaxUrl: `/ServiceRequest/${DepartmentRouting}/GetPlanRequests`,
        getFilterInput: getPlanRequestFilter,
        filterFormId: 'plan-request-filter-form-id',
        filterBtnId: 'filterPlanBtnId',
        clearFilterBtnId: 'clearFilterPlanRequestBtnId',
        tabLabelSelector: '#tabPlanRequestsAnchorTag',
        tabLabelKey: 'lblEvaluationPlanRequests',
        enableCardView: true,
        cardViewBtnId: 'cardViewPlanRequest',
        tableViewBtnId: 'tblViewPlanRequest',
        rowClass: 'plan-request-card',
        columns: [

            {
                data: "service",
                title: uiControlsSetup().GetUiControlText("lblRequestService"),
                className: "header-left ",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            //{
            //    data: "planName",
            //    title: uiControlsSetup().GetUiControlText("lblEvaluationPlan"),
            //    className: "header-left status",
            //    render: function (data) {
            //        return `<strong class="text-truncate-2">${data || ""}</strong>`;
            //    }
            //},
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
            }
        ],
        onRowClick: function (rowData) {
            openPlanRequestDetails(rowData.id);
        }
    });

    window.openPlanRequestDetails = function (requestId) {
        const options = {
            success: function (response) {

                window.formUtility = window.formUtility || {};
                formUtility.attachments = response.attachments || [];
                formUtility.renderPreviewView(
                    'plan-request-details-container',
                    response.formGroups,
                    response.actions,
                    response.actionTransactions,
                    response.attachments,
                    {
                        actionsContainerId: 'plan-actions-container',
                        templateContainerId: 'plan-divTemplates',
                        modalContainerId: 'plan-Action-container-fields',
                        requestId: requestId,
                        serviceId: response.serviceId,
                        ctx: { root: '#planRequestModal' }
                    }
                );

                formUtility.addQueryParameter('id', requestId)
                formUtility.addQueryParameter('serviceId', response.serviceId)

                $('#planRequestModalLabel').text(response.status || '');
                $('#planRequestNoText').text(response.requestNumber || '');

                $('#planRequestModal').modal('show');

                $('#planRequestModal')
                    .off('shown.bs.modal.redraw')
                    .on('shown.bs.modal.redraw', function () {
                        if (window.Tabulator?.findTable) {
                            Tabulator.findTable("#planRequestModal .tabulator")
                                .forEach(t => t.redraw(true));
                        }
                    });
            }
        };

        jqClient(options).Get(`/ServiceRequest/${DepartmentRouting}/GetApplicationDetails?requestId=${requestId}`);
    };


    $('#btnAddEvaluationPlanRequest').on('click', function () {

        const el = document.getElementById("CreateRequestModal");
        const modal = bootstrap.Modal.getOrCreateInstance(el);
        modal.show();

        window.InitializeCreatePlanRequest();

    });
    //Evaluation.Loaders.loadPlans('planIdFilter');
    $('#planRequestStatusFilter').select2({
        placeholder: "اختر الحالة",
        allowClear: true,
        width: '100%',
        multiple: true
    });
    Evaluation.Loaders.loadServiceStatus('planRequestStatusFilter');
    planRequestsListing.reload();
});
