app.controller('RunCardCtrl', function ($scope, $http) {
    $scope.QueryLot = function () {
        //$scope.cell_supplier_desc = "test";
        var promise = $.ajax({
            url: '../api/RunCard/QueryLotID',
            type: 'post',
            cache: false,
            async: true,
            contentType: "application/json",
            data: JSON.stringify({                
                lotid: $("#LotID").val()  
            }),
        });
        promise.done(function (r) {
            $scope.lotidinfo = r[0];
            $scope.$apply();
        });
        promise.fail(function (error) {
            console.log(error)
            alert(error);
        });

        //焊接信息
        var promiseweld = $.ajax({
            url: '../api/RunCard/WeldStationInfo',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseweld.done(function (r) {
            $scope.weldinfo = r;
            $scope.$apply();
        });
        promiseweld.fail(function (error) {
            console.log(error)
            alert(error);
        });
    }
})