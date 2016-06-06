(function (app) {

    "use strict";

    function appService($http, $rootScope, data) {

        function getExpectedInputs() {
            return $http.get('app/data/inputs.json')
		        .then(function (response) {
		            $rootScope.broadcast('data-changed:app', response.data);
		            data.expectedInputs = response.data;
                });
        }

        return {
            getExpectedInputs: getExpectedInputs
        }
    }

    appService.$inject = ['$http', '$rootScope', '_data'];

    app.factory("appService", appService);

})(app);
