const API_ENDPOINTS = window.API_ENDPOINTS || {};

// Team Members Endpoints
API_ENDPOINTS.GET_TEAMS = '/TeamMember/GetTeams';
API_ENDPOINTS.GET_MEMBERS_BY_TEAM = '/TeamMember/GetMembersByTeam';
API_ENDPOINTS.GET_SCOPES = '/TeamMember/GetScopes';
API_ENDPOINTS.GET_PENDING_STATUS = '/NDA/GetPendingStatus';

// NEW: Evaluation Request Assignment Endpoints
API_ENDPOINTS.GET_TEAM_MEMBERS_BY_EVALUATION_REQUEST = '/TeamMember/GetTeamMemberByEvaluationRequest';
API_ENDPOINTS.SUBMIT_EVALUATION_REQUEST_ASSIGNMENT = '/TeamMember/Submit';
API_ENDPOINTS.DELETE_EVALUATION_REQUEST_ASSIGNMENT = '/TeamMember/Delete';

window.API_ENDPOINTS = API_ENDPOINTS;
