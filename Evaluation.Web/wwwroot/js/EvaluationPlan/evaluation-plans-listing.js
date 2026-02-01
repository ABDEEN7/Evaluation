$(document).ready(function () {

    /* =========================
     * API ENDPOINTS
     * ========================= */
    const API_ENDPOINTS = {
        GET_ACADEMIC_YEARS: '/AcademicYear/GetAcademicYearByDepartment'
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
        return {
            YearId: $('#planYearFilter').val(),
            schoolName: $('#schoolName').val()
        };
    }

    /* =========================
     * INITIALIZE LISTING
     * ========================= */
    const plansListing = evaluationListing.createListing({
        tableId: 'evaluationPlansTable',
        ajaxUrl: `/Plan/${departmentRoutePath}/GetPlans`,
        getFilterInput: getPlansFilter,
        filterFormId: 'plan-filter-form-id',
        filterBtnId: 'filterPlanBtnId',
        clearFilterBtnId: 'clearFilterPlanBtnId',
        tabLabelSelector: '#tabPlansAnchorTag',
        tabLabelKey: 'lblEvaluationPlans',
        enableCardView: false,
        cardViewBtnId: 'cardViewEvaluationPlans',
        tableViewBtnId: 'tblViewEvaluationPlans',
        rowClass: 'plan-row',

        onAjaxSuccess: function (response) {
            return {
                data: response.items || response.data || [],
                totalDataCount: response.totalCount || response.totalDataCount || 0,
                TotalDataCount: response.totalCount || response.TotalDataCount || 0
            };
        },

        columns: [
            {
                data: "name",
                className: "td-left"
            },
            {
                data: "countSchools",
                className: "td-left"
            },
            {
                data: null,
                className: "td-left",
                render: function (data, type, row) {
                    return `${row.startDate} - ${row.endDate}`;
                }
            },
            {
                data: "statusCode",
                className: "td-right",
                render: function (data) {
                    return `<span class="request-status">${data || ""}</span>`;
                }
            },
            {
                data: null,
                orderable: false,
                className: "td-center",
                render: function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-primary view-plan" data-id="${row.id}">
                            عرض
                        </button>
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

                    const dropdownId = `dropdownMenuButton_${row?.id || meta?.row || Math.random().toString(36).slice(2)}`;

                    let actionsHtml = `<div class="dropdown d-block w-100">`;

                    actionsHtml += `
                        <button
                            class="btn mb-0 dropdown-toggle w-100 btn-draft"
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

                    
                    const baseAppUrl = sharedUtility().GetCookie("webAppBaseURL") || "";
                    services.forEach(function (service) {

                        const serviceId = service?.id || service?.Id || "";
                        const planId = row?.id || "";

                        const serviceName =
                            service?.nameAr ||
                            service?.NameAr ||
                            service?.nameEn ||
                            service?.NameEn ||
                            service?.name ||
                            service?.Name ||
                            "";

                        const serviceIcon = service?.icon || service?.Icon || "fa-solid fa-file";

                        const url = decodeURIComponent(
                            baseAppUrl.concat(`/Scholarship//NewRequest?serviceId=${serviceId}&scholarshipId=${scholarshipId}`)
                        );

                        actionsHtml += `
                <a class="dropdown-item"
                   type="button"
                   href="${url}"
                   onclick="event.stopPropagation();"
                >
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

        onRowClick: function (rowData) {
            openPlanDetails(rowData.id);
        }
    });

    /* =========================
     * EVENTS
     * ========================= */

    // Reload plans when year changes
    $('#planYearFilter').on('change', function () {
        plansListing.reload();
    });
    $('#searchPlansBtn').on('click', function (e) {
        e.preventDefault();
        plansListing.reload();
    });
    // View plan details
    function openPlanDetails(planId) {
        //jqClient({
        //    success: function (response) {
        //        $('#planDetailsModalLabel').text(response.name);
        //        $('#planDetailsModalBody').html(response.htmlContent || '');
        //        $('#planDetailsModal').modal('show');
        //    }
        //}).Get(`/Plan/Details?planId=${planId}`);
        window.location.href = `/Plan/Details?planId=${planId}`;
    }

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
