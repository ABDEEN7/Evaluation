var popupname = "";
let showMore = false,  pageNumber = 0, table = null, searchParams = {}, dialogElem = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let AllStatusList = [];
let AllActionList = [];

const btnAddContentId = 'btn-add-content',
    searchPanelSectionId = "search-panel-section",
    progressBarAppendId = "scroll-progress-bar",
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);


$tblContentContainer.hide();

$('#submitBtn').click(function () {
    currentPage = 0;
    table.setData([]);
    loadData({ pageNum: pageNumber, serviceId: services_div_select2.val() }, false);

    commonUtil.infiniteScroll(null, () => {
        if (showMore) {
            pageNumber = pageNumber + 1;
            searchParams = {
                ...searchParams, pageNum: pageNumber, serviceId: services_div_select2.val()
            };

            loadData(searchParams, true);
        }
    });
    $tblContentContainer.show();
    $formSection.hide();
    $btnAddContent.removeAttr("disabled");
    GetAllActionList();
    getLookup();
});

function CreateEditForFormGroup(pkId) {
    if (popupname == "ActionStatusConfigNotification") {
        $tblContentContainer.show();
        $formSection.hide();
    }
    
    const obj = table.getData().find(f => f.id == pkId);
    if (ActionStatusConfigNotificationcontrolvalidationlist.length > 0) {
        ActionStatusConfigNotificationcontrolvalidationlist.forEach(item => {
            var contrains = item.constraint;
            var fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);
            if (contrains.controlType == 'TEXT_BOX') {
                $('#' + contrains.uibackendName).val(obj[fieldname]);
            }
            if (contrains.controlType == 'COLOR') {
                $('#' + contrains.uibackendName).val(obj[fieldname]);
                var colorpicker = '#' + contrains.uibackendName + 'picker';
                $(colorpicker).val(obj[fieldname]);
            }
            if (contrains.controlType == 'CHECK_BOX') {
                $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                $('#' + contrains.uibackendName).trigger("change");
            }
            if (contrains.controlType == 'DROPDOWN') {
                $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
                $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
            }
            if (contrains.controlType == 'MULTIDROPDOWN') {
                fieldvalueselectedlist = obj[fieldname];
                $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
            }
            if (contrains.controlType == 'TEXT_TINY') {
                tinyMCE.get(contrains.uibackendName).setContent(obj[fieldname]);
            }
            if (contrains.controlType == 'DATE') {
                var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                $('#' + contrains.uibackendName).val(date);
            }

        });
    }
}
function ClearControlByPage() {
    sharedFn().SetValueToDropdown();
    
}

const loadData = (reqData, isScroll) => {
    if (isScroll) {
        showMore = false;
        commonUtil.createLoader(progressBarAppendId, true);
    } else {
        Showloader(true);
    }

    jqClient.Post({
        url: "ActionStatusConfiguration/GetAllActionStatusConfiguration",

        config: {
            dataType: 'json'
        },
        jsonData: reqData,
        onError: (xhr) => {
            if (isScroll) {
                showMore = true;
                commonUtil.createLoader(progressBarAppendId);
            } else {
                Showloader(false);
            }
        },
        onSuccess: (data) => {
            if (isScroll) {
                showMore = true;
                commonUtil.createLoader(progressBarAppendId);
            } else {
                Showloader(false);
                showMore = true;
            }
            if (data) {
                //const { data } = JSON.parse(result);

                const _isInit = isScroll ? false : true;
                if (!data || data.length <= 0) {
                    showMore = false;
                    if (!isScroll) {
                        table.setData([]);
                    }
                    commonUtil.createLoader(progressBarAppendId, false, true);
                    return;
                } else {
                    showMore = true;
                    if (_isInit) {
                        table.setData(data).then(function () {
                            setAllColumnWidths(table, columnWidths);
                        });
                    } else {
                        table.addData(data)
                            .then(function () {
                                setAllColumnWidths(table, columnWidths);
                            });

                    }

                    var rows = table.getRows();

                                    rows.forEach(row => {
                                        // Get the row's HTML element
                                        var rowElement = row.getElement();
                                        var count = row.getData().notificationCount;

                                        // Get the first cell (first column)
                                        var spanElement = rowElement.querySelectorAll("span.actionnotificationconfigcount");


                                        if (spanElement) {
                                            $(spanElement).text(count);
                                        }
                                    });
                }
            }
        }
    });
};

const GetAllActionList = () => {
    const serviceId = services_div_select2.val();
    const options1 = {
        success: function (data) {
            if (data) {


                if (data && data.length > 0) {
                    AllActionList = data;
                }

            }
        }
    };

    jqClientAdvanced(options1).Get("ActionStatusConfiguration/GetAllActionList".concat('?ServiceId=', serviceId));
}

const deleteData = (id, urlname = null) => {
    if (urlname == null) {
        if (popupname == "ActionStatusConfigNotification") {
            urlname = "ActionStatusConfiguration/DeleteActionStatusConfigurationNotification";
        }
        else {
            urlname = "ActionStatusConfiguration/DeleteActionStatusConfiguration";

        }
    }
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {
                    if (data.responseStatus == '3') {
                        table.deleteRow(id);
                        notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
                        if (popupname == "ActionStatusConfigNotification") {
                            sharedFn().ResetVisibleControls("PopupForm");
                            SetDropDown();
                        }
                    }
                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
        };
        jqClientAdvanced(options).Post(urlname.concat('?Id=', id));

    });
};

function Loadtabledata() {
    var actionstatusconfigid = $("#actionstatusconfigid").val();
    if (popupname == "ActionStatusConfigNotification") {
        const options = {
            success: function (data) {
                if (data) {


                    if (data && data.length > 0) {
                        table.addData(data);

                    }
                    else {
                        table.setData([]);
                    }

                }
            }
        };

        jqClientAdvanced(options).Get("ActionStatusConfiguration/GetAllActionStatusConfigurationNotification".concat('?actionstatusconfigid=', actionstatusconfigid));
    }
    
}

function ActionStatusConfigClick(event) {
    const cellElem = event.closest('section');
    const actionstatusconfigid = cellElem.getAttribute('data-key');
    IsEdit = IsEdit_ActionStatusConfig_Notification;
    IsDelete = IsDelete_ActionStatusConfig_Notification;
    IsView = '';
    containsOrderNo = 'False';
    var ActionStatusConfigNotificationtabulatorcolumns = sharedFn().PopulateColumn(ActionStatusConfigNotificationcolumnList);
    var modaltitle = sharedFn().GetUiControlText('ActionStatusConfigNotificationHeader');
    
    sharedFn().OpenFormPopup(modaltitle, ActionStatusConfigNotificationcontrolvalidationlist, null, ActionStatusConfigNotificationtabulatorcolumns, settingList);
    $("#actionstatusconfigid").val(actionstatusconfigid);
    popupname = "ActionStatusConfigNotification";
   
}
function DefaultSetUp() {
    $("#ActionStatusConfigurationIsRemark").prop("checked", false);
    $("#ActionStatusConfigurationIsRemark").trigger("change");
    $("#ActionStatusConfigurationIsOtherAttachment").prop("checked", false);
    $("#ActionStatusConfigurationIsOtherAttachment").trigger("change");
    $("#ActionStatusConfigurationIsRemarkRequired").prop("checked", false);
    $("#ActionStatusConfigurationIsOtherAttachmentRequired").prop("checked", false);
    $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
    

}
$("#ActionStatusConfigurationServiceActionId").on("change", function () {
    
    if ($("#ActionStatusConfigurationServiceActionId").val() != "") {
        var Actiontype = AllActionList.find(x => x.id == $("#ActionStatusConfigurationServiceActionId").val());
        if (Actiontype) {
            if (Actiontype.bakendName == "ASSIGN" || Actiontype.bakendName == "APPROVE_AND_ASSIGN") {
                $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().show();
            }
            else {
                $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().hide();
                $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
            }
        }
        else {
            $("#ActionStatusConfigurationShowIsDefaultAssigner").parent().parent().parent().hide();
            $("#ActionStatusConfigurationShowIsDefaultAssigner").prop("checked", false);
        }
    }
})

function SetPopupCount() {
    // Get the Tabulator instance
    const tableInstance = Tabulator.prototype.findTable("#" + gridContainerId)[0];

    if (tableInstance) {
        // Get the object from the Tabulator table data that matches the input value
        const targetId = $("#ActionStatusConfigurationNotificationConfigId").val();
        const targetObj = tableInstance.getData().find(f => f.id == targetId);

        if (targetObj) {
            // Update the notificationCount
            const tableCount = table.getDataCount();
            targetObj.notificationCount = tableCount;

            // Update the row data directly in Tabulator
            const row = tableInstance.getRow(targetId);
            if (row) {
                row.update({ notificationCount: tableCount });

                // Optional: Update the HTML manually if you have custom rendering
                const rowElement = row.getElement();
                const spanElements = rowElement.querySelectorAll("span.actionnotificationconfigcount");

                spanElements.forEach(span => {
                    $(span).text(tableCount);
                });
            }
        }
    }


}
function SetDropDown() {

    $("#ActionStatusConfigurationNotificationIsEmailSend").prop("checked", false); 
    $("#ActionStatusConfigurationNotificationIsMessageSend").prop("checked", false);   
    $("#ActionStatusConfigurationNotificationIsNotificationSend").prop("checked", false);
    
    $("#ActionStatusConfigurationNotificationIsEmailSend").on("change", function () {

        if (this.checked) {
            $("#ActionStatusConfigurationNotificationEmailTemplateId").parent().show();
        }
        else {
            $("#ActionStatusConfigurationNotificationEmailTemplateId").val('').trigger("change");
            $("#ActionStatusConfigurationNotificationEmailTemplateId").parent().hide();

        }
    })
    $("#ActionStatusConfigurationNotificationIsEmailSend").trigger("change");
    $("#ActionStatusConfigurationNotificationIsMessageSend").on("change", function () {

        if (this.checked) {
            $("#ActionStatusConfigurationNotificationSMSTemplateId").parent().show();
        }
        else {
            $("#ActionStatusConfigurationNotificationSMSTemplateId").val('').trigger("change");
            $("#ActionStatusConfigurationNotificationSMSTemplateId").parent().hide();

        }
    })
    $("#ActionStatusConfigurationNotificationIsMessageSend").trigger("change");
    $("#ActionStatusConfigurationNotificationIsNotificationSend").on("change", function () {

        if (this.checked) {
            $("#ActionStatusConfigurationNotificationNotificationTemplateId").parent().show();
        }
        else {
            $("#ActionStatusConfigurationNotificationNotificationTemplateId").val('').trigger("change");
            $("#ActionStatusConfigurationNotificationNotificationTemplateId").parent().hide();

        }
    })
    $("#ActionStatusConfigurationNotificationIsNotificationSend").trigger("change");
    sharedFn().SetValueToDropdown();
}

$("#ActionStatusConfigurationIsRemark").on("change", function () {
    $("#ActionStatusConfigurationRemarkLabelEn").val('');
    $("#ActionStatusConfigurationRemarkLabelAr").val('');
    $("#ActionStatusConfigurationIsRemarkRequired").prop("checked", false);
    if (this.checked) {
        $("#ActionStatusConfigurationRemarkLabelEn").parent().show();
        $("#ActionStatusConfigurationRemarkLabelAr").parent().show();
        $("#ActionStatusConfigurationIsRemarkRequired").parent().parent().parent().show();
    }
    else {
        $("#ActionStatusConfigurationRemarkLabelEn").parent().hide();
        $("#ActionStatusConfigurationRemarkLabelAr").parent().hide();
        $("#ActionStatusConfigurationIsRemarkRequired").parent().parent().parent().hide();

    }
})
$("#ActionStatusConfigurationIsOtherAttachment").on("change", function () {
    $("#ActionStatusConfigurationAttachmentLabelAr").val('');
    $("#ActionStatusConfigurationAttachmentLabelEn").val('');
    $("#ActionStatusConfigurationIsOtherAttachmentRequired").prop("checked", false);

    if (this.checked) {
        $("#ActionStatusConfigurationAttachmentLabelAr").parent().show();
        $("#ActionStatusConfigurationAttachmentLabelEn").parent().show();
        $("#ActionStatusConfigurationIsOtherAttachmentRequired").parent().parent().parent().show();
    }
    else {
       
        $("#ActionStatusConfigurationAttachmentLabelAr").parent().hide();
        $("#ActionStatusConfigurationAttachmentLabelEn").parent().hide();
        $("#ActionStatusConfigurationIsOtherAttachmentRequired").parent().parent().parent().hide();

    }
})
$('#btn-submit_popup').click(function () {
    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
        commonUtil.btnProgress("btn-submit_popup");
        var requestdata = sharedFn().GetSaveObject(ActionStatusConfigNotificationcontrolvalidationlist, $('#Id').val());
        var url = ($('#Id').val() != '' ? "ActionStatusConfiguration/UpdateActionStatusConfigurationNotification" : "ActionStatusConfiguration/SaveActionStatusConfigurationNotification");

       
        const options = {
            success: function (data) {
                if (data) {
                    commonUtil.btnProgress("btn-submit_popup", true);
                        var { responseStatus } = data;
                        //debugger
                        switch (responseStatus) {
                            case 1:

                                sharedFn().ResetVisibleControls("PopupForm");
                                SetDropDown();
                                table.addData([data], true);
                                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                                break;

                            case 2:
                                sharedFn().ResetVisibleControls("PopupForm");
                                SetDropDown();
                                table.updateData([data]);
                                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                break;
                            
                            default:
                                notificationUtil.error(data.message);

                                break;
                        }
                   

                }




            }
        };

        jqClientAdvanced(options).PostFormData(url, requestdata);
    }

   
});
$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData();
        }
    }
});

const searchColsDef = () => {
    return [
        {
            field: 'serviceActionId',
            header: sharedFn().GetUiControlText('ActionStatusConfigurationServiceActionId'),
            type: 'DROPDOWN',
            collections: []
        },
        {
            field: 'currentStatusId',
            header: sharedFn().GetUiControlText('ActionStatusConfigurationCurrentStatusId'),
            type: 'DROPDOWN',
            collections: []
        },
        {
            field: 'nextStatusId',
            header: sharedFn().GetUiControlText('ActionStatusConfigurationNextStatusId'),
            type: 'DROPDOWN',
            collections: []
        },



    ];
};
const btnSeachEvent = (searchEvent) => {
    pageNumber = 0;
    const { data, event, params } = searchEvent;
    if ($.isEmptyObject(data)) {

        notificationUtil.error(sharedFn().GetUiControlText('SEARCHVALIDATION'));
        return;
    }
    const reqData = { ...data, ...params, pageNum: pageNumber, serviceId: services_div_select2.val() };
    searchParams = reqData;
    loadData(reqData, false);
};

const btnClearEvent = (event) => {
    searchParams = {};
    pageNumber = 0;
    loadData({ pageNum: pageNumber, serviceId: services_div_select2.val() }, false);
};
const columnSearch = (ctrlId, colDef, params) => {
    const searchPanel = columnSearchUtil.createColumnSearch({
        ctrlId, colDef, params,
        actions: [
            { btnId: 'btn-seach-clear', text: sharedFn().GetUiControlText('CLEAR_BUTTON'), eventName: btnClearEvent, type: 'CLEAR' },
            { btnId: 'btn-seach-start', text: sharedFn().GetUiControlText('SEARCH_BUTTON'), eventName: btnSeachEvent, type: 'SEARCH' }
        ]
    });

    $('#' + searchPanelSectionId).empty().append(searchPanel);


};

const getLookup = () => {
    const serviceId = services_div_select2.val();
    const options = {
        success: function (data) {
            
                
                if (data) {
                    
                    const ddlData = data.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    const ddlElm = document.querySelector(`[data-key="serviceActionId"]`);
                    if (ddlElm) {
                        const ddlId = ddlElm.getAttribute('id');
                        $('#' + ddlId).empty();
                        $('#' + ddlId).select2({
                            width: '100%',
                            dir: "ltr",
                            dropdownAutoWidth: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('ActionStatusConfigurationServiceActionId'),
                            allowClear: true,
                            dropdownCssClass: "manageselect2zindex"
                        });
                        $('#' + ddlId).val('').trigger('change');
                    }

                }
            
        }
    };
    jqClientAdvanced(options).Get("ActionStatusConfiguration/GetActionList".concat('?ServiceId=', serviceId));
    const options1 = {
        success: function (data) {
            
                if (data) {

                    const ddlData = data.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    const ddlElm = document.querySelector(`[data-key="currentStatusId"]`);
                    if (ddlElm) {
                        const ddlId = ddlElm.getAttribute('id');
                        $('#' + ddlId).empty();
                        $('#' + ddlId).select2({
                            width: '100%',
                            dir: "ltr",
                            dropdownAutoWidth: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('ActionStatusConfigurationCurrentStatusId'),
                            allowClear: true,
                            dropdownCssClass: "manageselect2zindex"
                        });
                        $('#' + ddlId).val('').trigger('change');
                    }
                    const ddlElm1 = document.querySelector(`[data-key="nextStatusId"]`);
                    if (ddlElm1) {
                        const ddlId = ddlElm1.getAttribute('id');
                        $('#' + ddlId).empty();
                        $('#' + ddlId).select2({
                            width: '100%',
                            dir: "ltr",
                            dropdownAutoWidth: true,
                            data: ddlData,
                            placeholder: sharedFn().GetUiControlText('ActionStatusConfigurationNextStatusId'),
                            allowClear: true,
                            dropdownCssClass: "manageselect2zindex"
                        });
                        $('#' + ddlId).val('').trigger('change');
                    }

                }
           
        }
    };
    jqClientAdvanced(options1).Get("ActionStatusConfiguration/GetStatusList".concat('?ServiceId=', serviceId));
};
$(document).ready(function () {
    columnSearch('search-panel-userrole', searchColsDef(), {});
    var viewItem = IsView_ActionStatusConfig_Notification == "True" ?
        `<span class="Attr pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_NOTIFICATION') + `"><i class="Attr fa fa-bell" onclick="ActionStatusConfigClick(this)">
        <span class="actionnotificationconfigcount">0</span>
</i></span>`
        : '';
    let TableColumns = sharedFn().PopulateColumn(columnList, viewItem);
    //$("#ActionStatusConfigurationServiceId").parent().hide();
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
             jqClientAdvanced(options).PostFormData("ActionStatusConfiguration/UpdateActionStatusConfigurationOrder", formData);

        }
    });

    dialogElem = commonUtil.createDailog({ dailogId: dailogId });

   
   
    
    $(`#${btnAddContentId}`).click(function (e) {
        table = Tabulator.prototype.findTable("#" + gridContainerId)[0];
        sharedFn().ClearForm();
        sharedFn().EditMode();
        sharedFn().SetDefaultValueFromConfig();
        DefaultSetUp();
        sharedFn().SetValueToDropdown();

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
                                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                                    sharedFn().ViewMode();
                                    break;

                                case 2:
                                    table.updateData([data]);
                                    notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                    sharedFn().ViewMode();
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
                url = "ActionStatusConfiguration/UpdateActionStatusConfiguration";
            } else {
                url = "ActionStatusConfiguration/SaveActionStatusConfiguration";

            }

            jqClientAdvanced(options).PostFormData(url, requestdata);

        }
    });

   

});



