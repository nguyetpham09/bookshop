/// <reference path="../assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('bookshop',
        ['bookshop.products',
         'bookshop.product_categories',
         'bookshop.common'])
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('home', {
            url: "/admin",
            templateUrl: "/app/components/home/homeView.html",
            controller: "homeController"
        });
        $urlRouterProvider.otherwise('/admin')
    }
})();