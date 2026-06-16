// ==============================
// API Endpoints
// ==============================
const GET_FORMS_API = {
    getItems: (depRoutePath, formId) =>
        `/Form/${depRoutePath}/GetItems?formId=${formId}`,

    getMatrixValues: (depRoutePath, formId) =>
        `/Form/${depRoutePath}/GetFormEvalMarixValues?formId=${formId}`,
};


const SUBMIT_FORM_API = {
    calculateEvaluationResult: (depRoutePath) =>
        `/Form/${depRoutePath}/CalculateEvaluationFormResult`,

    validateEvaluationForm: (depRoutePath) =>
        `/Form/${depRoutePath}/ValidateEvaluationForm`,

    saveEvaluationForm: (depRoutePath) =>
        `/Form/${depRoutePath}/SaveEvaluationForm`,
};


let depRoutePath = sharedUtility().extractDepartmentName();


const ItemPropertyType = Object.freeze({
    SELECT: 1,
    NOTE: 2
});


// ==============================
// Module-level state
// (replaces the page-level P_* globals that used to be set
//  inside generateFullFormPageHtml in the old get-forms.js)
// ==============================
let evalForm = null;          // set from response.value.evalForm in initForm
let P_hasMuliEvaluation = false; // derived from evalForm.hasMuliEvaluation
let P_matrixResponse = null;     // raw response from GetFormEvalMarixValues

// These three are NOT derivable from the API responses - the page
// that calls initForm must pass them in (same values that used to be
// passed into generateFullFormPageHtml).
let P_fieldId = null;
let P_evaluationRequestId = null;
let P_serviceRequestId = null;

let MOCK_DATA;


// Minimal escaping for values placed inside HTML attributes (value="...").
// The old build-form.js injected item.name directly with no escaping;
// this guards against item names containing a double quote breaking the markup.
function escapeAttr(text = '') {
    return String(text)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

//// Build MOCK_DATA-shaped structure dynamically from API tree
//// Top-level tree node  -> Criterion (criterion-card)
//// node.children        -> Aspects (aspect-card)
//// child.items          -> Rows (row-item)
//// child.name           -> Domain title (right-side column)
//function buildFormData(tree) {
//    return tree.map(criterionNode => ({
//        title: criterionNode.name,
//        aspects: (criterionNode.children || []).map(child => ({
//            title: child.name,
//            domainTitle: child.name,
//            rows: (child.items || []).map(item => ({
//                id: item.id,
//                text: item.name,
//                hasNote: !!item.hasNote
//            }))
//        }))
//    }));
//}

// ==============================
// Build MOCK_DATA-shaped structure dynamically from API tree
// ==============================
// Top-level tree node  -> Criterion (criterion-card)
// node.children        -> Aspects (aspect-card)
// child.items          -> Rows (row-item)
// item.subFormItems    -> Sub-rows (collapsible, under the main row)
// child.name           -> Domain title (right-side column)
function buildFormData(tree, evalFormData) {
    // BA decision: weight percentages are only used when the form allows
    // renaming (evalForm.allowRename === true). When false, weight is
    // always 0 and formItemConfigs is ignored entirely.
    const allowRename = !!(evalFormData && evalFormData.allowRename);

    const getWeightPercentage = (itemLike) => {
        if (!allowRename) return 0;
        const cfg = (itemLike.formItemConfigs || [])[0];
        return cfg ? (Number(cfg.formItemConfig_Percentage) || 0) : 0;
    };

    const mapRow = (item) => ({
        id: item.id,
        text: item.name,
        hasNote: !!item.hasNote,
        weightPercentage: getWeightPercentage(item),
        subItems: (item.subFormItems || []).map(sub => ({
            id: sub.id,
            text: sub.name,
            // NOTE: current "New Json Response" sample doesn't include
            // hasNote/formItemConfigs on subFormItems. Defaults to
            // false/0 until that structure is updated.
            hasNote: !!sub.hasNote,
            weightPercentage: getWeightPercentage(sub)
        }))
    });

    return tree.map(criterionNode => ({
        title: criterionNode.name,
        aspects: (criterionNode.children || []).map(child => ({
            title: child.name,
            domainTitle: child.name,
            rows: (child.items || []).map(mapRow)
        }))
    }));
}


// ==============================
// Row rendering
// ==============================

function renderSelectAndNote(itemId, hasNote, readOnly, matrixValues) {
    return `
        <div class="row-select-wrap">
            <select class="row-select" ${readOnly ? 'disabled' : ''}>
                <option disabled selected>Please Select</option>
                ${matrixValues.map(o => `<option value="${o.id}" data-actual-value="${o.actualMatrixValue}">${o.actualMatrixValue}</option>`).join('')}
            </select>
            <span class="validation-message" id="validation-${itemId}-${ItemPropertyType.SELECT}"></span>
        </div>

        <div class="row-note-wrap">
            ${hasNote
            ? `<input class="row-note" ${readOnly ? 'disabled' : ''} type="text" placeholder="اكتب ملاحظة هنا..." />`
            : `<div class="row-note-placeholder"></div>`}
            <span class="validation-message" id="validation-${itemId}-${ItemPropertyType.NOTE}"></span>
        </div>
    `;
}

function renderSubRowHtml(sub, parentId, label, readOnly, matrixValues) {
    return `
        <div class="row-item child-row" data-item-id="${sub.id}" data-parent-id="${parentId}" data-weight="${sub.weightPercentage}">
            <span class="row-index">
                <span class="row-index-num">${label}</span>
            </span>

            <input class="row-name" type="text" value="${escapeAttr(sub.text)}" readonly />

            ${renderSelectAndNote(sub.id, sub.hasNote, readOnly, matrixValues)}
        </div>
    `;
}

function toast(msg) {
    const el = document.getElementById('toast');
    el.textContent = msg;
    el.classList.add('show');
    setTimeout(() => el.classList.remove('show'), 3000);
}

// ==============================
// Init
// ==============================

// fieldId / evaluationRequestId / serviceRequestId are page-level values
// that used to be passed into generateFullFormPageHtml; the page calling
// initForm must supply them here.
async function initForm(formId, readOnly, savedResults, fieldId, evaluationRequestId, serviceRequestId) {
    P_fieldId = fieldId;
    P_evaluationRequestId = evaluationRequestId;
    P_serviceRequestId = serviceRequestId;

    const response = await jqClient().Get(
        GET_FORMS_API.getItems(depRoutePath, formId)
    );

    let tree = response?.value?.tree ?? [];
    evalForm = response?.value?.evalForm ?? null;
    P_hasMuliEvaluation = !!(evalForm && evalForm.hasMuliEvaluation);

    const matrixResponse = await jqClient().Get(
        GET_FORMS_API.getMatrixValues(depRoutePath, formId)
    );

    P_matrixResponse = matrixResponse;
    const matrixValues = matrixResponse?.value ?? matrixResponse ?? [];

    MOCK_DATA = buildFormData(tree, evalForm);

    const root = document.getElementById('form-root');
    root.innerHTML = '';

    // Build lookup maps: itemId -> saved result, subItemId -> saved sub-result
    const savedMap = {};
    const savedSubMap = {};
    if (Array.isArray(savedResults)) {
        savedResults.forEach(r => {
            savedMap[r.id] = r;
            (r.subItems || []).forEach(sub => { savedSubMap[sub.id] = sub; });
        });
    }

    MOCK_DATA.forEach((crit, cIdx) => {
        const cCard = document.createElement('div');
        cCard.className = 'criterion-card';

        cCard.innerHTML = `
            <div class="criterion-header">
                <span class="criterion-number">معيار ${cIdx + 1}</span>
                <input class="criterion-title-input" type="text" value="${escapeAttr(crit.title)}" readonly />
            </div>
            <div class="criterion-body" id="crit-body-${cIdx}"></div>
        `;

        root.appendChild(cCard);

        const body = document.getElementById(`crit-body-${cIdx}`);

        crit.aspects
            .filter(asp => asp.rows && asp.rows.length > 0)
            .forEach((asp, aIdx) => {
                const aCard = document.createElement('div');
                aCard.className = 'aspect-card';

                const aId = `asp-${cIdx}-${aIdx}`;

                aCard.innerHTML = `
                    <div class="aspect-header">
                        <span class="aspect-label">جانب ${aIdx + 1}</span>
                        <input class="aspect-title-input" type="text" value="${escapeAttr(asp.title)}" readonly />
                    </div>

                    <div class="domain-row">
                        <div class="domain-title">
                            ${asp.domainTitle}
                        </div>

                        <div class="rows-area" id="rows-${aId}"></div>
                    </div>
                `;

                body.appendChild(aCard);

                const rowsArea = document.getElementById(`rows-${aId}`);

                asp.rows.forEach((row, rIdx) => {
                    const hasSubItems = Array.isArray(row.subItems) && row.subItems.length > 0;

                    const rowDiv = document.createElement('div');
                    rowDiv.className = 'row-item main-row' + (hasSubItems ? ' has-subitems' : '');
                    rowDiv.dataset.itemId = row.id;
                    rowDiv.dataset.weight = row.weightPercentage;

                    rowDiv.innerHTML = `
                        <span class="row-index">
                            <span class="row-index-num">${rIdx + 1}</span>
                            ${hasSubItems
                            ? `<button type="button" class="row-toggle" aria-expanded="false" aria-label="toggle sub items">&#9656;</button>`
                            : ''}
                        </span>

                        <input class="row-name" type="text" value="${escapeAttr(row.text)}" readonly />

                        ${renderSelectAndNote(row.id, row.hasNote, readOnly, matrixValues)}
                    `;

                    rowsArea.appendChild(rowDiv);

                    // Prefill main row from saved results
                    const saved = savedMap[row.id];
                    if (saved) {
                        const selectEl = rowDiv.querySelector('.row-select');
                        if (selectEl && saved.valueId) {
                            selectEl.value = saved.valueId;
                        }

                        const noteEl = rowDiv.querySelector('.row-note');
                        if (noteEl && saved.note !== null && saved.note !== undefined) {
                            noteEl.value = saved.note;
                        }
                    }

                    // Sub-items: rendered into a collapsible container, toggled from the main row
                    if (hasSubItems) {
                        const subContainer = document.createElement('div');
                        subContainer.className = 'subitems-container';
                        subContainer.dataset.parentId = row.id;

                        row.subItems.forEach((sub, sIdx) => {
                            subContainer.insertAdjacentHTML(
                                'beforeend',
                                renderSubRowHtml(sub, row.id, `${rIdx + 1}.${sIdx + 1}`, readOnly, matrixValues)
                            );
                        });

                        rowsArea.appendChild(subContainer);

                        // Prefill sub-items from saved results
                        subContainer.querySelectorAll('.row-item.child-row').forEach(childRow => {
                            const childId = childRow.dataset.itemId;
                            const savedSub = savedSubMap[childId];
                            if (savedSub) {
                                const selectEl = childRow.querySelector('.row-select');
                                if (selectEl && savedSub.valueId) {
                                    selectEl.value = savedSub.valueId;
                                }

                                const noteEl = childRow.querySelector('.row-note');
                                if (noteEl && savedSub.note !== null && savedSub.note !== undefined) {
                                    noteEl.value = savedSub.note;
                                }
                            }
                        });

                        // Toggle expand/collapse
                        const toggleBtn = rowDiv.querySelector('.row-toggle');
                        toggleBtn.addEventListener('click', () => {
                            const expanded = subContainer.classList.toggle('expanded');
                            toggleBtn.classList.toggle('is-open', expanded);
                            toggleBtn.setAttribute('aria-expanded', String(expanded));
                        });
                    }
                });
            });
    });
}


// ==============================
// Build payload from current DOM state
// ==============================
function evaluationFormResult(formId) {
    const items = [];

    document.querySelectorAll('.row-item.main-row').forEach(row => {
        const itemId = row.dataset.itemId;
        const selectEl = row.querySelector('.row-select');
        const noteEl = row.querySelector('.row-note');

        const valueId = selectEl.value || null;
        const actualValue = parseFloat(selectEl.selectedOptions[0]?.dataset.actualValue) || 0;
        const note = noteEl ? (noteEl.value.trim() || null) : null;
        const weightPercentage = parseFloat(row.dataset.weight) || 0;

        const subItems = [];
        document.querySelectorAll(`.row-item.child-row[data-parent-id="${itemId}"]`).forEach(childRow => {
            const childSelect = childRow.querySelector('.row-select');
            const childNote = childRow.querySelector('.row-note');

            subItems.push({
                id: childRow.dataset.itemId,
                valueId: childSelect.value || null,
                // NOTE: ported as-is from the old code, which used the
                // selected option's display text here (not actualMatrixValue)
                // for sub-items - flagged "NEED TO CHECK" in the original.
                value: childSelect.selectedOptions[0]?.textContent ?? '',
                note: childNote ? (childNote.value.trim() || null) : null
            });
        });

        items.push({
            id: itemId,
            value: actualValue,
            valueId: valueId,
            weightPercentage: weightPercentage,
            note: note,
            subItems: subItems
        });
    });

    return {
        id: formId,
        items: items,
        formSettings: evalForm
    };
}

// ==============================
// Calculate (calls backend)
// ==============================
function calculate(result) {
    return new Promise((resolve, reject) => {
        jqClient().Post(
            SUBMIT_FORM_API.calculateEvaluationResult(depRoutePath),
            result
        )
            .done((res) => resolve(res))
            .fail((err) => reject(err));
    });
}

// ==============================
// Validate (calls backend, shows inline error spans)
// ==============================
async function validateForm(formId) {
    const result = evaluationFormResult(formId);

    return new Promise((resolve, reject) => {
        jqClient().Post(
            SUBMIT_FORM_API.validateEvaluationForm(depRoutePath),
            result
        )
            .done((res) => {
                clearValidation();

                if (res.value.isValid) {
                    resolve(true);
                } else {
                    res.value.errors.forEach(error => {
                        showValidation(error.itemId, error.message, error.itemPropertyType);
                    });
                    resolve(false);
                }
            })
            .fail((err) => reject(err));
    });
}

// ==============================
// Save (validate -> calculate -> persist)
// ==============================
async function SubmitForm(formId) {

    const finalResult = getFormResults(formId)

    return new Promise((resolve, reject) => {
        jqClient().Post(
            SUBMIT_FORM_API.saveEvaluationForm(depRoutePath),
            finalResult
        )
            .done((res) => {
                toast('تم حفظ التقييم بنجاح');
                resolve(res);
            })
            .fail((err) => {
                toast('حدث خطأ أثناء الحفظ');
                reject(err);
            });
    });
}

// ==============================
// Save (validate -> calculate -> persist)
// ==============================
async function getFormResults(formId) {
    //const isValid = await validateForm(formId);
    //if (!isValid) {
    //    toast('يوجد حقول غير مكتملة، يرجى مراجعة الأخطاء');
    //    return null;
    //}

    const result = evaluationFormResult(formId);
    const calculation = await calculate(result);

    const finalResult = {
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
// Validation message helpers
// ==============================
function showValidation(itemId, message, itemPropertyType) {
    const el = document.getElementById(`validation-${itemId}-${itemPropertyType}`);
    if (!el) return;

    el.textContent = "*" + message;
    el.style.display = 'block';
}

function clearValidation() {
    const els = document.getElementsByClassName('validation-message');
    if (!els) return;

    for (const el of els) {
        el.textContent = '';
        el.style.display = 'none';
    }
}

