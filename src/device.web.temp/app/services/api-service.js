(function (forte) {
    "use strict";

    function service($log, $resource) {

        var serviceUrl = '/api/device/';

        var service = $resource(serviceUrl, { }, {
            fetchSettings: { method: 'GET', url: serviceUrl + 'settings' },
            fetchState: { method: 'GET' },
            updateSetting: { method: 'POST', url: serviceUrl + 'settings/:setting' }
        });

        var fetchSettings = function() {
            return service.fetchSettings().$promise;
        }

        var fetchState = function () {
            return service.fetchState().$promise;
        }

        var updateSetting = function(setting, value) {
            return service.updateSetting({ setting: setting }, value).$promise;
        }

        return {
            fetchSettings: fetchSettings,
            fetchState: fetchState,
            updateSetting: updateSetting
        }
    }

    service.$inject = ['$log', '$resource'];
    forte.app.factory("ApiService", service);

})(forte);