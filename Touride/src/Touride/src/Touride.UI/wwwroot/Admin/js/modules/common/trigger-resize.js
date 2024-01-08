/*
// Trigger Resize
*/

// Triggers a window resize event from any element
(function() {
    'use strict';

    $(initTriggerResize);

    function initTriggerResize() {
        var elements = [].slice.call(document.querySelectorAll('[data-trigger-resize]'));
        elements.forEach(function(el) {
            el.addEventListener('click', toggleResizeHandler);
        });
        function toggleResizeHandler(e) {
            var element = e.currentTarget;
            var value = element.getAttribute('data-trigger-resize');
            setTimeout(function() {
                // all IE friendly dispatchEvent
                var evt = document.createEvent('UIEvents');
                evt.initUIEvent('resize', true, false, window, 0);
                window.dispatchEvent(evt);
                // modern dispatchEvent way
                // window.dispatchEvent(new Event('resize'));
            }, value || 300);
        }
    }
})();
