// form.submit.js

window.formUtility = window.formUtility || {};

window.serviceRequestForm = window.serviceRequestForm || {};

(function (ns, fu) {

    const { RENDER_TYPE, ACTION_TYPE } = window.FormConstants || {};

    // ================================
    // #region 🔹 Helpers
    // ================================

    const getText = (key) =>
        (window.uiControlsSetup && uiControlsSetup().GetUiControlText(key)) || "";

    const getUrlParam = (key) => {
        const params = new URLSearchParams(window.location.search);
        return (params.get(key) || "").replace("#", "");
    };

    const getRequestId = () => getUrlParam("id");
    const getScholarshipId = () => getUrlParam("scholarshipId") || getUrlParam("schId");

    const normalizeFormGroups = (formGroups) => {
        if (!formGroups) return [];
        if (!Array.isArray(formGroups)) return [formGroups];
        return formGroups;
    };

    function flattenFieldsFromFormGroups(formGroups) {
        const groups = normalizeFormGroups(formGroups);
        return groups.flatMap(g => g.fields || []);
    }

    function getFieldDomId(field, renderType) {
        const prefix = renderType === RENDER_TYPE.PREVIEW
            ? "field_view_"
            : "field_";
        return `${prefix}${field.fieldId}`;
    }

    function getListDomId(field, renderType) {
        const prefix = renderType === RENDER_TYPE.PREVIEW
            ? "table_View_"
            : renderType === RENDER_TYPE.MAJOR
                ? "table_Major_View_"
                : "table_";
        return `${prefix}${field.fieldId}`;
    }

    // ================================
    // #region 🔹 collectFieldValues (from formGroups)
    // ================================
    function collectFieldValues(formGroups, renderType) {
        const fields = flattenFieldsFromFormGroups(formGroups);
        const valuesMap = {};

        fields.forEach(field => {
            const id = getFieldDomId(field, renderType);
            const $el = $("#" + id);

            let value = null;

            switch ((field.type || "").toLowerCase()) {
                case "checkbox":
                    value = $el.is(":checked");
                    break;

                case "list": {
                    const tableId = getListDomId(field, renderType);
                    if (window.Tabulator) {
                        const tables = Tabulator.findTable("#" + tableId);
                        value = (tables && tables.length) ? tables[0].getData() : [];
                    } else {
                        value = [];
                    }
                    break;
                }

                case "select2":
                case "dropdown": {
                    const v = $el.val();
                    if ($el.attr("multiple")) {
                        value = Array.isArray(v) ? v : (v ? [v] : []);
                    } else {
                        value = v;
                    }
                    break;
                }

                case "datetime":
                case "date":
                case "phone":
                case "text":
                case "textarea":
                case "number":
                case "vacancyseat":
                case "tinymce":
                case "jqte":
                default:
                    value = $el.val();
                    break;
            }

            valuesMap[field.fieldId] = value;
        });

        return { fields, valuesMap };
    }

    // ================================
    // #region 🔹 validation
    // ================================
    function validateAll(formGroups, renderType) {
        fu.resetValidationErrors && fu.resetValidationErrors();

        const { fields, valuesMap } = collectFieldValues(formGroups, renderType);

        let errors = [];

        if (typeof fu.validateFields === "function") {
            errors = fu.validateFields(fields, valuesMap) || [];
        }

        if (typeof fu.validateDateGroups === "function") {
            const dateErrors = fu.validateDateGroups() || [];
            errors = errors.concat(dateErrors);
        }

        if (typeof fu.validateNotEqualFields === "function") {
            const notEqualErrors = fu.validateNotEqualFields(fields) || [];
            errors = errors.concat(notEqualErrors);
        }

        if (errors.length) {
            if (typeof fu.showFieldErrors === "function") {
                fu.showFieldErrors(errors);
            } else if (typeof fu.showFieldError === "function") {
                errors.forEach(err => fu.showFieldError(err));
            }
        }

        return { isValid: errors.length === 0, fields, valuesMap, errors };
    }

    // ================================
    // #region 🔹 buildFormData
    // ================================
    function buildFormData(actionDetails, formGroups, renderType) {
        const formData = new FormData();

        const { fields, valuesMap } = collectFieldValues(formGroups, renderType);

        const payloadFields = fields.map(f => ({
            fieldId: f.fieldId,
            type: f.type,
            value: valuesMap[f.fieldId]
        }));

        formData.append("RequestId", getRequestId());
        const schId = getScholarshipId();
        if (schId) {
            formData.append("ScholarshipId", schId);
        }

        if (actionDetails?.actionId) {
            formData.append("ActionId", actionDetails.actionId);
        }
        if (actionDetails?.actionType?.backEndName) {
            formData.append("ActionType", actionDetails.actionType.backEndName);
        }

        formData.append("Fields", JSON.stringify(payloadFields));

        if (typeof fu.validateRemarks === "function") {
            const remarksOk = fu.validateRemarks(formData);
            if (!remarksOk) {
                return { formData, ok: false };
            }
        }

        if (typeof fu.addAssignmentData === "function") {
            const actionTypeName = actionDetails?.actionType?.backEndName;
            const assignOk = fu.addAssignmentData(formData, actionTypeName);
            if (!assignOk) {
                return { formData, ok: false };
            }
        }

        return { formData, ok: true };
    }

    // ================================
    // #region 🔹 submitAction
    // ================================
   
    async function submitAction(actionDetails, formGroups, options = {}) {
        const url = options.url || "/FormRender/PerformAction";
        const renderType = RENDER_TYPE.ACTION;

        const normalizedGroups = normalizeFormGroups(formGroups);

        const validationResult = validateAll(normalizedGroups, renderType);
        if (!validationResult.isValid) {
            return;
        }

        const { formData, ok } = buildFormData(actionDetails, normalizedGroups, renderType);
        if (!ok) {
            return;
        }

        fu.coverSpin && fu.coverSpin(true);

        try {
            const resp = await fetch(url, {
                method: "POST",
                body: formData
            });

            if (!resp.ok) {
                const text = await resp.text();
                if (typeof options.onError === "function") {
                    options.onError(text, resp);
                } else {
                    console.error("Submit error:", text);
                    const msg = getText("lblDefaultValidationMessage");
                    if (msg && typeof window.DisplayAlert === "function") {
                        window.DisplayAlert(msg, "error");
                    }
                }
                return;
            }

            const data = await resp.json().catch(() => null);

            if (typeof options.onSuccess === "function") {
                options.onSuccess(data);
            } else {
                const successMsg = getText("lblActionSuccess");
                if (successMsg && typeof window.DisplayAlert === "function") {
                    window.DisplayAlert(successMsg, "success");
                }
                if (data && data.redirectUrl) {
                    window.location.href = data.redirectUrl;
                }
            }
        } catch (err) {
            console.error("Submit exception:", err);
            if (typeof options.onError === "function") {
                options.onError(err);
            } else {
                const msg = getText("lblDefaultValidationMessage");
                if (msg && typeof window.DisplayAlert === "function") {
                    window.DisplayAlert(msg, "error");
                }
            }
        } finally {
            fu.coverSpin && fu.coverSpin(false);
        }
    }

    // ================================
    // #region 🔹 Expose
    // ================================
    ns.submitAction = submitAction;

})(window.serviceRequestForm, window.formUtility);
