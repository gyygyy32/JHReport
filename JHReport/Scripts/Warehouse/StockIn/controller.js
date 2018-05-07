app.controller('StockInCtrl', function ($scope, $http) {

    $.init = function () {
        QueryStock();
    }

    //查询库位
    function QueryStock() {
        var promise = $.ajax({
            url: '../api/StockIn/Stock',
            type: 'post',
            cache: false,
            async: true,
            contentType: "application/json",
            //data: JSON.stringify({
            //    lotid: $("#LotID").val()
            //}),
        });
        promise.done(function (r) {
            $scope.stock = r;
            var options = '';
            $.each(r, function (i, v) {
                options += "<option value='" + v.CodeID + "'>" + v.StockName + "</option>";
            })

            $('#ddlStock').html(options);

            
        });
        promise.fail(function (error) {
            console.log(error)
            alert(error);
        });
    }
})