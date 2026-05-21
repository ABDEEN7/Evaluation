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

// ==============================
// TREE HELPERS (NEW)
// ==============================
function flattenTreeItems(nodes = []) {
    const result = [];

    function walk(list) {
        for (const node of list) {
            if (node.items?.length) {
                result.push(...node.items);
            }
            if (node.children?.length) {
                walk(node.children);
            }
        }
    }

    walk(nodes);
    return result;
}

// ==============================
// MAIN RESULT BUILDER (UPDATED)
// ==============================
function evaluationFormResult(formId) {

    const mainItems = [];

    // Select ALL tbodies generated from scopes
    $(`tbody[id^="${P_fieldId}_"][id$="-${SELECTORS.tbody}"]`)
        .each(function () {

            const tbody = $(this);

            tbody.find("tr.main-row").each(function () {

                const row = $(this);

                const selects = row.find("select.eval-select");

                if (!selects.length) {
                    return;
                }

                const firstSelect = selects.first();

                const mainId = firstSelect.data("id");

                const note =
                    row.find("textarea.note-input").val() || null;

                const subItems = [];

                // Child rows INSIDE SAME TBODY ONLY
                tbody.find(
                    `tr.child-row[data-parent-id="${mainId}"]`
                ).each(function () {

                    const childRow = $(this);

                    const childSelect =
                        childRow.find("select.eval-select");

                    if (!childSelect.length) {
                        return;
                    }

                    const selectedChildOption =
                        childSelect.find("option:selected");

                    subItems.push({
                        id: childSelect.data("id"),

                        valueId:
                            childSelect.val() || null,

                        value:
                            parseFloat(
                                selectedChildOption.data("actual-value")
                            ) || 0,

                        note:
                            childRow.find("textarea.note-input").val() ||
                            null
                    });
                });

                const selectedMainOption =
                    firstSelect.find("option:selected");

                mainItems.push({
                    id: mainId,

                    valueId:
                        firstSelect.val() || null,

                    value:
                        parseFloat(
                            selectedMainOption.data("actual-value")
                        ) || 0,

                    weightPercentage:
                        parseFloat(
                            firstSelect.data(
                                "config-weight-percentage"
                            )
                        ) || 0,

                    note,

                    subItems
                });
            });
        });

    return {
        id: formId,
        items: mainItems,
        formSettings: evalForm
    };
}

// ==============================
// CALCULATION (UPDATED SAFE)
// ==============================
async function calculate(result) {
    return jqClient()
        .Post(
            SUBMIT_FORM_API.calculateEvaluationResult(depRoutePath),
            result
        )
        .then(res => res)
        .catch(err => {
            console.error(err);
            throw err;
        });
}

// ==============================
// FE LIVE CALCULATION (FIXED)
// ==============================
function calculateFE(select, formId) {

    const formResult = evaluationFormResult(formId);

    let total = 0;
    let count = 0;

    formResult.items.forEach(item => {

        if (P_hasMuliEvaluation) {
            total += (item.value * (item.weightPercentage / 100)) || 0;
        } else {
            total += item.value || 0;
        }

        count++;
    });

    let resultValue = 0;

    if (evalForm.calcMethod === "AVERAGE") {

        if (P_hasMuliEvaluation) {
            resultValue = total / (formResult.items.length / P_countOfColumnsValue);
        } else {
            resultValue = total / (formResult.items.length || 1);
        }
    }

    const evalMatrixValue = (P_matrixResponse?.value || []).find(v =>
        v.minValue <= resultValue && v.maxValue >= resultValue
    );

    const result = {
        Value: resultValue,
        Name: evalMatrixValue?.name ?? "",
        Id: evalMatrixValue?.id ?? ""
    };

    $(`#${P_fieldId}-result-value`)
        .text(`(${Number(result.Value).toFixed(2)}) ${result.Name}`);

    $(`#${P_fieldId}-result-div`)
        .removeClass("d-none");

    return result;
}

// ==============================
// VALIDATION (FIXED)
// ==============================
function validateForm(formId) {

    const result = evaluationFormResult(formId);

    jqClient()
        .Post(
            SUBMIT_FORM_API.validateEvaluationForm(depRoutePath),
            result
        )
        .done(res => {

            if (res.value.isValid) return true;

            clearValidation();

            res.value.errors.forEach(error => {
                showValidation(
                    error.itemId,
                    error.message,
                    error.itemPropertyType
                );
            });

            return false;
        });
}

// ==============================
// SAVE / SUBMIT (UNCHANGED STRUCTURE)
// ==============================
function submitForm(formId) {

    const result = evaluationFormResult(formId);

    jqClient()
        .Post(
            SUBMIT_FORM_API.saveEvaluationForm(depRoutePath),
            result
        )
        .done(() => {

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

// ==============================
// RENAME (WORKS WITH TREE UI)
// ==============================
function renameFormItems(formId) {

    const mainItems = [];

    $(`#${P_fieldId}-${SELECTORS.tbody} tr.main-row`).each(function () {

        const row = $(this);
        const input = row.find("input.item-name");

        mainItems.push({
            id: input.data("id"),
            name: input.val() || null,
            subItems: []
        });
    });

    return {
        id: formId,
        items: mainItems
    };
}

// ==============================
// VALIDATION UI HELPERS
// ==============================
function showValidation(itemId, message, itemPropertyType) {

    const el =
        document.getElementById(`validation-${itemId}-${itemPropertyType}`);

    if (!el) return;

    el.textContent = "*" + message;
    el.style.display = "block";
}

function clearValidation() {

    const els =
        document.getElementsByClassName("validation-message");

    for (const el of els) {
        el.textContent = '';
        el.style.display = 'none';
    }
}