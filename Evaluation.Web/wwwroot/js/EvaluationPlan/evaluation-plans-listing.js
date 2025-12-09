$(document).ready(function () {
    function getPlansFilter() {
        return {
            PlanName: $('#planNameFilter').val(),
            YearId: $('#planYearFilter').val(),
            StatusId: $('#planStatusFilter').val()
        };
    }

    const plansListing = evaluationListing.createListing({
        tableId: 'planTable',
        ajaxUrl: '/EvaluationPlan/GetPlans',
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
            {
                data: "name",
                title: uiControlsSetup().GetUiControlText("lblEvaluationPlan"),
                className: "td-left name",
                render: function (data) {
                    return `<strong class="text-truncate-2">${data || ""}</strong>`;
                }
            },
            {
                data: "yearName",
                title: uiControlsSetup().GetUiControlText("lblAcademicYear"),
                className: "td-left"
            },
            {
                data: "schoolsCount",
                title: uiControlsSetup().GetUiControlText("lblSchoolsCount"),
                className: "td-right"
            },
            {
                data: "status",
                title: uiControlsSetup().GetUiControlText("lblPlanStatus"),
                className: "td-right",
                render: function (data, type, row) {
                    const color = row.statusColor || "#cccccc";
                    const textColor = getContrastingTextColor(color);
                    return `<span class="request-status m-0" style="background-color:${color};color:${textColor};">${data || ""}</span>`;
                }
            }
        ],
        onRowClick: function (rowData) {
            openPlanDetails(rowData.id);
        }
    });

    function openPlanDetails(planId) {
        const options = {
            success: function (response) {
                $('#planDetailsModalLabel').text(response.name);
                $('#planDetailsModalBody').html(response.htmlContent || '');
                $('#planDetailsModal').modal('show');
            }
        };
        jqClient(options).Get(`/EvaluationPlan/Details?planId=${planId}`);
    }

    $('#addPlanBtn').on('click', function () {
        window.location.href = '/EvaluationPlan/Create';
    });

    plansListing.reload();
});
