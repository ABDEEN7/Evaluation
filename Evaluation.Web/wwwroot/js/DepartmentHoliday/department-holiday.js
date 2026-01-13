let table = null, currentPage = 0, isLoading = true;
const gridContainerId = "view-container",
    $tblContentContainer = $('#tbltemplatecontainer'),
    $formSection = $('#formsection'),
    $formContent = $('#formcontent'),
    $btnAddbutton = $('#btnaddcontent'),
    btnSubmitId = "btn-submit";
let lang = sharedUtility().GetCookie('lang');
let txtDir = lang === "ar" ? "RTL" : "LTR";

const loadData = () => {
    isLoading = true;

    const options = {
        success: function (data) {
            if (data) {
                if (data && data.length > 0) {
                    table.addData(data);
                    currentPage = currentPage + 1;
                    isLoading = false;
                }
            }
        }
    };

    jqClient(options).Get("/DepartmentHoliday/GetAllHolidayDepartments".concat('?page=', currentPage));
};

$btnAddbutton.click(function () {
    sharedFn().InitialPageControls(uiControlItems);
});

const deleteData = (id) => {
    if (!id) return;

    const obj = table.getData().find(f => f.id == id);
    if (!obj) return;

    const deleteurl = "/DepartmentHoliday/DeleteDepartmentHoliday";

    notificationUtil.confirmation({
        title: sharedFn().GetUiControlText('WEB_WARNING_DELETE'),
        okText: sharedFn().GetUiControlText('WEB_DELETE_BUTTON'),
        cancelText: sharedFn().GetUiControlText('WEB_CANCEL')
    }, result => {
        if (!id) return;

        const options = {
            success: function (data) {
                if (data) {
                    if (data.responseStatus == '3') {
                        table.deleteRow(id);
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_DELETE'));
                    }
                    else {
                        notificationUtil.error(data.message);
                    }
                }
            },
            error: function (xhr) {
                notificationUtil.error(xhr.responseJSON.Message);
            }
        };
        jqClient(options).Post(deleteurl.concat('?Id=', id));
    });
};

$("#btn-submit").click(function (e) {
    if (sharedFn().NewvalidateForm("form-control",
        sharedFn().GetUiControlText('WEB_CNTRL_REQUIRED'),
        sharedFn().GetUiControlText('WEB_MSG_MAX_CHAR_LENGTH'),
        sharedFn().GetUiControlText('WEB_MSG_MIN_CHAR_LENGTH'))) {

        commonUtil.btnProgress(btnSubmitId);
        var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());

        const options = {
            success: function (response) {
                commonUtil.btnProgress(btnSubmitId, true);

                if (response.responseStatus == '1') {
                    table.addData([response], true);
                    table.deselectRow();
                    table.getRows()[0].select();
                    if (response) {
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_SAVE'));
                    }
                }
                else if (response.responseStatus == '2') {
                    table.updateData([response]);
                    if (response) {
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_UPDATE'));
                    }
                }
                else {
                    notificationUtil.error(response.message);
                }
                sharedFn().ViewMode();
            }
        };

        let url = '';
        let id = $('#Id').val();

        if (id) {
            url = "/DepartmentHoliday/UpdateDepartmentHoliday";
        } else {
            url = "/DepartmentHoliday/SaveDepartmentHoliday";
        }
        jqClient(options).PostFormData(url, requestdata);
    }
});

$(document).ready(function () {
    $formSection.hide();
});

initTables = () => {
    sharedFn().PopulateUiControl(controlsList);
    var TableColumns = sharedFn().PopulateColumn(columnList);

    if (IsAdd) {
        $btnAddbutton.show();
    }
    else {
        $btnAddbutton.hide();
    }

    table = tableUtil.createTabulator({
        id: gridContainerId,
        config: {
            textDirection: txtDir,
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,
    });

    loadData();
};
