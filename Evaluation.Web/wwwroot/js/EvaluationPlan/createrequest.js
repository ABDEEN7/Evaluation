const formUtility = FormGeneratorsUtility();
const ActionTypes = formUtility.ACTION_TYPE;
window.dropdowns = window.dropdowns || [];
let PlanId;
let SchoolId;
let initialAction;

const GetUrlParam = (param) => {
    const params = new URLSearchParams(window.location.search);
    return params.get(param);
};

function redirectToDefault() {
    if (typeof sharedUtility === "function") {
        sharedUtility().RedirectToModuleOrDefault();
    } else if (window.locationReferrer) {
        window.location.href = document.referrer;
    } else {
        window.location.href = "/";
    }
}

async function InitializeCreatePlanRequest() {
   

    const createPlanService = await formUtility.fetchJSON(`/FormRender/GetCreatePlanService`);
    if (!createPlanService) {
        redirectToDefault();
        return;
    }
    const serviceName = (window.currentLang === "ar" ? createPlanService.nameAr : createPlanService.nameEn) || "";
    const headerEl = document.getElementById("serviceName");
    if (headerEl) headerEl.textContent = serviceName ? " - " + serviceName : "";

    const actions = createPlanService.actions || [];

    if (actions && actions.filter(a => a.isInitialAction === true).length === 1) {
        const firstAction = actions.find(a => a.actionTypeBackEndKey !== ActionTypes.SaveAsDraft);
        initialAction = firstAction ? firstAction.bakendName : initialAction;
        $("#ActionsDropDown").hide();
        $("label[for='ActionsDropDown']").hide();
        RenderActionFields(service.serviceRequestDTO)
    } else {
        fillActionDropDown(actions);
    }
    

}

const fillActionDropDown = (actions) => {
    if (!actions || actions.length < 2) {
        $("#ActionsDropDown").hide();
        $("label[for='ActionsDropDown']").hide();
        return;
    }

    actions.forEach(action => {
        const text = action.title || (window.currentLang === "ar" ? action.nameAr : action.nameEn);
        const option = new Option(text, action.bakendName);
        $("#ActionsDropDown").append($(option));
    });

    initialAction = $("#ActionsDropDown").val();

    $("#ActionsDropDown").on("change", function () {
        initialAction = $(this).val();
    });
};

const fillSchoolsDropDown = (schools) => {
    schools.forEach(s => {
        const text = `${s.schoolName || ""} - ${s.code || ""} - ${s.typeName || ""} - ${s.stageName || ""}`;
        const option = new Option(text, s.id);
        $("#School").append($(option));
    });

    $("#School").select2({
        width: "100%",
        dropdownCssClass: "manageselect2zindex",
        dropdownParent: $("#SchoolModal"),
        allowClear: true,
        placeholder: uiControlsSetup().GetUiControlText("lblSelect") || "Select…",
        minimumResultsForSearch: 0
    });

    if (SchoolId) $("#School").val(SchoolId).trigger("change");

    $("#School")
        .removeAttr("hidden")
        .on("change", function () {
            SchoolId = $(this).val();
        });

    $("label[for='School']").removeAttr("hidden");
};

const configureSchoolModal = () => {
    $("#SchoolModal").modal({ backdrop: "static", keyboard: false });
    $("#SchoolModal").modal("show");

    $("#submitButtonModal").off("click").on("click", async function () {
        if ($("#School").val()) {
            $("#School").closest(".mb-3").find(".messages").removeClass("text-danger").text("");
            await GetActionFields();
            $("#SchoolModal").modal("hide");
        } else {
            $("#School").closest(".mb-3").find(".messages")
                .addClass("text-danger")
                .text(uiControlsSetup().GetUiControlText("lblIsRequired"));
        }
    });

    $("#cancelButtonModal, .btn-close").off("click").on("click", function () {
        redirectToDefault();
    });
};

$("#Requestbtns1, #CloseRequestup").off("click").on("click", function () {
    redirectToDefault();
});

$("#goBackBtn").off("click").on("click", function () {
    redirectToDefault();
});

async function CheckCanCreateingDraft() {
    try {
        const url = `/EvaluationPlanRequest/CheckCanCreateingDraft?planId=${PlanId}`;
        const response = await jqClient().SyncGet(url);
        return response;
    } catch (e) {
        return false;
    }
}

async function GetActionFields() {
    try {
        let url = `/EvaluationPlanRequest/GetActionField?planId=${PlanId}`;
        if (initialAction) url += `&actionBackendKey=${encodeURIComponent(initialAction)}`;
        if (SchoolId) url += `&schoolId=${encodeURIComponent(SchoolId)}`;
        url += `&dropDownTypeIds=${encodeURIComponent(formUtility.dropDownTypeIds || [])}`;

        const request = await formUtility.fetchJSON(url);
        await RenderActionFields(request);
    } catch (e) {
        console.error("Error loading action fields:", e);
    }
}

async function RenderActionFields(request) {
    try {
        if (!request || !request.actionCustom) return;

        const actionDetails = request.actionCustom;
        const formGroups = actionDetails.formGroups || [];
        const dropdownsData = request.dropDownValues || [];

        if (dropdownsData && Array.isArray(dropdownsData)) {
            dropdownsData.forEach(item => {
                if (!dropdowns.some(x => x.id === item.id && x.dropDownTypeId === item.dropDownTypeId)) {
                    dropdowns.push(item);
                    formUtility.dropDownTypeIds.push(item.dropDownTypeId);
                }
            });
        }

        const attachments = request.planAttachments || request.attachments;
        if (attachments && attachments.length > 0) {
            formUtility.attachments.push(...attachments);
        }

        formUtility.renderActionView("content-container", formGroups, actionDetails);

        if (typeof InitializeTooltip === "function") InitializeTooltip();
    } catch (e) {
        console.error("Error rendering action fields:", e);
    }
}


