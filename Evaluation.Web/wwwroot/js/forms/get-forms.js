// ==============================
// Globals & Constants
// ==============================
const params = new URLSearchParams(window.location.search);
const FORM_ID = params.get('formId');

let matrixValues = [];
let itemsResult = [];

const SELECTORS = {
    tbody: 'tbodyRows'
};

// ==============================
// Utilities
// ==============================
const escapeHtml = (text = '') => {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
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
const generateAccordionItem = ({ id, title, icon, content, badge }) => `
<div class="accordion-item mb-3 rounded">
    <h2 class="accordion-header">
        <button class="accordion-button collapsed" type="button"
                data-bs-toggle="collapse"
                data-bs-target="#${id}">
            <div class="d-flex align-items-center gap-2 fs-18">
                <i class="la ${icon} text-primary fs-25"></i>
                <span class="fw-semibold">${escapeHtml(title)}</span>
                ${badge ? `<span class="badge bg-success ms-2">${escapeHtml(badge)}</span>` : ''}
            </div>
        </button>
    </h2>
    <div id="${id}" class="accordion-collapse collapse">
        <div class="accordion-body">${content}</div>
    </div>
</div>
`;

const generateFormAccordionItem = (rowsHtml, hasAnyNote) => `
<div class="accordion-item mb-3 rounded">
    <div id="item3" class="accordion-collapse collapse show">
        <div class="accordion-body">
            <table class="table table-bordered text-center align-middle">
                <thead class="table-grey">
                    <tr>
                        <th></th>
                        <th>#</th>
                        <th>المعايير</th>
                        <th>اختر التقييم</th>
                        ${hasAnyNote ? '<th>الشواهد وأثرها</th>' : ''}
                    </tr>
                </thead>
                <tbody id="${SELECTORS.tbody}">
                    ${rowsHtml}
                </tbody>
            </table>
        </div>
    </div>
</div>
`;

// ==============================
// Field Builders
// ==============================
const buildSelection = ({ id }, fieldId, readOnly) => `
<select class="form-select eval-select"
        id="${fieldId}_${id}_Select"
        data-id="${id}"
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
<button class="btn btn-sm"
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
    readOnly
}) => `
<tr class="${isChild ? 'child-row collapse' : 'main-row'} align-middle"
    ${isChild ? `id="${collapseId}" data-parent-id="${item.parentId}"` : ''}>
    
    ${hasChildrenColumn ? `
        <td>${!isChild && item.subFormItems?.length ? createToggleButton(collapseId) : ''}</td>
    ` : ''}

    <td>${order}</td>
    <td class="text-start">${escapeHtml(item.name)}</td>
    <td>${buildSelection(item, fieldId, readOnly)}</td>
    ${hasAnyNote ? `<td>${buildNote(item, fieldId, readOnly)}</td>` : ''}
</tr>
`;

// ==============================
// Table Generator
// ==============================
const generateTableBodyHtml = async (items, hasAnyNote, hasAnyChildren, fieldId, readOnly) => {
    return items.map((item, i) => {
        const collapseId = `collapse-${item.id}`;

        const mainRow = createRow({
            item,
            order: i + 1,
            collapseId,
            hasChildrenColumn: hasAnyChildren,
            hasAnyNote,
            fieldId,
            readOnly
        });

        const childrenRows = (item.subFormItems || []).map((child, idx) =>
            createRow({
                item: { ...child, parentId: item.id },
                order: `${i + 1}.${idx + 1}`,
                collapseId,
                isChild: true,
                hasChildrenColumn: hasAnyChildren,
                hasAnyNote,
                fieldId,
                readOnly
            })
        ).join('');

        return mainRow + childrenRows;
    }).join('');
};

// ==============================
// Page Generator
// ==============================
const generateFullFormPageHtml = async ({ title, fieldId, readOnly }) => {
    itemsResult = await jqClient().Get(`/Form/GetItems?formId=${FORM_ID}`);
    const items = itemsResult?.value ?? [];

    const hasAnyNote = items.some(i => i.hasNote);
    const hasAnyChildren = items.some(i => i.subFormItems?.length);

    const rowsHtml = await generateTableBodyHtml(
        items,
        hasAnyNote,
        hasAnyChildren,
        fieldId,
        readOnly
    );

    return `${generateFormAccordionItem(rowsHtml, hasAnyNote)}`;
};

// ==============================
// Initialize Controls
// ==============================
async function initializeControls(fieldId, controlValues) {

    const { value: items = [] } =
        await jqClient().Get(`/Form/GetItems?formId=${FORM_ID}`);

    const matrixResponse =
        await jqClient().Get(`/Form/GetFormEvalMarixValues?formId=${FORM_ID}`);

    matrixValues = matrixResponse?.value ?? matrixResponse ?? [];

    const populateSelect = (itemId, parentId) => {
        const select = document.getElementById(`${fieldId}_${itemId}_Select`);
        if (!select) return;

        select.add(createPlaceholderOption());

        matrixValues.forEach(({ id, name }) => {
            select.add(new Option(name, id));
        });

        if (controlValues != null) {
            if (parentId != null) {
                const result = controlValues.items
                    .flatMap(item => item.subItems)
                    .find(subItem => subItem.id === itemId);
                select.value = result.value;
            }
            else {
                const result = controlValues.items
                    .find(item => item.id === itemId)
                select.value = result.value;
            }
        }
    };

    items.forEach(item => {
        populateSelect(item.id);
        (item.subFormItems || []).forEach(child => populateSelect(child.id, item.id));
    });

    /*
    {
    "id": "b8fb67a9-b09a-4e0c-a466-d0625d92521d",
    "items": [
        {
            "id": "9115be55-4872-485f-9738-05dbdb77ffbe",
            "value": "8ae5207d-324b-4d20-ad29-036de6accb1c",
            "note": null,
            "subItems": [
                {
                    "id": "346b0ff1-bb12-4f34-be79-3dec77b2b93a",
                    "value": "8356b853-3526-4d57-991d-6f654aea11d9",
                    "note": null
                },
                {
                    "id": "7b3ded9e-6e78-41cf-9875-9c22430993d5",
                    "value": "8ae5207d-324b-4d20-ad29-036de6accb1c",
                    "note": null
                }
            ]
        },
        {
            "id": "1ab1a3f0-6e8f-43ab-b91d-b7803e7fa2d3",
            "value": "8356b853-3526-4d57-991d-6f654aea11d9",
            "note": null,
            "subItems": []
        }
    ],
    "strengths": null,
    "improvements": null
}
    */
 
}

// ==============================
// Init
// ==============================
document.addEventListener('DOMContentLoaded', async () => {
    try {
        const html = await generateFullFormPageHtml({
            title: 'استمارة المشاهدات الصفية',
            fieldId: 'ADD_YOUR_FIELD_ID_HERE',
            readOnly: false
        });

        document.getElementById('app').innerHTML = html;

    } catch (error) {
        console.error('Form builder error:', error);
    }
});
