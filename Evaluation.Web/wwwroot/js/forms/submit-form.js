let departmentPath = sharedUtility().extractDepartmentName();

$(document).ready(function () {
    $("#btnSubmitForm").on("click", function (e) {
        e.preventDefault();
        submitForm();
    });
});

$(document).ready(function () {
    $("#btnSaveForm").on("click", function (e) {
        e.preventDefault();
        validateForm();
    });
});
function evaluationFormResult(formId) { 
    const mainItems = [];

    // ========== LOOP MAIN ITEMS ONLY ==========
    $(`#${P_fieldId}-${SELECTORS.tbody} tr.main-row`).each(function () {

        const row = $(this);
        const select = row.find("select.eval-select");

        const mainId = select.data("id");

        const selectedValue =
            select.find("option:selected").data("id") ||
            select.val() ||
            null;

        const note =
            row.find("textarea.note-input").val() || null;

        const mainObj = {
            id: mainId,
            valueId: selectedValue,
            //value: selectedValue,
            note: note,
            subItems: []
        };

        // ========== LOOP SUB ITEMS RELATED TO THIS MAIN ==========
        $(`tr.child-row[data-parent-id="${mainId}"]`).each(function () {

            const childRow = $(this);
            const childSelect = childRow.find("select.eval-select");

            const childId = childSelect.data("id");

            const childValue =
                childSelect.find("option:selected").data("id") ||
                childSelect.val() ||
                null;

            const childnote =
                childRow.find("textarea.note-input").val() || null;

            mainObj.subItems.push({
                id: childId,
                valueId: childValue,
                //value: selectedValue,
                note: childnote
            });
        });

        mainItems.push(mainObj);
    });

    // Strengths & Improvements
    const strengths = $("textarea[data-row='1']").eq(0).val() || null;
    const improvements = $("textarea[data-row='1']").eq(1).val() || null;

    const payload = {
        id: formId,
        items: mainItems
    };

    console.log("FINAL NESTED JSON:", payload);

    return payload;
}


function renameFormItems(formId) {
    const mainItems = [];

    // ========== LOOP MAIN ITEMS ONLY ==========
    $(`#${P_fieldId}-${SELECTORS.tbody} tr.main-row`).each(function () {

        const row = $(this);

        const itemInput = row.find("input.item-name");

        const itemInputVal =
            itemInput.val() || null;

        const mainId = itemInput.data("id");


        const mainObj = {
            id: mainId,
            name: itemInputVal,
            subItems: []
        };

        mainItems.push(mainObj);
    });

    const payload = {
        id: formId,
        items: mainItems
    };

    console.log("FINAL NESTED JSON:", payload);

    return payload;
}

//function renameitemForm(formId) {
//    var result = renameFormItems(formId);
//    jqClient().Post(`/Form/${departmentPath}/RenameFormItems`, result)
//        .done((res) => {
//            Swal.fire({
//                icon: "success",
//                title: "تم الإرسال",
//                text: "تم الإرسال بنجاح"
//            });
//        });
//}

function submitForm(formId) {
    var result = evaluationFormResult(formId);
    jqClient().Post(`/Form/${departmentPath}/SaveEvaluationForm`, result)
        .done((res) => {
            Swal.fire({
                icon: "success",
                title: "تم الإرسال",
                text: "تم إرسال الاستمارة للاعتماد بنجاح"
            });
        });
}
function validateForm(formId) {
    var result = evaluationFormResult(formId);
    jqClient().Post(`/Form/${departmentPath}/ValidateEvaluationForm`, result)
        .done((res) => {

            if (res.value.isValid) {
                return res.value.isValid;
            }
            else {
                clearValidation();
                res.value.errors.forEach(error => {
                    showValidation(error.itemId, error.message, error.itemPropertyType);
                });
                return res.value.isValid;
            }
        });
}
function saveForm(formId) {
    return evaluationFormResult(formId);
}

function showValidation(itemId, message, itemPropertyType) {
    const el = document.getElementById(`validation-${itemId}-${itemPropertyType}`);
    if (!el) return;

    el.textContent = "*" + message;
    el.style.display = 'block';
}

function clearValidation() {
    const els = document.getElementsByClassName(`validation-message`);
    if (!els) return;

    for (const el of els) {
        el.style.textContent = '';
        el.style.display = 'none';
    }
}