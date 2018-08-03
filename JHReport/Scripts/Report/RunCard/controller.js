app.controller('RunCardCtrl', function ($scope, $http) {
    $scope.QueryLot = function () {
        $('#tbRuncard').show(1000);
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

        /*装框接线盒 */
        var promiseFrameBox = $.ajax({
            url: '../api/RunCard/FrameBox',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseFrameBox.done(function (r) {
            $scope.FrameBoxinfo = r[0];
            $scope.$apply();
        });
        promiseFrameBox.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*叠层EVA */
        var promiseLaminationEVA = $.ajax({
            url: '../api/RunCard/LaminationEVA',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseLaminationEVA.done(function (r) {
            $scope.LaminationEVAinfo = r[0];
            $scope.$apply();
        });
        promiseLaminationEVA.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*叠层高透EVA */
        var promiseLaminationHighEVA = $.ajax({
            url: '../api/RunCard/LaminationHighEVA',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseLaminationHighEVA.done(function (r) {
            $scope.LaminationHighEVAinfo = r[0];
            $scope.$apply();
        });
        promiseLaminationHighEVA.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*叠层玻璃 */
        var promiseLaminationGlass = $.ajax({
            url: '../api/RunCard/LaminationGlass',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseLaminationGlass.done(function (r) {
            $scope.LaminationGlassinfo = r[0];
            $scope.$apply();
        });
        promiseLaminationGlass.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*叠层背板 */
        var promiseLaminationBack = $.ajax({
            url: '../api/RunCard/LaminationBack',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseLaminationBack.done(function (r) {
            $scope.LaminationBackinfo = r[0];
            $scope.$apply();
        });
        promiseLaminationBack.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*IV */
        var promiseIV = $.ajax({
            url: '../api/RunCard/IV',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseIV.done(function (r) {
            $scope.IVinfo = r[0];
            $scope.$apply();
        });
        promiseIV.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*包装 */
        var promisePack = $.ajax({
            url: '../api/RunCard/Pack',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promisePack.done(function (r) {
            $scope.Packinfo = r[0];
            $scope.$apply();
        });
        promisePack.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*测试后EL */
        var promiseELAfterTest = $.ajax({
            url: '../api/RunCard/ELAfterTest',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseELAfterTest.done(function (r) {
            $scope.ELAfterTestinfo = r[0];
            $scope.$apply();
        });
        promiseELAfterTest.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*层压前EL */
        var promiseELBeforeLayup= $.ajax({
            url: '../api/RunCard/ELBeforeLayup',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseELBeforeLayup.done(function (r) {
            $scope.ELBeforeLayupinfo = r[0];
            $scope.$apply();
        });
        promiseELBeforeLayup.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*清洗 */
        var promiseClean = $.ajax({
            url: '../api/RunCard/Clean',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseClean.done(function (r) {
            $scope.Cleaninfo = r[0];
            $scope.$apply();
        });
        promiseClean.fail(function (error) {
            console.log(error)
            alert(error);
        });

        /*层压后检验*/
        var promiseQCAfterLayup = $.ajax({
            url: '../api/RunCard/QCAfterLayup',
            type: 'get',
            cache: false,
            async: true,
            data: {
                lotid: $("#LotID").val()
            },
        });
        promiseQCAfterLayup.done(function (r) {
            $scope.QCAfterLayupinfo = r[0];
            $scope.$apply();
        });
        promiseQCAfterLayup.fail(function (error) {
            console.log(error)
            alert(error);
        });
    }
})

app.filter('result', function () {
    return function (text) {
        if (text == 0) {
            return "合格";
        }
        else if(text>0) {
            return "扣留";
        }
    }
})