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
        tabLabelSelector: '#tabEvaluationOperations .my-1',
        tabLabelKey: 'TabEvaluationOperations',
        columns: [
        {
    data: "evaluationType",
        title: uiControlsSetup().GetUiControlText("lblEvaluationPlan"),
        className: "td-full mb-4",
        render: function(data, type, row) {

            const statusColor = row.statusColor || "#A63D40";
            const statusText = `${row.status || ""}
`;

            return `
            <div class="request-info">

                <div class="request-icon"
                     style="background-color:${statusColor}; color:#000;">
                    <i class="las la-certificate"></i>
                </div>

                <div class="request-text">

                    <div class="request-header">
                        ${data || ""}
                    </div>

                    <div class="request-status-text card-only-row"
                         style="color:${statusColor};">
                        ${statusText}
                    </div>

                </div>

            </div>
        `;
        }
            },
    {
        data: null,
        title: uiControlsSetup().GetUiControlText("lblStatus"),
        className: "status-column",
        render: function(data, type, row) {

            const isCompleted = row.StatusISOPen === false;
            const statusColor = isCompleted ? "#0E6B32" : "#A63D40";
            const statusText = isCompleted ? "مكتمل" : "غير مكتمل";

            return `
            <span class="status-padding" style="color:${statusColor};">
                ${statusText}
            </span>
            `;
        }
    },
          {
        data: "orgTreeName",
        title: uiControlsSetup().GetUiControlText("lblSchoolName"),
        className: "td-full",
        render: function(data) {

            if (!data) return "_";

            return `  
        <div class="ellipsis school-name-row">
            <i class="las la-school card-only-icon me-1"></i>

            <span class="data-text">
                ${data}
            </span>
        </div>
        `;
        }
    },
      {
        data: "planName",
        title: uiControlsSetup().GetUiControlText("lblPlanName"),
        className: "td-full",
        render: function(data) {

            if (!data) return "_";

            return `  
                    
                        <i class="las la-file-signature card-only-icon me-1"></i>
                         <span class="data-text">${data}</span>
                    
                `;
        }
    },
         
               
  {
        data: "evlDateFrom",
        title: uiControlsSetup().GetUiControlText("lblEvaluationDateFrom"),
        className: "td-full td-date-range-block period-column",
        render: function(data, type, row) {
            if (!data) return "_";

            const daysLeft = getDaysUntil(data);
            const isOverdue = daysLeft !== null && daysLeft < 0;
            const isUrgent = daysLeft !== null && daysLeft >= 0 && daysLeft <= 3;
            const isWarning = daysLeft !== null && daysLeft > 3 && daysLeft <= 10;

            let countdownBadge = '';
            let nearNote = '';

           if (isOverdue) {

        countdownBadge = `
        <span class="status-countdown-badge info">
            <i class="las la-exclamation-circle"></i>
            متأخر ${Math.abs(daysLeft)} يوم
        </span>`;

        nearNote = `
        <span class="evaluation-near-note success">
            <i class="las la-exclamation-triangle"></i>
            تجاوز موعد التقييم
        </span>`;

    }
    else if (isUrgent) {

        countdownBadge = `
        <span class="status-countdown-badge info">
            <i class="las la-clock"></i>
            ${daysLeft} يوم متبقي
        </span>`;

        nearNote = `
        <span class="evaluation-near-note success">
            <i class="las la-exclamation-triangle"></i>
            هذه المدرسة على وشك التقييم
        </span>`;

    }
    else if (isWarning) {

        countdownBadge = `
        <span class="status-countdown-badge info">
            <i class="las la-clock"></i>
            ${daysLeft} يوم متبقي
        </span>`;

        nearNote = `
        <span class="evaluation-near-note success">
            <i class="las la-exclamation-triangle"></i>
            هذه المدرسة على وشك التقييم
        </span>`;
    }

            const fmt = ["M/D/YYYY", "MM/DD/YYYY", "YYYY-MM-DD", "DD-MM-YYYY"];
            const displayFrom = moment(data.split(' ')[0], fmt, false).format("DD-MM-YYYY");
            const displayTo = row.evlDateTo
                ? moment(row.evlDateTo.split(' ')[0], fmt, false).format("DD-MM-YYYY")
                : null;

           const dateRange = displayTo
        ? `
    <span>
        <span class="period-label me-1">
            ${getUiText("lblFrom", "From")}
        </span>

        <span class="data-text me-1">
            ${displayFrom}
        </span>

        <span class="period-label me-1">
            ${getUiText("lblTo", "To")}
        </span>

        <span class="data-text me-1">
            ${displayTo}
        </span>
    </span>
`
        : `
    <span>
        <span class="period-label me-1">
            ${getUiText("lblFrom", "From")}
        </span>

        <span class="data-text">
            ${displayFrom}
        </span>
    </span>
`;

            return `
            <div class="date-range-wrapper">

                <div class="d-flex align-items-center gap-1 flex-wrap">
                    <i class="las la-calendar card-only-icon"></i>
                    ${dateRange}
                </div>

               <div class="period-status-wrapper">
                    ${countdownBadge}
                    ${nearNote}
                </div>

            </div>`;
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

        <span class="data-text me-1">
            ${moment(data).format("DD-MM-YYYY")}
        </span>
        `;
        }
    },
    {
        data: "createOn",
        title: uiControlsSetup().GetUiControlText("lblRequestCreatedTime"),
        className: "td-left place-content-end border-top-card",
        render: function(data) {

            if (!data) return "_";

            return `
        <div class="d-flex justify-content-end place-content-end">

            <i class="las la-clock card-only-icon color-primary me-1"></i>

            <span class="data-text me-1">
                ${moment(data).format("hh:mm A")}
            </span>

        </div>
        `;
        }
    },
            
        ],
        rowCallback: function (row, data) {
            const days = getDaysUntil(data.evlDateFrom);
            if (days !== null && days < 0) {
                $(row).css('border-left', '3px solid #E24B4A');    
            } else if (days !== null && days <= 3) {
                $(row).css('border-left', '3px solid #E24B4A');    
            } else if (days !== null && days <= 10) {
                $(row).css('border-left', '3px solid #BA7517');    
            }
        },
        onRowClick: function (rowData) {
            openEvaluationRequestDetails(rowData.id);
        }
    });

    window.openEvaluationRequestDetails = function (requestId) {
        const options = {
            success: function (response) {

                formUtility.attachments = response.attachments || [];
                $('#evaluationRequeststatus').text(response.status || '');
                $('#evaluationRequestNo').text(response.requestNumber || '444');

                $('#breadcrumbSchoolName').text((window.currentLang === "ar" ? response.school.nameAr : response.school.nameEn) || '');
                $('#evaluationRequestDetailsModal').modal('show');

                window.isEvaluationRequestOpen = response.statusISOPen;
                const formAccordionItem = document.getElementById("formAccordionItem");

                if (!response.formGroups || response.formGroups.length === 0) {
                    if (formAccordionItem) formAccordionItem.style.display = "none";
                } else {
                    if (formAccordionItem) formAccordionItem.style.display = "block";
                }

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
                        templateContainerId: 'evaluationRequestDetailsModal_divTemplates',
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
                bindEvaluationDates(response);
                if (response.assignment && response.assignment.length > 0) {

                    $("#forceAssignmentAccordion").removeClass("d-none");
                    renderForceAssignments(response.assignment, "forceAssignmentDiv");

                } else {

                    $("#forceAssignmentAccordion").addClass("d-none");
                }

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

  function bindEvaluationDates(response) {
        const fmt = ["M/D/YYYY", "MM/DD/YYYY", "YYYY-MM-DD", "DD-MM-YYYY"];
        const fromRaw = response.evlDateFrom || '';
        const toRaw = response.evlDateTo || '';

        const $container = $('#evaluationDateRangeContainer').empty();
        if (!fromRaw) return;

        const displayFrom = moment(fromRaw.split(' ')[0], fmt, false).format("DD-MM-YYYY");
        const displayTo = toRaw
            ? moment(toRaw.split(' ')[0], fmt, false).format("DD-MM-YYYY")
            : null;

        const daysLeft = getDaysUntil(fromRaw);
        const isOverdue = daysLeft !== null && daysLeft < 0;
        const isUrgent = daysLeft !== null && daysLeft >= 0 && daysLeft <= 3;
        const isWarning = daysLeft !== null && daysLeft > 3 && daysLeft <= 10;

        let badge = '';
        let note = '';

        if (isOverdue) {
            badge = `
            <span class="status-countdown-badge danger">
                <i class="las la-exclamation-circle"></i>
                متأخر ${Math.abs(daysLeft)} يوم
            </span>`;

            note = `
            <span class="evaluation-near-note danger">
                <i class="las la-exclamation-triangle"></i>
                تجاوز موعد التقييم
            </span>`;

        } else if (isUrgent) {
            badge = `
            <span class="status-countdown-badge danger">
                <i class="las la-clock"></i>
                ${daysLeft} يوم متبقي
            </span>`;

            note = `
            <span class="evaluation-near-note danger">
                <i class="las la-exclamation-triangle"></i>
                هذه المدرسة على وشك التقييم
            </span>`;

        } else if (isWarning) {
            badge = `
            <span class="status-countdown-badge warning">
                <i class="las la-clock"></i>
                ${daysLeft} يوم متبقي
            </span>`;

            note = `
            <span class="evaluation-near-note warning">
                <i class="las la-exclamation-triangle"></i>
                هذه المدرسة على وشك التقييم
            </span>`;
        }

        const dateRange = displayTo
            ? `${displayFrom} <i class="las la-arrow-right mx-1 opacity-50"></i> ${displayTo}`
            : displayFrom;

        $container.html(`
        <div class="date-range-wrapper">
            <div class="d-flex align-items-center gap-2 flex-wrap">
                <i class="las la-calendar text-muted fs-14"></i>
                <span class="modal-date-range-text">${dateRange}</span>
            </div>

            <div class="d-flex flex-wrap gap-2 align-items-center">
                ${badge}
                ${note}
            </div>
        </div>
    `);
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

    function getDaysUntil(dateStr) {
        if (!dateStr) return null;
        // strip time portion: "10/8/2025 12:00:00 AM" → "10/8/2025"
        const datePart = dateStr.split(' ')[0];
        const target = moment(datePart, ["M/D/YYYY", "MM/DD/YYYY", "YYYY-MM-DD", "DD-MM-YYYY"], false).startOf('day');
        if (!target.isValid()) return null;
        return target.diff(moment().startOf('day'), 'days');
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
const isAr = document.documentElement.lang.toLowerCase().startsWith("ar");

    flatpickr(".datePicker", {
        locale: isAr ? "ar" : "en",
        dateFormat: "Y-m-d",
        allowInput: true
    });
    Evaluation.Loaders.loadPlans('evaluationPlanIdFilter');
    //Evaluation.Loaders.loadServiceStatus('evaluationRequestStatusFilter');
    evaluationRequestsListing.reload();
});



