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
                className: "td-left py-1 td-70",
                 render: function(data, type, row) {
           
                return `
                    <div class="plan-title-row mb-3"> 
                        <i class="las la-file-signature card-only-icon title-icon"></i>
                        <span class="plan-text-wrap px-2">
                    <span class="card-only-label title-label">${uiControlsSetup().GetUiControlText("lblRequest")}: </span>
                    <span class="plan-title-text">${data || ""}</span>
                    </span>
                    </div>
                    `;
                }
            },
            {
        data: "status",
        title: uiControlsSetup().GetUiControlText("lblRequestStatus"),
        className: "td-right py-1 place-content-end td-30",
        render: function(data, type, row) {
            const statusColor = row.statusColor || "#cccccc";
            const textColor = "#000";
            return `
            <div class="d-flex justify-content-end w-100 mb-3">
                <span class="request-status py-1 px-3"
                      style="background-color:${statusColor};color:${textColor};">
                    ${data || ""}
                </span>
            </div>
        `;
        }
    },
            {
        data: "requestNumber",
        className: "td-left small-width",
        render: function(data) {
            return `
            <i class="las la-file-alt card-only-icon me-1"></i>
            <span class="card-only-label me-2">${uiControlsSetup().GetUiControlText("lblRequestNumber")}:</span>
            <span class="data-text">${data || "_"}</span>
        `;
        }
    },
    {
        data: "planName",
        className: "td-right small-width",
        render: function(data) {
            return `
            <i class="las la-school card-only-icon me-1"></i>
            <span class="card-only-label me-2">${uiControlsSetup().GetUiControlText("lblPlanName")}:</span>
            <span class="data-text">${data || 0}</span>
        `;
        }
    },
    {
        data: "schoolsCount",
        className: "td-right small-width",
        render: function(data) {
            return `
            <i class="las la-school card-only-icon me-1"></i>
            <span class="card-only-label me-2">${uiControlsSetup().GetUiControlText("lblSchoolsCount")}:</span>
            <span class="data-text">${data || 0}</span>
        `;
        }
    },
    {
        data: "createOn",
        title: uiControlsSetup().GetUiControlText("lblRequestCreatedDate"),
        className: "td-left small-width",
        render: function(data) {
            if (!data) return "_";
            return `
            <i class="las la-calendar card-only-icon"></i>
            <span class="card-only-label mx-1">${uiControlsSetup().GetUiControlText("evalRequestlblcreatedOn")}:</span>
            <span class="data-text">${moment(data).format("DD-MM-YYYY")}</span>
        `;
        }
    },
    {
        data: "createOnTime",
        title: uiControlsSetup().GetUiControlText("lblRequestCreatedTime"),
        className: "td-right small-width",
        render: function(data) {
            if (!data) return "_";

            return `
            <div class="d-flex justify-content-end align-items-center">
                <i class="las la-clock card-only-icon"></i>
                <span class="card-only-label mx-1">${uiControlsSetup().GetUiControlText("evalRequestlblCreatedAt")}:</span>
                <span class="data-text">${moment(data).format("hh:mm A")}</span>
            </div>
        `;
        }
    },
     {
        data: null,
        className: "td-full small-width",
        render: function(data, type, row) {
            return `
            <i class="las la-calendar-week card-only-icon me-1"></i>
            <span class="card-only-label me-1">${uiControlsSetup().GetUiControlText("lblPeriod")}:</span>
            <spandir="rtl" class="data-text">
                ${uiControlsSetup().GetUiControlText("lblFrom")} ${moment(row.planDateFrom).format("DD/MM/YYYY")}
                            ${uiControlsSetup().GetUiControlText("lblTo")} ${moment(row.planDateTo).format("DD/MM/YYYY")}
            </span>
        `;
        }
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
    $('#filterPlanRequestBtnsId').on('click', function () {
        planRequestsListing.reload();
    });

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
    //Evaluation.Loaders.loadServiceStatus('planRequestStatusFilter');
    planRequestsListing.reload();

    async function toggleAddEvaluationPlanRequestButton() {

        $('#btnAddEvaluationPlanRequest').hide();

        const options = {
            success: function (response) {

                if (response?.canCreate === true) {
                    $('#btnAddEvaluationPlanRequest').show();
                }
                else {
                    $('#btnAddEvaluationPlanRequest').hide();
                }
            }
        };

        jqClient(options)
            .Get(`/ServiceRequest/${DepartmentRouting}/CanCreateEvaluationPlanRequest`);
    }
});
