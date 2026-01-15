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
            openEvaluationRequestDetails(rowData.id); 
        }
    });

    function openEvaluationRequestDetails(requestId) {
        const options = {
            success: function (response) {

                formUtility.attachments = response.attachments || [];
                $('#evaluationRequestModalLabel').text(response.status || '');
                $('#evaluationRequestNoText').text(response.requestNumber || '');
                $('#evaluationRequestDetailsModal').modal('show');

                formUtility.renderPreviewView(
                    'formGroupsAccordion',
                    response.formGroups,
                    response.actions,
                    response.actionTransactions,
                    response.attachments,
                    {
                        requestId: requestId,                
                        serviceId: response.serviceId, 
                        actionsContainerId: 'actions-container',
                        templateContainerId: 'divTemplates',
                        modalContainerId: 'Action-container-fields',
                        ctx: { root: '#evaluationRequestDetailsModal' }
                    }
                );


                renderEvaluationPartiesSection(response);

                bindSchoolDetails(response);
            }
        };

        jqClient(options).Get(`/ServiceRequest/GetEvaluationDetails?requestId=${requestId}`);
    }

    function bindSchoolDetails(response) {
        const s = response && response.school ? response.school : null;
        if (!s) return;

        const $root = $("#school-details-container");

        const formatDate = (val) => {
            if (!val) return "";
            const d = new Date(val);
            if (isNaN(d.getTime())) return val;
            const dd = String(d.getDate()).padStart(2, "0");
            const mm = String(d.getMonth() + 1).padStart(2, "0");
            const yyyy = d.getFullYear();
            return `${dd}-${mm}-${yyyy}`;
        };

        const schoolName =
            (window.currentLang === "ar" ? s.nameAr : s.nameEn) ||
            s.nameAr ||
            s.nameEn ||
            "";

        $root.find("#schoolName").text(schoolName);

        $root.find("#managerName").text(s.managerQID || "");
        $root.find("#establishmentDate").text(formatDate(s.establishmentDate));
        $root.find("#teachers").text(s.teachersCount ?? s.teachers ?? "-");
        $root.find("#students").text(s.studentsCount ?? s.students ?? "-");

        const phone = s.phone || s.mobile || "";
        $root.find("#phone").text(phone).attr("href", phone ? `tel:${phone}` : "#");

        const email = s.orgEmail || s.manageEmail || "";
        $root.find("#email").text(email).attr("href", email ? `mailto:${email}` : "#");

        $root.find("#address").text(s.address || "");

        $root.find("#currentRating").text(s.rating ?? "-");
        $root.find("#currentRatingDate").text(
            s.lastEvaluationDate ? formatDate(s.lastEvaluationDate) : ""
        );

        $root.find("#previousRating").text(response.previousRating ?? "-");
        $root.find("#previousRatingDate").text(
            response.previousRatingDate ? formatDate(response.previousRatingDate) : ""
        );
    }



    function renderEvaluationPartiesSection(response) {
        const parties = response?.evaluationParties || [];

        if (!window.formUtility || typeof formUtility.renderEvaluationParties !== "function") {
            console.error("formUtility.renderEvaluationParties is not loaded.");
            return;
        }

        formUtility.renderEvaluationParties(parties, {
            containerId: "evaluationPartiesContainer",
            parentAccordionId: "evaluationRootAccordion",
            expandFirst: true
        });
    }

    evaluationRequestsListing.reload();
});



