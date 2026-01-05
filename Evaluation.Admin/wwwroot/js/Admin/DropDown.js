
let showMore = false, table = null, dialogElem = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let DropDownTypeList = [];

const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);

function ClearControlByPage() {
    getLookup();
    sharedFn().DisableDropdownOptions("DropDownParentDropDownId", $('#Id').val());
}
const getLookup = () => {

    if (controlvalidationlist || SearchuiControlItems) {
       
        //initializing FILEUPLOAD
        var dropdownlist = controlvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN');
        if (dropdownlist.length > 0) {
            dropdownlist.forEach(item => {
                var constrain = item.constraint;
                if (constrain.controlName == "DropDownTypeId") {
                    const options = {
                        success: function (result) {
                            if (result) {
                                const { data } = result;
                                if (data) {
                                    const { DropDownType } = data;
                                    DropDownTypeList = DropDownType;
                                    const ddlData = DropDownType.map(item => (
                                        {
                                            id: item.id,
                                            text: txtDir === "RTL" ? item.titleAr : item.titleEn
                                        }
                                    ));
                                    var $dropdown = $('#' + constrain.uibackendName);
                                    $dropdown.select2({
                                        allowClear: true,
                                        width: 'resolve',

                                        data: ddlData,
                                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                        dropdownCssClass: "manageselect2zindex",

                                    }).on("change", event => {
                                        var data = event.target.value;
                                        if (data) {
                                            var parentid = DropDownTypeList.find(x => x.id == data).parentId;
                                            LoadParentDropDown(parentid);
                                        }
                                        else {
                                            $("#DropDownParentDropDownId").empty();
                                        }

                                    }).on("select2:unselecting", function (e) {
                                        $("#DropDownParentDropDownId").empty();

                                    });
                                   
                                    $('#DropDownSearchDropDownType').select2({
                                        allowClear: true,
                                        width: 'resolve',

                                        data: ddlData,
                                        placeholder: sharedFn().GetUiControlText('DropDownSearchDropDownType'),
                                        dropdownCssClass: "manageselect2zindex",

                                    }).on("change", function () {
                                        currentPage = 0;
                                        isLoading = false;
                                        if (table) {
                                            table.setData([]);

                                        }

                                        loadData();

                                    });
                                    $("#DropDownSearchDropDownType").val('').trigger('change');
                                }
                            }
                        }
                    };
                    jqClientAdvanced(options).Get("DropDown/GetAllDropDownType");

                }
                
            });
        }
    }


};

function LoadParentDropDown(parentdropdowntype) {
    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { DropDown } = data;
                    const ddlData = DropDown.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.titleAr : item.titleEn
                        }
                    ));
                    var $dropdown = $('#DropDownParentDropDownId');
                    $dropdown.select2({
                        allowClear: true,
                        width: 'resolve',

                        data: ddlData,
                        placeholder: sharedFn().GetUiControlText('DropDownParentDropDownId'),
                        dropdownCssClass: "manageselect2zindex",

                    });
                    var datavalue = $("#DropDownParentDropDownId").attr('data-value');
                    if (datavalue) {
                        $("#DropDownParentDropDownId").val(datavalue).trigger('change');
                    }
                    else {
                        $("#DropDownParentDropDownId").val('').trigger('change');
                    }
                    
                }
            }
        }
    };
    jqClientAdvanced(options).Get("DropDown/GetDropDownByType".concat('?dropdowntype=', parentdropdowntype));

}


const loadData = (isScroll) => {
    var dropdowntype = $('#DropDownSearchDropDownType').val() == null ? "-1" : $('#DropDownSearchDropDownType').val();
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
    jqClientAdvanced(options).Get("DropDown/GetAllDropDown".concat('?dropdowntype=', dropdowntype).concat('&Page=', currentPage));

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
        jqClientAdvanced(options).Post("DropDown/DeleteDropDown".concat('?Id=', id));

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

    getLookup();
    
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
             jqClientAdvanced(options).PostFormData("DropDown/UpdateDropDownOrder", formData);

        }
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

    $(`#${btnAddContentId}`).click(function (e) {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().EnableDropdownOptions("DropDownParentDropDownId");
        sharedFn().SetDefaultValueFromConfig();
    });


    $("#btn-submit").click(function (e) {


        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());

         
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
                                notificationUtil.error(sharedFn().GetUiControlText('DROPDOWN_BACKENDNAME_ALREADY_EXISTS'));
                                $('#btn-submit').removeAttr("disabled");
                                break;
                            case 17:
                                notificationUtil.error(sharedFn().GetUiControlText('SAME_RECORD_CANNOT_ADD_AS_PARENT'));
                                $('#btn-submit').removeAttr("disabled");

                                break;

                            default:
                                notificationUtil.error(data.message);
                                $('#btn-submit').removeAttr("disabled");
                                break;
                        }
                       


                    }




                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "DropDown/UpdateDropDown";
            } else {
                url = "DropDown/SaveDropDown";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);

        }
    });

});



