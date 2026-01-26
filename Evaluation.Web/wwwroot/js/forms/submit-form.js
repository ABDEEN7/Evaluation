//let departmentRoutePath = sharedUtility().extractDepartmentName();

$(document).ready(function () {
    $("#btnSubmitForm").on("click", function (e) {
        e.preventDefault();
        submitForm();
    });
});

$(document).ready(function () {
    $("#btnSaveForm").on("click", function (e) {
        e.preventDefault();
        saveForm();
    });
});
function evaluationFormResult() {
    const params = new URLSearchParams(window.location.search);
    const formId = params.get('formId');  
    const mainItems = [];

    // ========== LOOP MAIN ITEMS ONLY ==========
    $("#tbodyRows tr.main-row").each(function () {

        const row = $(this);
        const select = row.find("select.eval-select");

        const mainId = select.data("id");

        const selectedValue =
            select.find("option:selected").data("id") ||
            select.val() ||
            null;

        const note =
            row.find("textarea.note-input").val() || null;

        const mainObj = {
            id: mainId,
            value: selectedValue,
            note: note,
            subItems: []
        };

        // ========== LOOP SUB ITEMS RELATED TO THIS MAIN ==========
        $(`tr.child-row[data-parent-id="${mainId}"]`).each(function () {

            const childRow = $(this);
            const childSelect = childRow.find("select.eval-select");

            const childId = childSelect.data("id");

            const childValue =
                childSelect.find("option:selected").data("id") ||
                childSelect.val() ||
                null;

            const childnote =
                childRow.find("textarea.note-input").val() || null;

            mainObj.subItems.push({
                id: childId,
                value: childValue,
                note: childnote
            });
        });

        mainItems.push(mainObj);
    });

    // Strengths & Improvements
    const strengths = $("textarea[data-row='1']").eq(0).val() || null;
    const improvements = $("textarea[data-row='1']").eq(1).val() || null;

    const payload = {
        id: formId,
        items: mainItems,
        strengths: strengths,
        improvements: improvements
    };

    console.log("FINAL NESTED JSON:", payload);

    return payload;
}

function submitForm() {
    var result = evaluationFormResult();
    jqClient().Post(`/Form/${departmentRoutePath}/SaveEvaluationForm`, result)
        .done((res) => {
            Swal.fire({
                icon: "success",
                title: "تم الإرسال",
                text: "تم إرسال الاستمارة للاعتماد بنجاح"
            });
        });
}

function saveForm() {
  return evaluationFormResult();
}