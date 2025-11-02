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



$('#BackToEmailId').click(function (event) {
    event.preventDefault();
   
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
                    if (result.data.ssoRedirectUrl) {
                        window.location.replace(result.data.ssoRedirectUrl);
                    } else {
                        notificationUtil.error('SSO URL غير صالح.');
                    }
             
        },
        error: function (xhr) {
            notificationUtil.error(xhr.responseText || uiControlsSetup().GetUiControlText('lblUnexpectedError'));
        }
    };

    jqClient(options).Get(`/Account/CheckUserAuth?username=${username}`);
};


