
const uiControlsSetup = (uiControlsSetupOptions) => {

    //===========================================================


    const populateUiControl = (controlsList) => {

        if (controlsList) {

            let listToBeAdded = [];
            $.each(controlsList, function (index, item) {

                let exist = uiControls.applabel.find(c => c.backEndName === item.backEndName);
                if (!exist) {
                    listToBeAdded.push(item);
                }
            });

            $.each(listToBeAdded, function (index, item) {
                uiControls.applabel.push(item);
            });
        }
    }

    //===========================================================


    const getUiControlText = (backEndName) => {

        let lang = sharedUtility().GetCookie('lang');

        if (!lang)
            lang = 'en';

        let output = ''
        let uicontrolItem = uiControls.applabel.find(c => c.backEndName === backEndName);

        if (uicontrolItem) {
            if (lang == "ar" && uicontrolItem.arValue) {
                output = uicontrolItem.arValue
            }
            else
                if (uicontrolItem.enValue)
                    output = uicontrolItem.enValue;
        } else {
            output = `Missing [${backEndName}]`;
        }
        return output;

    }

    const anyUiBackendLabel = (backEndName) => {
        if (backEndName) {
            let uicontrolItem = uiControls.applabel.find(c => c.backEndName === backEndName);
            return !!uicontrolItem;
        }
        return false;
    }

    const getUiControl = (backEndName) => {

        if (backEndName) {

            let uicontrolItem = uiControls.applabel.find(c => c.backEndName === backEndName);

            return uicontrolItem;
        }

        return null;
    }

    //===========================================================

    const getPageNameText = () => {

        let lang = sharedUtility().GetCookie('lang');

        if (!lang)
            lang = 'en';

        let output = '';
        let result = uiControls.applabel.find(c => c.backEndName === 'lblPageName');

        if (result) {
            if (lang == "ar" && result.arValue) {
                output = result.arValue
            }
            else
                if (result.enValue)
                    output = result.enValue;
        }

        return output;
    }

    //===========================================================

    function init(uiControls) {

        const uiControlsSetupOptions = {
            appName: 'lblApplicationName',
            pageTitle: 'homePageTitle',
        };

        let lang = sharedUtility().GetCookie('lang');

        if (!lang)
            lang = 'en';

        if (uiControls) {

            //debugger
            if (uiControls) {
                if (uiControls.length > 0) {
                    populateUiControl(uiControls);

                    if (uiControlsSetupOptions) {
                        if (uiControlsSetupOptions.pageTitle && uiControlsSetupOptions.appName) {
                            $(document).prop('title', uiControlsSetup().GetUiControlText(uiControlsSetupOptions.pageTitle).concat(" | ", uiControlsSetup().GetUiControlText(uiControlsSetupOptions.appName)));
                        }
                    }

                    replaceLabelText(uiControls);
                }
            }
        }

        CheckAndResumeSessionTimer();
    }


    const replaceLabelText = (uiControlList, showLoader = false, hideElementWhileLoading = true) => {

        let lang = sharedUtility().GetCookie('lang');
        if (!lang)
            lang = 'en';

        if (uiControlList && uiControlList.length > 0) {
            if (hideElementWhileLoading)
                $.each(uiControlList, function (index, item) {
                    if (item.backEndName) {
                        $('.' + item.backEndName).each(function () {
                            $(this).hide();
                        });
                    }
                });

            if (showLoader) {
                sharedUtility().CoverSpin(true);
            }

            $.each(uiControlList, function (index, item) {

                if (item.backEndName && (item.enValue || item.arValue)) {
                    $('.' + item.backEndName).each(function () {
                        if (lang && lang == "ar") {

                            if (item.arValue) {
                                // Check if the object has a placeholder
                                let placeholder = $(this).attr("placeholder");

                                if (placeholder !== undefined) {
                                    $(this).attr("placeholder", item.arValue);
                                } else {
                                    $(this).text(item.arValue);
                                }
                            }

                        }
                        else
                            if (item.enValue) {

                                // Check if the object has a placeholder
                                let placeholder = $(this).attr("placeholder");

                                if (placeholder !== undefined) {
                                    $(this).attr("placeholder", item.enValue);
                                } else {
                                    $(this).text(item.enValue);
                                }
                            }
                    });
                }
            });

            if (hideElementWhileLoading)
                $.each(uiControlList, function (index, item) {
                    if (item.backEndName) {
                        $('.' + item.backEndName).each(function () {
                            $(this).show();
                        });
                    }
                });

            if (showLoader) {
                sharedUtility().CoverSpin(false);
            }
        }
    }

    const updatePageTitle = () => {
        let pageTitle = '';
        const pageTitleItem = uiControls.applabel.find(c => c.backEndName === 'pageTitle');

        let lang = sharedUtility().GetCookie('lang') || 'en';

        if (pageTitleItem) {
            pageTitle = lang === 'ar' ? pageTitleItem.arValue : pageTitleItem.enValue;
            $(document).prop('title', `${pageTitle} | ${uiControlsSetup().GetUiControlText('lblApplicationName')}`);
        } else {
            $(document).prop('title', uiControlsSetup().GetUiControlText('lblApplicationName'));
        }
    };


    let result = {};

    //===========================================================

    result.Init = init;
    result.GetUiControlText = getUiControlText;
    result.GetUiControl = getUiControl;
    result.GetPageNameText = getPageNameText;
    result.UpdatPageTitle = updatePageTitle;
    result.AnyUiBackendLabel = anyUiBackendLabel;
    result.UpdateUiControlText = replaceLabelText;
    result.PopulateUiControl = populateUiControl;

    return result;

}



