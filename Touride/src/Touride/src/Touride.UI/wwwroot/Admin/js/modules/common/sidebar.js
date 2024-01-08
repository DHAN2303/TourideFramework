/*
// Sidebar
*/
(function() {
    'use strict';

    $(initSidebar);

    var $sidebar;

    function initSidebar() {
        $sidebar = $('.sidebar');

        // AUTOCOLLAPSE ITEMS
        // -----------------------------------

        var sidebarCollapse = $sidebar.find('.collapse');
        sidebarCollapse.on('show.bs.collapse', function(event) {
            event.stopPropagation();
            if ($(this).parents('.collapse').length === 0)
                sidebarCollapse
                    .filter('.show')
                    .not('#user-block')
                    .collapse('hide');
        });

        // SIDEBAR ACTIVE STATE
        // -----------------------------------

        // Find current active item
        var currentItem = $('.sidebar .active').parents('li');

        // hover mode don't try to expand active collapse
        currentItem
            .addClass('active') // activate the parent
            .children('.collapse') // find the collapse
            .collapse('show'); // and show it

        // SIDEBAR COLLAPSED ITEM HANDLER
        // -----------------------------------

        var eventName = isTouch() ? 'click' : 'mouseenter';
        var subNav = $();
        $sidebar.find('.sidebar-nav > li').on(eventName, function(e) {
            if (isSidebarCollapsed()) {
                subNav.trigger('mouseleave');
                subNav = toggleMenuItem($(this));

                // Used to detect click and touch events outside the sidebar
                sidebarAddBackdrop();
            }
        });

        // Autoclose sidebar on external clicks
        $('.wrapper').on('click.sidebar', function(e) {
            // don't check if sidebar not visible
            if (!document.body.classList.contains('aside-toggled')) return;

            var $target = $(e.target);
            if (
                !$target.parents('.aside-container').length && // if not child of sidebar
                !$target.is('#user-block-toggle') && // user block toggle anchor
                !$target.parent().is('#user-block-toggle') // user block toggle icon
            ) {
                document.body.classList.remove('aside-toggled');
            }
        });
    }

    function sidebarAddBackdrop() {
        var $backdrop = $('<div/>', { class: 'sidebar-backdrop' });
        $backdrop.insertAfter('.aside-container').on('click mouseenter', function() {
            removeFloatingNav();
        });
    }

    // Open the collapse sidebar submenu items when on touch devices
    // - desktop only opens on hover
    function toggleTouchItem($element) {
        $element.siblings('li').removeClass('open');
        $element.toggleClass('open');
    }

    // Handles hover to open items under collapsed menu
    // -----------------------------------
    function toggleMenuItem($listItem) {
        removeFloatingNav();

        var ul = $listItem.children('ul');

        if (!ul.length) return $();
        if ($listItem.hasClass('open')) {
            toggleTouchItem($listItem);
            return $();
        }

        var $aside = $('.aside-container');
        var $asideInner = $('.aside-inner'); // for top offset calculation
        // float aside uses extra padding on aside
        var mar =
            parseInt($asideInner.css('padding-top'), 0) + parseInt($aside.css('padding-top'), 0);

        var subNav = ul.clone().appendTo($aside);

        toggleTouchItem($listItem);

        var itemTop = $listItem.position().top + mar - $sidebar.scrollTop();
        var vwHeight = document.body.clientHeight;

        subNav.addClass('nav-floating').css({
            position: 'fixed',
            top: itemTop,
            bottom: subNav.outerHeight(true) + itemTop > vwHeight ? 0 : 'auto'
        });

        subNav.on('mouseleave', function() {
            toggleTouchItem($listItem);
            subNav.remove();
        });

        return subNav;
    }

    function removeFloatingNav() {
        $('.sidebar-subnav.nav-floating').remove();
        $('.sidebar-backdrop').remove();
        $('.sidebar li.open').removeClass('open');
    }

    function isTouch() {
        return 'ontouchstart' in window || navigator.msMaxTouchPoints;
    }

    function isSidebarCollapsed() {
        return document.body.classList.contains('aside-collapsed');
    }

    function isSidebarToggled() {
        return document.body.classList.contains('aside-toggled');
    }

    function isMobile() {
        return document.body.clientWidth < APP_MEDIAQUERY.tablet;
    }
})();
