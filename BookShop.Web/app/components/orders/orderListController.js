(function (app) {
    app.controller('orderListController', orderListController);

    orderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function orderListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.orders = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getOrders = getOrders;
        $scope.keyword = '';
        $scope.search = search;

        function search() {
            getOrders();
        }

        function getOrders(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 20
                }
            }
            apiService.get('/api/product/getallorder', config, function (result) {
                $scope.orders = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log('Load order failed.');
            });
        }

        
        $scope.getOrders();
    }
})(angular.module('tedushop.orders'));