/*
// Navbar search
*/

(function() {
    'use strict';

    $(initNavbarSearch);

    function initNavbarSearch() {
        var navbarFormSelector = 'form.navbar-form';
        var navbarForm = document.querySelector(navbarFormSelector);

        if (!navbarForm) return; // if not search form in page simply abort

        var formInput = navbarForm.querySelector('input[type="text"]');
        // Open search elements
        var searchOpener = [].slice.call(document.querySelectorAll('[data-search-open]'));
        // Close search elements
        var searchDismiss = [].slice.call(document.querySelectorAll('[data-search-dismiss]'));

        searchOpener.forEach(function(el) {
            el.addEventListener('click', function(e) {
                e.stopPropagation();
                navbarSearchToggle();
            });
        });

        if (formInput) {
            formInput.addEventListener('click', function(e) {
                // stop event to avoid handle as external click
                e.stopPropagation();
            });
            formInput.addEventListener('keyup', function(e) {
                // Close on ESC key press
                if (e.keyCode == 27) navbarSearchDismiss();
            });
        }

        // click anywhere closes the search
        document.addEventListener('click', navbarSearchDismiss);

        // dismissable options
        searchDismiss.forEach(function(el) {
            el.addEventListener('click', function(e) {
                e.stopPropagation();
                navbarSearchDismiss();
            });
        });

        // Helper methods
        function navbarSearchToggle() {
            if (navbarForm) {
                navbarForm.classList.toggle('open');
                var isOpen = navbarForm.classList.contains('open');
                if (formInput) formInput[isOpen ? 'focus' : 'blur']();
            }
        }
        function navbarSearchDismiss() {
            if (navbarForm) {
                navbarForm.classList.remove('open'); // Close control
                if (formInput) formInput.blur(); // remove focus
                // formInput.value = ''             // Empty input
            }
        }
    }
})();
