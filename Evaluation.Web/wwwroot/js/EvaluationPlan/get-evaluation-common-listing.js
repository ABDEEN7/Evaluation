window.getevaluationListing = window.getevaluationListing || {};

(function (ns) {

    const DEFAULT_PAGE_SIZE = 18;

    function createListing(config) {

        let datatable = null;
        let isCardView = true;

        const $table = $('#' + config.tableId);
        const $cardBtn = $('#' + config.cardViewBtnId);
        const $tblBtn = $('#' + config.tableViewBtnId);
        const $clearBtn = $('#' + (config.clearFilterBtnId || ''));
        const $filterBtn = $('#' + (config.filterBtnId || ''));

        /* =========================
         * VIEW MODE
         * ========================= */
        function applyViewMode() {
            if (!config.enableCardView) return;

            const $thead = $table.find('thead');
            const $wrapper = $('#' + config.tableId + '_wrapper');

            let $responsive = $table.closest('.table-responsive');
            if (!$responsive.length) {
                $table.wrap('<div class="table-responsive"></div>');
                $responsive = $table.closest('.table-responsive');
            }

            if (isCardView) {
                $table.removeClass('table table-bordered table-hover w-100').addClass('card');
                $table.find('tr').addClass('card');
                $thead.hide();

                $cardBtn.addClass('active');
                $tblBtn.removeClass('active');

                $responsive.removeClass('table-responsive');
            } else {
                $table.removeClass('card').addClass('table table-bordered table-hover w-100');
                $table.find('tr').removeClass('card');
                $thead.show();

                $tblBtn.addClass('active');
                $cardBtn.removeClass('active');

                $responsive.addClass('table-responsive');
                $wrapper.removeClass('table-responsive');
            }
        }

        /* =========================
         * BUILD QUERY OBJECT
         * ========================= */
        function buildQuery(dt) {

            const baseFilter =
                typeof config.getFilterInput === 'function'
                    ? (config.getFilterInput() || {})
                    : {};

            const size = dt.length || DEFAULT_PAGE_SIZE;
            const page = Math.floor((dt.start || 0) / size) + 1;

            return {
                ...baseFilter,
                PageNumber: page,
                PageSize: size,
                Draw: dt.draw
            };
        }

        /* =========================
         * INIT DATATABLE
         * ========================= */
        function initTable() {

            if ($.fn.dataTable.isDataTable($table)) {
                datatable = $table.DataTable();
                datatable.ajax.reload(null, false);
                return;
            }

            datatable = $table.DataTable({
                processing: true,
                serverSide: true,
                paging: true,
                searching: false,
                lengthChange: false,
                pageLength: config.pageSize || DEFAULT_PAGE_SIZE,
                autoWidth: false,
                deferRender: true,
                dom: 'ftipr',
                order: config.order || [],

                ajax: function (dt, callback) {

                    const queryObject = buildQuery(dt);
                    const queryString = $.param(queryObject);

                    jqClient({
                        success: function (resp) {

                            const total =
                                resp.totalDataCount ||
                                resp.TotalDataCount ||
                                resp.totalCount ||
                                0;

                            const rows =
                                resp.data ||
                                resp.Data ||
                                resp.items ||
                                [];

                            if (config.tabLabelSelector && config.tabLabelKey) {
                                const lbl = uiControlsSetup().GetUiControlText(config.tabLabelKey);
                                $(config.tabLabelSelector).html(
                                    total ? `${lbl} (${total})` : lbl
                                );
                            }

                            callback({
                                draw: dt.draw,
                                recordsTotal: total,
                                recordsFiltered: total,
                                data: rows
                            });
                        },
                        error: function () {
                            callback({
                                draw: dt.draw,
                                recordsTotal: 0,
                                recordsFiltered: 0,
                                data: []
                            });
                        }
                    }).Get(`${config.ajaxUrl}?${queryString}`);
                },

                columns: config.columns,

                createdRow: function (row, data) {
                    if (config.rowClass) $(row).addClass(config.rowClass);
                    if (config.enableCardView && isCardView) $(row).addClass('card');
                    if (typeof config.onCreatedRow === 'function') {
                        config.onCreatedRow(row, data);
                    }
                },

                drawCallback: function () {
                    applyViewMode();

                    if (typeof config.onDraw === 'function') {
                        config.onDraw(datatable);
                    }

                    $table.find('tbody')
                        .off('click', 'tr')
                        .on('click', 'tr', function (e) {
                            if ($(e.target).closest('.dropdown').length) return;
                            const rowData = datatable.row(this).data();
                            if (rowData && typeof config.onRowClick === 'function') {
                                config.onRowClick(rowData, e);
                            }
                        });
                }
            });
        }

        /* =========================
         * PUBLIC METHODS
         * ========================= */
        function reload() {
            if (datatable) {
                datatable.ajax.reload(null, false);
            } else {
                initTable();
            }
        }

        function clearFilter() {
            if (!config.filterFormId) return;

            const $form = $('#' + config.filterFormId);
            $form.find('input').val('');
            $form.find('select').val(null).trigger('change');

            if (typeof config.onClear === 'function') {
                config.onClear();
            }

            reload();
        }

        /* =========================
         * EVENTS
         * ========================= */
        if (config.enableCardView) {
            $cardBtn.on('click', e => {
                e.preventDefault();
                isCardView = true;
                applyViewMode();
            });

            $tblBtn.on('click', e => {
                e.preventDefault();
                isCardView = false;
                applyViewMode();
            });
        }

        if ($filterBtn.length) {
            $filterBtn.on('click', e => {
                e.preventDefault();
                reload();
            });
        }

        if ($clearBtn.length) {
            $clearBtn.on('click', e => {
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

        initTable();

        return {
            reload,
            clearFilter,
            applyViewMode,
            getDataTable: () => datatable
        };
    }

    ns.createListing = createListing;

})(window.getevaluationListing);
