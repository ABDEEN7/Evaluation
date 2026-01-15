const dailogId = commonUtil.CONTENT_DAILOG_ID;
let showMore = false, table = null, tableCondition=null, dialogElem = null;
let currentPage = 0;
let isSearch = false;
let isLoading = true;

const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section'),
    thumbailId = "thumbnail";

const gridContainerId = "view-container",
    $tblContentContainer = $('#tbl-template-container'),
    $thumbnail = $('#' + thumbailId),
    $btnAddContent = $('#' + btnAddContentId);

const loadData = (isSearch) => {
    isLoading = true;

    const options = {
        success: function (data) {
            if (data) {
                if (data && data.length > 0) {
                    if (isSearch) {
                        table.setData([]).then(function () {

                        });
                        currentPage = 1;
                    }
                    table.addData(data).then(function () {
                        setAllColumnWidths(table, columnWidths);
                    });
                    currentPage = currentPage + 1;
                    isLoading = false;
                }
            }
        }
    };
    jqClientAdvanced(options).Get("Team/GetAllTeam".concat('?page=', currentPage));
};
const deleteData = (id) => {
    if (!id) return;

    const obj = table.getData().find(f => f.id == id);
    if (!obj) return;


    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {


                    if (data.responseStatus == '3') {
                        table.deleteRow(id);
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
                    }

                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
        };
        jqClientAdvanced(options).Post("Team/DeleteTeam".concat('?Id=', id));

    });
};



$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading && currentPage > 0) {
            loadData();
        }
    }
});
function ClearControlByPage() {
    $("#UserTeamScopeRelationbutton").hide();
    if (IsView_UserTeamScope == "True") {
        $("#UserTeamScopeRelationdiv").show();
        $("#UserTeamScopeRelationtabulator").css("pointer-events", "");
        IsEdit = '';
        IsDelete = IsDelete_UserTeamScope;
        IsView = '';
        let ConditionTableColumns = sharedFn().PopulateColumn(UserTeamScopecolumnList);
        tableCondition = tableUtil.createTabulator({
            id: "UserTeamScopeRelationtabulator",
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
            columns: ConditionTableColumns,

        });
        tableCondition.addRow({
            id: 0,
            scopeId: null,
            userId: null,
            isActive: true,
            updateBy: "",
            updateDate: ""
        });

        //const options = {
        //    success: function (data) {
        //        if (data) {


        //            if (data && data.length > 0) {
        //                tableCondition.addData(data);

        //            }
        //            else {
        //                //tableCondition.setData([]);
        //            }

        //        }
        //    }
        //};

        //jqClientAdvanced(options).Get("Team/GetAllUserTeamScope".concat('?reminderid=', reminderid));
    }
    else {
        $("#UserTeamScopeRelationdiv").hide();
    }
    $("#UserTeamScopeRelationdiv").parent().show();
}
$(document).ready(function () {
    IsEdit = IsEdit_Team;
    IsDelete = IsDelete_Team;
    IsView = IsView_Team;
    let TableColumns = sharedFn().PopulateColumn(columnList);


    table = tableUtil.createTabulator({
        id: gridContainerId,
        config: {
            textDirection: txtDir,
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        isResponsiveLayout: false,
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,
        columnResized: function (column) {

            // Get the resized column width
            var columnField = column.getField();
            var columnWidth = column.getWidth();
            columnWidths[columnField] = columnWidth;
        },
        rowMoved: function (row) {
            var request = [];
            table.getData().map(function (d, index) {

                request.push({
                    "Id": d.id,
                    "OrderNo": index
                });
            });
            var formData = new FormData();
            //debugger
            formData.append('OrderObj', JSON.stringify(request));
            const options = {
                success: function (data) {
                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                }
            };
            jqClientAdvanced(options).PostFormData("Team/UpdateTeamOrder", formData);

        }

    });


    dialogElem = commonUtil.createDailog({ dailogId: dailogId });
    loadData();


    $(`#${btnAddContentId}`).click(function (e) {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        $("#UserTeamScopeRelationdiv").parent().hide();
        $("#UserTeamScopeRelationbutton").hide();
    });
    

    $("#btn-submit").click(function (e) {
        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {

            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());

            const RESPONSE_STATUS = {
                CREATE: 1,
                UPDATE: 2
            };

            const options = {
                success: function (data) {

                    // Fail fast
                    if (!data) {
                        notificationUtil.error('Invalid server response');
                        $('#btn-submit').removeAttr('disabled');
                        return;
                    }

                    commonUtil.btnProgress(btnSubmitId, true);

                    const responseStatus = data?.responseStatus;

                    switch (responseStatus) {

                        case RESPONSE_STATUS.CREATE:
                            table.addData([data], true);
                            table.deselectRow();
                            table.getRows()[0]?.select();
                            notificationUtil.success(
                                sharedFn().GetUiControlText('ADMIN_MSG_SAVE')
                            );
                            sharedFn().ViewMode();
                            break;

                        case RESPONSE_STATUS.UPDATE:
                            table.updateData([data]);
                            notificationUtil.success(
                                sharedFn().GetUiControlText('ADMIN_MSG_UPDATE')
                            );
                            sharedFn().ViewMode();
                            break;

                        default:
                            notificationUtil.error(
                                data.responseMessage || 'Unexpected response status'
                            );
                            break;
                    }

                    $('#btn-submit').removeAttr('disabled');
                },

                error: function () {
                    notificationUtil.error('Request failed. Please try again.');
                    $('#btn-submit').removeAttr('disabled');
                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "Team/UpdateTeam";
            } else {
                url = "Team/SaveTeam";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);
        }

    })


})   