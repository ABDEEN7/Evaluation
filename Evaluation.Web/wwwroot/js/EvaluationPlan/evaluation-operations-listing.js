$(document).ready(function () {
    var DepartmentRouting = sharedUtility().extractDepartmentName();
    const API_ENDPOINTS = {
        GET_Service_Status: `/ServiceRequest/${DepartmentRouting}/GetServiceStatus`,
        GET_Plans: `/Plan/${DepartmentRouting}/GetPlansDDL`,
        GET_Schools: `Schools/${DepartmentRouting}/GetSchoolsDDL`
    };

    function getUiText(key, fallback = '') {
        try {
            const text = uiControlsSetup().GetUiControlText(key);

            if (
                text === null ||
                text === undefined ||
                text === '' ||
                (typeof text === 'string' && text.startsWith('Missing ['))
            ) {
                return fallback;
            }

            return text;
        } catch {
            return fallback;
        }
    }
    function loadSchools() {
        const $scl = $('#evaluationOrgTreeFilter');
        $scl
            .empty()
            .append('<option value="">الكل</option>')
            .prop('disabled', true);

        jqClient()
            .Get(API_ENDPOINTS.GET_Schools)
            .done(function (response) {
                $scl.prop('disabled', false);
                if (response) {
                    response.forEach(function (school) {
                        $scl.append(`
                        <option value="${school.id}">${school.name}</option>`
                        );
                    });
                }
            }).fail(function (jqXHR, textStatus) {
                console.error('Failed to load academic years:', textStatus);
                $scl.prop('disabled', false);
            });
    }
   

    function getEvaluationRequestFilter() {
        return {
            RequestNo: $('#evaluationRequestNoFilter').val(),
            PlanId: $('#evaluationPlanIdFilter').val(),
            OrgTreeId: $('#evaluationOrgTreeFilter').val(),
            StatusesList: $('#evaluationRequestStatusFilter').val() || null,
            RequestDateFrom: $('#evaluationRequestDateFrom').val() || null,
            RequestDateTo: $('#evaluationRequestDateTo').val() || null
        };
    }

    const evaluationRequestsListing = evaluationListing.createListing({
        tableId: 'evaluationRequestTable',
        ajaxUrl: `/ServiceRequest/${DepartmentRouting}/GetEvaluationRequests`,
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
                        return ` <span class="badge bg-success-light ms-auto me-2 fw-semibold br-0">
                                    <i class="la la-check fs-14"></i>
                                    مكتمل
                                </span>`;
                    }
                    else
                        return `<span class="badge bg-danger-light ms-auto me-2 fw-semibold br-0">
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
                        root: '#evaluationRequestDetailsModal'
                    }
                );
                formUtility.addQueryParameter('Evlid', requestId)
                //formUtility.addQueryParameter('serviceId', response.serviceId)
                if (response.isNdaApprovalPending) {
                    const container =
                        document.getElementById('evaluationMainContainer') ||
                        document.querySelector('#content-container');

                    container.insertAdjacentHTML('afterbegin', generateNdaApprovalDiv());
                    NdaSubmit(response);
                }
                else {
                    renderEvaluationPartiesSection(response);
                }

                bindSchoolDetails(response);
            }
        };

        jqClient(options).Get(`/ServiceRequest/${DepartmentRouting}/GetEvaluationDetails?requestId=${requestId}`);
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

    //    function generateNdaApprovalDiv() {

    //        const title = getUiText(
    //            'lblNdaConflictTitle',
    //            'هل لديك تضارب مصالح مع هذه المدرسة؟'
    //        );

    //        const yesText = getUiText('lblYes', 'نعم');
    //        const noText = getUiText('lblNo', 'لا');

    //        const reasonLabel = getUiText(
    //            'lblNdaConflictReason',
    //            'أوضح سبب التضارب'
    //        );

    //        const reasonPlaceholder = getUiText(
    //            'lblNdaConflictReasonPlaceholder',
    //            'اكتب سبب تضارب المصالح هنا...'
    //        );

    //        const submitText = getUiText(
    //            'lblSubmit',
    //            'إرسال'
    //        );

    //        return `
    //<div id="ndaApprovalWrapper" class="card mt-3">
    //  <div class="card-body">

    //    <h5 class="text-center mb-4 fw-bold">
    //        ${title}
    //    </h5>

    //    <div class="d-flex justify-content-center gap-4 mb-3">
    //      <label class="d-flex align-items-center gap-2">
    //        <input type="radio" name="nda_conflict_choice" value="true">
    //        <span>${yesText}</span>
    //      </label>

    //      <label class="d-flex align-items-center gap-2">
    //        <input type="radio" name="nda_conflict_choice" value="false" checked>
    //        <span>${noText}</span>
    //      </label>
    //    </div>

    //    <div class="mb-2">
    //      <label class="form-label fw-bold">
    //        ${reasonLabel} <span class="text-danger">*</span>
    //      </label>

    //      <textarea id="ndaConflictReason"
    //                class="form-control"
    //                rows="4"
    //                placeholder="${reasonPlaceholder}"></textarea>

    //      <div id="ndaConflictError"
    //           class="text-danger small mt-1 d-none"></div>
    //    </div>

    //    <div class="d-flex justify-content-end mt-4">
    //      <button id="ndaSubmitBtn" class="btn btn-primary px-4">
    //        ${submitText}
    //        <i class="la la-send ms-2"></i>
    //      </button>
    //    </div>

    //  </div>
    //</div>`;
    //    }
    //    function NdaSubmit(response) {

    //        const btn = document.getElementById('ndaSubmitBtn');
    //        const reasonEl = document.getElementById('ndaConflictReason');
    //        const errEl = document.getElementById('ndaConflictError');

    //        const reasonRequiredText = getUiText('lblRequired','هذا الحقل مطلوب');

    //        btn.addEventListener('click', async function (e) {
    //            e.preventDefault();

    //            const reason = (reasonEl.value || '').trim();

    //            if (!reason) {
    //                errEl.textContent = reasonRequiredText;
    //                errEl.classList.remove('d-none');
    //                reasonEl.focus();
    //                return;
    //            }

    //            errEl.classList.add('d-none');

    //            const hasConflict =
    //                document.querySelector('input[name="nda_conflict_choice"]:checked')
    //                    ?.value === 'true';

    //            const payload = {
    //                evaluationRequestId: response.requestId,
    //                planId: response.planId,
    //                hasConflict: hasConflict,
    //                conflictReason: reason
    //            };

    //                const res = await $.ajax({
    //                    url: '/Evaluation/Nda/Approve',
    //                    method: 'POST',
    //                    contentType: 'application/json; charset=utf-8',
    //                    data: JSON.stringify(payload)
    //                });

    //                document.getElementById('ndaApprovalWrapper')?.remove();

    //                if (res?.isNdaApprovalPending === false) {
    //                    renderEvaluationPartiesSection(res);
    //                }

    //            } catch (err) {
    //                console.error(err);
    //                alert(getUiText('lblSaveFailed', 'حدث خطأ أثناء الحفظ'));
    //            } finally {
    //                if (window.formUtility?.coverSpin)
    //                    window.formUtility.coverSpin(false);
    //            }
    //        });
    $('#evaluationRequestStatusFilter').select2({
        placeholder: "اختر الحالة",
        allowClear: true,
        width: '100%',
        multiple: true
    });
    flatpickr('#evaluationRequestDateFrom', {
        dateFormat: "Y-m-d",
        allowInput: true
    });
    flatpickr("#evaluationRequestDateTo", {
        dateFormat: "Y-m-d",
        allowInput: true
    });
    Evaluation.Loaders.loadPlans('evaluationPlanIdFilter');
    Evaluation.Loaders.loadServiceStatus('evaluationRequestStatusFilter');
    evaluationRequestsListing.reload();
});



