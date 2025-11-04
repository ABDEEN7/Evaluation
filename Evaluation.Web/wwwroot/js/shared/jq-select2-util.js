const InitializeSelect2_Ajax = (config) => {
    const $select2Element = config.$element ?? (!config.dropdownParent ? $(`#${config.selectorId}`) : config.dropdownParent.find(`#${config.selectorId}`));
    if (config.data) {
        initSelect2(config.data);
    }
    else if (config.url) {
        const options = {
            success: initSelect2
        };
        if (config && config.payload !== undefined) {
            jqClient(options).Post(config.url, config.payload);
        } else {
            jqClient(options).Get(config.url);
        }
    }
    else {
        console.error('No data or URL provided for Select2 initialization.');
    }

    function initSelect2(result) {
        $select2Element.find('option').remove();
        const dataList = config.sourceResult ? config.sourceResult(result) : result;
        if (dataList) {
            if(!config.enableSelectAll) {
                $select2Element.select2({
                    ...config,
                    placeholder: config.placeholder || uiControlsSetup().GetUiControlText('lblPleaseSelect'),
                    width: config.width || '100%',
                    allowClear: config.allowClear !== undefined ? config.allowClear : false,
                    multiple: config.multiple !== undefined ? config.multiple : false,
                    dropdownParent: config.dropdownParent || null,
                    data: dataList.length === 0 ? [] : dataList.map(config.mapResult).sort((a, b) => a.text.localeCompare(b.text)),

                });
            }else {
                $select2Element.S2MasterCheckbox({
                    ...config,
                    placeholder: config.placeholder || uiControlsSetup().GetUiControlText('lblPleaseSelect'),
                    width: config.width || '100%',
                    allowClear: config.allowClear !== undefined ? config.allowClear : false,
                    multiple: config.multiple !== undefined ? config.multiple : false,
                    dropdownParent: config.dropdownParent || null,
                    data: dataList.length === 0 ? [] : dataList.map(config.mapResult).sort((a, b) => a.text.localeCompare(b.text)),

                    selectAll: true,
                    selectAllText: 'Select all (visible)',
                    unselectAllText: 'Unselect all (visible)',
                    keepOrder: true,
                    templateSelection: function(selected, total) {
                        if(!selected) return "";
                        return `${selected?.length > 1 ? selected.length + ' ' + sharedFn().GetUiControlText('lblSelected') : ''} ${selected?.text || selected?.name}`;
                    },
                });
            }

            // Disable the first option if a flag is set
            if (config.disableFirstOption && dataList.length > 0) {
                const firstId = config.mapResult(dataList[0]).id;
                $select2Element.find(`option[value="${firstId}"]`).attr('disabled', 'disabled');
            }

            if (config.selectedValue) {
                $select2Element.val(config.selectedValue).trigger('change');
            }
             else {
                 if (dataList.length === 1) {
                    const firstValue = config.mapResult(dataList[0]).id;
                    $select2Element.val(firstValue).trigger('change');
                } else {
                    if (config.selectedFirstValue && dataList.length > 0) {
                        const firstValue = config.mapResult(dataList[0]).id;
                        $select2Element.val(firstValue).trigger('change');
                    } else {
                        $select2Element.val("").trigger('change');
                    }
                }
            }
        }
    }
}
const InitializeSelect2_Local = (config) => {
    if (!config || !config.selectorId) {
        console.error('InitializeSelect2_Local: Missing config or selectorId.');
        return;
    }

    const $element = $(`#${config.selectorId}`);
    if ($element.length === 0) {
        console.error(`InitializeSelect2_Local: No element found with id '${config.selectorId}'.`);
        return;
    }


    // Prepare data
    let select2Data = Array.isArray(config.data) ? config.data : [];
    if (typeof config.mapResult === 'function') {
        select2Data = select2Data.map(config.mapResult);
    }
    $element.empty();
    $element.select2({
        width: '100%',
        allowClear: config.hasOwnProperty('allowClear') ? config.allowClear : false,
        multiple: config.hasOwnProperty('multiple') ? config.multiple : false,
        data: select2Data,
        placeholder: config.placeholder || uiControlsSetup().GetUiControlText('lblPleaseSelect'),
        dropdownParent: config.dropdownParent ? $(config.dropdownParent) : null,
    });

    if (!config.hasOwnProperty('selectedFirstValue') || !config.selectedFirstValue) {
        $element.val(null).trigger('change');
    }

    return $element;
};
