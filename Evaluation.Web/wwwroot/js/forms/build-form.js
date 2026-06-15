// ==============================
// API Endpoints
// ==============================
const GET_FORMS_API = {
    getItems: (depRoutePath, formId) =>
        `/Form/${depRoutePath}/GetItems?formId=${formId}`,

    getMatrixValues: (depRoutePath, formId) =>
        `/Form/${depRoutePath}/GetFormEvalMarixValues?formId=${formId}`,
};

let depRoutePath = sharedUtility().extractDepartmentName();

// Build MOCK_DATA-shaped structure dynamically from API tree
// Top-level tree node  -> Criterion (criterion-card)
// node.children        -> Aspects (aspect-card)
// child.items          -> Rows (row-item)
// child.name           -> Domain title (right-side column)
function buildFormData(tree) {
    return tree.map(criterionNode => ({
        title: criterionNode.name,
        aspects: (criterionNode.children || []).map(child => ({
            title: child.name,
            domainTitle: child.name,
            rows: (child.items || []).map(item => ({
                id: item.id,
                text: item.name,
                hasNote: !!item.hasNote
            }))
        }))
    }));
}


let MOCK_DATA;

function toast(msg) {
    const el = document.getElementById('toast');
    el.textContent = msg;
    el.classList.add('show');
    setTimeout(() => el.classList.remove('show'), 3000);
}


async function initForm(formId) {

    const response = await jqClient().Get(
        GET_FORMS_API.getItems(depRoutePath, formId)
    );

    let tree = response?.value?.tree ?? [];

    const matrixResponse = await jqClient().Get(
        GET_FORMS_API.getMatrixValues(depRoutePath, formId)
    );

    const matrixValues = matrixResponse?.value ?? matrixResponse ?? [];

    MOCK_DATA = buildFormData(tree);

    const root = document.getElementById('form-root');

    MOCK_DATA.forEach((crit, cIdx) => {
        const cCard = document.createElement('div');
        cCard.className = 'criterion-card';

        cCard.innerHTML = `
                <div class="criterion-header">
                    <span class="criterion-number">معيار ${cIdx + 1}</span>
                    <input class="criterion-title-input" type="text" value="${crit.title}" readonly />
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
                        <input class="aspect-title-input" type="text" value="${asp.title}" readonly />
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
                    const rowDiv = document.createElement('div');
                    rowDiv.className = 'row-item';
                    rowDiv.dataset.itemId = row.id;

                    rowDiv.innerHTML = `
                        <span class="row-index">${rIdx + 1}</span>
 
                        <input class="row-name" type="text" value="${row.text}" readonly />
 
                        <select class="row-select">
                            ${matrixValues.map(o => `<option value="${o.id}">${o.actualMatrixValue}</option>`).join('')}
                        </select>
 
                        ${row.hasNote
                            ? `<input class="row-note" type="text" placeholder="اكتب ملاحظة هنا..." />`
                            : `<div class="row-note-placeholder"></div>`}
                    `;

                    rowsArea.appendChild(rowDiv);
                });
            });
    });
}

function saveForm() {
    const results = [];
    let complete = true;

    document.querySelectorAll('.row-item').forEach(row => {
        const val = row.querySelector('.row-select').value;
        const text = row.querySelector('.row-name').value;
        const noteEl = row.querySelector('.row-note');
        const note = noteEl ? noteEl.value.trim() : null;

        if (!val) complete = false;

        results.push({
            id: row.dataset.itemId,
            text,
            value: val,
            valueId: val,
            note,
            weightPercentage: null,
            subItems: null,
        });
    });

    if (!complete) {
        console.log('Final Assessment Data Payload:', results);
        return results;
    }

    console.log('Final Assessment Data Payload:', results);
    return results;
}
//mainItems.push({
//    id: mainId,

//    valueId:
//        firstSelect.val() || null,

//    value:
//        parseFloat(
//            selectedMainOption.data("actual-value")
//        ) || 0,

//    weightPercentage:
//        parseFloat(
//            firstSelect.data(
//                "config-weight-percentage"
//            )
//        ) || 0,

//    note,

//    subItems
//});
