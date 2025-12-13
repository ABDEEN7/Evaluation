

window.formUtility = window.formUtility || {};
const formUtility = window.formUtility;

(function (ns) {

    // ================================
    //  SORT FIELDS
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
    // 🔹 BUILD TREE STRUCTURE
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
    // 🔹 Disable Chrome autofill in Select2
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
    // Full Trigger Logic
    // ================================
    ns.triggerChangesForDropdowns = function triggerChangesForDropdowns(cascadingData) {

        function triggerField(fieldItem) {

            const id = `field_${fieldItem.id}`;
            const $field = $("#" + id);

            if (!$field.length) return;

            disableAutofillOnSelect2($field);

            $field.on("change.__cascade", async function () {

                const value = $(this).val();

                if (fieldItem.children?.length) {

                    for (const child of fieldItem.children) {

                        const childId = `field_${child.id}`;
                        const $child = $("#" + childId);

                        if ($child.length === 0) continue;

                        $child.val(null).trigger("change");

                        if (!child.dropDownTypeId) continue;

                        const requestId = new URLSearchParams(window.location.search).get("id")?.replace("#", "");
                        const scholarshipId = new URLSearchParams(window.location.search)
                            .get("schId")?.replace("#", "");

                        const url = `/FormRender/GetDropDownValuesByTypeId` +
                            `?dropDownTypeId=${encodeURIComponent(child.dropDownTypeId)}` +
                            `&requestId=${encodeURIComponent(requestId || '')}` +
                            `&schId=${encodeURIComponent(scholarshipId || '')}` +
                            `&parentDropDownId=${encodeURIComponent(value || '')}`;

                        const childValues = await formUtility.fetchJSON(url);

                        $child.empty();
                        $child.append(`<option value="">${window.currentLang === "ar" ? "اختر" : "Select"}</option>`);

                        if (childValues?.length) {
                            childValues.forEach(item => {
                                $child.append(
                                    `<option value="${item.id}">${item.nameAr || item.nameEn}</option>`
                                );
                            });
                        }

                        $child.trigger("change");
                    }
                }
            });

            // Trigger default load
            $field.trigger("change.__cascade");
        }

        cascadingData.forEach(parent => {
            triggerField(parent);

            if (parent.children?.length) {
                parent.children.forEach(child => triggerField(child));
            }
        });

    };

    // ================================
    // 🔥 Initialize Cascading Dropdown
    // ================================
    function InitializeCascadingDropdown(formGroupDTOs) {

        if (formGroupDTOs && !Array.isArray(formGroupDTOs)) {
            formGroupDTOs = [formGroupDTOs];
        }

        let allDropdownFields = formGroupDTOs
            .flatMap(x => x.fields)
            .filter(f => f.type === "select2" || f.type === "dropdown" || f.type === "VacancySeat")
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
