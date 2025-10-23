/**
 * Template Name: Evalution Dashboard
 * Bootstrap Version: v5.3.1
 */
(function(){
  /******Sidebar Collapsing********/

document.addEventListener('DOMContentLoaded', () => {
  const sidebar = document.getElementById('sidebar');
  const collapseBtn = document.getElementById('collapseBtn');
  const mobileToggle = document.getElementById('mobileToggle');

  if (!sidebar) return;

  // Collapse / expand via collapse button
  collapseBtn?.addEventListener('click', () => {
    sidebar.classList.toggle('collapsed');
    const icon = collapseBtn.querySelector('i');
    if (icon) {
      if (sidebar.classList.contains('collapsed')) {
        icon.classList.replace('bi-chevron-left', 'bi-chevron-right');
      } else {
        icon.classList.replace('bi-chevron-right', 'bi-chevron-left');
      }
    }
  });

  // Collapse / expand via mobile toggle
  mobileToggle?.addEventListener('click', () => {
    sidebar.classList.toggle('collapsed');
  });

  // Close sidebar when clicking outside on mobile
  document.addEventListener('click', (e) => {
    if (!mobileToggle) return;
    if (
      window.innerWidth < 992 &&
      !sidebar.contains(e.target) &&
      !mobileToggle.contains(e.target)
    ) {
      sidebar.classList.add('collapsed');
    }
  });

  // ✅ Automatically collapse/expand based on window size
  const handleResize = () => {
    if (window.innerWidth < 992) {
      sidebar.classList.add('collapsed');
    } else {
      sidebar.classList.remove('collapsed');
    }
  };

  // Run once on page load
  handleResize();

  // Listen for screen resize
  window.addEventListener('resize', handleResize);
});



/******Select 2********/
$('#userRole').select2({
      placeholder: "اختر الدور",
      allowClear: true,
      width: '100%',
      
    });

// Initialize DataTable

      const table = $('#userTable').DataTable({
        retrieve: true,
        paging: true,
        searching: true, // keep true to allow custom search to work
        ordering: false,
        info: false,
        lengthChange: false,
        // Important: include 'p' and 'i' in dom so DataTables creates the nodes,
        // but we will move them to custom containers after initialization.
        dom: '<"dt-table-area"t><"dt-footer"ip>',

        // Optional: language or other options...
        language: {
          paginate: {
            previous: '&laquo;',
            next: '&raquo;'
          }
        },
        // After init, move the paging and info to our custom containers
        initComplete: function() {
          var $wrapper = $(this.api().table().container()); // DataTables wrapper

          // Move the pagination node into #dtPagination
          $wrapper.find('.dataTables_paginate').appendTo('#dtPagination');

          // Move the table info node into #dtInfo
          $wrapper.find('.dataTables_info').appendTo('#dtInfo');

          // Optional: style the pagination with Bootstrap classes
          $('#dtPagination .paginate_button').addClass('btn btn-sm btn-outline-secondary mx-1').removeClass('paginate_button');
          // make current page button look active
          $('#dtPagination .current').removeClass('btn-outline-secondary').addClass('btn-secondary text-white');
        }
        
      });
      // Keep active page style updated on draw
          table.on('draw', function () {
            // reset pagination buttons classes
            $('#dtPagination .btn').removeClass('btn-secondary text-white').addClass('btn-outline-secondary');
            $('#dtPagination .current').removeClass('btn-outline-secondary').addClass('btn-secondary text-white');
          });
      // Custom search input
      $('#customSearch').on('keyup', function() {
        table.search(this.value).draw();
      });

      // Select all checkboxes
      $('#selectAll').on('click', function() {
        const checked = this.checked;
        $('.custom-checkbox .row-select').prop('checked', checked);
      });

    $('#selectAll').on('change', function() {
      $('.custom-checkbox .row-select').prop('checked', this.checked);
    });
   

  /******DateRange flatpickr********/

  const fp = flatpickr("#dateRange", {
    mode: "range",
    locale: "ar",
    dateFormat: "Y-m-d",
    allowInput: true,
    onReady: function(selectedDates, dateStr, instance) {
      const monthsContainer = instance.calendarContainer.querySelector('.flatpickr-months');

      // Create arrow stack container
      const prev = instance.calendarContainer.querySelector('.flatpickr-prev-month');
      const next = instance.calendarContainer.querySelector('.flatpickr-next-month');

      const arrowStack = document.createElement('div');
      arrowStack.className = 'fp-arrow-stack';
      arrowStack.appendChild(prev);
      arrowStack.appendChild(next);

      // Insert arrow stack at start (left side in RTL)
      monthsContainer.insertBefore(arrowStack, monthsContainer.firstChild);

      // Month/year container remains for dropdowns (right side in RTL)
      const monthYear = monthsContainer.querySelector('.flatpickr-current-month');
      monthsContainer.appendChild(monthYear);

      // Add Apply/Cancel buttons if not already
      if (!instance.calendarContainer.querySelector('.fp-btns')) {
        const btns = document.createElement('div');
        btns.className = 'fp-btns';
        const cancel = document.createElement('button');
        cancel.className = 'fp-cancel';
        cancel.textContent = 'إلغاء';
        cancel.addEventListener('click', e => {
          e.preventDefault();
          instance.clear();
          instance.close();
        });
        const apply = document.createElement('button');
        apply.className = 'fp-apply';
        apply.textContent = 'تأكيد';
        apply.addEventListener('click', e => {
          e.preventDefault();
          instance.close();
        });
        btns.appendChild(cancel);
        btns.appendChild(apply);
        instance.calendarContainer.appendChild(btns);
      }
    }
  });
})();