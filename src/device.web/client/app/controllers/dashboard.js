(function (forte) {
    "use strict";

    function dashboardController($scope, $routeParams, $log, $window, apiService, signalrService) {
        
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
            toastr.success('Device state refreshed');
        }

        function couldNotFetchState(err) {
            $log.error(err);
            toastr.error(err);
        }

        $scope.toCss = function (level) {

            switch(level) {
                case 'Debug':
                    return 'active';
                case 'Information':
                    return 'info';
                case 'Warning':
                    return 'warning';
                case 'Error':
                case 'Fatal':
                    return 'danger';

                default:
                    return '';
            }
        }

        $scope.logs = [];

        $scope.$on('log-event', function (e, logEvent) {
            //$log.debug('Received log event', logEvent);
            $scope.$apply(function() {
                $scope.logs.unshift(logEvent);
                if ($scope.logs.length > 100) {
                    $scope.logs.pop();
                }
            });
        });

        $scope.fetchCommand = function() {
            apiService.fetchCommand()
               .then(operationSucceeded)
               .catch(couldExecuteOperation);
        }

        $scope.refreshState = function() {
            fetchState();
        }

        $scope.publishState = function () {
            apiService.publishState()
               .then(operationSucceeded)
               .catch(couldExecuteOperation);
        }

        $scope.reset = function () {
            apiService.reset()
               .then(operationSucceeded)
               .catch(couldExecuteOperation);
        }

        $scope.shutdown = function () {
            apiService.shutdown()
               .then(shutdownSucceeded)
               .catch(couldExecuteOperation);
        }

        function shutdownSucceeded() {
            toastr.success('Server shut down successfully');
            $window.close();
        }

        function operationSucceeded() {
            $log.debug('Succeess');
            toastr.success('Operation executed successfully');
        }

        function couldExecuteOperation(err) {
            $log.error(err);
            toastr.error(err);
        }
    }

    dashboardController.$inject = ['$scope', '$routeParams', '$log', '$window', 'ApiService', 'SignalrService'];
    forte.app.controller("DashboardCtrl", dashboardController);

})(forte);