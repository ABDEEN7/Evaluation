var deprouting = sharedUtility().extractDepartmentName();

const API = {
    dropList: `/ReassignAssignment/${deprouting}/GetEvalFormItemLists`,
    load: `/ReassignAssignment/${deprouting}/GetUserAssignments`,
    save: `/ReassignAssignment/${deprouting}/UpdateReassignEvaluationRequestUser`
};