
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

});
function ClearControlByPage() {
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
$("#btnAddParent").click(function () {
    popupname = "EvalFormItem";
    var EvalformId = $("#Id").val();
    $("#evalformidvalue").val(EvalformId);
    var modaltitle = sharedFn().GetUiControlText('FormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, FormItemcontrolvalidationlist, null, null, null);

});
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
    deleteData(id, 'EvalFormItem');
});

// SubFormItem delete button
$(document).on("click", ".deleteChild", function () {
    var id = $(this).data("id");
    deleteData(id, 'EvalSubFormItem');
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
const deleteData = (id,popupname=null) => {
    if (!id) return;
    var deleteurl = '';
    if (popupname == 'EvalFormItem') {
        deleteurl = "/EvaluationForm/DeleteEvaluationFormItem";
    }
    else if (popupname == 'EvalSubFormItem') {
        deleteurl = "/EvaluationForm/DeleteEvaluationSubFormItem";
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
                    table.deselectRow();
                    table.getRows()[0].select();
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
        var validationlist = popupname == "EvalFormItem" ? FormItemcontrolvalidationlist : popupname == "EvalSubFormItem" ? SubFormItemcontrolvalidationlist:null;
        var requestdata = sharedFn().GetSaveObject(validationlist, $('#PopupId').val());
        const options = {
            success: function (response) {
                commonUtil.btnProgress(btnSubmitId, true);


                if (response.responseStatus == '1') {
                    if (popupname == "EvalFormItem") {
                        
                       
                        var formitemrow =GetformItemrow(response);
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
            url = popupname == "EvalFormItem" ? "/EvaluationForm/UpdateEvaluationFormItem" : popupname == "EvalSubFormItem" ? "/EvaluationForm/UpdateEvaluationSubFormItem":null;
        } else {
            url = popupname == "EvalFormItem" ? "/EvaluationForm/SaveEvaluationFormItem" : popupname == "EvalSubFormItem" ? "/EvaluationForm/SaveEvaluationSubFormItem":null;

        }
        jqClient(options).PostFormData(url, requestdata);



    }
});
$(document).ready(function () {
    $formSection.hide();
});
initTables = () => {
    
    sharedFn().PopulateUiControl(controlsList);
    
    var TableColumns = sharedFn().PopulateColumn(columnList);
    
   
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
    loadData();
};














