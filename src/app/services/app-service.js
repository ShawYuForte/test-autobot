(function (app) {
    
	"use strict";
	
    function appService($http, $q, $log) {
		
		function getExpectedInputs(){
			return $http.get('app/data/inputs.json');
		}
				
		return {
			getExpectedInputs: getExpectedInputs
		}
	}

    appService.$inject = ['$http', '$q', '$log'];

    app.factory("appService", appService);

})(app);
