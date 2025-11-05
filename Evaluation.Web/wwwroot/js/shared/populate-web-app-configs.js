let userPermissions = [];

const webAppConfigsSetup = (uiControlsOptions) => {
    const mainSection = $('#app-container');
    async function init({ pageNames = [], pages = [], permissions = [], additional_keys = [] }) {
        mainSection.hide();

        const lang = sharedUtility().GetCookie('lang') || 'en';

        const configPayload = {
            keys: additional_keys,
            pageNames,
            lang
        };
        const configForm = new FormData();
        configForm.append('request', JSON.stringify(configPayload));

        const controlsForm = new FormData();
        controlsForm.append('pages', JSON.stringify([...pages, 'WebCommonApp']));
        controlsForm.append('permissions', JSON.stringify(permissions));
        controlsForm.append('systemSettings', JSON.stringify(additional_keys));

        // Build two promises
        const getWebAppConfigsPromise = $.ajax({
            url: "/Home/GetWebAppConfigs",
            type: "POST",
            dataType: 'json',
            processData: false,
            contentType: false,
            data: configForm
        }).then((configResult)=>{
            if (configResult) {
                uiControlsSetup().Init(configResult.uiControls);
                systemSettingsSetup().Init(configResult.systemSettings);
            }
        }).catch(error => {
            console.error("Failed to fetch app configs", error);
            return null; // Continue even if this one fails
        });
        //Prepare requests but don't await yet
        const populatePromise = populateUserPagePermissions({ pageNames });

        // If no pages/permissions, skip GetPageControls call
        if (pages.length === 0 && permissions.length === 0) {
            // Run both promises at the same time
            const [_, configResult] = await Promise.all([
                populatePromise,
                getWebAppConfigsPromise
            ]);
            mainSection.show();
            return Promise.resolve({});
        }

        const getPageControlsPromise = $.ajax({
            url: "/Home/GetPageControls",
            type: "POST",
            headers: {
                "Authorization": "Bearer " + GetLocalStorageValue(LocalStorageKeys.Token),
                'lang': lang
            },
            dataType: 'json',
            processData: false,
            contentType: false,
            data: controlsForm
        });

        // Run both promises at the same time
        const [_, configResult, pageControlsResult] = await Promise.all([
            populatePromise,
            getWebAppConfigsPromise,
            getPageControlsPromise
        ]);

        mainSection.show();
        return pageControlsResult;
    }


    async function populateUserPagePermissions({ pageNames }) {



        if (sharedUtility().ExistingToken()) {
            userPermissions = [];
            //debugger
            var data = {};
            data.pageNames = pageNames;
            //data.lang = sharedUtility().GetCookie('lang') || 'en';
            //debugger
            let result = await jqClient().BackgroundSyncPostRequest("/Account/GetUserPagePermissions", data);

            if (result.data && result.data.length > 0) {

                $.each(result.data, function (index, item) {
                    userPermissions.push(item);
                });


            }
            return userPermissions;
        }
        return [];

    }

    let result = {};
    result.Init = init;
    return result;
};