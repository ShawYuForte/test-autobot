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

        $scope.logs = [
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Information",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Error",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            },
            {
                "timestamp": moment("2016-08-17T13: 49: 45.5850744-04: 00").date(),
                "messageTemplate": "Fetchingcommand...",
                "level": "Debug",
                "renderedMessage": "Fetchingcommand...",
                "properties": {
            
                }
            }
        ];
    }

    dashboardController.$inject = ['$scope', '$routeParams', '$log', 'ApiService', 'SignalrService'];
    forte.app.controller("DashboardCtrl", dashboardController);

})(forte);