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
            className: "td-full mb-3",
            render: function(data, type, row) {
                const safe = data || "";

                return `
                    <div data-bs-toggle= 'modal' data-bs-target= '#schoolDetailsPopup' class="plan-title-row text-decoration-none text-primary fw-bold school-details-link" data-id="${row.id}">
                        <i class="las la-school card-only-icon title-icon"></i>

                        <span class="plan-text-wrap px-1">
                            <span class="card-only-label title-label"></span>
                            <span class="plan-title-text-school">${data || ""}</span>
                        </span>
                    </div>
                    `;
            }
        },
        {
            data: "code",
            title: uiControlsSetup().GetUiControlText("lblSchoolCode"),
            className: "td-left small-width",
            render: function(data) {
                return `
                <i class="las la-barcode card-only-icon me-1"></i>
                <span class="card-only-label me-2">Code: </span>
                <span>${data || "_"}</span>
            `;
            }
        },
        {
            data: "schoolTypeName",
            title: uiControlsSetup().GetUiControlText("lblSchoolType"),
            className: "td-right small-width",
            render: function(data) {
                return `
                <i class="las la-graduation-cap card-only-icon me-1"></i>
                <span class="card-only-label me-2">Sector: </span>
                <span>${data || ""}</span>
            `;
            }
        },
        {
            data: "null",
            title: uiControlsSetup().GetUiControlText("lblSchoolLevel"),
            className: "td-full small-width",
            render: function(data) {
                return `
                <i class="las la-school card-only-icon me-1"></i>
                <span class="card-only-label me-2">Level: </span>
                <span>${ currentLang == 'ar' ? "ابتدائي, اعدادي" : "Primary, Preparatory"}</span>
            `;
            }
        },

            //{
             //   data: "nameAr",
             //   title: uiControlsSetup().GetUiControlText("lblSchoolName"),
             //   className: "td-left name",   
             //   render: function (data, type, row) {
             //       const safe = data || "";
             //       return `<a href="javascript:void(0)" data-bs-toggle= 'modal' data-bs-target= '#schoolDetailsPopup' class="text-decoration-none text-primary fw-bold school-details-link" data-id="${row.id}">${safe}</a>`;
              //  }
            //   },


           // {
           //     data: "code",
           //     title: uiControlsSetup().GetUiControlText("lblSchoolCode"),
           //     className: "td-left code"
           // },
           // {
             //   data: "schoolTypeName",
             //   title: uiControlsSetup().GetUiControlText("lblSchoolType"),
             //   className: "td-left type"
            //},
            //{
            //    data: "levelName",
            //    title: uiControlsSetup().GetUiControlText("lblSchoolLevel"),
            //    className: "td-left level"
            //},
            //{
            //    data: null,
            //    title: uiControlsSetup().GetUiControlText("lblSchoolLevel"),
            //    className: "td-left level",
            //    defaultContent: "ابتدائي, اعدادي"
            //}
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
            $('#schoolDetailsPopup').data('schoolId', rowData.id);
            const modal = new bootstrap.Modal(document.getElementById('schoolDetailsPopup'));
            modal.show();
        },
        onDraw: function () {
            $('#schoolTable').off('click', '.plan-request-card').on('click', '.plan-request-card', function (e) {
                const id = $(this).data('id');
                if (!id) return;
                $('#schoolDetailsPopup').data('schoolId', id);
                const modal = new bootstrap.Modal(document.getElementById('schoolDetailsPopup'));
                modal.show();
            });
        }
    });

   // schoolsListing.reload();
});
