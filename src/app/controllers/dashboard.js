(function (app) {
	
    'use strict';

    function dashboardCtrl($scope, $routeParams, $log, vMixService, appService) {
		
		$scope.title = 'Dashboard';
        $scope.status = 'Not started';
		
		$scope.activate = function(){
			$log.debug('View activated');
            $scope.busy = true;
            appService.getExpectedInputs().then(function(response){
               $scope.expectedInputs = response.data.inputs; 
               getStatus();
            });
		}	
        
        function getStatus() {
            vMixService.getStatus().then(function(data){
                $log.debug('status', data);
                $scope.vmix = data;
                fixResponse();
                $log.debug('fixed', data);
                $scope.busy = false;
             });
        }
		
		function fixResponse(){
			$scope.vmix.active = findFromExpected($scope.vmix.active);
			$scope.vmix.preview = findFromExpected($scope.vmix.preview);
		}
        
        function findFromExpected(vmixInput){
			var input = _.find($scope.vmix.inputs.input, function(item){
				return item._number == vmixInput;
			});
			if (!input) return vmixInput;
			input = _.find($scope.expectedInputs, function(item){
				return input._title && (item._title === input._title);
			});
			return input || vmixInput;
        }
	}
	
	dashboardCtrl.$inject = ['$scope', '$routeParams', '$log', 'vMixService', 'appService'];
	
	app.controller("DashboardCtrl", dashboardCtrl);
 
})(app);