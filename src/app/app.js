var app = undefined;

(function () {
    "use strict";
	
	app = angular.module('automation', ['ngRoute','ngResource']);
	
//	app.config(function ($httpProvider) {
//		$httpProvider.interceptors.push('xmlHttpInterceptor');
//	});
	
	app.config(['$routeProvider', function ($routeProvider) {
	  
		$routeProvider
			.when('/dashboard', {
				templateUrl: 'app/views/dashboard.html',
				controller: 'DashboardCtrl'
			}).otherwise({
				redirectTo: '/dashboard'
			});
	}]);

})();