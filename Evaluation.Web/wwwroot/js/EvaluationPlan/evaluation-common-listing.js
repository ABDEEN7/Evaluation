window.evaluationListing = window.evaluationListing || {};

(function (ns) {
    const defaultPageSize = 18;

    function createListing(config) {
        let datatable = null;
        let isCardView = true;

        const tableSelector = '#' + config.tableId;
        const $table = $(tableSelector);
        const $cardBtn = $('#' + config.cardViewBtnId);
        const $tblBtn = $('#' + config.tableViewBtnId);
        const $clearBtn = $('#' + (config.clearFilterBtnId || ''));
        const $filterBtn = $('#' + (config.filterBtnId || ''));

        function applyViewMode() {
            if (!config.enableCardView) return;

            const wrapperSelector = '#' + config.tableId + '_wrapper';

            if (isCardView) {
                $table.addClass('card');
                $table.find('tr').addClass('card');
                $cardBtn.addClass('active');
                $tblBtn.removeClass('active');
                $(wrapperSelector).removeClass('table-responsive');
            } else {
                $table.removeClass('card');
                $table.find('tr').removeClass('card');
                $tblBtn.addClass('active');
                $cardBtn.removeClass('active');
                $(wrapperSelector).addClass('table-responsive');
            }
        }


        function buildPayload(dt) {
            const baseFilter = typeof config.getFilterInput === 'function'
                ? (config.getFilterInput() || {})
                : {};

            const size = dt.length || defaultPageSize;
            const page = Math.floor((dt.start || 0) / size) + 1;

            return Object.assign({}, baseFilter, {
                PageNumber: page,
                PageSize: size,
                Draw: dt.draw
            });
        }

        function initTable() {
            // استخدم dataTable (حروف صغيرة)
            if ($.fn.dataTable && $.fn.dataTable.isDataTable($table)) {
                datatable = $table.DataTable();
                datatable.page('first').draw(false);
                datatable.ajax.reload(null, false);
                return;
            }

            datatable = $table.DataTable({
                processing: true,
                serverSide: true,
                paging: true,
                pageLength: config.pageSize || defaultPageSize,
                lengthChange: false,
                searching: false,
                autoWidth: false,
                deferRender: true,
                dom: 'ftipr',
                order: config.order || [],
                language: {
                    info: uiControlsSetup().GetUiControlText("lblShowingEntries"),
                    paginate: {
                        previous: `${uiControlsSetup().GetUiControlText("lblprevious")} <i class="fas fa-angle-left"></i>`,
                        next: `${uiControlsSetup().GetUiControlText("lblnext")} <i class="fas fa-angle-right"></i>`
                    }
                },
                ajax: function (dt, callback) {
                    const payload = buildPayload(dt);
                    const options = {
                        success: function (resp) {
                            const total = resp.totalDataCount || resp.TotalDataCount || 0;
                            const rows = resp.data || resp.Data || [];

                            if (config.tabLabelSelector && config.tabLabelKey) {
                                const lbl = uiControlsSetup().GetUiControlText(config.tabLabelKey);
                                $(config.tabLabelSelector).html(total ? `${lbl} (${total})` : lbl);
                            }

                            callback({
                                draw: dt.draw,
                                recordsTotal: total,
                                recordsFiltered: total,
                                data: rows
                            });
                        },
                        error: function () {
                            callback({ draw: dt.draw, recordsTotal: 0, recordsFiltered: 0, data: [] });
                        }
                    };

                    jqClient(options).Post(config.ajaxUrl, payload);
                },
                columns: config.columns,
                createdRow: function (row, data) {
                    if (config.rowClass) $(row).addClass(config.rowClass);
                    if (config.enableCardView && $table.hasClass("card")) $(row).addClass('card');
                    if (typeof config.onCreatedRow === 'function') config.onCreatedRow(row, data);
                },
                drawCallback: function () {
                    applyViewMode();

                    if (typeof config.onDraw === 'function') {
                        config.onDraw(datatable);
                    }

                    $table.find('tbody')
                        .off('click', 'tr')
                        .on('click', 'tr', function (event) {
                            if ($(event.target).closest('.dropdown').length) return;
                            const rowData = datatable.row(this).data();
                            if (rowData && typeof config.onRowClick === 'function') {
                                config.onRowClick(rowData, event);
                            }
                        });
                }
            });
        }

        function reload() {
            const $tbl = $('#' + config.tableId);

            // حماية لو الـ plugin مش محمّل
            if (!$.fn.dataTable || !$.fn.dataTable.isDataTable) {
                initTable();
                return;
            }

            if ($.fn.dataTable.isDataTable($tbl)) {
                if (datatable) {
                    datatable.ajax.reload(null, false);
                } else {
                    $tbl.DataTable().ajax.reload(null, false);
                }
            } else {
                initTable();
            }
        }

        function clearFilter() {
            if (!config.filterFormId) return;
            const $form = $('#' + config.filterFormId);

            $form.find('input').val('');
            $form.find('select').each(function () {
                $(this).val(null).trigger('change');
            });

            if (typeof config.onClear === 'function') config.onClear();
            reload();
        }

        if (config.enableCardView) {
            $cardBtn.on('click', function (e) {
                e.preventDefault();
                isCardView = true;
                applyViewMode();
            });

            $tblBtn.on('click', function (e) {
                e.preventDefault();
                isCardView = false;
                applyViewMode();
            });
        }

        if ($filterBtn.length) {
            $filterBtn.on('click', function (e) {
                e.preventDefault();
                reload();
            });
        }

        if ($clearBtn.length) {
            $clearBtn.on('click', function (e) {
                e.preventDefault();
                clearFilter();
            });
        }

        if (config.filterFormId) {
            $('#' + config.filterFormId)
                .find('.filter-enter')
                .on('keypress', function (e) {
                    if (e.keyCode === 13) {
                        e.preventDefault();
                        reload();
                    }
                });
        }

        return {
            reload,
            clearFilter,
            applyViewMode,
            getDataTable: () => datatable
        };
    }

    ns.createListing = createListing;
})(window.evaluationListing);
