(function (global) {
    const uniqueIndexId = 'IndexForTabulator';

    const RENDER_TYPE = {
        PREVIEW: 'preview',
        ACTION: 'action',
    };

    const ACTION_TYPE = {
        INFO: "INFO",
        APPROVE: "APPROVE",
        REJECT: "REJECT",
        ASSIGN: "ASSIGN",
        INITIALACTION: "INITIALACTION",
        RETURNBACK: "RETURNBACK",
        CLOSE: "CLOSE",
        CLOSE_AND_UPDATE: "CLOSE_AND_UPDATE",
        EDIT: "Edit",
        EditDraft: "EditDraft",
        Approve_And_Assign: "APPROVE_AND_ASSIGN",
        RequestDataChange: "REQUEST_DATA_CHANGE",
        SubmitMissingData: "SUBMIT_MISSING_DATA",
        SaveAsDraft: "SaveAsDraft",
        INFO_Override_Approve: "INFO_Override_Approve",
        INFO_WITH_DRAFT: "INFO_WITH_DRAFT"
    };

    const ReadOnly_ACTION_TYPES = [
        ACTION_TYPE.REJECT,
        ACTION_TYPE.RETURNBACK,
        ACTION_TYPE.RequestDataChange,
    ];

    global.FormConstants = {
        uniqueIndexId,
        RENDER_TYPE,
        ACTION_TYPE,
        ReadOnly_ACTION_TYPES
    };

})(window);
