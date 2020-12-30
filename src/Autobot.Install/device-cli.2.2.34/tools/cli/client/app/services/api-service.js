(function (forte) {
    "use strict";

    function service($log, $resource) {

        var serviceUrl = '/api/device/';

        var service = $resource(serviceUrl, { }, {
            fetchCommand: { method: 'POST', url: serviceUrl + 'fetch' },
            fetchSettings: { method: 'GET', url: serviceUrl + 'settings' },
            fetchState: { method: 'GET' },
            publishState: { method: 'POST', url: serviceUrl + 'publish' },
            reset: { method: 'POST', url: serviceUrl + 'reset' },
            shutdown: { method: 'POST', url: serviceUrl + 'shutdown' },
            updateSetting: { method: 'POST', url: serviceUrl + 'settings/:setting' }
        });

        var fetchCommand = function () {
            return service.fetchCommand().$promise;
        }

        var fetchSettings = function() {
            return service.fetchSettings().$promise;
        }

        var fetchState = function () {
            return service.fetchState().$promise;
        }

        var publishState = function () {
            return service.publishState().$promise;
        }

        var reset = function () {
            return service.reset().$promise;
        }

        var shutdown = function () {
            return service.shutdown().$promise;
        }

        var updateSetting = function(setting, value) {
            return service.updateSetting({ setting: setting }, value).$promise;
        }

        return {
            fetchCommand: fetchCommand,
            fetchSettings: fetchSettings,
            fetchState: fetchState,
            publishState: publishState,
            reset: reset,
            shutdown: shutdown,
            updateSetting: updateSetting
        }
    }

    service.$inject = ['$log', '$resource'];
    forte.app.factory("ApiService", service);

})(forte);