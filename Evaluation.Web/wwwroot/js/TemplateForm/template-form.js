
let table = null, currentPage = 0, isLoading = true, popupname = "",
formItems = [];
const gridContainerId = "view-container",
    $tblContentContainer = $('#tbltemplatecontainer'),
    $formSection = $('#formsection'),
    $itemcontent = $('#itemcontent'),
    $formContent = $('#formcontent'),
    $btnAddbutton = $('#btnaddcontent'),
    $btnAddParent = $('#btnAddParent'),
    btnSubmitId = "btn-submit",
    btnpopupSubmitId = "btn-submit_popup";
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

    jqClient(options).Get("/EvaluationForm/GetAllEvalForm".concat('?page=', currentPage));
};

$btnAddbutton.click(function () {
    sharedFn().InitialPageControls(uiControlItems);
    $itemcontent.hide();
    $('#Id').val('');
});
function ClearControlByPage() {
    if (popupname == "FormScope") {
        var id = $('#PopupId').val();
        const obj = table.getData().find(f => f.id == id);
        if (!obj) return;
        if (typeof FormScopecontrolvalidationlist !== 'undefined') {
            var controlvalidationlist = FormScopecontrolvalidationlist;
            if (controlvalidationlist.length > 0) {
                var othercontrollist = controlvalidationlist.filter(c => c.constraint.controlType != 'DUAL_LIST');
                var duallistcontrollist = controlvalidationlist.filter(c => c.constraint.controlType == 'DUAL_LIST');
                othercontrollist.forEach(item => {
                    var contrains = item.constraint;
                    var fieldname = item.controlName.charAt(0).toLowerCase() + item.controlName.slice(1);
                    if (contrains.controlType == 'COLOR') {
                        $('#' + contrains.uibackendName).val(obj[fieldname]);
                        var colorpicker = '#' + contrains.uibackendName + 'picker';
                        $(colorpicker).val(obj[fieldname]);
                    }
                    else if (contrains.controlType == 'CHECK_BOX' || item.constraint.controlType == "CHECK_BOX_HIDDEN" || item.constraint.controlType == "RADIO_BUTTON") {
                        $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                        $('#' + contrains.uibackendName).trigger("change");
                    }
                    else if (contrains.controlType == 'CHECK_BOX_DISABLED') {
                        $('#' + contrains.uibackendName).prop("checked", obj[fieldname] ?? false);
                        $('#' + contrains.uibackendName).attr("disabled", "disbaled");
                    }
                    else if (contrains.controlType == 'DROPDOWN') {
                        $('#' + contrains.uibackendName).val(obj[fieldname]).trigger('change');
                        $('#' + contrains.uibackendName).attr("data-value", obj[fieldname]);
                    }
                    else if (contrains.controlType == 'TAGS') {
                        if (obj[fieldname]) {
                            var tags = obj[fieldname].split(',');
                            tags.forEach(item => {
                                var newTags = new Option(item, item, true, true);
                                $('#' + contrains.uibackendName).append(newTags).trigger('change');
                            });
                        }
                    }
                    else if (contrains.controlType == 'MULTIDROPDOWN') {
                        if (obj[fieldname]) {
                            if (obj[fieldname].length == 1) {
                                var entity = obj[fieldname][0];
                                $('#' + contrains.uibackendName).val(entity).trigger('change');
                                $('#' + contrains.uibackendName).attr("data-value", entity);

                            }
                            else {
                                var entity = obj[fieldname].map(x => x);
                                $('#' + contrains.uibackendName).val(entity).trigger('change');
                                $('#' + contrains.uibackendName).attr("data-value", entity);

                            }
                        }

                    }
                    else if (contrains.controlType == 'TEXT_TINY') {
                        obj[fieldname] = obj[fieldname] == null ? "" : obj[fieldname];
                        tinyMCE.get(contrains.uibackendName).setContent(obj[fieldname]);
                    }
                    else if (contrains.controlType == 'DATE') {
                        var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD);
                        $('#' + contrains.uibackendName).val(date);
                    }
                    else if (contrains.controlType == 'DATETIME') {
                        var date = sharedFn().GetActionDate(obj[fieldname], commonUtil.DATE_FORMAT.DASH_YYYY_MM_DD_HH_mm_ss);
                        $('#' + contrains.uibackendName).val(date);
                    }
                    else if (contrains.controlType == 'FILEUPLOAD') {
                        if (obj[fieldname]) {
                            var labelid = '#' + contrains.uibackendName + '_label';
                            var labelfield = fieldname + "_UiFileName";
                            var imageid = '#' + contrains.uibackendName + '_image';
                            var imagefield = fieldname + "_BlobURL";
                            $(labelid).html(obj[labelfield]);
                            $(imageid).attr("src", obj[imagefield]);
                        }
                    }
                    else if (contrains.controlType == 'FILEUPLOAD_ATTACH') {
                        if (obj[fieldname]) {
                            var labelid = '#' + contrains.uibackendName + '_label';
                            var labelfield = fieldname + "_UiFileName";
                            var imageid = '#' + contrains.uibackendName + '_image';
                            var imagefield = fieldname + "_BlobURL";
                            $(labelid).html(obj[labelfield]);
                            $(imageid).attr("src", obj[imagefield]);
                        }
                    }
                    else if (contrains.controlType == 'DROPZONE') {
                        if (obj[fieldname]) {
                            $('#thumbnail').empty();
                            var labelfield = fieldname + "_UiFileName";
                            var imagefield = fieldname + "_BlobURL";
                            if (obj[labelfield]) {
                                const icon = commonUtil.getFileIcon(obj[labelfield]);

                                if (icon.trim() === commonUtil.FILES_TYPE.IMAGE) {

                                    var img = `<div class="card">
                                                <img src="${obj[imagefield]}" class="card-img-top" style="max-height:200px">
                                                        <div class="card-body"><p class="card-text">${obj[labelfield]}</p></div>
                                                </div>
                                        `;

                                    $('#thumbnail').append(img);
                                }
                                else if (icon.trim() === commonUtil.FILES_TYPE.PDF) {
                                    var title = $('<div>').addClass('title').text(obj[labelfield]);
                                    $('#thumbnail').append(icon).append(title);
                                }
                                else if (icon.trim() === commonUtil.FILES_TYPE.VIDEO) {

                                    var vedio = `<div class="card">
                                                        <video src="${obj[imagefield]}" class="card-img-top"  type = "video/mp4" style="max-height:200px"></video>
                                                        <div class="card-body"><p class="card-text">${obj[labelfield]}</p></div>
                                                </div>
                                        `;

                                    $('#thumbnail').append(vedio);

                                }
                                else {
                                    var title = $('<div>').addClass('title').text(obj?.uiFileName);
                                    $('#thumbnail').append(icon).append(title);

                                };
                                $('#thumbnail').addClass('review');
                                $('#thumbnail').data('url', `${obj[imagefield]}`);
                                $('#thumbnail').data('name', `${obj[labelfield]}`);
                            }

                            $('#dropzonejs').hide();
                        }
                    }
                    else if (contrains.controlType == 'TEXT_BOX_DISABLED' || contrains.controlType == 'NUMBER_DISABLED' || contrains.controlType == 'TEXT_BOX_DISABLED_COPY') {
                        $('#' + contrains.uibackendName).val(obj[fieldname]);
                        $('#' + contrains.uibackendName).attr("disabled", "disbaled");
                    }

                    else if (contrains.controlType == 'CHECK_BOX_LIST') {
                        var checkboxlist = obj[fieldname];
                        if (checkboxlist) {
                            checkboxlist.forEach(function (value) {
                                $('#' + contrains.uibackendName + ' input[type="checkbox"][value="' + value + '"]').prop('checked', true);
                            });
                        }

                    }
                    else {
                        $('#' + contrains.uibackendName).val(obj[fieldname]);
                    }
                });
                if (duallistcontrollist.length > 0) {
                    processDualListControls(duallistcontrollist, obj).then(() => {
                    });

                }
            }
        }
    }
    else {
        if (IsViewFormItem) {
            $itemcontent.show();
            if (IsAddFormItem) {
                $btnAddParent.show();
            }
            else {
                $btnAddParent.hide();
            }
        }
        else {
            $itemcontent.hide();
        }

        BindFormItem();
    }
    
    
}
$("#btnAddParent").click(function () {
    popupname = "EvalFormItem";
    var EvalformId = $("#Id").val();
    $("#evalformidvalue").val(EvalformId);
    var modaltitle = sharedFn().GetUiControlText('FormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, FormItemcontrolvalidationlist, null, null, null);

});
function FormScopeClick(event) {
    const cellElem = event.closest('section');
    const Evalformid = cellElem.getAttribute('data-key');
    IsEdit = IsEditFormScope;
    IsDelete = IsDeleteFormScope;
    IsView = '';
    containsOrderNo = 'False';
    var FormScopetabulatorcolumns = sharedFn().PopulateColumn(FormScopecolumnList);
    var modaltitle = sharedFn().GetUiControlText('FormScopeHeader');

    sharedFn().OpenFormPopup(modaltitle, FormScopecontrolvalidationlist, null, FormScopetabulatorcolumns, settingList);
    $("#formIdValue").val(Evalformid);
    popupname = "FormScope";
}
function Loadtabledata() {
    var formIdValue = $("#formIdValue").val();
    if (popupname == "FormScope") {
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

        jqClient(options).Get("/EvaluationForm/GetAllFormScope".concat('?formIdValue=', formIdValue));
    }

}
function SetPopupMode() {
    if (popupname == "EvalFormItem") {
        if ($("#EvalFormsHasEvaluation").prop("checked")) {
            $("#EvalFormItemIsEvaluation").prop("checked", true);
            $("#EvalFormItemDropDownTypeId").val('').trigger('change');
            $("#EvalFormItemDropDownTypeId").parent().hide();
        }
        else {
            $("#EvalFormItemIsEvaluation").prop("checked", false);
            $("#EvalFormItemDropDownTypeId").parent().show();
        }
        if ($("#EvalFormItemIsEvaluation").prop("checked")) {
            $("#EvalFormItemDropDownTypeId").val('').trigger('change');
            $("#EvalFormItemDropDownTypeId").parent().hide();
        }
        else {
            $("#EvalFormItemDropDownTypeId").parent().show();
        }
        $("#EvalFormsHasEvaluation").on("change", function () {
            if (this.checked) {
                $("#EvalFormItemIsEvaluation").prop("checked", true);
                $("#EvalFormItemDropDownTypeId").val('').trigger('change');
                $("#EvalFormItemDropDownTypeId").parent().hide();
            }
            else {
                $("#EvalFormItemIsEvaluation").prop("checked", false);
                $("#EvalFormItemDropDownTypeId").parent().show();
            }
        })

        $("#EvalFormItemIsEvaluation").on("change", function () {
            if (this.checked) {
                $("#EvalFormItemDropDownTypeId").val('').trigger('change');
                $("#EvalFormItemDropDownTypeId").parent().hide();
            }
            else {
                $("#EvalFormItemDropDownTypeId").parent().show();
            }
        })
    }
}
function SetDropDown() {
    if (popupname == "EvalFormItem") {
        var EvalformId = $("#Id").val();
        const options = {
            success: function (result) {
                if (result) {

                    const ddlData = result.map(item => (
                        {
                            id: item.id,
                            text: txtDir === "RTL" ? item.nameAr : item.nameEn
                        }
                    ));
                    var $dropdown = $('#EvalFormItemFormItemRelated');
                    $dropdown.empty();
                    $dropdown.select2({
                        allowClear: true,
                        width: '100%',
                        multiple: true,
                        data: ddlData,
                        placeholder: sharedFn().GetUiControlText('EvalFormItemFormItemRelated'),
                        dropdownCssClass: "manageselect2zindex",
                        dropdownParent: $("#ModalPopup"),
                    });
                    var raw = $dropdown.attr("data-value");   // NOT .data()

                    if (raw) {
                        var values = raw.split(",");         // convert CSV → array

                        // Trim spaces (important)
                        values = values.map(x => x.trim());

                        // Set to Select2
                        $dropdown.val(values).trigger("change.select2");
                    }
                    else {
                        $dropdown.val(null).trigger("change.select2");
                    }

                    

                }
            }
        };
        jqClient(options).Get("/EvaluationForm/GetAllFormItemsFromDepartment".concat('?EvalformId=', EvalformId));
        //$("label[for='EvalFormItemNoteRequired']").hide();

        //$("#EvalFormItemNoteRequired").parent().hide();
        toggleNoteRequired();
        $("#EvalFormItemHasNote").on("change", function () {
            toggleNoteRequired();
        });
    }
    
}

function BindFormItem() {
    $("#formitemTable tbody").empty();
    var EvalformId = $("#Id").val();
    const options = {
        success: function (response) {
            formItems = response;
            renderFormItemTable();
        }
    };
    jqClient(options).Get("/EvaluationForm/GetAllEvalFormItems".concat('?EvalformId=', EvalformId));
}
function renderFormItemTable() {
    let html = "";

    formItems.forEach((p, pIndex) => {
        const jsonString = JSON.stringify(p);
        var actionButtons = "";
        if (IsAddSubFormItem) {
            actionButtons += `<button type='button' class="btn btn-sm btn-link addSub p-0" data-id="${p.id}"><i class="las la-plus-square"></i></button>`;
        }
        if (IsEditFormItem) {
            actionButtons += ` <button type="button" class="btn btn-sm btn-link editParent p-0" data-id="${p.id}"><i class="las la-edit"></i></button>`;
        }
        if (IsDeleteFormItem) {
            actionButtons += `<button type='button' class="btn btn-sm btn-link deleteParent p-0"  data-id="${p.id}"><i class="las la-trash"></i></button>`;
        }
        html += `
        <tr class="parent-row" data-id="${p.id}">
            <td class="toggle">➖</td>
            <td>
            ${actionButtons}
            </td>
            <td>
               ${p.nameAr}
            </td>
            <td>
                ${p.nameEn}
            </td>
            <td style="display:none">
                ${jsonString}
            </td>
        </tr>`;
        p.subFormItems.forEach((c, cIndex) => {
            var actionButtons = "";
            if (IsEditSubFormItem) {
                actionButtons += `<button type="button" class="btn btn-sm btn-link editChild p-0" data-id="${c.id}"><i class="las la-edit"></i></button>`;
            }
            if (IsDeleteSubFormItem) {
                actionButtons += ` <button type='button' class="btn btn-sm btn-link deleteChild p-0" data-id="${c.id}"><i class="las la-trash"></i></button>`;
            }
            const jsonString = JSON.stringify(c);
            html += `
            <tr class="child-row" data-parent="${p.id}" data-id="${c.id}">
                <td></td>
                <td> 
                   ${actionButtons}
               </td>
                <td style="padding-right:30px">
                    ${c.nameAr}
                </td>
                <td>
                   ${c.nameEn}
                </td>
                 <td style="display:none">
                   ${jsonString}
                </td>

            </tr>`;
        });
    });

    $("#formitemTable tbody").html(html);
}
$(document).on("click", ".toggle", function () {
    let parentId = $(this).closest("tr").data("id");
    let children = $(`tr[data-parent='${parentId}']`);

    if (children.is(":visible")) {
        children.hide();
        $(this).text("➕");
    } else {
        children.show();
        $(this).text("➖");
    }
});

// FormItem delete button
$(document).on("click", ".deleteParent", function () {
    var id = $(this).data("id"); 
    popupname = 'EvalFormItem';
    deleteData(id);
});

// SubFormItem delete button
$(document).on("click", ".deleteChild", function () {
    var id = $(this).data("id");
    popupname = 'EvalSubFormItem';
    deleteData(id);
});

// FormItem Edit button
$(document).on("click", ".editParent", function () {
    var row = $(this).closest("tr");
    var id = $(this).data("id");  
    var jsonString = row.find("td:last").text();
    var objdata = JSON.parse(jsonString);
    popupname = "EvalFormItem";
    var modaltitle = sharedFn().GetUiControlText('FormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, FormItemcontrolvalidationlist, objdata, null, null);
    $("#PopupId").val(id);
});

// SubFormItem Add button
$(document).on("click", ".addSub", function () {
    var id = $(this).data("id");
    popupname = "EvalSubFormItem";
    var modaltitle = sharedFn().GetUiControlText('SubFormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, SubFormItemcontrolvalidationlist, null, null, null);
    $("#evalformitemidvalue").val(id);
});

// SubFormItem Edit button
$(document).on("click", ".editChild", function () {
    var row = $(this).closest("tr");
    var id = $(this).data("id");
    var jsonString = row.find("td:last").text();
    var objdata = JSON.parse(jsonString);
    popupname = "EvalSubFormItem";

    var modaltitle = sharedFn().GetUiControlText('SubFormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, SubFormItemcontrolvalidationlist, objdata, null, null);
    $("#PopupId").val(id);
});
const deleteData = (id) => {
    if (!id) return;
    var deleteurl = '';
    if (popupname == 'EvalFormItem') {
        deleteurl = "/EvaluationForm/DeleteEvaluationFormItem";
    }
    else if (popupname == 'EvalSubFormItem') {
        deleteurl = "/EvaluationForm/DeleteEvaluationSubFormItem";
    }
    else if (popupname == 'FormScope') {
        deleteurl = "/EvaluationForm/DeleteFormScope";
    }
    else {
        const obj = table.getData().find(f => f.id == id);
        if (!obj) return;

        deleteurl = "/EvaluationForm/DeleteEvaluationForm";
    }

    


    notificationUtil.confirmation({ title: sharedFn().GetUiControlText('WEB_WARNING_DELETE'), okText: sharedFn().GetUiControlText('WEB_DELETE_BUTTON'), cancelText: sharedFn().GetUiControlText('WEB_CANCEL') }, result => {
        if (!id) return;



        const options = {
            success: function (data) {
                if (data) {


                    if (data.responseStatus == '3') {
                        if (popupname == 'EvalFormItem' || popupname == 'EvalSubFormItem') {
                            $("#formitemTable tr[data-id='" + id + "']").remove();
                        }
                        else {
                            table.deleteRow(id);
                        }
                       
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_DELETE'));
                    }

                    else {
                        notificationUtil.error(data.message);
                    }
                }
            }
            ,
            error: function (xhr) {
                notificationUtil.error(xhr.responseJSON.Message);
                
            }
        };
        jqClient(options).Post(deleteurl.concat('?Id=', id));

    });
};
$("#btn-submit").click(function (e) {

   
    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('WEB_CNTRL_REQUIRED'), sharedFn().GetUiControlText('WEB_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('WEB_MSG_MIN_CHAR_LENGTH'))) {


        commonUtil.btnProgress(btnSubmitId);
        var requestdata = sharedFn().GetSaveObject(controlvalidationlist, $('#Id').val());
        const options = {
            success: function (response) {
                commonUtil.btnProgress(btnSubmitId, true);

                
                if (response.responseStatus == '1') {
                    table.addData([response], true);
                   
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
                table.refreshFilter();
            }
        };

        let url = '';
        let id = $('#Id').val();

        if (id) {
            url = "/EvaluationForm/UpdateEvaluationForm";
        } else {
            url = "/EvaluationForm/SaveEvaluationForm";

        }
        jqClient(options).PostFormData(url, requestdata);



    }
});

function GetformItemrow(response) {
    const jsonString = JSON.stringify(response);
    var actionButtons = "";
    if (IsAddSubFormItem) {
        actionButtons += `<button type='button' class="btn btn-sm btn-link addSub p-0" data-id="${response.id}"><i class="las la-plus-square"></i></button>`;
    }
    if (IsEditFormItem) {
        actionButtons += ` <button type="button" class="btn btn-sm btn-link editParent p-0" data-id="${response.id}"><i class="las la-edit"></i></button>`;
    }
    if (IsDeleteFormItem) {
        actionButtons += `<button type='button' class="btn btn-sm btn-link deleteParent p-0" data-id="${response.id}"  ><i class="las la-trash"></i></button>`;
    }
    var formitemrow = `
                        <tr class="parent-row" data-id="${response.id}">
                            <td class="toggle">➖</td>
                            <td>
                            ${actionButtons}
                            </td>
                            <td>
                                ${response.nameAr}
                            </td>
                            <td>
                                ${response.nameEn}
                            </td>
                            <td style="display:none">
                                ${jsonString}
                            </td>
                        </tr>`;
    return formitemrow;
}
function GetSubformItemrow(response) {
    const jsonString = JSON.stringify(response);
    var actionButtons = "";
    if (IsEditSubFormItem) {
        actionButtons += `<button type="button" class="btn btn-sm btn-link editChild p-0" data-id="${response.id}"><i class="las la-edit"></i></button>`;
    }
    if (IsDeleteSubFormItem) {
        actionButtons += ` <button type='button' class="btn btn-sm btn-link deleteChild p-0" data-id="${response.id}"><i class="las la-trash"></i></button>`;
    }
    var subformitemrow = `<tr class="child-row" data-parent="${response.formItemId}" data-id="${response.id}">
                <td></td>
                <td> 
                   ${actionButtons}
               </td>
                <td style="padding-right:30px">
                    ${response.nameAr}
                </td>
                <td>
                   ${response.nameEn}
                </td>
                 <td style="display:none">
                   ${jsonString}
                </td>

            </tr>`;
    return subformitemrow;
}
$("#btn-submit_popup").click(function (e) {


    if (sharedFn().NewvalidateForm("form-control", sharedFn().GetUiControlText('WEB_CNTRL_REQUIRED'), sharedFn().GetUiControlText('WEB_MSG_MAX_CHAR_LENGTH'), sharedFn().GetUiControlText('WEB_MSG_MIN_CHAR_LENGTH'))) {


        commonUtil.btnProgress(btnpopupSubmitId);
        var validationlist = popupname == "EvalFormItem" ? FormItemcontrolvalidationlist : popupname == "EvalSubFormItem" ? SubFormItemcontrolvalidationlist : popupname == "FormScope" ? FormScopecontrolvalidationlist: null;
        var requestdata = sharedFn().GetSaveObject(validationlist, $('#PopupId').val());
        const options = {
            success: function (response) {
                commonUtil.btnProgress(btnSubmitId, true);


                if (response.responseStatus == '1') {
                    if (popupname == "EvalFormItem") {


                        var formitemrow = GetformItemrow(response);
                        $("#formitemTable tbody").prepend(formitemrow);
                        sharedFn().ClearPopup();
                    }
                    else if (popupname == "EvalSubFormItem") {
                        var parentId = response.formItemId;
                        var parentRow = $("#formitemTable tbody").find(`tr.parent-row[data-id='${parentId}']`);

                        var formitemrow = GetSubformItemrow(response);

                        parentRow.after(formitemrow);
                        sharedFn().ClearPopup();

                    }
                    else if (popupname == "FormScope") {
                        table.addData([response], true);
                        table.deselectRow();
                        table.getRows()[0].select();
                       
                        sharedFn().ResetVisibleControls('PopupForm');
                    }
                    if (response) {
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_SAVE'));

                    }
                }
                else if (response.responseStatus == '2') {
                    if (popupname == "EvalFormItem") {

                        var formitemrow = GetformItemrow(response);

                        // Check if the row already exists
                        var existingRow = $("#formitemTable tbody").find(`tr[data-id='${response.id}']`);

                        if (existingRow.length) {
                            // Update row by replacing HTML
                            existingRow.replaceWith(formitemrow);
                        }

                        sharedFn().ClearPopup();
                    }
                    else if (popupname == "EvalSubFormItem") {

                        var formitemrow = GetSubformItemrow(response);

                        // Check if the row already exists
                        var existingRow = $("#formitemTable tbody").find(`tr[data-id='${response.id}']`);

                        if (existingRow.length) {
                            // Update row by replacing HTML
                            existingRow.replaceWith(formitemrow);
                        }

                        sharedFn().ClearPopup();
                    }
                    else if (popupname == "FormScope") {
                        table.updateData([response]);
                        sharedFn().ResetVisibleControls('PopupForm');
                    }
                    if (response) {
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_UPDATE'));

                    }

                }
                else {
                    notificationUtil.error(response.message);

                }
               
            }
        };

        let url = '';
        let id = $('#PopupId').val();

        if (id) {
            url = popupname == "EvalFormItem" ? "/EvaluationForm/UpdateEvaluationFormItem" : popupname == "EvalSubFormItem" ? "/EvaluationForm/UpdateEvaluationSubFormItem" : popupname == "FormScope" ? "/EvaluationForm/UpdateFormScope" : null;
        } else {
            url = popupname == "EvalFormItem" ? "/EvaluationForm/SaveEvaluationFormItem" : popupname == "EvalSubFormItem" ? "/EvaluationForm/SaveEvaluationSubFormItem" : popupname == "FormScope" ? "/EvaluationForm/SaveFormScope" : null;

        }
        jqClient(options).PostFormData(url, requestdata);



    }
});
$(document).ready(function () {
    $formSection.hide();
});
initTables = () => {
    
    sharedFn().PopulateUiControl(controlsList);
    var viewItem = IsViewFormScope == true ?
        `<span class="Attr pointer" title="` + sharedFn().GetUiControlText('WEB_TOOLTIP_VIEW_FORMSCOPE') + `"><i class="Attr las la-bell" onclick="FormScopeClick(this)">
</i></span>`
        : '';
    var TableColumns = sharedFn().PopulateColumn(columnList, viewItem);
    
   
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
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: sharedFn().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        isResponsiveLayout: false,
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,

    });
    loadData();
};
function toggleNoteRequired() {
    if ($("#EvalFormItemHasNote").prop("checked")) {
        //$("#EvalFormItemNoteRequired")
        //    .closest(".col-3")
        //    .show();
        $("label[for='EvalFormItemNoteRequired']").show();

        $("#EvalFormItemNoteRequired").parent().show();
    } else {
        //$("#EvalFormItemNoteRequired")
        //    .closest(".col-3")
        //    .hide();
        $("label[for='EvalFormItemNoteRequired']").hide();

        $("#EvalFormItemNoteRequired").parent().hide();
    }
}
