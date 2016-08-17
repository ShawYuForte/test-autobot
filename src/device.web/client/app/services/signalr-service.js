(function (forte) {

    function signalrService($rootScope, $log) {

        $.connection.hub.url = '/signalr';
        //debugger;
        // Declare a proxy to reference the hub.
        var notifications = $.connection.notificationHub;
        if (notifications) {

            notifications.client.sendLogEvent = function (event) {
                event = angular.fromJson(event);
                $log.debug('Log event', event);
            }

            $log.debug('signalR:starting hub');
            $.connection.hub.start().done(function () {
                $log.debug('signalR:hub started');
            }).fail(function (err) {
                $log.error('signalR:hub not started!', err);
            });
        }

        return {};
    }

    signalrService.$inject = ['$rootScope', '$log'];

    forte.app.factory("SignalrService", signalrService);

})(forte);