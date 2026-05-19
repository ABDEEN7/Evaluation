$(document).ready(function () {

    let currentLang = sharedUtility().GetCookie('lang');
    function getSchoolFilterInput() {
        return {
            Name: $('#schoolNameFilter').val(),
            SchoolCode: $('#schoolCodeFilter').val(),
            PhaseId: $('#schoolPhaseFilter').val(),
            TypeId: $('#schoolTypeFilter').val(),
            RegionId: $('#schoolRegionFilter').val(),
            StatusId: $('#schoolStatusFilter').val(),
            //DepartmentRoutingPath: departmentName
        };
    }

    const schoolsListing = getevaluationListing.createListing({
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

                data: currentLang == 'ar' ? "nameAr" : "nameEn",
                title: uiControlsSetup().GetUiControlText("lblSchoolName"),
                className: "td-left name",
               
                render: function (data, type, row) {
                    const safe = data || "";
                    return `<a href="javascript:void(0)" data-bs-toggle= 'modal' data-bs-target= '#schoolDetailsPopup' class="text-decoration-none text-primary fw-bold school-details-link" data-id="${row.id}">${safe}</a>`;
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
            {
                data: null,
                title: uiControlsSetup().GetUiControlText("lblSchoolLevel"),
                className: "td-left level",
                defaultContent: currentLang == 'ar' ? "ابتدائي, اعدادي" : "Primary, Preparatory"
            }
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

    schoolsListing.reload();
});
