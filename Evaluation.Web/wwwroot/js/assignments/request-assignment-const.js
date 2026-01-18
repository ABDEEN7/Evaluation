const API_ENDPOINTS = window.API_ENDPOINTS || {};

// Team Members Endpoints
API_ENDPOINTS.GET_TEAMS = '/Assignment/GetTeams';
API_ENDPOINTS.GET_MEMBERS_BY_TEAM = '/Assignment/GetMembersByTeam';
API_ENDPOINTS.GET_SCOPES = '/Assignment/GetScopes';
API_ENDPOINTS.GET_PENDING_STATUS = '/NDA/GetPendingStatus';

// NEW: Evaluation Request Assignment Endpoints
API_ENDPOINTS.GET_TEAM_MEMBERS_BY_EVALUATION_REQUEST = '/Assignment/GetAssignmentByEvaluationRequest';
API_ENDPOINTS.SUBMIT_EVALUATION_REQUEST_ASSIGNMENT = '/Assignment/Submit';
API_ENDPOINTS.DELETE_EVALUATION_REQUEST_ASSIGNMENT = '/Assignment/Delete';

window.API_ENDPOINTS = API_ENDPOINTS;
