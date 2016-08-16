(function (forte) {
    "use strict";

    function settingsController($scope, $routeParams, $log, apiService) {

        $scope.activate = function() {
            $log.debug('Activating settings view');

            apiService.fetchSettings()
                .then(settingsFetched)
                .catch(couldNotFetchSettings);
        }

        function settingsFetched(data) {
            $log.debug('Fetched settings', data);
            $scope.settings = data.settings;
        }

        function couldNotFetchSettings(err) {
            $log.error(err);
        }
    }

    settingsController.$inject = ['$scope', '$routeParams', '$log', 'ApiService'];
    forte.app.controller("SettingsCtrl", settingsController);

})(forte);