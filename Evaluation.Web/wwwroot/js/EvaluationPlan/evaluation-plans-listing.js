$(document).ready(function () {
    const fu = window.formUtility || {};
    const fapi = window.FormApi;

    /* =========================
     * API ENDPOINTS
     * ========================= */
    var deprouting = sharedUtility().extractDepartmentName();

    const API_ENDPOINTS = {
        GET_ACADEMIC_YEARS: `/AcademicYear/${deprouting}/GetAcademicYearByDepartment`
    };

    /* =========================
     * LOAD ACADEMIC YEARS
     * ========================= */
    function loadAcademicYears() {
        const $ddl = $('#planYearFilter');

        $ddl
            .empty()
            .append('<option value="">الكل</option>')
            .prop('disabled', true);

        jqClient()
            .Get(API_ENDPOINTS.GET_ACADEMIC_YEARS)
            .done(function (response) {
                $ddl.prop('disabled', false);

                //if (response && response.result && response.result.length > 0) {
                if (response) {
                    response.forEach(function (year) {
                        $ddl.append(`
                            <option value="${year.id}">
                                ${year.nameAr}
                            </option>
                        `);
                    });
                }
            })
            .fail(function (jqXHR, textStatus) {
                console.error('Failed to load academic years:', textStatus);
                $ddl.prop('disabled', false);
            });
    }

    /* =========================
     * FILTER OBJECT
     * ========================= */
    function getPlansFilter() {

        const yearId = $('#planYearFilter').val();
        const schoolName = $('#schoolName').val();

        const filters = {};

        if (yearId) filters.yearId = yearId;
        if (schoolName) filters.schoolName = schoolName;

        return filters;
    }


    /* =========================
     * INITIALIZE LISTING
     * ========================= */
    const plansListing = getevaluationListing.createListing({
        tableId: 'evaluationPlansTable',
        ajaxUrl: `/Plan/${departmentRoutePath}/GetPlans`,
        getFilterInput: getPlansFilter,
        filterFormId: 'plan-filter-form-id',
        filterBtnId: 'filterPlanBtnId',
        clearFilterBtnId: 'clearFilterPlanBtnId',
        tabLabelSelector: '#tabPlansAnchorTag',
        tabLabelKey: 'lblEvaluationPlans',
        enableCardView: true,
        cardViewBtnId: 'cardViewEvaluationPlans',
        tableViewBtnId: 'tblViewEvaluationPlans',
        rowClass: 'plan-row',

        // Transform API response to match expected format
        transformResponse: function (response) {
            // API returns { items: [...], totalCount: 24 }
            // Common listing expects { data: [...], totalDataCount: ... }
            return {
                data: response.items || [],
                totalDataCount: response.totalCount || 0
            };
        },

        columns: [
           {
            data: "name",
            className: "td-left py-1",
            render: function(data, type, row) {
                const isApproved = row.statusCode === "Approved";
                return `
                    <div class="plan-title-row mb-3">
 
                        <i class="las la-file-signature card-only-icon title-icon"></i>
 
                        <span class="plan-text-wrap px-2">
                    <span class="card-only-label title-label">Plan: </span>
                    <span class="plan-title-text">${data || ""}</span>
                    </span>
 
                        ${isApproved ? `
                    <span class="request-status approved-status mx-2 p-1">
                    <i class="las la-check"></i>
                                ${row.statusCode}
                    </span>
                        ` : ''}
 
                    </div>
                    `;
                              }
            },
            {
                data: "countSchools",
                className: "td-left py-1",
                render: function(data) {
                    return `
                    <i class="las la-school card-only-icon"></i>
                    <span class="card-only-label me-1">Schools count: </span>
                    ${data || ""}
                    `;
                }
            },
            {
            data: null,
            className: "td-left py-1",
                render: function(data, type, row) {
                    return `
                    <i class="las la-calendar-week card-only-icon"></i>
                    <span class="card-only-label me-1">Period: </span>
                    من ${row.startDate} إلى ${row.endDate}
                    `;
                }
            },
            {
                data: 'services',
                className: "td-full p-0 process",
                title: uiControlsSetup().GetUiControlText('lblActions'),
                orderable: false,
                render: function (data, type, row, meta) {

                    const services = Array.isArray(data) ? data : [];
                    if (services.length === 0) return '';

                    const dropdownId = `dropdownMenuButton_${row?.id || meta?.row || Math.random().toString(36).slice(2)}`;

                    let actionsHtml = `<div class="dropdown d-block w-100 p-1">`;

                    actionsHtml += `
                                <button
                                    class="btn dropdown-toggle w-100 btn-primary mt-1 py-2"
                                    type="button"
                                    id="${dropdownId}"
                                    data-bs-toggle="dropdown"
                                    aria-haspopup="true"
                                    aria-expanded="false"
                                    onclick="event.stopPropagation();"
                                >
                                    <span>${uiControlsSetup().GetUiControlText('lblProcedures')}</span>
                                </button>
                            `;

                    actionsHtml += `<div class="dropdown-menu w-100" aria-labelledby="${dropdownId}" onclick="event.stopPropagation();">`;

                    const planId = row?.id || "";

                    services.forEach(function (service) {

                        const serviceId = service?.id || service?.Id || "";
                        if (!serviceId) return;

                        const lang = window.currentLang || "ar";

                        const serviceName =
                            (lang === "ar"
                                ? (service?.nameAr || service?.NameAr)
                                : (service?.nameEn || service?.NameEn)
                            ) ||
                            service?.name || service?.Name || "";

                        const serviceIcon = service?.icon || service?.Icon || "fa-solid fa-file";

                        actionsHtml += `
                                        <a class="dropdown-item"
                                           href="#"
                                           onclick="InitializeCreatePlanRequestService('${serviceId}','${planId}'); return false;">
                                            <i class="${serviceIcon} mx-1"></i>
                                            ${serviceName}
                                        </a>
                                    `;
                    });

                    actionsHtml += `</div></div>`;
                    return actionsHtml;
                }
            }


        ],

        onRowClick: function(rowData, e) {

          // prevent dropdown clicks from opening details
          if ($(e.target).closest('.dropdown, .dropdown-menu, .dropdown-item').length) {
                return;
          }
          InitializePlanDetails(rowData.id);
    }

    });

    /* =========================
     * EVENTS
     * ========================= */
    window.InitializeCreatePlanRequestService = async function (serviceId, planId) {
        try {
            if (!serviceId || !planId) {
                console.error("Missing serviceId or planId", { serviceId, planId });
                return;
            }

            const createPlanRequestService = await fapi.fetchJSON(
                `/FormRender/${departmentRoutePath}/GetCreatePlanRequestService` +
                `?serviceId=${encodeURIComponent(serviceId)}` +
                `&planId=${encodeURIComponent(planId)}`
            );

            if (!createPlanRequestService) return;


            const el = document.getElementById("CreateRequestModal");
            const modal = bootstrap.Modal.getOrCreateInstance(el);
            modal.show();

            const serviceName = createPlanRequestService.name;

            const headerEl = document.getElementById("CreateRequestModalLabel");
            if (headerEl) headerEl.textContent = serviceName ? " - " + serviceName : "";

            const actions = createPlanRequestService.actions || [];

            const initialActions = actions.filter(a => a.isInitialAction === true);

            if (initialActions.length === 1) {
                const firstAction = initialActions[0];
                initialAction = firstAction?.bakendName || initialAction;

                $("#ActionsDropDown").hide();
                $("label[for='ActionsDropDown']").hide();

                await RenderActionFields(createPlanRequestService.serviceRequestDTO);
            } else {
                $("#ActionsDropDown").show();
                $("label[for='ActionsDropDown']").show();
                fillActionDropDown(actions);
            }

        } catch (err) {
            console.error("InitializeCreatePlanRequestService error:", err);
        }
    };

    /* =========================
   * VIEW PLAN DETAILS - MINIMAL VERSION
   * ========================= */
    /* =========================
   * VIEW PLAN DETAILS - MINIMAL VERSION
   * ========================= */
    window.InitializePlanDetails = async function (planId) {
        try {
            if (!planId) {
                console.error("Missing planId", { planId });
                return;
            }

            // Fetch plan details
            const response = await fapi.fetchJSON(
                `/Plan/${departmentRoutePath}/GetPlanDetails?planId=${encodeURIComponent(planId)}`
            );

            if (!response || !response.result) {
                console.error("No plan details received");
                return;
            }

            // Extract the actual plan data from the result property
            const planDetails = response.result;

            // Show the modal
            const el = document.getElementById("PlanDetailsModal");
            const modal = bootstrap.Modal.getOrCreateInstance(el);
            formUtility.addQueryParameter('planId', planId);
            modal.show();

            // Clear any previous content in modal body
            const modalBody = el.querySelector('.modal-body');
            if (modalBody) {
                modalBody.innerHTML = '';
            }

            // Generate the plan fields HTML
            const html = planUtility.generatePlanFieldsHTML('view');

            // Insert the HTML into the modal body instead of main content
            if (modalBody) {
                modalBody.innerHTML = html;
            }

            // Initialize the plan handler with the fetched data
            PlanHandler.init(true, 'view', planDetails);

        } catch (err) {
            console.error("InitializePlanDetails error:", err);
        }
    };
    // Helper function to render actions if needed
    function renderPlanActions(actions, planId) {
        const actionsContainer = document.getElementById("plan-actions-container");
        if (!actionsContainer) return;

        let actionsHtml = `
        <div class="dropdown">
            <button class="btn btn-primary dropdown-toggle" 
                    type="button" 
                    id="planActionsDropdown" 
                    data-bs-toggle="dropdown" 
                    aria-expanded="false">
                الإجراءات
            </button>
            <ul class="dropdown-menu" aria-labelledby="planActionsDropdown">
    `;

        actions.forEach(function (action) {
            const actionName = action.nameAr || action.name || "";
            const actionIcon = action.icon || "fa-solid fa-file";

            actionsHtml += `
            <li>
                <a class="dropdown-item" href="#" 
                   onclick="handlePlanAction('${action.id}', '${planId}'); return false;">
                    <i class="${actionIcon} mx-1"></i>
                    ${actionName}
                </a>
            </li>
        `;
        });

        actionsHtml += `</ul></div>`;
        actionsContainer.innerHTML = actionsHtml;
    }
    $('#filterPlanBtnsId').on('click', function () {
        plansListing.reload();
    });
  
    $('#addPlanBtn').on('click', function () {
        window.location.href = '/Plan/Create';
    });

    /* =========================
     * TAB HANDLING
     * ========================= */
    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        if ($(e.target).attr('href') === '#tab-plan-details') {
            plansListing.reload();
            loadAcademicYears();

        }
    });

    // Initial load if tab already active
    setTimeout(function () {
        const $tab = $('#tab-plan-details');
        if ($tab.is(':visible') && ($tab.hasClass('active') || $tab.hasClass('show'))) {
            plansListing.reload();
        }
    }, 500);

    /* =========================
     * INITIAL PAGE LOAD
     * ========================= */

});