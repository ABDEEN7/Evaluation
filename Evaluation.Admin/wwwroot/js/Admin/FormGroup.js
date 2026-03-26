
var isScroll = false;
var popupname = "";
var dynamicaction = "";
let FormGroupTypeList = [];
let FieldTypeList = [];
let SystemFieldList = [];
let PartyTypeList = [];
let DropDownTypeList = [];
let ParentDropDownFieldList = [];
let FieldList = [];
let FormGroupList = [];
let EvalFormList = [];
let FormGroupListByTypeList = [];
let DropDownValueList = [];
let AttributeList = [];
let fieldvalueselectedlist = '';
let fieldtabulatorcolumns = null;
let ClickedFormGroupType = 'FormGroup';
$('#submitBtn').click(function () {

    $("#firstbutton").addClass("active");
    $("#secondbutton").removeClass("active");
    LoadAllFormGroup('FormGroup');
    ClickedFormGroupType = 'FormGroup';
    getlookup();
});
function clickHandle(evt, animalName) {
    // Set the tab to be "active".
    const serviceId = services_div_select2.val();
    let tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    if (animalName == 'FormGroup') {
        ClickedFormGroupType = 'FormGroup';
        LoadAllFormGroup('FormGroup');
        const options6 = {
            success: function (result) {
                if (result) {
                    const { data } = result;
                    if (data) {
                        const { Field } = data;
                        FieldList = Field;
                    }
                }
            }
        };
        jqClientAdvanced(options6).Get("FormGroup/GetAllField".concat('?serviceid=', serviceId));
    }
    else {
        ClickedFormGroupType = 'List';
        LoadAllFormGroup('List');
        const options6 = {
            success: function (result) {
                if (result) {
                    const { data } = result;
                    if (data) {
                        const { Field } = data;
                        FieldList = Field;
                    }
                }
            }
        };
        jqClientAdvanced(options6).Get("FormGroup/GetAllFieldFormGroupList".concat('?serviceid=', serviceId));
    }
    evt.currentTarget.className += " active";
    
    
    
}
const LoadAllFormGroup = (FormGrouptype) => {

    const serviceId = services_div_select2.val();
    if (serviceId) {
        const options = {
            success: function (result) {
                if (result) {
                    $('#divaccordian').empty();
                    if (result && result.length > 0) {
                        FormGroupList = result;
                        
                        $('#divaccordian').append('<div id="accordion-container" class="ui-sortable"></div>');
                        createAccordion(result);
                       
                       
                        
                    }
                    $('#divFormGroup').show();
                }
            }
        };
        jqClientAdvanced(options).Get("FormGroup/GetAllFormGroup".concat('?ServiceId=', serviceId).concat('&FormGrouptype=', FormGrouptype));
    } else {
        $('#divFormGroup').hide();
    }
}
const createAccordion = (formGroupList) => {
    const $accordionContainer = $("#accordion-container"); // Ensure this element exists in your HTML
    $accordionContainer.empty(); // Clear existing content
    var Newbutton = sharedFn().GetUiControlText('ADD_NEW_FIELD');
    var viewItem = IsView_Field_ATTRIBUTE == "True" ?
        `<span class="Attr pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_ATTRIBUTE') + `"><i class="Attr fa fa-clipboard" onclick="FieldAttributeClick(this)"></i></span>`
        : '';
    var duplicateItem = IsView_Field_CONDITION == "True" ?
        `<span class="condition pointer" title="` + sharedFn().GetUiControlText('ADMIN_TOOLTIP_VIEW_CONDITION') + `"><i class="viewCondition fa fa-binoculars" onclick="FieldConditionClick(this)"></i></span>`
        : '';
    var dynamicaction = viewItem + duplicateItem;
    fieldtabulatorcolumns = sharedFn().PopulateColumn(FieldcolumnList, dynamicaction);
    formGroupList.forEach((item) => {
        var table = null;
        const tabId = `tab-${item.id}`;
        const contentId = `content-${item.id}`;
        const className = `card-body-${item.id}`;
        const tableId = `fieldTable-${item.id}`;
        const title = Lang === "ar" ? item.titleAr : item.titleEn;
       
 
        const $accordionItem = $(`
  <div class="card form-group-item mb-3" id="${item.id}">
    <a class="collapsed" data-bs-toggle="collapse" href="#${contentId}" role="button" aria-expanded="false" aria-controls="${contentId}">
      <div class="card-header" role="tab" id="${tabId}">
        <div class="row align-items-center">
          <div class="col">
            <h5 class="mb-0 text-white fs-18 font-r titleSearch">${title}</h5>
          </div>
          <div class="col-auto">
            ${IsEdit_FormGroup == "True"
                ? `<button onclick="FormGroupEdit(this)" 
                            
                            class="btn btn-sm edit-formGroup" 
                            data-group-id="${item.id}" 
                            data-action="edit" 
                            data-toggle="modal" 
                            data-target="#FormGroupModal">
                        <i class="fas fa-edit text-white"></i>
                    </button>` : ''
            }
            ${IsDelete_FormGroup == "True"
                ? `<button onclick="FormGroupDelete(this)" 
                            class="btn btn-sm delete-group" 
                            data-group-id="${item.id}">
                        <i class="fas fa-trash-can text-white"></i>
                    </button>` : ''
            }
          </div>
        </div>
      </div>
    </a>
    <div id="${contentId}" class="collapse" role="tabpanel" aria-labelledby="${tabId}" data-bs-parent="#accordion">
      <div class="${className} card-body">
        <div class="table-container" id="tableContainer_${item.id}">
          <div class="row">
            <div class="col-12">
              <div class="btns-group">
                ${IsEdit_Field == "True"
                ? `<button class="btn btn-sm btn-secondary" onclick="AddFieldRow('${tableId}', '${item.id}', '${title}')">
                        <i class="fa fa-plus-circle"></i> ${Newbutton}
                       </button>` : ''
            }
              </div>
            </div>
          </div>
          <div id="${tableId}"></div>
        </div>
      </div>
    </div>
  </div>
`);

        $accordionContainer.append($accordionItem);
      
        
        table=tableUtil.createTabulator({
            id: tableId,
            config: {
                textDirection: txtDir,
                paginationSize: 10,
                placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
                headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            },
            isResponsiveLayout: false,
            uniqueRowId: 'id',
            sortColumn: "updateDate",
            sortDir: "desc",
            columns: fieldtabulatorcolumns,
        });
       
        
        // Add new items to the div
        $(".action-items").append(viewItem).append(duplicateItem);
        loadFieldData(table, item.id);
        
    });
    var serviceid = $("#ServiceId").val();
    $("#accordion-container").sortable({
        axis: "y",
        handle: ".card-header",
        update: function (event, ui) {
            var newOrder = $("#accordion-container").sortable("toArray");

            var formGroups = newOrder.map(function (itemId, index) {

                return {
                    ServiceId: serviceid,
                    Id: itemId,
                    Order: index + 1
                };
            });
            const options = {
                success: function (data) {
                }
            };

            jqClientAdvanced(options).Post("FormGroup/UpdateFormGroupOrder", formGroups);

        }


    });
    
};


const loadFieldData = (table, formgroupid) => {
    IsEdit = IsEdit_Field;
    IsDelete = IsDelete_Field;

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

    jqClientAdvanced(options).Get("FormGroup/GetAllFormGroupFields".concat('?FormGroupId=', formgroupid));
};


function Loadtabledata() {
    var fieldid = $("#fieldid").val();
    if (popupname == "FieldAttribute") {
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

        jqClientAdvanced(options).Get("FormGroup/GetAllFieldAttribute".concat('?FieldId=', fieldid));
    }
    if (popupname == "FieldCondition") {
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

        jqClientAdvanced(options).Get("FormGroup/GetAllFieldCondition".concat('?FieldId=', fieldid));
    }
}

$('#btn-add-content-formgroup').click(function () {
    popupname = "FormGroup";
    
    var modaltitle = sharedFn().GetUiControlText('FormGroupHeader');
    sharedFn().OpenFormPopup(modaltitle, FormGroupcontrolvalidationlist);
    
});
function AddFieldRow(tableid, formgroupid,formgroupname) {
    
    
    table = Tabulator.prototype.findTable("#" + tableid)[0];
    controlvalidationlist = Fieldcontrolvalidationlist;
    popupname = "Field";
    var modaltitle = sharedFn().GetUiControlText('FieldHeader') + "  "+formgroupname;
    sharedFn().OpenFormPopup(modaltitle, Fieldcontrolvalidationlist);
    $("#formgroupid").val(formgroupid);
    
   
}
function CreateEditForFormGroup(pkId) {
    const obj = table.getData().find(f => f.id == pkId);
    if (popupname == "Field") {
        var title = $("#FieldFormGroup").text();
        var modaltitle = sharedFn().GetUiControlText('FieldHeader') + "  " + title;
        sharedFn().OpenFormPopup(modaltitle, Fieldcontrolvalidationlist, obj);
    }
    if (popupname == "FieldAttribute" || popupname == "FieldCondition") {
        var controlvalidationlist = (popupname == "FieldAttribute" ? FieldAttributecontrolvalidationlist : popupname == "FieldCondition" ? FieldConditioncontrolvalidationlist :null)
        $('#Id').val(obj.id);
        if (controlvalidationlist.length > 0) {
            controlvalidationlist.forEach(item => {
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
                }
                if (contrains.controlType == 'DROPDOWN') {
                    $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
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
    if (popupname == "FieldAttribute") {
        var attribute = $('#FieldAttributeAttributeKey').val();
        var exist = AttributeList.filter(x => x.id == attribute).length;
        if (exist > 0) {
            $('#FieldAttributeAttributeKeyDropdown').val(attribute).trigger('change');
            $('#FieldAttributeAttributeKey').attr("disabled", "disabled");
            
        }
        else {
            $('#FieldAttributeAttributeKeyDropdown').val('Others').trigger('change');
            $('#FieldAttributeAttributeKey').removeAttr("disabled");
            
        }
        $('#FieldAttributeAttributeKey').val(attribute);
    }
}

function FormGroupEdit(item) {
    var groupid = $(item).data('group-id');

    var objlist = FormGroupList.find(x => x.id == groupid);
    popupname = "FormGroup";
    var modaltitle = sharedFn().GetUiControlText('FormGroupHeader');
    sharedFn().OpenFormPopup(modaltitle, FormGroupcontrolvalidationlist, objlist);
    
}
function FormGroupDelete(item) {
    popupname = "FormGroup";
    const groupid = $(item).data('group-id');
    var urlname = "FormGroup/DeleteFormGroup";
    deleteData(groupid, urlname);
}
const deleteData = (id, urlname=null) => {
    if (urlname == null) {
        if (popupname == "Field") {
            //const obj = table.getData().find(f => f.id == id);
            //if (obj.formGroupCustomListId != null && obj.formGroupCustomListId != '') {
            //    notificationUtil.error(sharedFn().GetUiControlText('CUSTOM_FIELD_CANNOT_DELETE'));
            //    return;
            //}
            urlname = "FormGroup/DeleteField";
        }
        if (popupname == "FieldAttribute") {
            urlname = "FormGroup/DeleteFieldAttribute";
        }
        if (popupname == "FieldCondition") {
            urlname = "FormGroup/DeleteFieldCondition";
        }
    }
    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('ADMIN_WARNING_DELETE'), okText: sharedFn().GetUiControlText('DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('ADMIN_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {
                    if (popupname == "FormGroup") {
                        if (data.responseStatus == '3') {
                            LoadAllFormGroup(ClickedFormGroupType);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));

                        }
                        else if (data.responseStatus == '4') {
                            notificationUtil.error(sharedFn().GetUiControlText('FORMGROUP_CANNOT_BE_DELETED'));

                        }
                       
                    }
                    else if (popupname == "Field") {

                        if (data.responseStatus == '3') {
                            table.deleteRow(id);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
                        }
                        else if (data.responseStatus == '4') {
                            notificationUtil.error(sharedFn().GetUiControlText('FIELD_IN_FIELD_VIEW_CONDITION'));
                        }
                        else if (data.responseStatus == '17') {
                            notificationUtil.error(sharedFn().GetUiControlText('CUSTOM_FIELD_CANNOT_DELETE'));
                        }
                    }
                    else if (popupname == "FieldAttribute") {

                        if (data.responseStatus == '3') {
                            table.deleteRow(id);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
                        }
                       
                    }
                    else if (popupname == "FieldCondition") {

                        if (data.responseStatus == '3') {
                            table.deleteRow(id);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_DELETE'));
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
function SetPopupMode() {
    
    if (popupname == "FormGroup") {
        if (ClickedFormGroupType == "List") {
            $('#FormGroupFormGroupCustomList').parent().show();
            if ($("#Id").val() != "") {
                $('#FormGroupFormGroupCustomList').attr("disabled", "disabled");
                var datavalue = $('#FormGroupFormGroupCustomList').attr("data-value");
                if (datavalue) {
                    $('#FormGroupFormGroupCustomList').val(datavalue).trigger('change');
                }
                else {
                    $('#FormGroupFormGroupCustomList').val('').trigger('change');
                }
            }
            else {
                $('#FormGroupFormGroupCustomList').removeAttr("disabled");
            }
            
        }
        else {
            $('#FormGroupFormGroupCustomList').parent().hide();
        }
        var formgrouptypeid = FormGroupTypeList.find(x => x.backendName == ClickedFormGroupType).id;
        $("#FormGroupFormGroupType").val(formgrouptypeid);
        
    }
    if (popupname == "Field") {
        var FormGroupCustomListId = $("#FieldFormGroupCustomListId").val();
        if (FormGroupCustomListId != null && FormGroupCustomListId != '') {
            $('#FieldIsActive').prop('disabled', true);

        }
        

    }
}


function SetDropDown() {
   
    var controllist = popupname == "FormGroup" ? FormGroupcontrolvalidationlist : popupname == "Field" ? Fieldcontrolvalidationlist : popupname == "FieldCondition" ? FieldConditioncontrolvalidationlist : popupname == "FieldAttribute" ? FieldAttributecontrolvalidationlist : null;
   
    if (controllist) {
        var dropdownlist = controllist.filter(c => c.constraint.controlType == 'DROPDOWN' || c.constraint.controlType == 'MULTIDROPDOWN');
        if (dropdownlist.length > 0) {
            dropdownlist.forEach(item => {
                var constrain = item.constraint;
                
                if (constrain.controlName == "FormGroupId") {
                   
                  var  FormGroup= FormGroupList.map(dpitem => (
                        {
                            id: dpitem.id,
                            text: txtDir === "RTL" ? dpitem.titleAr : dpitem.titleEn
                        }
                    ));
                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: FormGroup,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),   
                    })
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "FieldTypeId") {
                    var ddldata = FieldTypeList.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: ddldata,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),   
                    }).on("change", event => {
                        var data = event.target.value;
                        if (data) {
                            var selectedvalue = FieldTypeList.find(x => x.id == data).backendName;
                            if (selectedvalue == "dropdown" || selectedvalue == "select2" || selectedvalue == "VacancySeat") {
                                showdropdown(true);
                                $("#FieldFormGroupList").parent().hide();
                                $("#FieldFormGroupList").val('').trigger('change');
                                $("#FieldEvalFormId").parent().hide();
                                $("#FieldEvalFormId").val('').trigger('change');
                            }
                            else if (selectedvalue == "list")
                            {
                                $("#FieldFormGroupList").parent().show();
                                showdropdown(false);
                            }
                            else if (selectedvalue == "Evl_Form") {
                                $("#FieldEvalFormId").parent().show();
                                showdropdown(false);
                            }
                            else {
                                $("#FieldFormGroupList").parent().hide();
                                $("#FieldFormGroupList").val('').trigger('change');
                                $("#FieldEvalFormId").parent().hide();
                                $("#FieldEvalFormId").val('').trigger('change');
                                showdropdown(false);
                               
                            }
                            
                        }
                        else {
                            showdropdown(false);
                            $("#FieldFormGroupList").parent().hide();
                            $("#FieldFormGroupList").val('').trigger('change');
                            $("#FieldEvalFormId").parent().hide();
                            $("#FieldEvalFormId").val('').trigger('change');
                        }
                        
                    }).on("select2:unselecting", function (e) {
                        showdropdown(false);
                        $("#FieldFormGroupList").parent().hide();
                        $("#FieldFormGroupList").val('').trigger('change');
                        $("#FieldEvalFormId").parent().hide();
                        $("#FieldEvalFormId").val('').trigger('change');

                    });
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "MappingFieldId" || constrain.controlName == "ReadFieldId") {
                    var ddldata = SystemFieldList.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.titleAr : item.titleEn
                        }
                    ));
                    var $dropdown = $('#' + constrain.uibackendName);
                    if (constrain.controlName == "MappingFieldId") {
                        $dropdown.select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddldata,
                            placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                            dropdownCssClass: "manageselect2zindex",
                            dropdownParent: $("#ModalPopup"),
                        }).on("change", event => {
                            var data = event.target.value;
                            if (data) {
                                //var fieldTypeId = SystemFieldList.find(x => x.id == data).fieldTypeId;
                                //if (fieldTypeId) {
                                //    $("#FieldFieldType").val(fieldTypeId).trigger('change');
                                //}
                            }
                        });
                    }
                    else {
                        $dropdown.select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddldata,
                            placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                            dropdownCssClass: "manageselect2zindex",
                            dropdownParent: $("#ModalPopup"),
                        })
                    }
                    
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "FieldPartyTypes") {

                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: PartyTypeList,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),   
                    })
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "DropDownTypeId") {

                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: DropDownTypeList,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),   
                    })
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "DropDownParentFieldId") {

                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: ParentDropDownFieldList,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup")   
                    })
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "ParentFieldId") {

                    var Field = FieldList.map(dpitem => (
                        {
                            id: dpitem.id,
                            text: txtDir === "RTL" ? dpitem.titleAr : dpitem.titleEn
                        }
                    ));
                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: Field,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),  
                    }).on("change", event => {
                        var data = event.target.value;
                        if (data) {
                          
                            var selectdata = FieldList.find(x => x.id == data);
                            var selectedvalue = selectdata.type;
                            var DropDownTypeId = selectdata.dropDownTypeId;
                            if (selectedvalue == "dropdown" || selectedvalue == "select2" || selectedvalue == "VacancySeat") {
                                $("#FieldConditionFieldValue").parent().hide();
                                $("#FieldConditionFieldDropDownValueIds").parent().show();
                                
                                const options = {
                                    success: function (result) {
                                        if (result) {
                                            const { data } = result;
                                            if (data) {
                                                const { FieldDropDownValueList } = data;
                                                 DropDownValueList = FieldDropDownValueList.map(dpitem => (
                                                    {
                                                        id: dpitem.id,
                                                        text: txtDir === "RTL" ? dpitem.titleAr : dpitem.titleEn
                                                    }
                                                ));
                                                var $dropdown = $("#FieldConditionFieldDropDownValueIds");
                                                if ($("#FieldConditionConditionOperator").val() == "in" || $("#FieldConditionConditionOperator").val() == "not in") {
                                                    $dropdown.attr('multiple', 'multiple');
                                                }
                                                else {
                                                    $dropdown.removeAttr('multiple');
                                                }
                                                $dropdown.empty();
                                                $dropdown.select2({
                                                    width: 'resolve',
                                                    allowClear: true,
                                                    data: DropDownValueList,
                                                    placeholder: sharedFn().GetUiControlText('FieldConditionFieldDropDownValueIds'),
                                                    dropdownCssClass: "manageselect2zindex",
                                                    dropdownParent: $("#ModalPopup")   
                                                })
                                                if (fieldvalueselectedlist) {
                                                    $dropdown.val(fieldvalueselectedlist).trigger('change');
                                                }
                                            }
                                        }
                                    }
                                };
                                jqClientAdvanced(options).Get("FormGroup/GetFieldDropDownValues".concat('?DropDownTypeId=', DropDownTypeId));

                            }
                            else {
                                $("#FieldConditionFieldValue").parent().show();
                                $("#FieldConditionFieldDropDownValueIds").parent().hide();
                            }
                        }
                        else {
                            $("#FieldConditionFieldValue").parent().show();
                            $("#FieldConditionFieldDropDownValueIds").parent().hide();
                        }
                        
                    });
                    $dropdown.val('').trigger('change');

                };

                if (constrain.controlName == "AttributeKeyId") {

                    var $dropdown = $('#' + constrain.uibackendName);
                    $dropdown.empty();
                    $dropdown.select2({
                        width: 'resolve',
                        allowClear: true,
                        data: AttributeList,
                        placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),
                    }).on("change", event => {
                        var data = event.target.value;
                        if (data) {
                            if (data == "Others") {
                                $('#FieldAttributeAttributeKey').val('');
                                $('#FieldAttributeAttributeKey').removeAttr("disabled");
                            }
                            else {
                                $('#FieldAttributeAttributeKey').val(data);
                                $('#FieldAttributeAttributeKey').attr("disabled", "disabled");
                            }
                        }

                    }).on("select2:unselecting", function (e) {
                        $('#FieldAttributeAttributeKey').val('');
                        $('#FieldAttributeAttributeKey').removeAttr("disabled");

                    });
                    $dropdown.val('').trigger('change');

                };
                if (constrain.controlName == "FormGroupListId") {
                    var serviceid = $("#ServiceId").val();
                    const options8 = {
                        success: function (result) {
                            if (result) {
                                FormGroupListByTypeList = result.map(item => (
                                    {
                                        id: item.id,
                                        text: txtDir === "RTL" ? item.titleAr : item.titleEn
                                    }
                                ));
                                var $dropdown = $('#' + constrain.uibackendName);
                                $dropdown.empty();
                                $dropdown.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: FormGroupListByTypeList,
                                    placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                                    dropdownCssClass: "manageselect2zindex",
                                    dropdownParent: $("#ModalPopup"),
                                });
                                var datavalue = $dropdown.attr("data-value");
                                if (datavalue) {
                                    $dropdown.val(datavalue).trigger('change');
                                }
                                else {
                                    $dropdown.val('').trigger('change');
                                }
                                
                            }
                        }
                    };
                    jqClientAdvanced(options8).Get("FormGroup/GetFormGroupListByTypeList".concat('?serviceid=', serviceid));
                    

                };
                if (constrain.controlName == "EvalFormId") {
                  
                       var ddldata = EvalFormList.map(item => (
                            {
                                id: item.id,
                               text: txtDir === "RTL" ? item.nameAr : item.nameEn
                            }
                        ));
                        var $dropdown = $('#' + constrain.uibackendName);
                        $dropdown.empty();
                        $dropdown.select2({
                            width: 'resolve',
                            allowClear: true,
                            data: ddldata,
                            placeholder: sharedFn().GetUiControlText(constrain.uibackendName),
                            dropdownCssClass: "manageselect2zindex",
                            dropdownParent: $("#ModalPopup"),
                        });
                        var datavalue = $dropdown.attr("data-value");
                        if (datavalue) {
                            $dropdown.val(datavalue).trigger('change');
                        }
                        else {
                            $dropdown.val('').trigger('change');
                        }

                    
                };
            });
        }
    }
    if (popupname == "FieldCondition") {
        $("#FieldConditionConditionOperator").on("change", event => {
            var data = event.target.value;
            if (data) {
                if ($("#FieldConditionParentField").val() != '') {
                    var selectdata = FieldList.find(x => x.id == $("#FieldConditionParentField").val());
                    if (selectdata) {
                        var selectedvalue = selectdata.type;

                        if (selectedvalue == "dropdown" || selectedvalue == "select2" || selectedvalue == "VacancySeat") {
                            if (data == "in" || data == "not in") {

                                var $FieldDropDown = $("#FieldConditionFieldDropDownValueIds");
                                if ($FieldDropDown.data('select2')) {
                                    $FieldDropDown.select2('destroy').select2();
                                }

                                $FieldDropDown.attr('multiple', 'multiple'); $FieldDropDown.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: DropDownValueList,
                                    placeholder: sharedFn().GetUiControlText('FieldConditionFieldDropDownValueIds'),
                                    dropdownCssClass: "manageselect2zindex",
                                    dropdownParent: $("#ModalPopup")   
                                })
                                if (fieldvalueselectedlist) {
                                    $FieldDropDown.val(fieldvalueselectedlist).trigger('change');
                                }
                            }
                            else {
                                var $FieldDropDown = $("#FieldConditionFieldDropDownValueIds");
                                if ($FieldDropDown.data('select2')) {
                                    $FieldDropDown.select2('destroy').select2();
                                }
                                $FieldDropDown.removeAttr('multiple');
                                $FieldDropDown.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: DropDownValueList,
                                    placeholder: sharedFn().GetUiControlText('FieldConditionFieldDropDownValueIds'),
                                    dropdownCssClass: "manageselect2zindex", dropdownParent: $("#ModalPopup"),
                                })
                                if (fieldvalueselectedlist) {
                                    $FieldDropDown.val(fieldvalueselectedlist).trigger('change');
                                }
                            }



                            $("#FieldConditionFieldValue").parent().hide();
                            $("#FieldConditionFieldDropDownValueIds").parent().show();
                        }
                        else {
                            $("#FieldConditionFieldValue").parent().show();
                            $("#FieldConditionFieldDropDownValueIds").parent().hide();

                        }
                    }
                   
                }
                
            }
        });

        $("#FieldConditionParentField").on("change", event => {
            var data = event.target.value;
            if (data) {
                var operatordata = $("#FieldConditionConditionOperator").val();
                if (operatordata != '') {
                    var selectdata = FieldList.find(x => x.id == data);
                    if (selectdata) {
                        var selectedvalue = selectdata.type;

                        if (selectedvalue == "dropdown" || selectedvalue == "select2") {
                            if (operatordata == "in" || operatordata == "not in") {

                                var $FieldDropDown = $("#FieldConditionFieldDropDownValueIds");
                                if ($FieldDropDown.data('select2')) {
                                    $FieldDropDown.select2('destroy').select2();
                                }

                                $FieldDropDown.attr('multiple', 'multiple'); $FieldDropDown.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: DropDownValueList,
                                    placeholder: sharedFn().GetUiControlText('FieldConditionFieldDropDownValueIds'),
                                    dropdownCssClass: "manageselect2zindex",
                                    dropdownParent: $("#ModalPopup")   
                                })
                                if (fieldvalueselectedlist) {
                                    $FieldDropDown.val(fieldvalueselectedlist).trigger('change');
                                }
                            }
                            else {
                                var $FieldDropDown = $("#FieldConditionFieldDropDownValueIds");
                                if ($FieldDropDown.data('select2')) {
                                    $FieldDropDown.select2('destroy').select2();
                                }
                                $FieldDropDown.removeAttr('multiple');
                                $FieldDropDown.select2({
                                    width: 'resolve',
                                    allowClear: true,
                                    data: DropDownValueList,
                                    placeholder: sharedFn().GetUiControlText('FieldConditionFieldDropDownValueIds'),
                                    dropdownCssClass: "manageselect2zindex", dropdownParent: $("#ModalPopup"),
                                })
                                if (fieldvalueselectedlist) {
                                    $FieldDropDown.val(fieldvalueselectedlist).trigger('change');
                                }
                            }



                            $("#FieldConditionFieldValue").parent().hide();
                            $("#FieldConditionFieldDropDownValueIds").parent().show();
                        }
                        else {
                            $("#FieldConditionFieldValue").parent().show();
                            $("#FieldConditionFieldDropDownValueIds").parent().hide();

                        }
                    }

                }

            }
        });
    }
    
}
function showdropdown(condition) {
    
    if (condition == true) {
        $("#FieldDropDownType").parent().show();
        $("#FieldDropDownParentField").parent().show();
        $("#FieldFormGroupList").parent().hide();
        $("#FieldFormGroupList").val('').trigger('change');
        $("#FieldEvalFormId").parent().hide();
        $("#FieldEvalFormId").val('').trigger('change');
    }
    else {
        $("#FieldDropDownType").parent().hide();
        $("#FieldDropDownParentField").parent().hide();
        $("#FieldDropDownType").val('').trigger('change');
        $("#FieldDropDownParentField").val('').trigger('change');
    }
    
}
function FieldAttributeClick(event) {
    const cellElem = event.closest('section');
    const fieldid = cellElem.getAttribute('data-key');
    const tableid = event.closest('.tabulator').getAttribute('id');
    const currenttable = Tabulator.prototype.findTable("#" + tableid)[0];
    const obj = currenttable.getData().find(f => f.id == fieldid);
    IsEdit = IsEdit_Field_ATTRIBUTE;
    IsDelete = IsDelete_Field_ATTRIBUTE;
    IsView = '';
    var fieldAttributetabulatorcolumns = sharedFn().PopulateColumn(FieldAttributecolumnList);   
    var modaltitle = sharedFn().GetUiControlText('FieldAttributeHeader') + " " + (Lang == "ar" ? obj.titleAr : obj.titleEn);

    sharedFn().OpenFormPopup(modaltitle, FieldAttributecontrolvalidationlist, null, fieldAttributetabulatorcolumns);
    $("#fieldid").val(fieldid);
    popupname = "FieldAttribute";
}

function FieldConditionClick(event) {
    const cellElem = event.closest('section');
    const fieldid = cellElem.getAttribute('data-key');
    const tableid = event.closest('.tabulator').getAttribute('id');
    const currenttable = Tabulator.prototype.findTable("#" + tableid)[0];
    const obj = currenttable.getData().find(f => f.id == fieldid);
    IsEdit = IsEdit_Field_CONDITION;
    IsDelete = IsDelete_Field_CONDITION;
    IsView = '';
   
    var fieldConditiontabulatorcolumns = sharedFn().PopulateColumn(FieldConditioncolumnList); 
    var modaltitle = sharedFn().GetUiControlText('FieldConditionHeader') + " " + (Lang == "ar" ? obj.titleAr : obj.titleEn);
    sharedFn().OpenFormPopup(modaltitle, FieldConditioncontrolvalidationlist, null, fieldConditiontabulatorcolumns);
    $("#fieldid").val(fieldid);
    popupname = "FieldCondition";
    
}
const getlookup = () => {
    var systemmoduleid = $("#SystemModuleId").val();
    var serviceid = $("#ServiceId").val();
    const options = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { FormGroupType } = data;
                    FormGroupTypeList = FormGroupType;
                   
                }
            }
        }
    };
    jqClientAdvanced(options).Get("FormGroup/GetAllFormGroupType");
    
    const options1 = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { FieldType } = data;
                    FieldTypeList = FieldType;
                    
                }
            }
        }
    };
    jqClientAdvanced(options1).Get("FormGroup/GetAllFieldType");
    const options2 = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { SystemField } = data;
                    SystemFieldList = SystemField.filter(x => x.serviceId != serviceid);
                    
                }
            }
        }
    };
    jqClientAdvanced(options2).Get("FormGroup/GetAllSystemField".concat('?systemmoduleid=', systemmoduleid));
    const options3 = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { PartyType } = data;
                    PartyTypeList = PartyType.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                }
            }
        }
    };
    jqClientAdvanced(options3).Get("FormGroup/GetAllPartyType".concat('?systemmoduleid=', systemmoduleid));
    const options4 = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { DropDownType } = data;
                    DropDownTypeList = DropDownType.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.titleAr : item.titleEn
                        }
                    ));
                }
            }
        }
    };
    jqClientAdvanced(options4).Get("FormGroup/GetAllDropDownType");
    if (ClickedFormGroupType == "FormGroup") {
        const options6 = {
            success: function (result) {
                if (result) {
                    const { data } = result;
                    if (data) {
                        const { Field } = data;
                        FieldList = Field;
                    }
                }
            }
        };
        jqClientAdvanced(options6).Get("FormGroup/GetAllField".concat('?serviceid=', serviceid));
    }
    else {
        const options6 = {
            success: function (result) {
                if (result) {
                    const { data } = result;
                    if (data) {
                        const { Field } = data;
                        FieldList = Field;
                    }
                }
            }
        };
        jqClientAdvanced(options6).Get("FormGroup/GetAllFieldFormGroupList".concat('?serviceid=', serviceid));
    }
    
    const options7 = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { Attribute } = data;
                    AttributeList = Attribute.map(item => (
                        {
                            id: item.nameAr,
                            text: item.nameAr
                        }
                    ));
                }
            }
        }
    };
    jqClientAdvanced(options7).Get("FormGroup/GetAllAttribute");
    
    const options8 = {
        success: function (result) {
            if (result) {
                var data = result.data;
                ParentDropDownFieldList = data.ParentDropDownField.map(item => (
                    {
                        id: item.id,
                        text: txtDir === "RTL" ? item.formGroup.titleAr + " _ " + item.titleAr : item.formGroup.titleEn + " _ " +item.titleEn
                    }
                ));
            }
        },
        error: function (xhr) {
        }
    };
    jqClientAdvanced(options8).Get("FormGroup/GetAllParentDropDownField".concat('?serviceid=', serviceid));

    const options9 = {
        success: function (result) {
            if (result) {
                const { data } = result;
                if (data) {
                    const { EvalForm } = data;
                    EvalFormList = EvalForm;
                }
            }
        }
    };
    jqClientAdvanced(options9).Get("FormGroup/GetAllEvalForm".concat('?systemmoduleid=', systemmoduleid));

}
function searchFormGroup() {
    var input, filter, accordionContainer, cards, card, i, title, tabulator;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    accordionContainer = document.getElementById("accordion-container");
    if (accordionContainer != null) {
        cards = accordionContainer.getElementsByClassName("card");
        for (i = 0; i < cards.length; i++) {
            card = cards[i];
            title = card.querySelector(".titleSearch");
            if (filter.length > 0) {
                if (title) {
                    var titleText = title.textContent || title.innerText;
                    titleText = titleText.toUpperCase();  // Convert to uppercase
                    filter = filter.toUpperCase();  // Convert to uppercase


                    var tab = card.getElementsByClassName("tabulator");
                    tabulator = $(tab).attr('id');
                    var tableelement = Tabulator.prototype.findTable("#" + tabulator)[0];

                    tableelement.setFilter(function (data, filter) {
                        return data.titleEn.toLowerCase().includes(filter.toLowerCase()) ||
                            data.titleAr.toLowerCase().includes(filter.toLowerCase());
                    }, filter);

                    var divcard = card.getElementsByClassName("collapse");
                    if (tableelement.getDataCount(true) > 0) {

                        $(divcard).addClass("show");
                        card.style.display = "";

                    } else {
                        $(divcard).removeClass("show");
                        card.style.display = "none";
                    }
                    if (titleText.indexOf(filter) > -1) {
                        card.style.display = "";
                    }
                }
            }
            else {
                card.style.display = "";
                var tab = card.getElementsByClassName("tabulator");
                tabulator = $(tab).attr('id');
                var tableelement = Tabulator.prototype.findTable("#" + tabulator)[0];
                tableelement.clearFilter();
            }


        }
    }
    




}
$('#btn-submit_popup').click(function () {
    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('ADMIN_CNTRL_REQUIRED'), sharedFn().GetUiControlText('ADMIN_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('ADMIN_MSG_MIN_CHAR_LENGTH'))) {
        commonUtil.btnProgress("btn-submit_popup");
        var requestdata = {};
        var url = '';
        if (popupname == "FormGroup") {
            
            requestdata = sharedFn().GetSaveObject(FormGroupcontrolvalidationlist, $('#Id').val());
            url = ($('#Id').val() != '' ? "FormGroup/UpdateFormGroup" : "FormGroup/SaveFormGroup");
        }
        else if (popupname == "Field") {
            requestdata = sharedFn().GetSaveObject(Fieldcontrolvalidationlist, $('#Id').val());
            url = ($('#Id').val() != '' ? "FormGroup/UpdateField" : "FormGroup/SaveField");
        }
        else if (popupname == "FieldAttribute") {
            requestdata = sharedFn().GetSaveObject(FieldAttributecontrolvalidationlist, $('#Id').val());
            url = ($('#Id').val() != '' ? "FormGroup/UpdateFieldAttribute" : "FormGroup/SaveFieldAttribute");
        }
        else if (popupname == "FieldCondition") {
            requestdata = sharedFn().GetSaveObject(FieldConditioncontrolvalidationlist, $('#Id').val());
            url = ($('#Id').val() != '' ? "FormGroup/UpdateFieldCondition" : "FormGroup/SaveFieldCondition");
        }

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
                    if (popupname == "FormGroup") {
                        if (data.responseStatus == '1' || data.responseStatus == '2') {
                            $("#ModalPopup").modal("hide");
                            $("#PopupForm").trigger("reset");
                            LoadAllFormGroup(ClickedFormGroupType);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                        }
                        else if (data.responseStatus == '4') { 
                            notificationUtil.error(sharedFn().GetUiControlText('BACKENDNAME_ALREADY_EXISTS'));
                        }
                    }
                    else if (popupname == "Field") {
                        if (data.responseStatus == '1') {
                            $("#ModalPopup").modal("hide");
                            $("#PopupForm").trigger("reset");

                            table.addData([data], true);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                        }
                        else if (data.responseStatus == '2') {
                            $("#ModalPopup").modal("hide");
                            $("#PopupForm").trigger("reset");

                            table.updateData([data]);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));

                        }
                        else if (data.responseStatus == '5') {
                            notificationUtil.error(sharedFn().GetUiControlText('RECORD_NOT_FOUND'));
                        }
                        else if (data.responseStatus == '4') {
                            notificationUtil.error(sharedFn().GetUiControlText('BACKENDNAME_ALREADY_EXISTS'));
                        }
                        else if (data.responseStatus == '11') {
                            notificationUtil.error(sharedFn().GetUiControlText('FIELD_IN_SAME_ROW_COLUMN'));
                        }
                    }
                    else if (popupname == "FieldAttribute") {
                        if (data.responseStatus == '1') {
                            sharedFn().ResetVisibleControls("PopupForm");
                            $('#FieldAttributeAttributeKey').removeAttr("disabled");
                            table.addData([data], true);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_SAVE'));
                        }
                        else if (data.responseStatus == '2') {

                            sharedFn().ResetVisibleControls("PopupForm");
                            $('#FieldAttributeAttributeKey').removeAttr("disabled");
                            table.updateData([data]);
                            notificationUtil.success(sharedFn().GetUiControlText('ADMIN_MSG_UPDATE'));

                        }
                        else if (data.responseStatus == '5') {
                            notificationUtil.error(sharedFn().GetUiControlText('RECORD_NOT_FOUND'));
                        }

                    }
                    else if (popupname == "FieldCondition") {

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
                        else if (data.responseStatus == '5') {
                            notificationUtil.error(sharedFn().GetUiControlText('RECORD_NOT_FOUND'));

                        }
                        fieldvalueselectedlist = '';
                        $("#FieldConditionFieldValue").val('');
                    }
                    else {
                        notificationUtil.error(data.message);
                    }

                }



            }
        };


        jqClientAdvanced(options).PostFormData(url, requestdata);
       
    }

    $("#FormGroupModal").modal("hide");
    $("#FormGroupForm").trigger("reset");

});

