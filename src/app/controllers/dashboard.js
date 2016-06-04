(function (app) {
	
    'use strict';

    function dashboardCtrl($scope, $routeParams, $log, vMixService) {
		
		$scope.title = 'Dashboard';
		
		$scope.activate = function(){
			$log.debug('View activated');
			vMixService.getStatus().then(function(data){
				$log.debug('status', data);
                $scope.vmix = data;
				fixResponse();
                $log.debug('fixed', data);
			});
		}	
		
		function fixResponse(){
			var active = $scope.vmix.active;
			var preview = $scope.vmix.preview;
			var input = _.find($scope.vmix.inputs.input, function(item){
				return item._number == active;
			});
			if (input) $scope.vmix.active = input;
			input = _.find($scope.vmix.inputs.input, function(item){
				return item._number == preview;
			});
			if (input) $scope.vmix.preview = input;
		}
	}
	
	dashboardCtrl.$inject = ['$scope', '$routeParams', '$log', 'vMixService'];
	
	app.controller("DashboardCtrl", dashboardCtrl);
 
})(app);