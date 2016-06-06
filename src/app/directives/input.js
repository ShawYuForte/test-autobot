(function (app) {
    'use strict';
    
    function inputDirective($log) {
        
        var link = function (scope, element, attributes, ctrl, transclude) {
            
        }
        
        return {
            link: link,
            restrict: 'A',
            scope: {
                input:'='
            },
            templateUrl: 'app/templates/input.html',
            transclude: true,
            replace: 'true',
        }
    }
    
    inputDirective.$inject = ['$log'];
    app.directive('ngfitInput', inputDirective);
    
})(app);
