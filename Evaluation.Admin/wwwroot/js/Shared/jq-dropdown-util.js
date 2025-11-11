const jqDropdownInitializer = (configs) => {

    let result = {};
    result.InitParents = initParents;
    result.InitChildren = initChildren;

    let dropdowns = configs.dropdowns;
    let lang = configs.lang;
    let url = configs.url;

    function ClearCascadingDropdowns(uibackendName) {

        let data = buildDropdownListWithDependencies();
        let list = getHierarchy(data, uibackendName);
        $.each(list, function (i, item) {
            $(`#${item}`).empty();

        });

        //debugger

    }

    function getHierarchy(data, uibackendName) {
        let result = [];

        // Recursive function to build the hierarchy of uibackendNames
        function buildHierarchy(item) {
            // Add current item uibackendName to the result
            result.push(item.uibackendName);

            // Recursively process children
            item.fireChangeOnList.forEach(child => buildHierarchy(child));
        }

        // Find the starting point (ScholarshipActionServiceId)
        const startItem = data.find(item => item.uibackendName === uibackendName);

        if (startItem) {
            // Start building the hierarchy from ScholarshipActionServiceId
            buildHierarchy(startItem);
        }

        result.shift();
        return result;
    }

    function initializeDropdownOnChangeEventHandler(uibackendName, relatedUibackendNameList,modalpopup=false) {

        if (relatedUibackendNameList.length > 0) {
            $(`#${uibackendName}`).change(function () {

                // //debugger
                let currentValues = $(this).val();


                //
                $.each(relatedUibackendNameList, function (i, item) {


                    const options = {
                        success: function (data) {
                            if (data) {

                                const data_select2 = data
                                    .map(item => ({
                                        id: item.id,
                                        text: lang == "ar" ? item.nameAr : item.nameEn
                                    }));

                                $(`#${item.uibackendName}`).empty();


                                let dropdown = dropdowns.find(c => c.uibackendName == item.uibackendName);
                                //debugger

                                if (dropdown) {

                                    if (dropdown.constraint.controlJsonConfig) {

                                        let controlJsonConfig = JSON.parse(dropdown.constraint.controlJsonConfig);
                                        let select2Config = controlJsonConfig.Select2Config;
                                        ////debugger
                                        if (select2Config) {
                                            select2Config.data = data_select2;
                                            if (modalpopup)
                                            select2Config.dropdownParent = $("#ModalPopup");
                                            select2Config.placeholder = sharedFn().GetUiControlText(select2Config.placeholder);
                                        } else {
                                            if (modalpopup) {
                                                select2Config = {
                                                    width: '100%',
                                                    allowClear: true,
                                                    multiple: false,
                                                    data: data_select2,
                                                    dropdownCssClass: "manageselect2zindex",
                                                    dropdownParent: $("#ModalPopup"),
                                                    placeholder: sharedFn().GetUiControlText('PleaseSelect'),
                                                };
                                            }
                                            else {
                                                select2Config = {
                                                    width: '100%',
                                                    allowClear: true,
                                                    multiple: false,
                                                    data: data_select2,
                                                    dropdownCssClass: "manageselect2zindex",
                                                    placeholder: sharedFn().GetUiControlText('PleaseSelect'),
                                                };
                                            }
                                        }

                                        $(`#${item.uibackendName}`).select2(select2Config);

                                       

                                        let selectedDataValue = $(`#${item.uibackendName}`).attr("data-value");

                                        if (selectedDataValue) {

                                            if (selectedDataValue.includes(',')) {
                                                let selectedValue = selectedDataValue.split(',');
                                                $(`#${item.uibackendName}`).val(selectedValue);
                                            } else {
                                                $(`#${item.uibackendName}`).val(selectedDataValue);
                                            }


                                        }
                                        else {
                                            $(`#${item.uibackendName}`).val('');
                                        }

                                        $(`#${item.uibackendName}`).trigger('change');

                                    }

                                }

                            }
                        }
                    };
                    if (currentValues) {
                        if (currentValues.length == 0) {
                            ClearCascadingDropdowns(uibackendName);
                            //debugger

                        } else {


                            let currentValue = '';
                            if (Array.isArray(currentValues)) {
                                currentValue = currentValues.join(',');
                            } else {
                                currentValue = currentValues;
                            }

                            ////debugger
                            var data = {
                                controlUibackendName: item.uibackendName,
                                parentReferenceValue: currentValue
                            };

                            jqClientAdvanced(options).Post(url, data);
                        }

                    }
                    else {
                        ////debugger
                        $(`#${item.uibackendName}`).empty();

                    }
                    

                });
            });
        }
    }

    function initParents(modalpopup =false) {



        //initialize parents dropdowns and make them empty 
        $.each(dropdowns, function (i, dropdown) {
            if (dropdown.constraint.controlJsonConfig) {
                let controlJsonConfig = JSON.parse(dropdown.constraint.controlJsonConfig);
                let uibackendName = dropdown.uibackendName;

                if (uibackendName && controlJsonConfig.ParentReferenceId == null) {

                   
                    const options = {
                        success: function (data) {
                            if (data) {

                                const data_select2 = data
                                    .map(item => ({
                                        id: item.id,
                                        text: lang == "ar" ? item.nameAr : item.nameEn
                                    }));


                                let select2Config = controlJsonConfig.Select2Config;
                                ////debugger
                                if (select2Config) {
                                    select2Config.data = data_select2;
                                    select2Config.placeholder = sharedFn().GetUiControlText(select2Config.placeholder);
                                    if (modalpopup)
                                    select2Config.dropdownParent = $("#ModalPopup");
                                } else {
                                    if (modalpopup) {
                                        select2Config = {
                                            width: '100%',
                                            allowClear: true,
                                            multiple: false,
                                            data: data_select2,
                                            dropdownCssClass: "manageselect2zindex",
                                            dropdownParent: $("#ModalPopup"),
                                            placeholder: sharedFn().GetUiControlText('PleaseSelect'),
                                        };
                                    }
                                    else {
                                        select2Config = {
                                            width: '100%',
                                            allowClear: true,
                                            multiple: false,
                                            data: data_select2,
                                            dropdownCssClass: "manageselect2zindex",
                                            placeholder: sharedFn().GetUiControlText('PleaseSelect'),
                                        };
                                    }
                                   
                                }
                                $(`#${uibackendName}`).empty();

                                $(`#${uibackendName}`).select2(select2Config);

                                $(`#${uibackendName}`).val('').trigger('change');
                                if (window.hasOwnProperty("SetDropDownContraint")) {
                                    SetDropDownContraint();
                                }
                                let selectedDataValue = $(`#${uibackendName}`).attr("data-value");

                                if (selectedDataValue) {

                                    if (selectedDataValue.includes(',')) {
                                        let selectedValue = selectedDataValue.split(',');
                                        $(`#${uibackendName}`).val(selectedValue);
                                    } else {
                                        $(`#${uibackendName}`).val(selectedDataValue);
                                    }
                                    $(`#${uibackendName}`).trigger('change');

                                }
                            }
                        }
                    };


                    ////debugger
                    var data = {
                        controlUibackendName: uibackendName,
                        parentReferenceValue: null
                    };

                    jqClientAdvanced(options).Post(url, data);


                    //$(`#${uibackendName}`).empty();

                    //$(`#${uibackendName}`).select2({
                    //    width: '100%',
                    //    allowClear: true,
                    //    multiple: true,
                    //    data: [],
                    //    dropdownCssClass: "manageselect2zindex",
                    //    placeholder: sharedFn().GetUiControlText('PleaseSelect'),

                    //});

                    //$(`#${uibackendName}`).val(null);
                    //$(`#${uibackendName}`).trigger('change');


                }

            }
        });
    }

    function initChildren(modalpopup = false) {


        //dropdowns = dropdowns.filter(c => c.constraint.controlJsonConfig);


        //initialize dropdowns and make them empty except who have controlJsonConfig.ParentReferenceId not null
        $.each(dropdowns, function (i, dropdown) {
            if (dropdown.constraint.controlJsonConfig) {
                let controlJsonConfig = JSON.parse(dropdown.constraint.controlJsonConfig);
                let uibackendName = dropdown.uibackendName;

                if (uibackendName && controlJsonConfig.ParentReferenceId) {
                    let select2Config = controlJsonConfig.Select2Config;
                    ////debugger
                    if (select2Config) {
                        select2Config.data = [];
                        select2Config.placeholder = sharedFn().GetUiControlText(select2Config.placeholder);
                        if (modalpopup)
                        select2Config.dropdownParent = $("#ModalPopup");
                    } else {
                        if (modalpopup) {
                            select2Config = {
                                width: '100%',
                                allowClear: true,
                                multiple: false,
                                data: [],
                                dropdownCssClass: "manageselect2zindex",
                                dropdownParent: $("#ModalPopup"),
                                placeholder: sharedFn().GetUiControlText('PleaseSelect'),
                            };
                        }
                        else {
                            select2Config = {
                                width: '100%',
                                allowClear: true,
                                multiple: false,
                                data: [],
                                dropdownCssClass: "manageselect2zindex",
                                placeholder: sharedFn().GetUiControlText('PleaseSelect'),
                            };
                        }
                        
                    }

                    $(`#${uibackendName}`).empty();

                    $(`#${uibackendName}`).select2(select2Config);

                    $(`#${uibackendName}`).val('')
                    $(`#${uibackendName}`).trigger('change');


                }

            }
        });


        let list = buildDropdownListWithDependencies();

        $.each(list, function (i, item) {
            initializeDropdownOnChangeEventHandler(item.uibackendName, item.fireChangeOnList, modalpopup);
        });

    }

    function buildDropdownListWithDependencies() {
        let dropdownList = [];

        // Populate dropdownList with necessary data
        $.each(dropdowns, function (index, dropdown) {
            if (dropdown.constraint.controlJsonConfig) {
                dropdownList.push({
                    uibackendName: dropdown.uibackendName,
                    parentControlName: dropdown.constraint.controlJsonConfig ? JSON.parse(dropdown.constraint.controlJsonConfig).ParentControlName : {},
                    fireChangeOnList: []
                });
            }
        });

        // Link fireChangeOnList based on parentControlName
        $.each(dropdownList, function (index, item) {
            let dependentItems = dropdownList.filter(c => c.parentControlName == item.uibackendName);
            $.each(dependentItems, function (index, dependentItem) {
                item.fireChangeOnList.push(dependentItem);
            });
        });

        return dropdownList;
    }



    return result;

}
