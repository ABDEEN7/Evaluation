
let EvalFormtable = null, currentPage = 0, isLoading = true;
const gridContainerId = "view-container";
let lang = sharedUtility().GetCookie('lang');
let txtDir = lang === "ar" ? "RTL" : "LTR";
const loadData = () => {
    isLoading = true;

    const options = {
        success: function (data) {
            if (data) {
                if (data && data.length > 0) {

                    EvalFormtable.addData(data);
                    currentPage = currentPage + 1;
                    isLoading = false;
                }

            }
        }
    };

    jqClient(options).Get("/EvaluationForm/GetAllEvalForm".concat('?page=', currentPage));
};
//===========================================================
const getActionTemplate = (pkId, dynamicaction) => {

    var deleteTmpl = '';
    if (typeof IsDelete != 'undefined') {
        deleteTmpl = IsDelete == "True" ?
            `<span class="delete pointer" title="${uiControlsSetup().GetUiControlText('ADMIN_TOOLTIP_DELETE')}"><i class="delete fa fa-trash-can"></i></span>`
            : '';
    }
    var editTmpl = '';
    if (typeof IsEdit != 'undefined') {
        editTmpl = IsEdit == "True" ?
            `<span class="edit pointer" title="${uiControlsSetup().GetUiControlText('ADMIN_TOOLTIP_EDIT')}"><i class="edit fa fa-edit"></i></span>`
            : '';
    }
    return `
    <section class="sec-center" id="action__section__${pkId}" data-key="${pkId}">
        <div class="action-items">

            ${editTmpl}
            ${deleteTmpl} 

        </div>
    </section>
    `;
};

//===========================================================
const actionCellClick = (event, cell) => {

    let tableId = cell.getTable().element.id;
    table = Tabulator.prototype.findTable("#" + tableId)[0];

    let pkId = '';
    const cellElem = event.target.closest('section');
    if (cellElem) {
        pkId = cellElem.getAttribute('data-key');
        loaderId = cellElem.getAttribute('id');
    }
    const clazzList = event.target.classList;
   
     if (clazzList.contains('edit')) {
        edit(pkId);

    }
    else if (clazzList.contains('delete')) {
        deleteData(pkId);
    }



};
const columns = () => {
    return [
        {

            title: uiControlsSetup().GetUiControlText('ACTION'), field: "", cssClass: 'tbl-cell-actions',
            frozen: false, width: 120,
            formatter: function (cell, formatterParams, onRendered) {
                const { id, isActive } = cell.getRow().getData();
                return getActionTemplate(id, isActive);
            },
            cellClick: function (event, cell) {
                actionCellClick(event, cell);
            }

        },

        { title: uiControlsSetup().GetUiControlText('EvalFormsNameAr'), headerTooltip: uiControlsSetup().GetUiControlText('EvalFormsNameAr'), field: "nameAr", hozAlign: "center", headerFilter: "input" },
        { title: uiControlsSetup().GetUiControlText('EvalFormsNameEn'), headerTooltip: uiControlsSetup().GetUiControlText('EvalFormsNameEn'), field: "nameEn", hozAlign: "center", headerFilter: "input" },
        { title: uiControlsSetup().GetUiControlText('EvalFormsEvalFormType'), headerTooltip: uiControlsSetup().GetUiControlText('EvalFormsEvalFormType'), field: "EvalFormType", hozAlign: "center", headerFilter: "input" },
        { title: uiControlsSetup().GetUiControlText('EvalFormsFormEvalMatrix'), headerTooltip: uiControlsSetup().GetUiControlText('EvalFormsFormEvalMatrix'), field: "FormEvalMatrix", hozAlign: "center", headerFilter: "input" },

        { title: uiControlsSetup().GetUiControlText('EvalFormsIsActive'), headerTooltip: uiControlsSetup().GetUiControlText('EvalFormsIsActive'), field: "isActive", hozAlign: "center", formatter: "tickCross", sorter: "boolean", editor: false, width: 80 },

        {
            title: uiControlsSetup().GetUiControlText('LASTUPDATEDBY'), headerTooltip: uiControlsSetup().GetUiControlText('LASTUPDATEDBY'), field: 'updateBy', hozAlign: "center", headerFilter: "input",
        },
        {
            title: uiControlsSetup().GetUiControlText('LASTUPDATEDDATE'), headerTooltip: uiControlsSetup().GetUiControlText('LASTUPDATEDDATE'), field: "updateDate", width: 130, sorter: "datetime",
            tooltip: function (cell) {
                const { updateDate } = cell.getRow().getData();
                return getActionDate(updateDate, commonUtil.DATE_FORMAT.lll);
            },
            formatter: function (cell) {
                const { updateDate } = cell.getRow().getData();
                return getActionDate(updateDate, commonUtil.DATE_FORMAT.lll);
            }
        },

    ]
};
const InitialPopup =  (ControlItems) => {
    let popupdivcontent = '<div class="row">';
    if (ControlItems) {
        var formData = new FormData();
        formData.append('request', JSON.stringify(ControlItems));
        $.ajax({
            url: "/Home/UiControlList",
            type: "POST",
            dataType: "html",
            processData: false,
            contentType: false,
            data: formData,
            Mode: 'APP',
            success: function (response) {
                if (response) {
                    popupdivcontent += response + "</div>";
                    $('#formcontent').empty();
                    $('#formcontent').append(popupdivcontent);
                    $("#formsection").show();
                    $("#tbltemplatecontainer").hide();
                }
            },
            error: function (xhr, status, error) {
                console.error("UI Control load failed:", error);
            }
        });

        
        }
  
    return popupdivcontent;
};

$("#btnaddcontent").click(function () {
    InitialPopup(uiControlItems);

});

$("#btn-back").click(function () {
    $("#app-form").trigger("reset");
    $("#app-form select").each(function () {
        $(this).val('').trigger('change');
        $(this).removeAttr("data-value");
    });
    $("#formsection").hide();
    $("#tbltemplatecontainer").show();
});
//===========================================================
const getActionDate = (val, format) => {
    format = format ?? commonUtil.DATE_FORMAT.lll;
    return commonUtil.getLocalUtcDateStringEn(val, format);
};
initTables = () => {
    uiControlsSetup().PopulateUiControl(controlsList);
    $("#formsection").hide();
    if (IsAdd == true) {
        $("#btnaddcontent").show();
    }
    else {
        $("#btnaddcontent").hide();
    }
    loadData();
    table = tableUtil.createTabulator({
        id: gridContainerId,
        config: {
            textDirection: txtDir,
            paginationSize: 10,
            placeholder: uiControlsSetup().GetUiControlText('NO_DATA_FOUND'),
            headerFilterPlaceholder: uiControlsSetup().GetUiControlText('FILTER_COLUMN'),
            movableRows: true,
        },
        uniqueRowId: 'id',
        sortColumn: "updateDate",
        sortDir: "desc",
        columns: columns(),

    });
};














