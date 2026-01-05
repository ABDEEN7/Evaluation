
let showMore = false, table = null, dialogElem = null;
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


function ClearControlByPage() {
    sharedFn().DisableDropdownOptions("DropDownTypeParentId", $('#Id').val());
}

    
$("#DropDownTypeParentSearch").on("change", function () {
    currentPage = 0;
    isLoading = false;
    if (table) {
        table.setData([]);

    }

    loadData();

});




const loadData = (isScroll) => {
    var parent = $('#DropDownTypeParentSearch').val() == null ? "-1" : $('#DropDownTypeParentSearch').val();
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
    jqClientAdvanced(options).Get("DropDownType/GetAllDropDownType".concat('?id=', parent).concat('&Page=', currentPage));

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
                     else if (data.responseStatus == '4') {
                        table.deleteRow(id);
                        notificationUtil.error(sharedFn().GetUiControlText('DROPDOWNTYPE_IS_USED'));
                    }
                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
        };
        jqClientAdvanced(options).Post("DropDownType/DeleteDropDownType".concat('?Id=', id));

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
        columns: TableColumns,
        
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

   
   





    $(`#${btnAddContentId}`).click(function (e) {
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().EnableDropdownOptions("DropDownTypeParentId");
        sharedFn().SetDefaultValueFromConfig();
    });


    $("#btn-submit").click(function (e) {


        if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {


            commonUtil.btnProgress(btnSubmitId);
            var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());
            const options = {
                success: function (response) {
                    if(response)
                    {
                        var data=response.data;
                        if(data)
                        {
                            var {responseStatus}=data;
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
                             case 8:
                            notificationUtil.error(sharedFn().GetUiControlText('DROPDOWNTYPE_ALREADY_USED'));
                            $('#btn-submit').removeAttr("disabled");
                            break;
                        case 12:
                            notificationUtil.error(sharedFn().GetUiControlText('DROPDOWNTYPE_BACKENDNAME_ALREADY_EXISTS'));
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
                    
                    

                   
                }
            };

            let url = '';
            let id = $('#Id').val();
         
            if (id) {
                url = "DropDownType/UpdateDropDownType";
            } else {
                url = "DropDownType/SaveDropDownType";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);
           


        }
    });



});



