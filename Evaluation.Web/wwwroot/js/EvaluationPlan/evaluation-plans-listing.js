$(document).ready(function () {
    let plansListing = null;

    function getPlansFilter() {
        return {
            YearId: $('#planYearFilter').val(),
            StatusId: $('#planStatusFilter').val()
        };
    }

    function initPlansListing() {
        if (plansListing) {
            console.log('[PLANS] Already initialized, reloading...');
            plansListing.reload();
            return;
        }

        console.log('[PLANS] Creating new listing...');
        plansListing = evaluationListing.createListing({
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
            columns: [
                //{
                //    data: "planNumber",
                //    className: "td-left"
                //},
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
                        return `${row.startDate} - ${row.endDate}`
                    }
                }
                ,
                {
                    data: "statusCode",
                    className: "td-right",
                    render: function (data, type, row) {
                        //const color = row.statusColor || "#cccccc";
                        //const textColor = getContrastingTextColor(color);
                        //return `<span class="request-status" style="background:${color};color:${textColor}">${data || ""}</span>`;
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

        // **FORCE RELOAD IMMEDIATELY - Don't wait for visibility**
        console.log('[PLANS] Force reload immediately');
        setTimeout(function () {
            plansListing.reload();
        }, 50);
    }

    function openPlanDetails(planId) {
        const options = {
            success: function (response) {
                $('#planDetailsModalLabel').text(response.name);
                $('#planDetailsModalBody').html(response.htmlContent || '');
                $('#planDetailsModal').modal('show');
            }
        };
        jqClient(options).Get(`/Plan/Details?planId=${planId}`);
    }

    $('#addPlanBtn').on('click', function () {
        window.location.href = '/Plan/Create';
    });

    $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        const href = $(e.target).attr('href');
        console.log('[TAB] Tab shown:', href);

        if (href === '#tab-plan-details') {
            console.log('[TAB] Plan details tab activated');
            initPlansListing();
        }
    });

    setTimeout(function () {
        const parentActive = $('#tab-plans').hasClass('active') || $('#tab-plans').hasClass('show');
        const childActive = $('#tab-plan-details').hasClass('active') || $('#tab-plan-details').hasClass('show');

        console.log('[INIT] Parent tab active:', parentActive);
        console.log('[INIT] Child tab active:', childActive);

        if (parentActive && childActive) {
            console.log('[INIT] Both tabs active, initializing');
            initPlansListing();
        }
    }, 300);
});