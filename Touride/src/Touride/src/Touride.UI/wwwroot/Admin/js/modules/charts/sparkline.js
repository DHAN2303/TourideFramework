/*
// Sparkline
*/

(function() {
    'use strict';

    $(initSparkline);

    function initSparkline() {
        $('.sparkline').each(initSparkLine);

        function initSparkLine() {
            var $element = $(this),
                options = $element.data(),
                values = options.values && options.values.split(',');

            options.type = options.type || 'bar'; // default chart is bar
            options.disableHiddenCheck = true;

            if (APP_COLORS && APP_COLORS[options.barColor]) {
                options.barColor = APP_COLORS[options.barColor];
            }
            options.resize = true;

            $element.sparkline(values, options);
        }
    }
})();
