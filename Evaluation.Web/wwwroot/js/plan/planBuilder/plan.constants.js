(function (global) {
    'use strict';
    const uniqueIndexId = 'IndexForPlanTable';



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
        CREATEDTO_DATE: 'createdToDate',
        NEXT_EVAL_DATE: 'nextEvalDate',
        PREVIOUS_RESULT: 'previousResult',
        VISIT_TYPE: 'visitType',
        SCHOOL_LEVEL: 'schoolLevel',
        GENDER: 'gender',
        GRADE: 'grade'
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
    var deprouting = sharedUtility().extractDepartmentName();

    // API Endpoints
    const API_ENDPOINTS = {
        GET_SCHOOLS: `/School/${deprouting}/GetSchoolsPlan`,
        GET_PLAN_TYPES: `/PlanType/${deprouting}/GetPlanTypes`,
        GET_SEMESTERS: `/Plan/${deprouting}/GetSemesters`,
        GET_VISITS: `/School/${deprouting}/GetVisits`,
        GET_VACATION_DATES: `/DepartmentHoliday/${deprouting}/GetAllHolidayDepartmentsOrg`,
        CREATE_PLAN: `/Plan/${deprouting}/CreatePlan`,
        INSERTORUPDATEPLAN: `/Plan/${deprouting}/InsertOrUpdatePlan`,
        UPDATE_PLAN: `/Plan/${deprouting}/UpdatePlan`,
        GET_PLAN_DETAILS: `/Plan/${deprouting}/GetPlansWithunSelectedSchoolsDetails`,
        GET_PARNT_ORGTREE: `/Org/${deprouting}/GetParentOrgTree`,
        GetFomrEvalMatrixValue: `/Plan/${deprouting}/GetFomrEvalMatrixValueList`,
        GET_CurrentAcademicYear: `/AcademicYear/${deprouting}/GetCurrentAcademicYearByDepartment`,
        GETEDUCATION_LEVEL: `/School/${deprouting}/GetEducationLevel`,
        GET_SCHOOL_GENDER: `/School/${deprouting}/GetSchoolGender`,
        GET_DEPARTMENT_CONFIG: `/Plan/${deprouting}/GetDepartmentConfig`

    };

    // Export all constants
    global.PlanConstants = {
        uniqueIndexId,
        VALIDATION_RULES,
        TABLE_CONFIG,
        FILTER_FIELDS,
        RATING_CLASSES,
        PLAN_TYPE_BACKEND,
        API_ENDPOINTS
    };

})(window);