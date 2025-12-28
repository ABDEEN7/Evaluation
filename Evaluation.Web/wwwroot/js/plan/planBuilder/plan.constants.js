(function (global) {
    'use strict'; 
    const uniqueIndexId = 'IndexForPlanTable';

    // Action types for workflow
    const ACTION_TYPE = {
        CREATE: "CREATE",                    // Creating new plan
        EDIT: "EDIT",                        // Editing existing plan
        APPROVE: "APPROVE",                  // Approving plan
        REJECT: "REJECT",                    // Rejecting plan
        VIEW: "VIEW",                        // View only
        EDIT_DRAFT: "EDIT_DRAFT",           // Editing draft
        APPROVE_WITH_CHANGES: "APPROVE_WITH_CHANGES", // Approve changes
        REQUEST_CHANGE: "REQUEST_CHANGE"     // Request changes
    };

    // Action types that should be read-only
    const ReadOnly_ACTION_TYPES = [
        ACTION_TYPE.APPROVE,
        ACTION_TYPE.REJECT,
        ACTION_TYPE.VIEW
    ];

    // Plan field types
    const PLAN_FIELD_TYPE = {
        TITLE: 'title',
        PLAN_TYPE: 'planType',
        SEMESTER: 'semester',
        DATE_RANGE: 'dateRange'
    };

    // School table field types
    const SCHOOL_FIELD_TYPE = {
        SELECT: 'select',
        SCHOOL_NAME: 'schoolName',
        VISIT_DATE: 'visitDate',
        LAST_EVAL_DATE: 'lastEvalDate',
        VISIT_TYPE: 'visitType',
        ACADEMIC_YEAR: 'academicYear',
        ACTIONS: 'actions'
    };

    // Plan validation rules
    const VALIDATION_RULES = {
        TITLE: {
            required: true,
            minLength: 3,
            maxLength: 200
        },
        PLAN_TYPE: {
            required: true
        },
        SEMESTER: {
            required: false, // Required only for semester type
            conditional: true
        },
        DATE_RANGE: {
            required: true,
            validRange: true
        },
        SCHOOLS: {
            required: true,
            minCount: 1
        },
        VISIT_DATE: {
            required: true,
            withinPlanRange: true
        },
        VISIT_TYPE: {
            required: true
        }
    };

    // Table configuration
    const TABLE_CONFIG = {
        pageSize: 10,
        defaultSortColumn: 'name',
        defaultSortDirection: 'asc',
        enableSearch: true,
        enableFilters: true,
        enablePagination: true
    };

    // Filter field types
    const FILTER_FIELDS = {
        SCHOOL_NAME: 'schoolName',
        LAST_EVAL_DATE: 'lastEvalDate',
        CREATED_DATE: 'createdDate',
        NEXT_EVAL_DATE: 'nextEvalDate',
        PREVIOUS_RESULT: 'previousResult',
        VISIT_TYPE: 'visitType'
    };

    // Rating classes mapping
    const RATING_CLASSES = {
        'Perfect': 'bg-success',
        'VeryGood': 'bg-info',
        'Good': 'bg-primary',
        'Acceptable': 'bg-secondary',
        'Week': 'bg-danger'
    };

    // Plan type backend names
    const PLAN_TYPE_BACKEND = {
        YEAR: 'Year',
        MONTH: 'Month',
        SEMESTER: 'Semester',
        CUSTOM: 'Custom'
    };

    // API Endpoints
    const API_ENDPOINTS = {
        GET_SCHOOLS: '/School/GetSchoolsPlan',
        GET_PLAN_TYPES: '/PlanType/GetPlanTypes',
        GET_SEMESTERS: '/Plan/GetSemesters',
        GET_VISITS: '/School/GetVisits',
        GET_VACATION_DATES: '/AcademicYear/GetVcationDate',
        CREATE_PLAN: '/Plan/CreatePlan',
        //INSERTORUPUDATEPLAN: '/Plan/ApprovePlan',
        INSERTORUPDATEPLAN: '/Plan/InsertOrUpdatePlan',
        UPDATE_PLAN: '/Plan/UpdatePlan',
        GET_PLAN_DETAILS: '/Plan/GetPlansWithunSelectedSchoolsDetails'
    };

    // Export all constants
    global.PlanConstants = {
        uniqueIndexId,
        ACTION_TYPE,
        ReadOnly_ACTION_TYPES,
        PLAN_FIELD_TYPE,
        SCHOOL_FIELD_TYPE,
        VALIDATION_RULES,
        TABLE_CONFIG,
        FILTER_FIELDS,
        RATING_CLASSES,
        PLAN_TYPE_BACKEND,
        API_ENDPOINTS
    };

})(window);