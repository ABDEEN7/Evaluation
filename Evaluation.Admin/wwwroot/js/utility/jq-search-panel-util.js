
const columnSearchUtil = (function () {
    'use strict';

    const formatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const actionSearchEvent = (ctrlId, callback, event, params) => {
        let ctlValue = {};
        $('#' + ctrlId + ', .search-control').each(function () {
            const val = $(this).val();
            const dataKey = $(this).attr('data-key');
            if (val && val.trim().length > 0 && dataKey) {
                ctlValue[dataKey] = val.trim();
            }
        });
        callback({ data: ctlValue, event, params });
    };

    const clearSearchEvent = (ctrlId, callback, event, params) => {
        $('#' + ctrlId + ', .search-control').each(function () {
            $(this).val('');
            $(this).prop('checked', false);
            $(this).val('').trigger("change");

        });
        callback({ event, params });
    };

    const createControl = (col, rowId,colcount) => {
        const { type, field, header, collections, value, prefix, suffix, multiple, ddlLabelAttr, ddlValueAttr } = col;
        const dir = "RTL";
        const cell = document.createElement('div');
        const cellId = rowId + '-cell-' + field;
        cell.id = cellId;
        //cell.style.padding = '5px';
        const cellClazz1 = 'cell-' + field;
        //const classesToAdd = [cellClazz1, 'col-md-3', 'col-ms-6', 'div-search-control'];
        var col = 12;
        if (colcount > 0) {
            col = col / colcount;
        }
        const classesToAdd = [cellClazz1, 'div-search-control', 'fire-action', 'col-md-' + col];
        cell.classList.add(...classesToAdd);
        const cellVal = value || null;
        switch (type) {
            case 'TEXT_BOX':
                const textBox = document.createElement('input');
                textBox.id = cellId + "-textbox-" + field;
                textBox.type = "text";
                //textBox.placeholder = header;
                textBox.classList.add("textbox-" + field, 'search-control', 'form-control');
                textBox.value = cellVal;
                //textBox['data-key'] = field;
                textBox.setAttribute("data-key", field);
                textBox.setAttribute("placeholder", header);
                cell.appendChild(textBox);
                break;
            case 'TEXT_AREA':
                const textArea = document.createElement('textarea');
                textArea.id = cellId + "-textarea-" + field;
                textArea.rows = col['rows'] || 2;
                textArea.setAttribute("placeholder", header);
                textArea.classList.add("textarea-" + field, 'search-control', 'form-control');
                textArea.value = cellVal;
                //textArea['data-key'] = field;
                textArea.setAttribute("data-key", field);
                cell.appendChild(textArea);
                break;
            case 'TEXT':
                cell.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                break;
            case 'NUMBER_BOX':
                const numberBox = document.createElement('input');
                numberBox.id = cellId + "-numberbox-" + field;
                numberBox.type = "number";
                numberBox.setAttribute("placeholder", header);
                numberBox.classList.add("numberbox-" + field, 'search-control', 'form-control');
                numberBox.value = cellVal;
                //numberBox['data-key'] = field;
                numberBox.setAttribute("data-key", field);
                cell.appendChild(numberBox);
                break;
            case 'NUMBER':
                cell.innerHTML = `${(prefix || '')} ${formatter.format(cellVal || 0)} ${(suffix || '')}`;
                break;
            case 'DATE':
                break;
            case 'DATE_TIME':
                break;
            case 'DATE_BOX':
                //const inputGroupDate = document.createElement('div');
                //inputGroupDate.id = field;
                //inputGroupDate.setAttribute("data-target-input", `nearest`);
                //inputGroupDate.classList.add("input-group", 'date');

                //const inputGroupAppendDate = document.createElement('div');
                //inputGroupAppendDate.setAttribute("data-target", `#${field}`);
                //inputGroupAppendDate.setAttribute("data-toggle", `datetimepicker`);
                //if (dir == "RTL")
                //    inputGroupAppendDate.classList.add("input-group-append", "rtl-cal-icon");
                //else
                //    inputGroupAppendDate.classList.add("input-group-append");


                //const inputGroupTxtDate = document.createElement('div');
                //inputGroupTxtDate.classList.add("input-group-text");
                //inputGroupTxtDate.innerHTML = `<i class="fa fa-calendar"></i>`;

                //const dateBox = document.createElement('input');
                //dateBox.name = field;
                //dateBox.type = "text";
                //dateBox.setAttribute("placeholder", header);
                //dateBox.setAttribute("data-key", field);
                //dateBox.setAttribute("dir", dir);
                //dateBox.setAttribute("data-target", `#${field}`);
                //if (dir == "RTL")
                //    dateBox.classList.add("datebox-" + field, 'view-control', 'form-control', 'datepicker', 'datetimepicker-input', 'rtl-cal-ctrl');
                //else
                //    dateBox.classList.add("datebox-" + field, 'view-control', 'form-control', 'datepicker', 'datetimepicker-input');

                //dateBox.value = cellVal;

                //inputGroupAppendDate.appendChild(inputGroupTxtDate);
                //inputGroupDate.appendChild(dateBox);
                //inputGroupDate.appendChild(inputGroupAppendDate);
                //cell.appendChild(inputGroupDate);
                //data-key="${field}" 
                const dateBoxId = cellId + "-datebox-" + field;
                /*
                const dateBox = `<div class="input-group date" data-provide="datepicker">
                                    <input type="text" id="datebox-${dateBoxId}" data-key="${field}" class="search-control form-control" placeholder="${header}">
                                    <div class="input-group-addon">
                                        <span class="glyphicon glyphicon-th"></span>
                                    </div>
                                 </div>`;
                                 */
                const dateBox = document.createElement('input');
                dateBox.id = dateBoxId;
                dateBox.type = "text";
                dateBox.setAttribute("placeholder", header);
                dateBox.setAttribute("data-key", field);
                dateBox.classList.add("datebox-" + field, 'search-control', 'form-control', 'datepicker');
                dateBox.value = cellVal;
                cell.appendChild(dateBox);
                //const dateBox = `<input type="text" id="datebox-${dateBoxId}" data-key="${field}" class="search-control form-control datepicker">`;
                /* const dateBox = `<div class="date input-wrapper" data-provide="datepicker">
                                  <input type="text" id="datebox-${dateBoxId}" class="search-control form-control" placeholder=${header}>
                                 </div>`;*/

                //cell.innerHTML = dateBox;
                break;
            case 'DROPDOWN':
                const select = document.createElement('select');
                select.id = cellId + "-selectbox-" + field;
                select.setAttribute("placeholder", header);
                //select['data-key'] = field;
                select.setAttribute("data-key", field);
                if (multiple) {
                    select.multiple = "multiple";
                }
                select.classList.add("select2", "selectbox-" + field, 'search-control', 'form-control');
                const placeholderOption = document.createElement('option');
                //placeholderOption.disabled = true;
                //placeholderOption.selected = true;
                //placeholderOption.text = header;
                select.appendChild(placeholderOption);
                collections.forEach((item, i) => {
                    const option = document.createElement('option');
                    //option.text = item.label;
                    //option.value = item.value;
                    option.text = item.label ?? item[ddlLabelAttr];
                    option.value = item.value ?? item[ddlValueAttr];
                    select.appendChild(option);
                });

                cell.appendChild(select);
                break;
            case 'MULTI_CHECKBOX':
                const selectcheck = document.createElement('select');
                selectcheck.id = cellId + "-selectcheckbox-" + field;
                selectcheck.setAttribute("placeholder", header);
                selectcheck.setAttribute("data-key", field);

                if (multiple) {
                    selectcheck.multiple = "multiple";
                }
                selectcheck.classList.add('select2-multiple2');

                if (collections != null) {
                    var selectedValues = complaintUtil.defaultStatus;
                    collections.forEach((item, i) => {
                        const option = document.createElement('option');

                        option.text = item.label ?? item[ddlLabelAttr];
                        option.value = item.value ?? item[ddlValueAttr];
                        if ( selectedValues.filter(e => e.value === item.value).length > 0) {
                            option.selected = true;
                        }
                        selectcheck.appendChild(option);
                    });
                }

                cell.appendChild(selectcheck);

                break;

            case 'CHECKBOX':
              

                const checkbox = document.createElement('input');
                checkbox.id = cellId + "-checkbox-" + field;
                checkbox.type = "checkbox";
                checkbox.classList.add('select2-multiple2-selectall');
                cell.appendChild(checkbox);

                const newlabel = document.createElement("Label");
                newlabel.id = cellId + "-Label-" + field;
                newlabel.innerHTML = header;
                newlabel.classList.add('select2-multiple2-selectall-Lable');

                cell.appendChild(newlabel);
                break;

            default:
                cell.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                break;
        }

        return cell;
    };

    const createColumnSearch = ({ ctrlId, colDef, type, actions, params }) => {
        const controlsRow = document.createElement('div');
        controlsRow.id = `controls-${ctrlId}`;
        controlsRow.classList.add('app-column-search');

        colDef.forEach((col, index) => {
            controlsRow.appendChild(createControl(col, index, colDef.length
));
        });

        //$colSearch.append(controlsRow);
        //var datepicker = $.fn.datepicker.noConflict(); // return $.fn.datepicker to previously assigned value
        // $.fn.bootstrapDP = datepicker;

        const actionsRow = document.createElement('div');
        actionsRow.id = `actions-${ctrlId}`;
        actionsRow.classList.add('actions-search-panel','btns-group','mt-3','mb-0');
        controlsRow.classList.add('actions-column-search', 'row');
        actions.forEach((item, index) => {
            const { btnId, text, eventName, type } = item;
            const btn = document.createElement('button');
            btn.innerHTML = text;
            btn.id = btnId;
            btn.setAttribute("data-loader", btnId);
            btn.setAttribute("data-loading-text", `<i class='fa fa-spinner fa-spin'>${text}</i>`);
            btn.style.minWidth = '8vw';
            btn.style.maxWidth = 'fit-content';
            actionsRow.appendChild(btn);
            if (eventName) {
                switch (type) {
                    case 'SEARCH':
                        btn.classList.add('btn', 'btn-primary', 'button-loader');

                        btn.addEventListener('click', (event) => actionSearchEvent(ctrlId, eventName, event, params));
                        //actionSearchEvent(ctrlId, eventName);
                        break;
                    case 'CLEAR':
                        btn.classList.add('btn', 'btn-secondary', 'button-loader');
                        btn.addEventListener('click', (event) => clearSearchEvent(ctrlId, eventName, event, params));
                        //btn.addEventListener('click', eventName);
                        break;
                    default:
                        btn.classList.add('btn', 'btn-primary', 'button-loader');

                        btn.addEventListener('click', eventName, params);
                        break;
                }
            }
        });

        const main = document.createElement('div');
        main.innerHTML = null;
        main.id = ctrlId;
        main.classList.add('search-column-container', 'full-column' ,'row');
        const searchTitle = document.createElement('span');
        //searchTitle.innerHTML = 'لوحة البحث';
        searchTitle.classList.add('search-title');
        main.appendChild(searchTitle);
        main.appendChild(controlsRow);
        main.appendChild(actionsRow);

        return main;
    };

    const datepickerRangeValidation =(startDateKey, endDateKey) => {
        const startElem = document.querySelector(`[data-key="${startDateKey}"]`);
        const endElem = document.querySelector(`[data-key="${endDateKey}"]`);
        const stDateId = startElem.getAttribute('id');
        const edDateId = endElem.getAttribute('id');
        commonUtil.dateRangeValidation(stDateId, edDateId);
    };



    let result = {};
    result.createColumnSearch = createColumnSearch;
    result.datepickerRangeValidation = datepickerRangeValidation;

    return result;
})();

