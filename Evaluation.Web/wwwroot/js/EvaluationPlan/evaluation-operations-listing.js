$(document).ready(function () {
    var DepartmentRouting = sharedUtility().extractDepartmentName();
    const API_ENDPOINTS = {
        GET_Service_Status: `/ServiceRequest/${DepartmentRouting}/GetServiceStatus`,
        GET_Plans: `/Plan/${DepartmentRouting}/GetPlansDDL`,
        GET_Schools: `Schools/${DepartmentRouting}/GetSchoolsDDL`
    };
    let lang = sharedUtility().GetCookie('lang');
     window.currentLang = lang;
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
            PlanId: $('#evaluationPlanIdFilter').val() || null,
            OrgTree: $('#evaluationOrgTreeFilter').val(),
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

    window.openEvaluationRequestDetails = function (requestId) {
        const options = {
            success: function (response) {

                formUtility.attachments = response.attachments || [];
                $('#evaluationRequeststatus').text(response.status || '');
                $('#evaluationRequestNoText').text(response.requestNumber || '');

                $('#breadcrumbSchoolName').text((window.currentLang === "ar" ? response.school.nameAr : response.school.nameEn) || '');
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
                        document.getElementById('NDAContainerDiv');

                    container.insertAdjacentHTML('afterbegin', generateNdaApprovalDiv());
                    NdaSubmit(response, requestId);
                }
                else {
                    renderEvaluationPartiesSection(response, requestId);
                    window.openRequestsModule.render(response.evaluationParties || []);
                }

                bindSchoolDetails(response);
            }
        };

        jqClient(options).Get(`/ServiceRequest/${DepartmentRouting}/GetEvaluationDetails?requestId=${requestId}`);
    };
  
    function bindSchoolDetails(response) {
        const $root = $("#school-details-container");
        const s = response.school || {};

        $root.find("#modalLabel").text((window.currentLang === "ar" ? s.nameAr : s.nameEn) || '');
        $root.find("#managerName").text(s.manageName || '');
        $root.find("#establishmentDate").text(s.establishmentDate || '');
        $root.find("#teachers").text(s.teachers || '');
        $root.find("#students").text(s.students || '');
        $root.find("#phone").text(s.phone || '');
        $root.find("#email").text(s.orgEmail || '');
        $root.find("#address").text(s.address || '');
    }
    function renderEvaluationPartiesSection(response, requestId) {
        const parties = response?.evaluationParties || [];

        if (!window.formUtility || typeof formUtility.renderEvaluationParties !== "function") {
            console.error("formUtility.renderEvaluationParties is not loaded.");
            return;
        }

        formUtility.renderEvaluationParties(parties, requestId, {
            containerId: "evaluationPartiesContainer",
            parentAccordionId: "evaluationRootAccordion",
            expandFirst: true
        });
    }

    function generateNdaApprovalDiv() {

        const title = getUiText(
            'lblNdaConflictTitle',
            'هل لديك تضارب مصالح مع هذه المدرسة؟'
        );

        const yesText = getUiText('lblYes', 'نعم');
        const noText = getUiText('lblNo', 'لا');

        const reasonLabel = getUiText(
            'lblNdaConflictReason',
            'أوضح سبب التضارب'
        );

        const reasonPlaceholder = getUiText(
            'lblNdaConflictReasonPlaceholder',
            'اكتب سبب تضارب المصالح هنا...'
        );

        const submitText = getUiText(
            'lblSubmit',
            'إرسال'
        );

        return `
    <div id="ndaApprovalWrapper" class="card mt-3">
      <div class="card-body">

        <h5 class="text-center mb-4 fw-bold">
            ${title}
        </h5>

        <div class="d-flex justify-content-center gap-4 mb-3">
          <label class="d-flex align-items-center gap-2">
            <input type="radio" name="nda_conflict_choice" value="true">
            <span>${yesText}</span>
          </label>

          <label class="d-flex align-items-center gap-2">
            <input type="radio" name="nda_conflict_choice" value="false" checked>
            <span>${noText}</span>
          </label>
        </div>

        <div id="ndaReasonWrapper" class="mb-2 d-none">
          <label class="form-label fw-bold">
            ${reasonLabel} <span class="text-danger">*</span>
          </label>

          <textarea id="ndaConflictReason"
                    class="form-control"
                    rows="4"
                    placeholder="${reasonPlaceholder}"></textarea>

          <div id="ndaConflictError"
               class="text-danger small mt-1 d-none"></div>
        </div>

        <div class="d-flex justify-content-end mt-4">
          <button id="ndaSubmitBtn" class="btn btn-primary px-4">
            ${submitText}
            <i class="la la-send ms-2"></i>
          </button>
        </div>

      </div>
    </div>`;
    }
    function NdaSubmit(response, requestId) {
        const btn = document.getElementById('ndaSubmitBtn');
        const reasonEl = document.getElementById('ndaConflictReason');
        const errEl = document.getElementById('ndaConflictError');
        const wrapperEl = document.getElementById('ndaReasonWrapper');
        const radios = document.querySelectorAll('input[name="nda_conflict_choice"]');

        const reasonRequiredText = getUiText('lblRequired', 'هذا الحقل مطلوب');

        // Toggle reason textarea visibility based on Yes/No
        radios.forEach(r => {
            r.addEventListener('change', function () {
                if (this.value === 'true' && this.checked) {
                    wrapperEl.classList.remove('d-none');
                } else {
                    wrapperEl.classList.add('d-none');
                    errEl.classList.add('d-none');
                }
            });
        });

        btn.addEventListener('click', function (e) {
            e.preventDefault();

            const selectedValue =
                document.querySelector('input[name="nda_conflict_choice"]:checked')?.value;

            const hasConflict = selectedValue === 'true';
            const reason = (reasonEl.value || '').trim();

            if (hasConflict && !reason) {
                errEl.textContent = reasonRequiredText;
                errEl.classList.remove('d-none');
                reasonEl.focus();
                return;
            }
            errEl.classList.add('d-none');

            const payload = {
                evaluationRequestId: requestId,
                hasConflict: hasConflict,
                conflictReason: hasConflict ? reason : null
            };

            console.log('NDA payload:', payload);

            const options = {
              
                success: function (res) {
                    document.getElementById('ndaApprovalWrapper')?.remove();

                    if (res?.isNdaApprovalPending === false) {
                        renderEvaluationPartiesSection(res);
                    }
                },
                error: function (err) {
                    console.error('NDA submit failed:', err);
                    alert(getUiText('lblSaveFailed', 'حدث خطأ أثناء الحفظ'));
                }
            };

            jqClient(options).Post(`/ServiceRequest/${DepartmentRouting}/ApproveNda`,payload);
        });
    }

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



