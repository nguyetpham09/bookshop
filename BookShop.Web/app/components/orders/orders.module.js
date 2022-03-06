/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('tedushop.orders', ['tedushop.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('orders', {
                url: "/orders",
                parent: 'base',
                templateUrl: "/app/components/orders/orderListView.html",
                controller: "orderListController"
            }).state('order_edit', {
                url: "/order_edit",
                parent: 'base',
                templateUrl: "/app/components/products/orderEditView.html",
                controller: "orderEditController"
            });
    }
})();