// form.api.js
(function (global) {

  
    async function fetchJSON(url) {
  
        let returndata = null;

        const options = {
            success: function (data) {
                if (data !== undefined && data !== null) returndata = data;
            }
        };

        await jqClient(options).SyncGet(url);
        return returndata;
    }

    async function fetchJSONWithoutLoader(url) {
     

        let returndata = null;

        const options = {
            success: function (data) {
                if (data !== undefined && data !== null) returndata = data;
            }
        };

        await jqClient(options).SyncGetWithoutLoader(url);
        return returndata;
    }

    async function fetchJSONHandelError(url) {
       
        let returndata = null;

        const options = {
            success: function (data) {
                if (data !== undefined && data !== null) returndata = data;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (typeof global.HandleAjaxError === "function") {
                    global.HandleAjaxError(jqXHR, textStatus, errorThrown);
                } else {
                    console.error("AJAX Error:", textStatus, errorThrown);
                }

                setTimeout(() => {
                    if (typeof global.sharedUtility === "function") {
                        global.sharedUtility().RedirectToModuleOrDefault();
                    } else if (document.referrer) {
                        global.location.href = document.referrer;
                    } else {
                        global.location.href = "/";
                    }
                }, 5000);
            }
        };

        await jqClient(options).SyncGet(url);
        return returndata;
    }

    async function fetchBlob(url) {
        try {
            const response = await fetch(url, { method: "GET" });
            if (!response.ok) return null;
            return await response.blob();
        } catch (error) {
            console.error("Error fetching blob:", error);
            return null;
        }
    }

    function getUrlParam(param) {
        const params = new URLSearchParams(global.location.search);
        return params.get(param);
    }

    function addQueryParameter(key, value) {
        const url = new URL(global.location.href);
        url.searchParams.set(key, value);
        history.pushState(null, "", url.toString());
    }

    function removeQueryParameter(keys) {
        const url = new URL(global.location.href);
        (keys || []).forEach(key => url.searchParams.delete(key));
        history.pushState(null, "", url.pathname + url.search);
    }

    function loadAllCssClasses() {
       
        const options = {
            success: function (data) {
                const css = data || [];
                css.forEach(function (cssClass) {
                    if (!cssClass || !cssClass.className) return;
                    const styles = cssClass.styles || "";
                    const className = cssClass.className;

                    $('head').append(`<style>.${className} {${styles}}</style>`);
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("LoadAllCssClasses failed:", textStatus, errorThrown);
            }
        };

        jqClient(options).Get("/FormRender/GetCssClasses");
    }

    global.FormApi = {
        fetchJSON,
        fetchJSONWithoutLoader,
        fetchBlob,
        fetchJSONHandelError,
        getUrlParam,
        addQueryParameter,
        removeQueryParameter,
        loadAllCssClasses
    };

})(window);
