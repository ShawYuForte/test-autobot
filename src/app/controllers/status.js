(function (app) {

    'use strict';

    function statusCtrl($scope, data) {

        $on('data-changed:vmix', vmixDataChanged);

        $scope.ready = false;

        $scope.activated = function() {
            refresh();
        }

        function vmixDataChanged() {
            refresh();
        }

        function refresh() {
            for (var input in data.expectedInputs) {
                
            }
        }

    }

    statusCtrl.$inject = ['$scope', '_data'];

    app.controller("DashboardCtrl", statusCtrl);

})(app);