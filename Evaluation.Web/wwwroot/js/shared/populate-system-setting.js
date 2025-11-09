
const systemSettingsSetup = (systemSettingsSetupOptions) => {

    //===========================================================


    const init = (systemSettingsList) => {

        if (systemSettingsList && systemSettingsList.length > 0) {

            let listToBeAdded = [];
            $.each(systemSettingsList, function (index, item) {

                let exist = systemSetting.values.find(c => c.settingKey.toLowerCase() === item.key.toLowerCase());
                if (!exist) {
                    listToBeAdded.push(item);
                }
            });

            $.each(listToBeAdded, function (index, item) {
                systemSetting.values.push(item);
            });
        }
    }

    //===========================================================


    const getSystemSettingValue = (key) => {

        let value = null;

        if (key && systemSetting && systemSetting.values && systemSetting.values.length > 0) {

            let systemSettingItem = systemSetting.values.find(c => c.settingKey.toLowerCase() === key.toLowerCase());

            if (systemSettingItem && systemSettingItem.settingKey) {
                value = systemSettingItem.settingValue;
            }
        }

        return value;

    }

    //===========================================================


    let result = {};

    //===========================================================

    result.Init = init;
    result.GetSystemSettingValue = getSystemSettingValue;
    return result;

}



