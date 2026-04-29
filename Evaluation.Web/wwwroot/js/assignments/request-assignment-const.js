const API_ENDPOINTS = window.API_ENDPOINTS || {};
var deprouting = sharedUtility().extractDepartmentName();
// Team Members Endpoints
API_ENDPOINTS.GET_TEAMS = `/Assignment/${deprouting}/GetTeams`;
API_ENDPOINTS.GET_MEMBERS_BY_TEAM = `/Assignment/${deprouting}/GetMembersByTeam`;
API_ENDPOINTS.GET_SCOPES = `/Assignment/${deprouting}/GetScopes`;
API_ENDPOINTS.GET_PENDING_STATUS = `/NDA/${deprouting}/GetPendingStatus`;

// NEW: Evaluation Request Assignment Endpoints
API_ENDPOINTS.GET_TEAM_MEMBERS_BY_EVALUATION_REQUEST = `/Assignment/${deprouting}/GetAssignmentByEvaluationRequest`;
API_ENDPOINTS.SUBMIT_EVALUATION_REQUEST_ASSIGNMENT = `/Assignment/${deprouting}/Submit`;
API_ENDPOINTS.DELETE_EVALUATION_REQUEST_ASSIGNMENT = `/Assignment/${deprouting}/Delete`;
API_ENDPOINTS.SEND_MAIL_NOTIFICATION = `/Assignment/${deprouting}/SendNotificationMailUser`;
window.API_ENDPOINTS = API_ENDPOINTS;
