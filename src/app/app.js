var app = undefined;

(function (angular) {
    "use strict";

    app = angular.module("automation", ["ngRoute", "ngResource"]);

    app.config([
        "$routeProvider", function ($routeProvider) {

            $routeProvider
                .when("/dashboard", {
                    templateUrl: "app/views/dashboard.html",
                    controller: "DashboardCtrl"
                }).otherwise({
                    redirectTo: "/dashboard"
                });

        }
    ]);

    app.value("_config", {});
    app.value("_data", {});

    app.run([
        "$log", "$http", "_config", function ($log, $http, config) {
            $log.debug("App started, config", config);

            $http.get("app/data/config.json")
                .then(function (response) {
                    angular.copy(response.data, config);
                }, function (err) {
                    $log.error("Could not load configuration", err);
                });

        }
    ]);

})(angular);