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
            className: "td-full mb-4",
            render: function(data, type, row) {
                const statusColor = row.statusColor || "#cccccc";

                return `
                <div class="request-info">
                    <div class="request-icon" style="background-color:${statusColor}; color:#000;">
                        <i class="las la-file-signature"></i>
                    </div>

                    <div class="request-text">
                        <div class="request-header">
                          <span class="request-header card-only">  ${uiControlsSetup().GetUiControlText("lblRequest")}: </span> ${data || ""}
                        </div>

                        <div class="request-status-text card-only-row" style="color:${statusColor};">
                            ${row.status || ""}
                        </div>
                    </div>
                </div>
            `;
            }
        },

        {
            data: "status",
            className: "status-column",
            render: function(data, type, row) {
                return `
                <span class="status-padding" style="color:${row.statusColor || "#666"};">
                    ${data || "_"}
                </span>
            `;
            }
        },

        {
            data: "requestNumber",
            className: "td-full",
            render: function(data) {
                return `
                <i class="las la-file-alt card-only-icon me-1"></i>
                <span class="card-only-label me-2">${uiControlsSetup().GetUiControlText("lblRequestNumber")}:</span>
                <span class="data-text">${data || "_"}</span>
            `;
            }
        },

        {
            data: "schoolsCount",
            className: "td-full",
            render: function(data) {
                return `
                <i class="las la-school card-only-icon me-1"></i>
                <span class="card-only-label me-2">${uiControlsSetup().GetUiControlText("lblSchoolsCount")}:</span>
                <span class="data-text">${data || 0}</span>
            `;
            }
        },

        {
            data: "planName",
            className: "td-full",
            render: function(data) {
                return `
                <i class="las la-school card-only-icon me-1"></i>
                <span class="card-only-label me-2">${uiControlsSetup().GetUiControlText("lblPlanName")}:</span>
                <span class="data-text plan-name-text">${data || "_"}</span>
            `;
            }
        },

        {
            data: null,
            className: "td-full mb-3",
            render: function(data, type, row) {
                return `
                <i class="las la-calendar-week card-only-icon me-1"></i>
                <span class="card-only-label me-1">${uiControlsSetup().GetUiControlText("lblPeriod")}:</span>
                <span>
                    <span class="period-label me-1">${uiControlsSetup().GetUiControlText("lblFrom")}</span>
                    <span class="data-text me-1">${moment(row.planDateFrom).format("DD/MM/YYYY")}</span>
                    <span class="period-label me-1">${uiControlsSetup().GetUiControlText("lblTo")}</span>
                    <span class="data-text me-1">${moment(row.planDateTo).format("DD/MM/YYYY")}</span>
                </span>
            `;
            }
        },

        {
            data: "createOn",
            title: uiControlsSetup().GetUiControlText("lblRequestCreatedDate"),
            className: "td-right border-top-card",
         
            render: function(data) {
                if (!data) return "_";
                return `
                <i class="las la-calendar card-only-icon color-primary me-1"></i>
                <span class="data-text me-1">${moment(data).format("DD-MM-YYYY")}</span>
            `;
            }
        },

        {
            data: "createOnTime",
            title: uiControlsSetup().GetUiControlText("lblRequestCreatedTime"),
            className: "td-left place-content-end border-top-card",
            render: function(data) {
                if (!data) return "_";

                return `
                <div class="d-flex justify-content-end place-content-end">
                    <i class="las la-clock card-only-icon color-primary me-1"></i>
                    <span class="data-text me-1">${moment(data).format("hh:mm A")}</span>
                </div>
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
    //planRequestsListing.reload();
    function waitForUiControls(callback, maxWait = 3000) {
        const interval = 50;
        let elapsed = 0;
        const timer = setInterval(function () {
            elapsed += interval;
            if (uiControlsSetup().AnyUiBackendLabel('lblRequest')) {
                clearInterval(timer);
                callback();
            } else if (elapsed >= maxWait) {
                clearInterval(timer);
                callback(); 
            }
        }, interval);
    }

    waitForUiControls(function () {
        planRequestsListing.reload();
    });
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
            .Get(`/ServiceRequest/${DepartmecntRouting}/CanCreateEvaluationPlanRequest`);
    }
});
