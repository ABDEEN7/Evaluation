$(document).ready(function () {
    function getSchoolFilterInput() {
        return {
            SchoolName: $('#schoolNameFilter').val(),
            SchoolCode: $('#schoolCodeFilter').val(),
            PhaseId: $('#schoolPhaseFilter').val(),
            TypeId: $('#schoolTypeFilter').val(),
            RegionId: $('#schoolRegionFilter').val(),
            StatusId: $('#schoolStatusFilter').val(),
            //DepartmentRoutingPath: departmentName
        };
    }
    //let departmentRoutePath = sharedUtility().extractDepartmentName();

    const schoolsListing = evaluationListing.createListing({
        tableId: 'schoolTable',
        ajaxUrl: `/School/${departmentRoutePath}/GetSchools`,
        getFilterInput: getSchoolFilterInput,
        filterFormId: 'school-filter-form-id',
        filterBtnId: 'filterSchoolBtnId',
        clearFilterBtnId: 'clearFilterSchoolBtnId',
        tabLabelSelector: '#tabSchoolsAnchorTag',
        tabLabelKey: 'lblSchools',
        enableCardView: true,
        cardViewBtnId: 'cardViewSchool',
        tableViewBtnId: 'tblViewSchool',
        rowClass: 'plan-request-card',
        columns: [
            {
                data: "nameAr",
                title: uiControlsSetup().GetUiControlText("lblSchoolName"),
                className: "td-left name",
               
                render: function (data, type, row) {
                    const safe = data || "";
                    return `<a href="javascript:void(0)" data-bs-toggle= 'modal' data-bs-target= '#SCHOOL' class="text-decoration-none text-primary fw-bold school-details-link" data-id="${row.id}">${safe}</a>`;
                }
            },
            {
                data: "code",
                title: uiControlsSetup().GetUiControlText("lblSchoolCode"),
                className: "td-left code"
            },
            {
                data: "schoolTypeName",
                title: uiControlsSetup().GetUiControlText("lblSchoolType"),
                className: "td-left type"
            },
            //{
            //    data: "phaseName",
            //    title: uiControlsSetup().GetUiControlText("lblSchoolPhase"),
            //    className: "td-left phase"
            //},
            //{
            //    data: "region",
            //    title: uiControlsSetup().GetUiControlText("lblRegion"),
            //    className: "td-left region"
            //},
            //{
            //    data: "currentPlanStatus",
            //    title: uiControlsSetup().GetUiControlText("lblCurrentEvaluationStatus"),
            //    className: "td-right status",
            //    render: function (data, type, row) {
            //        const color = row.currentPlanStatusColor || "#cccccc";
            //        const textColor = getContrastingTextColor(color);
            //        const safe = data || "";
            //        return `<span class="request-status m-0" style="background-color:${color};color:${textColor};">${safe}</span>`;
            //    }
            //}
        ],
        onRowClick: function (rowData, event) {
            if ($(event.target).closest('.school-details-link').length) return;
            //openSchoolDetails(rowData.id);
        },
        onDraw: function () {
            $('#schoolTable').off('click', '.school-details-link').on('click', '.school-details-link', function (e) {
                e.preventDefault();
                //const id = $(this).data('id');
                //openSchoolDetails(id);
            });
        }
    });

    //function openSchoolDetails(schoolId) {
    //    const options = {
    //        success: function (response) {
    //            $('#schoolModalLabel').text(response.name || '');
    //            $('#schoolModalBody').html(response.htmlContent || '');
    //            $('#schoolDetailsModal').modal('show');
    //        }
    //    };
    //    //jqClient(options).Get(`/School/GetSchoolDetails?schoolID=${schoolId}`);
    //}

    schoolsListing.reload();
});
