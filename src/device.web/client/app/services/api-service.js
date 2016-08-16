(function (forte) {
    "use strict";

    function service($log, $resource) {

        var serviceUrl = '/api/device/';

        var service = $resource(serviceUrl, { }, {
            fetchSettings: { method: 'GET', url: serviceUrl + 'settings' }
        });

        var fetchSettings = function() {
            return service.fetchSettings().$promise;
        }

        return {
            fetchSettings: fetchSettings
        }
    }

    service.$inject = ['$log', '$resource'];
    forte.app.factory("ApiService", service);

})(forte);