
const formUtilities = (function () {
    'use strict';

    const NO_DATA = `...`;

    const CONTROL_TYPES = [
        {
            label: 'Text',
            value: 'TEXT'
        },
        {
            label: 'Number',
            value: 'NUMBER'
        },
        {
            label: 'Date',
            value: 'DATE'
        },
        {
            label: 'Date Time',
            value: 'DATE_TIME'
        },
        {
            label: 'Tick',
            value: 'TICK_BOX'
        },
        {
            label: 'Checkbox',
            value: 'CHECK_BOX'
        },
        {
            label: 'Text Editor',
            value: 'TEXT_EDITOR'
        },
        {
            label: 'Dropdown',
            value: 'DROPDOWN'
        },
        {
            label: 'Date Control',
            value: 'DATE_BOX'
        },
        {
            label: 'Number Control',
            value: 'NUMBER_BOX'
        },
        {
            label: 'Textarea',
            value: 'TEXT_AREA'
        },
        {
            label: 'TinyTextarea',
            value: 'TEXT_TINY'
        },
        {
            label: 'Text Control',
            value: 'TEXT_BOX'
        },
        {
            label: 'Upload',
            value: 'UPLOAD'
        },
        {
            label: 'Template',
            value: 'TEMPLATE'
        },
        {
            label: 'DIV',
            value: 'DIV'
        },
    ];


    const numberFormatter = new Intl.NumberFormat('en-US', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });

    const dateFormatter = (val, dir, format, isEnglish) => {
        isEnglish = true;
        format = format ?? commonUtil.DATE_FORMAT.ll;
        if (isEnglish) {
            return commonUtil.getLocalUtcDateStringEn(val, format);
        }
        if (dir) {
            return dir === 'RTL' ? commonUtil.getLocalUtcDateStringAr(val, format) : commonUtil.getLocalUtcDateStringEn(val, format);
        }
        return txtDir === 'RTL' ? commonUtil.getLocalUtcDateStringAr(val, format) : commonUtil.getLocalUtcDateStringEn(val, format);
    };

    const dateTimeFormatter = (val, dir, format, isEnglish) => {
        isEnglish = true;
        format = format ?? commonUtil.DATE_FORMAT.lll;
        if (isEnglish) {
            return commonUtil.getLocalUtcDateStringEn(val, format);
        }
        if (dir) {
            return dir === 'RTL' ? commonUtil.getLocalUtcDateStringAr(val, format) : commonUtil.getLocalUtcDateStringEn(val, format);
        }
        return txtDir === 'RTL' ? commonUtil.getLocalUtcDateStringAr(val, format) : commonUtil.getLocalUtcDateStringEn(val, format);
    };

    const getFormControlValue = (ctrlId) => {
        let ctlValue = {};
        $('#' + ctrlId + ', .view-control').each(function () {
            const dataKey = $(this).attr('data-key');
            if (dataKey) {
                const ctrlType = $(this).attr('type') ?? $(this).prop('nodeName');;
                if (ctrlType && ctrlType == 'checkbox') {
                    ctlValue[dataKey] = $(this).prop('checked');
                }
                else if (ctrlType && ctrlType?.toLocaleLowerCase() == 'select' && $(this).attr('multiple')) {
                    const val = $(this).val();
                    if (val && val?.length > 0)
                        ctlValue[dataKey] = val;
                }
                else {
                    const val = $(this).val();
                    if (val && val?.trim()?.length > 0 && dataKey) {
                        ctlValue[dataKey] = val.trim();
                    }
                }
            }
        });
        return ctlValue;
    };

    const createCtrlView = (col, rowId, dir, labelWidthClazz, ctrlWidthClazz, jsonData) => {
        let { type, field, header, value, prefix, suffix, html, clazz, icon, placeholder, collection, isLabel, format, isEnglish } = col;
        isLabel = isLabel ?? true;
        value = _.get(jsonData, field) ?? value;
        const cellVal = value ?? NO_DATA;
       
        const template = document.createElement('div');
        template.classList.add('form-group');
        const cellLbl = document.createElement('label');
        cellLbl.innerHTML = header;
        const cellCtrl = document.createElement('div');
        let cellDir = dir;
        cellCtrl.setAttribute('id', field);
        switch (type) {
            case 'TEXT':
                cellCtrl.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                //cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                //if (cellDir === 'RTL')
                //    cellCtrl.classList.add('text-rtl');

                //cellCtrl.setAttribute("dir", cellDir);
                break;
            case 'NUMBER':
                cellCtrl.innerHTML = `${(prefix || '')} ${numberFormatter.format(cellVal || 0)} ${(suffix || '')}`;
                break;
            case 'DATE':
                cellCtrl.innerHTML = `${(prefix || '')} ${dateFormatter(cellVal, dir, format, isEnglish)} ${(suffix || '')}`;
                break;
            case 'DATE_TIME':
                cellCtrl.innerHTML = cellVal != '...' ?  `${(prefix || '')} ${dateTimeFormatter(cellVal, dir, format, isEnglish)} ${(suffix || '')}`:'';
                break;
            case 'LIST':
                const cellTxt = collection.map(d => d[field]).join(', ');
                cellCtrl.innerHTML = `${(prefix || '')} ${cellTxt} ${(suffix || '')}`;
                break;
            case 'TICK_BOX':
                const checkbox = document.createElement('span');
                checkbox.classList.add('bi');
                if (cellVal) {
                    checkbox.classList.add('bi-check2-square');
                }
                else {
                    checkbox.classList.add('bi-x-square');
                }
                cellCtrl.appendChild(checkbox);
                break;
            case 'TEMPLATE':
                cellCtrl.innerHTML = html;
                break;
            case 'DIV':
                cellCtrl.innerHTML = `<div id="${field}" class="${clazz?.replace(',', ' ')}"></div>`;
                break;
            default:
                cellCtrl.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                //cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                //if (cellDir === 'RTL')
                //    cellCtrl.classList.add('text-rtl');
                break;
        }
        cellCtrl.setAttribute("dir", cellDir);
        if (isLabel)
            template.append(cellLbl);

        //template.appendChild(cellLbl);
        template.appendChild(cellCtrl);
        return template;
    };

    const createDailogView = ({ appendId, ctrlId, colDef, textDirection, buttons,
        labelWidthClazz, ctrlWidthClazz, legend, jsonData, isRowColView = false, isFieldset }) => {
        const rowGroup = _.groupBy(colDef, 'rowNo');
        const size = Object.keys(rowGroup).length;
        let controlsDef = [];
        legend = legend ?? "Details";
        jsonData = jsonData ?? {};
        isFieldset = isFieldset ?? true;
        if (size > 1) {
            isRowColView = true;
            for (const prop in rowGroup) {
                if (rowGroup.hasOwnProperty(prop)) {
                    let columnArr = [];
                    rowGroup[prop].forEach(item => {
                        columnArr.push(item);
                    });
                    controlsDef.push({ column: columnArr });
                }
            }
        }
        else {
            const colGroup = _.groupBy(colDef, 'colNo');
            for (const prop in colGroup) {
                if (colGroup.hasOwnProperty(prop)) {
                    let columnArr = [];
                    colGroup[prop].forEach(item => {
                        columnArr.push(item);
                    });
                    controlsDef.push({ column: columnArr });
                }
            }
        }
        const formContainer = document.createElement('div');
        const formId = `${ctrlId}`;
        formContainer.id = formId;
        formContainer.classList.add('dailog-body');
        let appRowClazz = 'row-align', controlsRowClazz = ['control-row-view', 'q-column'];
        let colId = `${formId}_COLUM`;
        if (isRowColView) {
            appRowClazz = 'q-column';
            colId = `${formId}_ROW`;
            controlsRowClazz = ['control-row-view', 'control-row-margin', 'row-align-wrap'];
        }
        const appRow = document.createElement('div');
        appRow.classList.add(appRowClazz);
        //const appRow = document.createElement('div');
        //appRow.classList.add('row-align');

        controlsDef.forEach((item, ind) => {
            const controlsRow = document.createElement('div');
            controlsRow.classList.add(...controlsRowClazz);
            //controlsColumn.classList.add('q-column', 'p-3');

            const { column } = item ?? {};
            column.forEach((col, index) => {
                controlsRow.appendChild(createCtrlView(col, index, textDirection, labelWidthClazz, ctrlWidthClazz, jsonData));
            });
            appRow.appendChild(controlsRow);
        });


        //const appColumn = document.createElement('div');
        //appColumn.classList.add('q-column', 'p-2');

        if (isFieldset) {
            const fieldsetElem = document.createElement('fieldset');
            fieldsetElem.classList.add('app-fieldset', 'p-2');
            const legendElem = document.createElement('legend');
            legendElem.innerHTML = legend;

            fieldsetElem.appendChild(legendElem);
            fieldsetElem.appendChild(appRow);
            formContainer.appendChild(fieldsetElem);
        }
        else {
            formContainer.appendChild(appRow);
        }
       

        if (appendId) {
            const appendElem = document.getElementById(appendId);
            appendElem.appendChild(formContainer);
            return;
        }

        return formContainer;
    };

    const createControl = (col, rowId, dir, labelWidthClazz, ctrlWidthClazz, jsonData) => {
        try {
            labelWidthClazz = 'label-width';//labelWidthClazz ?? 'col-sm-3';
            ctrlWidthClazz = 'ctrl-width';//ctrlWidthClazz?? 'col-sm-9';
            let { type, field, jsonFiled, header, collections, value, prefix, suffix, multiple, html, isLabel, clazz, placeholder,
                controlType, isActive, rowNo, colNo, format, isEnglish, icon, width, height, isFieldset, isCellMsg, legendTxt } = col;
            //value = _.get(jsonData, field) ?? value;
            value = _.get(jsonData, jsonFiled ?? field) ?? value;
            const cellVal = value;
            isLabel = isLabel ?? true;
            isFieldset == isFieldset ?? false;
            isCellMsg = isCellMsg ?? true;
            clazz = clazz ?? '';
            const clazzArr = clazz.split(',');
            const cell = document.createElement('div');
            const cellMsg = document.createElement('div');
            const cellId = rowId + '-cell-' + field;
            cell.id = cellId;
            cell.style.padding = '5px';
            const cellClazz = 'cell-' + field;
            //cell.classList.add(cellClazz, 'row', 'main-row-group', 'div-form-control', 'form-group');
            cell.classList.add(cellClazz, 'form-row-group', 'div-form-control', 'form-group');
            cellMsg.classList.add('messages');
            //const cellVal = value ?? null;
            let label = document.createElement('label');
            //const divLabel = document.createElement('div');
            if (type !== 'BUTTON') {
                label.classList.add('col-form-label');
                label.innerHTML = header ?? '';
                label.setAttribute("for", field);
                //divLabel.classList.add(labelWidthClazz); //col-sm-3
                //divLabel.appendChild(label);
            }
            else {
                //label.classList.add('col-form-label', 'px-3');
                //divLabel.classList.add(labelWidthClazz); //col-sm-3
                //divLabel.appendChild(label);
            }

            const cellCtrl = document.createElement('div');
            let cellCtrlClazz = ['form-cell-control', ctrlWidthClazz]; //col-sm-9

            let cellCtrlHtml = null, cellDir = dir;
            placeholder = placeholder ?? (header ?? '');
            switch (type) {
                case 'TEXT_BOX':
                    const textBox = document.createElement('input');
                    //textBox.id = cellId + "-textbox-" + field;
                    textBox.id = field;
                    textBox.name = field;
                    textBox.type = "text";
                    //textBox.placeholder = header;
                    textBox.classList.add("textbox-" + field, 'view-control', 'form-control');
                    textBox.value = cellVal;
                    textBox.setAttribute("data-key", field);
                    textBox.setAttribute("placeholder", placeholder);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(textBox);
                    if (value) {
                        cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                        if (cellDir === 'RTL')
                            cellCtrl.classList.add('text-rtl');
                    }
                    break;
                case 'TEXT_AREA':
                    const textArea = document.createElement('textarea');
                    //textArea.id = cellId + "-textarea-" + field;
                    textArea.id = field;
                    textArea.name = field;
                    textArea.rows = col['rows'] || 2;
                    textArea.setAttribute("placeholder", placeholder);
                    textArea.classList.add("textarea-" + field, 'view-control', 'form-control');
                    textArea.value = cellVal;
                    textArea.setAttribute("data-key", field);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(textArea);
                    if (value) {
                        cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                        if (cellDir === 'RTL')
                            cellCtrl.classList.add('text-rtl');
                    }
                    break;
                case 'NUMBER_BOX':
                    const numberBox = document.createElement('input');
                    //numberBox.id = cellId + "-numberbox-" + field;
                    numberBox.id = field;
                    numberBox.name = field;
                    numberBox.type = "number";
                    numberBox.setAttribute("placeholder", placeholder);
                    numberBox.classList.add("numberbox-" + field, 'view-control', 'form-control');
                    numberBox.value = cellVal;
                    numberBox.setAttribute("data-key", field);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(numberBox);
                    break;
                case 'DATE_BOX':
                    const inputGroupDate = document.createElement('div');
                    inputGroupDate.id = field;
                    inputGroupDate.setAttribute("data-target-input", `nearest`);
                    inputGroupDate.classList.add("input-group", 'date');

                    const inputGroupAppendDate = document.createElement('div');
                    inputGroupAppendDate.setAttribute("data-target", `#${field}`);
                    inputGroupAppendDate.setAttribute("data-toggle", `datetimepicker`);
                    if (dir == "RTL")
                        inputGroupAppendDate.classList.add("input-group-append", "rtl-cal-icon");
                    else
                        inputGroupAppendDate.classList.add("input-group-append");


                    const inputGroupTxtDate = document.createElement('div');
                    inputGroupTxtDate.classList.add("input-group-text");
                    inputGroupTxtDate.innerHTML = `<i class="fa fa-calendar"></i>`;

                    const dateBox = document.createElement('input');
                    dateBox.name = field;
                    dateBox.type = "text";
                    dateBox.setAttribute("placeholder", placeholder);
                    dateBox.setAttribute("data-key", field);
                    dateBox.setAttribute("dir", dir);
                    dateBox.setAttribute("data-target", `#${field}`);
                    if (dir == "RTL")
                        dateBox.classList.add("datebox-" + field, 'view-control', 'form-control', 'datepicker', 'datetimepicker-input', 'rtl-cal-ctrl');
                    else
                        dateBox.classList.add("datebox-" + field, 'view-control', 'form-control', 'datepicker', 'datetimepicker-input');

                    dateBox.value = cellVal;

                    inputGroupAppendDate.appendChild(inputGroupTxtDate);
                    inputGroupDate.appendChild(dateBox);
                    inputGroupDate.appendChild(inputGroupAppendDate);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(inputGroupDate);
                    break;
                case 'TIME_BOX':
                    const inputGroup = document.createElement('div');
                    inputGroup.id = field;
                    inputGroup.setAttribute("data-target-input", `nearest`);
                    inputGroup.classList.add("input-group", 'date');

                    const inputGroupAppend = document.createElement('div');
                    inputGroupAppend.setAttribute("data-target", `#${field}`);
                    inputGroupAppend.setAttribute("data-toggle", `datetimepicker`);
                    if (dir == "RTL")
                        inputGroupAppend.classList.add("input-group-append", "rtl-cal-icon");
                    else
                        inputGroupAppend.classList.add("input-group-append");


                    const inputGroupTxt = document.createElement('div');
                    inputGroupTxt.classList.add("input-group-text");
                    inputGroupTxt.innerHTML = `<i class="fa far fa-clock"></i>`;

                    const timeBox = document.createElement('input');
                    timeBox.type = "text";
                    timeBox.setAttribute("placeholder", placeholder);
                    timeBox.setAttribute("data-key", field);
                    timeBox.setAttribute("dir", dir);
                    timeBox.setAttribute("data-target", `#${field}`);
                    timeBox.setAttribute("name", `${field}`);
                    if (dir == "RTL")
                        timeBox.classList.add("timebox-" + field, 'view-control', 'form-control', 'datetimepicker-input', 'rtl-cal-ctrl');
                    else
                        timeBox.classList.add("timebox-" + field, 'view-control', 'form-control', 'datetimepicker-input');

                    timeBox.value = cellVal;

                    inputGroupAppend.appendChild(inputGroupTxt);
                    inputGroup.appendChild(timeBox);
                    inputGroup.appendChild(inputGroupAppend);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(inputGroup);
                    break;
                case 'DROPDOWN':
                    const select = document.createElement('select');
                    select.id = field;
                    select.name = field;
                    select.setAttribute("placeholder", placeholder);
                    select.setAttribute("data-key", field);
                    if (multiple) {
                        select.setAttribute("multiple", true);
                    }
                    //"select2"
                    select.classList.add("selectbox-" + field, 'view-control', 'form-control');
                    const placeholderOption = document.createElement('option');
                    select.appendChild(placeholderOption);
                    const ddlCollections = collections ?? [];
                    ddlCollections.forEach((item, i) => {
                        const option = document.createElement('option');
                        option.text = item.label;
                        option.value = item.value;
                        select.appendChild(option);
                    });
                    //select.value = cellVal;
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(select);
                    break;
                case 'TEXT_EDITOR':
                    const textAreaEditor = document.createElement('div');
                    textAreaEditor.id = field;
                    textAreaEditor.classList.add("textarea-" + field, 'view-control', 'form-control');
                    textAreaEditor.setAttribute("data-key", field);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(textAreaEditor);
                    if (value) {
                        cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                        if (cellDir === 'RTL')
                            cellCtrl.classList.add('text-rtl');
                    }
                    break;
                case 'CHECK_BOX':
                    const checkBox = document.createElement('input');
                    checkBox.id = field;
                    checkBox.name = field;
                    checkBox.type = "checkbox";
                    checkBox.classList.add("checkbox-" + field, 'view-control');
                    checkBox.checked = cellVal ?? false;
                    checkBox.setAttribute("data-key", field);
                    const checkboxLabel = document.createElement('label');
                    const checkboxSpan = document.createElement('span');
                    checkboxLabel.classList.add('switch-small');
                    checkboxSpan.classList.add('slider-small', 'round');
                    checkboxLabel.appendChild(checkBox);
                    checkboxLabel.appendChild(checkboxSpan);
                    cellCtrl.append(cellMsg);
                    cellCtrl.appendChild(checkboxLabel);
                    break;
                case 'TICK_BOX':
                    const tickbox = document.createElement('span');
                    tickbox.classList.add('bi');
                    if (cellVal) {
                        tickbox.classList.add('bi-check2-square');
                    }
                    else {
                        tickbox.classList.add('bi-x-square');
                    }
                    cellCtrl.appendChild(tickbox);
                    break;
                case 'BUTTON':
                    const button = document.createElement('button');
                    let btnIcon = '';
                    if (icon)
                        btnIcon = `<i class="fas ${icon}"></i>&nbsp;`;

                    button.innerHTML = `${btnIcon} ${header}`;
                    button.setAttribute("id", field);
                    button.setAttribute("type", "button");
                    button.classList.add("btn", "btn-primary", 'view-control-button', "two-tone-button",);
                    cellCtrl.appendChild(button);
                    break;
                case 'TEXT':
                    cellCtrl.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                    cellCtrl.classList.add('view-ctrl');
                    isCellMsg = false;
                    if (value) {
                        cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                        if (cellDir === 'RTL')
                            cellCtrl.classList.add('text-rtl');
                    }
                    break;
                case 'NUMBER':
                    cellCtrl.innerHTML = `${(prefix || '')} ${numberFormatter.format(cellVal || 0)} ${(suffix || '')}`;
                    cellCtrl.classList.add('view-ctrl');
                    if (cellDir === 'RTL')
                        cellCtrl.classList.add('text-rtl', 'view-ctrl');
                    isCellMsg = false;
                    break;
                case 'DATE':
                    cellCtrl.innerHTML = `${(prefix || '')} ${dateFormatter(cellVal, dir, format, isEnglish)} ${(suffix || '')}`;
                    cellCtrl.classList.add('view-ctrl');
                    if (cellDir === 'RTL')
                        cellCtrl.classList.add('text-rtl');
                    break;
                case 'DATE_TIME':
                    cellCtrl.innerHTML = `${(prefix || '')} ${dateTimeFormatter(cellVal, dir, format, isEnglish)} ${(suffix || '')}`;
                    cellCtrl.classList.add('view-ctrl');
                    if (cellDir === 'RTL')
                        cellCtrl.classList.add('text-rtl');
                    break;
                case 'DIV':
                    const divCtrl = document.createElement('div');
                    divCtrl.setAttribute("name", field);
                    divCtrl.setAttribute("id", field);
                    divCtrl.setAttribute("data-key", field);
                    divCtrl.classList.add(`div-${field}`);
                    divCtrl.classList.add(...clazz);
                    divCtrl.style.height = height ?? 'auto';
                    divCtrl.style.width = width ?? 'auto';
                    if (isFieldset) {
                        const fieldset = document.createElement('fieldset');
                        fieldset.classList.add(`app-fieldset`);
                        fieldset.classList.add(...clazz);
                        const legend = document.createElement('legend');
                        legend.append(legendTxt ?? header);
                        fieldset.append(legend);

                        fieldset.appendChild(divCtrl);
                        if (isCellMsg)
                            cellCtrl.append(cellMsg);

                        cellCtrl.appendChild(fieldset);
                    }
                    else {
                        if (isCellMsg)
                            cellCtrl.append(cellMsg);

                        cellCtrl.appendChild(divCtrl);
                    }
                    //isCellMsg = false;
                    break;
                case 'TEMPLATE':
                    //cellCtrl.appendChild(cellMsg);
                    cellCtrl.innerHTML = html;
                    isCellMsg = false;
                    cellCtrlClazz = ['form-cell-template'];
                    break;
                default:
                    cellCtrl.innerHTML = `${(prefix || '')} ${(cellVal || '')} ${(suffix || '')}`;
                    if (value) {
                        cellDir = commonUtil.isRTL(value) ? 'RTL' : 'LTR';
                        cellCtrl.classList.add('view-ctrl');
                        if (cellDir === 'RTL')
                            cellCtrl.classList.add('text-rtl');

                    }
                    isCellMsg = false;
                    break;
            }

            cellCtrl.classList.add(...cellCtrlClazz);
            cell.setAttribute("dir", cellDir);

            if (isLabel && type !== 'BUTTON')
                cell.append(label);

            cell.append(cellCtrl);
            if (controlType == 'UPLOAD' && !isActive) {
                cell.style.display = 'none';
            }
            return cell;
        } catch (e) {
        }
    };

    const createViewWindow = ({ ctrlId, colDef, type, actions, params, textDirection, isRowColView = false,
        jsonData, labelWidthClazz, ctrlWidthClazz }) => {
        const formContainer = document.createElement('form');
        const formId = `${ctrlId}`;
        formContainer.id = formId;
        formContainer.classList.add('app-form-controls');
        let appRowClazz = 'row-align', controlsRowClazz = ['control-column-view', 'q-column'];
        let colId = `${formId}_COLUM`;
        if (isRowColView) {
            appRowClazz = 'q-column';
            colId = `${formId}_ROW`;
            controlsRowClazz = ['control-row-view', 'row-align-wrap'];
        }
        
        const appRow = document.createElement('div');
        appRow.classList.add(appRowClazz);

        colDef.forEach((item, ind) => {
            const controlsRow = document.createElement('div');
            colId = `${colId}_${ind}`;
            controlsRow.id = colId;
            controlsRow.classList.add(...controlsRowClazz);
            const { column } = item ?? {};
            if (column && column.length > 0) {
                column.forEach((col, index) => {
                    controlsRow.appendChild(createControl(col, index, textDirection, labelWidthClazz, ctrlWidthClazz, jsonData));
                });
                appRow.appendChild(controlsRow);
            }
        });
        
        const main = document.createElement('div');
        main.innerHTML = null;
        main.id = `MAIN_${ctrlId}`;
        const culture = textDirection === "RTL" ? "ar" : "en";
        const cultureClazz = textDirection === "RTL" ? 'culture-rtl' : 'culture-ltr';
        main.classList.add('container', 'app-form-container', culture, cultureClazz);
        formContainer.appendChild(appRow);
        main.appendChild(formContainer);
        main.setAttribute('dir', textDirection);
        return main;
    };

    const createViewRows = ({ ctrlId, colDef, textDirection, isRowColView = false,
        jsonData, labelWidthClazz, ctrlWidthClazz }) => {
        const formId = `${ctrlId}`;
        let appRowClazz = 'row-align', controlsRowClazz = ['control-column-view', 'q-column'];
        let colId = `${formId}_COLUM`;
        if (isRowColView) {
            appRowClazz = 'q-column';
            colId = `${formId}_ROW`;
            controlsRowClazz = ['control-row-view', 'row-align-wrap'];
        }

        const appRow = document.createElement('div');
        appRow.classList.add(appRowClazz);

        colDef.forEach((item, ind) => {
            const { column } = item ?? {};
            if (column && column.length > 0) {
                const { rowNo } = column[0];
                const _ind = rowNo + 1 + ind;
                const controlsRow = document.createElement('div');
                colId = `${colId}_${_ind}`;
                controlsRow.id = colId;
                controlsRow.classList.add(...controlsRowClazz);
                column.forEach((col, index) => {
                    const _index = col.rowNo + 1 + index;
                    controlsRow.appendChild(createControl(col, _index, textDirection, labelWidthClazz, ctrlWidthClazz, jsonData));
                });
                appRow.appendChild(controlsRow);
            }
        });

        return appRow;
    };

    const createEditor = ({ containerId, colDef, formConstraints, textDirection, jsonData }) => {
        try {
            const editors = colDef.filter(c => c.type === 'TEXT_EDITOR');
            if (editors && editors?.length > 0) {
                editors.forEach(col => {
                    //const fieldName = col.type === 'TEXT_EDITOR' ? `text-editor-${col.field}` : col.field;
                    //const constraints = formConstraints[fieldName];
                    $(`#${col.field}`).jqte();
                    const cellValue = _.get(jsonData, col.field) ?? col.value;
                    if (cellValue)
                        $(`#${col.field}`).jqteVal(cellValue);

                    commonUtil.manageTextEditorUi(col.field, containerId);
                    const txtEditor = document.querySelector(`#text-editor-${col.field}`);
                    if (txtEditor) {
                        if (textDirection === 'RTL') {
                            const leftIconElem = txtEditor.querySelector('.jqte_toolbar .jqte_tool_15');
                            if (leftIconElem)
                                leftIconElem.click();
                        }
                        else {
                            const rightIconElem = txtEditor.querySelector('.jqte_toolbar .jqte_tool_13');
                            if (rightIconElem)
                                rightIconElem.click();
                        }
                    }
                });
            }
        } catch (e) {
        }
    };

    const datepickerRangeValidation = ({ startDateKey, endDateKey, isRtl, format, isDisbalePastDate, isDisbaleFutureDate }) => {
        try {
            const startElem = document.querySelector(`[data-key="${startDateKey}"]`);
            const endElem = document.querySelector(`[data-key="${endDateKey}"]`);
            const stDateId = startElem.getAttribute('name');
            const edDateId = endElem.getAttribute('name');
            //commonUtil.dateRangeValidation(stDateId, edDateId);
            const today = new Date();
            const _today = commonUtil.toFormateMoment(today);
            format = format ?? 'YYYY/MM/DD';
            const minDate = isDisbalePastDate ? _today : null;
            const maxDate = isDisbaleFutureDate ? _today : null;
            let options = {
                useCurrent: false,
                format: format,
                //format: 'L',
                //endDate: ed,
                changeMonth: true,
                changeYear: true,
                //daysOfWeekHighlighted: ["5,6"],
                todayHighlight: true,
                autoClose: true,
                //inline: true,
                //sideBySide: true,
                //rtl: isRtl ?? false,
                icons: { clear: 'fa fa-trash' },
                buttons: { showClear: true },
                //minDate: minDate
            };

            if (isRtl) {
                //options = { ...options, locale: { calender: 'ummalqura', lang: 'ar' } };
            }
            const $startDate = $('#' + stDateId), $endDate = $('#' + edDateId);
            $startDate.datetimepicker(options);
            $endDate.datetimepicker(options);

            if (minDate) {
                $startDate.datetimepicker('minDate', minDate);
                $endDate.datetimepicker('minDate', minDate);
            }
            if (maxDate) {
                $startDate.datetimepicker('maxDate', maxDate);
                $endDate.datetimepicker('maxDate', maxDate);
            }

            $startDate.on("change.datetimepicker", ({ date, oldDate }) => {
                if (date) {
                    $endDate.datetimepicker('minDate', date);
                }
            });
            $endDate.on("change.datetimepicker", ({ date, oldDate }) => {
                if (date) {
                    $startDate.datetimepicker('maxDate', date);
                }
            });
        } catch (e) {
        }
    };

    const timeRangeValidation = ({ startDateKey, endDateKey }) => {
        try {
            const startElem = document.querySelector(`[data-key="${startDateKey}"]`);
            const endElem = document.querySelector(`[data-key="${endDateKey}"]`);
            const stDateId = startElem.getAttribute('name');
            const edDateId = endElem.getAttribute('name');
           
            const $startDate = $('#' + stDateId), $endDate = $('#' + edDateId);
           
            $startDate.on("change.datetimepicker", ({ date, oldDate }) => {
                if (date) {
                    $endDate.datetimepicker('minDate', date);
                }
            });
            $endDate.on("change.datetimepicker", ({ date, oldDate }) => {
                if (date) {
                    $startDate.datetimepicker('maxDate', date);
                }
            });
        } catch (e) {
        }
    };

    const dateRangeValidation = ({ startDateKey, endDateKey }) => {
        try {
            const startElem = document.querySelector(`[data-key="${startDateKey}"]`);
            const endElem = document.querySelector(`[data-key="${endDateKey}"]`);
            const stDateId = startElem.getAttribute('name');
            const edDateId = endElem.getAttribute('name');

            const $startDate = $('#' + stDateId), $endDate = $('#' + edDateId);

            $startDate.on("change.datetimepicker", ({ date, oldDate }) => {
                if (date) {
                    $endDate.datetimepicker('minDate', date);
                }
            });
            $endDate.on("change.datetimepicker", ({ date, oldDate }) => {
                if (date) {
                    $startDate.datetimepicker('maxDate', date);
                }
            });
        } catch (e) {
        }
    };

    const createDatepicker = ({ colDef, textDirection, isRtl, format, jsonData }) => {
        try {
            const controls = colDef.filter(c => c.type === 'DATE_BOX');
            if (!controls || controls?.length <= 0) return;

            const today = new Date();
            const _today = commonUtil.toFormateMoment(today);

            controls.forEach(col => {
                let { value, field, jsonFiled, isDisbalePastDate, isDisbaleFutureDate, format } = col;
                const dateElem = document.querySelector(`[data-key="${field}"]`);
                const elemName = dateElem.getAttribute('name');

                format = format ?? 'YYYY/MM/DD';
                const minDate = isDisbalePastDate ? _today : null;
                const maxDate = isDisbaleFutureDate ? _today : null;
                value = _.get(jsonData, jsonFiled) ?? value;
                value = commonUtil.toFormateMoment(value);
                let options = {
                    useCurrent: false,
                    format: format,
                    //format: 'L',
                    //endDate: ed,
                    changeMonth: true,
                    changeYear: true,
                    //daysOfWeekHighlighted: ["5,6"],
                    todayHighlight: true,
                    autoClose: true,
                    //inline: true,
                    //sideBySide: true,
                    //rtl: isRtl ?? false,
                    icons: { clear: 'fa fa-trash' },
                    buttons: { showClear: true },
                    //minDate: minDate
                };
                if (isRtl) {
                    //options = { ...options, locale: { calender: 'ummalqura', lang: 'ar' } };
                }
                const $dateElem = $('#' + elemName);
                $dateElem.datetimepicker(options);
                $dateElem.datetimepicker('date', value);

                if (minDate) {
                    $dateElem.datetimepicker('minDate', minDate);
                }
                if (maxDate) {
                    $dateElem.datetimepicker('maxDate', maxDate);
                }

            });

            //$startDate.on("change.datetimepicker", ({ date, oldDate }) => {
            //    if (date) {
            //        $endDate.datetimepicker('minDate', date);
            //    }
            //});
        } catch (e) {
        }
    };

    const createTimepicker = ({ colDef, textDirection, jsonData }) => {
        try {
            const controls = colDef.filter(c => c.type === 'TIME_BOX');
            if (!controls || controls?.length <= 0) return;

            controls.forEach(col => {
                let { value, field, jsonFiled } = col;
                value = _.get(jsonData, jsonFiled) ?? value;
                value = commonUtil.getOnlyTime(value);
                const $timeElem = $(`#${field}`);
                $timeElem.datetimepicker({
                    format: 'LT',
                    defaultDate: new Date(),
                    autoClose: true,
                    stepping: 5,
                });
                $timeElem.datetimepicker('date', value);
                //value = _.get(jsonData, value) ?? value;
            });
        } catch (e) {
        }
    };

    const createDualList = ({ colDef, textDirection, sourceList, selectedList, idAttr, valueAttr,
        itemTxtColor, itemBgColor, itemHoverBgColor  }) => {
        const controls = colDef.filter(c => c.type === 'DUAL_LIST');
        if (!controls || controls?.length <= 0) return;

        let candidateItems = [], selectionItems = [], instances= {} ;
        if (sourceList && sourceList.length > 0 && idAttr && valueAttr) {
            candidateItems = sourceList.map(x => ({ id: x[idAttr], value: x[valueAttr] }));
        }
        else if (sourceList && sourceList.length > 0)
            candidateItems = sourceList;
        
        if (selectedList && selectedList.length > 0 && idAttr && valueAttr) {
            selectionItems = selectedList.map(x => ({ id: x[idAttr], value: x[valueAttr] }));
        }
        else if (selectedList && selectedList.length > 0)
            selectionItems = selectedList;

        itemTxtColor = itemTxtColor ?? '#000000';
        itemBgColor = itemBgColor ?? '#eeeeee';
        itemHoverBgColor = itemHoverBgColor ?? '#3c3c3c';
        controls.forEach(col => {
            let { value, field } = col;
            //$(`#${col.field}`).datetimepicker({
            //    format: 'LT',
            //    defaultDate: new Date(),
            //    autoClose: true,
            //});
            instances[field] = $j(`#${field}`).DualSelectList({
                // Change Item from pure String to an Json Object.
                // the "value" field will be displayed on the list.
                'elementTargetId': `${field}`,
                'candidateItems': candidateItems,
                'selectionItems': selectionItems,
                'colors': {
                    'itemText': itemTxtColor,
                    'itemBackground': itemBgColor,
                    'itemHoverBackground': itemHoverBgColor
                },
            });
        });
        return instances;
    };

    const setConstraint = ({ controls, uiConrols }) => {
        const { allUIControls, requireMsgErr, lblRemarks, msgLengthErr, msgMinCharLength, msgMaxCharLength, requireCheckboxErr } = uiConrols;
        let constraint = {};
        //const allCols = _.flattenDeep(controlsDef.map(x => x.column));
        controls.forEach(item => {
            const { controlName, isRequired, maxLength, minLength, regex, controlType, uibackendName, header } = item || {};
            const headerTxt = allUIControls.find(u => u.backEndName === uibackendName)?.txtValue ?? header;
            constraint = {
                ...constraint,
                [controlName]: {}
            };
            const fieldName = (controlType && controlType === 'TEXT_EDITOR') ? `text-editor-${controlName}` : controlName;
            if (isRequired) {
                if (controlType === 'CHECK_BOX') {
                    constraint = {
                        ...constraint,
                        [fieldName]: {
                            presence: { message: `^${commonUtil.stringFormat(requireCheckboxErr, headerTxt)}` },
                            inclusion: {
                                within: [true],
                                message: `^${commonUtil.stringFormat(requireCheckboxErr, headerTxt)}`
                            },
                        }
                    };
                }
                else {
                    constraint = {
                        ...constraint,
                        [fieldName]: {
                            presence: { message: `^${commonUtil.stringFormat(requireMsgErr, headerTxt)}` },
                        }
                    };

                    if (regex) {
                        //constraint[fieldName]['format'] = {
                        //    //pattern: new RegExp(regex),
                        //    pattern: regex,
                        //    flags: "i",
                        //    message: `^${commonUtil.stringFormat(requireMsgErr, headerTxt)}`
                        //};
                        //format: {
                        //    pattern: /^(34|37|4|5[1-5]).*$/,
                        //        message: function(value, attribute, validatorOptions, attributes, globalOptions) {
                        //            return validate.format("^%{num} is not a valid credit card number", {
                        //                num: value
                        //            });
                        //        }
                        //},
                    }
                }
            }
            if (minLength && minLength > 0) {
                constraint[fieldName]['length'] = {
                    minimum: Number(minLength ?? 1),
                    tooShort: `^${commonUtil.stringFormat(msgMinCharLength, lblRemarks, Number(minLength ?? 1))}`
                };
                //constraint = {
                //    ...constraint[controlName],
                //    [constraint[controlName].length]: {
                //        minimum: Number(minLength ?? 1),
                //        tooShort: `^${commonUtil.stringFormat(msgMinCharLength, lblRemarks, Number(minLength ?? 1))}`
                //    }
                //};
            }
            if (maxLength && maxLength > 0) {
                constraint[fieldName]['length'] = {
                    ...constraint[fieldName]['length'],
                    maximum: Number(maxLength ?? 50),
                    tooLong: `^${commonUtil.stringFormat(msgMaxCharLength, lblRemarks, Number(maxLength ?? 50))}`
                };
            }
        });
        return constraint;
    };

    const createColDef = ({ controls, uiConrols }) => {
        const { allUIControls, placeholderSelect, placeholderCtrl } = uiConrols;
        controls = _.orderBy(controls, 'columnOrder');
        const rowGroup = _.groupBy(controls, 'rowOrder');
        const size = Object.keys(rowGroup).length;
        let controlsDef = [], isRowColView = false;
        if (size > 1) {
            isRowColView = true;
            for (const prop in rowGroup) {
                if (rowGroup.hasOwnProperty(prop)) {
                    let columnArr = [];
                    rowGroup[prop].forEach(item => {
                        const header = allUIControls.find(u => u.backEndName === item.uibackendName)?.txtValue ?? item.header;
                        const jsonVal = item?.jsonFiled ?? null;
                        const jsonValue = jsonVal === 'true' ? true : jsonVal === 'false' ? false : jsonVal;
                        let def = {
                            ...item,
                            field: item.controlName,
                            header,
                            //uibackendName: item?.uibackendName,
                            type: item?.controlType,
                            //controlType: item?.controlType,
                            placeholder: commonUtil.stringFormat(placeholderCtrl, header),
                            value: jsonValue,
                            //isActive: item?.isActive,
                            rowNo: item?.rowOrder,
                            colNo: item?.columnOrder,
                            //clazz: item?.clazz,
                            //height: item?.height,
                            //width: item?.width
                        };
                        if (item?.controlType === "DROPDOWN") {
                            def = {
                                ...def,
                                placeholder: commonUtil.stringFormat(placeholderSelect, header),
                                allowClear: true,
                            };
                        }
                        if (item?.jsonFiled && !item?.controlType) {
                            def = {
                                ...def,
                                value: complaintUtil.selectedRowData[item.jsonFiled]
                            };
                        }
                        if (item?.controlType === "UPLOAD") {
                            def = {
                                ...def,
                                type: 'TEMPLATE',
                                isLabel: false,
                                html: `<div class='messages'></div>
                                       <div id="${item.controlName}" name="${item.controlName}" class="dropzone upload-container"></div>`
                            };
                        }
                        //switch (actionName) {
                        //    case commonUtil.STATUS_MASTER_TYPE.SENT_TO_FEEDBACK:
                        //        break;
                        //    default:
                        //        break;
                        //}
                        columnArr.push(def);
                    });
                    controlsDef.push({ column: columnArr });
                }
            }
        }
        else {
            const colGroup = _.groupBy(controls, 'columnOrder');
            for (const prop in colGroup) {
                if (colGroup.hasOwnProperty(prop)) {
                    let columnArr = [];
                    colGroup[prop].forEach(item => {
                        const header = allUIControls.find(u => u.backEndName === item.uibackendName)?.txtValue ?? item.header;
                        const jsonVal = item?.jsonFiled ?? null;
                        const jsonValue = jsonVal === 'true' ? true : jsonVal === 'false' ? false : jsonVal;
                        let def = {
                            ...item,
                            field: item.controlName,
                            header,
                            type: item?.controlType,
                            //controlType: item?.controlType,
                            placeholder: commonUtil.stringFormat(placeholderCtrl, header),
                            value: jsonValue,
                            //isActive: item?.isActive,
                            rowNo: item?.rowOrder,
                            colNo: item?.columnOrder,
                        };
                        if (item?.controlType === "DROPDOWN") {
                            def = {
                                ...def,
                                placeholder: commonUtil.stringFormat(placeholderSelect, header),
                                allowClear: true,
                            };
                        }
                        if (item?.jsonFiled && !item?.controlType) {
                            def = {
                                ...def,
                                value: complaintUtil.selectedRowData[item.jsonFiled]
                            };
                        }
                        if (item?.controlType === "UPLOAD") {
                            def = {
                                ...def,
                                type: 'TEMPLATE',
                                isLabel: false,
                                html: `<div class='messages'></div>
                                       <div id="${item.controlName}" name="${item.controlName}" class="dropzone upload-container"></div>`
                            };
                        }
                        //switch (actionName) {
                        //    case commonUtil.STATUS_MASTER_TYPE.SENT_TO_FEEDBACK:
                        //        break;
                        //    default:
                        //        break;
                        //}
                        columnArr.push(def);
                    });
                    controlsDef.push({ column: columnArr });
                }
            }
        }

        return { controlsDef, isRowColView };
    };

    // Updates the inputs with the validation errors
    const showErrors = (form, errors, controlsName) => {
        // We loop through all the inputs and show the errors for that input
        let selectors = `input[name], select[name], textarea[name]`;
        if (controlsName && controlsName?.length > 0) {
            controlsName.forEach(ctrl => {
                selectors = selectors + `, [id='${ctrl}']`;
            });
        }
        
        _.each(form.querySelectorAll(selectors), function (input) {
            // Since the errors can be null if no errors were found we need to handle that
            const errorMsg = errors ? errors[input.name] ?? errors[input.getAttribute('name')] : '';
            showErrorsForInput(input, errors && errorMsg);
        });
    };

    // Shows the errors for a specific input
    const showErrorsForInput = (input, errors) => {
        // This is the root of the input
        const formGroup = closestParent(input.parentNode, "form-group"),
            id = input.getAttribute('id');
        if (!formGroup) return;

        // Find where the error messages will be insert into
        const messages = formGroup.querySelector(".messages");

        // First we remove any old messages and resets the classes
        resetFormGroup(formGroup);
        // If we have errors
        if (errors) {
            // we first mark the group has having errors
            formGroup.classList.add("has-error");
            // then we append all the errors
            _.each(errors, function (error) {
                addError(id, messages, error);
            });
        } else {
            // otherwise we simply mark it as success
            formGroup.classList.add("has-success");
        }
    };

    // Recusively finds the closest parent that has the specified class
    const closestParent = (child, className) => {
        if (!child || child == document) {
            return null;
        }
        if (child.classList.contains(className)) {
            return child;
        } else {
            return closestParent(child.parentNode, className);
        }
    };

    const resetFormGroup = (formGroup) => {
        // Remove the success and error classes
        formGroup.classList.remove("has-error");
        formGroup.classList.remove("has-success");
        // and remove any old messages
        _.each(formGroup.querySelectorAll(".help-block.error"), function (el) {
            el.parentNode.removeChild(el);
        });
    };

    // Adds the specified error with the following markup
    // <p class="help-block error">[message]</p>
    const addError = (id, messages, error) => {
        const div = document.createElement("div");
        const section = document.createElement("section");
        const ul = document.createElement("ul");
        const li = document.createElement("li");
        const iconSpan = document.createElement("span");
        const msgSpan = document.createElement("span");
        div.classList.add('notify-container');
        div.classList.add("help-block");
        div.classList.add("error");
        section.classList.add('section-notification');
        ul.classList.add('notifications');
        li.classList.add('msg-section');
        iconSpan.innerHTML = `<i class="material-icons bi bi-info-circle-fill"></i>`;
        msgSpan.innerHTML = error;

        li.append(iconSpan);
        li.append(msgSpan);
        ul.append(li);
        section.append(ul);
        div.append(section);
        messages.appendChild(div);
    };

    const createFormEvents = (formId, formConstraints) => {
        if (!formConstraints) {
            alert('Form Constraints not found.')
            return;
        }
        const formElem = complaintUtil.getFormElem(formId);
        if (!formElem) {
            alert('Form ID not found.')
            return;
        }
        const inputs = formElem.querySelectorAll("input, textarea, select");
        for (let i = 0; i < inputs.length; ++i) {
            inputs.item(i).addEventListener("change", function (ev) {
                const errors = validate(formElem, formConstraints) || {};
                formUtilities.showErrorsForInput(this, errors[this.name])
            });
        }
    };

    const createFullCalendar = ({ appendId, eventsData, initDate, textDir, isFutureDisable, isPastDisable, config }, searchCallback) => {
        //document.addEventListener('DOMContentLoaded', function () {
        //$(`#${storeDivId}`).empty();
        const today = new Date();
        initDate = initDate ?? commonUtil.toMMDDYYYY(today)
        const initDateObj = commonUtil.toFormatedDate(initDate);
        const initialDate = initDateObj.date;
        //if (agendaCalender) {
        //    agendaCalender.removeAllEvents();
        //    agendaCalender.addEventSource(eventsData)
        //    return;
        //}

        isPastDisable = isPastDisable ?? false;
        isFutureDisable = isFutureDisable ?? true;
        config = config ?? {};
        config.eventColor = config?.eventColor ?? '#676767';
        config.selectable = config?.selectable ?? true;
        config.navLinks = config?.navLinks ?? true;
        config.contentHeight = config?.contentHeight ?? '65vh';
        
        const calendarEl = document.getElementById(appendId);
        const locale = textDir == "LTR" ? "en" : "ar";
        let appFullCalender = new FullCalendar.Calendar(calendarEl, {
            themeSystem: 'bootstrap',
            initialView: 'dayGridMonth',
            initialDate: initialDate, //'2021-02-07',
            //contentHeight: '65vh',
            weekNumbers: true,
            locale,
            //dayMaxEvents: true, // allow "more" link when too many events
            //aspectRatio: 5,
            headerToolbar: {
                left: 'prev,next today', //addEventButton
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay,listMonth'
            },
            ...config,
            //backgroundColor: '#ddd',
            events: eventsData,
            eventTimeFormat: {
                hour: '2-digit',
                minute: '2-digit',
                meridiem: true,
                hour12: false
            },
            datesSet: function (e) {
                const type = e.view.type;
                const { start, end } = e.view.getCurrentData().dateProfile.currentRange;
                let startDate = null, endDate = null;
                switch (type) {
                    case 'dayGridMonth':
                        const mStart = commonUtil.toFormatedDate(start, null, commonUtil.DATE_FORMAT.SLASH_M_D_Y);
                        const mEnd = commonUtil.toFormatedDate(end, null, commonUtil.DATE_FORMAT.SLASH_M_D_Y);
                        startDate = mStart.date;
                        endDate = mEnd.momentObj.subtract(1, 'days').format(commonUtil.DATE_FORMAT.SLASH_M_D_Y);
                        break;
                    case 'timeGridWeek':
                        break;
                    case 'timeGridDay':
                        break;
                    case 'listMonth':
                        break;
                    default:
                }


                //if (!isFutureDisable) {
                //    if (start.getTime() > today.getTime()) {
                //        $('.fc-next-button').prop('disabled', true);
                //        $('.fc-next-button').css('opacity', 0.5);
                //        return;
                //    }
                //    $('.fc-next-button').prop('disabled', false);
                //    $('.fc-next-button').css('opacity', 1);
                //}
               
                //if (!isPastDisable) {
                //    if (start.getTime() < today.getTime()) {
                //        $('.fc-prev-button').prop('disabled', true);
                //        $('.fc-prev-button').css('opacity', 0.5);
                //        return;
                //    }
                //    $('.fc-prev-button').prop('disabled', false);
                //    $('.fc-prev-button').css('opacity', 1);
                //}
                

                if (!startDate || !endDate) return;

                if (searchCallback)
                    searchCallback({ startDate, endDate, isRender: true });
            },
            loading: function (bool) {
                //$('#loading').toggle(bool);
            }
        });
        return appFullCalender;
    };

    const getRandomColor = () => {
        //return '#'+ Math.floor(Math.random() * 16777215).toString(16);
        const red = Math.floor(Math.random() * 256 / 2);
        const green = Math.floor(Math.random() * 256 / 2);
        const blue = Math.floor(Math.random() * 256 / 2);
        return "rgb(" + red + ", " + green + ", " + blue + ")";
    };

    const getFormElem = (formId) => {
        return document.querySelector("form#" + formId);
    };

    let result = {};

    result.CONTROL_TYPES = CONTROL_TYPES;
    result.createViewWindow = createViewWindow;
    result.getFormControlValue = getFormControlValue;
    result.showErrors = showErrors;
    result.showErrorsForInput = showErrorsForInput;
    result.closestParent = closestParent;
    result.resetFormGroup = resetFormGroup;
    result.addError = addError;
    result.createEditor = createEditor;
    result.createDailogView = createDailogView;
    result.createFormEvents = createFormEvents;
    result.createViewRows = createViewRows;
    result.createDatepicker = createDatepicker;
    result.createTimepicker = createTimepicker;
    result.createDualList = createDualList;
    result.createFullCalendar = createFullCalendar;
    result.setConstraint = setConstraint;
    result.createColDef = createColDef;
    result.getFormElem = getFormElem;
    result.datepickerRangeValidation = datepickerRangeValidation;
    result.timeRangeValidation = timeRangeValidation;
     
    return result;

})();