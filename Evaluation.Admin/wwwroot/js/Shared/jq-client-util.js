const jqClientAdvanced = (options) => {

    let http = {};
    http.SyncGet = SyncHttpGet;
    http.Get = HttpGet;
    http.Delete = HttpDelete;

    http.SyncPost = SyncHttpPost;
    http.Post = HttpPost;

    http.Delete = HttpDelete;
    http.PostFormData = HttpPostFormData;

    let defaults = {
        //baseUrl: baseApiUrl(),
        //headers: SharedHeader(),

    };

    // Extend defaults into options
    const settings = $.extend({}, defaults, options);

    async function SyncHttpGet(url) {
        settings.url = url;
        settings.method = 'Get';
        const response = await $.ajax(settings);
        return response;
    }

    function HttpGet(url) {

        settings.url = url;
        settings.method = 'Get';


        $.ajax(settings);

    }
    function HttpDelete(url) {

        settings.url = url;
        settings.method = 'DELETE';


        $.ajax(settings);

    }

    async function SyncHttpPost(url, jsonData) {
        settings.url = url;
        settings.method = 'Post';
        settings.data = JSON.stringify(jsonData);
        settings.contentType = 'application/json';
        const response = await $.ajax(settings);
        return response;

    }

    function HttpPost(url, jsonData) {
        //settings.url = settings.baseUrl.concat(url);
        settings.url = url;
        settings.method = 'Post';
        settings.data = JSON.stringify(jsonData);
        settings.contentType = 'application/json';
        $.ajax(settings);
    }


    function HttpPostFormData(url, formData) {
        settings.url = url;
        settings.method = 'Post';
        settings.dataType = 'json';
        settings.processData = false;
        settings.contentType = false;
        settings.data = formData;
        $.ajax(settings);
    }

    function HttpDelete(url) {

        settings.options = options;
        settings.url = url;
        settings.method = 'Delete';
        return $.ajax(settings);
    }

    return http;
}

