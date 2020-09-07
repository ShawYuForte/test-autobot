(function (forte) {
    "use strict";

    function settingsController($scope, $routeParams, $log, apiService) {

        var model = this;

        $scope.activate = function() {
            $log.debug('Activating settings view');
            fetchSettings();
        }

        function fetchSettings() {
            apiService.fetchSettings()
                .then(settingsFetched)
                .catch(couldNotFetchSettings);
        }

        $scope.temp = function(entry) {
            $log.debug('settingsForm[\'settings_0\']', model.settingsForm['settings_0']);
        }

        $scope.save = function(setting, value) {
            $log.debug('Updating setting/value', [setting, value]);
            apiService.updateSetting(setting, value)
                .then(settingUpdated)
                .catch(couldNotUpdateSetting);
        }

        function settingUpdated(data) {
            $scope.settings = [];
            fetchSettings();
        }

        function couldNotUpdateSetting(err) {
            $log.error(err);
        }

        $scope.cancel = function () {
            $log.debug('Canceling');
            $scope.settings = [];
            fetchSettings();
        }

        function settingsFetched(data) {
			$log.debug('Fetched settings', data);

			var ordered = {};
			Object.keys(data.settings).sort().forEach(function (key) {
				ordered[key] = data.settings[key];
			});

			$scope.settings = ordered;
            model.settings = data.settings;
        }

        function couldNotFetchSettings(err) {
            $log.error(err);
        }
    }

    settingsController.$inject = ['$scope', '$routeParams', '$log', 'ApiService'];
    forte.app.controller("SettingsCtrl", settingsController);

})(forte);