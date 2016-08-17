(function (forte) {
    "use strict";

    function dashboardController($scope, $routeParams, $log, apiService, signalrService) {
        
        $scope.activate = function() {
            fetchState();
        }

        function fetchState() {
            apiService.fetchState()
                .then(stateFetched)
                .catch(couldNotFetchState);
        }

        function stateFetched(data) {
            $scope.state = data;
        }

        function couldNotFetchState(err) {
            $log.error(err);
        }
    }

    dashboardController.$inject = ['$scope', '$routeParams', '$log', 'ApiService', 'SignalrService'];
    forte.app.controller("DashboardCtrl", dashboardController);

})(forte);