/**
* Template Name: MOE Web Site
* Bootstrap Version:v5.3.1
*/
(function() {
  "use strict";

  /**
   * Easy selector helper function
   */
  const select = (el, all = false) => {
    el = el.trim()
    if (all) {
      return [...document.querySelectorAll(el)]
    } else {
      return document.querySelector(el)
    }
  }



  
  /**
   * Easy event listener function
   */
  const on = (type, el, listener, all = false) => {
    let selectEl = select(el, all)
    if (selectEl) {
      if (all) {
        selectEl.forEach(e => e.addEventListener(type, listener))
      } else {
        selectEl.addEventListener(type, listener)
      }
    }
  }

  /**
   * Easy on scroll event listener 
   */
  const onscroll = (el, listener) => {
    el.addEventListener('scroll', listener)
  }

  /**
   * Navbar links active state on scroll
   */
  let navbarlinks = select('#navbar .scrollto', true)
  const navbarlinksActive = () => {
    let position = window.scrollY + 200
    navbarlinks.forEach(navbarlink => {
      if (!navbarlink.hash) return
      let section = select(navbarlink.hash)
      if (!section) return
      if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
        navbarlink.classList.add('active')
      } else {
        navbarlink.classList.remove('active')
      }
    })
  }
  window.addEventListener('load', navbarlinksActive)
  onscroll(document, navbarlinksActive)

  /**
   * Scrolls to an element with header offset
   */
  const scrollto = (el) => {
    let header = select('#header')
    let offset = header.offsetHeight

    let elementPos = select(el).offsetTop
    window.scrollTo({
      top: elementPos - offset,
      behavior: 'smooth'
    })
  }

  /**
   * Toggle .header-scrolled class to #header when page is scrolled
   */
  let selectHeader = select('#header')
  let selectTopbar = select('#topbar')
  if (selectHeader) {
    const headerScrolled = () => {
      if (window.scrollY > 80) {
        selectHeader.classList.add('header-scrolled')
        if (selectTopbar) {
          selectTopbar.classList.add('topbar-scrolled')
        }
      } else {
        selectHeader.classList.remove('header-scrolled')
        if (selectTopbar) {
          selectTopbar.classList.remove('topbar-scrolled')
        }
      }
    }
    window.addEventListener('load', headerScrolled)
    onscroll(document, headerScrolled)
  }

 /**
     * Scroll top button
     */
    let scrollTop = document.querySelector('.scroll-top');

    function toggleScrollTop() {
        if (scrollTop) {
            window.scrollY > 100 ? scrollTop.classList.add('active') : scrollTop.classList.remove('active');
        }
    }
    scrollTop.addEventListener('click', (e) => {
        e.preventDefault();
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    window.addEventListener('load', toggleScrollTop);
    document.addEventListener('scroll', toggleScrollTop);

// Function to close the menu
function closeMobileMenu() {
  const navbar = select('#navbar');
  const toggleButton = select('.mobile-nav-toggle');

  if (navbar.classList.contains('navbar-mobile')) {
    navbar.classList.remove('navbar-mobile');
    toggleButton.classList.remove('bi-x');
    toggleButton.classList.add('bi-list');
  }
}

// Toggle menu when the button is clicked
on('click', '.mobile-nav-toggle', function(e) {
  e.stopPropagation(); // Prevent the click from propagating to the document
  const navbar = select('#navbar');
  navbar.classList.toggle('navbar-mobile');
  this.classList.toggle('bi-list');
  this.classList.toggle('bi-x');
});

// Add a click event listener to the document to close the menu when clicking outside
document.addEventListener('click', function(e) {
  const navbar = select('#navbar');
  const toggleButton = select('.mobile-nav-toggle');
  
  if (navbar.classList.contains('navbar-mobile') && e.target !== toggleButton && !navbar.contains(e.target)) {
    closeMobileMenu();
  }
});

  /**
   * Mobile nav dropdowns activate
   */
  on('click', '.navbar .dropdown > a', function(e) {
    if (select('#navbar').classList.contains('navbar-mobile')) {
      e.preventDefault()
      this.nextElementSibling.classList.toggle('dropdown-active')
    }
  }, true)

  /**
   * Scrool with ofset on links with a class name .scrollto
   */
  on('click', '.scrollto', function(e) {
    if (select(this.hash)) {
      e.preventDefault()

      let navbar = select('#navbar')
      if (navbar.classList.contains('navbar-mobile')) {
        navbar.classList.remove('navbar-mobile')
        let navbarToggle = select('.mobile-nav-toggle')
        navbarToggle.classList.toggle('bi-list')
        navbarToggle.classList.toggle('bi-x')
      }
      scrollto(this.hash)
    }
  }, true)



  $(".sub_parent").on("click", function(e) {
    var parent_id = $(this).attr('data');
    if ($(this).hasClass("fa-plus-square")) {
        $(this).removeClass("fa-plus-square");
        $(this).addClass("fa-minus-square");
        $(".sub_parent_menu_" + parent_id).show();
    } else {
        $(this).removeClass("fa-minus-square");
        $(this).addClass("fa-plus-square");
        $(".sub_parent_menu_" + parent_id).hide();
    }
});
// Menu Dropdown Toggle
if ($('.menu-trigger').length) {
    $(".menu-trigger").on('click', function() {
        $(this).toggleClass('active');
        $('.header-area .nav').slideToggle(200);
    });
}
$("#parent-element").on("click", ".dropdown-menu", function(e) {
    $(this).parent().is(".open") && e.stopPropagation();
});
const spans = document.querySelectorAll(".accordion-item-header span");

/***Third level menu*/
spans.forEach(span => {
    span.addEventListener("click", function() {
        const header = this.parentElement;
        const body = header.nextElementSibling;
        const isActive = header.classList.contains("active");

        // Close other accordion items
        const allHeaders = document.querySelectorAll(".accordion-item-header");
        allHeaders.forEach(otherHeader => {
            if (otherHeader !== header) {
                otherHeader.classList.remove("active");
            }
        });

        // Toggle active class and display accordion body
        if (isActive) {
            header.classList.remove("active");

        } else {
            header.classList.add("active");

        }
    });
});

/***Swiper Slider*/


const swiper = new Swiper('.mySwiper', {
      loop: true,
      pagination: {
        el: '.swiper-pagination',
        clickable: true,
      },
      
      autoplay:true
    });

})()