$('#Email').keypress(function (event) {
    if (event.keyCode === 13) {
        event.preventDefault();
        checkUserAndRedirect();
    }
});

$('#ContinueBtnId').click(function (event) {
    event.preventDefault();
    checkUserAndRedirect();
});



$('#BackToEmailId').click(function (event) {
    event.preventDefault();
   
});

/*------------------------------------------------------
   STEP 1: Check user type by email
------------------------------------------------------*/
const checkUserAndRedirect = () => {
    const username = $('#Email').val()?.trim();
    if (!username) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterTheEmailOrQID'));
        return;
    }

    const options = {
        success: function (result) {
            if (!result || !result.ssoRedirectUrl) {
                notificationUtil.error(uiControlsSetup().GetUiControlText('lblUserNotFound'));
                return;
            }

            if (result.ssoRedirectUrl) {
                window.location.replace(result.ssoRedirectUrl);
            } else {
                notificationUtil.error('SSO URL غير صالح.');
            }
        },
        error: function (xhr) {
            if (xhr.status === 200 && xhr.responseText?.startsWith("http")) {
                window.location.replace(xhr.responseText);
                return;
            }

            notificationUtil.error(
                xhr.responseText || uiControlsSetup().GetUiControlText('lblUnexpectedError')
            );
        }
    };

    jqClient(options).Post(`/Account/CheckUserAuth?username=${username}`);
};


