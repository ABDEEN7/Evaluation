// ==============================
// API Endpoints
// ==============================
const GET_FORMS_API = {
    getItems: (depRoutePath, formId) =>
        `/Form/${depRoutePath}/GetItems?formId=${formId}`,

    getMatrixValues: (depRoutePath, formId) =>
        `/Form/${depRoutePath}/GetFormEvalMarixValues?formId=${formId}`,
};

// ==============================
// Globals & Constants
// ==============================
const params = new URLSearchParams(window.location.search);

let depRoutePath = sharedUtility().extractDepartmentName();

let itemsResult = [];
let renameItems = [];
let lastOrder;

let P_readOnly;
let P_allowRename;
let P_isRename;
let P_allowDelete;
let P_allowAdd;
let P_evaluationRequestId;
let P_serviceRequestId;
let P_matrixResponse;
let evalForm;
let P_countOfColumnsValue;
let P_hasMuliEvaluation;
let P_fieldId;

const SELECTORS = {
    tbody: 'tbodyRows'
};

const ItemPropertyType = Object.freeze({
    SELECT: 1,
    NOTE: 2
});

// ==============================
// Utilities
// ==============================
const escapeHtml = (
    text = '',
    isRename = false,
    itemId = null,
    fieldId = null,
    allowRename = false
) => {

    if (!isRename) {

        const div = document.createElement('div');
        div.textContent = text;

        return div.innerHTML;
    }

    const input = document.createElement("input");

    input.type = "text";
    input.className = "form-control form-control-sm item-name";
    input.id = `${fieldId}_${itemId}_ItemName`;

    input.setAttribute("data-id", itemId);

    if (!allowRename) {
        input.disabled = true;
    }

    return input.outerHTML;
};

const createPlaceholderOption = (text = 'Please select') => {

    const option = new Option(text, '');

    option.disabled = true;
    option.selected = true;

    return option;
};

// ==============================
// Tree Helpers
// ==============================
function flattenTreeItems(nodes, result = []) {

    for (const node of nodes) {

        result.push(...(node.items || []));

        if (node.children?.length) {
            flattenTreeItems(node.children, result);
        }
    }

    return result;
}

//// ==============================
//// Accordion Builders
//// ==============================
//const generateFormAccordionItem = (
//    rowsHtml,
//    hasAnyNote,
//    hasAnyChildren,
//    fieldId,
//    isRename,
//    allowDelete,
//    allowAdd,
//    hasMuliEvaluation,
//    countOfColumnsValue,
//    formItemConfigs
//) => `
//<div class="accordion-item mb-3 rounded">

//    <div id="item3" class="accordion-collapse collapse show">

//        <div class="accordion-body">

//            <div id="${fieldId}-index-table" class=""></div>

//            <table id="${fieldId}"
//                   class="table table-bordered table-hover align-middle w-100 dataTable no-footer">

//                <thead class="table-light">

//                    <tr>

//                        ${hasAnyChildren ? '<th></th>' : ''}

//                        <th>#</th>

//                        ${!isRename
//        ? `
//                                <th>المعايير</th>

//                                ${hasMuliEvaluation
//            ? Array.from(
//                { length: countOfColumnsValue },
//                (_, i) =>
//                    `<th>${formItemConfigs?.[i]?.formItemConfig_NameAr ?? ''}</th>`
//            ).join('')
//            : '<th>اختر التقييم</th>'
//        }

//                                ${hasAnyNote ? '<th>الشواهد وأثرها</th>' : ''}
//                                `
//        : `
//                                <th>الاولويات</th>
//                                ${allowDelete ? '<th></th>' : ''}
//                                `
//    }

//                    </tr>

//                </thead>

//                <tbody id="${fieldId}-${SELECTORS.tbody}">
//                    ${rowsHtml}
//                </tbody>

//            </table>

//            ${allowAdd
//        ? `
//                    <div>
//                        <button type="button"
//                                onclick="addNewRow(this)"
//                                class="btn btn-sm add-btn">
//                            <i class="la la-plus"></i>
//                            Add New
//                        </button>
//                    </div>
//                    `
//        : ''
//    }

//            <div id="${fieldId}-result-div"
//                 class="d-none bg-primary d-flex justify-content-between align-items-center py-2">

//                <div class="text-white">Result:</div>

//                <div class="text-white"
//                     id="${fieldId}-result-value"></div>

//            </div>

//        </div>

//    </div>

//</div>
//`;

// ==============================
// Field Builders
// ==============================
const buildSelection = (
    item,
    fieldId,
    readOnly,
    index = 0
) => `
<select class="form-select eval-select"
        id="${fieldId}_${item.id}_Select_${index}"
        data-id="${item.id}"
        ${P_hasMuliEvaluation
        ? `data-config-weight-percentage="${item.formItemConfigs[index].formItemConfig_Percentage}"`
        : ``
    }
        ${readOnly ? 'disabled' : ''}>
</select>
`;

const buildNote = ({ id }, fieldId, readOnly) => `
<textarea class="form-control form-control-sm note-input"
          rows="2"
          id="${fieldId}_${id}_Note"
          data-id="${id}"
          placeholder="أضف ملاحظات..."
          ${readOnly ? 'disabled' : ''}></textarea>
`;

// ==============================
// Row Builders
// ==============================
const createToggleButton = (collapseId) => `
<button type="button"
        class="btn btn-sm"
        data-bs-toggle="collapse"
        data-bs-target="#${collapseId}">

    <i class="la la-plus"></i>

</button>
`;

const createRow = ({
    item,
    order,
    collapseId,
    isChild = false,
    hasChildrenColumn,
    hasAnyNote,
    fieldId,
    readOnly,
    isRename,
    allowRename,
    allowDelete,
    hasMuliEvaluation,
    countOfColumnsValue
}) => `
<tr class="${isChild ? 'child-row collapse' : 'main-row'} align-middle"
    ${isChild ? `id="${collapseId}" data-parent-id="${item.parentId}"` : ''}>

    ${hasChildrenColumn
        ? `
            <td>
                ${!isChild && item.subFormItems?.length
            ? createToggleButton(collapseId)
            : ''
        }
            </td>
            `
        : ''
    }

    <td>${order}</td>

    <td class="text-start">

        ${escapeHtml(item.name, isRename, item.id, fieldId, allowRename)}

        ${Array.isArray(item.relatedItems) &&
        item.relatedItems.length > 0
        ? `
                <span class="info-icon"
                      onclick="openRelatedItemModal('${item.id}')">
                    ⓘ
                </span>
                `
        : ''
    }

    </td>

    ${!isRename
        ? `
                ${hasMuliEvaluation
            ? `
                        ${Array.from(
                { length: countOfColumnsValue },
                (_, index) => `
                                <td>

                                    ${buildSelection(item, fieldId, readOnly, index)}

                                    <span class="validation-message text-danger small mt-1"
                                          id="validation-${item.id}-${ItemPropertyType.SELECT}_${index}"
                                          style="display:none;">
                                    </span>

                                </td>
                                `
            ).join('')
            }
                        `
            : `
                        <td>

                            ${buildSelection(item, fieldId, readOnly, 0)}

                            <span class="validation-message text-danger small mt-1"
                                  id="validation-${item.id}-${ItemPropertyType.SELECT}"
                                  style="display:none;">
                            </span>

                        </td>
                        `
        }

                ${hasAnyNote
            ? `
                        <td>

                            ${buildNote(item, fieldId, readOnly)}

                            <span class="validation-message text-danger small mt-1"
                                  id="validation-${item.id}-${ItemPropertyType.NOTE}"
                                  style="display:none;">
                            </span>

                        </td>
                        `
            : ''
        }
            `
        : `
                ${allowDelete
            ? `
                        <td>
                          <button
    type="button"
    data-field-id="${fieldId}"
    onclick="deleteRow(this)"
    class="btn btn-sm btn-danger">
    Delete
</button>
                        </td>
                        `
            : ''
        }
            `
    }

</tr>
`;

const createRowRelatedItem = ({
    item,
    order,
    hasAnyNote
}) => `
<tr class="main-row align-middle">

    <td>${order}</td>

    <td class="text-start">${escapeHtml(item.name)}</td>

    <td>${escapeHtml(item.value)}</td>

    ${hasAnyNote ? `<td>${escapeHtml(item.note)}</td>` : ''}

</tr>
`;

// ==============================
// Table Generator
// ==============================
const generateTableBodyHtml = async (
    items,
    hasAnyNote,
    hasAnyChildren,
    fieldId,
    readOnly,
    isRename,
    allowRename,
    allowDelete,
    allowAdd,
    hasMuliEvaluation,
    countOfColumnsValue
) => {

    if (!items?.length) {
        return '';
    }

    lastOrder = items.length;

    if (isRename) {

        let firstitem = items[0];

        const firstitemCollapseId = `collapse-${firstitem.id}`;

        let firstRow = createRow({
            item: firstitem,
            order: 1,
            collapseId: firstitemCollapseId,
            hasChildrenColumn: hasAnyChildren,
            hasAnyNote: hasAnyNote,
            fieldId: fieldId,
            readOnly: readOnly,
            isRename: isRename,
            allowRename: allowRename,
            allowDelete: allowDelete,
            hasMuliEvaluation: hasMuliEvaluation,
            countOfColumnsValue: countOfColumnsValue
        });

        renameItems.shift();

        return firstRow;
    }

    return items.map((item, i) => {

        const collapseId = `collapse-${item.id}`;

        const mainRow = createRow({
            item,
            order: i + 1,
            collapseId,
            hasChildrenColumn: hasAnyChildren,
            hasAnyNote,
            fieldId,
            readOnly,
            isRename,
            allowRename,
            allowDelete,
            hasMuliEvaluation,
            countOfColumnsValue
        });

        const childrenRows = (item.subFormItems || [])
            .map((child, idx) =>
                createRow({
                    item: {
                        ...child,
                        parentId: item.id
                    },
                    order: `${i + 1}.${idx + 1}`,
                    collapseId,
                    isChild: true,
                    hasChildrenColumn: hasAnyChildren,
                    hasAnyNote,
                    fieldId,
                    readOnly,
                    isRename,
                    hasMuliEvaluation,
                    countOfColumnsValue
                })
            )
            .join('');

        return mainRow + childrenRows;

    }).join('');
};

// ==============================
// Recursive Scope Renderer
// ==============================
const renderScopeTree = async (
    nodes,
    fieldId,
    readOnly,
    isRename,
    allowRename,
    allowDelete,
    allowAdd,
    hasMuliEvaluation,
    countOfColumnsValue
) => {

    let html = '';

    for (const node of nodes) {

        const items = node.items || [];

        const hasAnyNote = items.some(i => i.hasNote);

        const hasAnyChildren =
            items.some(i => i.subFormItems?.length);

        const rowsHtml = await generateTableBodyHtml(
            items,
            hasAnyNote,
            hasAnyChildren,
            `${fieldId}_${node.id}`,
            readOnly,
            isRename,
            allowRename,
            allowDelete,
            allowAdd,
            hasMuliEvaluation,
            countOfColumnsValue
        );

        html += `
        <div class="scope-container mb-4">

            <div class="scope-header p-3 rounded text-white"
                 style="background:${node.colorCode || '#0d6efd'}">

                <div class="d-flex justify-content-between align-items-center">

                    <div>

                        <h5 class="mb-0">
                            ${escapeHtml(node.scopeNameAr)}
                        </h5>

                        <small>
                            ${escapeHtml(node.scopeTypeNameAr)}
                        </small>

                    </div>

                </div>

            </div>

            ${items.length
                ? generateFormAccordionItem(
                    rowsHtml,
                    hasAnyNote,
                    hasAnyChildren,
                    `${fieldId}_${node.id}`,
                    isRename,
                    allowDelete,
                    allowAdd,
                    hasMuliEvaluation,
                    countOfColumnsValue,
                    items[0]?.formItemConfigs || []
                )
                : ''
            }

            <div class="scope-children ms-4 mt-3">

                ${await renderScopeTree(
                node.children || [],
                fieldId,
                readOnly,
                isRename,
                allowRename,
                allowDelete,
                allowAdd,
                hasMuliEvaluation,
                countOfColumnsValue
            )
            }

            </div>

        </div>
        `;
    }

    return html;
};

// ==============================
// Add/Delete
// ==============================
function addNewRow(button) {

    const fieldId = button.dataset.fieldId || P_fieldId;

    if (renameItems.length <= 0) {
        button.style.display = "none";
        return;
    }

    const firstItem = renameItems[0];

    lastOrder++;

    const collapseId = `collapse-${firstItem.id}`;

    const rowHtml = createRow({
        item: firstItem,
        order: lastOrder,
        collapseId: collapseId,
        hasChildrenColumn: false,
        hasAnyNote: false,
        fieldId: fieldId,
        readOnly: false,
        isRename: P_isRename,
        allowRename: P_allowRename,
        allowDelete: P_allowDelete,
    });

    const tbody = document.getElementById(
        `${fieldId}-${SELECTORS.tbody}`
    );

    tbody.insertAdjacentHTML("beforeend", rowHtml);

    renameItems.shift();

    // hide only THIS button if empty
    if (renameItems.length === 0) {
        button.style.display = "none";
    }
}

function deleteRow(button) {

    const fieldId = button.dataset.fieldId || P_fieldId;

    const row = button.closest("tr");

    const input = row.querySelector('.item-name');
    const dataId = input?.getAttribute('data-id');

    const allItems = flattenTreeItems(
        itemsResult?.value?.tree ?? []
    );

    const item = allItems.find(x => x.id === dataId);

    if (item) {
        renameItems.push(item);
    }

    row.remove();

    // ONLY current table rows
    const tbody = document.getElementById(
        `${fieldId}-${SELECTORS.tbody}`
    );

    const rows = tbody.querySelectorAll("tr");

    let lastIndex = 0;

    rows.forEach((tr, index) => {
        tr.children[0].textContent = index + 1;
        lastIndex = index + 1;
    });

    lastOrder = lastIndex;

    // show ONLY correct add button
    const addBtn = document.querySelector(
        `button.add-btn[data-field-id="${fieldId}"]`
    );

    if (addBtn && renameItems.length > 0) {
        addBtn.style.display = "inline-block";
    }
}
//// ==============================
//// Page Generator
//// ==============================
//const generateFullFormPageHtml = async ({
//    formId,
//    fieldId,
//    evaluationRequestId,
//    serviceRequestId,
//    readOnly,
//    allowRename,
//    allowDelete,
//    allowAdd,
//    namingResult = null
//}) => {

//    P_evaluationRequestId = evaluationRequestId;

//    P_serviceRequestId = serviceRequestId;

//    itemsResult = await jqClient().Get(
//        GET_FORMS_API.getItems(depRoutePath, formId)
//    );

//    evalForm = itemsResult?.value.evalForm;

//    let tree = itemsResult?.value.tree ?? [];

//    if (readOnly) {

//        allowRename = false;
//        allowDelete = false;
//        allowAdd = false;
//    }

//    let isRename = evalForm.allowRename;

//    let hasMuliEvaluation = evalForm.hasMuliEvaluation;

//    let countOfColumnsValue =
//        evalForm.evalCountOfColumnsValue;

//    P_countOfColumnsValue =
//        evalForm.evalCountOfColumnsValue;

//    P_hasMuliEvaluation =
//        evalForm.hasMuliEvaluation;

//    P_fieldId = fieldId;

//    P_allowRename = allowRename;

//    P_allowDelete = allowDelete;

//    P_allowAdd = allowAdd;

//    P_isRename = isRename;

//    if (isRename) {

//        renameItems = flattenTreeItems(tree);
//    }

//    const html = await renderScopeTree(
//        tree,
//        fieldId,
//        readOnly,
//        isRename,
//        allowRename,
//        allowDelete,
//        allowAdd,
//        hasMuliEvaluation,
//        countOfColumnsValue
//    );

//    return html;
//};

// ==============================
// Initialize Controls
// ==============================
async function initializeControls(
    formId,
    fieldId,
    controlValues
) {

    const matrixResponse = await jqClient().Get(
        GET_FORMS_API.getMatrixValues(depRoutePath, formId)
    );

    P_matrixResponse = matrixResponse;

    const tree = itemsResult?.value.tree ?? [];

    const items = flattenTreeItems(tree);

    const matrixValues =
        matrixResponse?.value ??
        matrixResponse ??
        [];

    const itemValueMap = new Map();

    const subItemValueMap = new Map();

    if (controlValues?.items?.length) {

        controlValues.items.forEach(item => {

            itemValueMap.set(item.id, item);

            (item.subItems || []).forEach(subItem => {
                subItemValueMap.set(subItem.id, subItem);
            });
        });
    }

    const matrixOptions = matrixValues.map(
        ({ id, name, actualMatrixValue }) => {

            const option = new Option(name, id);

            option.setAttribute(
                'data-actual-value',
                actualMatrixValue
            );

            return option;
        }
    );

    function populateForm(itemId, isSubItem = false) {

        let length = 1;

        if (P_hasMuliEvaluation) {
            length = P_countOfColumnsValue;
        }

        if (isSubItem) {
            length = 1;
        }

        for (let i = 0; i < length; i++) {

            const selects =
                document.querySelectorAll(
                    `[id$="${itemId}_Select_${i}"]`
                );

            selects.forEach(select => {

                select.length = 0;

                select.add(createPlaceholderOption());

                matrixOptions.forEach(option =>
                    select.add(option.cloneNode(true))
                );

                const valueSource = isSubItem
                    ? subItemValueMap.get(itemId)
                    : itemValueMap.get(itemId);

                if (valueSource) {
                    select.value =
                        valueSource.valueId ?? "";
                }

                $(select).on("change", function () {
                    calculateFE(select, formId);
                });
            });
        }
    }

    items.forEach(item => {

        populateForm(item.id, false);

        (item.subFormItems || []).forEach(subItem =>
            populateForm(subItem.id, true)
        );
    });

    const table = buildHorizontalTable(matrixValues);

    const containers =
        document.querySelectorAll(
            `[id$="-index-table"]`
        );

    containers.forEach(container => {
        container.appendChild(table.cloneNode(true));
    });
}

// ==============================
// Related Items
// ==============================
const relatedItemPopup = (rowsHtml) => `
<div class="modal fade"
     id="RealatedItemModal"
     tabindex="-1"
     aria-hidden="true">

    <div class="modal-dialog modal-xl modal-dialog-centered">

        <div class="modal-content">

            <div class="modal-header align-items-start border-0">

                <div>
                    <h4 class="modal-title fw-semibold mb-2"
                        id="modalTitle">
                    </h4>
                </div>

                <button type="button"
                        class="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close">
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
                                <th>ملاحظات</th>
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

const generateTableBodyHtmlForRelatedItems = async (
    items,
    hasAnyNote
) => {

    return items.map((item, i) =>
        createRowRelatedItem({
            item,
            order: i + 1,
            hasAnyNote
        })
    ).join('');
};

async function openRelatedItemModal(id) {

    const allItems = flattenTreeItems(
        itemsResult?.value.tree ?? []
    );

    let relatedItems =
        allItems.find(r => r.id == id)?.relatedItems ?? [];

    const relatedItemsRowsHtml =
        await generateTableBodyHtmlForRelatedItems(
            relatedItems
        );

    const popupHtml =
        await relatedItemPopup(
            relatedItemsRowsHtml
        );

    document.body.insertAdjacentHTML(
        'beforeend',
        popupHtml
    );

    let modalElement =
        document.getElementById('RealatedItemModal');

    let modal =
        new bootstrap.Modal(modalElement);

    modalElement.addEventListener(
        'hidden.bs.modal',
        () => {
            modalElement.remove();
        }
    );

    modal.show();
}

// ==============================
// Horizontal Matrix Table
// ==============================
function buildHorizontalTable(data) {

    const table = document.createElement("table");

    table.border = "1";

    table.style.borderCollapse = "collapse";

    table.className =
        "table table-bordered table-hover align-middle w-100 dataTable no-footer";

    const tHeadnameRow = document.createElement("thead");

    tHeadnameRow.className = 'table-light';

    const nameRow = document.createElement("tr");

    const rangeRow = document.createElement("tr");

    const nameCell = document.createElement("th");

    nameCell.textContent = "Name";

    nameRow.appendChild(nameCell);

    tHeadnameRow.appendChild(nameRow);

    const rangeCell = document.createElement("td");

    rangeCell.textContent = `Range`;

    rangeRow.appendChild(rangeCell);

    data.forEach(item => {

        const nameCell = document.createElement("th");

        nameCell.textContent = item.name;

        nameRow.appendChild(nameCell);

        const rangeCell = document.createElement("td");

        rangeCell.textContent =
            `(${item.minValue} - ${item.maxValue})`;

        rangeRow.appendChild(rangeCell);
    });

    table.appendChild(tHeadnameRow);

    table.appendChild(rangeRow);

    return table;
}

// ==============================
// Accordion Builders
// ==============================
const generateFormAccordionItem = (
    rowsHtml,
    hasAnyNote,
    hasAnyChildren,
    fieldId,
    isRename,
    allowDelete,
    allowAdd,
    hasMuliEvaluation,
    countOfColumnsValue,
    formItemConfigs
) => `
<div class="accordion-item mb-3 rounded">

    <div id="item3" class="accordion-collapse collapse show">

        <div class="accordion-body">

            <div id="${fieldId}-index-table" class=""></div>

            <table id="${fieldId}"
                   class="table table-bordered table-hover align-middle w-100 dataTable no-footer">

                <thead class="table-light">

                    <tr>

                        ${hasAnyChildren ? '<th></th>' : ''}

                        <th>#</th>

                        ${!isRename
        ? `
            <th>المعايير</th>

            ${hasMuliEvaluation
            ? Array.from(
                { length: countOfColumnsValue },
                (_, i) =>
                    `<th>${formItemConfigs?.[i]?.formItemConfig_NameAr ?? ''}</th>`
            ).join('')
            : '<th>اختر التقييم</th>'
        }

            ${hasAnyNote ? '<th>الشواهد وأثرها</th>' : ''}
        `
        : `
            <th>الاولويات</th>
            ${allowDelete ? '<th></th>' : ''}
        `
    }

                    </tr>

                </thead>

                <tbody id="${fieldId}-${SELECTORS.tbody}">
                    ${rowsHtml}
                </tbody>

            </table>

            ${allowAdd
        ? `
                <div>

                    <button
    type="button"
    class="btn btn-sm add-btn"
    data-field-id="${fieldId}"
    onclick="addNewRow(this)">
    <i class="la la-plus"></i> Add New
</button>
                </div>
                `
        : ''
    }

        </div>

    </div>

</div>
`;

// ==============================
// Global Result Builder
// ==============================
function buildGlobalResultHtml(fieldId) {

    return `
    <div id="${fieldId}-result-div"
         class="d-none bg-primary d-flex justify-content-between align-items-center py-2 px-3 rounded mt-4">

        <div class="text-white fw-bold">
            Result:
        </div>

        <div class="text-white"
             id="${fieldId}-result-value">
        </div>

    </div>
    `;
}

// ==============================
// Page Generator
// ==============================
const generateFullFormPageHtml = async ({
    formId,
    fieldId,
    evaluationRequestId,
    serviceRequestId,
    readOnly,
    allowRename,
    allowDelete,
    allowAdd,
    namingResult = null
}) => {

    P_evaluationRequestId = evaluationRequestId;
    P_serviceRequestId = serviceRequestId;

    const response = await jqClient().Get(
        GET_FORMS_API.getItems(depRoutePath, formId)
    );

    itemsResult = response;

    evalForm = response?.value?.evalForm;

    let tree = response?.value?.tree ?? [];

    // -----------------------------
    // readonly overrides
    // -----------------------------
    if (readOnly) {
        allowRename = false;
        allowDelete = false;
        allowAdd = false;
    }

    let isRename = evalForm.allowRename;
    let hasMuliEvaluation = evalForm.hasMuliEvaluation;
    let countOfColumnsValue = evalForm.evalCountOfColumnsValue;

    P_countOfColumnsValue = countOfColumnsValue;
    P_hasMuliEvaluation = hasMuliEvaluation;

    P_fieldId = fieldId;
    P_allowRename = allowRename;
    P_allowDelete = allowDelete;
    P_allowAdd = allowAdd;
    P_isRename = isRename;

    // -----------------------------
    // detect renamed evaluation mode
    // -----------------------------
    let isRenamedEvaluation = false;

    if (
        allowRename === false &&
        evalForm.allowRename === true &&
        readOnly === false &&
        namingResult?.items?.length
    ) {
        isRenamedEvaluation = true;
    }

    // -----------------------------
    // apply naming result
    // -----------------------------
    if (isRenamedEvaluation) {

        isRename = false;

        tree = applyNamingResultToTree(
            tree,
            namingResult.items
        );
    }

    // -----------------------------
    // rename mode setup
    // -----------------------------
    if (isRename) {
        renameItems = flattenTreeItems(tree);
    }

    // -----------------------------
    // render UI
    // -----------------------------
    const html = await renderScopeTree(
        tree,
        fieldId,
        readOnly,
        isRename,
        allowRename,
        allowDelete,
        allowAdd,
        hasMuliEvaluation,
        countOfColumnsValue
    );

    return html;
};

function applyNamingResultToTree(tree, namingItems) {

    const renameMap = new Map(
        (namingItems || []).map(x => [x.id, x.name])
    );

    function mapNodes(nodes) {

        return (nodes || []).map(node => ({

            ...node,

            items: (node.items || []).map(item => ({
                ...item,
                name: renameMap.get(item.id) ?? item.name
            })),

            children: mapNodes(node.children || [])
        }));
    }

    return mapNodes(tree);
}