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

//let matrixValues = [];
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


const escapeHtml = (text = '', isRename = false, itemId = null, fieldId = null, allowRename = false) => {

    if (!isRename) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
    else
    {
        const input = document.createElement("input");
        input.type = "text";
        input.className = "form-control form-control-sm item-name";
        input.id = `${fieldId}_${itemId}_ItemName`;
        input.setAttribute("data-id", itemId);

        if (!allowRename) {

            input.disabled = true;

        }

        return input.outerHTML;
 
    }

};

const createPlaceholderOption = (text = 'Please select') => {
    const option = new Option(text, '');
    option.disabled = true;
    option.selected = true;
    return option;
};

// ==============================
// Accordion Builders
// ==============================

const generateFormAccordionItem = (rowsHtml, hasAnyNote, hasAnyChildren, fieldId, isRename, allowDelete, allowAdd, hasMuliEvaluation, countOfColumnsValue, formItemConfigs) => `
<div class="accordion-item mb-3 rounded">
    <div id="item3" class="accordion-collapse collapse show">
        <div class="accordion-body">
        <div id="${fieldId}-index-table" class=""></div>
            <table id="${fieldId}" class="table table-bordered table-hover align-middle w-100 dataTable no-footer">
                <thead class="table-light">
                    <tr>
                        ${hasAnyChildren ? '<th></th>' : ''}
                        <th>#</th>
                         ${!isRename ?
                        `
                        <th>المعايير</th>
                       
                        ${hasMuliEvaluation
                                ? Array.from({ length: countOfColumnsValue }, (_, i) =>
                                    `<th>${formItemConfigs[i].formItemConfig_NameAr}</th>`
                                ).join('')
                                : '<th>اختر التقييم</th>'}
                                ${hasAnyNote ? '<th>الشواهد وأثرها</th>' : ''}
                        ` : ` <th>الاولويات</th> ${allowDelete ?'<th></th>':''}`}
                   
                    </tr>
                </thead>
                <tbody id="${fieldId}-${SELECTORS.tbody}">
                    ${rowsHtml}
                </tbody>
            </table>
            ${allowAdd ? `<div><button type="button" onclick="addNewRow(this)" class="btn btn-sm add-btn"><i class="la la-plus"></i> Add New</button></div>` : ''}
           
<div id="${fieldId}-result-div" class="d-none bg-primary d-flex justify-content-between align-items-center py-2">
  <div class="text-white">Result:</div>
  <div class="text-white" id="${fieldId}-result-value"></div>
</div>
           
           </div>
        </div>
    </div>
</div>
`;

// ==============================
// Field Builders
// ==============================
const buildSelection = (item, fieldId, readOnly, index) => `
<select class="form-select eval-select"
        id="${fieldId}_${item.id}_Select_${index}"
        data-id="${item.id}"
        data-config-weight-percentage="${item.formItemConfigs[index].formItemConfig_Percentage}"
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
<button type="button" class="btn btn-sm"
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
    
    ${hasChildrenColumn ? `
        <td>${!isChild && item.subFormItems?.length ? createToggleButton(collapseId) : ''}</td>
    ` : ''}
    <td>${order}</td>
    <td class="text-start">${escapeHtml(item.name, isRename, item.id, fieldId, allowRename)} 
    ${Array.isArray(item.relatedItems) && item.relatedItems.length > 0
        ? `<span class="info-icon" onclick="openRelatedItemModal('${item.id}')">ⓘ</span>`
        : ''
    }
    
    </td>
    ${!isRename ?
        `


    ${Array.from({ length: countOfColumnsValue }, (_, index) => `
    <td>
        ${buildSelection(item, fieldId, readOnly, index)}
        <span 
            class="validation-message text-danger small mt-1"
            id="validation-${item.id}-${ItemPropertyType.SELECT}_${index}"
            style="display:none;">
        </span>
    </td>
`).join('')}
    ${hasAnyNote ? `<td>${buildNote(item, fieldId, readOnly)}
        <span class="validation-message text-danger small mt-1" id="validation-${item.id}-${ItemPropertyType.NOTE}"style="display:none;"></span>
    </td>` : ''}

    ` : `${allowDelete ?`<td><button type="button" class="btn btn-sm delete-btn" onclick="deleteRow(this)"><i class="la la-trash"></i></button></td>`:''}`
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
    countOfColumnsValue) => {

    let firstitem = items[0];
    const firstitemCollapseId = `collapse-${firstitem.id}`;
    lastOrder = 1;
    if (isRename)
    {
        let firstRow = createRow({
            item: firstitem,
            order: lastOrder,
            collapseId:firstitemCollapseId,
            hasChildrenColumn: hasAnyChildren,
            hasAnyNote:hasAnyNote,
            fieldId:fieldId,
            readOnly:readOnly,
            isRename:isRename,
            allowRename:allowRename,
            allowDelete:allowDelete,
            hasMuliEvaluation:hasMuliEvaluation,
            countOfColumnsValue:countOfColumnsValue
        });

        renameItems.shift()

        return firstRow;
    }
    else
    {
        let rows = items.map((item, i) => {

            const collapseId = `collapse-${item.id}`;
            const mainRow = createRow({
                item:item,
                order: i + 1,
                collapseId:collapseId,
                hasChildrenColumn: hasAnyChildren,
                hasAnyNote:hasAnyNote,
                fieldId:fieldId,
                readOnly:readOnly,
                isRename:isRename,
                allowRename:allowRename,
                allowDelete:allowDelete,
                hasMuliEvaluation:hasMuliEvaluation,
                countOfColumnsValue: countOfColumnsValue
            });

            const childrenRows = (item.subFormItems || []).map((child, idx) =>
                createRow({
                    item: { ...child, parentId: item.id },
                    order: `${i + 1}.${idx + 1}`,
                    collapseId:collapseId,
                    isChild: true,
                    hasChildrenColumn: hasAnyChildren,
                    hasAnyNote:hasAnyNote,
                    fieldId:fieldId,
                    readOnly:readOnly,
                    isRename:isRename,
                    hasMuliEvaluation:hasMuliEvaluation,
                    countOfColumnsValue:countOfColumnsValue
                })
            ).join('');

            return mainRow + childrenRows;


        }).join('');

        return rows;
    }

};


function addNewRow(button)
{
    if (renameItems.length > 0) {

        let firstitem = renameItems[0];
        lastOrder = lastOrder + 1;
        const firstitemCollapseId = `collapse-${firstitem.id}`;

        let row = createRow({
            item: firstitem,
            order: lastOrder,
            collapseId: firstitemCollapseId,
            hasChildrenColumn: false,
            hasAnyNote: false,
            fieldId: P_fieldId,
            readOnly: false,
            isRename: P_isRename,
            allowRename: P_allowRename,
            allowDelete: P_allowDelete,
        });

        const tbody = document.getElementById(`${P_fieldId}-${SELECTORS.tbody}`);
        tbody.insertAdjacentHTML(
            "beforeend",
            row
        );

        renameItems.shift()
        if (renameItems.length == 0) {
            button.style.display = "none";
        }
    }
    else
    {
        //return error message
    }

}

const generateTableBodyHtmlForRelatedItems = async (items, hasAnyNote) => {
    return items.map((item, i) => {

        const mainRow = createRowRelatedItem({
            item,
            order: i + 1,
            hasAnyNote
        });
        return mainRow;
    }).join('');
};

async function fillRenameControls(fieldId, controlValues) {
    if (!controlValues || !controlValues.items) return;

    P_fieldId = fieldId;

    const tbody = $(`#${P_fieldId}-${SELECTORS.tbody}`);

    controlValues.items.forEach((item, index) => {
        let row = tbody.find("tr.main-row").filter(function () {
            return $(this).find("input.item-name").data("id") === item.id;
        });

        row.find("input.item-name").val(item.name || "");

        if (index < controlValues.items.length - 1) {
            addNewRow();
        }
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

// ==============================
// Page Generator 
// ==============================
const generateFullFormPageHtml = async ({ formId,
    fieldId,
    evaluationRequestId,
    serviceRequestId,
    readOnly,
    allowRename,
    allowDelete,
    allowAdd,
    namingResult = null }) => {

    P_evaluationRequestId = evaluationRequestId;
    P_serviceRequestId = serviceRequestId;

    itemsResult = await jqClient().Get(
        GET_FORMS_API.getItems(depRoutePath, formId)
    );

    evalForm = itemsResult?.value.evalForm;
    let items = itemsResult?.value.items ?? [];

    if (readOnly) {
        allowRename = false;
        allowDelete = false;
        allowAdd = false;
    }

    let isRename = itemsResult?.value.evalForm.allowRename
    let hasMuliEvaluation = evalForm.hasMuliEvaluation;
    let countOfColumnsValue = evalForm.evalCountOfColumnsValue;
    P_countOfColumnsValue = evalForm.countOfColumnsValue;


    let isRenamedEvaluation = false;

    if (allowRename == false && isRename == true && readOnly == false)
        isRenamedEvaluation = true;

    if (isRenamedEvaluation) {
        isRename = false;
        const map = new Map(namingResult.items.map(item => [item.id, item.name]));

        items = items
            .filter(item => map.has(item.id))
            .map(item => ({
                ...item,
                name: map.get(item.id) // replace name
            }));
    }
    

    const hasAnyNote = items.some(i => i.hasNote);
    const hasAnyChildren = items.some(i => i.subFormItems?.length);



    P_fieldId = fieldId;
    P_allowRename = allowRename;
    P_allowDelete = allowDelete;
    P_allowAdd = allowAdd;
    P_isRename = isRename;

    if (isRename)
    {
        renameItems = itemsResult?.value.items;
    }

    const rowsHtml = await generateTableBodyHtml(
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
    );

    return `${generateFormAccordionItem(rowsHtml, hasAnyNote, hasAnyChildren, fieldId, isRename, allowDelete, allowAdd, evalForm.hasMuliEvaluation, countOfColumnsValue, items[0].formItemConfigs)}`;
};


const relatedItemPopup = (rowsHtml) => `<div class="modal fade" id="RealatedItemModal" tabindex="-1" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header align-items-start border-0">
                <div>
                    <h4 class="modal-title fw-semibold mb-2" id="modalTitle"></h4>
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
</div>`;

// ==============================
// Initialize Controls
// ==============================
async function initializeControls(formId, fieldId, controlValues) {

    const matrixResponse = await jqClient().Get(
        GET_FORMS_API.getMatrixValues(depRoutePath, formId)
    );

    P_matrixResponse = matrixResponse;

    const items = itemsResult?.value ?? [];
    const matrixValues = matrixResponse?.value ?? matrixResponse ?? [];

    // Build lookup maps for faster access
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

    const matrixOptions = matrixValues.map(({ id, name, actualMatrixValue }) => {
        const option = new Option(name, id);

        option.setAttribute('data-actual-value', actualMatrixValue);

        return option;
    });

    function populateForm(itemId, isSubItem = false) {

        let length = 2;

        if (isSubItem) {
            length = 1;
        }

        for (var i = 0; i < length; i++) {
            const select = document.getElementById(`${fieldId}_${itemId}_Select_${i}`);

            if (!select) return;

            // Reset select
            select.length = 0;
            select.add(createPlaceholderOption());

            // Add matrix options
            matrixOptions.forEach(option =>
                select.add(option.cloneNode(true))
            );

            // Apply saved values
            const valueSource = isSubItem
                ? subItemValueMap.get(itemId)
                : itemValueMap.get(itemId);

            if (valueSource) {
                select.value = valueSource.valueId ?? "";
            }

            $(select).on("change", function () {
                calculateFE(select, formId);
            });
        }
     
        const note = document.getElementById(`${fieldId}_${itemId}_Note`);

    }

    // Populate main items and sub-items
    items.items.forEach(item => {
        populateForm(item.id, false);
        (item.subFormItems || []).forEach(subItem =>
            populateForm(subItem.id, true)
        );
    });

    const table = buildHorizontalTable(matrixValues);

    const container = document.getElementById(`${fieldId}-index-table`);

    container.appendChild(table);
}


async function openRelatedItemModal(id) {


    let relatedItems = itemsResult?.value.items.find(r => r.id == id)?.relatedItems ?? [];

    const relatedItemsRowsHtml = await generateTableBodyHtmlForRelatedItems(
        relatedItems
    );

    const popupHtml = await relatedItemPopup(relatedItemsRowsHtml);

    document.body.insertAdjacentHTML('beforeend', popupHtml);

    let modalElement = document.getElementById('RealatedItemModal');

    let modal = new bootstrap.Modal(modalElement);

    // Remove modal from DOM after it is closed
    modalElement.addEventListener('hidden.bs.modal', () => {
        modalElement.remove();
    });

    modal.show();
}

function deleteRow(button)
{
    const row = button.closest("tr");

    const input = row.querySelector('.item-name');

    const dataId = input.getAttribute('data-id');

    const item = itemsResult?.valueOrDefault.items.find(x => x.id === dataId);

    renameItems.push(item);

    if (row) {
        row.remove();
        const rows = document.querySelectorAll(`#${P_fieldId}-${SELECTORS.tbody} tr`);
        var lastIndex = 0;
        rows.forEach((tr, index) => {
            tr.children[0].textContent = index + 1;
            lastIndex = index + 1;
        });
        lastOrder = lastIndex;
    }

    if (renameItems.length > 0) {
        $(".add-btn").show();
    }
}