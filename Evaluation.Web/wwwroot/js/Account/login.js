$('#Email').keypress(function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        Continue();
    }
});

$('#ContinueBtnId').click(function (event) {
    event.preventDefault();
    Continue();
});

$('#LoginBtnId').click(function (event) {
    event.preventDefault();
    Login();
});

$('#Password').keypress(function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        Login();
    }
});

$('#BackToEmailId').click(function (event) {
    event.preventDefault();
    $('#LoginPart2Id').hide();
    $('#LoginPart1Id').show();
});

/*------------------------------------------------------
   STEP 1: Check user type by email
------------------------------------------------------*/
const Continue = () => {
    const username = $('#Email').val()?.trim();
    if (!username) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterTheEmailOrQID'));
        return;
    }

    const options = {
        success: function (result) {
            if (!result || !result.data) {
                notificationUtil.error(uiControlsSetup().GetUiControlText('lblUserNotFound'));
                return;
            }

            switch (result.data.userType) {
                case 0: // FORM LOGIN (Email + Password)
                    if (result.data.forceRedirectToResetPassword) {
                        sharedUtility().RedirectToUrl(ConstantUrls.MobileResetPasswordURL);
                    } else {
                        $('#LoginPart1Id').hide();
                        $('#LoginPart2Id').fadeIn();
                        $('#Password').focus();
                    }
                    break;

                case 1: // SSO LOGIN
                    if (result.data.ssoRedirectUrl) {
                        window.location.replace(result.data.ssoRedirectUrl);
                    } else {
                        notificationUtil.error('SSO URL غير صالح.');
                    }
                    break;

                default:
                    notificationUtil.error('نوع المستخدم غير معروف.');
                    break;
            }
        },
        error: function (xhr) {
            notificationUtil.error(xhr.responseText || uiControlsSetup().GetUiControlText('lblUnexpectedError'));
        }
    };

    jqClient(options).Get(ConstantUrls.GetUserAuthType.replace('{username}', encodeURIComponent(username)));
};

/*------------------------------------------------------
   STEP 2: Normal form login (after email check)
------------------------------------------------------*/
const Login = () => {
    const redirectUrl = $('#RedirectUrl').val();
    const username = $('#Email').val()?.trim();
    const password = $('#Password').val()?.trim();

    if (!username) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterTheEmailOrQID'));
        return;
    }

    if (!password) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterThePassword'));
        return;
    }

    const data = { Username: username, Password: password };

    const options = {
        success: function (result) {
            if (result?.data) {
                SetLocalStorageValue(LocalStorageKeys.Token, result.data);
                const tokenPayload = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token));
                const expiration = parseInt(tokenPayload['TokenExpirationTime']);
                StartSessionTimer(expiration);

                sharedUtility().RedirectToUrl(redirectUrl || ConstantUrls.Home);
            } else {
                notificationUtil.error(uiControlsSetup().GetUiControlText('lblLoginFailed'));
            }
        },
        error: function (xhr) {
            notificationUtil.error(xhr.responseText || uiControlsSetup().GetUiControlText('lblLoginFailed'));
        }
    };

    jqClient(options).Post("/Account/Login", data);
};
