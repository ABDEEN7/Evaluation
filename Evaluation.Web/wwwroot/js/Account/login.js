



$('#Email').keypress(function (event) {
    if (event.keyCode == 13) {
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
    if (event.keyCode == 13) {
        event.preventDefault();
        Login();
    }
});



$('#BackToEmailId').click(function (event) {
    $('#LoginPart2Id').hide();
    $('#LoginPart1Id').show();
});





const Continue = () => {
    //debugger
    //debugger
    let username = $('#Email').val();

    if (username) {

        const options = {
            success: function (result) {

                switch (result.data.userType) {
                    case 0://LDAP_OR_Form
                        if (result.data.forceRedirectToResetPassword) {
                            sharedUtility().RedirectToUrl(ConstantUrls.MobileResetPasswordURL);
                        } else {
                            $('#LoginPart2Id').show();
                            $('#LoginPart1Id').hide();
                        }
                        break;
                    case 1://SSO
                        window.location.replace(result.data.ssoRedirectUrl);
                        break;
                    default:
                }

            }
        };

        jqClient(options).Get(ConstantUrls.GetUserAuthType.replace('{username}', encodeURI(username)));

    } else {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterTheEmailOrQID'))
    }
}

const Login = () => {
    let redirectUrl = $('#RedirectUrl').val();
    let username = $('#Email').val();
    let password = $('#Password').val();

    if (!username) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterTheEmailOrQID'));
        return;
    }

    if (!password) {
        notificationUtil.error(uiControlsSetup().GetUiControlText('lblEnterThePassword'));
        return;
    }

    let data = { Username: username, Password: password };

    const options = {
        success: function (result) {

            //debugger
            if (result.data) {
                SetLocalStorageValue(LocalStorageKeys.Token, result.data);

                let tokenExpirationTime = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token))['TokenExpirationTime'];
                StartSessionTimer(parseInt(tokenExpirationTime));
                if (redirectUrl) {
                    sharedUtility().RedirectToUrl(redirectUrl);

                } else {
                    sharedUtility().RedirectToUrl(ConstantUrls.Home);

                }
            }

        },
    };

    jqClient(options).Post("/Account/LoginStudent", data);

}




