
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let showMore = false, table = null, dialogElem = null;let currentPage = 0;let isSearch = false;let isLoading = true;

const btnAddContentId = 'btn-add-content',    btnSubmitId = "btn-submit",
    $formSection = $('#form-section'),    thumbnailId = "thumbnail";const gridContainerId = "view-container",
    $tblContentContainer = $('#tbl-template-container'),    $thumbnail = $('#' + thumbnailId),
    $btnAddContent = $('#' + btnAddContentId);


const loadData = (isSearch) => {
    isLoading = true;

    const options = {
        success: function (data) {
            if (data) {


                if (data && data.length > 0) {
                    if (isSearch) {
                        table.setData([]).then(function () {
                            setAllColumnWidths(table, columnWidths);
                        });
                        currentPage = 1;
                    }

                    table.addData(data).then(function () {
                        setAllColumnWidths(table, columnWidths);
                    });
                    currentPage = currentPage + 1;
                    isLoading = false;
                }
                else {
                    //currentPage = 0;
                }
            }
        }
    };

    jqClientAdvanced(options).Get("SMSTemplate/GetAllSMSTemplate".concat('?page=', currentPage));
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
        jqClientAdvanced(options).Post("SMSTemplate/DeleteSMSTemplate".concat('?Id=', id));

    });
};$(window).scroll(function () {    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {        if (!isLoading && currentPage > 0) {            loadData();        }    }});$(document).ready(function () {
          table = tableUtil.createTabulator({        id: gridContainerId,        config: {            textDirection: txtDir,            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),            movableRows: true,        },
        isResponsiveLayout: false,        uniqueRowId: 'id',
        sortColumn: "updateDate",        sortDir: "desc",        columns: TableColumns,
        columnResized: function (column) {

            // Get the resized column width
            var columnField = column.getField();
            var columnWidth = column.getWidth();
            columnWidths[columnField] = columnWidth;
        },           });


    dialogElem = commonUtil.createDailog({ dailogId: dailogId });    loadData();        document.getElementById(btnAddContentId).addEventListener('click', event => {        sharedFn().ClearForm();        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
    });
        $("#btn-submit").click(function (e) {
        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {

            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());
            requestdata.append('SMSTemplateDocument', JSON.stringify($("#SMSTemplateSMSTemplateDocument").val()));         
            const options = {
                success: function (response) {
                    if (response.data.stateStatus == false) {
                        notificationUtil.error(response.data.message);
                        commonUtil.btnProgress(btnSubmitId, true);
                        return;
                    }
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

                    else if (data.responseStatus == '14') {
                        notificationUtil.error(sharedFn().GetUiControlText('SERVICE_FREEZED'));
                        return;
                    }
                    else if (data.responseStatus == '12') {
                        notificationUtil.error(sharedFn().GetUiControlText('SMS_TEMPLATE_BACKENDNAME_ALREADY_EXISTS'));
                        return;
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
                url = "SMSTemplate/UpdateSMSTemplate";
            } else {
                url = "SMSTemplate/SaveSMSTemplate";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);
           
         
        }

    })
})   