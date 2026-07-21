// ==============================
// API Endpoints
// ==============================
const GET_FORMS_API = {
    getItems: (depRoutePath, formId, academicYearId) =>
        `/Form/${depRoutePath}/GetItems?formId=${formId}&academicYearId=${academicYearId}`,

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
let selectPleaceHolder = currentLang == 'ar' ? 'يرجى الاختيار' : 'Please Select';


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
            formData: null,
            // Rename-mode state (ported from get-forms_Old.js /
            // submit-form_Old.js: renameItems, P_isRename, P_allowRename,
            // P_allowDelete, P_allowAdd). Scoped per fieldId/per aspect so
            // multiple forms - and multiple aspect cards within a form -
            // each manage their own pool of not-yet-added top-level items.
            isRename: false,
            allowRename: false,
            allowDelete: false,
            allowAdd: false,
            renamePools: new Map(),    // aspectKey -> top-level items not yet rendered
            renameItemsById: new Map() // itemId -> { item, aspectKey }
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
        // Full config list (one entry per evaluation column when
        // hasMuliEvaluation is true) kept around so the row can render one
        // select per column, each carrying its own name/weight - mirrors
        // the old buildSelection(item, fieldId, readOnly, index) loop in
        // get-forms_Old.js. Only ever populated on top-level items, same
        // as getWeightPercentage above.
        formItemConfigs: item.formItemConfigs ?? [],
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

    // The tree depth before items appear varies per form/department:
    //
    //  D=1  root has items directly (e.g. "Criteria" node with items: [...])
    //       → one aspect (title = root.name), one implicit domain (no label)
    //
    //  D=2  root → children with items directly (original shape)
    //       → each child = one aspect, one implicit domain (no label)
    //
    //  D=3  root → child → grandchildren with items (new 3-level shape, e.g.
    //       "Assembly Points" → "School Leadership" → "Visit and Follow-up Form",
    //       "Teaching and Learning")
    //       → each child = one aspect, each grandchild-with-items = one NAMED
    //         domain within that aspect (label shown in the left domain column)
    //
    //  D=4+ further nesting: recurse, building named domain titles by joining
    //       intermediate node names so no items are ever silently dropped.
    //
    // Output per aspect: { title, domains: [{ title: string|null, rows[] }] }
    // domain.title = null → no label rendered (D=1 / D=2 implicit single domain)
    // domain.title = string → shown as the left-column label in the UI (D=3+)

    // True when node has at least one item directly on itself.
    const hasDirectItems = (node) =>
        Array.isArray(node.items) && node.items.length > 0;

    // True when node has items anywhere in its subtree (including itself).
    const hasAnyItems = (node) =>
        hasDirectItems(node) ||
        (node.children || []).some(hasAnyItems);

    // Given a node that has NO direct items, return its named domains by
    // walking its children. Each child-with-direct-items → one domain
    // (title = child.name). Each child-without-direct-items → recurse and
    // prefix the child name so nested titles remain meaningful.
    const collectDomains = (node) => {
        let domains = [];
        (node.children || []).filter(hasAnyItems).forEach(child => {
            if (hasDirectItems(child)) {
                domains.push({ title: child.name, rows: child.items.map(mapRow) });
            } else {
                // Recurse: bring deeper domains up, preserving hierarchy in title.
                collectDomains(child).forEach(d => {
                    domains.push({
                        title: d.title ? `${child.name} › ${d.title}` : child.name,
                        rows: d.rows
                    });
                });
            }
        });
        return domains;
    };

    // Build aspects for a single top-level criterion node.
    const buildAspects = (criterionNode) => {
        // D=1: items sit directly on the root → single aspect, single implicit domain.
        // isDirect flags this aspect as "not a real aspect" - its title is
        // just a copy of the criterion's own title, so the renderer skips
        // the aspect-card wrapper/header entirely for it (see aspectCardsHtml
        // below) instead of showing the same name twice (once on the
        // criterion card, once on the aspect card).
        if (hasDirectItems(criterionNode)) {
            return [{
                title: criterionNode.name,
                isDirect: true,
                domains: [{ title: null, rows: criterionNode.items.map(mapRow) }]
            }];
        }

        return (criterionNode.children || [])
            .filter(hasAnyItems)
            .map(child => {
                if (hasDirectItems(child)) {
                    // D=2: child has items directly → single implicit domain, no label.
                    return {
                        title: child.name,
                        domains: [{ title: null, rows: child.items.map(mapRow) }]
                    };
                } else {
                    // D=3+: child's content is deeper → child becomes the aspect
                    // header and its descendants become named domain groups.
                    return {
                        title: child.name,
                        domains: collectDomains(child)
                    };
                }
            })
            .filter(asp => asp.domains.length > 0);
    };

    return tree.map(criterionNode => ({
        title: criterionNode.name,
        aspects: buildAspects(criterionNode)
    }));
}


// ==============================
// Row rendering
// ==============================
// fieldId is threaded through so the validation message ids stay unique
// per form (validation-${fieldId}-${itemId}-${itemPropertyType}).
//
// selectedValueId/noteValue let a saved result be baked directly into
// the returned HTML string (via the option's `selected` attribute and
// the input's `value` attribute) instead of being applied afterward via
// element.value - required now that initForm returns a string instead
// of mutating live DOM nodes.
function renderSelectAndNote(fieldId, itemId, hasNote, readOnly, matrixValues, customOptions = null, selectedValueId = null, noteValue = null) {
    // If this item has its own subItemLists (non-empty), those options
    // replace the shared evaluation matrix for this select only - same
    // priority order as the old populateForm()/subItemListsMap logic.
    const useCustomOptions = Array.isArray(customOptions) && customOptions.length > 0;

    const isSelected = (id) => selectedValueId && String(id) === String(selectedValueId);

    const optionsHtml = useCustomOptions
        ? customOptions.map(o => `<option value="${o.id}" ${isSelected(o.id) ? 'selected' : ''}>${escapeAttr(currentLang == 'ar' ? o.nameAr : o.nameEn)}</option>`).join('')
        : matrixValues.map(o => `<option value="${o.id}" data-actual-value="${o.actualMatrixValue}" ${isSelected(o.id) ? 'selected' : ''}>${o.actualMatrixValue}</option>`).join('');

    return `
        <div class="row-select-wrap">
            <select class="row-select" ${readOnly ? 'disabled' : ''}>
                <option disabled ${selectedValueId ? '' : 'selected'}>${selectPleaceHolder}</option>
                ${optionsHtml}
            </select>
            <span class="validation-message" id="validation-${fieldId}-${itemId}-${ItemPropertyType.SELECT}"></span>
        </div>

        <div class="row-note-wrap">
            ${hasNote
            ? `<input class="row-note" ${readOnly ? 'disabled' : ''} type="text" placeholder="اكتب ملاحظة هنا..." value="${escapeAttr(noteValue ?? '')}" />`
            : `<div class="row-note-placeholder"></div>`}
            <span class="validation-message" id="validation-${fieldId}-${itemId}-${ItemPropertyType.NOTE}"></span>
        </div>
    `;
}

// ==============================
// Multi-evaluation select rendering (top-level rows only)
// ==============================
// When the form's evalForm.hasMuliEvaluation is true, a top-level row
// gets one select PER evaluation column (evalCountOfColumnsValue), each
// one scoped to its own formItemConfigs entry and carrying its own
// data-config-weight-percentage - mirrors the old buildSelection(item,
// fieldId, readOnly, index) loop + data-config-weight-percentage
// attribute from get-forms_Old.js. The note column is still rendered
// once per row, not once per select, same as before.
//
// Column names (formItemConfig_NameAr/NameEn) are NOT rendered inline
// per select anymore - they're shown once, as a shared header row above
// all rows in the aspect (see renderMultiSelectHeaderRow), the same way
// a table header applies to a whole column rather than repeating per
// data row.
//
// Falls back to the existing single-select renderSelectAndNote when
// hasMuliEvaluation is false or there are no formItemConfigs to drive
// multiple columns from.
function renderMultiSelectWrap(fieldId, itemId, readOnly, matrixValues, formItemConfigs, index, selectedValueId = null) {
    const cfg = formItemConfigs[index];
    const weight = cfg ? (Number(cfg.formItemConfig_Percentage) || 0) : 0;
    const isSelected = (id) => selectedValueId && String(id) === String(selectedValueId);

    const optionsHtml = matrixValues
        .map(o => `<option value="${o.id}" data-actual-value="${o.actualMatrixValue}" ${isSelected(o.id) ? 'selected' : ''}>${o.actualMatrixValue}</option>`)
        .join('');

    return `
        <div class="row-select-wrap">
            <select class="row-select" data-config-weight-percentage="${weight}" ${readOnly ? 'disabled' : ''}>
                <option disabled ${selectedValueId ? '' : 'selected'}>${selectPleaceHolder}</option>
                ${optionsHtml}
            </select>
            <span class="validation-message" id="validation-${fieldId}-${itemId}-${ItemPropertyType.SELECT}_${index}"></span>
        </div>
    `;
}

// Shared column-header row for multi-evaluation selects: one label per
// column (formItemConfig_NameAr/NameEn), rendered once above the rows
// instead of repeating inside every row's select. The empty leading
// cells line up with the row-index/name cells so labels sit directly
// over their corresponding select column.
function renderMultiSelectHeaderRow(formItemConfigs, countOfColumnsValue, extraLeadingCells = 2) {
    const leadingCells = Array.from({ length: extraLeadingCells }, () => '<div class="row-select-header-spacer"></div>').join('');

    const labelCells = Array.from({ length: countOfColumnsValue }, (_, index) => {
        const cfg = formItemConfigs[index];
        const label = cfg ? (cfg.formItemConfig_NameAr || cfg.formItemConfig_NameEn || '') : '';
        return `<div class="row-select-col-label">${escapeAttr(label)}</div>`;
    }).join('');

    return `
        <div class="row-item row-select-header">
            ${leadingCells}
            ${labelCells}
        </div>
    `;
}

function renderSelectsAndNote(fieldId, itemId, hasNote, readOnly, matrixValues, hasMuliEvaluation, formItemConfigs = [], countOfColumnsValue = 1, selectedValueIdOrIds = null, noteValue = null) {
    const useMulti = !!hasMuliEvaluation && Array.isArray(formItemConfigs) && formItemConfigs.length > 0;

    if (!useMulti) {
        return renderSelectAndNote(fieldId, itemId, hasNote, readOnly, matrixValues, null, selectedValueIdOrIds, noteValue);
    }

    const selectedValueIds = Array.isArray(selectedValueIdOrIds) ? selectedValueIdOrIds : [];

    const selectsHtml = Array.from({ length: countOfColumnsValue }, (_, index) =>
        renderMultiSelectWrap(fieldId, itemId, readOnly, matrixValues, formItemConfigs, index, selectedValueIds[index] ?? null)
    ).join('');

    return selectsHtml;
}

// Selects-only variant (no note column) used by rename mode, where the
// note input is intentionally not rendered (rename rows only collect a
// renamed label + an evaluation value, no free-text note - see
// fillRenameControls). Mirrors renderSelectsAndNote's column-count logic
// without the trailing note wrap.
function renderSelectsOnly(fieldId, itemId, readOnly, matrixValues, hasMuliEvaluation, formItemConfigs = [], countOfColumnsValue = 1, selectedValueIdOrIds = null) {
    const useMulti = !!hasMuliEvaluation && Array.isArray(formItemConfigs) && formItemConfigs.length > 0;

    if (!useMulti) {
        const isSelected = (id) => selectedValueIdOrIds && String(id) === String(selectedValueIdOrIds);
        const optionsHtml = matrixValues
            .map(o => `<option value="${o.id}" data-actual-value="${o.actualMatrixValue}" ${isSelected(o.id) ? 'selected' : ''}>${o.actualMatrixValue}</option>`)
            .join('');

        return `
            <div class="row-select-wrap">
                <select class="row-select" ${readOnly ? 'disabled' : ''}>
                    <option disabled ${selectedValueIdOrIds ? '' : 'selected'}>${selectPleaceHolder}</option>
                    ${optionsHtml}
                </select>
                <span class="validation-message" id="validation-${fieldId}-${itemId}-${ItemPropertyType.SELECT}"></span>
            </div>
        `;
    }

    const selectedValueIds = Array.isArray(selectedValueIdOrIds) ? selectedValueIdOrIds : [];
    return Array.from({ length: countOfColumnsValue }, (_, index) =>
        renderMultiSelectWrap(fieldId, itemId, readOnly, matrixValues, formItemConfigs, index, selectedValueIds[index] ?? null)
    ).join('');
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
            <input class="row-name item-name" type="text" value="${escapeAttr(text)}" readonly style="flex:1 1 auto;min-width:0;" />
            ${hasRelated
            ? `<span class="info-icon info-button"
                     title="عرض البنود المرتبطة"
                     onclick="openRelatedItemModal('${fieldId}', '${itemId}')"
                     ><i class="las la-info-circle"></i></span>`
            : ''}
        </div>
    `;
}

function renderSubRowHtml(fieldId, sub, parentId, label, readOnly, matrixValues, savedValueId = null, savedNote = null) {
    return `
        <div class="row-item child-row" data-item-id="${sub.id}" data-parent-id="${parentId}" data-weight="${sub.weightPercentage}">
            <span class="row-index">
                <span class="row-index-num">${label}</span>
            </span>

            ${renderRowName(fieldId, sub.id, sub.text, sub.relatedItems)}

            ${renderSelectAndNote(fieldId, sub.id, sub.hasNote, readOnly, matrixValues, sub.subItemLists, savedValueId, savedNote)}
        </div>
    `;
}

// ==============================
// Rename-mode row rendering
// ==============================
// In rename mode, a row shows an editable name input (instead of the
// select+note pair) plus an optional delete button. No sub-items are
// rendered or tracked here - rename mode only ever operates on top-level
// row items, same as the old isRename branch of createRow().
//
// The "item-name" class and "data-id" attribute are kept identical to the
// old markup so getFormResult()/fillRenameControls() can keep using the
// same lookup convention ($(...).find("input.item-name").data("id")).
function renderRenameRowName(itemId, text, readOnly = false) {
    return `
        <div class="row-name-cell" style="display:flex;align-items:center;gap:6px;min-width:0;">
            <input class="row-name item-name" type="text" value="${escapeAttr(text)}" data-id="${itemId}"
                   ${readOnly ? 'readonly' : ''} style="flex:1 1 auto;min-width:0;" />
        </div>
    `;
}

function renderDeleteButton(allowDelete) {
    return `
        <div class="row-delete-wrap">
            ${allowDelete
            ? `<button type="button" class="btn btn-sm delete-btn" onclick="deleteRow(this)"><i class="la la-trash"></i></button>`
            : ''}
        </div>
    `;
}

// data-field-id/data-aspect-key let addNewRow/deleteRow find their way
// back to the right per-aspect pool in formStates without needing to
// thread fieldId/aspectKey through every caller.
//
// Rename-mode rows render ONLY the editable name input + delete button
// at creation time (initForm / addNewRow). They intentionally do NOT
// render any evaluation select(s) or a note input here - selects are
// inserted later, on demand, by fillRenameControls (see
// insertRenameRowSelects), once there's saved data to drive them. No
// note input is rendered for rename rows at all.
function renderRenameRowHtml(fieldId, aspectKey, item, orderLabel, allowDelete, state) {
    const readOnly = !!(state && state.readOnly);
    return `
        <div class="row-item main-row rename-row" data-item-id="${item.id}" data-field-id="${fieldId}" data-aspect-key="${aspectKey}" data-weight="${item.weightPercentage ?? 0}">
            <span class="row-index">
                <span class="row-index-num">${orderLabel}</span>
            </span>

            ${renderRenameRowName(item.id, item.text, readOnly)}

            ${renderDeleteButton(allowDelete)}
        </div>
    `;
}

// Wires change -> calculateFE on every select inside a freshly-inserted
// rename-mode row, same as eval-mode rows do. Needed because rename rows
// are inserted via insertAdjacentHTML (raw markup), so listeners have to
// be attached afterward rather than declared inline.
function bindRenameRowSelects(fieldId, rowEl) {
    if (!rowEl) return;
    const state = getFormState(fieldId);
    const formId = state.formId;

    rowEl.querySelectorAll('.row-select').forEach(sel => {
        sel.addEventListener('change', () => {
            calculateFE(formId, fieldId);
        });
    });
}

// Mirrors the old "Add New" button rendered below the table in
// generateFormAccordionItem() when allowAdd is true.
function renderAddNewRowButton(fieldId, aspectKey) {
    return `
        <div class="add-row-wrap">
            <button type="button" class="btn btn-sm add-btn" onclick="addNewRow(this, '${fieldId}', '${aspectKey}')">
                <i class="la la-plus"></i> Add New
            </button>
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
        <td>${escapeAttr(item.value ?? '')}</td>
        ${hasAnyNote ? `<td>${escapeAttr(item.note ?? '')}</td>` : ''}
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
                     <button type="button" class="btn btn-outline-secondary btn-sm" data-bs-dismiss="modal" aria-label="Close">
                        <i class="la la-close me-1 fs-14"></i><span class="close-text">Close</span>
                    </button>

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
// initForm no longer touches the DOM. It fetches the form's data,
// populates per-fieldId state, and returns the FULL HTML string. The
// caller inserts it then calls bindFormEvents(fieldId).
//
// savedResults — the full object returned by getFormResults():
//   { id, items: [{ id, name, valueId, valueIds, note, subItems }], ... }
//   Pass null for a blank/new form.
//
// evaluateRenamedItems — when true, the form is in "evaluate renamed
//   items" mode: row labels come from savedResults.items[i].name instead
//   of the API template name, and evaluation selects are rendered
//   alongside a readonly name display (no editable .item-name input).
//   Used for cases 5 & 6.
//
// Six cases covered:
//   1  Regular form, get result              — no savedResults, !evaluateRenamedItems
//   2  Regular form, fill eval (edit)        — savedResults with valueId, !evaluateRenamedItems
//   3  Rename edit                           — isRename, savedResults with name, !readOnly
//   4  Rename view                           — isRename, savedResults with name, readOnly
//   5  Eval renamed items, blank selects     — evaluateRenamedItems, savedResults with name only
//   6  Eval renamed items, prefilled selects — evaluateRenamedItems, savedResults with name+valueId
async function initForm(formId, academicYearId, fieldId, readOnly, savedResults, evaluationRequestId, serviceRequestId, allowRename = false, allowDelete = false, allowAdd = false, evaluateRenamedItems = false) {

    if (evaluationRequestId == null) {
        console.log("initialization failed !");
        return;
    }

    const state = getFormState(fieldId);
    state.formId = formId;
    state.evaluationRequestId = evaluationRequestId;
    state.serviceRequestId = serviceRequestId;
    state.readOnly = readOnly;
    state.evaluateRenamedItems = evaluateRenamedItems;

    const response = await jqClient().Get(
        GET_FORMS_API.getItems(depRoutePath, formId, academicYearId)
    );

    let tree = response?.value?.tree ?? [];
    state.evalForm = response?.value?.evalForm ?? null;
    state.hasMuliEvaluation = !!(state.evalForm && state.evalForm.hasMuliEvaluation);

    if (readOnly) {
        allowRename = false;
        allowDelete = false;
        allowAdd = false;
    }

    state.isRename = !!(state.evalForm && state.evalForm.allowRename);
    state.allowRename = allowRename;
    state.allowDelete = allowDelete;
    state.allowAdd = allowAdd;
    state.renamePools = new Map();
    state.renameItemsById = new Map();

    const matrixResponse = await jqClient().Get(
        GET_FORMS_API.getMatrixValues(depRoutePath, formId)
    );

    state.matrixResponse = matrixResponse;
    const matrixValues = matrixResponse?.value ?? matrixResponse ?? [];
    state.matrixValues = matrixValues;

    state.formData = buildFormData(tree, state.evalForm);

    state.relatedItemsMap = new Map();
    state.hasAnyNote = false;

    state.formData.forEach(crit => {
        crit.aspects.forEach(asp => {
            // aspects now carry domains[], each with rows[] — walk them all.
            asp.domains.forEach(domain => {
                domain.rows.forEach(row => {
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
    });

    // savedResults is the full getFormResults() object. Build lookup maps
    // from its .items array. Multi-evaluation rows are expanded into N
    // flat items with the same id (one per column) by getFormResult — so
    // we group them back by id here, collecting each column's valueId in
    // order, so renderSelectsAndNote can prefill column[i] from valueIds[i].
    const savedItems = savedResults?.items ?? [];
    const savedMap = {};   // id -> { name, valueIds: [], note, subItems }
    const savedSubMap = {};

    savedItems.forEach(r => {
        if (!savedMap[r.id]) {
            savedMap[r.id] = {
                name: r.name ?? null,
                valueIds: [],
                note: r.note ?? null,
                subItems: r.subItems ?? []
            };
        }
        // Collect valueId for each column in the order they appear.
        // Single-select rows produce exactly one entry → valueIds[0].
        if (r.valueId !== undefined) {
            savedMap[r.id].valueIds.push(r.valueId ?? null);
        }
        (r.subItems || []).forEach(sub => { savedSubMap[sub.id] = sub; });
    });

    const tableHtml = buildHorizontalTableHtml(matrixValues);

    const resultBannerHtml = `
        <div id="${fieldId}-form-result-div" class="d-none result-bg d-flex justify-content-between align-items-center py-2 px-4 mb-3">
            <div class="text-white">Result:</div>
            <div class="text-white" id="${fieldId}-form-result-value"></div>
        </div>
    `;

    const critCardsHtml = state.formData.map((crit, cIdx) => {
        const critBodyId = `${fieldId}-crit-body-${cIdx}`;

        const aspectCardsHtml = crit.aspects
            // Keep only aspects that have at least one domain with rows.
            .filter(asp => asp.domains?.some(d => d.rows?.length > 0))
            .map((asp, aIdx) => {
                const aId = `${fieldId}-asp-${cIdx}-${aIdx}`;

                // domainsHtml accumulates one <div class="domain-row"> per
                // domain in the aspect. For D=1/D=2 there is exactly one
                // domain (no label shown); for D=3+ there are multiple,
                // each showing its title in the left domain-title column.
                //
                // Rename mode (Branch A) always has a single implicit domain
                // (rename forms are always D=2). The rows-area id is kept as
                // "rows-${aId}" so the rename pool lookup in addNewRow /
                // deleteRow / fillRenameControls works unchanged.
                //
                // Eval mode with multiple domains (Branch C, D=3) uses
                // "rows-${aId}-dom-${dIdx}" so each domain has a unique id,
                // while a single-domain aspect keeps "rows-${aId}" for
                // backward compatibility with any external callers.
                let domainsHtml = '';
                let addButtonHtml = '';

                // Helper: build the rows HTML for one flat list of rows.
                // Used by branches B and C to avoid repeating the row
                // rendering logic for each domain.
                const buildRowsHtml = (rows, useDisplayName = false) =>
                    rows.map((row, rIdx) => {
                        const hasSubItems = Array.isArray(row.subItems) && row.subItems.length > 0;
                        const saved = savedMap[row.id];
                        const savedValueIdOrIds = saved?.valueIds?.length > 0 ? saved.valueIds : null;
                        const displayName = useDisplayName ? (saved?.name ?? row.text) : row.text;

                        const mainRowHtml = `
                            <div class="row-item main-row${hasSubItems ? ' has-subitems' : ''}" data-item-id="${row.id}" data-weight="${row.weightPercentage}">
                                <span class="row-index">
                                    <span class="row-index-num">${rIdx + 1}</span>
                                    ${hasSubItems
                                ? `<button type="button" class="row-toggle" aria-expanded="false" aria-label="toggle sub items"><i class="las la-angle-right"></i></button>`
                                : ''}
                                </span>

                                ${renderRowName(fieldId, row.id, displayName, row.relatedItems)}

                                ${renderSelectsAndNote(
                                    fieldId, row.id, row.hasNote, readOnly, matrixValues,
                                    state.hasMuliEvaluation, row.formItemConfigs,
                                    state.evalForm?.evalCountOfColumnsValue || 1,
                                    savedValueIdOrIds,
                                    saved?.note ?? null
                                )}
                            </div>
                        `;

                        let subItemsHtml = '';
                        if (hasSubItems) {
                            const subRowsHtml = row.subItems.map((sub, sIdx) => {
                                const savedSub = savedSubMap[sub.id];
                                return renderSubRowHtml(
                                    fieldId, sub, row.id, `${rIdx + 1}.${sIdx + 1}`, readOnly, matrixValues,
                                    savedSub?.valueId ?? null,
                                    savedSub?.note ?? null
                                );
                            }).join('');

                            subItemsHtml = `
                                <div class="subitems-container" data-parent-id="${row.id}">
                                    ${subRowsHtml}
                                </div>
                            `;
                        }

                        return mainRowHtml + subItemsHtml;
                    }).join('');

                // ── Branch A: rename mode (cases 3 & 4) ──────────────────
                // Rename forms are always D=2 (one implicit domain per aspect).
                // We flatten all domain rows (there will only be one domain)
                // and treat them as a single pool, same as before.
                if (state.isRename && !evaluateRenamedItems) {
                    const allRows = asp.domains.flatMap(d => d.rows);

                    allRows.forEach(row => {
                        state.renameItemsById.set(row.id, { item: row, aspectKey: aId });
                    });

                    let rowsAreaHtml = '';
                    if (savedItems.length > 0) {
                        const savedIds = new Set(savedItems.map(s => s.id));
                        const poolRows = allRows.filter(row => !savedIds.has(row.id));
                        state.renamePools.set(aId, poolRows);

                        rowsAreaHtml = allRows
                            .filter(row => savedIds.has(row.id))
                            .map((row, idx) => {
                                const saved = savedMap[row.id];
                                const rowWithSavedName = { ...row, text: saved?.name ?? '' };
                                return renderRenameRowHtml(fieldId, aId, rowWithSavedName, idx + 1, state.allowDelete, state);
                            }).join('');
                    } else {
                        const [firstRow, ...poolRows] = allRows;
                        state.renamePools.set(aId, poolRows);
                        if (firstRow) {
                            rowsAreaHtml = renderRenameRowHtml(fieldId, aId, firstRow, 1, state.allowDelete, state);
                        }
                    }

                    if (state.allowAdd && state.renamePools.get(aId)?.length > 0) {
                        addButtonHtml = renderAddNewRowButton(fieldId, aId);
                    }

                    // Single implicit domain — no label, rows-area id = aId
                    // so rename pool lookups keep working unchanged.
                    domainsHtml = `
                        <div class="domain-row">
                            <div class="domain-title"></div>
                            <div class="rows-area" id="rows-${aId}">${rowsAreaHtml}</div>
                        </div>
                    `;

                    // ── Branch B: evaluate renamed items (cases 5 & 6) ───────
                } else if (evaluateRenamedItems) {
                    domainsHtml = asp.domains.map((domain, dIdx) => {
                        const dId = asp.domains.length === 1 ? aId : `${aId}-dom-${dIdx}`;
                        // Only render rows that were actually renamed (exist in savedResults).
                        const renamedRows = domain.rows.filter(row => savedMap[row.id]);
                        const rowsAreaHtml = buildRowsHtml(renamedRows, /* useDisplayName */ true);

                        return `
                            <div class="domain-row">
                                <div class="domain-title">${domain.title ? escapeAttr(domain.title) : ''}</div>
                                <div class="rows-area" id="rows-${dId}">${rowsAreaHtml}</div>
                            </div>
                        `;
                    }).join('');

                    // ── Branch C: regular eval (cases 1 & 2) ─────────────────
                } else {
                    domainsHtml = asp.domains.map((domain, dIdx) => {
                        const dId = asp.domains.length === 1 ? aId : `${aId}-dom-${dIdx}`;
                        const rowsAreaHtml = buildRowsHtml(domain.rows);

                        return `
                            <div class="domain-row">
                                <div class="domain-title">${domain.title ? escapeAttr(domain.title) : ''}</div>
                                <div class="rows-area" id="rows-${dId}">${rowsAreaHtml}</div>
                            </div>
                        `;
                    }).join('');
                }

                // D=1 (asp.isDirect): this "aspect" is just the criterion's
                // own items with no separate aspect concept above them - its
                // title is a duplicate of the criterion title, so we skip the
                // aspect-card wrapper/header entirely and let the rows sit
                // straight inside the criterion body. The rows-area/domain
                // divs (with their ids used by rename add/delete lookups)
                // are still rendered as-is via domainsHtml.
                if (asp.isDirect) {
                    return `${domainsHtml}${addButtonHtml}`;
                }

                return `
                    <div class="aspect-card">
                        <div class="aspect-header">
                            <span class="aspect-label">جانب ${aIdx + 1}</span>
                            <input class="aspect-title-input" type="text" value="${escapeAttr(asp.title)}" readonly />
                        </div>

                        ${domainsHtml}

                        ${addButtonHtml}
                    </div>
                `;
            }).join('');

        return `
            <div class="criterion-card">
                <div class="criterion-header">
                    <span class="criterion-number">معيار ${cIdx + 1}</span>
                    <input class="criterion-title-input" type="text" value="${escapeAttr(crit.title)}" readonly />
                </div>
                <div class="criterion-body" id="${critBodyId}">${aspectCardsHtml}</div>
            </div>
        `;
    }).join('');

    return `${tableHtml}${critCardsHtml}${resultBannerHtml}`;
}

// Inserts the HTML returned by initForm into the page and wires up all
// event listeners (row-select change -> calculateFE, row-toggle
// expand/collapse, rename-row selects once they exist). Split out from
// initForm because listeners can't be serialized into the HTML string -
// call this right after assigning the returned markup into
// `#${fieldId}`.
//
// Typical usage:
//   const html = await initForm(formId, fieldId, ...);
//   document.getElementById(`${fieldId}`).innerHTML = html;
//   bindFormEvents(fieldId);
function bindFormEvents(fieldId) {
    const state = getFormState(fieldId);
    const root = document.getElementById(`${fieldId}`);

    if (!root) {
        console.error(`bindFormEvents: no element with id "${fieldId}" found on the page.`);
        return;
    }

    const formId = state.formId;

    // Eval-mode rows: change -> recalc, toggle -> expand/collapse
    root.querySelectorAll('.row-item.main-row:not(.rename-row) .row-select').forEach(sel => {

        if (sel.selectedIndex > 0) {
            sel.classList.add("selected");
        }

        sel.addEventListener('change', function () {
            this.classList.toggle("selected", this.selectedIndex > 0);
            calculateFE(formId, fieldId);
        });
    });

    root.querySelectorAll('.row-item.child-row .row-select').forEach(sel => {

        if (sel.selectedIndex > 0) {
            sel.classList.add("selected");
        }

        sel.addEventListener('change', function () {
            this.classList.toggle("selected", this.selectedIndex > 0);
            calculateFE(formId, fieldId);
        });
    });


    root.querySelectorAll('.row-item.main-row.has-subitems').forEach(rowDiv => {
        const itemId = rowDiv.dataset.itemId;
        const subContainer = root.querySelector(`.subitems-container[data-parent-id="${itemId}"]`);
        const toggleBtn = rowDiv.querySelector('.row-toggle');
        if (!subContainer || !toggleBtn) return;

        const clickTarget = rowDiv.querySelector('.row-index');

        clickTarget.addEventListener('click', () => {
            const expanded = subContainer.classList.toggle('expanded');

            toggleBtn.classList.toggle('is-open', expanded);
            toggleBtn.setAttribute('aria-expanded', String(expanded));
        });
    });

    // Rename-mode rows: only the first row per aspect exists at this
    // point (the rest live in state.renamePools until added) - bind
    // whatever selects are already present (normally none, since rename
    // rows get their selects inserted later by fillRenameControls, but
    // this stays harmless/idempotent either way).
    root.querySelectorAll('.row-item.rename-row').forEach(rowEl => {
        bindRenameRowSelects(fieldId, rowEl);
    });
}


// ==============================
// Rename mode: add / delete / collect / prefill
// ==============================
// Ported from get-forms_Old.js (addNewRow, deleteRow, fillRenameControls)
// and submit-form_Old.js (renameFormItems). The old versions worked
// against a single flat table (one tbody, one "renameItems" pool, one
// global "lastOrder"); these versions are scoped per fieldId AND per
// aspect, since rename mode now renders grouped under criterion/aspect
// cards like eval mode instead of one flat table.

// Pulls the next pooled item for this aspect into the DOM. `button` is
// the clicked "Add New" button (used to hide itself once the pool is
// empty) - pass null when calling programmatically, e.g. from
// fillRenameControls.
function addNewRow(button, fieldId, aspectKey) {
    const state = getFormState(fieldId);
    const pool = state.renamePools.get(aspectKey);

    if (!pool || pool.length === 0) {
        return; // nothing left in this aspect's pool to add
    }

    const item = pool.shift();
    const rowsArea = document.getElementById(`rows-${aspectKey}`);
    if (!rowsArea) return;

    const order = rowsArea.querySelectorAll('.row-item.rename-row').length + 1;

    rowsArea.insertAdjacentHTML(
        'beforeend',
        renderRenameRowHtml(fieldId, aspectKey, item, order, state.allowDelete, state)
    );
    bindRenameRowSelects(fieldId, rowsArea.lastElementChild);

    if (pool.length === 0 && button) {
        button.style.display = 'none';
    }
}

// Removes a rename row from the DOM, renumbers the remaining rows in
// that aspect, and returns the item to the back of its aspect's pool
// (using the original item data, not whatever the user may have typed
// into the name field - mirrors the old itemsResult lookup).
function deleteRow(button) {
    const row = button.closest('.row-item.rename-row');
    if (!row) return;

    const itemId = row.dataset.itemId;
    const fieldId = row.dataset.fieldId;
    const aspectKey = row.dataset.aspectKey;

    const state = getFormState(fieldId);
    const meta = state.renameItemsById.get(itemId);

    row.remove();

    const rowsArea = document.getElementById(`rows-${aspectKey}`);
    if (rowsArea) {
        rowsArea.querySelectorAll('.row-item.rename-row').forEach((r, index) => {
            const numEl = r.querySelector('.row-index-num');
            if (numEl) numEl.textContent = index + 1;
        });
    }

    if (!meta) return;

    const pool = state.renamePools.get(aspectKey) || [];
    pool.push(meta.item);
    state.renamePools.set(aspectKey, pool);

    // Re-show this aspect's "Add New" button if it had been hidden when
    // the pool ran out.
    const addBtn = rowsArea?.closest('.aspect-card')?.querySelector('.add-btn');
    if (addBtn) addBtn.style.display = '';
}

// renameFormItems was retired - its logic (collecting the edited `name`
// per rename row) is now folded into getFormResult, which reports both
// the rename payload (name) and the eval payload (valueId/value/note/
// subItems) together for every row. See getFormResult below.

// Inserts the evaluation select(s) into a single rename row, on demand.
// Safe to call more than once per row - if selects are already present
// (e.g. the row was already filled by an earlier fillRenameControls
// pass) it does nothing and returns the existing wraps unchanged. The
// select(s) are inserted right before the delete-button wrap so the
// column order stays name -> select(s) -> delete, matching eval-mode
// rows' name -> select(s) -> note layout.
function insertRenameRowSelects(fieldId, row, item, state, selectedValueIdOrIds = null) {
    if (row.querySelector('.row-select')) return; // already inserted

    const matrixValues = state.matrixValues ?? (state.matrixResponse?.value ?? state.matrixResponse ?? []);
    const countOfColumnsValue = state.evalForm?.evalCountOfColumnsValue || 1;

    const selectsHtml = renderSelectsOnly(
        fieldId, item.id, false, matrixValues,
        state.hasMuliEvaluation, item.formItemConfigs, countOfColumnsValue,
        selectedValueIdOrIds
    );

    const deleteWrap = row.querySelector('.row-delete-wrap');
    if (deleteWrap) {
        deleteWrap.insertAdjacentHTML('beforebegin', selectsHtml);
    } else {
        row.insertAdjacentHTML('beforeend', selectsHtml);
    }

    bindRenameRowSelects(fieldId, row);
}

// Inserts the shared multi-evaluation column-header row once above an
// aspect's rename rows (see renderMultiSelectHeaderRow). No-op when the
// form isn't multi-evaluation, or the header was already inserted for
// this aspect. Mirrors the per-row column labels eval-mode used to show,
// but rendered once for the whole aspect instead of once per row - see
// request to stop repeating the column title on every row.
function insertRenameAspectHeader(fieldId, aspectKey, item, rowsArea, state) {
    if (!state.hasMuliEvaluation) return;
    if (rowsArea.previousElementSibling?.classList?.contains('row-select-header')) return; // already inserted

    const countOfColumnsValue = state.evalForm?.evalCountOfColumnsValue || 1;
    const headerHtml = renderMultiSelectHeaderRow(item.formItemConfigs || [], countOfColumnsValue);

    rowsArea.insertAdjacentHTML('beforebegin', headerHtml);
}

// Prefills previously-saved rename values (e.g. a prior renameFormItems
// payload) into the rendered form. For each saved item: if its row is
// already on the page, just set the name; otherwise pull it (and
// anything queued ahead of it) out of its aspect's pool via addNewRow
// until it appears. Call this after the caller has inserted initForm's
// returned HTML into the page (and ideally after bindFormEvents).
//
// This is also where the evaluation select(s) actually get built for
// rename rows - they are NOT rendered upfront by initForm/addNewRow.
// Once a row's select(s) are inserted, the shared column-header row for
// that aspect is inserted too (multi-evaluation forms only), then the
// saved valueId/valueIds are applied. There is no note input in rename
// mode, so no note prefill happens here.
async function fillRenameControls(fieldId, controlValues) {
    if (!controlValues || !controlValues.items) return;

    const state = getFormState(fieldId);
    if (!state.isRename) return;

    // Group expanded items by id (multi-eval produces N flat items with
    // the same id, one per column) to reconstruct the valueIds array
    // needed by insertRenameRowSelects. Single-select rows produce one
    // entry → valueIds: [valueId]. Also deduplicate so we process each
    // row id exactly once.
    const groupedById = new Map();
    controlValues.items.forEach(r => {
        if (!groupedById.has(r.id)) {
            groupedById.set(r.id, { id: r.id, name: r.name ?? null, valueIds: [] });
        }
        groupedById.get(r.id).valueIds.push(r.valueId ?? null);
    });

    groupedById.forEach(savedItem => {
        const meta = state.renameItemsById.get(savedItem.id);
        if (!meta) return;

        const { item, aspectKey } = meta;
        const rowsArea = document.getElementById(`rows-${aspectKey}`);
        if (!rowsArea) return;

        let row = rowsArea.querySelector(`.row-item.rename-row[data-item-id="${savedItem.id}"]`);

        if (!row) {
            const pool = state.renamePools.get(aspectKey) || [];
            const poolIndex = pool.findIndex(p => p.id === savedItem.id);
            if (poolIndex === -1) return;

            for (let i = 0; i <= poolIndex; i++) {
                addNewRow(null, fieldId, aspectKey);
            }

            if ((state.renamePools.get(aspectKey) || []).length === 0) {
                const addBtn = rowsArea.closest('.aspect-card')?.querySelector('.add-btn');
                if (addBtn) addBtn.style.display = 'none';
            }

            row = rowsArea.querySelector(`.row-item.rename-row[data-item-id="${savedItem.id}"]`);
        }

        if (!row) return;

        const nameInput = row.querySelector('.item-name');
        if (nameInput) nameInput.value = savedItem.name || '';

        // savedItem.valueIds is the grouped array: [valueId_col0, valueId_col1, ...]
        const savedValueIdOrIds = savedItem.valueIds.length > 0 ? savedItem.valueIds : null;

        insertRenameAspectHeader(fieldId, aspectKey, item, rowsArea, state);
        insertRenameRowSelects(fieldId, row, item, state, savedValueIdOrIds);
    });
}

// ==============================
// Build payload from current DOM state
// ==============================
// Produces the canonical result object consumed by getFormResults,
// validateForm, calculateFE, and saved back as savedResults into initForm.
//
// Every item always carries:
//   - id, name (null for eval-mode rows with no .item-name input)
//   - valueId, value, weightPercentage, note
//   - subItems[]
//
// Multi-evaluation rows (hasMuliEvaluation, N selects per row) are
// EXPANDED into N individual flat items — one per select column — each
// carrying its own valueId/value/weightPercentage. Array fields
// (valueIds/values/weightPercentages) are NOT produced. This keeps the
// payload shape uniform regardless of how many columns a form has, and
// lets initForm prefill each column independently via savedResults.items.
//
// calculateFE reads item.value * item.weightPercentage per item, so the
// expanded shape produces identical arithmetic to the old array shape.
//
// Scoped to this form's own container so it never picks up rows
// belonging to a different form rendered on the same page.
function getFormResult(formId, fieldId) {
    const state = getFormState(fieldId);
    const root = document.getElementById(`${fieldId}`);

    if (!root) {
        console.error(`getFormResult: no element with id "${fieldId}" found.`);
        return { id: formId, items: [], formSettings: state.evalForm ?? null };
    }

    const items = [];

    root.querySelectorAll('.row-item.main-row').forEach(row => {
        const itemId = row.dataset.itemId;

        const nameInput = row.querySelector('.item-name');
        const name = nameInput ? (nameInput.value.trim() || null) : null;

        const noteEl = row.querySelector('.row-note');
        const note = noteEl ? (noteEl.value.trim() || null) : null;

        const selectEls = Array.from(row.querySelectorAll('.row-select'));

        const subItems = [];
        root.querySelectorAll(`.row-item.child-row[data-parent-id="${itemId}"]`).forEach(childRow => {
            const childSelect = childRow.querySelector('.row-select');
            const childNote = childRow.querySelector('.row-note');

            subItems.push({
                id: childRow.dataset.itemId,
                valueId: childSelect?.value || null,
                value: childSelect?.selectedOptions[0]?.textContent ?? null,
                note: childNote ? (childNote.value.trim() || null) : null
            });
        });

        if (selectEls.length > 1) {
            // Multi-evaluation: expand into one flat item per select column.
            // Each carries its own valueId/value/weightPercentage so the
            // payload is always a uniform array of flat items.
            selectEls.forEach(sel => {
                items.push({
                    id: itemId,
                    name: name,
                    valueId: sel.value || null,
                    value: parseFloat(sel.selectedOptions[0]?.dataset.actualValue) || null,
                    weightPercentage: parseFloat(sel.dataset.configWeightPercentage) || 0,
                    note: note,
                    subItems: subItems
                });
            });
        } else {
            // Single-select (the common case): one item as before.
            const sel = selectEls[0];
            items.push({
                id: itemId,
                name: name,
                valueId: sel?.value || null,
                value: sel ? (parseFloat(sel.selectedOptions[0]?.dataset.actualValue) || null) : null,
                weightPercentage: parseFloat(row.dataset.weight) || 0,
                note: note,
                subItems: subItems
            });
        }
    });

    return {
        id: formId,
        items: items,
        formSettings: state.evalForm ?? null
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
    const formResult = getFormResult(formId, fieldId);
    const result = { Value: 0, Name: null, Id: '00000000-0000-0000-0000-000000000000' };
    const hasMuliEvaluation = state.evalForm.hasMuliEvaluation;
    const countOfColumnsValue = state.evalForm.evalCountOfColumnsValue;

    switch (state.evalForm?.calcMethod) {
        case "AVERAGE": {
            const hasWeights = formResult.items.some(item => item.weightPercentage > 0);

            let total = 0;
            formResult.items.forEach((item) => {
                total += hasMuliEvaluation
                    ? (item.value * (item.weightPercentage / 100)) || 0
                    : (item.value || 0);
            });

            result.Value = hasMuliEvaluation
                ? total / (formResult.items.length / countOfColumnsValue)
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
    const result = getFormResult(formId, fieldId);

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
async function getFormResults(formId, fieldId, enableValidation = false, enableCalculation = false) {


    if (enableValidation) {

        const isValid = await validateForm(formId, fieldId);
        if (!isValid) {
            toast('يوجد حقول غير مكتملة، يرجى مراجعة الأخطاء', fieldId);
            return null;
        }

    }

    const state = getFormState(fieldId);

    const result = getFormResult(formId, fieldId);
    let calculation;

    if (enableCalculation) {
        calculation = await calculate(result);
    }

    const finalResult = {
        id: result.id,
        items: result.items,
        formSettings: result.formSettings,
        results: calculation?.value ?? null,
        evaluationRequestId: state.evaluationRequestId ?? null,
        serviceRequestId: state.serviceRequestId ?? null
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
    const root = document.getElementById(`${fieldId}`);
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

// String-returning twin of buildHorizontalTable, used by initForm now
// that it returns an HTML string instead of building DOM nodes directly.
// Same markup/structure as the DOM version above.
function buildHorizontalTableHtml(data) {
    const nameCellsHtml = data.map(item => `<th>${escapeAttr(item.name)}</th>`).join('');
    const rangeCellsHtml = data.map(item => `<td>${item.displayRange}</td>`).join('');
    return `
<div style="border-radius:20px; overflow:hidden;" class="mb-3">
    <table class="table table-bordered table-hover align-middle text-center w-100 dataTable">
        <thead class="table-light">
            <tr>
                <th>Name</th>
                ${nameCellsHtml}
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Range</td>
                ${rangeCellsHtml}
            </tr>
        </tbody>
    </table>
</div>
`;
}