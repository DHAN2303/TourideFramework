/*
// Login Multi
*/

(function() {
    'use strict';

    $(initParsleyForPages);

    function initParsleyForPages() {
        var loginMulti = document.getElementById('login-multi');

        if (loginMulti) $('.collapse').on('show.bs.collapse', toggleActiveCard);
    }

    function toggleActiveCard(e) {
        $('.card-header.bg-primary')
            .removeClass('bg-primary py-4 font-weight-bold')
            .addClass('bg-transparent');

        $(e.currentTarget.previousElementSibling)
            .removeClass('bg-transparent')
            .addClass('bg-primary py-4 font-weight-bold');
    }
})();
