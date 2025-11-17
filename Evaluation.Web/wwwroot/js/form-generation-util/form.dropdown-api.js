
(function (global) {
    const { fetchJSON } = FormApi;

    const getDropDownValues = () => {
        const options = {
            success: function (data) {
                dropdowns = data;
            }
        };
        const url = "/ServiceRequest/GetDropDownValues";
        jqClient(options).Get(url);
    };

    const GetDropDownValuesByTypeId = (requestId, dropDownTypeId, parentDropDownId = null) => {
        return new Promise((resolve, reject) => {
            const scholarshipId = new URLSearchParams(window.location.search)
                .get("scholarshipId")?.replace("#", "");
            const url =
                `/ServiceRequest/GetDropDownValuesByTypeId?dropDownTypeId=${encodeURIComponent(dropDownTypeId)}`
                + `&requestId=${encodeURIComponent(requestId || '')}`
                + `&schId=${encodeURIComponent(scholarshipId || '')}`
                + `&parentDropDownId=${encodeURIComponent(parentDropDownId || '')}`;

            const options = {
                success: function (data) {
                    if (data && Array.isArray(data)) {
                        data.forEach(item => {
                            if (!dropdowns.some(existing => existing.id === item.id)) {
                                dropdowns.push(item);
                            }
                        });
                        resolve(data);
                    } else {
                        resolve([]);
                    }
                },
                error: function (err) {
                    reject(err);
                }
            };

            jqClient(options).Get(url);
        });
    };

    const GetDropDownValuesById = (requestId, dropDownTypeId, value) => {
        return new Promise((resolve, reject) => {
            const scholarshipId = new URLSearchParams(window.location.search)
                .get("scholarshipId")?.replace("#", "");
            const url =
                `/ServiceRequest/GetDropDownValuesById?dropDownTypeId=${encodeURIComponent(dropDownTypeId)}`
                + `&requestId=${encodeURIComponent(requestId || '')}`
                + `&schId=${encodeURIComponent(scholarshipId || '')}`
                + `&value=${encodeURIComponent(value || '')}`;

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
    };

  
    async function getCountry(reqId, enrollmentTypeId, $dropdown, selectedValue) {
        const url = `/ServiceRequest/GetCountry?reqId=${reqId}`
            + `&EnrollmentTypeId=${enrollmentTypeId}`;
        return await fetchJSON(url);
    }

  

    global.FormDropdownApi = {
        getDropDownValues,
        GetDropDownValuesByTypeId,
        GetDropDownValuesById,
        getCountry,
    };
})(window);
