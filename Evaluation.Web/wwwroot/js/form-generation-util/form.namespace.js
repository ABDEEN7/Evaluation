// form.entry.js
(function (w) {

    const fu = w.formUtility = w.formUtility || {};

    // ================= API =================
    if (w.FormApi) {
        fu.fetchJSON = w.FormApi.fetchJSON;
        fu.fetchJSONWithoutLoader = w.FormApi.fetchJSONWithoutLoader;
        fu.fetchBlob = w.FormApi.fetchBlob;
        fu.fetchJSONHandelError = w.FormApi.fetchJSONHandelError;
    }

    // ================= RENDER =================
    if (w.serviceRequestForm) {
        fu.renderActionView = w.serviceRequestForm.renderActionView;
        fu.renderPreviewView = w.serviceRequestForm.renderPreviewView;
    }

    // ================= SUBMIT =================
    if (w.serviceRequestForm) {
        fu.submitAction = w.serviceRequestForm.submitAction;
    }

    // ================= SHARED STATE =================
    fu.attachments = fu.attachments || [];
    fu.dropDownTypeIds = fu.dropDownTypeIds || [];

})(window);
