let sessionTimer;
let countDownTimer;
let seconds = 0;
let alertDurationBeforeSessionEnd = 5;//minutes
//let _sessionTimeoutDuration = 0;

let activeToken;
const StartSessionTimer = (sessionTimeoutDuration) => {

    activeToken = GetLocalStorageValue(LocalStorageKeys.Token);

    //_sessionTimeoutDuration = sessionTimeoutDuration;
    //let delay = (sessionTimeoutDuration - alertDurationBeforeSessionEnd); // delay in minutes
    let delay = sessionTimeoutDuration * .75; // delay in minutes

    if (sessionTimer) clearTimeout(sessionTimer); // Clear the existing timer
    if (countDownTimer) clearInterval(countDownTimer); // Clear the existing timer
    Swal.close();

    if (delay <= 0) {
        delay = 0;
    }

    sessionTimer = setTimeout(function () {
        notificationUtil.confirmation(
            {
                title: '',
                body: '',
                okText: uiControlsSetup().GetUiControlText('lblExtend'),
                cancelText: uiControlsSetup().GetUiControlText('lblLogout')
            },
            function () {
                ExtendTokenSession();
            },
            function (result) {

                // Check if user clicked the "Logout" button
                if (result.dismiss === Swal.DismissReason.cancel) {
                    // Force redirect to logout if cancel button was clicked (Logout action)
                    sharedUtility().RedirectUnauthorized(true);
                } else {
                    // Otherwise, check remaining time on cancel
                    let seconds = GetRemainingTimeInSeconds();

                    if (seconds <= 0) {
                        // Redirect if session has expired
                        sharedUtility().RedirectUnauthorized(true);
                    } else {
                        // Clear the existing timer and start a new session timer
                        clearInterval(sessionTimer);
                        StartSessionTimer(sessionTimeoutDuration);
                    }
                }
            },




        );
        StartSwalMessageCountDownTimer();
    }, delay * 60 * 1000); // Show the warning before the timeout

}

const StartSwalMessageCountDownTimer = () => {
    clearInterval(countDownTimer); // Clear the existing timer

    seconds = GetRemainingTimeInSeconds();

    countDownTimer = setInterval(function () {


        let real_timeout_seconds = GetRemainingTimeInSeconds();

        if (real_timeout_seconds <= 0) {
            sharedUtility().RedirectUnauthorized();
        } else {
            let sessionTimeoutDuration = real_timeout_seconds / 60;

            let currentToken = GetLocalStorageValue(LocalStorageKeys.Token);
            if (currentToken !== activeToken) {
                activeToken = currentToken;
                StartSessionTimer(sessionTimeoutDuration);
            }

            if (seconds <= 0) {

                clearInterval(countDownTimer); // Clear the existing timer
                //debugger
                StartSessionTimer(sessionTimeoutDuration);
            }
        }


        let title = uiControlsSetup().GetUiControlText('lblSessionWillExpireSoonTitle');
        let html = uiControlsSetup().GetUiControlText('lblSessionWillExpireSoonMessage');

        let mins = Math.floor(seconds / 60);
        let secs = seconds % 60;

        // Pad seconds with a leading zero if needed (e.g., 1:09 instead of 1:9)
        let displayTime = `${mins}:${secs.toString().padStart(2, '0')}`;

        Swal.update({
            title: title,
            html: html.replace('{seconds}', displayTime)
        });

        seconds--; // Decrement seconds
    }, 1000);
}


const ExtendTokenSession = () => {
    if (sharedUtility().ExistingToken()) {
        clearInterval(countDownTimer);

        const options = {
            beforeSend: function (xhr, options) {
                options.url = decodeURIComponent(sharedUtility().BaseApiUrl()) + options.url;
            },
            success: function (result) {
                if (result.data) {
                    SetLocalStorageValue(LocalStorageKeys.Token, result.data);

                    let tokenExpirationTime = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token))['TokenExpirationTime'];
                    StartSessionTimer(tokenExpirationTime); // Reset timer with new expiration time
                } else {
                    sharedUtility().RedirectUnauthorized();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                switch (jqXHR.status) {
                    case 401:
                        //debugger
                        SetLocalStorageValue(LocalStorageKeys.Token, '');
                        sharedUtility().RedirectToUrl(ConstantUrls.LoginURL);
                        break;

                    case 403:
                        sharedUtility().RedirectAccessDenied();
                        break;
                    default:
                        break;

                }
            }
        };
        jqClient(options).Get(ConstantUrls.RefreshTokenURL);

    } else {
        sharedUtility().RedirectUnauthorized();
    }
}

const GetRemainingTimeInSeconds = () => {
    if (sharedUtility().ExistingToken()) {
        let tokenExpiry = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token))['exp'];
        const currentTimestamp = Math.floor(Date.now() / 1000);  // current time in seconds


        let remainingTimeInSeconds = tokenExpiry - currentTimestamp;
        if (remainingTimeInSeconds > 0) {
            const result = remainingTimeInSeconds > 0 ? remainingTimeInSeconds : 0;
            return result;
        }
    }

    return 0;
}

const CheckAndResumeSessionTimer = () => {
    if (sharedUtility().ExistingToken()) {
        // Calculate the remaining time in seconds until token expiry
        let remainingTimeInSeconds = GetRemainingTimeInSeconds();

        const adjustedDelay = remainingTimeInSeconds > 0 ? remainingTimeInSeconds / 60 : 0;

        // Start the session timer with the adjusted delay in minutes
        StartSessionTimer(adjustedDelay);

    }
};



function GetTokenTotalDuration() {
    if (sharedUtility().ExistingToken()) {

        const payload = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token));
        const exp = parseInt(payload?.TokenExpirationTime);
        return exp * 60;
    }

    return 0;
}


const ExtendTokenSessionWithoutRedirect = () => {
    if (sharedUtility().ExistingToken()) {
        clearInterval(countDownTimer);
        const options = {
            beforeSend: function (xhr, options) {
                options.url = decodeURIComponent(sharedUtility().BaseApiUrl()) + options.url;
            },
            success: function (result) {
                if (result.data) {
                    SetLocalStorageValue(LocalStorageKeys.Token, result.data);

                    let tokenExpirationTime = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token))['TokenExpirationTime'];
                    StartSessionTimer(tokenExpirationTime); // Reset timer with new expiration time
                } 
            },
            error: function (jqXHR, textStatus, errorThrown) {
               
            }
        };
        jqClient(options).Get(ConstantUrls.RefreshTokenURL);
    } 
}


