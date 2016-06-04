(function (app) {
    
	"use strict";
	
    function vMixService($http, $q, $log) {
		
		
		function getStatus(){
			var deferred = $q.defer();
			$http.get('//localhost:8088/api')
				.then(statusRetrieved.bind(deferred), couldNotRetrieveStatus.bind(deferred));
            return deferred.promise;
		}
		
		function statusRetrieved(response){
            var deferred = this;
            $log.debug(arguments);
			var x2js = new X2JS();
            var jsonObj = x2js.xml_str2json( response.data );
            deferred.resolve(jsonObj.vmix);
		}
		
		function couldNotRetrieveStatus(){
			
		}
		
		return {
			getStatus: getStatus
		}
	}

    vMixService.$inject = ['$http', '$q', '$log'];

    app.factory("vMixService", vMixService);

})(app);
