
let table = null, currentPage = 1, isLoading = true, popupname = "",
    formItems = [];
let lookupSources = {};
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
        success: function (response) {
            if (response && response.items && response.items.length > 0) {
                table.addData(response.items);
                currentPage = currentPage + 1;
                if (currentPage > response.totalPages) {
                    isLoading = true;
                } else {
                    isLoading = false;
                }
            }
        }
    };

    const url = API_ROUTES.getAllForms() +
        `?PageNumber=${currentPage}&PageSize=${TABLE_CONFIG.pageSize}`;

    jqClient(options).Get(url);
};
function getlookup() {
    var EvalformId = $("#Id").val();
    return new Promise(function (resolve, reject) {
        const options = {
            success: function (result) {
                if (result) {
                    const { data } = result;
                    if (data) {
                        const { PartyTypeList, FormItemList, CalcMethodsList } = data;
                        lookupSources["PartyTypeList"] = PartyTypeList;
                        lookupSources["FormItemList"] = FormItemList;
                        lookupSources["CalcMethodsList"] = CalcMethodsList;
                    }
                }
                resolve();
            },
            error: function () {
                resolve();
            }
        };
        jqClient(options).Get(API_ROUTES.getFormItemLists(EvalformId));
    });
}
$btnAddbutton.click(function () {
    sharedFn().InitialPageControls(uiControlItems);
    //renderFormItemRelatedButton();
    $itemcontent.hide();
    $('#Id').val('');
});
function CreateEditForFormGroup(pkId) {
    CommonLogicAfterInitial();
}
function toggleFormItemColumn(disabled) {
    if (!tableFormItemConfig) return;
    if (disabled) {
        setTimeout(() => tableFormItemConfig.hideColumn("formItemConfig_Percentage"), 100);
    } else {
        setTimeout(() => tableFormItemConfig.showColumn("formItemConfig_Percentage"), 100);
    }
}
function CommonLogicAfterInitial() {
    $(document).off("change", "#EvalFormsIsFinalEval")
        .on("change", "#EvalFormsIsFinalEval", function () {
            if (!this.checked) {
                $("label[for='EvalFormsHasOneValue']").show();
                $("#EvalFormsHasOneValue").parent().show();
            } else {
                $("label[for='EvalFormsHasOneValue']").hide();
                $("#EvalFormsHasOneValue").parent().hide();
            }
        });
    $(document).off("change", "#EvalFormHasMuliEvaluation")
        .on("change", "#EvalFormHasMuliEvaluation", function () {
            if (this.checked) {
                $('label[for=EvalCountOfColumnsValue').show();
                $("#EvalCountOfColumnsValue").parent().show();
                $("#IsMultipleEvaluationWrapper").show();
            }
            else {
                $("#IsMultipleEvaluationWrapper").hide();
                $("label[for='EvalCountOfColumnsValue']").hide();
                $("#EvalCountOfColumnsValue").parent().hide();
            }
        });
}

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
        CommonLogicAfterInitial();
    }


}
$("#btnAddParent").click(function () {
    popupname = "EvalFormItem";
    var EvalformId = $("#Id").val();
    $("#evalformidvalue").val(EvalformId);
    var modaltitle = sharedFn().GetUiControlText('FormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, FormItemcontrolvalidationlist, null, null, null);

});
//function renderFormItemRelatedButton() {
//    $("#formItemConfig").remove();
//    const btn = `
//        <button type="button" id="formItemConfig" class="btn btn-sm btn-info mb-2">
//            <i class="las la-link"></i>
//        </button>
//   `;
//    $('#custm-code').prepend(btn);
//}
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

        jqClient(options).Get(API_ROUTES.getAllFormScopes(formIdValue));
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
    if (popupname == "EvalSubFormItem") {
        $("#EvalSubFormItemHasNote").on("change", function () {
            if (this.checked) {
                $("label[for='EvalSubFormItemNoteRequired']").show();
                $("#EvalSubFormItemNoteRequired").parent().show();
            }
            else {
                $("label[for='EvalSubFormItemNoteRequired']").hide();

                $("#EvalSubFormItemNoteRequired").parent().hide();
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
        jqClient(options).Get(API_ROUTES.getFormItemsFromDepartment(EvalformId));
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
    jqClient(options).Get(API_ROUTES.getAllFormItems(EvalformId));
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
$(document).on("click", ".editParent", async function () {
    var row = $(this).closest("tr");
    var id = $(this).data("id");
    var jsonString = row.find("td:last").text();
    var objdata = JSON.parse(jsonString);
    popupname = "EvalFormItem";

    await getlookup();

    var modaltitle = sharedFn().GetUiControlText('FormItemHeader');
    sharedFn().OpenFormPopup(modaltitle, FormItemcontrolvalidationlist, objdata, null, null);

    $("#PopupId").val(id);
    $("#evalformidvalue").val($("#Id").val());


    setTimeout(() => {
        BuildFormItemConfigTable(id, objdata.calcMethodId);
    }, 800);
});


function BuildFormItemConfigTable(formItemId, formItemCalcMethodId) {
    if ($('#FormItemConfigRelationtabulator').length) return;

    var filteredColList = FormItemConfigCollist.filter(c =>
        c.constraint.controlType !== 'TEXT_BOX_HIDDEN' &&
        c.controlName !== 'FormItemConfig_FormItemId' &&
        c.controlName !== 'FormItemConfig_CalcMethodId'
    );

    IsEdit = IsEditFormItemConfig ? true : "";
    IsDelete = IsDeleteFormItemConfig ? true : "";
    IsView = '';
    var checkboxColumn = {
        title: "",
        field: "selected",
        width: 40,
        headerSort: false,
        formatter: "rowSelection",
        titleFormatter: "rowSelection",
        cellClick: function (e, cell) {
            cell.getRow().toggleSelect();
        }
    };

    var tableColumns = sharedFn().PopulateColumn(filteredColList, '', true);
    tableColumns.splice(1, 0, checkboxColumn); 
    $('#ModalPopup .modal-body #PopupForm').append(`
        <div class="row mt-3">
            <div class="card-header justify-content-end d-flex align-items-center bg-light">
                <button type="button" id="FormItemConfigRelationbutton" class="btn btn-primary">
                    ${sharedFn().GetUiControlText('FormItemConfigAddButton')}
                </button>
            </div>
            <div id="FormItemConfigRelationdiv">
                <div id="FormItemConfigRelationtabulator"></div>
            </div>
            
            <div class="d-flex justify-content-end mt-2">
                <button type="button" id="FormItemConfigSaveButton" class="btn btn-success">
                    ${sharedFn().GetUiControlText('SAVE_BUTTON')}
                </button>
            </div>
        </div>
    `);

    tableFormItemConfig = tableUtil.createTabulator({
        id: "FormItemConfigRelationtabulator",
        config: {
            textDirection: txtDir,
            pagination: "local",
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            movableRows: true,
            editable: true,
            lookupSources: lookupSources,
            selectable: true
        },
        uniqueRowId: 'id',
        columns: tableColumns,
        rowClick: function (e, row) {
            currentrowclicked = row.getPosition();
        }
    });

    tableFormItemConfig.setData([]);

    jqClient({
        success: function (data) {
            if (data && data.length > 0) {

                // ✅ فلتر بـ prefix الصحيح + formItemId
                var filtered = data.filter(x =>
                    x.formItemConfig_FormItemIds &&
                    x.formItemConfig_FormItemIds.includes(formItemId)
                );

                filtered.forEach(x => {
                    x.formItemConfig_CalcMethod = x.formItemConfig_CalcMethodId || null;
                    x.formItemConfig_PartyType = x.formItemConfig_PartyTypeId || null;
                    x.formItemConfig_FormItem = Array.isArray(x.formItemConfig_FormItemIds)
                        ? x.formItemConfig_FormItemIds
                        : (x.formItemConfig_FormItemIds ? [x.formItemConfig_FormItemIds] : []);
                });

                tableFormItemConfig.setData(filtered);
            }
        }
    }).Get(API_ROUTES.getAllFormItemConfig($("#evalformidvalue").val()));

    $("#FormItemConfigRelationbutton").off("click").on("click", function () {
        tableFormItemConfig.addRow({
            id: "00000000-0000-0000-0000-000000000000",
            evalFormId: null,
            formItemId: formItemId,
            formItemIds: [formItemId],
            calcMethodId: formItemCalcMethodId,
            partyTypeId: null,
            nameAr: "",
            nameEn: "",
            percentage: 0,
            isActive: true
        });
    });


    $("#FormItemConfigSaveButton").off("click").on("click", function () {
        const allRows = tableFormItemConfig.getData();
        if (!allRows || allRows.length === 0) {
            notificationUtil.error('No data to save');
            return;
        }

        const dataToSave = allRows.map(obj => ({
            Id: obj.id === "00000000-0000-0000-0000-000000000000" ? null : obj.id,
            EvalFormId: $("#evalformidvalue").val(),
            NameAr: obj.formItemConfig_NameAr || null,
            NameEn: obj.formItemConfig_NameEn || null,
            PartyTypeId: obj.formItemConfig_PartyTypeId || obj.formItemConfig_PartyType || null,
            FormItemIds: [formItemId],
            CalcMethodId: formItemCalcMethodId,
            Percentage: obj.formItemConfig_Percentage || 0
        }));

        var formData = new FormData();
        formData.append('request', JSON.stringify(dataToSave));

        jqClient({
            success: function (response) {
                if (response?.responseStatus == 1 || response?.responseStatus == 2) {
                    notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_SAVE'));
                } else {
                    notificationUtil.error(response?.message);
                }
            },
            error: function () {
                notificationUtil.error('Request failed');
            }
        }).PostFormData(API_ROUTES.saveFormItemConfig(), formData);
    });
}
CommonLogicAfterInitial();
$(document).on("click", "#formItemConfig", async function () {
    popupname = "FormItemConfig";

    var EvalformId = $("#Id").val();
    $("#evalformidvalue").val(EvalformId);

    await getlookup();

    var modaltitle = sharedFn().GetUiControlText('FormItemHeader');

    IsEdit = IsEditFormItem ? "True" : "";
    IsDelete = IsDeleteFormItem ? "True" : "";
    IsView = IsViewFormItem ? "True" : "";
    var filteredColList = FormItemConfigCollist.filter(x =>
        x.constraint.controlType !== 'TEXT_BOX_HIDDEN'
    );

    var tableColumns = sharedFn().PopulateColumn(filteredColList, '', true);

    OpenFormItemConfigPopup(
        modaltitle,
        FormItemConfiglist,
        tableColumns,
        settingList
    );

    // show/hide IsMultipleEvaluationWrapper based on EvalFormHasMuliEvaluation
    if ($("#EvalFormHasMuliEvaluation").prop("checked")) {
        $("#IsMultipleEvaluationWrapper").show();
    } else {
        $("#IsMultipleEvaluationWrapper").hide();
        $("#IsMultipleEvaluation").prop("checked", false);
    }
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
$(document).on("click", ".edit", function () {
    if (popupname !== "FormItemConfig") return;

    const cellElem = $(this).closest('section')[0];
    const id = cellElem.getAttribute('data-key');
    const obj = tableFormItemConfig.getData().find(f => f.id == id);

    if (!obj) return;

    var data = {
        Id: obj.id,
        EvalFormId: $("#evalformidvalue").val(),
        NameAr: obj.nameAr || null,
        NameEn: obj.nameEn || null,
        PartyTypeId: obj.partyTypeId || obj.partyType || null,
        FormItemIds: obj.formItemId
            ? (Array.isArray(obj.formItemId) ? obj.formItemId : [obj.formItemId])
            : obj.formItem
                ? (Array.isArray(obj.formItem) ? obj.formItem : [obj.formItem])
                : null,
        CalcMethodId: obj.calcMethodId || obj.calcMethod || null,
        Percentage: obj.percentage || 0
    };

    var formData = new FormData();
    formData.append('request', JSON.stringify(data));

    jqClient({
        success: function (response) {
            if (response?.responseStatus == 2) {
                notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_UPDATE'));
                const updatedRow = {
                    ...response,
                    calcMethod: response.calcMethodId || null,
                    partyType: response.partyTypeId || null,
                    formItem: Array.isArray(response.formItemIds)
                        ? response.formItemIds
                        : (response.formItemIds ? [response.formItemIds] : [])
                };

                tableFormItemConfig.updateData([{ id: response.id, ...updatedRow }]);
            } else {
                notificationUtil.error(response?.message);
            }
        },
        error: function () {
            notificationUtil.error('Request failed');
        }
    }).PostFormData(
        API_ROUTES.updateFormItemConfig(),
        formData
    );
});
const deleteData = (id) => {
    if (!id) return;
    var deleteurl = '';
    if (popupname == 'EvalFormItem') {
        deleteurl = `${API_ROUTES.deleteFormItem()}`;
    }
    else if (popupname == 'EvalSubFormItem') {
        deleteurl = `${API_ROUTES.deleteSubFormItem()}`;
    }
    else if (popupname == 'FormScope') {
        deleteurl = API_ROUTES.deleteFormScope();
    }
    else if (popupname == 'FormItemConfig') {
        notificationUtil.confirmation({
            title: sharedFn().GetUiControlText('WEB_WARNING_DELETE'),
            okText: sharedFn().GetUiControlText('WEB_DELETE_BUTTON'),
            cancelText: sharedFn().GetUiControlText('WEB_CANCEL')
        }, result => {
            tableFormItemConfig.deleteRow(id);
            notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_DELETE'));
        });
        return;
    }
    else {
        const obj = table.getData().find(f => f.id == id);
        if (!obj) return;

        deleteurl = `${API_ROUTES.deleteForm()}`;
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
            url = API_ROUTES.updateForm();
        } else {
            url = API_ROUTES.saveForm();

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

        if (popupname == "FormItemConfig") {
            const allRows = tableFormItemConfig.getData();

            if (!allRows || allRows.length === 0) {
                const formId = $("#evalformidvalue").val();
                jqClient({
                    success: function (response) {
                        commonUtil.btnProgress(btnpopupSubmitId, true);
                        if (response?.responseStatus == 3) {
                            notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_DELETE'));
                        } else {
                            notificationUtil.error(response?.responseMessage);
                        }
                    },
                    error: function () {
                        commonUtil.btnProgress(btnpopupSubmitId, true);
                        notificationUtil.error('Request failed');
                    }
                }).Post(API_ROUTES.deleteFormItemConfig() + `?formId=${formId}`);
                return;
            }

            const isMultipleEvaluation = $("#EvalFormHasMuliEvaluation").prop("checked");

            const dataToSave = allRows.map(obj => {
                const isAdd = obj.id === "00000000-0000-0000-0000-000000000000" || !obj.id;

                const item = {
                    Id: isAdd ? null : obj.id,
                    EvalFormId: $("#evalformidvalue").val(),
                    NameAr: obj.formItemConfig_NameAr || null,
                    NameEn: obj.formItemConfig_NameEn || null,
                    PartyTypeId: obj.formItemConfig_PartyTypeId || obj.formItemConfig_PartyType || null,
                    FormItemIds: (obj.formItemConfig_FormItemId || obj.formItemConfig_FormItem)
                        ? (Array.isArray(obj.formItemConfig_FormItemId || obj.formItemConfig_FormItem)
                            ? (obj.formItemConfig_FormItemId || obj.formItemConfig_FormItem)
                            : [obj.formItemConfig_FormItemId || obj.formItemConfig_FormItem])
                        : null,
                    CalcMethodId: obj.formItemConfig_CalcMethodId || obj.formItemConfig_CalcMethod || null,
                    Percentage: obj.formItemConfig_Percentage || 0
                };

                if (isAdd) {
                    item.EvalFormHasMuliEvaluation = isMultipleEvaluation;
                }

                return item;
            });

            var formData = new FormData();
            formData.append('request', JSON.stringify(dataToSave));

            jqClient({
                success: function (response) {
                    commonUtil.btnProgress(btnpopupSubmitId, true);
                    if (response?.responseStatus == 1 || response?.responseStatus == 2) {
                        notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_SAVE'));
                        LoadFormItemConfigData();
                    } else {
                        notificationUtil.error(response?.message);
                    }
                },
                error: function () {
                    commonUtil.btnProgress(btnpopupSubmitId, true);
                    notificationUtil.error('Request failed');
                }
            }).PostFormData(API_ROUTES.saveFormItemConfig(), formData);

            return;
        }
        var validationlist = popupname == "EvalFormItem" ? FormItemcontrolvalidationlist
            : popupname == "EvalSubFormItem" ? SubFormItemcontrolvalidationlist
                : popupname == "FormScope" ? FormScopecontrolvalidationlist
                    : null;

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
                        var existingRow = $("#formitemTable tbody").find(`tr[data-id='${response.id}']`);
                        if (existingRow.length) {
                            existingRow.replaceWith(formitemrow);
                        }
                        sharedFn().ClearPopup();
                    }
                    else if (popupname == "EvalSubFormItem") {
                        var formitemrow = GetSubformItemrow(response);
                        var existingRow = $("#formitemTable tbody").find(`tr[data-id='${response.id}']`);
                        if (existingRow.length) {
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
            url = popupname == "EvalFormItem" ? API_ROUTES.updateFormItem()
                : popupname == "EvalSubFormItem" ? API_ROUTES.updateSubFormItem()
                    : popupname == "FormScope" ? API_ROUTES.updateFormScope()
                        : null;
        } else {
            url = popupname == "EvalFormItem" ? API_ROUTES.saveFormItem()
                : popupname == "EvalSubFormItem" ? API_ROUTES.saveSubFormItem()
                    : popupname == "FormScope" ? API_ROUTES.saveFormScope()
                        : null;
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
            selectable:true
        },
        isResponsiveLayout: false,
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: TableColumns,

    });
    loadData();
    getlookup();
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

$("#FormItemConfigRelationbutton").click(function () {
    tableFormItemConfig.addRow({
        id: "00000000-0000-0000-0000-000000000000",
        evalFormId: null,
        formItemId: null,
        partyTypeId: null,
        nameAr: "",
        nameEn: "",
        calcMethodId: null,
        percentage: 0,
        isActive: true
    });
});
function ClearForViewMode() {
    $("#FormItemConfigRelationbutton").hide();
    $("#FormItemConfigRelationtabulator").css("pointer-events", "none");
}
function OpenFormItemConfigPopup(
    modaltitle,
    ControlItems,
    tablecolumnlist,
    settingList
) {

    InitFormItemConfigPopup(
        modaltitle,
        ControlItems,
        tablecolumnlist,
        settingList
    );
    $("#IsMultipleEvaluation").off("change").on("change", function () {
        if (this.checked) {
            setTimeout(() => tableFormItemConfig.hideColumn("formItemConfig_Percentage"), 100);
            setTimeout(() => {
                $("#FormItemConfigRelationtabulator .tabulator-cell[tabulator-field='formItemConfig_FormItem']")
                    .css("pointer-events", "none")
                    .css("opacity", "0.5");
            }, 150);
        } else {
            setTimeout(() => tableFormItemConfig.showColumn("formItemConfig_Percentage"), 100);
            setTimeout(() => {
                $("#FormItemConfigRelationtabulator .tabulator-cell[tabulator-field='formItemConfig_FormItem']")
                    .css("pointer-events", "")
                    .css("opacity", "");
            }, 150);
        }
    });
    //if ($("#IsMultipleEvaluation").prop("checked")) {
    //    tableFormItemConfig.hideColumn("formItemConfig_Percentage");
    //}

    $("#ModalPopup").modal("show");

    $("#PopupForm").trigger("reset");
}


async function InitFormItemConfigPopup(
    modaltitle,
    ControlItems,
    tablecolumnlist,
    settingList
) {
    $("#PopupId").val('');
    $('#ModalPopup .modal-body #PopupForm').empty();
    $('#ModalPopup .modal-title').html(modaltitle);
    $("#btn-back_popup").html(sharedFn().GetUiControlText("BACK_BUTTON"));
    $("#btn-submit_popup").html(sharedFn().GetUiControlText("SAVE_BUTTON"));
    $("#btn-clear_popup").html(sharedFn().GetUiControlText("CLEAR_BUTTON"));

    let popupdivcontent = `
    <div class="row">
        <div class="card-header justify-content-end d-flex align-items-center bg-light">
            <div id="IsMultipleEvaluationWrapper" class="me-auto d-flex align-items-center gap-2" style="display:none;">
    <div class="form-check form-switch mb-0">
        <input class="form-check-input" type="checkbox" role="switch" id="IsMultipleEvaluation" />
        <label class="form-check-label" for="IsMultipleEvaluation">
            ${sharedFn().GetUiControlText('IsMultipleEvaluation')}
        </label>
    </div>
</div>
<div class="me-2 d-flex align-items-center gap-2">
    <button type="button" id="FormItemConfigDeleteSelectedButton" class="btn btn-danger d-none">
        <i class="las la-trash"></i>
        <span id="FormItemConfigSelectedCount"></span>
    </button>
</div>
            <button type="button" id="FormItemConfigRelationbutton" class="btn btn-primary">
                ${sharedFn().GetUiControlText('FormItemConfigAddButton')}
            </button>
        </div>
        <div id="FormItemConfigRelationdiv">
            <div id="FormItemConfigRelationtabulator"></div>
        </div>
    </div>
`;

    $('#ModalPopup .modal-body #PopupForm').html(popupdivcontent);

    var viewItem = IsAdd_FormItemConfig
        ? `<span class="Attr pointer" title="${sharedFn().GetUiControlText('ADMIN_TOOLTIP_SAVE_FORMITEMCONFIG')}">
            <i class="Attr fa fa-save" onclick="SaveFormItemConfig(this)"></i>
           </span>`
        : '';

    IsEdit = IsEditFormItemConfig ? true : "";
    IsDelete = IsDeleteFormItemConfig ? true : "";
    IsView = '';
    const checkboxColumn = {
        field: "rowSelected",
        width: 40,
        minWidth: 40,
        hozAlign: "center",
        headerHozAlign: "center",
        headerSort: false,
        resizable: false,
        editable: false,
        formatter: "rowSelection",
        titleFormatter: "rowSelection"
    };
    const tablecolumnlistWithCheckbox = [checkboxColumn, ...tablecolumnlist];

    tableFormItemConfig = tableUtil.createTabulator({
        id: "FormItemConfigRelationtabulator",
        config: {
            textDirection: txtDir,
            pagination: "local",
            paginationSize: 10,
            placeholder: sharedFn().GetUiControlText('NO_DATA_FOUND'),
            movableRows: true,
            selectable: true,
            selectableRangeMode: "click",
            editable: true,
            lookupSources: lookupSources
        },
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: tablecolumnlistWithCheckbox,

        rowClick: function (e, row) {
            currentrowclicked = row.getPosition();
        }
    });
    setTimeout(() => {
        $("#FormItemConfigRelationtabulator .tabulator-header-filter").hide();
    }, 100);
    tableFormItemConfig.setData([]);


    if (IsAdd_FormItemConfig) {
        $("#FormItemConfigRelationbutton").show();
    } else {
        $("#FormItemConfigRelationbutton").hide();
    }

    $("#FormItemConfigRelationbutton").off("click").on("click", function () {
        tableFormItemConfig.addRow({
            id: "00000000-0000-0000-0000-000000000000",
            evalFormId: null,
            formItemId: null,
            partyTypeId: null,
            nameAr: "",
            nameEn: "",
            calcMethodId: null,
            percentage: 0,
            isActive: true
        });
    });

    // row selection logic
    tableFormItemConfig.on("rowSelectionChanged", function (data, rows) {
        const count = rows.length;
        const totalRows = tableFormItemConfig.getRows().length;

        // update delete button
        if (count > 0) {
            $("#FormItemConfigDeleteSelectedButton")
                .removeClass("d-none")
                .find("#FormItemConfigSelectedCount")
                .text(` (${count})`);
        } else {
            $("#FormItemConfigDeleteSelectedButton").addClass("d-none");
            $("#FormItemConfigSelectedCount").text('');
        }

        // sync header checkbox
        $("#selectAllFormItemConfig").prop("checked", count > 0 && count === totalRows);

        // refresh all row checkboxes
        tableFormItemConfig.getRows().forEach(row => {
            const checkbox = row.getElement().querySelector("input[type='checkbox']");
            if (checkbox) checkbox.checked = row.isSelected();
        });
    });

    $("#FormItemConfigSelectAllButton").off("click").on("click", function () {
        tableFormItemConfig.selectRow();
    });

    $("#FormItemConfigClearSelectionButton").off("click").on("click", function () {
        tableFormItemConfig.deselectRow();
    });

    $("#FormItemConfigDeleteSelectedButton").off("click").on("click", function () {
        const selectedRows = tableFormItemConfig.getSelectedRows();
        if (!selectedRows || selectedRows.length === 0) return;

        notificationUtil.confirmation({
            title: sharedFn().GetUiControlText('WEB_WARNING_DELETE'),
            okText: sharedFn().GetUiControlText('WEB_DELETE_BUTTON'),
            cancelText: sharedFn().GetUiControlText('WEB_CANCEL')
        }, function () {
            selectedRows.forEach(row => row.delete());
            tableFormItemConfig.deselectRow();
            notificationUtil.success(sharedFn().GetUiControlText('WEB_MSG_DELETE'));
        });
    });

    LoadFormItemConfigData();
    // handle Percentage column visibility based on IsMultipleEvaluation
    $(document).off("change", "#IsMultipleEvaluation")
        .on("change", "#IsMultipleEvaluation", function () {
            if (this.checked) {
                setTimeout(() => tableFormItemConfig.hideColumn("formItemConfig_Percentage"), 100);
            } else {
                setTimeout(() => tableFormItemConfig.showColumn("formItemConfig_Percentage"), 100);
            }
        });

    // apply initial state after tabulator renders
    setTimeout(() => {
        if ($("#IsMultipleEvaluation").prop("checked")) {
            tableFormItemConfig.hideColumn("formItemConfig_Percentage");
        } else {
            tableFormItemConfig.showColumn("formItemConfig_Percentage");
        }
    }, 200);
}
function LoadFormItemConfigData() {
    const evalformId = $("#evalformidvalue").val();

    const options = {
        success: function (data) {
            if (data && data.length > 0) {

                data.forEach(x => {
                    x.formItemConfig_FormItem = Array.isArray(x.formItemConfig_FormItemIds) ? x.formItemConfig_FormItemIds : (x.formItemConfig_FormItemIds ? [x.formItemConfig_FormItemIds] : []);
                    x.formItemConfig_CalcMethod = x.formItemConfig_CalcMethodId || null;
                    x.formItemConfig_PartyType = x.formItemConfig_PartyTypeId || null;

                });

                tableFormItemConfig.setData(data);

            } else {
                tableFormItemConfig.setData([]);
            }
        }
    };

    jqClient(options).Get(
        `${API_ROUTES.getAllFormItemConfig(evalformId)}`
    );
}