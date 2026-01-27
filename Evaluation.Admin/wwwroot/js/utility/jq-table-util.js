
const tableUtil = (function () {
    'use strict';

    /*   const format = (num, decimals) => num.toLocaleString('en-US', {
           minimumFractionDigits: 2,
           maximumFractionDigits: 2,
       });*/
    const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const createCell=(row, col, cellIndex, rowId)=> {
        const td = document.createElement('td');
        const cellId = rowId + '-cell-' + cellIndex;
        td.id = cellId;
        td.style.padding = '5px';
        const cellClazz1 = 'cell-' + col.field;
        td.classList.add(cellClazz1);
        const cellVal = row[col.field];
        const dataKeyVal = row[col['dataKey']];
        const dataFlagVal = row[col['dataFlag']];
        switch (col.type) {
            case 'HIDE':
                const hideInput = document.createElement('input');
                hideInput.id = cellId + "-hidden-" + col.field;
                hideInput.setAttribute("type", "hidden");
                //hideInput.setAttribute("name", "name_you_want");
                hideInput.setAttribute("value", cellVal);
                td.appendChild(hideInput);
                break;
            case 'TEXT_BOX':
                const textBox = document.createElement('input');
                textBox.id = cellId + "-textbox-" + col.field;
                textBox.type = "text";
                textBox.classList.add("textbox-" + col.field, 'form-control');
                textBox.value = cellVal || '';
                textBox.setAttribute("data-key", dataKeyVal);
                textBox.setAttribute("data-flag", dataFlagVal);
                textBox.setAttribute('dir', col.dir || 'ltr');
                td.appendChild(textBox);
                break;
            case 'CHECK_BOX':
                const chkBox = document.createElement('input');
                chkBox.id = cellId + "-chkbox-" + col.field;
                chkBox.type = "checkbox";
                chkBox.classList.add("chkbox-" + col.field);
                //chkBox.value = cellVal || '';
                chkBox.checked = cellVal || false;
                td.appendChild(chkBox);
                break;
            case 'TEXT_AREA':
                const textArea = document.createElement('textarea');
                textArea.id = cellId + "-textarea-" + col.field;
                textArea.rows = col['rows'] || 2;
                textArea.classList.add("textarea-" + col.field, 'form-control');
                textArea.setAttribute('dir', col.dir || 'ltr');
                textArea.value = cellVal || '';
                td.appendChild(textArea);
                break;
            case 'TEXT':
                td.innerHTML = `${(col['prefix'] || '')} ${(cellVal || '')} ${(col['suffix'] || '')}`;
                td.setAttribute("data-key", dataKeyVal);
                td.setAttribute("data-flag", dataFlagVal);
                break;
            case 'NUMBER_BOX':
                const numberBox = document.createElement('input');
                numberBox.id = cellId + "-numberbox-" + col.field;
                numberBox.type = "number";
                numberBox.classList.add("numberbox-" + col.field, 'form-control');
                numberBox.value = cellVal || '';
                td.appendChild(numberBox);
                break;
            case 'NUMBER':
                td.innerHTML = `${(col['prefix'] || '')} ${formatter.format(cellVal || 0)} ${(col['suffix'] || '')}`;
                break;
            case 'DATE':
                td.innerHTML = cellVal;
                break;
            case 'DATE_TIME':
                td.innerHTML = cellVal;
                break;
            case 'DATE_BOX':
                const dateBox = document.createElement('input');
                dateBox.type = "date";
                dateBox.className = "datebox-" + col.field;
                dateBox.value = cellVal || '';
                td.appendChild(dateBox);
                break;
            case 'DROPDOWN':
                const select = document.createElement('select');
                select.id = cellId + "-selectbox-" + col.field;
                if (col.multiple) {
                    select.multiple = "multiple";
                }
                select.classList.add("selectbox-" + col.field, 'form-control');
                col.collections.forEach((item, i) => {
                    const option = document.createElement('option');
                    option.text = item.label;
                    option.value = item.value;
                    select.appendChild(option);
                });
                td.appendChild(select);
                break;
            default:
                td.innerHTML = `${(col['prefix'] || '')} ${(cellVal || '')} ${(col['suffix'] || '')}`;
                break;
        }

        return td;
    }

    const addRow=({ tblId, colDef, rowsData, isPrepend = true })=> {
        const table = document.getElementById(tblId);
        if (!table || !rowsData || rowsData.length <= 0) return;

        var rowsLength = $('#'+ tblId + ' tr').length - 1; 
        const tbody = table.getElementsByTagName("TBODY")[0];
        rowsData.forEach((row, rowIndex) => {
            const rowId = 'row-' + tblId + '-' + (rowIndex + rowsLength);
            const tr = document.createElement('TR');
            tr.id = rowId;
            colDef.forEach((col, cellIndex) => {
                const td = createCell(row, col, cellIndex, rowId);
                tr.appendChild(td);
            });
            if (isPrepend) {
                tbody.prepend(tr);
            } else {
                tbody.append(tr);
            }
           
        });
    } 

    const createTable=(
        {
            tblId,
            colDef,
            jsonArr,
            appendId
        }
    )=>{
        //var myTableDiv = document.getElementById("myDynamicTable");
        const table = document.createElement('TABLE');
        const thead = document.createElement('THEAD');
        const tableBody = document.createElement('TBODY');

        table.border = '1';
        table.id = tblId;
        table.style.fontWeight = '400';
        table.classList.add('table', 'table-hover', 'table-sm');
        
        tableBody.id = `tbody-${tblId}`;

        thead.id = `thead-${tblId}`;
        thead.classList.add('bg-primary');
        //thead.style.backgroundColor = "#007bff";
        //thead.style.borderColor = "#007bff";
        thead.style.color = '#fff';

        const headTr = document.createElement('tr');

        colDef.forEach((col, cellIndex) => {
            const th = document.createElement("th");
            const cellClazz1 = 'cell-' + col.field;// TABLE HEADER.
            th.classList.add(cellClazz1);
            th.innerHTML = col.header;
            th.style.textAlign = 'center';
            th.style.padding = '5px';
            headTr.appendChild(th);
        });
        thead.appendChild(headTr);
        
        if (!jsonArr || jsonArr.length <= 0) {
            const bodyTr = document.createElement('tr');
            bodyTr.style.textAlign = 'center';
            const td = document.createElement('td');
            td.setAttribute('colspan', colDef.length);
            td.innerHTML = '<h5>No Data Found</h5>';
            bodyTr.appendChild(td);
            tableBody.append(bodyTr);
        } else {
            jsonArr.forEach((row, rowIndex) => {
                const bodyTr = document.createElement('tr');
                const rowId = 'row-' + tblId + '-' + rowIndex;
                bodyTr.id = rowId;

                colDef.forEach((col, cellIndex) => {
                    const td = createCell(row, col, cellIndex, rowId);
                    bodyTr.appendChild(td);
                });
                tableBody.append(bodyTr);
            });
        }
     
        table.appendChild(thead);
        table.appendChild(tableBody);

        return table;

    }

    const getTabulatorPaginationConfig = (paginationItem) => {
        if (!paginationItem || paginationItem.length <= 0) return;
        let paginationConfig = {};
        paginationItem.forEach(x => {
            switch (x.key) {
                case 'ADMIN_PAGI_PAGE_SIZE':
                    paginationConfig.pageSize = x.label;
                    break;
                case 'ADMIN_PAGI_FIRST':
                    paginationConfig.first = x.label;
                    break;
                case 'ADMIN_PAGI_FIRST_TITLE':
                    paginationConfig.firstTitle = x.label;
                    break;
                case 'ADMIN_PAGI_LAST':
                    paginationConfig.last = x.label;
                    break;
                case 'ADMIN_PAGI_LAST_TITLE':
                    paginationConfig.lastTitle = x.label;
                    break;
                case 'ADMIN_PAGI_PREV':
                    paginationConfig.prev = x.label;
                    break;
                case 'ADMIN_PAGI_PREV_TITLE':
                    paginationConfig.prevTitle = x.label;
                    break;
                case 'ADMIN_PAGI_NEXT':
                    paginationConfig.next = x.label;
                    break;
                case 'ADMIN_PAGI_NEXT_TITLE':
                    paginationConfig.nextTitle = x.label;
                    break;
                case 'ADMIN_PAGI_ALL':
                    paginationConfig.all = x.label;
                    break;
                case 'ADMIN_PAGI_SORT_DESC':
                    paginationConfig.sortDesc = x.label;
                    break;
                case 'ADMIN_PAGI_SORT_ASC':
                    paginationConfig.sortAsc = x.label;
                    break;
                case 'ADMIN_PAGI_SEARCH':
                    paginationConfig.search = x.label;
                    break;
                case 'ADMIN_PAGI_PROCESSING':
                    paginationConfig.processing = x.label;
                    break;
                case 'ADMIN_PAGI_LOADING_RECORD':
                    paginationConfig.loading = x.label;
                    break;
                case 'ADMIN_PAGI_LENGTH_MENU':
                    paginationConfig.lengthMenu = x.label;
                    break;
                case 'ADMIN_PAGI_THOUSAND':
                    paginationConfig.thousand = x.label;
                    break;
                case 'ADMIN_PAGI_INFO_POST_FIX':
                    paginationConfig.infoPostFix = x.label;
                    break;
                case 'ADMIN_PAGI_INFO_FILTERED':
                    paginationConfig.infoFiltered = x.label;
                    break;
                case 'ADMIN_PAGI_INFO_EMPTY':
                    paginationConfig.infoEmpty = x.label;
                    break;
                case 'ADMIN_PAGI_INFO':
                    paginationConfig.info = x.label;
                case 'NO_DATA_FOUND':
                    paginationConfig.noDataFound = x.label;
                    break;
                default:
            }
        });
        return paginationConfig;
    };

    const createTabulatorHeaderMenu = (columns) => {
        const headerMenu = function () {
            let menu = [];
            const columns = this.getColumns();

            for (let column of columns) {

                //create checkbox element using font awesome icons
                let icon = document.createElement("i");
                icon.classList.add("fa");
                icon.classList.add(column.isVisible() ? "fa-check-square" : "fa-square");

                //build label
                let label = document.createElement("span");
                let title = document.createElement("span");

                title.textContent = " " + column.getDefinition().title;

                label.appendChild(icon);
                label.appendChild(title);

                //create menu item
                menu.push({
                    label: label,
                    action: function (e) {
                        //prevent menu closing
                        e.stopPropagation();

                        //toggle current column visibility
                        column.toggle();

                        //change menu item icon
                        if (column.isVisible()) {
                            icon.classList.remove("fa-square");
                            icon.classList.add("fa-check-square");
                        } else {
                            icon.classList.remove("fa-check-square");
                            icon.classList.add("fa-square");
                        }
                    }
                });
            }

            return menu;
        };
        return headerMenu;
    }

    const createTabulator = ({ id, uniqueRowId, config, sortColumn, sortDir, columns, rowMoved, rowFormatter, rowClick,isShowHeaderMenu, paginationConfig, isResponsiveLayout = true }) => {
        //const responsiveLayout = config?.responsiveLayout ?? "collapse";
        const responsiveLayout = isResponsiveLayout ? config?.responsiveLayout ?? "collapse" : null;
        paginationConfig = paginationConfig ?? {};
        if (responsiveLayout == "collapse") {
            const collapseCol = { formatter: "responsiveCollapse", width: 30, minWidth: 30, hozAlign: "center", resizable: false, headerSort: false };
            columns.unshift(collapseCol);
        }
        if (isShowHeaderMenu) {
            createTabulatorHeaderMenu(columns);
        }
        let table = new Tabulator(`#${id}`, {
            ...config,
            langs: {
                "en-app": {
                    //"columns": {
                    //    "name": "Name", //replace the title of column name with the value "Name"
                    //},
                    //"ajax": {
                    //    "loading": "Loading", //ajax loader text
                    //    "error": "Error", //ajax error text
                    //},
                    //"groups": { //copy for the auto generated item count in group header
                    //    "item": "item", //the singular  for item
                    //    "items": "items", //the plural for items
                    //},
                    "pagination": {
                        "page_size": paginationConfig.pageSize ?? "Page Size",
                        "page_title": paginationConfig.pageTitle ?? "Show Page",
                        "first": paginationConfig.first ?? "First",
                        "first_title": paginationConfig.firstTitle ?? "First Page",
                        "last": paginationConfig.last ?? "Last",
                        "last_title": paginationConfig.lastTitle ?? "Last Page",
                        "prev": paginationConfig.prev ?? "Prev",
                        "prev_title": paginationConfig.prevTitle ?? "Prev Page",
                        "next": paginationConfig.next ?? "Next",
                        "next_title": paginationConfig.nextTitle ?? "Next Page",
                        "all": paginationConfig.all ?? "All",
                    },
                    //"headerFilters": {
                    //    "default": "filter column...", //default header filter placeholder text
                    //    "columns": {
                    //        "name": "filter name...", //replace default header filter text for column name
                    //    }
                    //}
                }
            },
            index: uniqueRowId,
            layout: config?.layout ?? "fitColumns",
            responsiveLayout: responsiveLayout,
            movableColumns: config?.movableColumns ?? true,
            resizableRows: config?.resizableRows ?? true,
            tooltips: config?.tooltips ?? true,
            selectable: config?.selectable ?? true,
            editable: config?.editable ?? false,
            selectableRangeMode: 'click',
            editTriggerEvent: "dblclick",
            initialSort: [
                { column: sortColumn ?? uniqueRowId, dir: sortDir ?? "desc" },
            ],
            columns,
            rowMoved,
            rowClick,
            rowFormatter,
        });
        return table;
    };

    const createDataTable = ({ id, dataSet, config, columns, paginationConfig, isDrawCallback }, rowCallback) => {
        if (!columns || columns.length <= 0) {
            alert("datatable column definiation not found");
            return;
        }
        if (!id) {
            alert("datatable DOM id not found");
            return;
        }
        config = config ?? {};
        dataSet = dataSet ?? [];
        const pageLength = config?.pageLength;
        const paging = config?.paging ?? true;
        if (paging) {
            let lengthMenuOpts = [10, 25, 50];
            let lengthMenuOptsV = [10, 25, 50];
            let lengthMenu = [lengthMenuOpts, lengthMenuOptsV];
            if (pageLength) {
                lengthMenuOpts.push(pageLength);
                lengthMenuOptsV.push(pageLength);
                lengthMenuOpts.sort((a, b) => a - b);
                lengthMenuOptsV.sort((a, b) => a - b);
                lengthMenuOpts = commonUtil.unique(lengthMenuOpts);
                lengthMenuOptsV = commonUtil.unique(lengthMenuOptsV);
                lengthMenu = [lengthMenuOpts, lengthMenuOptsV];
            }
            lengthMenuOpts.push(-1);
            lengthMenuOptsV.push("All");
            config = {
                ...config,
                lengthMenu: lengthMenu,
            }
        }

        //if (isDrawCallback) {
        //    config = {
        //        ...config,
        //        "drawCallback": function (settings) {
        //            if (dtapi) {
        //                var visibleRows = $("td.cbcell").closest("tr");
        //                visibleRows.each(function (i, item) {
        //                    $("td.cbcell input", item).prop("checked", dtapi.rows(item).data()[0].isChecked);
        //                })
        //            }
        //        }
        //    }
        //}
        
        // Table definition

        if ($.fn.dataTable.isDataTable(`#${id}`)) {
            try {
                var table = $(`#${id}`).DataTable();
                table.destroy();
            } catch (e) {
            }
            
        }
       

        var dtapi = $(`#${id}`).removeAttr('width').DataTable({
            data: dataSet,
            ...config,
            // Use the createdRow callback to check column type
            //createdRow: function (row, data, dataIndex) {
            //    // get the column defs from settings
            //    var colDefs = this.api().settings()[0].aoColumns;

            //    // for each column in the columns
            //    // this logic assumes that columns:[] contains an entery for every column.
            //    $.each(colDefs, function (i, item) {
            //        // get the associated td
            //        var cell = $("td", row).eq(i);
            //        // figure out data associated with the row
            //        // it may be an array, it may be an object
            //        var cellData = null;
            //        if (typeof item.data == "string" && typeof data == "object") {
            //            cellData = data[item.data];
            //        }
            //        else if (Array.isArray(data)) {
            //            cellData = data[i];
            //        }
            //        switch (item.type) {
            //            case "money":
            //                // not implemented
            //                break;
            //            case "selectbox":
            //                //not implemented
            //                break;
            //            case "checkbox":
            //                // assumed that if the data is type boolean and it is true
            //                // apply it to the checkbox
            //                if (cellData === true) {
            //                    cell.html("<input type='checkbox' checked />");
            //                }
            //                else {
            //                    cell.html("<input type='checkbox'  />");
            //                }

            //                break;
            //            default:
            //                // take no action, use defaults
            //                break;
            //        }
            //    })

            //},
            //// We have the option of invalidating and redrawing the table on each checkox change or
            //// as I did, use the drawCallback to set the checkbox to match the row data 
            //"drawCallback": function (settings) {
            //    // not important on first draw so not worried about dtapi being defined on table initialization
            //    if (dtapi) {
            //        var visibleRows = $("td.cbcell").closest("tr");
            //        visibleRows.each(function (i, item) {
            //            $("td.cbcell input", item).prop("checked", dtapi.rows(item).data()[0].isChecked);
            //        })
            //    }
            //},
            //// this is the default but note that the drawCallback will not be called on each page change if set to true.
            //"deferRender": false,
            //"initComplete": function (settings, json) {
            //    $('body').find('.dataTables_scrollBody').addClass("app-datatable-scrollbar");
            //},
            //"order": [1],
            "columns": columns,
            "language": {
                "decimal": paginationConfig?.decimal ?? "",
                "emptyTable": paginationConfig?.noDataFound ?? "No data available in table",
                "info": paginationConfig?.info ? commonUtil.stringFormat(paginationConfig?.info, '_START_' , '_END_', '_TOTAL_')  : "Showing _START_ to _END_ of _TOTAL_ entries",
                "infoEmpty": paginationConfig?.infoEmpty ? commonUtil.stringFormat(paginationConfig?.infoEmpty, 0, 0, 0) : "Showing 0 to 0 of 0 entries",
                "infoFiltered": paginationConfig?.infoFiltered ? commonUtil.stringFormat(paginationConfig?.infoFiltered, '_MAX_') : "(filtered from _MAX_ total entries)",
                "infoPostFix": paginationConfig?.infoPostFix ?? "",
                "thousands": paginationConfig?.thousand ?? ",",
                "lengthMenu": paginationConfig?.lengthMenu ? commonUtil.stringFormat(paginationConfig?.lengthMenu, '_MENU_') : "Show _MENU_ entries",
                "loadingRecords": paginationConfig?.loading ?? "Loading...",
                "processing": paginationConfig?.processing ?? "Processing...",
                "search": paginationConfig?.search ?? "Search:",
                "zeroRecords": paginationConfig?.noDataFound ?? "No matching records found...",
                "paginate": {
                    "first": paginationConfig?.first ?? "First",
                    "last": paginationConfig?.last ?? "Last",
                    "next": paginationConfig?.next ??  "Next",
                    "previous": paginationConfig?.prev ?? "Previous"
                },
                "aria": {
                    "sortAscending": paginationConfig?.sortAsc ?? ": activate to sort column ascending",
                    "sortDescending": paginationConfig?.sortDesc ?? ": activate to sort column descending"
                }
            },
        });

        $(`#${id} tbody`).on('click', 'tr', function () {
            let rowEvent = {};
            const table = $('#' + id).DataTable().rows().data();
            let rowData = table.row(this).data();
            rowEvent = {
                ...rowEvent,
                element: this,
                rowData
            };
            const chkboxElem = $(this).find('input[type=checkbox]');
            if (chkboxElem) {
                rowEvent = {
                    ...rowEvent,
                    selectionChecked: chkboxElem.prop("checked")
                };
            }

            if (rowCallback)
                rowCallback(rowEvent);
        });
        return dtapi;
    };

    const getDataSetOfDataTable = (id) => {
        return $('#' + id).DataTable().rows().data();
    };

    function fn_GetTableButton(tblID, dt, node) {
        $(dt.table().node()).toggleClass('cards');
        $('.bi', node).toggleClass(['bi-table', 'bi-card-list']);
        if ($('.bi', node).hasClass('bi-card-list')) {
         
            $(`.bi-card-list`, node).parent('span').html(`${complaintUtil.uiConrols.varCARD_VIEW} <i class="bi bi-card-list cardortable"  title="Table" key-data="${tblID}-table"></i>`);
        }
        else {
            
            $(`.bi-table`, node).parent('span').html(`${complaintUtil.uiConrols.varTABLE_VIEW } <i class= "bi bi-table cardortable"  title = "Table" key-data="${tblID}-table" ></i>`);
        }
        

        dt.draw('page');

        /*
        const prevArrow = $('<span>').addClass('slick-prev slick-arrow').html('<i class="bi bi-arrow-right-circle"></i>');

        //const nextArrow = $('<span>').addClass('slick-next slick-arrow').html('<i class="bi bi-arrow-left-circle"></i>');

        //const $tblElem = $(`#${tblID}`);

        //const $outerDiv = $tblElem.closest('div');
       

        
        //if ($tblElem.hasClass("cards")) {
            

        //    //$tblElem.addClass('slick-initialized slick-slider');
        //    //$tblElem.find('tbody').addClass('slick-track draggable');
        //    const arrowElem = document.querySelector(`#view_datatable_${tblID} .slick-prev`);
        //    if (!arrowElem) {
        //        $outerDiv.prepend(prevArrow);
        //        $outerDiv.append(nextArrow);
        //    }

        //    //$tblElem.find('tbody').addClass('slider blog-active dot-style');
        //    $tblElem.find('tbody').addClass('app_slider');
        //    //$tblElem.find('tbody tr').addClass('solution_cards_box slick-slideshow__slide');            
           

        //}
        //else {
        //    $tblElem.find('tbody tr').removeClass('solution_cards_box slick-slideshow__slide  slick-slide ');
        //    $tblElem.find('tbody').removeClass('slick-list draggable slider blog-active dot-style');
        //    //$tblElem.removeClass('slick-list draggable slick-initialized slick-slider');
        //    $tblElem.find('tbody').removeClass('app_slider blog-active dot-style');
        //    $outerDiv.find('.slick-arrow').remove();
        //}
        */
    }
    function fn_GetDrawCallBack(thisapi,settings) {
        var api = thisapi.api();
        var $table = $(api.table().node());
        var tblID = $table[0].id;
        
        
        if ($table.hasClass('cards'))
        {
            //$("#btn_cardview").text(complaintUtil.uiConrols.varTABLE_VIEW);

            //setCookie(tblID, 'card');
           
            //slider blog-active dot-style slick-initialized slick-slider
            // Create an array of labels containing all table headers
            var labels = [];
            $('thead th', $table).each(function () {
                labels.push($(this).text());
            });

            // Add data-label attribute to each cell
            const dataTbl = $(`#${tblID}`).DataTable();
            const data = dataTbl.rows().data();
            const dataLength = data?.length;
            $('tbody tr', $table).each(function (trX, trY, trZ) {
                $(this).find('td').each(function (column, tdx, tdy) {
                    const tdElem = $(this);
                    const trElem = tdElem.parent();

                    if (tdElem.hasClass('cell-actionlist')) {
                        tdElem.attr('data-label', trElem.find('.cell-createdate').text());
                    }
                    else {
                        if (dataLength > 0)
                            tdElem.attr('data-label', `${labels[column]}`);
                        else
                            tdElem.attr('data-label', '');
                    }
                });
            });

            //var max = 0;
            //$('tbody tr', $table).each(function () {
            //    max = Math.max($(this).height(), max);
            //}).height(max);

        } else {
            //setCookie(tblID, 'table');
           // $("#btn_cardview").text(complaintUtil.uiConrols.varCARD_VIEW);
            // Remove data-label attribute from each cell
            $('tbody td', $table).each(function () {
                $(this).removeAttr('data-label');
            });

           /* $('tbody tr', $table).each(function () {
                $(this).height('auto');
            });*/
        }
    }


    let result = {};

    result.createTable = createTable;
    result.createCell = createCell;
    result.addRow = addRow;
    result.createTabulator = createTabulator;
    result.createDataTable = createDataTable;
    result.getTabulatorPaginationConfig = getTabulatorPaginationConfig;
    result.getDataSetOfDataTable = getDataSetOfDataTable;
    result.fn_GetTableButton = fn_GetTableButton;
    result.fn_GetDrawCallBack = fn_GetDrawCallBack;

    return result;
})();



