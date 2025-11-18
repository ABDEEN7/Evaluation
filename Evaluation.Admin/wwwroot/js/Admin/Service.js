var popupname = "";
let showMore = false, table = null, dialogElem = null;
const dailogId = commonUtil.CONTENT_DAILOG_ID;
let currentPage = 0;
let isSearch = false;
let isLoading = true;
let AllStatusList = [];
let RequestFieldList = [];
let EvaluationFieldList = [];
let fieldvalueselectedlist = '';
const btnAddContentId = 'btn-add-content',
    btnSubmitId = "btn-submit",
    $formSection = $('#form-section')
    ;

const gridContainerId = "view-container",
    tblContentContainerId = "tbl-template-container",
    $tblContentContainer = $('#' + tblContentContainerId),
    $btnAddContent = $('#' + btnAddContentId);

IconPicker.Init({
    jsonUrl: '../lib/iconpicker/dist/iconpicker-1.5.0.json',
    searchPlaceholder: 'Search Icon',
    showAllButton: 'Show All',
    cancelButton: 'Cancel',
    noResultsFound: 'No results found.',
    borderRadius: '20px',
});




function CreateEditForFormGroup(pkId) {
    if (popupname == "PlaceHolder") {
        $tblContentContainer.show();
        $formSection.hide();
    }
    
    const obj = table.getData().find(f => f.id == pkId);
    if (Placeholdercontrolvalidationlist.length > 0) {
        Placeholdercontrolvalidationlist.forEach(item => {
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
                $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
            }
            if (contrains.controlType == 'TEXT_TINY') {
                tinyMCE.get(contrains.uibackendName).setContent(obj[fieldname]);
            }
            if (contrains.controlType == 'DATE') {
                var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                $('#' + contrains.uibackendName).val(date);
            }

        });
        var PlaceHolderFieldId = $('#PlaceHolderFieldId').attr("data-value");
        if (PlaceHolderFieldId) {
            $("#PlaceHolderFieldId").val(PlaceHolderFieldId).trigger('change');
        }
        else {
            $("#PlaceHolderFieldId").val('').trigger('change');
        }
        
        if (fieldvalueselectedlist) {
            $('#PlaceHolderChildFieldIds').val(fieldvalueselectedlist).trigger('change');
        }
        else {
            $("#PlaceHolderChildFieldIds").val('').trigger("change");
        }
    }
}
function ClearControlByPage() {
    sharedFn().SetValueToDropdown();
    
}



const loadData = (isSearch) => {
    if (popupname == "") {
        var SystemModuleId = $("#ServiceParentSystemModuleId").val();
        if (SystemModuleId) {
            SystemModuleId = SystemModuleId;
        }
        else {
            SystemModuleId = "-1";
        }
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

                }
            }
        }

    
    jqClientAdvanced(options).Get("Service/GetAllService".concat('?SystemModuleId=', SystemModuleId).concat('&page=', currentPage));
}
};


const deleteData = (id, urlname = null) => {
    if (urlname == null) {
        if (popupname == "PlaceHolder") {
            urlname = "Service/DeletePlaceHolder";
        }
        else {
            urlname = "Service/DeleteService";

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
                        if (popupname == "PlaceHolder") {
                            sharedFn().ResetVisibleControls("PopupForm");
                            
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
    var serviceid = $("#serviceid").val();
    if (popupname == "PlaceHolder") {
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

        jqClientAdvanced(options).Get("Service/GetAllPlaceHolder".concat('?serviceid=', serviceid));
    }
    
}

function PlaceHolderClick(event) {
    const cellElem = event.closest('section');
    const serviceid = cellElem.getAttribute('data-key');
    var maintable = Tabulator.prototype.findTable("#" + gridContainerId)[0];
    const obj = maintable.getData().find(f => f.id == serviceid);
    IsEdit = IsEdit_Service_PlaceHolder;
    IsDelete = IsDelete_Service_PlaceHolder;
    IsView = '';
    containsOrderNo = 'False';
    var PlaceHoldertabulatorcolumns = sharedFn().PopulateColumn(PlaceholdercolumnList);
    var modaltitle = sharedFn().GetUiControlText('PlaceholderHeader');
    var name = lang == "ar" ? obj.nameAr : obj.nameEn;
    var title = commonUtil.stringFormat(modaltitle, name);
    sharedFn().OpenFormPopup(title, Placeholdercontrolvalidationlist, null, PlaceHoldertabulatorcolumns, settingList);
    $("#serviceid").val(serviceid);
    $("#systemmoduleid").val(obj.systemModuleId);
    popupname = "PlaceHolder";
    
}
function LoadChildFields() {
    var FieldId = $("#PlaceHolderFieldId").val();
    $("#PlaceHolderChildFieldIds").parent().show();
    const options = {
        success: function (data) {
            if (data) {
                var { ChildFields } = data;
                const ddlData = ChildFields.map(item => (
                    {
                        id: item.id,
                        text: item.title
                    }
                ));

                $("#PlaceHolderChildFieldIds").select2({
                    width: 'resolve',
                    allowClear: true,
                    data: ddlData,
                    placeholder: sharedFn().GetUiControlText('PlaceHolderChildFieldIds'),
                    dropdownCssClass: "manageselect2zindex",
                    dropdownParent: $("#ModalPopup")
                })

                if (fieldvalueselectedlist) {
                    $('#PlaceHolderChildFieldIds').val(fieldvalueselectedlist).trigger('change');
                }
                else {
                    $("#PlaceHolderChildFieldIds").val('').trigger("change");
                }
               
            }
        }
    }


    jqClientAdvanced(options).Get("Service/GetAllChildFields".concat('?FieldId=', FieldId));
}
function SetDropDown (){

    if (Placeholdercontrolvalidationlist) {

        var dropdownlist = Placeholdercontrolvalidationlist.filter(c => c.constraint.controlType == 'DROPDOWN');
        if (dropdownlist.length > 0) {
            dropdownlist.forEach(item => {
                var constrain = item.constraint;
               if (constrain.controlName == "FieldId") {
                    const options = {
                        success: function (result) {
                            if (result) {
                              
                                const { RequestField, EvaluationField } = result;
                                    RequestFieldList = RequestField;
                                EvaluationFieldList = EvaluationField;
                                var $dropdownTarget = $('#' + constrain.uibackendName); 
                                $dropdownTarget.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: [],
                                    placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                    dropdownCssClass: "manageselect2zindex",
                                    dropdownParent: $("#ModalPopup"),
                                })
                                
                            }
                        }
                    };
                    jqClientAdvanced(options).Get("Service/GetAllFields".concat('?serviceid=', $("#serviceid").val()).concat('&systemmoduleid=', $("#systemmoduleid").val()));
                }
            });
        }
    }
    $("#PlaceHolderTypeDisplay").on("change", event => {
        var data = event.target.value;
        if (data) {
            $("#PlaceHolderFieldId").empty();
            if (data == "Request") {
                const ddlData = RequestFieldList.map(item => (
                    {
                        id: item.id,
                        text: item.title
                    }
                ));

                $("#PlaceHolderFieldId").select2({
                    width: 'resolve',
                    allowClear: true,
                    data: ddlData,
                    placeholder: sharedFn().GetUiControlText('PlaceHolderFieldId'),
                    dropdownCssClass: "manageselect2zindex",
                    dropdownParent: $("#ModalPopup")
                }).on("change", event => {
                    var data = event.target.value;
                    if (data) {
                        var type = RequestFieldList.find(x => x.id == data);
                        if (type) {
                            $('#PlaceHolderType').val(type.type);
                            if (type.fieldType == "list") {
                                LoadChildFields();
                            }
                            else {
                                $("#PlaceHolderChildFieldIds").empty();
                                $("#PlaceHolderChildFieldIds").parent().hide();
                            }
                        }
                        else {
                            $('#PlaceHolderType').val('');
                            $("#PlaceHolderChildFieldIds").empty();
                            $("#PlaceHolderChildFieldIds").parent().hide();
                        }
                    }
                    else {
                        $('#PlaceHolderType').val('');
                        $("#PlaceHolderChildFieldIds").empty();
                        $("#PlaceHolderChildFieldIds").parent().hide();
                    }
                });
                var PlaceHolderFieldId = $('#PlaceHolderFieldId').data("value");
                if (PlaceHolderFieldId) {
                    $("#PlaceHolderFieldId").val(PlaceHolderFieldId).trigger('change');
                }
                else {
                    $("#PlaceHolderFieldId").val('').trigger('change');
                }
             
                if (fieldvalueselectedlist) {
                    $('#PlaceHolderChildFieldIds').val(fieldvalueselectedlist).trigger('change');
                }
                else {
                    $("#PlaceHolderChildFieldIds").val('').trigger("change");
                }
                
            }
            else {
                $("#PlaceHolderChildFieldIds").empty();
                $("#PlaceHolderChildFieldIds").parent().hide();
                const ddlData = EvaluationFieldList.map(item => (
                    {
                        id: item.id,
                        text: item.title
                    }
                ));
                $("#PlaceHolderFieldId").select2({
                    width: 'resolve',
                    allowClear: true,
                    data: ddlData,
                    placeholder: sharedFn().GetUiControlText('PlaceHolderFieldId'),
                    dropdownCssClass: "manageselect2zindex",
                    dropdownParent: $("#ModalPopup")
                }).on("change", event => {
                    var data = event.target.value;
                    if (data) {
                        var type = EvaluationFieldList.find(x => x.id == data);
                        if (type) {
                            $('#PlaceHolderType').val(type.type);
                            
                        }
                        else {
                            $('#PlaceHolderType').val('');
                           
                        }
                    }
                    else {
                        $('#PlaceHolderType').val('');
                       
                    }
                });
                var PlaceHolderFieldId = $('#PlaceHolderFieldId').data("value");
                if (PlaceHolderFieldId) {
                    $("#PlaceHolderFieldId").val(PlaceHolderFieldId).trigger('change');
                }
                else {
                    $("#PlaceHolderFieldId").val('').trigger('change');
                }
               
                if (fieldvalueselectedlist) {
                    $('#PlaceHolderChildFieldIds').val(fieldvalueselectedlist).trigger('change');
                }
                else {
                    $("#PlaceHolderChildFieldIds").val('').trigger("change");
                }
                
            }

        }
        else {
            $("#PlaceHolderFieldId").empty();
            $('#PlaceHolderType').val('');
            $("#PlaceHolderChildFieldIds").empty();
            $("#PlaceHolderChildFieldIds").parent().hide();
        }

    });
    $("#PlaceHolderTypeDisplay").on("select2:unselecting", function (e) {
        $("#PlaceHolderFieldId").empty();
        $('#PlaceHolderType').val('');
        $("#PlaceHolderChildFieldIds").empty();
        $("#PlaceHolderChildFieldIds").parent().hide();

    });

}

function ServiceFreezeClick(event) {
    const cellElem = event.closest('section');
    const serviceid = cellElem.getAttribute('data-key');
    var modaltitle = sharedFn().GetUiControlText('ServiceFreezeTitle');
    const obj = table.getData().find(f => f.id == serviceid);
    sharedFn().OpenFormPopup(modaltitle, Freezecontrolvalidationlist, obj);
    $("#serviceid").val(serviceid);
    popupname = "ServiceFreeze";
    $("#btn-clear_popup").hide();
}
function DefaultSetUp() {
    $("#ServiceInitialservice").prop("checked", false);
}

function SetfreezedRow() {
    var rows = table.getRows();

    rows.forEach(row => {
        // Get the row's HTML element
        var rowElement = row.getElement();
        var count = row.getData().isFreez;

        // Get the first cell (first column)
        var spanElement = rowElement.querySelectorAll("span.actionnotificationconfigcount");


        if (spanElement) {
            $(spanElement).text(count);
        }
    });
}
$('#btn-submit_popup').click(function () {
    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
        commonUtil.btnProgress("btn-submit_popup");
        var requestdata = sharedFn().GetSaveObject(Placeholdercontrolvalidationlist, $('#Id').val());

        if (popupname == "ServiceFreeze") {
            var serviceid = $("#serviceid").val();
            if ($('#Id').val() == '') {
                $('#Id').val(serviceid);
            }
            var data = {
                Id: $('#Id').val(),
                IsFreez: $('#ServiceIsFreez').prop("checked")
            };
            var formData = new FormData();

            formData.append('request', JSON.stringify(data));
            const options = {
                success: function (response) {
                    if (response) {


                        if (response.responseStatus == '2') {
                            var tablerow = table.getRows()
                                .filter(row => row.getData().id == $('#Id').val())[0];
                            table.updateRow(tablerow, { isFreez: response.isFreez, isFreezDate: response.isFreezDate });
                            //var datanew = table.getData();
                            //table.setData(datanew);
                            if (response) {
                                notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                                $("#ModalPopup").modal("hide");
                                $("#PopupForm").trigger("reset");
                                commonUtil.btnProgress("btn-submit_popup", true);
                            }
                        }
                        else {
                            notificationUtil.error(data.message);
                        }
                    }
                }
            };

            jqClientAdvanced(options).PostFormData("Service/UpdateServiceIsFreeze", formData);
         

        }
        else {
            const options = {
                success: function (response) {
                    if (response.data.stateStatus == false) {
                        notificationUtil.error(response.data.message);
                        commonUtil.btnProgress("btn-submit_popup", true);
                        return;
                    }
                    commonUtil.btnProgress("btn-submit_popup", true);

                    let { data } = response;

                    if (response) {
                        if (data.responseStatus == '1') {
                            sharedFn().ResetVisibleControls("PopupForm");

                            table.addData([data], true);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                        }
                        else if (data.responseStatus == '2') {
                            sharedFn().ResetVisibleControls("PopupForm");

                            table.updateData([data]);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));

                        }
                        else if (data.responseStatus == '14') {
                            notificationUtil.success(sharedFn().GetUiControlText('SERVICE_FREEZED'));

                        }
                        else if (data.responseStatus == '12') {
                            notificationUtil.success(sharedFn().GetUiControlText('PLACEHOLDER_ALREADY_EXISTS'));

                        }
                        else {
                            notificationUtil.error(data.message);
                        }

                    }

                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "Service/UpdatePlaceholder";
            } else {
                url = "Service/SavePlaceholder";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);
           
        }
         
       
        
    }

   
});
$(window).scroll(function () {
    if ($(window).scrollTop() >= ($(document).height() - $(window).height()) * .60) {
        if (!isLoading) {
            loadData();
        }
    }
});
$("#ServiceParentSystemModuleId").on("change", function () {
    currentPage = 0;
    isLoading = false;
   
    if (table && popupname=='') {
        table.setData([]);

    }
    loadData();

});
$(document).ready(function () {
    var viewItem = IsView_Service_PlaceHolder == "True" ?
        `<span class="Placeholder pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_PLACEHOLDER') + `"><i class="Placeholder fa fa-address-book" onclick="PlaceHolderClick(this)">
</i></span>`
        : '';
    var freezeItem = IsFreeze == "True" ?
        `<span class="freeze pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_FREEZE') + `"><i class="freeze fa fa-snowflake" onclick="ServiceFreezeClick(this)">
</i></span>`
        : '';
    var dynamicitem = viewItem + freezeItem;
    let TableColumns = sharedFn().PopulateColumn(columnList, dynamicitem);

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
             jqClientAdvanced(options).PostFormData("Service/UpdateServiceOrder", formData);

        },
        rowFormatter: function (row) {
            var iconElement = row.getElement().querySelector("i.freeze");
            if (iconElement) {
                if (row.getData().isFreez === true) {

                    iconElement.classList.add("freezedclass");

                }
            else {
                    iconElement.classList.remove("freezedclass");
            }
        }
        }
    });
  
   
    dialogElem = commonUtil.createDailog({ dailogId: dailogId });
    if (controlvalidationlist) {

        //initializing ICON
        var iconlist = controlvalidationlist.filter(c => c.constraint.controlType == 'ICON');
        if (iconlist.length > 0) {

            iconlist.forEach(item => {
                var constrain = item.constraint;
                var id = '#' + constrain.uibackendName;
                IconPicker.Run(id, function (e) {
                    document.getElementById('IconPreview').className = document.getElementById(constrain.uibackendName).value;
                    var selectedIcon = document.getElementById(constrain.uibackendName).value;
                    if (selectedIcon) {
                        sharedFn().NewvalidateInput($('#' + constrain.uibackendName).attr('id'), sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'));


                    }
                });
            });
        }
    }
   
   
    
    document.getElementById(btnAddContentId).addEventListener('click', event => {
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
                success: function (response) {
                    commonUtil.btnProgress(btnSubmitId, true);

                    let { data } = response;
                    if (data.responseStatus == '1') {
                        table.addData([data], true);
                        if (response) {
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                            sharedFn().ViewMode();

                        }
                    }
                    else if (data.responseStatus == '2') {
                        table.updateData([data]);
                        if (response) {
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));
                            sharedFn().ViewMode();
                        }

                    }
                    else if (data.responseStatus == '12') {

                        if (response) {
                            notificationUtil.error(sharedFn().GetUiControlText('SERVICE_BACKENDNAME_ALREADY_EXISTS'));

                        }

                    }
                    else if (data.responseStatus == '13') {

                        if (response) {
                            notificationUtil.error(sharedFn().GetUiControlText('SERVICE_PREFIX_ALREADY_EXISTS'));

                        }

                    }
                    else if (data.responseStatus == '14') {

                        if (response) {
                            notificationUtil.error(sharedFn().GetUiControlText('SERVICE_FREEZED'));

                        }

                    }
                    else if (data.responseStatus == '4') {

                        if (response) {
                            notificationUtil.error(data.responseMessage);

                        }

                    }
                    else {
                        notificationUtil.error(data.message);

                    }
                    $('#btn-submit').removeAttr("disabled");
                }
            };

            let url = '';
            let id = $('#Id').val();

            if (id) {
                url = "Service/UpdateService";
            } else {
                url = "Service/SaveService";

            }
            jqClientAdvanced(options).PostFormData(url, requestdata);
         


        }
    });

   

});



