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
        ajaxUrl: '/Plan/GetPlans',
        getFilterInput: getPlansFilter,
        filterFormId: 'plan-filter-form-id',
        filterBtnId: 'filterPlanBtnId',
        clearFilterBtnId: 'clearFilterPlanBtnId',
        tabLabelSelector: '#tabPlansAnchorTag',
        tabLabelKey: 'lblEvaluationPlans',
        enableCardView: false,
        cardViewBtnId: 'cardViewPlan',
        tableViewBtnId: 'tblViewPlan',
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
