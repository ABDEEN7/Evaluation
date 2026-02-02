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
    const DisplayAlert = (msg, icon = null) => {

        if (msg) {

            if (icon && icon == 'success') {
                notificationUtil.success(msg);
            } else {
                if (icon && icon == 'warning') {
                    notificationUtil.warning(msg);
                } else {
                    notificationUtil.error(msg);
                }
            }
        }
    }
    const getRequestId = () => {
        const id = getUrlParam("id");
        return id ? id : getUrlParam("Evlid");
    };
    const getPlanId = () => getUrlParam("PlanId") || getUrlParam("PlanId");

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
                case "evaluationplan": {
                    const fieldId = `field_${field.fieldId}`;
                    const planObj = window.SubmitPlanHandler?.getFormPlanJson(fieldId);
                    value = planObj ? JSON.stringify(planObj) : null;
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
        let actionTypeName = actionDetails.actionType.backEndName;
        const { fields, valuesMap } = collectFieldValues(formGroups, renderType);

        const payloadFields = fields.map(f => ({
            fieldId: f.fieldId,
            type: f.type,
            value: valuesMap[f.fieldId]
        }));

        formData.append("requestId", getRequestId());

        formData.append("fieldValues", JSON.stringify(payloadFields));

        if (actionTypeName === ACTION_TYPE.ASSIGNT_TEAM) {
            const teamData = getAssignmentsDataByFieldId('assign');
            if (!teamData) return { formData, ok: false };
            formData.append("teamUsers", JSON.stringify(teamData));

        }
        

         //formData.append("ActionRemarks", remarksValue);

        if (typeof fu.validateRemarks === "function") {
            const remarksOk = fu.validateRemarks(formData);
            if (!remarksOk) return { formData, ok: false };
        }

        if (typeof fu.addAssignmentData === "function") {
            const actionTypeName = actionDetails?.actionType?.backEndName || actionDetails?.bakendName || "";
            const assignOk = fu.addAssignmentData(formData, actionTypeName);
            if (!assignOk) return { formData, ok: false };
        }

        return { formData, ok: true };
    }


    // ================================
    // #region 🔹 submitAction
    // ================================
   

    async function submitAction(actionDetails, formGroups, saveAsDraft) {
        var DepartmentRouting = sharedUtility().extractDepartmentName();

        const baseUrl =  `/ServiceRequest/${DepartmentRouting}/HandleRequest`;
        const renderType = RENDER_TYPE.ACTION;

        const normalizedGroups = normalizeFormGroups(formGroups);

        const validationResult = validateAll(normalizedGroups, renderType);
        if (!validationResult.isValid) return;

        const { formData, ok } = buildFormData(actionDetails, normalizedGroups, renderType);
        if (!ok) return;

        const actionName =actionDetails?.bakendName ||"";

        const planId = getPlanId();

        const serviceId =  actionDetails?.serviceId;
        if (serviceId) formData.set("serviceId", serviceId);

        formData.set("saveAsDraft", String(saveAsDraft));

        const qs = new URLSearchParams();
        qs.set("actionName", actionName);
        if (planId) qs.set("planId", planId);

        const postUrl = `${baseUrl}?${qs.toString()}`;

        const successFunction = function (result) {
            if (!result) {
                DisplayAlert("Unexpected empty response.", "danger");
                return;
            }
            let RequestId  = getRequestId();
            if (RequestId) {
                window.tempFileStorage = {};
               DisplayAlert('Form submitted successfully!', 'success');
                setTimeout(() => {
                    sharedUtility().RedirectToModuleOrDefault();
                }, 1000);
            } else {
                let message = saveAsDraft
                    ? uiControlsSetup().GetUiControlText('lblRequestSavedAsDraftSuccessfully')
                    : uiControlsSetup().GetUiControlText('lblRequestCreatedSuccessfully');

                if (result.requestNumber) {
                    message = message.replace('{requestNumber}', result.requestNumber);
                }

                notificationUtil.confirmation(
                    {
                        title: message,
                        body: '',
                        okText: uiControlsSetup().GetUiControlText('lblOk'),
                        showCancelButton: false,
                    },
                    function () {
                        sharedUtility().RedirectToModuleOrDefault();
                    }
                );
            }
        };

        const errorFunction = function (xhr) {
            try {
                const response = xhr?.responseText ? JSON.parse(xhr.responseText) : null;
                if (Array.isArray(response)) {
                    showErrors(response);
                } else {
                    DisplayAlert(response?.message || "An unexpected error occurred.", 'danger');
                }
            } catch {
                DisplayAlert("An unexpected error occurred.", 'danger');
            }
        };

        jqClient({})
            .PostFormData(postUrl, formData)
            .done(successFunction)
            .fail(errorFunction);
    }




    // ================================
    // #region 🔹 Expose
    // ================================
    ns.submitAction = submitAction;

})(window.serviceRequestForm, window.formUtility);
