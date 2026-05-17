(function (ns) {
    "use strict";

    let table = null;

    function extractOpenRequests(parties) {

        let result = [];

        (parties || []).forEach(party => {

            if (party.isSupportFiles) return;

            (party.services || []).forEach(service => {

                (service.requests || []).forEach(request => {

                    //if (request.statusISOPen === true) {

                        result.push({
                            id: request.id,
                            requestNumber: request.requestNumber,
                            serviceName: request.service || service.nameEn,
                            status: request.status,
                            createBy: request.createBy,
                            createDate: request.createDate
                        });

                    //}

                });

            });

        });

        return result;
    }

    function initTable(data) {

        if (table) {
            table.replaceData(data);
            return;
        }

        table = new Tabulator("#allOpenRequestsTable", {
            data: data,
            layout: "fitColumns",
            pagination: true,
            paginationSize: 10,
            placeholder: "لا توجد طلبات مفتوحة",

            columns: [
                {
                    title: uiControlsSetup().GetUiControlText("lblRequestNo"),
                    field: "requestNumber",
                    formatter: function (cell) {
                        const row = cell.getRow().getData();

                        return `
                <a href="javascript:void(0)" 
                   class="fw-bold text-primary">
                    ${row.requestNumber || ""}
                </a>`;
                    },
                    cellClick: function (e, cell) {
                        const row = cell.getRow().getData();
                        window.openRequestDetails(row.id);
                    }
                },
                {
                    title: uiControlsSetup().GetUiControlText("lblRequestService"),
                    field: "serviceName"
                },
                {
                    title: uiControlsSetup().GetUiControlText("lblRequestStatus"),
                    field: "status"
                },
                {
                    title: uiControlsSetup().GetUiControlText("lblCreatedBy"),
                    field: "createBy"
                },
                {
                    title: uiControlsSetup().GetUiControlText("lblVisitDate") || "Visit Date",
                    field: "visitDate",
                    formatter: function (cell) {
                        const row = cell.getRow().getData();

                        return formatVisitDate(
                            row.visitFrom,
                            row.visitTo,
                            row.createDate
                        );
                    }
                }
            ]
        });
    }

    function formatVisitDate(from, to, createDate) {

        if (!from && !to) {
            return formatDate(createDate);
        }

        if (from && !to) {
            return formatDate(from);
        }

        if (!from && to) {
            return formatDate(to);
        }
        const f = formatDate(from);
        const t = formatDate(to);

        if (f === t) {
            return f;
        }

        return `${f} - ${t}`;
    }
    function bindSearch() {

        $(document)
            .off("keyup", "#allOpenRequestsSearch")
            .on("keyup", "#allOpenRequestsSearch", function () {

                const value = $(this).val();

                if (!table) return;

                if (!value) {
                    table.clearFilter();
                    return;
                }

                table.setFilter([
                    [
                        { field: "requestNumber", type: "like", value: value },
                        { field: "serviceName", type: "like", value: value },
                        { field: "status", type: "like", value: value },
                        { field: "createBy", type: "like", value: value }
                    ]
                ]);
            });
    }
    function formatDate(value) {
        if (!value) return "-";

        const cleanValue = value.toString().split(".")[0];
        const dateObj = new Date(cleanValue);

        if (isNaN(dateObj.getTime())) return value;

        const day = String(dateObj.getDate()).padStart(2, "0");
        const month = String(dateObj.getMonth() + 1).padStart(2, "0");
        const year = dateObj.getFullYear();

        return `${day}/${month}/${year}`;
    }
    function render(parties) {

        const data = extractOpenRequests(parties);

        const accordion = document.querySelector('[data-bs-target="#itemAllOpenRequests"]')
            ?.closest('.accordion-item');

        if (accordion) {
            accordion.style.display = data.length === 0 ? "none" : "block";
        }

        if (data.length === 0) return;

        initTable(data);
        bindSearch();
    }

    ns.openRequestsModule = {
        render
    };

})(window);