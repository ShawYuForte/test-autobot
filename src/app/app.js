var app = angular
  .module('automation', ['ngRoute','ngResource','xml'])
  .config(function ($httpProvider) {
    $httpProvider.interceptors.push('xmlHttpInterceptor');
  })
  .config(['$routeProvider', function ($routeProvider) {
	$routeProvider
		.when('/dashboard', {
			templateUrl: '/app/views/dashboard.html',
        })
        .otherwise('/forbidden', {
			redirectTo: '/dashboard',
		})
    }]);
