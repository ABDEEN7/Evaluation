// Manage Language
const pathParts = window.location.pathname.split("/");
let currentLang = pathParts[1];

const loadMainNavbar = () => {
    const options = {
        success: function (data) {
            let navContainer = $("#navbar ul");
            let fragment = $(document.createDocumentFragment());
            // Add your static first item
            fragment.append(`
    <li class="nav-item">
        <a class="nav-link" href="index.html"></i></a>
    </li>
`);
            $.each(data, function (index, navItem) {
                fragment.append(createNavItem(navItem));
            });

            navContainer.append(fragment);
        },
        error: function () {

        }
    };
    return jqClient(options).Get(`/website/getnavbar?deprouting=${deprouting}`);
};
function createNavItem(item) {
    let dropdownClass = 'nav-item';
    let isNavParent = (item.children.length > 0)
    if (isNavParent) {
        dropdownClass = 'dropdown';
    }
    let $li = $("<li>").addClass(dropdownClass);
    let url = item.isInternal ? `/${currentLang}${item.url}` : item.url;
    let $a = $("<a class='nav-link'>").attr("href", url || "#")
        .append($("<span>").text(item.title));
    if (isNavParent) {
        $a.append($("<i>").addClass("fa fa-chevron-down toggle-dropdown"));
    }


    if (item.target) {
        $a.attr("target", item.target);
    }

    $li.append($a);

    if (item.children && item.children.length > 0) {
        let $ul = $("<ul>");
        let fragment = $(document.createDocumentFragment());

        $.each(item.children, function (index, child) {
            fragment.append(createNavItem(child));
        });

        $ul.append(fragment);
        $li.append($ul);
    }

    return $li;
}


function removeParameterFromUrl(url, parameterKey) {
    var baseUrl = url.split('?')[0];
    var urlQueryString = '?' + url.split('?')[1];
    var newParamQueryString = '';

    if (urlQueryString.indexOf(parameterKey + '=') > -1) {
        var params = urlQueryString.split('&');

        for (var i = 0; i < params.length; i++) {
            var parameterName = params[i].split('=')[0];
            if (parameterName !== parameterKey) {
                newParamQueryString += params[i] + '&';
            }
        }
        newParamQueryString = newParamQueryString.slice(0, -1);

        return baseUrl + newParamQueryString;
    }
    return url;
}



$(document).ready(async function () {

    loadMainNavbar().then(() => {
        document.querySelectorAll('.navmenu .toggle-dropdown').forEach(navmenu => {
            navmenu.addEventListener('click', function (e) {
                e.preventDefault();
                this.parentNode.classList.toggle('active');
                this.parentNode.nextElementSibling.classList.toggle('dropdown-active');
                e.stopImmediatePropagation();
            });
        });
    });
    loadDepartments();
});


$(document).on('select2:open', () => {
    setTimeout(() => {
        const searchField = document.querySelector('.select2-container--open .select2-search__field');
        if (searchField) {
            searchField.focus();
        }
    }, 0);
});

$langToggler.prop("checked", currentLang === "ar").change(() => {
    const newLang = $langToggler.is(":checked") ? "ar" : "en";

    sharedUtility().SetCookie("lang", newLang, 30);

    pathParts[1] = newLang;

    window.location.href =
        window.location.origin +
        pathParts.join("/") +
        window.location.search;
});


