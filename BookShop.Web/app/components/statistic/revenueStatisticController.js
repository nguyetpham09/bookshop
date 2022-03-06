(function (app) {
    app.controller('revenueStatisticController', revenueStatisticController);

    revenueStatisticController.$inject = ['$scope', 'apiService', 'notificationService','$filter'];

    function revenueStatisticController($scope, apiService, notificationService,$filter) {
        $scope.tabledata = [];
        $scope.labels = [];
        $scope.series = ['Doanh số', 'Lợi nhuận'];
        $scope.fromDate = '01/01/2022';
        $scope.toDate = '01/10/2022';
        $scope.search = search;

        $scope.chartdata = [];

        function search() {
            getStatistic();
        }

        function getStatistic() {
            var from, to;
            if (typeof $scope.fromDate === 'string' || $scope.fromDate instanceof String) {
                from = $scope.fromDate;
            }
            // it's a string
            else {
                from = $scope.fromDate?.toLocaleDateString('en-us');
            }

            if (typeof $scope.toDate === 'string' || $scope.toDate instanceof String) {
                to = $scope.toDate;
            }
            // it's a string
            else {
                to = $scope.toDate?.toLocaleDateString('en-us');
            }

            var config = {
                param: {
                    //mm/dd/yyyy
                    fromDate: from,
                    toDate: to
                }
            }
            apiService.get('api/statistic/getrevenue?fromDate=' + config.param.fromDate + "&toDate=" + config.param.toDate, null, function (response) {
                $scope.tabledata = response.data;
                var labels = [];
                var chartData = [];
                var revenues = [];
                var benefits = [];
                $.each(response.data, function (i, item) {
                    labels.push($filter('date')(item.Date,'dd/MM/yyyy'));
                    revenues.push(item.Revenues);
                    benefits.push(item.Benefit);
                });
                chartData.push(revenues);
                chartData.push(benefits);

                $scope.chartdata = chartData;
                $scope.labels = labels;
            }, function (response) {
                notificationService.displayError('Không thể tải dữ liệu');
            });
        }

        getStatistic();
    }

})(angular.module('tedushop.statistics'));