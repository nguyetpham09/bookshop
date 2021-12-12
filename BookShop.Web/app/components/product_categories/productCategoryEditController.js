(function (app) {
    app.controller('productCategoryEditController', productCategoryEditController);

    productCategoryEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService']

    function productCategoryEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.productCategory = {
            UpdatedDate: new Date(),
            Status: true,
        }

        //$scope.parentCategories = []
        
        function loadProductCategory() {
            apiService.get('api/productcategory/' + $stateParams.id, null, function (result) {
                $scope.productCategory = result.data;

            }, function () {
                console.log('Cannot get product category');
            })
        }

        $scope.UpdateProductCategory = UpdateProductCategory;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }

        function UpdateProductCategory() {
            apiService.put('api/category/update', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + 'đã được cập nhật');
                $state.go('product_categories')
            }, function () {
                notificationService.displaySuccess('Cập nhật không thành công');
            })
        }

        function loadParentCategory() {
            apiService.get('api/category/getallparents', null, function (result) {
                $scope.parentCategories = result.data;

            }, function () {
                console.log('Cannot get list parent');
            })
        }

        loadParentCategory();
        loadProductCategory();
    }
})(angular.module('bookshop.product_categories'))