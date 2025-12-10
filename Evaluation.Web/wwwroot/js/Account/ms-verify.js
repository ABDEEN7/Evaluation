

async function VerifyMSToken() {
    let authorizationCode = $("#code").val();

    if (authorizationCode) {
        let data = { code: authorizationCode };
        //debugger

        const options = {
            success: function (result) {

                debugger
                if (result.data) {
                    SetLocalStorageValue(LocalStorageKeys.Token, result.data);
                    let tokenExpirationTime = sharedUtility().ParseJwt(GetLocalStorageValue(LocalStorageKeys.Token))['TokenExpirationTime'];

                    let redirectUrl = GetLocalStorageValue(LocalStorageKeys.RedirectUrl);
                    if (redirectUrl) {
                        SetLocalStorageValue(LocalStorageKeys.RedirectUrl, '');
                        sharedUtility().RedirectToUrl(redirectUrl);
                    } else {
                        sharedUtility().RedirectToUrl(ConstantUrls.Home);

                    }
                }
                else {
                    sharedUtility().RedirectUnauthorized();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                debugger
                SetLocalStorageValue(LocalStorageKeys.ErrorTempMessage, jqXHR.responseJSON.Message)
                sharedUtility().RedirectUnauthorized();
            }
        };

        jqClient(options).Post(ConstantUrls.LoginMinistry, data);

    } else {
        sharedUtility().RedirectUnauthorized();
    }

}


