/*
// Table Check All
*/

(function() {
    'use strict';

    $(initTableCheckAll);

    function initTableCheckAll() {
        var elements = [].slice.call(document.querySelectorAll('[data-check-all]'));

        elements.forEach(function(el) {
            return (el.onchange = checkAllHandler);
        });

        function checkAllHandler(e) {
            var element = e.currentTarget; // TH
            var table = closest(element, 'table');
            var checkbox = element.querySelector('input[type="checkbox"]');
            if (table && checkbox) {
                var ths = [].slice.call(table.querySelectorAll('th'));
                ths.some(function(el, index) {
                    if (el === element) {
                        var cbs = [].slice.call(
                            table.querySelectorAll(
                                'td:nth-child(' + (index + 1) + ') input[type="checkbox"]'
                            )
                        );
                        cbs.forEach(function(el) {
                            return (el.checked = checkbox.checked);
                        });
                        return true;
                    }
                });
            }
        }
    }

    function closest(element, selector) {
        if (Element.prototype.closest) {
            return element.closest(selector);
        } else {
            if (!Element.prototype.matches) {
                Element.prototype.matches =
                    Element.prototype.msMatchesSelector || Element.prototype.webkitMatchesSelector;
            }
            var el = element;
            do {
                if (el.matches(s)) return el;
                el = el.parentElement || el.parentNode;
            } while (el !== null && el.nodeType === 1);
            return null;
        }
    }
})();
