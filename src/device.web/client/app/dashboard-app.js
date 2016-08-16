(function () {
    "use strict";

    var forte = window.forte = {};
    //angular_dependencies.push('ngCookies');
    //angular_dependencies.push('ngSanitize');
    //angular_dependencies.push('angular-loading-bar');
    //angular_dependencies.push('kendo.directives');
    //angular_dependencies.push('ngAnimate');
    //angular_dependencies.push('ngMessages');

    forte.app = angular.module('forte-dashboard', ['ngRoute', 'ngResource']);

    //Declare moment.js globals as an angular constant for DI
    forte.app.constant('moment', window.moment);

    forte.app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/dashboard',
            {
                templateUrl: '/app/views/dashboard.html',
                controller: 'DashboardCtrl'
            })
            .when('/settings',
            {
                templateUrl: '/app/views/settings.html',
                controller: 'SettingsCtrl'
            })
            .otherwise({
                redirectTo: '/dashboard'
            });
    }]);

})();