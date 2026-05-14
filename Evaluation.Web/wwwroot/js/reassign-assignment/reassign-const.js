var deprouting = sharedUtility().extractDepartmentName();

const API = {
    dropList: `/Reassign/${deprouting}/GetEvalFormItemLists`,
    load: `/Reassign/${deprouting}/GetUserAssignments`,
    save: `/Reassign/${deprouting}/UpdateReassignEvaluationRequestUser`
};