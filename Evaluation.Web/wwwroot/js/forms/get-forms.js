// Constants
const params = new URLSearchParams(window.location.search);
const FORM_ID = params.get('formId');  
let matrixValues;
const SELECTORS = {
    noteHeader: 'thead th:contains("الشواهد وأثرها")',
    tableBody: '#tbodyRows',
    toggleColumn: 'thead th:first-child, tbody td:first-child'
};

// Initialize on DOM ready
document.addEventListener('DOMContentLoaded', () => {
    loadFormItems(FORM_ID);
});

/**
 * Loads and renders form items
 * @param {string} formId - The form identifier
 */
const loadFormItems = async (formId) => {
    try {
        const result = await jqClient().Get(`/Form/GetItems?formId=${formId}`);
        matrixValues = await jqClient().Get(`/Form/GetFormEvalMarixValues?formId=${formId}`);

        //const items = Array.isArray(result.result.value) ? result : [result];
        const items = result.value ? result.value : [result];

        // Check if any item has note
        const hasAnyNote = items.some(item => item.hasnote);

        // Check if any item has children (subFormItems)
        const hasAnyChildren = items.some(item => item.subFormItems?.length > 0);

        togglenoteColumn(hasAnyNote);
        toggleCollapseColumn(hasAnyChildren);
        await renderFormItems(items, hasAnyNote, hasAnyChildren);
    } catch (error) {
        console.error('Error loading form items:', error);
        // Consider adding user-friendly error handling here
    }
};

/**
 * Toggles the note column visibility
 * @param {boolean} show - Whether to show the column
 */
const togglenoteColumn = (show) => {
    const $header = $(SELECTORS.noteHeader);
    show ? $header.show() : $header.hide();
};

/**
 * Toggles the collapse/toggle column visibility
 * @param {boolean} show - Whether to show the column
 */
const toggleCollapseColumn = (show) => {
    const $toggleElements = $(SELECTORS.toggleColumn);
    show ? $toggleElements.show() : $toggleElements.hide();
};

/**
 * Renders all form items and their children
 * @param {Array} items - Array of form items
 * @param {boolean} hasAnyNote - Whether any item has note
 * @param {boolean} hasAnyChildren - Whether any item has children
 */
const renderFormItems = async (items, hasAnyNote, hasAnyChildren) => {
    const $tbody = $(SELECTORS.tableBody);
    $tbody.empty();

    items.forEach((item, index) => {
        const orderItem = index + 1;
        const collapseId = `collapse-${item.id}`;
        const hasChildren = item.subFormItems?.length > 0;

        // Render main row

        createMainRow(item, orderItem, collapseId, hasChildren, hasAnyNote, hasAnyChildren)
            .then(html => {
            console.log(html);
            $tbody.append(html);
        });

        // Render child rows only if there are any children in the dataset
        if (hasChildren && hasAnyChildren) {
            item.subFormItems.forEach((child, childIndex) => {
                const subOrder = `${orderItem}.${childIndex + 1}`;
                $tbody.append(createChildRow(child, item.id, collapseId, subOrder, hasAnyNote, hasAnyChildren));
            });
        }
    });
};

/**
 * Creates a main row element
 * @param {Object} item - Form item data
 * @param {number} orderItem - Row order number
 * @param {string} collapseId - Collapse identifier
 * @param {boolean} hasChildren - Whether item has children
 * @param {boolean} hasAnyNote - Whether note column is shown
 * @param {boolean} hasAnyChildren - Whether any item has children
 * @returns {string} HTML string for the row
 */
const createMainRow = async (item, orderItem, collapseId, hasChildren, hasAnyNote, hasAnyChildren) => {
    return `
    <tr class="main-row align-middle">
      ${hasAnyChildren ? `
      <td>
        ${hasChildren ? createToggleButton(collapseId) : ''}
      </td>
      ` : ''}
      <td class="serial">${orderItem}</td>
      <td class="text-start label">
        <div class="d-flex gap-2 align-items-center">
          <i class="la la-info-circle text-info"></i>
          ${escapeHtml(item.name)}
        </div>
      </td>
      <td>${await buildSelectionNew(item)}</td>
      ${hasAnyNote ? `<td>${item.hasnote ? buildnote(item) : ''}</td>` : ''}
    </tr>
  `;
};

/**
 * Creates a child row element
 * @param {Object} child - Child form item data
 * @param {string} parentId - Parent item ID
 * @param {string} collapseId - Collapse identifier
 * @param {string} subOrder - Sub-item order number
 * @param {boolean} hasAnyNote - Whether note column is shown
 * @param {boolean} hasAnyChildren - Whether any item has children
 * @returns {string} HTML string for the row
 */
const createChildRow = (child, parentId, collapseId, subOrder, hasAnyNote, hasAnyChildren) => {
    return `
    <tr class="collapse child-row"
        data-parent-id="${parentId}"
        data-bs-parent="#${collapseId}"
        id="${collapseId}">
      ${hasAnyChildren ? '<td></td>' : ''}
      <td class="serial">${subOrder}</td>
      <td class="text-start">${escapeHtml(child.name)}</td>
      <td>${buildSelection2(child)}</td>
      ${hasAnyNote ? `<td>${child.hasnote ? buildnote(child) : ''}</td>` : ''}
    </tr>
  `;
};

/**
 * Creates a toggle button for expandable rows
 * @param {string} collapseId - Collapse identifier
 * @returns {string} HTML string for the button
 */
const createToggleButton = (collapseId) => {
    return `
    <button class="toggle-btn collapsed" 
            type="button"
            data-bs-toggle="collapse" 
            data-bs-target="#${collapseId}"
            aria-expanded="false"
            aria-controls="${collapseId}">
      <i class="la la-plus"></i>
    </button>
  `;
};

/**
 * Builds selection dropdown based on item type
 * @param {Object} item - Form item data
 * @returns {string} HTML string for the select element
 */
const buildSelection = (item) => {
    const baseAttrs = `class="form-select eval-select" data-id="${item.id}"`;

    if (item.selectionCode === 'multiSelect') {
        return `
      <select ${baseAttrs}>
        <option value="">اختر من 1 إلى 5</option>
        ${[1, 2, 3, 4, 5].map(num => `<option value="${num}">${num}</option>`).join('')}
      </select>
    `;
    }

    return `
    <select ${baseAttrs}>
      <option value="">اختر</option>
      <option value="1">موافق</option>
      <option value="0">غير موافق</option>
    </select>
  `;
};

const buildSelectionNew = async (item) => {
    const baseAttrs = `class="form-select eval-select" data-id="${item.id}"`;

    let options = `<option value="">اختر</option>`;

    matrixValues.value.forEach(item => {
        options += `<option value="${item.id}">${item.name}</option>`;
    });

    return `
        <select ${baseAttrs}>
            ${options}
        </select>
    `;
};

const buildSelection2 = (item) => {
    const baseAttrs = `class="form-select eval-select" data-id="${item.id}"`;

    if (item.selectionCode === 'multiSelect') {
        return `
      <select ${baseAttrs}>
        <option value="">اختر من 1 إلى 5</option>
        ${[1, 2, 3, 4, 5].map(num => `<option value="${num}">${num}</option>`).join('')}
      </select>
    `;
    }

    return `
    <select ${baseAttrs}>
      <option value="">اختر</option>
      <option value="6b03c651-763c-4193-8c26-d810ff703fb4">موافق</option>
      <option value="2ca355ce-ce36-4f71-a16b-8f73a723ea9b">غير موافق</option>
    </select>
  `;
};

/**
 * Builds note textarea
 * @param {Object} item - Form item data
 * @returns {string} HTML string for the textarea
 */
const buildnote = (item) => {
    return `
    <textarea class="form-control form-control-sm note-input"
              rows="2" 
              data-id="${item.id}"
              placeholder="أضف ملاحظات..."
              aria-label="note notes for ${escapeHtml(item.name)}"></textarea>
  `;
};

/**
 * Escapes HTML to prevent XSS attacks
 * @param {string} text - Text to escape
 * @returns {string} Escaped text
 */
const escapeHtml = (text) => {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
};

// Optional: Add event delegation for better performance
$(document).on('change', '.eval-select', function () {
    const itemId = $(this).data('id');
    const value = $(this).val();
    console.log(`Selection changed for item ${itemId}: ${value}`);
    // Add your change handler logic here
});

$(document).on('input', '.note-input', function () {
    const itemId = $(this).data('id');
    const notes = $(this).val();
    console.log(`Notes updated for item ${itemId}`);
    // Add your input handler logic here
});