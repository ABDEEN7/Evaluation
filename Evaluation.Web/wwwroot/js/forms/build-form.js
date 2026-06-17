// ==============================
// API Endpoints
// ==============================
const GET_FORMS_API = {
    getItems: (depRoutePath, formId, academicYearId) =>
        `/Form/${depRoutePath}/GetItems?formId=${formId}&academicYearId=cbbace9d-08e1-4267-8471-cb30d2217a6e`,

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
// Per-form state
// ==============================
// Replaces the old single set of module-level globals (evalForm,
// P_hasMuliEvaluation, P_matrixResponse, P_fieldId, P_evaluationRequestId,
// P_serviceRequestId, MOCK_DATA). Each form rendered on the page gets its
// own entry here, keyed by fieldId, so multiple forms can coexist on the
// same page without overwriting each other's state.
const formStates = new Map();

function getFormState(fieldId) {
    if (!formStates.has(fieldId)) {
        formStates.set(fieldId, {
            evalForm: null,
            hasMuliEvaluation: false,
            matrixResponse: null,
            evaluationRequestId: null,
            serviceRequestId: null,
            formData: null
        });
    }
    return formStates.get(fieldId);
}


// Minimal escaping for values placed inside HTML attributes (value="...").
function escapeAttr(text = '') {
    return String(text)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

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
        // Related items are shown via an info icon next to the row name,
        // opening a read-only reference table. Mirrors relatedItems
        // handling from get-forms_Old.js.
        relatedItems: item.relatedItems ?? [],
        subItems: (item.subFormItems || []).map(sub => ({
            id: sub.id,
            text: sub.name,
            hasNote: !!sub.hasNote,
            weightPercentage: getWeightPercentage(sub),
            relatedItems: sub.relatedItems ?? [],
            // Sub-items can carry their own value list (subItemLists) that
            // overrides the shared evaluation matrix for that row only.
            // Mirrors the old subItemListsMap behavior in get-forms_Old.js.
            subItemLists: sub.subItemLists ?? []
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
// fieldId is threaded through so the validation message ids stay unique
// per form (validation-${fieldId}-${itemId}-${itemPropertyType}).
function renderSelectAndNote(fieldId, itemId, hasNote, readOnly, matrixValues, customOptions = null) {
    // If this item has its own subItemLists (non-empty), those options
    // replace the shared evaluation matrix for this select only - same
    // priority order as the old populateForm()/subItemListsMap logic.
    const useCustomOptions = Array.isArray(customOptions) && customOptions.length > 0;

    const optionsHtml = useCustomOptions
        ? customOptions.map(o => `<option value="${o.id}">${escapeAttr(o.nameAr || o.nameEn)}</option>`).join('')
        : matrixValues.map(o => `<option value="${o.id}" data-actual-value="${o.actualMatrixValue}">${o.actualMatrixValue}</option>`).join('');

    return `
        <div class="row-select-wrap">
            <select class="row-select" ${readOnly ? 'disabled' : ''}>
                <option disabled selected>Please Select</option>
                ${optionsHtml}
            </select>
            <span class="validation-message" id="validation-${fieldId}-${itemId}-${ItemPropertyType.SELECT}"></span>
        </div>

        <div class="row-note-wrap">
            ${hasNote
            ? `<input class="row-note" ${readOnly ? 'disabled' : ''} type="text" placeholder="اكتب ملاحظة هنا..." />`
            : `<div class="row-note-placeholder"></div>`}
            <span class="validation-message" id="validation-${fieldId}-${itemId}-${ItemPropertyType.NOTE}"></span>
        </div>
    `;
}

// ==============================
// Row name cell (name + optional related-items info icon)
// ==============================
// The name input and the info icon are rendered together as ONE wrapper
// element so a row's total number of top-level children is unchanged
// from before the icon existed (index cell, name cell, select-wrap,
// note-wrap). Adding the icon as a separate sibling node broke the
// row-item grid/flex layout, which relies on a fixed set of cells -
// this keeps the cell count stable regardless of the page's CSS.
function renderRowName(fieldId, itemId, text, relatedItems) {
    const hasRelated = Array.isArray(relatedItems) && relatedItems.length > 0;

    return `
        <div class="row-name-cell" style="display:flex;align-items:center;gap:6px;min-width:0;">
            <input class="row-name" type="text" value="${escapeAttr(text)}" readonly style="flex:1 1 auto;min-width:0;" />
            ${hasRelated
        ? `<span class="info-icon info-button"
                     title="عرض البنود المرتبطة"
                     onclick="openRelatedItemModal('${fieldId}', '${itemId}')"
                     >ⓘ</span>`
            : ''}
        </div>
    `;
}

function renderSubRowHtml(fieldId, sub, parentId, label, readOnly, matrixValues) {
    return `
        <div class="row-item child-row" data-item-id="${sub.id}" data-parent-id="${parentId}" data-weight="${sub.weightPercentage}">
            <span class="row-index">
                <span class="row-index-num">${label}</span>
            </span>

            ${renderRowName(fieldId, sub.id, sub.text, sub.relatedItems)}

            ${renderSelectAndNote(fieldId, sub.id, sub.hasNote, readOnly, matrixValues, sub.subItemLists)}
        </div>
    `;
}

// fieldId is optional: pass it when the page has a dedicated toast per
// form (id="${fieldId}-toast"); otherwise falls back to a single shared
// #toast element.
function toast(msg, fieldId = null) {
    const el = (fieldId && document.getElementById(`${fieldId}-toast`)) || document.getElementById('toast');
    if (!el) return;

    el.textContent = msg;
    el.classList.add('show');
    setTimeout(() => el.classList.remove('show'), 3000);
}

// ==============================
// Related items popup
// ==============================
// Read-only reference table shown when the user clicks the info icon next
// to a row (or sub-row) name. Ported from get-forms_Old.js
// (createRowRelatedItem / generateTableBodyHtmlForRelatedItems /
// relatedItemPopup / openRelatedItemModal), adapted to be scoped per
// fieldId so multiple forms on the same page don't collide on a shared
// modal id.
const createRowRelatedItem = ({ item, order, hasAnyNote }) => `
    <tr class="main-row align-middle">
        <td>${order}</td>
        <td class="text-start">${escapeAttr(item.name)}</td>
        <td>${escapeAttr(item.value)}</td>
        ${hasAnyNote ? `<td>${escapeAttr(item.note)}</td>` : ''}
    </tr>
`;

const generateTableBodyHtmlForRelatedItems = (items, hasAnyNote) =>
    items.map((item, i) => createRowRelatedItem({
        item,
        order: i + 1,
        hasAnyNote
    })).join('');

const relatedItemPopup = (fieldId, rowsHtml, hasAnyNote) => `
    <div class="modal fade" id="${fieldId}-related-item-modal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-xl modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header align-items-start border-0">
                    <div>
                        <h4 class="modal-title fw-semibold mb-2">البنود المرتبطة</h4>
                    </div>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

                <div class="modal-body py-0">
                    <div class="row">
                        <table class="table table-bordered text-center align-middle">
                            <thead class="table-grey">
                                <tr>
                                    <th>#</th>
                                    <th>البند</th>
                                    <th>القيمة</th>
                                    ${hasAnyNote ? '<th>ملاحظات</th>' : ''}
                                </tr>
                            </thead>
                            <tbody>
                                ${rowsHtml}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
`;

function openRelatedItemModal(fieldId, itemId) {
    const state = getFormState(fieldId);
    const relatedItems = state.relatedItemsMap?.get(itemId) ?? [];

    const rowsHtml = generateTableBodyHtmlForRelatedItems(relatedItems, !!state.hasAnyNote);
    const popupHtml = relatedItemPopup(fieldId, rowsHtml, !!state.hasAnyNote);

    // Remove any stale instance of this form's modal before re-inserting.
    const existing = document.getElementById(`${fieldId}-related-item-modal`);
    if (existing) existing.remove();

    document.body.insertAdjacentHTML('beforeend', popupHtml);

    const modalElement = document.getElementById(`${fieldId}-related-item-modal`);
    const modal = new bootstrap.Modal(modalElement);

    // Remove modal from DOM after it is closed
    modalElement.addEventListener('hidden.bs.modal', () => {
        modalElement.remove();
    });

    modal.show();
}

// ==============================
// Init
// ==============================
// Requires a container element already present on the page with
// id="${fieldId}-form-root". That's what makes it safe to call initForm
// more than once on the same page (one call per fieldId/form instance).
async function initForm(formId, fieldId, readOnly, savedResults, evaluationRequestId, serviceRequestId) {
    const state = getFormState(fieldId);
    state.evaluationRequestId = evaluationRequestId;
    state.serviceRequestId = serviceRequestId;

    const response = await jqClient().Get(
        GET_FORMS_API.getItems(depRoutePath, formId)
    );

    let tree = response?.value?.tree ?? [];
    state.evalForm = response?.value?.evalForm ?? null;
    state.hasMuliEvaluation = !!(state.evalForm && state.evalForm.hasMuliEvaluation);

    const matrixResponse = await jqClient().Get(
        GET_FORMS_API.getMatrixValues(depRoutePath, formId)
    );

    state.matrixResponse = matrixResponse;
    const matrixValues = matrixResponse?.value ?? matrixResponse ?? [];


    state.formData = buildFormData(tree, state.evalForm);

    // Lookup map (itemId -> relatedItems[]) used by openRelatedItemModal,
    // plus a form-level "does any row have a note column" flag used to
    // decide whether the related-items popup shows a notes column.
    // Mirrors relatedItems handling from get-forms_Old.js.
    state.relatedItemsMap = new Map();
    state.hasAnyNote = false;

    state.formData.forEach(crit => {
        crit.aspects.forEach(asp => {
            asp.rows.forEach(row => {
                if (row.hasNote) state.hasAnyNote = true;
                if (Array.isArray(row.relatedItems) && row.relatedItems.length > 0) {
                    state.relatedItemsMap.set(row.id, row.relatedItems);
                }

                (row.subItems || []).forEach(sub => {
                    if (sub.hasNote) state.hasAnyNote = true;
                    if (Array.isArray(sub.relatedItems) && sub.relatedItems.length > 0) {
                        state.relatedItemsMap.set(sub.id, sub.relatedItems);
                    }
                });
            });
        });
    });

    const root = document.getElementById(`${fieldId}-form-root`);
    if (!root) {
        console.error(`initForm: no element with id "${fieldId}-form-root" found on the page.`);
        return;
    }
    root.innerHTML = '';

    const table = buildHorizontalTable(matrixValues);

    const container = document.getElementById(`${fieldId}-form-root`);

    container.appendChild(table);

    // Result banner, scoped to this form. Inserted once per fieldId so
    // repeated initForm calls (e.g. re-init) don't duplicate it.
    if (!document.getElementById(`${fieldId}-form-result-div`)) {
        root.insertAdjacentHTML('afterend', `
            <div id="${fieldId}-form-result-div" class="d-none bg-primary d-flex justify-content-between align-items-center py-2">
                <div class="text-white">Result:</div>
                <div class="text-white" id="${fieldId}-form-result-value"></div>
            </div>
        `);
    }

    // Build lookup maps: itemId -> saved result, subItemId -> saved sub-result
    const savedMap = {};
    const savedSubMap = {};
    if (Array.isArray(savedResults)) {
        savedResults.forEach(r => {
            savedMap[r.id] = r;
            (r.subItems || []).forEach(sub => { savedSubMap[sub.id] = sub; });
        });
    }

    state.formData.forEach((crit, cIdx) => {
        const cCard = document.createElement('div');
        cCard.className = 'criterion-card';

        const critBodyId = `${fieldId}-crit-body-${cIdx}`;

        cCard.innerHTML = `
            <div class="criterion-header">
                <span class="criterion-number">معيار ${cIdx + 1}</span>
                <input class="criterion-title-input" type="text" value="${escapeAttr(crit.title)}" readonly />
            </div>
            <div class="criterion-body" id="${critBodyId}"></div>
        `;

        root.appendChild(cCard);

        const body = document.getElementById(critBodyId);

        crit.aspects
            .filter(asp => asp.rows && asp.rows.length > 0)
            .forEach((asp, aIdx) => {
                const aCard = document.createElement('div');
                aCard.className = 'aspect-card';

                const aId = `${fieldId}-asp-${cIdx}-${aIdx}`;

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

                        ${renderRowName(fieldId, row.id, row.text, row.relatedItems)}

                        ${renderSelectAndNote(fieldId, row.id, row.hasNote, readOnly, matrixValues)}
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

                    // Recalculate the result banner whenever this row's value changes
                    rowDiv.querySelector('.row-select').addEventListener('change', () => {
                        calculateFE(formId, fieldId);
                    });

                    // Sub-items: rendered into a collapsible container, toggled from the main row
                    if (hasSubItems) {
                        const subContainer = document.createElement('div');
                        subContainer.className = 'subitems-container';
                        subContainer.dataset.parentId = row.id;

                        row.subItems.forEach((sub, sIdx) => {
                            subContainer.insertAdjacentHTML(
                                'beforeend',
                                renderSubRowHtml(fieldId, sub, row.id, `${rIdx + 1}.${sIdx + 1}`, readOnly, matrixValues)
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

                            childRow.querySelector('.row-select').addEventListener('change', () => {
                                calculateFE(formId, fieldId);
                            });
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
// Scoped to this form's own container so it never picks up rows
// belonging to a different form rendered on the same page.
function evaluationFormResult(formId, fieldId) {
    const state = getFormState(fieldId);
    const root = document.getElementById(`${fieldId}-form-root`);

    if (!root) {
        console.error(`evaluationFormResult: no element with id "${fieldId}-form-root" found.`);
        return { id: formId, items: [], formSettings: state.evalForm };
    }

    const items = [];

    root.querySelectorAll('.row-item.main-row').forEach(row => {
        const itemId = row.dataset.itemId;
        const selectEl = row.querySelector('.row-select');
        const noteEl = row.querySelector('.row-note');

        const valueId = selectEl.value || null;
        const actualValue = parseFloat(selectEl.selectedOptions[0]?.dataset.actualValue) || 0;
        const note = noteEl ? (noteEl.value.trim() || null) : null;
        const weightPercentage = parseFloat(row.dataset.weight) || 0;

        const subItems = [];
        root.querySelectorAll(`.row-item.child-row[data-parent-id="${itemId}"]`).forEach(childRow => {
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
        formSettings: state.evalForm
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
// Live calculation on select change (client-side, mirrors calcMethod)
// ==============================
function calculateFE(formId, fieldId) {
    const state = getFormState(fieldId);
    const formResult = evaluationFormResult(formId, fieldId);
    const result = { Value: 0, Name: null, Id: '00000000-0000-0000-0000-000000000000' };

    switch (state.evalForm?.calcMethod) {
        case "AVERAGE": {
            const hasWeights = formResult.items.some(item => item.weightPercentage > 0);

            let total = 0;
            formResult.items.forEach((item) => {
                total += hasWeights
                    ? (item.value * (item.weightPercentage / 100)) || 0
                    : (item.value || 0);
            });

            result.Value = hasWeights
                ? total
                : total / (formResult.items.length || 1);

            const matrixValues = state.matrixResponse?.value ?? state.matrixResponse ?? [];
            const evalMatrixValue = matrixValues.find(
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

    const resultValueEl = document.getElementById(`${fieldId}-form-result-value`);
    const resultDiv = document.getElementById(`${fieldId}-form-result-div`);

    if (resultValueEl) {
        resultValueEl.textContent = `(${Number(result.Value).toFixed(2)}) ${result.Name ?? ''}`;
    }
    if (resultDiv) {
        resultDiv.classList.remove('d-none');
    }

    return result;
}

// ==============================
// Validate (calls backend, shows inline error spans)
// ==============================
async function validateForm(formId, fieldId) {
    const result = evaluationFormResult(formId, fieldId);

    return new Promise((resolve, reject) => {
        jqClient().Post(
            SUBMIT_FORM_API.validateEvaluationForm(depRoutePath),
            result
        )
            .done((res) => {
                clearValidation(fieldId);

                if (res.value.isValid) {
                    resolve(true);
                } else {
                    res.value.errors.forEach(error => {
                        showValidation(fieldId, error.itemId, error.message, error.itemPropertyType);
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
async function SubmitForm(formId, fieldId) {

    const finalResult = await getFormResults(formId, fieldId);

    return new Promise((resolve, reject) => {
        jqClient().Post(
            SUBMIT_FORM_API.saveEvaluationForm(depRoutePath),
            finalResult
        )
            .done((res) => {
                toast('تم حفظ التقييم بنجاح', fieldId);
                resolve(res);
            })
            .fail((err) => {
                toast('حدث خطأ أثناء الحفظ', fieldId);
                reject(err);
            });
    });
}

// ==============================
// Save (validate -> calculate -> persist)
// ==============================
async function getFormResults(formId, fieldId) {
    const isValid = await validateForm(formId, fieldId);
    if (!isValid) {
        toast('يوجد حقول غير مكتملة، يرجى مراجعة الأخطاء', fieldId);
        return null;
    }

    const result = evaluationFormResult(formId, fieldId);
    const calculation = await calculate(result);
    const state = getFormState(fieldId);

    const finalResult = {
        id: result.id,
        items: result.items,
        formSettings: result.formSettings,
        results: calculation.value,
        evaluationRequestId: state.evaluationRequestId,
        serviceRequestId: state.serviceRequestId
    };
    return finalResult;
}

// ==============================
// Validation message helpers
// ==============================
function showValidation(fieldId, itemId, message, itemPropertyType) {
    const el = document.getElementById(`validation-${fieldId}-${itemId}-${itemPropertyType}`);
    if (!el) return;

    el.textContent = "*" + message;
    el.style.display = 'block';
}

function clearValidation(fieldId) {
    const root = document.getElementById(`${fieldId}-form-root`);
    if (!root) return;

    root.querySelectorAll('.validation-message').forEach(el => {
        el.textContent = '';
        el.style.display = 'none';
    });
}


function buildHorizontalTable(data) {
    const table = document.createElement("table");
    table.border = "1";
    table.style.borderCollapse = "collapse";
    table.className = "table table-bordered table-hover align-middle w-100 dataTable no-footer";

    const tHeadnameRow = document.createElement("thead");
    tHeadnameRow.className = 'table-light';

    const nameRow = document.createElement("tr");
    const rangeRow = document.createElement("tr");

    // Row 1: Names
    const nameCell = document.createElement("th");
    nameCell.textContent = "Name";
    //nameCell.className = 'table-grey';
    nameRow.appendChild(nameCell);
    tHeadnameRow.appendChild(nameRow);
    // Row 2: Range
    const rangeCell = document.createElement("td");
    rangeCell.textContent = `Range`;
    rangeRow.appendChild(rangeCell);

    data.forEach(item => {
        // Row 1: Names
        const nameCell = document.createElement("th");
        nameCell.textContent = item.name;
        //nameCell.className = 'table-grey';
        nameRow.appendChild(nameCell);
        tHeadnameRow.appendChild(nameRow);

        // Row 2: Min - Max
        const rangeCell = document.createElement("td");
        rangeCell.textContent = `(${item.minValue} - ${item.maxValue})`;
        rangeRow.appendChild(rangeCell);
    });

    table.appendChild(tHeadnameRow);
    table.appendChild(rangeRow);

    return table;
}