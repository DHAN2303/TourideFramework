/*!
 *
 * 47admin - Bootstrap Admin Theme
 *
 * Version: 2.0
 * Author: @geedmo
 * Website: http://geedmo.com
 * License: https://codecanyon.net/licenses/standard
 *
 */

(function() {
    'use strict';

    $(initApp);

    // initialize app here
    function initApp() {
        // unhide offsidebar
        var offsidebar = document.querySelector('.offsidebar.d-none');
        if (offsidebar) offsidebar.classList.remove('d-none');
    }
})();
