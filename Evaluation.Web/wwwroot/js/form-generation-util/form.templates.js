// form.templates.js

window.formUtility = window.formUtility || {};
const formUtility = window.formUtility;

(function (ns) {

    // ===================== Helpers =====================
    const GetUiControlText = (key) =>
        (uiControlsSetup && uiControlsSetup().GetUiControlText(key)) || "";

    const tryCoverSpin = (show) => {
        if (ns.coverSpin && typeof ns.coverSpin === "function") {
            ns.coverSpin(show);
        } else if (typeof window.coverSpin === "function") {
            window.coverSpin(show);
        }
    };

    const buildTemplateUrl = (templateId) => {
        const params = new URLSearchParams(window.location.search);
        const rawRequestId = params.get("id") || "";
        const requestId = rawRequestId.replace("#", "");

        let url = `/Templates/GetLetterDocument?requestId=${encodeURIComponent(requestId)}&templeteId=${encodeURIComponent(templateId)}`;

        try {
            if (typeof sharedUtility === "function" && sharedUtility().BaseApiUrl) {
                let baseApi = sharedUtility().BaseApiUrl() || "";
                baseApi = decodeURIComponent(baseApi).replace(/\/+$/, "");
                if (baseApi) {
                    url = baseApi + url;
                }
            }
        } catch {
          
        }

        return url;
    };

    const openBlobInModal = (blob, name) => {
        if (!blob) return;
        const url = URL.createObjectURL(blob);
        if (typeof OpenFileModal === "function") {
            OpenFileModal("modal-fullscreen", name, url, name);
        } else {
            window.open(url, "_blank");
        }
        setTimeout(() => URL.revokeObjectURL(url), 10000);
    };

    // ===================== Core: getTemplate / ViewTemplate =====================

    /**
     * @param {string} id - templateId
     * @param {string} name - template name 
     * @param {boolean} useAspose
     */
    const getTemplate = async (id, name, useAspose = false) => {
        if (!id) return;

        const _url = buildTemplateUrl(id);
        const jq = jqClient({});

        tryCoverSpin(true);

        try {
            const blob = await jq.GetBlob(_url);
            openBlobInModal(blob, name);
        } catch (error) {
            console.error("Error fetching template PDF:", error);
        } finally {
            tryCoverSpin(false);
        }
    };

   
    const ViewTemplate = async (id, name, useAspose = false) => {
        return getTemplate(id, name, useAspose);
    };

    // ===================== renderTemplateFirst (Tabulator Table) =====================

    /**
 
     * @param {Array} allTemplates - list of templates { id, name, ... }
     * @param {string} container - 
     * @param {string} tabulatorId - 
     * @param {boolean} ignoreEmpty - 
     */
    const renderTemplateFirst = (allTemplates, container, tabulatorId = "template-table", ignoreEmpty = false) => {
        const templates = allTemplates || [];

        if (ignoreEmpty && templates.length === 0) return;

        const $container = $(`#${container}`);
        if (!$container.length) {
            console.warn(`Container #${container} not found for templates rendering.`);
            return;
        }

        $container.empty();

        const accordionId = "accordionItemTemplate";
        const collapseId = "collapseTemplate";
        const headingId = "headingTemplate";

        const $accordionItem = $("<div>")
            .addClass("accordion-item")
            .attr("id", accordionId);

        const $accordionHeader = $("<h2>")
            .addClass("accordion-header")
            .attr("id", headingId)
            .appendTo($accordionItem);

        $("<button>")
            .addClass("accordion-button")
            .attr("type", "button")
            .attr("data-bs-toggle", "collapse")
            .attr("data-bs-target", `#${collapseId}`)
            .attr("aria-expanded", "true")
            .attr("aria-controls", collapseId)
            .html(`<span>${GetUiControlText("lblTemplates")}</span>`)
            .appendTo($accordionHeader);

        const $accordionCollapse = $("<div>")
            .addClass("accordion-collapse collapse show")
            .attr("id", collapseId)
            .attr("aria-labelledby", headingId)
            .appendTo($accordionItem);

        const $accordionBody = $("<div>")
            .addClass("accordion-body")
            .appendTo($accordionCollapse);

        const $wrapperDiv = $("<div>").addClass("tabulator-wrapper");
        const $tableDiv = $("<div>")
            .attr("id", tabulatorId)
            .addClass("tabulator-dark");

        $wrapperDiv.append($tableDiv).appendTo($accordionBody);
        $container.prepend($accordionItem);

        const templateNameFormatter = (cell) => {
            const cellValue = cell.getValue();
            const rowData = cell.getRow().getData();
            const templateId = rowData.id;

            return `<a class="btn btn-link p-0" href="javascript:void(0);" 
                        onclick="formUtility.getTemplate('${templateId}', '${cellValue.replace(/'/g, "\\'")}')">
                        ${cellValue}
                    </a>`;
        };

        const columns = [
            {
                title: GetUiControlText("lblTemplateName") || "Template",
                field: "name",
                formatter: templateNameFormatter
            },
            {
                title: GetUiControlText("lblActions") || "Actions",
                field: "id",
                formatter: function (cell) {
                    const row = cell.getRow().getData();
                    const tmplId = cell.getValue();
                    const tmplName = (row.name || "").replace(/'/g, "\\'");
                    const btnText = GetUiControlText("lblDownload") || "Download";

                    return `<button class="btn btn-primary btn-sm"
                                    onclick="event.preventDefault(); formUtility.getTemplate('${tmplId}', '${tmplName}')">
                                ${btnText}
                            </button>`;
                }
            }
        ];

        if (typeof Tabulator === "undefined") {
            console.error("Tabulator is not defined. Include Tabulator JS before using renderTemplateFirst.");
            return;
        }

        new Tabulator(`#${tabulatorId}`, {
            data: templates,
            layout: "fitColumns",
            columns: columns
        });
    };

    // ===================== Exports =====================
    ns.getTemplate = getTemplate;
    ns.ViewTemplate = ViewTemplate;
    ns.renderTemplateFirst = renderTemplateFirst;

})(formUtility);
