class TreeWithTabulator {
    constructor(treeConfig = {}, tabulatorConfig = {}) {


        // Ensure treeConfig has valid defaults
        this.treeConfig = {
            container: null,
            data: [],
            multipleSelection: false,
           
            searchInput: null,
            ...treeConfig
        };

        // Ensure tabulatorConfig has valid defaults
        this.tabulatorConfig = {
            container: null,
            columns: [],
            height: "300px",
            layout: "fitColumns",
            useTabulator: true,
            searchInput: null,
            ...tabulatorConfig
        };

        //debugger
        this.cleanup();
        this.initTree();
        if (this.tabulatorConfig.useTabulator) {
            this.initTabulator();
        }
    }

    cleanup() {
        // Destroy the existing tree instance (if any)
        if (this.treeConfig.container) {
            $(this.treeConfig.container).jstree("destroy").empty();
        }

        // Destroy the existing Tabulator instance (if any)
        if (this.table) {
            this.table.destroy();
            this.table = null;  // Clear the table reference
        }
    }

    initTree() {
        if (!this.treeConfig.container) {
            console.error("Tree container is not defined.");
            return;
        }
        /*debugger*/

        // Initialize the jstree component
        $(this.treeConfig.container).jstree({
            core: {
                data: this.treeConfig.data,
                multiple: this.treeConfig.multipleSelection
            },
            plugins: ["wholerow", "checkbox", "search"],
        }).on('changed.jstree', (e, data) => {
            // Update table whenever a selection change occurs
            if (data && data.selected) {
                this.updateTable();
            }
        });
        //    .on("ready.jstree", function () {
        //    debugger
        //    // Select nodes where isSelect is true
        //    treeData.forEach(node => {
        //        if (node.isSelected) {
        //            $('#treeContainer').jstree(true).select_node(node.id);
        //        }
        //    });
        //});

        // Handle the tree search functionality
        if (this.treeConfig.searchInput) {
            let to = false;
            $(this.treeConfig.searchInput).keyup(() => {
                if (to) clearTimeout(to);
                to = setTimeout(() => {
                    let searchValue = $(this.treeConfig.searchInput).val();
                    $(this.treeConfig.container).jstree(true).search(searchValue);
                }, 250);
            });
        } else {
            console.warn("Tree searchInput is not provided.");
        }
    }

    initTabulator() {

        //debugger
        if (!this.tabulatorConfig.container) {
            console.error("Tabulator container is not defined.");
            return;
        }

        // Initialize Tabulator component
        this.table = new Tabulator(this.tabulatorConfig.container, {
            data: [], // Initially no data
            columns: this.tabulatorConfig.columns,
            groupBy: "parentName",  // Group data by parentName (or adjust accordingly)
            height: this.tabulatorConfig.height,
            placeholder: this.tabulatorConfig.placeholder,
        });

        // Handle the search functionality for Tabulator
        if (this.tabulatorConfig.searchInput) {
            $(this.tabulatorConfig.searchInput).on("input", (e) => {
                this.table.setFilter("text", "like", e.target.value);
            });
        } else {
            console.warn("Tabulator searchInput is not provided.");
        }
    }

    updateTable() {
        if (!this.treeConfig.container) return;
        var teeviewid = this.treeConfig.container;
        // Fetch the selected nodes from the tree
        const selectedNodes = $(teeviewid).jstree(true).get_selected(true);

        const allnodes = $(teeviewid).jstree(true).get_json('#', { flat: true });
        $(teeviewid).on('ready.jstree', function () {
            $(teeviewid).jstree(true).get_json('#', { flat: true }).forEach(function (node) {
                var childexist = allnodes.filter(x => x.parent == node.id);
                if (node.parent == "#" && childexist.length==0) {
                    $('#' + node.id + ' > a > .jstree-checkbox').hide();  // Hide checkbox
                }
                else {
                    $('#' + node.id + ' > a > .jstree-checkbox').show();  // Hide checkbox
                }
                
            });
        });
        // Filter out nodes that don't have 'parentName' or have children
        const selectedData = selectedNodes
            .map(node => node.original)
            .filter(item => !item.children && item.parentName);

        

        // Update the Tabulator table if enabled
        if (this.tabulatorConfig.useTabulator && this.table) {
            this.table.setData(selectedData);
        }
    }

    fetchSelectedData() {
        // Retrieve data currently in the table
        return this.table ? this.table.getData() : [];
    }
}
