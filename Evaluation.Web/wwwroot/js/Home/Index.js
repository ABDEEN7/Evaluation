document.addEventListener("DOMContentLoaded", function () {
    const toggleBtns = document.querySelectorAll(".toggle-btn");

    // Store current view for each tab
    const tabViews = {
        departments: 'card',
        education: 'card',
        special: 'card'
    };

    // Function to apply view to a container
    function applyView(container, view) {
        container.classList.remove("card-view", "list-view");
        container.classList.add(`${view}-view`);

        const items = container.querySelectorAll(".col-md-4, .col-md-12");
        items.forEach(item => {
            if (view === "card") {
                item.classList.remove("col-md-12");
                item.classList.add("col-md-4");
            } else {
                item.classList.remove("col-md-4");
                item.classList.add("col-md-12");
            }
        });
    }

    // Toggle button click
    toggleBtns.forEach(btn => {
        btn.addEventListener("click", function () {
            const view = this.getAttribute("data-view");

            // Find the currently active tab content
            const activeTab = document.querySelector(".tab-pane.active.show");
            if (!activeTab) return;

            const tabId = activeTab.id;
            tabViews[tabId] = view; // Save view for this tab

            // Update toggle button active state
            toggleBtns.forEach(b => b.classList.remove("active"));
            this.classList.add("active");

            // Apply view with smooth transition
            const container = activeTab.querySelector(".fade-switch");
            if (!container) return;

            container.classList.add("switching");
            setTimeout(() => {
                applyView(container, view);
                container.classList.remove("switching");
            }, 200);
        });
    });

    // Apply stored view when switching tabs
    const tabLinks = document.querySelectorAll('[data-bs-toggle="tab"]');
    tabLinks.forEach(tab => {
        tab.addEventListener("shown.bs.tab", event => {
            const tabId = event.target.getAttribute("data-bs-target").replace("#", "");
            const container = document.querySelector(`#${tabId} .fade-switch`);
            if (!container) return;

            const view = tabViews[tabId] || 'card';
            applyView(container, view);

            // Update toggle button active state to match stored view
            toggleBtns.forEach(btn => {
                btn.classList.remove("active");
                if (btn.getAttribute("data-view") === view) {
                    btn.classList.add("active");
                }
            });
        });
    });
});