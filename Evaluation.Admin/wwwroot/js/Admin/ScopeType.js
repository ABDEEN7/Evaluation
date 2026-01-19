let showMore = false, table = null, dialogElem = null, tree = null, selectedtree = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;


const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);

const loadData = (isScroll) => {
    isLoading = true;
    const options = {
        success: function (data) {

            if (isScroll) {
                showMore = true;
            } else {

                showMore = true;
            }
            if (data) {

                const _isInit = isScroll ? false : true;
                if (!data || data.length <= 0) {
                    showMore = false;

                    if (!isScroll) {
                        table.setData([]);
                    }
                    return;
                } else {
                    isLoading = false;
                    currentPage++;
                    showMore = true;
                    if (_isInit) {
                        table.setData(data);
                    } else {
                        table.addData(data);
                    }
                }
            }
        }
    };
    jqClientAdvanced(options).Get("ScopeType/GetAllScopeType".concat('?Page=', currentPage));

};



const deleteData = (id, event, cell) => {
    if (!id) return;

    const obj = table.getData().find(f => f.id == id);
    if (!obj) return;
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;
        const options = {
            success: function (data) {
                table.deleteRow(id);
                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
            }
        };
        jqClientAdvanced(options).Post("ScopeType/DeleteScopeType".concat('?Id=', id));

    });


};

$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData(true);
        }
    }
});

$(document).ready(function () {

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
        columns: TableColumns
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });
    loadData();



    $(`#${btnAddContentId}`).click(function (e) {

        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
    });


    $("#btn-submit").click(function (e) {


        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());


            const options = {
                success: function (response) {
                    commonUtil.btnProgress(btnSubmitId, true);

                    let { data } = response;
                    if (data.responseStatus == '1') {
                        table.addData([data], true);
                        table.deselectRow();
                        table.getRows()[0].select();
                        if (response) {
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));

                        }
                    }
                    else if (data.responseStatus == '2') {
                        table.updateData([data]);
                        if (response) {
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));

                        }

                    }
                    else {
                        notificationUtil.error(data.message);

                    }
                    sharedFn().ViewMode();
                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "ScopeType/UpdateScopeType";
            } else {
                url = "ScopeType/SaveScopeType";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);

        }
    });

});