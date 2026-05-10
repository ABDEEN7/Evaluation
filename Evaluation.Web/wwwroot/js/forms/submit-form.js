// ==============================
// API Endpoints
// ==============================
const SUBMIT_FORM_API = {
    calculateEvaluationResult: (departmentPath) =>
        `/Form/${departmentPath}/CalculateEvaluationFormResult`,

    validateEvaluationForm: (departmentPath) =>
        `/Form/${departmentPath}/ValidateEvaluationForm`,

    saveEvaluationForm: (departmentPath) =>
        `/Form/${departmentPath}/SaveEvaluationForm`,
};

let departmentPath = sharedUtility().extractDepartmentName();

function evaluationFormResult(formId) { 
    const mainItems = [];

    // ========== LOOP MAIN ITEMS ONLY ==========
    $(`#${P_fieldId}-${SELECTORS.tbody} tr.main-row`).each(function () {

        const row = $(this);
        const selects = row.find("select.eval-select");
        let mainObj;

        selects.each(function (index, element) {

            const $select = $(element);



            const mainId = $select.data("id");

            const selectedValue =
                $select.find("option:selected").data("id") ||
                $select.val() ||
                null;

            const note =
                row.find("textarea.note-input").val() || null;

            mainObj = {
                id: mainId,
                valueId: selectedValue,
                value: $("option:selected", $select).text(),
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
                    value: $("option:selected", $select).text(),
                    note: childnote
                });
            });


            mainItems.push(mainObj);
        });

      

        //const note =
        //    row.find("textarea.note-input").val() || null;

        //const mainObj = {
        //    id: mainId,
        //    valueId: selectedValue,
        //    value: $("option:selected", select).text(),
        //    note: note,
        //    subItems: []
        //};

        //// ========== LOOP SUB ITEMS RELATED TO THIS MAIN ==========
        //$(`tr.child-row[data-parent-id="${mainId}"]`).each(function () {

        //    const childRow = $(this);
        //    const childSelect = childRow.find("select.eval-select");

        //    const childId = childSelect.data("id");

        //    const childValue =
        //        childSelect.find("option:selected").data("id") ||
        //        childSelect.val() ||
        //        null;

        //    const childnote =
        //        childRow.find("textarea.note-input").val() || null;

        //    mainObj.subItems.push({
        //        id: childId,
        //        valueId: childValue,
        //        value: $("option:selected", select).text(),
        //        note: childnote
        //    });
        //});

        //mainItems.push(mainObj);
    });

    // Strengths & Improvements
    const strengths = $("textarea[data-row='1']").eq(0).val() || null;
    const improvements = $("textarea[data-row='1']").eq(1).val() || null;

    const payload = {
        id: formId,
        items: mainItems,
        formSettings: evalForm,
    };

    console.log("FINAL NESTED JSON:", payload);

    return payload;
}
async function evaluationFormWithCalculationResult(formId) {
    var result = evaluationFormResult(formId);
    var calculation = await calculate(result);

    var finalResult = {
        id: result.id,
        items: result.items,
        formSettings: result.formSettings,
        results: calculation.value,
        evaluationRequestId: P_evaluationRequestId,
        serviceRequestId: P_serviceRequestId
    };

    console.log(finalResult);

    return finalResult;

}
async function calculate(result) {
    return new Promise((resolve, reject) => {
        jqClient().Post(
            SUBMIT_FORM_API.calculateEvaluationResult(depRoutePath)
            , result)
        .done((res) => {
            console.log(res);
            resolve(res); 
        }).fail((err) => {
            reject(err);
        });
    });
}

function calculateFE(select, formId) {
    let formResult = evaluationFormResult(formId);
    let result = { Value: 0, Name: "", Id:"00000000-0000-0000-0000-000000000000"};

    switch (evalForm.calcMethod) {
        case "AVERAGE": {

            let total = 0;

            formResult.items.forEach((item) => {
                total += parseInt(item.value, 10) || 0;
            });

            result.Value = total / formResult.items.length;

            const evalMatrixValue = P_matrixResponse.value.find(
                v => v.minValue <= result.Value && v.maxValue >= result.Value
            );

            result.Name = evalMatrixValue?.name ?? null;
            result.Id = evalMatrixValue?.id ?? null;

            break;
        }

        case "SUM":
            break;

        case "WithoutCalc":
            break;
    }

    $(`#${P_fieldId}-result-value`).text(`${result.Name}/${result.Value}`);
    $(`#${P_fieldId}-result-div`).removeClass("d-none");

    return result
}

function validateForm(formId) {
    var result = evaluationFormResult(formId);
    jqClient().Post(
        SUBMIT_FORM_API.validateEvaluationForm(depRoutePath)
        , result)
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

function submitForm(formId) {
    var result = evaluationFormResult(formId);
    jqClient().Post(
        SUBMIT_FORM_API.saveEvaluationForm(depRoutePath)
        , result)
        .done((res) => {
            Swal.fire({
                icon: "success",
                title: "تم الإرسال",
                text: "تم إرسال الاستمارة للاعتماد بنجاح"
            });
        });
}

async function saveForm(formId) {
    var result = evaluationFormResult(formId);
    var calculation = await calculate(result);

    var finalResult = {
        id: result.id,
        items: result.items,
        formSettings: result.formSettings,
        results: calculation.value,
        evaluationRequestId: P_evaluationRequestId,
        serviceRequestId: P_serviceRequestId
    };
    return finalResult;
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