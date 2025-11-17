// form.validation.js

window.formUtility = window.formUtility || {};
const formUtility = window.formUtility;

(function (ns) {

    // #region =============  Get Label Text / UI Text
    const GetUiControlText = (key) => (uiControlsSetup().GetUiControlText(key) || "").trim();

    const getFieldLabelText = (elementId) => {
        const $lbl = $(`label[for='${elementId}']`);
        return $lbl.length ? $lbl.text().trim() : "";
    };
    // #endregion

    // #region ===============  Helpers

    const getFieldAttribute = (field, name) =>
        field?.attributes?.find(a => a.name?.trim().toLowerCase() === name.toLowerCase());

    const getFieldAttributeValue = (field, name) => {
        const attr = getFieldAttribute(field, name);
        return attr?.value?.trim() ?? "";
    };

    const isEmpty = (value) => {
        if (value === null || value === undefined) return true;
        if (Array.isArray(value)) return value.length === 0;
        if (typeof value === "string") return value.trim() === "";
        return false;
    };

    // dd/mm/yyyy أو dd/mm/yyyy HH:mm
    const parseDate = (str) => {
        if (!str) return null;
        const p = str.split(/[\/\s:]/);
        if (p.length < 3) return null;
        const day = parseInt(p[0], 10);
        const month = parseInt(p[1], 10) - 1;
        const year = parseInt(p[2], 10);
        const hour = p[3] ? parseInt(p[3], 10) : 0;
        const minute = p[4] ? parseInt(p[4], 10) : 0;
        const d = new Date(year, month, day, hour, minute);
        return isNaN(d.getTime()) ? null : d;
    };

    const showError = (selector, message) => {
        const el = $(selector);
        el.addClass("d-block").text(message || "");
    };

    const clearError = (selector) => {
        $(selector).removeClass("d-block").empty();
    };

    function resetValidationErrors() {
        $(".error").removeClass("error");
        $(".error-message").removeClass("d-block").empty();
    }

    const createError = (fieldId, message) => ({ fieldId, error: message });

    const shouldIgnoreValidation = (field) => {
        const attr = getFieldAttribute(field, "ignore_validation");
        return attr && (attr.value || "").toLowerCase() === "true";
    };

    const shouldIgnoreRequiredValidation = (field) => {
        const attr = getFieldAttribute(field, "ignore_validation_required");
        return attr && (attr.value || "").toLowerCase() === "true";
    };
    // #endregion

    // #region ===============   Remarks Validation

    const isRemarksValid = () => {
        const rmk = $("#remarks");
        if (!rmk.length) return true;
        const isRequired = rmk.prop("required");
        const val = (rmk.val() || "").toString().trim();
        if (isRequired && isEmpty(val)) return false;
        return true;
    };

    const validateRemarks = (formData) => {
        const rmk = $("#remarks");

        if (rmk.length && !isRemarksValid()) {
            const msg = GetUiControlText("lblRemarkISRequired");
            showError("#error_remarks", msg);
            rmk.addClass("error");
            return false;
        }

        if (formData && typeof formData.append === "function") {
            formData.append("actionRemarks", rmk.val() || "");
        }
        return true;
    };

    const validateRemark = () => {
        const rmk = $("#remarks");
        if (rmk.length && !isRemarksValid()) {
            const msg = GetUiControlText("lblRemarkISRequired");
            showError("#error_remarks", msg);
            rmk.addClass("error");
            return false;
        }
        return true;
    };
    // #endregion

    // #region ===============  File helpers (size / extension)

    const validateFileExtension = (field, fieldValue) => {
        const errors = [];
        const extensionAttr = getFieldAttribute(field, "accept");
        if (!extensionAttr || !fieldValue || !fieldValue.value) return errors;

        const allowedExtensions = (extensionAttr.value || "")
            .split(",")
            .map(ext => ext.trim().toLowerCase())
            .filter(x => x)
            .map(ext => (ext.startsWith(".") ? ext : `.${ext}`));

        const files = Array.isArray(fieldValue.value) ? fieldValue.value : [fieldValue.value];
        if (!files.length || !files[0]?.name || !files[0]?.isfile) return errors;

        const fileExtension = `.${files[0].name.split(".").pop()?.toLowerCase()}`;

        if (!allowedExtensions.includes(fileExtension)) {
            const message =
                extensionAttr.message ||
                GetUiControlText("lblInvalidFileExtension");
            if (message) {
                errors.push(createError(field.fieldId, message));
            }
        }

        return errors;
    };

    const validateFileSize = (field, fieldValue) => {
        const errors = [];
        const maxSizeAttr = getFieldAttribute(field, "data-max-size");
        if (!maxSizeAttr || !fieldValue || !fieldValue.value) return errors;

        const maxFileSize = parseInt(maxSizeAttr.value, 10);
        const files = Array.isArray(fieldValue.value) ? fieldValue.value : [fieldValue.value];

        for (const file of files) {
            if (file?.size && !isNaN(maxFileSize) && file.size > maxFileSize) {
                const message =
                    maxSizeAttr.message ||
                    GetUiControlText("lblFileSizeExceeded");
                if (message) {
                    errors.push(createError(field.fieldId, message));
                }
                break;
            }
        }

        return errors;
    };

    const validateFile = (field, fieldValue) => {
        const errors = [];
        errors.push(...validateFileExtension(field, fieldValue));
        errors.push(...validateFileSize(field, fieldValue));
        return errors;
    };
    // #endregion

    // #region ===============  Number Validation

    const validateNumber = (field, fieldValue) => {
        const errors = [];

        if (!fieldValue || fieldValue.value === "" || fieldValue.value == null) {
            return errors;
        }

        const numberValue = parseFloat(fieldValue.value);
        if (isNaN(numberValue)) {
            const msg =
                getFieldAttribute(field, "numbermessage")?.message ||
                GetUiControlText("lblInvalidNumberFormat");
            if (msg) {
                errors.push(createError(field.fieldId, msg));
            }
            return errors;
        }

        const minAttr = getFieldAttribute(field, "min");
        const maxAttr = getFieldAttribute(field, "max");

        if (minAttr && !isNaN(parseFloat(minAttr.value)) && numberValue < parseFloat(minAttr.value)) {
            const msg =
                minAttr.message ||
                GetUiControlText("lblValueMustBeGreaterOrEqual");
            if (msg) {
                errors.push(createError(field.fieldId, msg));
            }
        }

        if (maxAttr && !isNaN(parseFloat(maxAttr.value)) && numberValue > parseFloat(maxAttr.value)) {
            const msg =
                maxAttr.message ||
                GetUiControlText("lblValueMustBeLessOrEqual");
            if (msg) {
                errors.push(createError(field.fieldId, msg));
            }
        }

        return errors;
    };
    // #endregion

    // #region ===============  List Validation (Tabulator list fields)

    const validateList = (field, fieldValue) => {
        let errors = [];

       
        const data = fieldValue?.value || [];
        const rows = Array.isArray(data) ? data : [];

        //   min/max count
        const maxCountAttr = field.attributes?.find(attr => attr.name === "maxcount");
        const minCountAttr = field.attributes?.find(attr => attr.name === "mincount");

        //   min/max new count
        const maxCountnewAttr = field.attributes?.find(attr => attr.name === "maxcountnew");
        const minCountnewAttr = field.attributes?.find(attr => attr.name === "mincountnew");

        const maxCount = maxCountAttr ? parseInt(maxCountAttr.value, 10) : null;
        const minCount = minCountAttr ? parseInt(minCountAttr.value, 10) : null;

        const maxCountNew = maxCountnewAttr ? parseInt(maxCountnewAttr.value, 10) : null;
        const minCountNew = minCountnewAttr ? parseInt(minCountnewAttr.value, 10) : null;

        const newCount = rows.filter(r => !r?.IsOld).length;

        if (minCount !== null && rows.length < minCount) {
            const baseMsg =
                minCountAttr?.message ||
                GetUiControlText("lblMinimumRowsRequired");
            const msg = baseMsg ? `${baseMsg} (${minCount})` : baseMsg;
            errors.push(createError(field.fieldId, msg));
        }

        if (maxCount !== null && rows.length > maxCount) {
            const baseMsg =
                maxCountAttr?.message ||
                GetUiControlText("lblMaximumRowsExceeded");
            const msg = baseMsg ? `${baseMsg} (${maxCount})` : baseMsg;
            errors.push(createError(field.fieldId, msg));
        }

        if (minCountNew !== null && newCount < minCountNew) {
            const baseMsg =
                minCountnewAttr?.message ||
                GetUiControlText("lblMinimumNewRowsRequired");
            const msg = baseMsg ? `${baseMsg} (${minCountNew})` : baseMsg;
            errors.push(createError(field.fieldId, msg));
        }

        if (maxCountNew !== null && newCount > maxCountNew) {
            const baseMsg =
                maxCountnewAttr?.message ||
                GetUiControlText("lblMaximumNewRowsExceeded");
            const msg = baseMsg ? `${baseMsg} (${maxCountNew})` : baseMsg;
            errors.push(createError(field.fieldId, msg));
        }

     

        return errors;
    };
    // #endregion

    // #region ===============  Regex Validation

    const validateRegex = (field, fieldValue) => {
        const errors = [];
        const regexAttr =
            getFieldAttribute(field, "regex") ||
            getFieldAttribute(field, "pattern");

        if (!regexAttr || !fieldValue || fieldValue.value === "") return errors;

        let regexPattern;
        try {
            regexPattern = new RegExp(regexAttr.value);
        } catch {
            return errors;
        }

        if (!regexPattern.test(fieldValue.value)) {
            const errorMessage =
                regexAttr.message ||
                GetUiControlText("lblInvalidFormat");
            if (errorMessage) {
                errors.push(createError(field.fieldId, errorMessage));
            }
        }
        return errors;
    };
    // #endregion

    // #region ===============  Checkbox Validation

    const validateCheckbox = (field, fieldValue) => {
        const errors = [];
        const requiredAttr = getFieldAttribute(field, "required");
        const isChecked = !!(fieldValue && fieldValue.value);

        if (requiredAttr && !isChecked) {
            const errorMessage =
                requiredAttr.message ||
                GetUiControlText("lblRequiredField") ||
                GetUiControlText("lblDefaultValidationMessage");
            errors.push(createError(field.fieldId, errorMessage));
        }
        return errors;
    };
    // #endregion

    // #region ===============  Date Attribute Validation

    function isFieldDisabled(field) {
        if (field.disabled === true || field.isDisabled === true) return true;

        const hasDisableAttr = field.attributes?.some(a =>
            ["disablevalidation", "disabled"].includes(a.name?.trim().toLowerCase())
        );
        if (hasDisableAttr) return true;

        try {
            const el = document.getElementById(field.domId || `field_${field.fieldId}` || field.id);
            if (el) {
                if (el.disabled === true || el.readOnly === true) return true;
                if (el.getAttribute?.("aria-disabled") === "true") return true;

                const style = window.getComputedStyle(el);
                if (style.display === "none" || style.visibility === "hidden") return true;
            }
        } catch { }

        return false;
    }

    const createDateError = (field, message) => ({
        fieldId: field.fieldId,
        FieldName: field.fieldName,
        error: message
    });

    const calculateMaxDateFromNow = (daysFromNow) => {
        const maxDateFromNow = new Date();
        maxDateFromNow.setDate(maxDateFromNow.getDate() + parseInt(daysFromNow, 10));
        return maxDateFromNow;
    };

    const calculateMinDateFromNow = (daysFromNow) => {
        const minDateFromNow = new Date();
        minDateFromNow.setDate(minDateFromNow.getDate() + parseInt(daysFromNow, 10));
        return minDateFromNow;
    };

    const calculateMinAgeDate = (minAge) => {
        const now = new Date();
        now.setFullYear(now.getFullYear() - parseInt(minAge, 10));
        now.setHours(0, 0, 0, 0);
        return now;
    };

    const calculateMin7DaysFromField = () => {
        const relatedField = $('input[calcmin7daysFrom="true"]:not([id*="view"])');
        const relatedFieldValue = (relatedField.val() || "").toString().trim();
        if (!relatedFieldValue) return null;
        const baseDate = parseDate(relatedFieldValue);
        if (!baseDate) return null;
        const minDate = new Date(baseDate);
        minDate.setDate(minDate.getDate() - 7);
        return minDate;
    };

    const validateDate = (field, fieldValue) => {
        const dateErrors = [];
        if (!(field.type === "date" || field.type === "datetime") || !fieldValue?.value) {
            return dateErrors;
        }

        let currentDate = field.type === "date"
            ? (typeof getDate === "function" ? getDate(fieldValue.value) : parseDate(fieldValue.value))
            : (typeof getDateTime === "function" ? getDateTime(fieldValue.value) : parseDate(fieldValue.value));

        if (!currentDate) return dateErrors;

        (field.attributes || []).forEach(attr => {
            const attrName = (attr.name || "").toLowerCase();
            const attrValue = attr.value;
            const message =
                attr.message ||
                GetUiControlText("lblDefaultValidationMessage");

            switch (attrName) {
                case "min7days": {
                    const min7DaysDate = calculateMin7DaysFromField();
                    if (min7DaysDate && currentDate < min7DaysDate) {
                        dateErrors.push(createDateError(field, message));
                    }
                    break;
                }
                case "mindate": {
                    if (attrValue && currentDate < parseDate(attrValue)) {
                        dateErrors.push(createDateError(field, message));
                    }
                    break;
                }
                case "maxdate": {
                    if (attrValue && currentDate > parseDate(attrValue)) {
                        dateErrors.push(createDateError(field, message));
                    }
                    break;
                }
                case "maxdaysfromnow": {
                    if (isFieldDisabled(field) || fieldValue?.disabled === true) return;
                    if (currentDate > calculateMaxDateFromNow(attrValue)) {
                        dateErrors.push(createDateError(field, message));
                    }
                    break;
                }
                case "mindaysfromnow": {
                    if (isFieldDisabled(field) || fieldValue?.disabled === true) return;
                    if (currentDate < calculateMinDateFromNow(attrValue)) {
                        dateErrors.push(createDateError(field, message));
                    }
                    break;
                }
                case "minage": {
                    if (isFieldDisabled(field) || fieldValue?.disabled === true) return;
                    if (currentDate > calculateMinAgeDate(attrValue)) {
                        dateErrors.push(createDateError(field, message));
                    }
                    break;
                }
            }
        });

        return dateErrors;
    };
    // #endregion

    // #region ===============  Required & Length Validation

    const validateRequired = (field, fieldValue) => {
        const errors = [];
        if (shouldIgnoreValidation(field)) return errors;
        const requiredAttr = getFieldAttribute(field, "required");
        if (!requiredAttr) return errors;
        if (shouldIgnoreRequiredValidation(field)) return errors;

        const value = fieldValue?.value ?? field.value;

        if (isEmpty(value) || value === "[]" || value == null) {
            const msg =
                requiredAttr.message ||
                GetUiControlText("lblRequiredField");
            errors.push(createError(field.fieldId, msg));
        }
        return errors;
    };

    const validateTextLength = (field, fieldValue) => {
        const errors = [];
        const maxLengthAttr = getFieldAttribute(field, "maxlength");
        const minLengthAttr = getFieldAttribute(field, "minlength");

        const maxLength = maxLengthAttr ? parseInt(maxLengthAttr.value, 10) : null;
        const minLength = minLengthAttr ? parseInt(minLengthAttr.value, 10) : null;

        const value = (fieldValue?.value ?? field.value ?? "").toString();

        if (!value) return errors;

        if (maxLength !== null && value.length > maxLength) {
            const msg =
                maxLengthAttr?.message ||
                GetUiControlText("lblMaxLengthExceeded");
            errors.push(createError(field.fieldId, msg));
        }

        if (minLength !== null && value.length < minLength) {
            const msg =
                minLengthAttr?.message ||
                GetUiControlText("lblMinLengthNotReached");
            errors.push(createError(field.fieldId, msg));
        }

        return errors;
    };

    const validateTextareaLength = (field, fieldValue) => {
        const errors = [];
        const maxLengthAttribute = field.attributes?.find(
            attr => attr.name.toLowerCase() === "maxlength"
        );
        const maxLength = maxLengthAttribute ? parseInt(maxLengthAttribute.value, 10) : null;
        const value = fieldValue ? (fieldValue.value || "") : (field.value || "");

        if (maxLength !== null && value.length > maxLength) {
            const msg =
                maxLengthAttribute?.message ||
                GetUiControlText("lblMaxLengthExceeded");
            errors.push(createError(field.fieldId, msg));
        }

        return errors;
    };
    // #endregion

    // #region ===============  Date Group (Sequence) Validation (dategroup + dategroupindex)

    function validateDateFields() {
        const dateElements = document.querySelectorAll('.form-control[dategroup]:not([id*="view"])');
        const groupedFields = {};
        const errors = [];

        dateElements.forEach(element => {
            const dateGroup = element.getAttribute("dategroup");
            const dateGroupIndex = parseInt(element.getAttribute("dategroupindex"), 10);
            const fieldValue = (element.value || "").trim();

            if (!groupedFields[dateGroup]) groupedFields[dateGroup] = [];
            groupedFields[dateGroup].push({
                id: element.id,
                value: fieldValue,
                index: isNaN(dateGroupIndex) ? Number.MAX_SAFE_INTEGER : dateGroupIndex
            });
        });

        const parseDateString = (str) => {
            if (!str) return null;
            if (str.includes("/")) {
                const [dd, mm, yyyy] = str.split("/");
                const d = new Date(`${yyyy}-${mm}-${dd}T00:00:00`);
                return isNaN(d) ? null : d.getTime();
            }
            const iso = new Date(str);
            return isNaN(iso) ? null : iso.getTime();
        };

        for (const dateGroup in groupedFields) {
            const fields = groupedFields[dateGroup];
            if (!fields || fields.length < 2) continue;

            fields.sort((a, b) => a.index - b.index);

            for (let i = 0; i < fields.length - 1; i++) {
                const currentField = fields[i];
                const nextField = fields[i + 1];

                const currentVal = parseDateString(currentField.value);
                const nextVal = parseDateString(nextField.value);

                if (currentVal === null || nextVal === null) continue;

                if (currentVal >= nextVal) {
                    const msg = GetUiControlText("lblDateRangeInvalid") || GetUiControlText("lblDefaultValidationMessage");
                    const fieldId = nextField.id.replace("field_", "");
                    errors.push(createError(fieldId, msg));
                }
            }
        }

        return errors;
    }
    // #endregion

    // #region ===============  NotEqual Validation (attributes: notEqual*, with custom messages)

    const validateNotEqualFields = (fields) => {
        const notEqualElements = document.querySelectorAll(".form-control[notEqual]");
        let errors = [];

        notEqualElements.forEach(element => {
            const fieldId = element.id.replace("field_", "");

            const matchingField = fields.find(f => f.fieldId === fieldId);
            if (!matchingField) return;

            const notEqualAttributes = Array.from(element.attributes)
                .filter(attr => attr.name.toLowerCase().startsWith("notequal"));

            notEqualAttributes.forEach(attr => {
                const targetFieldAttr = attr.value.trim();
                const targetField = $(`.form-control[${targetFieldAttr}]`);

                if (targetField.length === 0) {
                    console.warn(`Target field with attribute [${targetFieldAttr}] not found.`);
                    return;
                }

                const currentValue = (element.value || "").trim();
                const targetValue = (targetField.val() || "").trim();

                if (currentValue === targetValue && currentValue !== "") {
                    let customErrorMessage = null;

                    if (matchingField.attributes) {
                        const matchingAttr = matchingField.attributes.find(a => a.value === targetFieldAttr);
                        if (matchingAttr && matchingAttr.message) {
                            customErrorMessage = matchingAttr.message;
                        }
                    }

                    const fallbackMessage = GetUiControlText("lblNotEqualValidation");
                    const msg = customErrorMessage || fallbackMessage;
                    errors.push(createError(fieldId, msg));
                }
            });
        });

        return errors;
    };
    // #endregion

    // #region ===============  Field Validators Registry

    const fieldValidators = {
        number: validateNumber,
        checkbox: validateCheckbox,
        file: validateFile,
        fileV2: validateFile,
        date: validateDate,
        datetime: validateDate,
        textarea: validateTextareaLength,
        list: validateList
    };
    // #endregion

    // #region ===============  Core validateField / validateFields / validateInput

    function validateField(field, fieldValues) {
        let errors = [];
        if (!field) return errors;

        const $fieldEl = $(`#field_${field.fieldId}`);
        if ($fieldEl.length && !$fieldEl.is(":visible")) {
            return errors;
        }

        const fieldValue =
            (fieldValues || []).find(fv => fv.fieldId === field.fieldId) ||
            { fieldId: field.fieldId, value: field.value };

        if (shouldIgnoreValidation(field)) {
            return errors;
        }

        errors.push(...validateRequired(field, fieldValue));

        errors.push(...validateTextLength(field, fieldValue));

        errors.push(...validateRegex(field, fieldValue));

        const validator = fieldValidators[field.type];
        if (validator) {
            errors.push(...validator(field, fieldValue));
        }

        return errors;
    }

    function validateFields(fields, valuesObj) {
        let all = [];
        const fieldValues = [];

        (fields || []).forEach(f => {
            const val = valuesObj ? valuesObj[f.fieldId] : undefined;
            fieldValues.push({ fieldId: f.fieldId, value: val });
        });

        (fields || []).forEach(f => {
            all.push(...validateField(f, fieldValues));
        });

        return all;
    }

    function validateInput(input, field) {
        const fieldId = field.fieldId;
        const value = $(input).val();
        const errors = validateField(field, [{ fieldId, value }]);

        const errSelector = `#error_${fieldId}`;
        if (errors.length) {
            $(input).addClass("error");
            showError(errSelector, errors[0].error);
        } else {
            $(input).removeClass("error");
            clearError(errSelector);
        }
    }
    // #endregion


    // ================== EXPORT ==================
    ns.resetValidationErrors = resetValidationErrors;

    ns.validateRemarks = validateRemarks;
    ns.validateRemark = validateRemark;
    ns.isRemarksValid = isRemarksValid;

    ns.getFieldAttribute = getFieldAttribute;
    ns.getFieldAttributeValue = getFieldAttributeValue;
    ns.shouldIgnoreValidation = shouldIgnoreValidation;
    ns.shouldIgnoreRequiredValidation = shouldIgnoreRequiredValidation;

    ns.validateField = validateField;
    ns.validateFields = validateFields;
    ns.validateInput = validateInput;

    ns.validateNotEqualFields = validateNotEqualFields;
    ns.validateDateFields = validateDateFields; // dategroup + dategroupindex
    ns.validateDateGroups = validateDateGroups; // from/to style

})(formUtility);
