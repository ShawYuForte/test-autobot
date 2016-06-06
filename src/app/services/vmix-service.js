(function (app) {

    "use strict";

    function vMixService($http, $q, $log, $timeout, $rootScope, config, data) {

        function getStatus() {
            var deferred = $q.defer();
            $http.get(config.apiPath)
				.then(statusRetrieved.bind(deferred), couldNotRetrieveStatus.bind(deferred));
            return deferred.promise;
        }

        function statusRetrieved(response) {
            var deferred = this;
            $log.debug(arguments);
            var x2js = new X2JS();
            var jsonObj = x2js.xml_str2json(response.data);
            data.vmix = jsonObj.vmix;
            deferred.resolve(jsonObj.vmix);
            $rootScope.broadcast('data-changed:vmix', jsonObj.vmix);
        }

        function couldNotRetrieveStatus(err) {
            var deferred = this;
            deferred.reject(err);
        }

        function loadPreset() {
            return $http.get(config.apiPath + '?Function=OpenPreset&Value=' + config.presetFilePath);
        }

        return {
            getStatus: getStatus,
            loadPreset: loadPreset
        }
    }

    vMixService.$inject = ['$http', '$q', '$log', '$timeout', '$rootScope', '_config', '_data'];

    app.factory("vMixService", vMixService);

})(app);
