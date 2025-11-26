
(function (global) {
    async function fetchJSON(url) {
        let returndata;
        const options = {
            success: function (data) {
                if (data) returndata = data;
            }
        };
        await jqClient(options).SyncGet(url);
        return returndata;
    }

    async function fetchJSONWithoutLoader(url) {
        let returndata;
        const options = {
            success: function (data) {
                if (data) returndata = data;
            }
        };
        await jqClient(options).SyncGetWithoutLoader(url);
        return returndata;
    }

    async function fetchBlob(url) {
        let returndata;
        try {
            const response = await fetch(url);
            if (response.ok) {
                returndata = await response.blob();
            }
        } catch (error) {
            console.error("Error fetching data: ", error);
        }
        return returndata;
    }

    async function fetchJSONHandelError(url) {
        let returndata;
        const options = {
            success: function (data) {
                if (data) returndata = data;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                HandleAjaxError(jqXHR, textStatus, errorThrown);
                setTimeout(() => {
                    sharedUtility().RedirectToModuleOrDefault();
                }, 5000);
            }
        };
        await jqClient(options).SyncGet(url);
        return returndata;
    }

    const GetUrlParam = (param) => {
        const params = new URLSearchParams(window.location.search);
        const result = params.get(param);
        return result;
    };

    function addQueryParameter(key, value) {
        const url = new URL(window.location.href);
        url.searchParams.set(key, value);
        history.pushState(null, null, url.toString());
    }

    function removeQueryParameter(keys) {
        const url = new URL(window.location.href);
        keys.forEach(key => url.searchParams.delete(key));
        history.pushState(null, null, url.pathname + url.search);
    }

    const LoadAllCssClasses = () => {
        const options = {
            success: function (data) {
                formUtility.cssClasses = data;
                data.forEach(function (cssClass) {
                    $('head').append(`<style>.${cssClass.className} {${cssClass.styles}}</style>`);
                });
            }
        };
        const url = "/ServiceRequest/GetCssClasses";
        jqClient(options).Get(url);
    };

    global.FormApi = {
        fetchJSON,
        fetchJSONWithoutLoader,
        fetchBlob,
        fetchJSONHandelError,
        GetUrlParam,
        addQueryParameter,
        removeQueryParameter,
        LoadAllCssClasses
    };
})(window);
