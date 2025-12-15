// form.assign.js

window.formUtility = window.formUtility || {};
var formUtility = window.formUtility;

(function (ns) {

    const { ACTION_TYPE } = window.FormConstants || {};

    // =========================================
    // #region 🔹 Helpers
    // =========================================

    const getText = (key) =>
        (window.uiControlsSetup && uiControlsSetup().GetUiControlText(key)) || "";

    const getAssignmentTable = () => {
        if (!window.Tabulator) return null;
        const tables = Tabulator.findTable('#user-tables');
        return tables && tables.length ? tables[0] : null;
    };

    const showGlobalError = (message) => {
        // تقدر بعدين تغيّر ده لأي مكان عرض Error في الصفحة
        if (typeof window.DisplayAlert === "function") {
            window.DisplayAlert(message, "error");
        } else {
            // fallback بسيط في حالة عدم وجود DisplayAlert
            console.error(message);
        }
    };

    // #endregion

    // =========================================
    // #region 🔹 Render Assign Table
    // =========================================

   
    function renderAssignTable(userData) {
        const lang = window.currentLang || "en";

       
        $("#user-wrapper").empty();

        const $tableHost = $('<div/>', { id: 'user-tables' });
        $("#user-wrapper").append($tableHost);

        const columns = [
            {
                title: getText('lblAssignedName'),
                field: lang === 'ar' ? "nameAr" : "nameEn",
            },
            {
                title: getText('lblAssignedEmail'),
                field: "email",
                editor: false
            },
            {
                title: getText('lblAssignedPartyType'),
                field: "partyTypeTitle",
                editor: false
            },
            {
                title: getText('lblAssignedIsSelected'),
                field: "isSelected",
                formatter: "tickCross",
                editor: "checkbox",
                cellClick: function (e, cell) {
                    const rowData = cell.getRow().getData();
                    const newVal = !rowData.isSelected;
                    cell.getRow().update({ isSelected: newVal });
                }
            },
            {
                title: "PartyTypeId",
                field: "partyTypeId",
                editor: false,
                visible: false
            }
        ];

        const showIsDefaultAssigner = Array.isArray(userData) &&
            userData.some(row => row.showIsDefaultAssigner === true);

        if (showIsDefaultAssigner) {
            columns.push({
                title: getText('lblDefaultAssigner'),
                field: "isDefault",
                formatter: "tickCross",
                editor: "checkbox",
                cellClick: function (e, cell) {
                    const rowData = cell.getRow().getData();
                    const newVal = !rowData.isDefault;
                    cell.getRow().update({ isDefault: newVal });
                }
            });
        }
        const table = new Tabulator("#user-tables", {
            data: userData || [],
            columns: columns,
            layout: "fitColumns",
        });

        setTimeout(() => {
            table.setSort("isSelected", "desc");
        }, 800);
    }

    // #endregion

    // =========================================
    // #region 🔹 Validation + Submit Helper
    // =========================================

   
    function validateAssignmentData(actionTypeName) {
        if (![ACTION_TYPE.ASSIGN, ACTION_TYPE.Approve_And_Assign].includes(actionTypeName)) {
            return true;
        }

        const table = getAssignmentTable();
        if (!table) {
           
            showGlobalError(getText('lblAtLeastOneSelected'));
            return false;
        }

        const assignmentData = table.getData() || [];

        if (!assignmentData.length) {
            showGlobalError(getText('lblAtLeastOneSelected'));
            return false;
        }

        const isAnySelected = assignmentData.some(row => row.isSelected === true);
        if (!isAnySelected) {
            showGlobalError(getText('lblAtLeastOneSelected'));
            return false;
        }

       

        return true;
    }

    
    function addAssignmentData(formData, actionTypeName) {
        if (![ACTION_TYPE.ASSIGN, ACTION_TYPE.Approve_And_Assign].includes(actionTypeName)) {
            return true;
        }

        const table = getAssignmentTable();
        if (!table) {
            showGlobalError(getText('lblAtLeastOneSelected'));
            return false;
        }

        const assignmentData = table.getData() || [];

        const isValid = validateAssignmentData(actionTypeName);
        if (!isValid) {
            return false;
        }

        formData.append('Users', JSON.stringify(assignmentData));
        return true;
    }

    // #endregion

    // =========================================
    // #region 🔹 Expose to namespace
    // =========================================

    ns.renderAssignTable = renderAssignTable;
    ns.validateAssignmentData = validateAssignmentData;
    ns.addAssignmentData = addAssignmentData;

    // #endregion

})(formUtility);
