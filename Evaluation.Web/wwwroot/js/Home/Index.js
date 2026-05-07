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


function loadDepartments() {
    let webGroupPath = webgroup;
    jqClient().Get(`/Website/GetDepartmentsForWebGroup?webGroupPath=${webGroupPath}`)
        .done((result) => {

            const data = (result && result.result) ? result.result : [];

            renderDepartmentsTable(data.result);
        })
        .fail((jqXHR, textStatus, err) => {
            console.error('GetAll department failed', textStatus, err);
        });
}

function renderDepartmentsTable(departments) {
    if (!Array.isArray(departments) || departments.length === 0) return;

    const tabsContainer = $('#viewTabs'); // <ul> for tabs
    const tabContentContainer = $('#viewTabsContent'); // <div> for tab content

    tabsContainer.empty();
    tabContentContainer.empty();

    // Group departments by typeName
  const educationTypes = [
        "مدارس حكومية",
        "مدارس خاصة",
        "رياض أطفال و دور الحضانة"
    ];

    const educationDepartments = departments.filter(item =>
        educationTypes.includes(item.name)
    );

    const otherDepartments = departments.filter(item =>
        !educationTypes.includes(item.name)
    );

    const grouped = {};

    if (otherDepartments.length > 0) {
        grouped["Other"] = otherDepartments;
    }

    // Helper: render a department card  <img src="${item.imgBlobUrl}"/>
    const getDepartmentIcon = (name) => {
        if (name.includes("حكومية")) return "las la-school";
        if (name.includes("خاصة")) return "las la-shapes";
        if (name.includes("رياض") || name.includes("حضانة")) return "las la-baby-carriage";
        return "las la-school";
    };

    const createDepartmentCard = (item) => `
        <div class="col-md-4 mb-4">
            <div class="card item-card shadow-sm border-0"
                 onclick="window.location.href='/evaluationplan/${item.routingPath}'"
                 style="cursor: pointer;">

                <div class="card-body p-0">
                    <div class="main-card text-center">

                        <div class="card-img-wrapper">
                            <img src="${item.imgBlobUrl ?? '/assets/img/login-bg.png'}" 
                                 alt="${item.name}" 
                                 class="card-img">
                        </div>

                        <div class="card-icon-circle shadow-sm">
                            <i class="${getDepartmentIcon(item.name)}"></i>
                        </div>

                        <div class="card-content">
                            <h5 class="card-title">${item.name}</h5>
                            <p class="card-text text-muted">${item.desc ?? ''}</p>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    `;

    // --- Create “All Departments” tab ---
    tabsContainer.append(`
        <li class="col-md-4 nav-item" role="presentation">
            <button class="nav-link fw-semibold active" id="all-tab"
                data-bs-toggle="tab" data-bs-target="#all"
                type="button" role="tab" aria-controls="all" aria-selected="true">
                كل الإدارات
            </button>
        </li>
    `);

    const allCardsHTML = departments.map(createDepartmentCard).join('');
    tabContentContainer.append(`
        <div class="tab-pane fade show active" id="all" role="tabpanel" aria-labelledby="all-tab">
            <div id="allContainer" class="row card-view fade-switch active">
                ${allCardsHTML || '<p class="text-muted">لا توجد إدارات متاحة.</p>'}
            </div>
        </div>
    `);

    // --- Create tabs for each typeName dynamically ---
    Object.keys(grouped).forEach((typeName) => {
        const safeId = typeName.replace(/\s+/g, '-').toLowerCase();

        tabsContainer.append(`
            <li class="col-md-4 nav-item" role="presentation">
                <button class="nav-link fw-semibold" id="${safeId}-tab"
                    data-bs-toggle="tab" data-bs-target="#${safeId}"
                    type="button" role="tab" aria-controls="${safeId}" aria-selected="false">
                    ${typeName}
                </button>
            </li>
        `);

        const cardsHTML = grouped[typeName].map(createDepartmentCard).join('');
        tabContentContainer.append(`
            <div class="tab-pane fade" id="${safeId}" role="tabpanel" aria-labelledby="${safeId}-tab">
                <div id="${safeId}Container" class="row card-view fade-switch active">
                    ${cardsHTML || '<p class="text-muted">لا توجد إدارات متاحة.</p>'}
                </div>
            </div>
        `);
    });
}

const loadMainBanner = () => {
    let webGroupPath = webgroup;

    const options = {
        success: function (response) {
            if (response) {
                let swiperWrapper = $('#banner-swiper-wrapper');
                bannerLoop = response.length > 1;
                response.forEach(function (banner) {
                    let titleWords = banner.title;
                    let slideHtml = `
        <div class="swiper-slide">
          <img src="${banner.imgURL}" alt="${banner.imgName}" class="img-fluid main-img">
           <div class="banner-content">
        <h1 class="text-white mb-4">${titleWords}</h1>
        <p>${banner.summary}</p>
      </div>
        </div>`;

                    swiperWrapper.append(slideHtml);
                });
            }
           


        },
        error: function () {

        }
    };
    return jqClient(options).Get(`/website/getbanner?webGroupPath=${webGroupPath}`);
};

$(document).ready(function () {
    loadDepartments();
    loadMainBanner().then(() => {
        new Swiper('.mySwiper', {
            loop: bannerLoop,
            speed: 1000,
            autoplay: {
                delay: 5000,
                disableOnInteraction: false,
            },
            slidesPerView: 'auto',
            navigation: {
                nextEl: '.swiper-button-next',
                prevEl: '.swiper-button-prev',
            },
            pagination: {
                el: '.swiper-pagination',
                type: 'bullets',
                clickable: 'true'
            },
            breakpoints: {
                320: {
                    slidesPerView: 1,
                    spaceBetween: 0
                },
                480: {
                    slidesPerView: 1,
                    spaceBetween: 0
                },

                99: {
                    slidesPerView: 1,
                    spaceBetween: 0
                }
            }
        });
    });
});