window.formUtility = window.formUtility || {};
var formUtility = window.formUtility;

(function (ns) {

    // ================================
    // SORT FIELDS
    // ================================
    function sortDropdownFields(data) {
        let order = 1;
        const parents = data.filter(c => c.parentFieldId == null);

        function assignOrder(parent, tempOrder) {
            parent.order = tempOrder++;
            const children = data.filter(c => c.parentFieldId === parent.id);
            children.forEach(child => assignOrder(child, tempOrder));
        }

        parents.forEach(parent => assignOrder(parent, order));
        return data;
    }

    // ================================
    // BUILD TREE STRUCTURE
    // ================================
    function buildCascadingStructureForDropDownFields(data) {
        const parents = data.filter(c => c.parentFieldId == null);

        function build(parent) {
            const children = data.filter(c => c.parentFieldId === parent.id);
            parent.children = children || [];
            parent.children.forEach(child => build(child));
        }

        const result = [];
        parents.forEach(parent => {
            build(parent);
            result.push(parent);
        });

        return result;
    }

    // ================================
    // Disable Chrome autofill in Select2
    // ================================
    function disableAutofillOnSelect2($select) {
        $select.attr({
            name: "disabled_" + Math.random().toString(36).substring(2),
            autocomplete: "off"
        });

        if ($select.data("orig-name") === undefined) {
            const origName = $select.attr("data-name") || $select.attr("name") || ("f_" + Date.now());
            $select.data("orig-name", origName);

            const $fake = $('<input type="text" class="auto-fill" ' +
                'tabindex="-1" autocomplete="username" name="' + origName + '">');
            $select.before($fake);
        }
    }

    $("form").on("submit", function () {
        $(this).find("select").each(function () {
            const $s = $(this);
            const orig = $s.data("original-name");
            if (orig) $s.attr("name", orig);
        });
    });

    // ================================
    // HELPERS
    // ================================
    function getRequestParams() {
        const params = new URLSearchParams(window.location.search);
        return {
            requestId: params.get("id")?.replace("#", "") || '',
            EvlReqId: params.get("schId")?.replace("#", "") || ''
        };
    }

    // 🔹 Fetch dropdown values by type (and optional parent value)
    function fetchDropdownValuesFromApi(dropDownTypeId, parentValue) {
        return new Promise((resolve, reject) => {
            const { requestId, EvlReqId } = getRequestParams();

            const url = `/FormRender/GetDropDownValuesByTypeId` +
                `?dropDownTypeId=${encodeURIComponent(dropDownTypeId)}` +
                `&requestId=${encodeURIComponent(requestId)}` +
                `&schId=${encodeURIComponent(EvlReqId)}` +
                `&parentDropDownId=${encodeURIComponent(parentValue || '')}`;

            const options = {
                success: function (data) {
                    resolve(Array.isArray(data) ? data : []);
                },
                error: function (err) {
                    reject(err);
                }
            };

            jqClient(options).Get(url);
        });
    }

    // 🔹 Fetch single value (for disabled/readonly fields)
    function GetDropDownValuesById(requestId, dropDownTypeId, value) {
        return new Promise((resolve, reject) => {
            const EvlReqId = new URLSearchParams(window.location.search)
                .get("EvlReqId")?.replace("#", "");

            const url = `/FormRender/GetDropDownValuesById` +
                `?dropDownTypeId=${encodeURIComponent(dropDownTypeId)}` +
                `&requestId=${encodeURIComponent(requestId || '')}` +
                `&schId=${encodeURIComponent(EvlReqId || '')}` +
                `&value=${encodeURIComponent(value || '')}`;

            const options = {
                success: function (data) {
                    resolve(Array.isArray(data) ? data : []);
                },
                error: function (err) {
                    reject(err);
                }
            };

            jqClient(options).Get(url);
        });
    }

    function getValuesFromGlobalList(dropDownTypeId, parentValue) {
        const list = dropdowns || [];

        if (parentValue) {
            return list.filter(c =>
                c.dropDownTypeId === dropDownTypeId &&
                c.parentDropDownId === parentValue
            );
        }
        return list.filter(c => c.dropDownTypeId === dropDownTypeId);
    }

    function populateDropdown($field, values, selectedValue) {
        $field.empty();
        $field.append(`<option value="">${window.currentLang === "ar" ? "اختر" : "Select"}</option>`);

        if (values?.length) {
            values.forEach(item => {
                $field.append(
                    `<option value="${item.id}">${item.titleAr || item.titleEn}</option>`
                );
            });
        }

        if (selectedValue) {
            $field.val(selectedValue);
        }

        $field.trigger("change");
    }

    // ================================
    // POPULATE AND CONTINUE
    // ================================
    async function populateAndContinue($field, values, fieldItem) {
        populateDropdown($field, values, fieldItem.value);

        if (fieldItem.children?.length) {
            const currentValue = $field.val();

            for (const child of fieldItem.children) {
                await loadFieldData(child, currentValue);
            }
        }
    }

    // ================================
    // LOAD FIELD DATA (one field)
    // ================================
    async function loadFieldData(fieldItem, parentValue) {
        const $field = $(`#field_${fieldItem.id}`);
        if (!$field.length) return;

        $field.empty().val(null);

        if (!fieldItem.dropDownTypeId) {
            await populateAndContinue($field, [], fieldItem);
            return;
        }

        let values;

        if ($field.is('[disabled]')) {
            const { requestId } = getRequestParams();
            values = await GetDropDownValuesById(requestId, fieldItem.dropDownTypeId, fieldItem.value);
        } else {
            values = getValuesFromGlobalList(fieldItem.dropDownTypeId, parentValue);

            if (!values || values.length === 0) {
                values = await fetchDropdownValuesFromApi(fieldItem.dropDownTypeId, parentValue);
            }
        }

        await populateAndContinue($field, values || [], fieldItem);
    }

    // ================================
    // Full Trigger Logic
    // ================================
    ns.triggerChangesForDropdowns = function triggerChangesForDropdowns(cascadingData) {

        function bindChangeHandler(fieldItem) {
            const $field = $(`#field_${fieldItem.id}`);
            if (!$field.length) return;

            disableAutofillOnSelect2($field);

            $field.off("change.__cascade").on("change.__cascade", async function () {
                const value = $(this).val();

                if (!fieldItem.children?.length) return;

                for (const child of fieldItem.children) {
                    const $child = $(`#field_${child.id}`);
                    if (!$child.length) continue;

                    $child.val(null).trigger("change");

                    if (!child.dropDownTypeId) continue;

                    let childValues;

                    // Disabled → fetch by id
                    if ($child.is('[disabled]')) {
                        const { requestId } = getRequestParams();
                        childValues = await GetDropDownValuesById(requestId, child.dropDownTypeId, child.value);
                    } else {
                        // Try global list, then API
                        childValues = getValuesFromGlobalList(child.dropDownTypeId, value);
                        if (!childValues || childValues.length === 0) {
                            childValues = await fetchDropdownValuesFromApi(child.dropDownTypeId, value);
                        }
                    }

                    populateDropdown($child, childValues, child.value || null);
                }
            });
        }

        // Bind change handlers for all fields recursively
        function bindAll(fields) {
            fields.forEach(f => {
                bindChangeHandler(f);
                if (f.children?.length) bindAll(f.children);
            });
        }
        bindAll(cascadingData);

        // Initial load: start from each root and cascade down
        cascadingData.forEach(parent => {
            loadFieldData(parent, '');
        });
    };

    // ================================
    // Initialize Cascading Dropdown
    // ================================
    function InitializeCascadingDropdown(formGroupDTOs) {

        if (formGroupDTOs && !Array.isArray(formGroupDTOs)) {
            formGroupDTOs = [formGroupDTOs];
        }

        let allDropdownFields = formGroupDTOs
            .flatMap(x => x.fields)
            .filter(f => f.type === "select2" || f.type === "dropdown")
            .map(f => ({
                fieldId: f.fieldId,
                value: f.value,
                id: f.fieldId.indexOf('_') !== -1 ? f.fieldId.split('_')[1] : f.fieldId,
                parentFieldId: f.dropDownParentFieldId,
                dropDownTypeId: f.dropDownTypeId,
                order: 1
            }));

        if (allDropdownFields.length === 0) return;

        const sorted = sortDropdownFields(allDropdownFields);
        const cascadingStructure = buildCascadingStructureForDropDownFields(sorted);

        if (cascadingStructure.length > 0) {
            ns.triggerChangesForDropdowns(cascadingStructure);
        }
    }

    // ================================
    // EXPORT
    // ================================
    ns.sortDropdownFields = sortDropdownFields;
    ns.buildCascadingStructureForDropDownFields = buildCascadingStructureForDropDownFields;
    ns.disableAutofillOnSelect2 = disableAutofillOnSelect2;
    ns.InitializeCascadingDropdown = InitializeCascadingDropdown;

})(formUtility);