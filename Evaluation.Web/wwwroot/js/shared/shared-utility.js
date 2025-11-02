



const sharedUtility = () => {
    const createAuthorizationAjaxHeader = (isMultipart = false) => {

        if (isMultipart)
            return {
                "Authorization": "Bearer " + "",//GetLocalStorageValue(LocalStorageKeys.Token),
                "type": "",//GetLocalStorageValue(LocalStorageKeys.Type),
                'lang': getCookie('lang') ? getCookie('lang') : 'en'
            }

        else
            return {
                "Authorization": "Bearer " + "",// GetLocalStorageValue(LocalStorageKeys.Token),
                "type": "",//GetLocalStorageValue(LocalStorageKeys.Type),
                'Content-Type': 'application/json',
                'lang': getCookie('lang') ? getCookie('lang') : 'en'
            }
    }

    const setCookie = (name, value, daysToExpire) => {
        var expirationDate = new Date();
        expirationDate.setDate(expirationDate.getDate() + daysToExpire);

        var cookieString = name + '=' + encodeURIComponent(value) + '; expires=' + expirationDate.toUTCString() + '; path=/';

        document.cookie = cookieString;
    }

    const getCookie = (name) => {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }

    const isFullUrl = (url) => {
        const urlPattern = /^(https?:\/\/)/i; // Matches "http://" or "https://"
        return urlPattern.test(url);
    }

    const RedirectToModuleOrDefault = () => {

        let url = decodeURIComponent(baseAppUrl());
        //if (userProfileDetailsInfo) {


        //    if (userProfileDetailsInfo.RoutingList && userProfileDetailsInfo.RoutingList.length > 0) {
        //        if (userProfileDetailsInfo.RoutingList.length == 1) {
        //            url = CombineWithBaseWebAppUrlOrDefault(['/home', userProfileDetailsInfo.RoutingList[0]]);
        //        }
        //    } else {
        //        let depName = "scholarship";//GetLocalStorageValue(LocalStorageKeys.ModuleName);
        //        if (depName) {
        //            url = CombineWithBaseWebAppUrlOrDefault(['/home', depName]);
        //        }

        //    }
        //}
        let depName = "/Scholarship";//GetLocalStorageValue(LocalStorageKeys.ModuleName);
        if (depName) {
            url = url.concat(depName)
        }
        window.location.href = url;

    }

    const redirectToUrl = (url, openNewBlank = false) => {
        if (url) {
            //debugger
            if (!isFullUrl(url)) {
                url = `${baseAppUrl()}${url}`;
            }
            url = decodeURIComponent(url);
            if (!openNewBlank) {
                window.location.replace(url);
            } else {
                window.open(url, '_blank');
                //window.close();
            }
        }
    }

    const coverSpin = (show) => {
        if (show) {
            //$('body').hide();
            $('#cover-spin').show();
        } else {
            $('#cover-spin').hide();
            //$('body').show();
        }

    }



    const existingToken = () => {
        let result = false;

        const token = GetLocalStorageValue(LocalStorageKeys.Token);
        if (token) {

            const tokenData = parseJwt(token);

            const expiryTimestamp = tokenData['exp'];
            const currentTimestamp = Math.floor(Date.now() / 1000);

            // Check if the token is expired
            if (expiryTimestamp < currentTimestamp) {
                SetLocalStorageValue(LocalStorageKeys.Token, '');
            } else {
                result = true;
            }
        }
        return result;
    }




    const redirectUnauthorized = (isRedirectToUrl = true) => {

        let url = ConstantUrls.LoginURL;

        // Check if `redirectUrl` is already in the current URL before appending
        if (isRedirectToUrl && !window.location.href.includes('redirectUrl=')) {
            url = url.concat('?redirectUrl=', encodeURIComponent(window.location.href));
            SetLocalStorageValue(LocalStorageKeys.RedirectUrl, window.location.href);
        }


        //if (existingToken()) {
            if (false) {
            let data = { token: '' };
            const options = {
                success: function (result) {
                    if (result.data) {
                        SetLocalStorageValue(LocalStorageKeys.Token, '');
                        redirectToUrl(url);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    switch (jqXHR.status) {
                        case 401:
                            SetLocalStorageValue(LocalStorageKeys.Token, '');
                            redirectToUrl(url);
                            break;
                        case 403:
                            sharedUtility().RedirectAccessDenied();
                            break;
                        default:
                            break;
                    }
                }

            };
            jqClient(options).Post(ConstantUrls.LogoutUrl, data);
        } else {
            redirectToUrl(url);
        }

    }

    const redirectAccessDenied = () => {

        let url = ConstantUrls.AccessDenied;
        url = url.concat('?returnurl=', encodeURIComponent(window.location.href));
        redirectToUrl(url);
    }
    const parseJwt = (token) => {
        const base64Url = token.split('.')[1]; // Get the payload part of the JWT
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/'); // Decode base64Url to base64
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    }

    const baseApiUrl = () => getCookie('webApiBaseURL'); //"https://localhost:14066/api";

    const baseAppUrl = () => getCookie("webAppBaseURL") //"https://localhost:44375";

    const displayAlert = (msg, icon = null) => {
        if (msg) {

            if (icon && icon == 'success') {
                notificationUtil.message(msg);
            } else {
                if (icon && icon == 'warning') {
                    notificationUtil.warning(msg);
                } else {
                    notificationUtil.error(msg);
                }
            }
        }
    }

    const InitializeTooltip = () => {
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    }





    //////////////////////

    //-----------------------------------------------local storage--------------------------------------------------------


    const GetLocalStorageValue = (key) => {
        let result = localStorage.getItem(key);
        return result;
    }

    const SetLocalStorageValue = (key, value) => {
        localStorage.setItem(key, value);
    }

    const RemoveLocalStorageValue = (key) => {
        localStorage.removeItem(key);
    }
    const SetLocalStorageValueByUserId = (userId, key, value) => {
        let userData = JSON.parse(localStorage.getItem(userId)) || {};
        userData[key] = value;
        localStorage.setItem(userId, JSON.stringify(userData));
    }
    const GetLocalStorageValueByUserId = (userId, key) => {
        let userData = JSON.parse(localStorage.getItem(userId));
        if (userData && key in userData) {
            return userData[key];
        } else {
            return null;
        }
    }

    const RemoveLocalStorageValueByUserId = (userId, key) => {
        let userData = JSON.parse(localStorage.getItem(userId));
        if (userData && key in userData) {
            delete userData[key];
            localStorage.setItem(userId, JSON.stringify(userData));
        }
    }

    const RemoveAllLocalStorageDataByUserId = (userId) => {
        localStorage.removeItem(userId);  // Remove the user's entire data entry
    }

    const tooltipSpan = (text) => {
        const escapedHtml = text.replace(/</g, "&lt;").replace(/>/g, "&gt;");
        const escapedJs = text.replace(/'/g, "\\'").replace(/"/g, '&quot;');
        return `<span class="copy-to-clipboard" title="${escapedHtml}">${escapedHtml}</span>`;
        // style="cursor: pointer;" onclick="sharedUtility().CopyToClipboard('${escapedJs}')" in case of copy needed
    }

    const copyToClipboard = (text) => {
        // Create a temporary textarea element
        const textarea = document.createElement('textarea');
        textarea.value = text;
        document.body.appendChild(textarea);

        // Select and copy the text
        textarea.select();
        document.execCommand('copy');

        // Remove the temporary textarea
        document.body.removeChild(textarea);

        // Show a confirmation message with the copied text
        notificationUtil.success(`Text copied to clipboard: ${text}`, 'success');

    }
    //--------------------------------------------------------------------


    //--------------------------------------------------------------------------------------
    let result = {};




    result.GetCookie = getCookie;
    result.SetCookie = setCookie;

    result.RedirectToUrl = redirectToUrl;



    result.RedirectUnauthorized = redirectUnauthorized;
    result.RedirectAccessDenied = redirectAccessDenied;
    result.ExistingToken = existingToken;
    result.CoverSpin = coverSpin;

    result.BaseApiUrl = baseApiUrl;
    result.BaseAppUrl = baseAppUrl;
    result.SharedHeader = createAuthorizationAjaxHeader;
    result.ParseJwt = parseJwt;
    result.ParseJwt = parseJwt;
    result.DisplayAlert = displayAlert;
    result.InitializeTooltip = InitializeTooltip;
    result.RedirectToModuleOrDefault = RedirectToModuleOrDefault;



    result.SetLocalStorageValue = SetLocalStorageValue;
    result.GetLocalStorageValue = GetLocalStorageValue;
    result.RemoveLocalStorageValue = RemoveLocalStorageValue;
    result.SetLocalStorageValueByUserId = SetLocalStorageValueByUserId;
    result.GetLocalStorageValueByUserId = GetLocalStorageValueByUserId;
    result.RemoveLocalStorageValueByUserId = RemoveLocalStorageValueByUserId;
    result.RemoveAllLocalStorageDataByUserId = RemoveAllLocalStorageDataByUserId;
    result.TooltipSpan = tooltipSpan;
    result.CopyToClipboard = copyToClipboard;




    return result;
}

