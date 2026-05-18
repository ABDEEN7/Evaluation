var deprouting = sharedUtility().extractDepartmentName();

const API = {
    dropList: `/ReassignAssignment/${deprouting}/GetEvalFormItemLists`,
    load: `/ReassignAssignment/${deprouting}/GetUserAssignments`,
    save: `/ReassignAssignment/${deprouting}/ReassignEvaluationRequestUser`
};
const REASSIGN_KEYS = {
    NoAssignmentsMessage: 'NoAssignmentsMessage',
    ReassignSuccessMessage: 'ReassignSuccessMessage',
    ReassignFailedMessage: 'ReassignFailedMessage',
    FromUserRequiredMessage: 'FromUserRequiredMessage',
    ToUserRequiredMessage: 'ToUserRequiredMessage',
    SelectAssignmentRequiredMessage: 'SelectAssignmentRequiredMessage',
    LoadingMessage: 'LoadingMessage',
    SavingMessage: 'SavingMessage',
    SelectUserPlaceholder: 'SelectUserPlaceholder'
};
function getLang(key) {
    const el = document.querySelector(`span.${key}, label.${key}, h5.${key}`);
    return el ? el.innerText.trim() : key;
}