
let activeAjaxRequests = 0;

// --------------------------------------------
// Token & Header Utilities
// --------------------------------------------
const updateTokenFromResponse = (xhr) => {
    const newToken = xhr?.getResponseHeader?.("newToken");
    const newExpiration = xhr?.getResponseHeader?.("expirationTime");

    if (newToken) SetLocalStorageValue(LocalStorageKeys.Token, newToken);
    if (newExpiration) SetLocalStorageValue(LocalStorageKeys.ExpirationTime, newExpiration);
};

const maybeExtendToken = () => {
    const remaining = GetRemainingTimeInSeconds();
    const total = GetTokenTotalDuration();
    if (!remaining || !total) return;

    const lower = total * 0.25;
    const upper = total * 0.5;
    if (remaining > lower && remaining <= upper) {
        ExtendTokenSessionWithoutRedirect();
    }
};

// --------------------------------------------
// Spinner Management
// --------------------------------------------
const showSpinner = () => sharedUtility().CoverSpin(true);
const hideSpinner = () => {
    activeAjaxRequests = Math.max(0, activeAjaxRequests - 1);
    if (activeAjaxRequests === 0) sharedUtility().CoverSpin(false);
};

// --------------------------------------------
// Success / Complete / Error Handlers
// --------------------------------------------
const handleAjaxSuccess = (result, status, xhr) => {
    // Reserved for specific per-request overrides if needed
};

const handleAjaxCompleted = (xhr) => {
    if (!sharedUtility().ExistingToken()) return;
    updateTokenFromResponse(xhr);
    maybeExtendToken();
};

const handleAjaxError = (jqXHR, textStatus, errorThrown, redirectUrl = false) => {
    if (errorThrown === "abort") return; // ignore cancelled requests

    const { status, responseJSON } = jqXHR;

    if (status === 401) return sharedUtility().RedirectUnauthorized(redirectUrl);
    if (status === 403) return sharedUtility().RedirectAccessDenied();

    const resp = responseJSON;

    if (Array.isArray(resp) && resp.length > 0 && resp[0]?.error) {
        const msg = resp.length > 3
            ? uiControlsSetup().GetUiControlText('lblSomeFieldsAreInvalid')
            : resp.map(e => e.error).join('<br>');
        notificationUtil.error(msg);
        return;
    }

    const message = resp?.Message || resp?.message || JSON.stringify(resp);
    if (uiControlsSetup().AnyUiBackendLabel(message)) {
        notificationUtil.error(uiControlsSetup().GetUiControlText(message));
    } else {
        notificationUtil.error(message);
    }
};

// --------------------------------------------
// Default jQuery AJAX Setup
// --------------------------------------------
$.ajaxSetup({
    cache: false,
    contentType: 'application/json',
    dataType: 'json',

    beforeSend: (xhr, options) => {
        activeAjaxRequests++;
        showSpinner();

        const baseUrl = options.Mode === "APP"
            ? decodeURIComponent(sharedUtility().BaseAppUrl())
            : decodeURIComponent(sharedUtility().BaseApiUrl());

        options.url = baseUrl + options.url;
        options.headers = sharedUtility().SharedHeader();
    },

    success: handleAjaxSuccess,

    complete: (xhr) => {
        handleAjaxCompleted(xhr);
        hideSpinner();
    },

    error: handleAjaxError
});


