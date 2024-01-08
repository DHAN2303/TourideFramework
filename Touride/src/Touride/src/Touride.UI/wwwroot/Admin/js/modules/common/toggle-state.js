/*
// Toggle State
*/
(function() {
    'use strict';

    $(initToggleState);

    function initToggleState() {
        var toggle = new StateToggler();

        var toggleElements = [].slice.call(document.querySelectorAll('[data-toggle-state]'));

        toggleElements.forEach(function(el) {
            el.addEventListener('click', toggleStateHandler);
        });

        function toggleStateHandler(e) {
            // e.preventDefault();
            e.stopPropagation();
            var element = e.currentTarget,
                targetSel = element.getAttribute('data-target'),
                classname = element.getAttribute('data-toggle-state'),
                noPersist = element.hasAttribute('data-no-persist');

            // Specify a target selector to toggle classname
            // use body by default
            var target = targetSel ? document.querySelector(targetSel) : document.body;

            if (classname) {
                if (target.classList.contains(classname)) {
                    target.classList.remove(classname);
                    if (!noPersist) toggle.removeState(classname);
                } else {
                    target.classList.add(classname);
                    if (!noPersist) toggle.addState(classname);
                }
            }

            // some elements may need this when toggled class change the content size
            if (typeof Event === 'function') {
                // modern browsers
                window.dispatchEvent(new Event('resize'));
            } else {
                // old browsers and IE
                var resizeEvent = window.document.createEvent('UIEvents');
                resizeEvent.initUIEvent('resize', true, false, window, 0);
                window.dispatchEvent(resizeEvent);
            }
        }
    }

    // Handle states to/from localstorage
    function StateToggler() {
        var STORAGE_KEY_NAME = 'jq-toggleState';

        /** Add a state to the browser storage to be restored later */
        this.addState = function(classname) {
            var data = Storages.localStorage.get(STORAGE_KEY_NAME);
            if (data instanceof Array) data.push(classname);
            else data = [classname];
            Storages.localStorage.set(STORAGE_KEY_NAME, data);
        };
        /** Remove a state from the browser storage */
        this.removeState = function(classname) {
            var data = Storages.localStorage.get(STORAGE_KEY_NAME);
            if (data) {
                var index = data.indexOf(classname);
                if (index !== -1) data.splice(index, 1);
                Storages.localStorage.set(STORAGE_KEY_NAME, data);
            }
        };
        /** Load the state string and restore the classlist */
        this.restoreState = function(elem) {
            var data = Storages.localStorage.get(STORAGE_KEY_NAME);
            if (data instanceof Array)
                data.forEach(function(c) {
                    return elem.classList.add(c);
                });
        };
    }

    window.StateToggler = StateToggler;
})();
