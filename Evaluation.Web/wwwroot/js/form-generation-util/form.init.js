// form.init.js

window.formUtility = window.formUtility || {};


(function (ns) {

    const { RENDER_TYPE, ACTION_TYPE } = window.FormConstants || {};
    const lang = window.currentLang || "en";

    // #region ========== Helpers ==========

    function getUiText(key, fallback = '') {
        try {
            return uiControlsSetup().GetUiControlText(key) || fallback;
        } catch {
            return fallback;
        }
    }

    function formatDateFromString(value) {
        if (!value) return null;
        if (value instanceof Date) return value;
        if (value.includes("/")) {
            const [d, m, y] = value.split("/");
            return new Date(`${y}-${m}-${d}`);
        }
        const d = new Date(value);
        return isNaN(d) ? null : d;
    }

    function getDate(strDate) {
        if (!strDate) return null;
        const parts = strDate.split('/');
        if (parts.length < 3) return null;

        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1;
        const year = parseInt(parts[2], 10);

        return new Date(year, month, day);
    }

    function getDateTime(strDateTime) {
        if (!strDateTime) return null;
        const parts = strDateTime.split(/[\/\s:]/);
        if (parts.length < 5) return null;

        const day = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10) - 1;
        const year = parseInt(parts[2], 10);
        const hour = parseInt(parts[3], 10);
        const minute = parseInt(parts[4], 10);

        return new Date(year, month, day, hour, minute);
    }

    function convertToPlain(html) {
        const tempDivElement = document.createElement("div");
        tempDivElement.innerHTML = html || "";
        const brElements = tempDivElement.querySelectorAll('br');
        for (let i = 0; i < brElements.length; i++) {
            brElements[i].outerHTML = '\n';
        }
        return tempDivElement.textContent || "";
    }

    function convertToHTML(plainText) {
        if (!plainText) return "";
        const htmlWithBreaks = plainText.replace(/\n/g, '<br>');
        const tempDivElement = document.createElement("div");
        tempDivElement.innerHTML = htmlWithBreaks;
        return tempDivElement.innerHTML;
    }

    function converthtmlToText(html) {
        if (!html) return "";
        html = html.replace(/\n/g, "").replace(/\t/g, "");
        html = html
            .replace(/<\/p>/g, "\n\n")
            .replace(/<\/h1>/g, "\n\n")
            .replace(/<br>/g, "\n")
            .replace(/<br( )*\/>/g, "\n");

        const temp = document.createElement("div");
        temp.innerHTML = html;
        let text = temp.textContent || "";
        text = text.replace(/  /g, "").replace(/\n /g, "\n").trim();
        return text;
    }

    function parseDateString(str) {
        if (!str) return null;
        const [day, month, year] = str.split('/');
        if (!day || !month || !year) return null;
        return new Date(`${year}-${month}-${day}`);
    }

    function getYearsMonthsDifference(from, to) {
        if (typeof from === "string") from = new Date(from);
        if (typeof to === "string") to = new Date(to);

        if (!(from instanceof Date) || isNaN(from)) return "";
        if (!(to instanceof Date) || isNaN(to)) return "";

        if (from > to) {
            const temp = from;
            from = to;
            to = temp;
        }

        let years = to.getFullYear() - from.getFullYear();
        let months = to.getMonth() - from.getMonth();

        if (to.getDate() < from.getDate()) {
            months--;
        }

        if (months < 0) {
            years--;
            months += 12;
        }

        const parts = [];
        if (years) parts.push(formatArabicDuration(years, "سنة", "سنتان", "سنوات", true));
        if (months) parts.push(formatArabicDuration(months, "شهر", "شهران", "أشهر", false));

        return parts.length ? parts.join(" و") : "0 أشهر";
    }

    function formatArabicDuration(value, singular, dual, plural, feminine) {
        if (value === 1) return feminine ? `${singular} واحدة` : `${singular} واحد`;
        if (value === 2) return dual;
        return `${value} ${plural}`;
    }

    function disableAutofillOnSelect2($select) {
        $select.attr({
            name: "disabled_" + Math.random().toString(36).substring(2),
            autocomplete: "off"
        });

        if ($select.data("orig-name") === undefined) {
            const origName = $select.attr("data-name") || $select.attr("name") || ("f_" + Date.now());
            $select.data("orig-name", origName);

            const $fake = $('<input type="text" class="auto-fill" ' +
                'tabindex="-1" autocomplete="username" name="' + origName + '">');
            $select.before($fake);
        }
    }

    $("form").on("submit", function () {
        $(this).find("select").each(function () {
            const $s = $(this);
            const orig = $s.data("original-name");
            if (orig) $s.attr("name", orig);
        });
    });

    // #endregion

    // #region ========== Core initializeFields ==========

    function initializeFields(fields, elementId, renderType, actionType) {
        const datePcikerElements = [];
        const datetimePcikerElements = [];
        const jqteElements = [];
        const select2Fields = [];
        const vacancySeatFields = [];
        const DualSelectFieldFields = [];
        const dropzoneElements = [];
        const checkboxElements = [];
        const tinyMceElements = [];
        let tabulatorTables = [];
        const telInputElements = [];

        let prefield = renderType === RENDER_TYPE.PREVIEW ? "field_view_" : "field_";
        let prefieldList =
            renderType === RENDER_TYPE.PREVIEW
                ? "table_View_"
                : renderType === RENDER_TYPE.MAJOR
                    ? "table_Major_View_"
                    : "table_";

        fields.forEach(field => {

            if (!field.value && field.attributes) {
                const defaultAttr = field.attributes?.find(attr => attr.name.trim().toLowerCase() === 'default');
                if (defaultAttr && defaultAttr.value) {
                    field.value = defaultAttr.value;
                }
            }

            switch (field.type.toLowerCase()) {
                case 'date': {
                    const isDisabledField = field.isApproved
                        ? true
                        : (typeof field.isEditable == "undefined" ? false : !field.isEditable);

                    if (!field.value &&
                        field.attributes &&
                        field.attributes?.find(attr =>
                            attr.name.trim().toLowerCase() === 'default' &&
                            attr.value.trim().toLowerCase() === 'todaydate')) {
                        field.value = new Date().toLocaleDateString('en-GB');
                    }

                    const calcMin7DaysFrom = field.attributes?.find(attr => attr.name.toLowerCase() === 'min7days')?.value;

                    datePcikerElements.push({
                        id: `${prefield}${field.fieldId}`,
                        value: field.value,
                        readonly: isDisabledField,
                        calcMin7DaysFrom: !!calcMin7DaysFrom
                    });
                    break;
                }

                case 'datetime': {
                    const isDisabledDatetime = field.isApproved
                        ? true
                        : (typeof field.isEditable == "undefined" ? false : !field.isEditable);

                    if (!field.value &&
                        field.attributes &&
                        field.attributes?.find(attr =>
                            attr.name.trim().toLowerCase() === 'default' &&
                            attr.value.trim().toLowerCase() === 'date')) {
                        field.value = new Date().toLocaleDateString('en-GB');
                    }

                    const calcMin7Days = field.attributes?.find(attr => attr.name.toLowerCase() === 'min7days')?.value;

                    datetimePcikerElements.push({
                        id: `${prefield}${field.fieldId}`,
                        value: field.value,
                        readonly: isDisabledDatetime,
                        calcMin7DaysFrom: !!calcMin7Days
                    });
                    break;
                }

                case 'jqte': {
                    const jqteValue =
                        field.value ||
                        field.attributes?.find(attr => attr.name.trim().toLowerCase() === 'default')?.value ||
                        '';
                    jqteElements.push({
                        id: `${prefield}${field.fieldId}`,
                        value: convertToHTML(jqteValue),
                        readonly: field.isApproved || (field.isEditable === false)
                    });
                    break;
                }

              

                case 'select2': {
                    const isMultiSelect = field.attributes?.some(attr => attr.name === 'Multi_Value_Select');

                    select2Fields.push({
                        id: `${prefield}${field.fieldId}`,
                        value: field.value,
                        isMultiSelect: isMultiSelect
                    });
                    break;
                }

                case 'dual_select': {
                    DualSelectFieldFields.push({
                        id: `${prefield}${field.fieldId}`,
                        value: field.value,
                        dropdownTypeId: field.dropDownTypeId,
                        attributes: field.attributes,
                        readonly: field.isEditable === false
                    });
                    break;
                }

                case 'dropzone': {
                    const acceptAttr = field.attributes?.find(attr => attr.name === 'accept');
                    const sizeAttr = field.attributes?.find(attr => attr.name === 'data-max-size');
                    const msgAttr = field.attributes?.find(attr => attr.name.toLowerCase() === 'message');

                    dropzoneElements.push({
                        id: `${prefield}${field.fieldId}`,
                        acceptedFiles: acceptAttr?.value || '',
                        maxFilesize: sizeAttr ? (parseInt(sizeAttr.value || "0", 10) / 1000000) : null,
                        messageKey: msgAttr?.value || null
                    });
                    break;
                }

                case 'tinymce': {
                    tinyMceElements.push({
                        id: `${prefield}${field.fieldId}`,
                        value: field.value || '',
                        readonly: !field.isEditable
                    });
                    break;
                }

                case 'list': {
                    const isDisabledList =
                        renderType === RENDER_TYPE.PREVIEW ||
                        actionType === ACTION_TYPE.RETURNBACK ||
                        actionType === ACTION_TYPE.RequestDataChange ||
                        ((field?.isApproved ?? false) && actionType != ACTION_TYPE.INFO_Override_Approve) ||
                        !(field?.isEditable ?? true);

                    let jsonSchema = field.jsonSchema ? JSON.parse(field.jsonSchema) : { fields: [] };
                    jsonSchema = ns.transformKeysToLowercase ? ns.transformKeysToLowercase(jsonSchema) : jsonSchema;
                    jsonSchema.fields = jsonSchema.fields || [];

                    const tabelId = `${prefieldList}${field.fieldId}`;
                    const modalId = "sharedListModal";
                    const modalBodyId = "sharedListDynamicForm";
                    const modalTitleId = "sharedListModalTitle";
                    const cancelModalBtnId = "cancelAddingListBtn";
                    const addObjectBtnId = "addListBtn";
                    const modalTitleTextForEdit = getUiText('lblEditTabelDetails');

                    const preventDelete = field.attributes?.some(attr => attr.name === 'preventDelete');
                    const preventDeleteOld = field.attributes?.some(attr => attr.name === 'preventDeleteOld');
                    const preventEdit = field.attributes?.some(attr => attr.name === 'preventEdit');
                    const preventEditOld = field.attributes?.some(attr => attr.name === 'preventEditOld');
                    const preventAdd = field.attributes?.some(attr => attr.name === 'preventAdd');

                    let maxCount = null;
                    let minCount = null;
                    const maxCountAttr = field.attributes?.find(attr => attr.name === "maxcount");
                    const minCountAttr = field.attributes?.find(attr => attr.name === "mincount");

                    const maxCountNewAttr = field.attributes?.find(attr => attr.name.toLowerCase() === "maxcountnew");
                    const minCountNewAttr = field.attributes?.find(attr => attr.name.toLowerCase() === "mincountnew");

                    let maxCountNew = maxCountNewAttr ? parseInt(maxCountNewAttr.value) : null;
                    let minCountNew = minCountNewAttr ? parseInt(minCountNewAttr.value) : null;

                    if (maxCountAttr) maxCount = parseInt(maxCountAttr.value);
                    if (minCountAttr) minCount = parseInt(minCountAttr.value);

                    const columns = ns.GetTabulatorColumns
                        ? ns.GetTabulatorColumns(
                            jsonSchema,
                            modalId,
                            modalBodyId,
                            addObjectBtnId,
                            modalTitleId,
                            modalTitleTextForEdit,
                            !isDisabledList,
                            tabelId,
                            renderType,
                            preventDelete,
                            preventEdit,
                            maxCount,
                            minCount,
                            preventDeleteOld,
                            preventEditOld,
                            maxCountNew,
                            minCountNew
                        )
                        : [];

                    const tableConfig = {
                        id: tabelId,
                        layout: "fitColumns",
                        columns: columns,
                        data: field.value || [],
                        autoColumns: true,
                        fitColumns: true,
                        modalId: modalId,
                        cancelModalBtnId: cancelModalBtnId,
                        addObjectBtnId: addObjectBtnId,
                        jsonSchema: jsonSchema,
                        modalTitleId: modalTitleId,
                        modalBodyId: modalBodyId,
                        modalTitleText: getUiText('lblAddNewList'),
                        maxCount: maxCount,
                        minCount: minCount,
                        maxCountNew: maxCountNew,
                        minCountNew: minCountNew
                    };

                    const inputElement = $(`#${prefield}${field.fieldId}`);
                    if (!isDisabledList && !preventAdd) {
                        let buttonLabelField = jsonSchema.fields.find(item => item.type === 'buttonLabel') || {};
                        let buttonText = buttonLabelField.fieldName || getUiText('lblAddNewList');

                        const addButtonId = `${tabelId}_addBtn`;
                        const addButton = $('<button>')
                            .attr('id', addButtonId)
                            .addClass('btn btn-primary mt-3')
                            .text(buttonText);

                        inputElement.append(addButton);
                        tableConfig.button = addButton;
                        tableConfig.addButtonId = addButtonId;

                        if (typeof ns.toggleAddButtonVisibility === "function") {
                            ns.toggleAddButtonVisibility(tabelId, addButtonId, maxCount, maxCountNew, minCountNew);
                        }
                    }

                    tabulatorTables.push(tableConfig);
                    break;
                }

                case 'phone': {
                    telInputElements.push({
                        id: `${prefield}${field.fieldId}`,
                        value: field.value || '',
                        readonly: field.isEditable === false
                    });
                    break;
                }

                case 'checkbox': {
                    checkboxElements.push({
                        id: `${prefield}${field.fieldId}`,
                        checked: field.value
                    });
                    break;
                }
                case 'evaluationplan':
                    {
                    const fieldId = `fld_${field.fieldId}`;
                    const PH = window.PlanHandler;
                   
                        let readonly = field.isEditable === false;
                        const isReadonly =
                            renderType === RENDER_TYPE.PREVIEW ||
                            (field.isApproved === true && actionType !== ACTION_TYPE.INFO_Override_Approve) ||
                            (field.isEditable != null && !field.isEditable && actionType !== ACTION_TYPE.SubmitMissingData)
                            //||
                            //(ReadOnly_ACTION_TYPES || []).includes(actionType) 
                            ;
                        PH.init(isReadonly,
                        fieldId,
                        JSON.parse(field.value), elementId
                    );
                    break;
                }

                default: {
                    const calcAgeAttr = field.attributes?.find(attr => attr.name.trim().toLowerCase() === 'calcage');
                    if (calcAgeAttr) {
                        const birthDateField = fields.find(x => x.attributes?.some(attr => attr.name === "birthDateValue"));
                        if (birthDateField) {
                            field.value = calculate(birthDateField.value);
                        }
                    }
                    const fieldElement = $(`#${prefield}${field.fieldId}`);
                    fieldElement.val(field.value);
                    break;
                }
            }
        });

        initializeCheckboxFields(checkboxElements);
        initializeSelect2Fields(select2Fields, elementId);
        initializeDateTimeFields(datetimePcikerElements);
        initializeDateFields_flatpickr(datePcikerElements);
        initializeJQTEFields(jqteElements);
        initializeDropzoneFields(dropzoneElements);
        initializeTinyMceFields(tinyMceElements);
        initializeDualSelectFields(DualSelectFieldFields);
        initializeTabulatorTables(tabulatorTables);
        

        initializeTelInputFields(telInputElements);

        if (typeof ns.InitializeTooltip === "function") {
            ns.InitializeTooltip();
        }
    }

    function calculate(birthDateValue) {
        if (!birthDateValue) return;

        const birthDate = new Date(birthDateValue);
        if (isNaN(birthDate)) return;

        const today = new Date();
        let age = today.getFullYear() - birthDate.getFullYear();
        const m = today.getMonth() - birthDate.getMonth();

        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }

        return age;
    }

    // #endregion

    // #region ========== Field-type Initializers ==========


    function initializeCheckboxFields(checkboxElements) {
        checkboxElements.forEach(checkbox => {
            const checkboxInput = $(`#${checkbox.id}`);
            if (checkboxInput.length) {
                const isChecked =
                    checkbox.checked === true ||
                    checkbox.checked === "true" ||
                    checkbox.checked === 'True';

                checkboxInput.prop("checked", isChecked);
            }
        });
    }

    function initializeTelInputFields(telInputElements) {
        telInputElements.forEach(telInput => {
            const inputElement = $(`#${telInput.id}`);
            inputElement.val(telInput.value);

            if (telInput.readonly) {
                inputElement.prop('readonly', true);
            }

            inputElement.on('input', function () {
                const value = $(this).val();
                $(this).val(value.replace(/[^0-9\-\(\)\s]/g, ''));
            });
        });
    }

    const initializeTabulatorTables = (tabulatorTables) => {
        tabulatorTables.forEach(({ id, columns, data, button, modalId, modalTitleId, cancelModalBtnId, addObjectBtnId, jsonSchema, modalBodyId, modalTitleText, maxCount, maxCountNew, minCountNew }) => {
            const table = new Tabulator(`#${id}`, {
                columns: columns,
                data: data,
                layout: "fitColumns",
                placeholder: getUiText('lblNodataAvailable')
            });

            if (button) {
                button.on('click', function (e) {
                    e.preventDefault();
                    if (typeof ns.ShowModalForAddModel === "function") {
                        ns.ShowModalForAddModel(
                            modalId,
                            modalTitleId,
                            addObjectBtnId,
                            jsonSchema,
                            modalBodyId,
                            modalTitleText,
                            id,
                            maxCount,
                            maxCountNew,
                            minCountNew
                        );
                    }
                });
            }
        });
    };

    const initializeDualSelectFields = (fields) => {
        fields.forEach(field => {
            const inputElement = $(`#${field.id}`);
            initializeDualSelectField(inputElement, field, window.dropdowns || [], field.readonly);
        });
    };

    const initializeDualSelectField = (inputElement, field, dropdowns, readonly) => {
        if (!Array.isArray(dropdowns) || dropdowns.length === 0) {
            console.error("Dropdowns array is empty or not defined!");
            return;
        }

        let filteredDropdowns = (dropdowns || [])
            .filter(dropdown => dropdown.dropDownTypeId === field.dropdownTypeId)
            .sort((a, b) => a.orderNo - b.orderNo);

        const orderByDecs = field.attributes?.find(c => c.name === 'orderBydesc');
        const orderByAsc = field.attributes?.find(c => c.name === 'orderByAsc');

        if (orderByDecs) {
            filteredDropdowns.sort((a, b) => b.titleEn.localeCompare(a.titleEn));
        }

        if (orderByAsc) {
            filteredDropdowns.sort((a, b) => a.titleEn.localeCompare(b.titleEn));
        }

        const candidateItems = filteredDropdowns.map(option => ({
            id: option.id,
            value: option.titleEn
        }));

        const selectionItems = candidateItems.slice(0, 2);

        inputElement.DualSelectList({
            candidateItems: candidateItems,
            selectionItems: selectionItems,
            onSelect: function (_item) { }
        });

        if (readonly && typeof ns.setReadOnlyAttribute === "function") {
            ns.setReadOnlyAttribute(inputElement, field);
        }
    };

    const initializeSelect2Fields = (fields, elementId) => {
        const parentElement = elementId ? $(`#${elementId}`) : null;

        fields.forEach(field => {
            if (!field.id) return;

            const $el = $(`#${field.id}`);

            if ($el.data('select2')) {
                try { $el.select2('destroy'); } catch { }
            }

            if (field.isMultiSelect) {
                $el.attr('multiple', 'multiple');
                if ($el.find('option[value=""]').length === 0) {
                    $el.prepend('<option value=""></option>');
                }
            } else {
                $el.removeAttr('multiple');
                $el.find('option[value=""]').remove();
            }

            const placeholder =
                field.placeholder ||
                $el.attr('placeholder') ||
                getUiText('lblSelect');

            const opts = {
                width: '100%',
                dropdownCssClass: 'manageselect2zindex',
                allowClear: true,
                placeholder,
                minimumResultsForSearch: 0,
                closeOnSelect: !field.isMultiSelect
            };

            if (parentElement && parentElement.length) {
                opts.dropdownParent = parentElement;
            } else {
                const fb = $('#content-container');
                opts.dropdownParent = fb.length ? fb : $('body');
            }

            $el.select2(opts);
            disableAutofillOnSelect2($el);

            const coerceToArray = v => {
                if (v == null || v === '') return [];
                if (Array.isArray(v)) return v.map(String).map(s => s.trim()).filter(Boolean);
                return String(v).split(',').map(s => s.trim()).filter(Boolean);
            };

            if (field.isMultiSelect) {
                const values = coerceToArray(field.value);
                $el.val(values.length ? values : []).trigger('change');
            } else {
                const v = field.value == null || field.value === '' ? null : String(field.value);
                $el.val(v).trigger('change');
            }
        });
    };

    function initializeDateTimeFields(datetimePickerElements) {
        datetimePickerElements.forEach(({ id, value, readonly, calcMin7DaysFrom }) => {
            const fieldId = `#${id}`;

            const dateTimeInputElement = flatpickr(fieldId, {
                enableTime: true,
                dateFormat: "d/m/Y H:i",
                position: 'below',
                allowInput: true,
                onOpen: function (_selectedDates, _dateStr, instance) {
                    if (calcMin7DaysFrom && typeof calculateMin7DaysFromField === "function") {
                        const minDate = calculateMin7DaysFromField();
                        if (minDate) {
                            instance.set('minDate', minDate);
                        }
                    }
                }
            });

            $(fieldId).addClass("is-calendar");

            if (value && dateTimeInputElement) {
                dateTimeInputElement.setDate(value);
            }

            if (readonly) {
                $(fieldId).prop('readonly', true).prop('disabled', true);
            } else {
                $(fieldId).prop('readonly', false).prop('disabled', false);
            }

            if (calcMin7DaysFrom && typeof calculateMin7DaysFromField === "function") {
                const relatedField = $('input[calcmin7daysFrom="true"]');
                const targetField = $(dateTimeInputElement._input);
                $(relatedField).add(targetField).on('change', function () {
                    const minDate = calculateMin7DaysFromField();
                    if (minDate) {
                        dateTimeInputElement.set('minDate', minDate);
                        const selectedDate = dateTimeInputElement.selectedDates[0];
                        if (selectedDate && selectedDate < new Date(minDate)) {
                            dateTimeInputElement.clear();
                        }
                    }
                });

                $(relatedField).trigger('change');
            }
        });
    }

    function initializeDateFields_flatpickr(flatpickrElements) {
        flatpickrElements.forEach(({ id, value, readonly, calcMin7DaysFrom }) => {
            const fieldElement = document.getElementById(id);

            if (!fieldElement) return;

            const options = {
                dateFormat: "d/m/Y",
                position: 'auto',
                disableMobile: true,
                allowInput: true,
                onOpen: function (_selectedDates, _dateStr, instance) {
                    if (calcMin7DaysFrom && typeof calculateMin7DaysFromField === "function") {
                        const minDate = calculateMin7DaysFromField();
                        if (minDate) {
                            instance.set('minDate', minDate);
                        }
                    }
                }
            };

            const dateTimeInputElement = flatpickr(fieldElement, options);

            if (!dateTimeInputElement) {
                console.error(`Flatpickr initialization failed for '${id}'`);
                return;
            }

            $('#' + id).addClass("is-calendar");

            if (value) {
                dateTimeInputElement.setDate(value, true);
            }

            if (readonly) {
                $('#' + id).prop('disabled', true);
            }

            if (calcMin7DaysFrom && typeof calculateMin7DaysFromField === "function") {
                const relatedField = $('input[calcmin7daysFrom="true"]');
                relatedField.on('change', function () {
                    const minDate = calculateMin7DaysFromField();
                    if (minDate) {
                        dateTimeInputElement.set('minDate', minDate);
                        const selectedDate = dateTimeInputElement.selectedDates[0];
                        if (selectedDate && selectedDate < minDate) {
                            dateTimeInputElement.clear();
                        }
                    }
                });
                relatedField.trigger('change');
            }

            if ($(fieldElement).attr("calculatedDurationField")) {
                $(fieldElement).on('change', function () {
                    setupDurationCalculation();
                });
            }
        });
    }

    function initializeJQTEFields(jqteElements) {
        jqteElements.forEach(({ id, readonly, value }) => {
            const editor = $(`#${id}`);
            editor.jqte();
            if (value) {
                editor.next('.jqte').find('.jqte_editor').html(value);
            }

            if (readonly) {
                editor.closest('.jqte').find('.jqte_editor').attr('contenteditable', false);
                editor.closest('.jqte').addClass('disabled-jqte');
            } else {
                editor.next('.jqte').find('.jqte_editor').prop('contenteditable', true);
            }
        });
    }

    function initializeDropzoneFields(dropzoneElements) {
        if (!Array.isArray(dropzoneElements) || dropzoneElements.length === 0) return;

        dropzoneElements.forEach(dropzone => {
            const rawId = dropzone.id || "";
            const fieldId = rawId
                .replace(/^field_view_/, "")
                .replace(/^field_/, "");

            const defaultMessageKey = dropzone.messageKey || 'lblDropzoneDefaultMessage';
            const dictMessage = getUiText(defaultMessageKey) || getUiText('lblDropzoneDefaultMessage') || "";

            new Dropzone(`#${rawId}`, {
                url: '/upload', 
                acceptedFiles: dropzone.acceptedFiles,     // ".pdf,.docx"
                maxFilesize: dropzone.maxFilesize,         // MB
                dictDefaultMessage: dictMessage,
                addRemoveLinks: true,
                init: function () {
                    const dz = this;

                    dz.on("addedfile", function (file) {
                        const fileExtension = '.' + (file.name.split('.').pop() || '').toLowerCase();
                        const fileSizeInMB = file.size / (1024 * 1024);

                        if (dropzone.acceptedFiles) {
                            const allowedExts = dropzone.acceptedFiles
                                .split(',')
                                .map(e => e.trim().toLowerCase());

                            if (!allowedExts.includes(fileExtension)) {
                                if (typeof ns.showFieldError === "function") {
                                    ns.showFieldError(fieldId, 'lblInvalidFileExtension');
                                } else if (typeof ns.showError === "function") {
                                    ns.showError(`#error_${fieldId}`, getUiText('lblInvalidFileExtension'));
                                }
                                dz.removeFile(file);
                                return;
                            }
                        }

                        if (dropzone.maxFilesize && fileSizeInMB > parseFloat(dropzone.maxFilesize)) {
                            if (typeof ns.showFieldError === "function") {
                                ns.showFieldError(fieldId, 'lblFileSizeExceeded');
                            } else if (typeof ns.showError === "function") {
                                ns.showError(`#error_${fieldId}`, getUiText('lblFileSizeExceeded'));
                            }
                            dz.removeFile(file);
                            return;
                        }
                    });

                    dz.on("error", function (file, _serverMessage) {
                        if (typeof ns.showFieldError === "function") {
                            ns.showFieldError(fieldId, 'lblFileUploadError');
                        } else if (typeof ns.showError === "function") {
                            ns.showError(`#error_${fieldId}`, getUiText('lblFileUploadError'));
                        }
                        dz.removeFile(file);
                    });

                    dz.on("success", function (_file, _response) {
                    });
                }
            });
        });
    }

    function initializeTinyMceFields(tinyMceElements) {
        tinyMceElements.forEach(tinyMce => {
            tinymce.init({
                selector: `#${tinyMce.id}`,
                readonly: tinyMce.readonly,
                menubar: false,
                toolbar: tinyMce.readonly
                    ? false
                    : 'undo redo | bold italic | alignleft aligncenter alignright | code',
                plugins: 'code',
                setup: function (editor) {
                    editor.on('init', function () {
                        editor.setContent(tinyMce.value || "");
                    });
                }
            });
        });
    }

    // #endregion

    // #region ========== Duration Calculation (Years / Months) ==========

    function setupDurationCalculation() {
        const startDateField = $('input[calculatedDurationField="startdate"]');
        const endDateField = $('input[calculatedDurationField="endDate"]');
        const durationField = $('input[calculatedDurationField="true"]');

        if (!startDateField.length || !endDateField.length || !durationField.length) return;

        $(startDateField).add(endDateField)
            .off('change.__duration')
            .on('change.__duration', () => calculateDuration());

        calculateDuration();
    }

    function calculateDuration() {
        const startDateVal = $('input[calculatedDurationField="startdate"]:not([id*="view"])').val();
        const endDateVal = $('input[calculatedDurationField="endDate"]:not([id*="view"])').val();
        const durationField = $('input[calculatedDurationField="true"]:not([id*="view"])');

        if (!startDateVal || !endDateVal) {
            durationField.val('');
            return;
        }

        const startDate = parseDateString(startDateVal);
        const endDate = parseDateString(endDateVal);

        if (!startDate || !endDate || startDate > endDate) {
            durationField.val('');
            return;
        }

        const durationText = getYearsMonthsDifference(startDate, endDate);
        durationField.val(durationText);
    }

    // #endregion

    // #region ========== initializeFormFieldsAndConditions (Wrapper) ==========

    const initializeFormFieldsAndConditions = (formGroups, elementId, renderType, actionType) => {
        const fields = formGroups.flatMap(group => group.fields);

        const NS = '.formInitV2';
        const $root = elementId ? $('#' + elementId) : $(document.body);
        const $modal = $root.closest('.modal');

        initializeFields(fields, elementId, renderType, actionType);

        if (renderType === RENDER_TYPE.ACTION && typeof ns.InitializeCascadingDropdown === "function") {
            ns.InitializeCascadingDropdown(formGroups);
        }

        if (typeof ns.evaluateConditionsAfterLoad === "function") ns.evaluateConditionsAfterLoad();
        if (typeof ns.handleConditionalFields === "function") ns.handleConditionalFields();
        if (typeof ns.handleFormGroupVisibility === "function") ns.handleFormGroupVisibility();
        if (typeof ns.evaluateConditionsAfterLoadForList === "function") ns.evaluateConditionsAfterLoadForList(formGroups);

        // scrollables & closing flatpickr on scroll
        const scrollables = new Set();
        const modalBodyEl = $modal.length ? $modal.find('.modal-body')[0] : null;
        if (modalBodyEl) scrollables.add(modalBodyEl);
        $root.find('.scroll-container').each((_, el) => scrollables.add(el));
        scrollables.add(window);

        const getFlatpickrInstances = () =>
            $root.find('.flatpickr-input').toArray()
                .map(el => el && el._flatpickr)
                .filter(fp => fp && typeof fp.isOpen !== 'undefined');

        const closeAllOpenSelect2 = () => {
            const $open = $('.select2-container--open');
            if (!$open.length) return false;

            $open.each(function () {
                const $select = $(this).prev('select');
                if ($select.length) {
                    try {
                        const s2 = $select.data('select2');
                        if (s2 && typeof s2.close === 'function') s2.close();
                        else $select.trigger('select2:close');
                    } catch {
                        $select.trigger('select2:close');
                    }
                }
            });

            if (document.activeElement && document.activeElement.blur) {
                document.activeElement.blur();
            }

            return true;
        };

        const onDocClick = (evt) => {
            const t = evt.target;
            const insideCal = t.closest && t.closest('.flatpickr-calendar');
            const insideInput = t.closest && t.closest('.flatpickr-input');
            if (!insideCal && !insideInput) {
                getFlatpickrInstances().forEach(fp => { if (fp.isOpen) fp.close(); });
            }
        };

        let rafId = null;
        const onAnyScroll = () => {
            if (rafId) return;
            rafId = requestAnimationFrame(() => {
                rafId = null;
                getFlatpickrInstances().forEach(fp => { if (fp.isOpen) fp.close(); });
            });
        };

        if (modalBodyEl) {
            const $modalBody = $(modalBodyEl);
            const onModalScrollCloseSelect2 = () => {
                closeAllOpenSelect2();
                onAnyScroll();
            };

            $modalBody
                .off('scroll.select2autoclose wheel.select2autoclose touchmove.select2autoclose')
                .on('scroll.select2autoclose', onModalScrollCloseSelect2)
                .on('wheel.select2autoclose', onModalScrollCloseSelect2)
                .on('touchmove.select2autoclose', onModalScrollCloseSelect2);
        }

        document.addEventListener('click', onDocClick, true);
        scrollables.forEach(sc => {
            (sc.addEventListener ? sc : window).addEventListener('scroll', onAnyScroll, { passive: true });
        });

        function initSelect2InModal($select) {
            const $hostModal = $select.closest('.modal');
            const dropdownParent = $hostModal.length ? $hostModal : $(document.body);

            if ($select.data('select2')) {
                try { $select.select2('destroy'); } catch { }
            }

            const isMulti = $select.is('[multiple]') ||
                $select.data('multiple') === true ||
                $select.data('multiple') === 'true';

            if (isMulti) {
                $select.attr('multiple', 'multiple');
                if ($select.find('option[value=""]').length === 0) {
                    $select.prepend('<option value=""></option>');
                }
            }

            if ($select.data('select2')) {
                try { $select.select2('destroy'); } catch { }
            }

            $select.select2({
                dropdownParent,
                width: '100%',
                dropdownCssClass: "manageselect2zindex",
                allowClear: true,
                placeholder: $select.attr('placeholder') || getUiText('lblSelect'),
                minimumResultsForSearch: 0
            });

            disableAutofillOnSelect2($select);

            if (modalBodyEl) {
                const onScrollReposition = () => {
                    const s2 = $select.data('select2');
                    if (s2 && s2.dropdown && typeof s2.dropdown._positionDropdown === 'function') {
                        try { s2.dropdown._positionDropdown(); } catch { }
                    }
                };
                $(modalBodyEl).on('scroll' + NS, onScrollReposition);
                $hostModal.on('hidden.bs.modal' + NS, () => $(modalBodyEl).off('scroll' + NS));
            }
        }

        if ($modal.length) {
            $modal.off('shown.bs.modal' + NS).on('shown.bs.modal' + NS, function () {
                $(this).find('select.select2').each(function () { initSelect2InModal($(this)); });
            });
            if ($modal.is(':visible')) {
                $modal.find('select.select2').each(function () { initSelect2InModal($(this)); });
            }
        } else {
            $root.find('select.select2').each(function () { initSelect2InModal($(this)); });
        }

        const cleanup = () => {
            document.removeEventListener('click', onDocClick, true);
            scrollables.forEach(sc => {
                (sc.removeEventListener ? sc : window).removeEventListener('scroll', onAnyScroll, { passive: true });
            });
            if (modalBodyEl) {
                $(modalBodyEl).off('scroll.select2autoclose wheel.select2autoclose touchmove.select2autoclose');
            }
            if ($modal.length) {
                $modal.off('shown.bs.modal' + NS).off('hidden.bs.modal' + NS);
                if (modalBodyEl) $(modalBodyEl).off('scroll' + NS);
            }
            if (rafId) cancelAnimationFrame(rafId);
        };

        $root.data('formInitCleanup', cleanup);

        document.querySelectorAll('input, textarea, select').forEach(el => {
            el.setAttribute('autocomplete', 'off');
            el.setAttribute('readonly', true);
            el.addEventListener('focus', () => el.removeAttribute('readonly'));
        });

        window.isPageLoad = true;

        $('input[visible="False"], input[display="none"]').each(function () {
            $(this).closest('.colContainer').hide();
        });

        return cleanup;
    };

    // #endregion

    // #region ========== Exports ==========

    ns.getDate = getDate;
    ns.getDateTime = getDateTime;
    ns.convertToPlain = convertToPlain;
    ns.convertToHTML = convertToHTML;
    ns.converthtmlToText = converthtmlToText;

    ns.initializeFields = initializeFields;
    ns.initializeFormFieldsAndConditions = initializeFormFieldsAndConditions;
    ns.initializeCheckboxFields = initializeCheckboxFields;
    ns.initializeTelInputFields = initializeTelInputFields;
    ns.initializeTabulatorTables = initializeTabulatorTables;
    ns.initializeDualSelectFields = initializeDualSelectFields;
    ns.initializeSelect2Fields = initializeSelect2Fields;

    ns.initializeDateTimeFields = initializeDateTimeFields;
    ns.initializeDateFields_flatpickr = initializeDateFields_flatpickr;
    ns.initializeJQTEFields = initializeJQTEFields;
    ns.initializeDropzoneFields = initializeDropzoneFields;
    ns.initializeTinyMceFields = initializeTinyMceFields;
    ns.getUiText = getUiText;

    window.initializeFormFieldsAndConditions = initializeFormFieldsAndConditions;

    // #endregion

})(window.formUtility);
