const jqClient = (options) => {


    async function SyncHttpGet(url) {
        settings.options = options;
        settings.url = url;
        settings.method = 'Get';
        settings.headers = sharedUtility().SharedHeader(true);
        const response = await $.ajax(settings);
        return response;
    }
    async function SyncHttpGetWithoutLoader(url) {
        settings.options = options;
        settings.url = url;
        settings.method = 'Get';
        settings.showLoader = false;
        settings.headers = sharedUtility().SharedHeader(true);
        const response = await $.ajax(settings);
        return response;
    }
    function HttpGet(url) {

        settings.options = options;
        settings.url = url;
        settings.method = 'Get';
        settings.headers = sharedUtility().SharedHeader(true);
        return $.ajax(settings);
    }
    function HttpGetBlob(url) {
        return fetch(url, {
            method: 'GET',
            headers: {
                'Accept': 'application/pdf',
                ...sharedUtility().SharedHeader(true)
            }
        })
            .then(response => {
                if (!response.ok) throw new Error('Network response was not ok');
                return response.blob();
            })
            .catch(error => {
                console.error('Fetch Blob error:', error);
                throw error;
            });
    }
    function HttpDelete(url) {

        settings.options = options;
        settings.url = url;
        settings.method = 'Delete';
        return $.ajax(settings);
    }

    function HttpWebPost(url, jsonData) {
        var formData = new FormData();

        formData.append('request', JSON.stringify(jsonData));

        Websettings.Mode = "APP";
        Websettings.options = options;
        Websettings.url = url;
        Websettings.method = 'POST';
        Websettings.data = formData;
        Websettings.processData = false;
        Websettings.contentType = false;
        settings.headers = sharedUtility().SharedHeader(true);
        return $.ajax(Websettings);
    }

    async function SyncHttpPost(url, jsonData) {
        settings.options = options;
        settings.url = url;
        settings.method = 'Post';
        settings.data = JSON.stringify(jsonData);
        const response = await $.ajax(settings);
        settings.headers = sharedUtility().SharedHeader(true);
        return response;
    }

    function HttpPost(url, jsonData) {
        //settings.url = settings.baseUrl.concat(url);
        settings.options = options;
        settings.url = url;
        settings.method = 'Post';
        settings.data = JSON.stringify(jsonData);
        settings.headers = sharedUtility().SharedHeader(true);
        return $.ajax(settings);
    }

    function HttpPut(url, jsonData) {
        //settings.url = settings.baseUrl.concat(url);
        settings.options = options;
        settings.url = url;
        settings.method = 'Put';
        settings.data = JSON.stringify(jsonData);
        return $.ajax(settings);
    }


    async function SyncHttpPostFormData(url, formData) {
        const customOptions = {
            beforeSend: function (xhr, options) {
                options.url = decodeURIComponent(sharedUtility().BaseApiUrl()) + options.url;
            },
            processData: false,
            contentType: false,
        };

        settings.options = customOptions;
        settings.url = url;
        settings.method = 'POST';
        settings.data = formData;
        const response = await $.ajax(settings);
        return response;
    }

    function HttpPostFormData(url, formData) {
        settings.options = options;
        settings.url = url;
        settings.processData = false;
        settings.contentType = false;
        settings.method = 'POST';
        settings.data = formData;
        settings.headers = sharedUtility().SharedHeader(true);
        return $.ajax(settings);
    }
    function HttpPutFormData(url, formData) {
        settings.options = options;
        settings.url = url;
        settings.processData = false;
        settings.contentType = false;
        settings.method = 'PUT';
        settings.data = formData;
        settings.headers = sharedUtility().SharedHeader(true);
        return $.ajax(settings);
    }

    function HttpPutFormData(url, formData) {
        settings.options = options;
        settings.url = url;
        settings.processData = false;
        settings.contentType = false;
        settings.method = 'PUT';
        settings.data = formData;
        settings.headers = sharedUtility().SharedHeader(true);
        return $.ajax(settings);
    }

    async function backgroundSyncPostRequest(url, jsonData) {

        const customOptions = {
            beforeSend: function (xhr, options) {
                options.url = decodeURIComponent(sharedUtility().BaseApiUrl()) + options.url;
            },

        };

        settings.options = customOptions;
        settings.url = url;
        settings.method = 'Post';
        settings.data = JSON.stringify(jsonData);
        const response = await $.ajax(settings);
        return response;
    }

    function FetchFile(url, nameFile) {
        sharedFn().CoverSpin(true);
        return fetch(`${decodeURIComponent(sharedUtility().BaseApiUrl())}${url}`, {
            headers: sharedUtility().SharedHeader(true),
            redirect: 'manual'
        })
            .then(response => {
                if (response.status === 302) {
                    throw new Error('Redirect detected - download not available');
                }
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const serverName = response.headers.get('Content-Disposition')
                    ?.split('filename=')[1]?.split(';')[0]
                    ?.replace(/['"]/g, '');
                const extension = serverName ? serverName.split('.').pop() : 'ext';
                const filename = nameFile ? `${nameFile}.${extension}` : (serverName || 'file.ext');

                return response.blob().then(blob => {
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = url;
                    a.download = filename;
                    document.body.appendChild(a);
                    a.click();
                    setTimeout(() => {
                        document.body.removeChild(a);
                        window.URL.revokeObjectURL(url);
                    }, 100);
                });
            })
            .catch(error => {
                console.error('Download error:', error);
                alert('Error downloading file: ' + error.message);
            }).finally(() => {
                sharedFn().CoverSpin(false);
            });
    }

    let defaults = {
        baseUrl: sharedUtility().BaseApiUrl(),
        headers: sharedUtility().SharedHeader(),
    };
    let Webdefaults = {
        baseUrl: sharedUtility().BaseAppUrl(),
        headers: sharedUtility().SharedHeader(),
    };

    // Extend defaults into options
    const settings = $.extend({}, defaults, options);
    const Websettings = $.extend({}, Webdefaults, options);


    let http = {};
    http.SyncGet = SyncHttpGet;
    http.SyncGetWithoutLoader = SyncHttpGetWithoutLoader;
    http.Get = HttpGet;
    http.GetBlob = HttpGetBlob;
    http.WebPost = HttpWebPost;

    http.SyncPost = SyncHttpPost;
    http.Post = HttpPost;
    http.Put = HttpPut;
    http.Delete = HttpDelete;
    http.BackgroundSyncPostRequest = backgroundSyncPostRequest;
    http.SyncPostFormData = SyncHttpPostFormData;
    http.PostFormData = HttpPostFormData;
    http.PutFormData = HttpPutFormData;
    http.fetchFile = FetchFile;

    return http;
}

