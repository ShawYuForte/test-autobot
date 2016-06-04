(function (app) {
	
    'use strict';

    function dashboardCtrl($scope, $routeParams, $log, vMixService) {
		
		$scope.title = 'Dashboard';
		
		$scope.activate = function(){
			$log.debug('View activated');
			vMixService.getStatus().then(function(data){
				$log.debug('status', data);
                $scope.vmix = data;
			});
		}	
	}
	
	dashboardCtrl.$inject = ['$scope', '$routeParams', '$log', 'vMixService'];
	
	app.controller("DashboardCtrl", dashboardCtrl);
 
})(app);