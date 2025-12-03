
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let showMore = false, table = null, dialogElem = null;let currentPage = 0;let isSearch = false;let isLoading = true;

const btnAddContentId = 'btn-add-content',    btnSubmitId = "btn-submit",
    $formSection = $('#form-section'),    thumbnailId = "thumbnail";const gridContainerId = "view-container",
    $tblContentContainer = $('#tbl-template-container'),    $thumbnail = $('#' + thumbnailId),
    $btnAddContent = $('#' + btnAddContentId);
function ClearControlByPage() {
    sharedFn().SetValueToDropdown();
    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { EmailTemplateDocument } = data;

                    if (EmailTemplateDocument.length == 1) {
                        var entity = EmailTemplateDocument[0].templateDocId;
                        $('#EmailTemplateEmailTemplateDocument').attr("data-value", entity);
                        $('#EmailTemplateEmailTemplateDocument').val(entity).trigger('change');
                    }
                    else {
                        var entity = EmailTemplateDocument.map(x => x['templateDocId']);
                        $('#EmailTemplateEmailTemplateDocument').attr("data-value", entity);
                        $('#EmailTemplateEmailTemplateDocument').val(entity).trigger('change');
                    }


                }
            }
        }
    };
    jqClientAdvanced(options).Get("EmailTemplate/EmailTemplateDocument".concat('?emailtemplateid=', $("#Id").val()));
    
}

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

    jqClientAdvanced(options).Get("EmailTemplate/GetAllEmailTemplate".concat('?page=', currentPage));
};
$("#EmailTemplateServiceId").on("change", function () {
    var value = $(this).val();
    if (value) {
        const options = {
            success: function (result) {
                if (result) {
                    const { data } = result;
                    if (data) {
                        const { ServiceFields, EvaluationFields } = data;
                        const ddldrop1Data = ServiceFields.map(item => (
                            {
                                id: item.id,
                                text: txtDir === "RTL" ? item.nameAr : item.nameEn
                            }
                        ));
                        const ddldrop2Data = EvaluationFields.map(item => (
                            {
                                id: item.id,
                                text: txtDir === "RTL" ? item.nameAr : item.nameEn
                            }
                        ));
                        var $dropdown1 = $('#EmailTemplatefielsFromRequest');
                        var $dropdown2 = $('#EmailTemplateFielsFromEvaluation');
                        $dropdown1.empty();
                        $dropdown2.empty();
                        $dropdown1.select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddldrop1Data,
                            placeholder: sharedFn().GetUiControlText('EmailTemplatefielsFromRequest'),
                            dropdownCssClass: "manageselect2zindex"
                        })
                        $dropdown2.select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddldrop2Data,
                            placeholder: sharedFn().GetUiControlText('EmailTemplateFielsFromEvaluation'),
                            dropdownCssClass: "manageselect2zindex"
                        })
                        var datavalue1 = $dropdown1.attr("data-value");
                        if (datavalue1) {
                            if (!datavalue1.includes(',')) {
                                
                                $dropdown1.val(datavalue1).trigger('change');

                            }
                            else {
                                var entity = datavalue1.split(",").map(x => x);
                                $dropdown1.val(entity).trigger('change');

                            }
                           
                        }
                        else {
                            $dropdown1.val('').trigger('change');
                        }
                        var datavalue2 = $dropdown2.attr("data-value");
                        if (datavalue2) {
                            if (!datavalue2.includes(',')) {

                                $dropdown2.val(datavalue2).trigger('change');

                            }
                            else {
                                var entity = datavalue2.split(",").map(x => x);
                                $dropdown2.val(entity).trigger('change');

                            }

                        }
                        else {
                            $dropdown2.val('').trigger('change');
                        }

                    }
                }
            }
        };
        
        jqClientAdvanced(options).Get("EmailTemplate/GetAllFileFields".concat('?serviceid=', value));
    }
})
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
        jqClientAdvanced(options).Post("EmailTemplate/DeleteEmailTemplate".concat('?Id=', id));

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
            requestdata.append('EmailTemplateDocument', JSON.stringify($("#EmailTemplateEmailTemplateDocument").val()));         
        
            const options = {
                success: function (data) {
                    if (data) {
                        commonUtil.btnProgress(btnSubmitId, true);

                        var { responseStatus } = data;
                        //debugger
                        switch (responseStatus) {
                            case 1:

                                table.addData([data], true);
                                table.deselectRow();
                                table.getRows()[0].select();
                                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                                sharedFn().ViewMode();
                                break;

                            case 2:
                                table.updateData([data]);
                                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                sharedFn().ViewMode();
                                break;
                            case 12:
                                notificationUtil.error(sharedFn().GetUiControlText('EMAIL_TEMPLATE_BACKENDNAME_ALREADY_EXISTS'));
                                break;
                            case 14:
                                notificationUtil.error(sharedFn().GetUiControlText('SERVICE_FREEZED'));
                                break;

                            default:
                                notificationUtil.error(data.message);

                                break;
                        }
                       
                        $('#btn-submit').removeAttr("disabled");

                    }




                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "EmailTemplate/UpdateEmailTemplate";
            } else {
                url = "EmailTemplate/SaveEmailTemplate";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);
        }

    })
})   