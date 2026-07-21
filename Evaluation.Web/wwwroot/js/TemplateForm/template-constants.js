// ================================================
// TABLE CONFIG
// ================================================
const TABLE_CONFIG = {
    pageSize: 10,
    defaultSortColumn: 'updateDate',
    defaultSortDirection: 'desc',
    enableSearch: true,
    enableFilters: true,
    enablePagination: true
};

// ================================================
// API ROUTES
// ================================================
var deprouting = sharedUtility().extractDepartmentName();
const API_ROUTES = {
    // EvalForm
    getAllForms: () => `/TemplateForm/${deprouting}/GetAllTemplateForm`,
    saveForm: () => `/TemplateForm/${deprouting}/SaveTemplateForm`,
    updateForm: () => `/TemplateForm/${deprouting}/UpdateTemplateForm`,
    deleteForm: () => `/TemplateForm/${deprouting}/DeleteTemplateForm`,

    // FormItem
    getAllFormItems: (id) => `/TemplateForm/${deprouting}/GetAllTemplateFormItems?TemplateFormId=${id}`,
    saveFormItem: () => `/TemplateForm/${deprouting}/SaveTemplateFormItem`,
    updateFormItem: () => `/TemplateForm/${deprouting}/UpdateTemplateFormItem`,
    deleteFormItem: () => `/TemplateForm/${deprouting}/DeleteTemplateFormItem`,
    getFormItemsFromDepartment: (id) => `/TemplateForm/${deprouting}/GetAllFormItemsFromDepartment?TemplateFormId=${id}`,

    // SubFormItem
    saveSubFormItem: () => `/TemplateForm/${deprouting}/SaveEvaluationSubFormItem`,
    updateSubFormItem: () => `/TemplateForm/${deprouting}/UpdateEvaluationSubFormItem`,
    deleteSubFormItem: () => `/TemplateForm/${deprouting}/DeleteEvaluationSubFormItem`,

    // FormScope
    getAllFormScopes: (id) => `/TemplateForm/${deprouting}/GetAllFormScope?formIdValue=${id}`,
    saveFormScope: () => `/TemplateForm/${deprouting}/SaveFormScope`,
    updateFormScope: () => `/TemplateForm/${deprouting}/UpdateFormScope`,
    deleteFormScope: () => `/TemplateForm/${deprouting}/DeleteFormScope`,

    // FormItemConfig
    getAllFormItemConfig: (id) => `/TemplateForm/${deprouting}/GetAllFormItemConfig?evalFormId=${id}`,
    saveFormItemConfig: () => `/TemplateForm/${deprouting}/SaveFormItemConfig`,
    deleteFormItemConfig: () => `/TemplateForm/${deprouting}/DeleteFormItemConfig`,
    updateFormItemConfig: () => `/TemplateForm/${deprouting}/UpdateFormItemConfig`,

    // Lookups
    getFormItemLists: (id) => `/TemplateForm/${deprouting}/GetEvalFormItemLists?formId=${id}`,
    getScopeList: () => `/TemplateForm/${deprouting}/GetScopesForFormItem`,
};