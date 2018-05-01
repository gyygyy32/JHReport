app.controller('StockInCtrl', function ($scope, $http) {
    $('#WIPDetailModal').on('shown.bs.modal', function () {
        var $table = $('#tbwipdetail');
       // var data = [{ 'id': 1, 'lotid': '001', 'length': '111', 'timeinterval': '123' }];

        //查询wip数据
        $http.post("/Webservice/WebService.asmx/QueryWIPDetail"
            , { "wo": $scope.wo, "product": $scope.producttype, "worksite": $scope.worksiteid }
        )
            .then(function successCallback(res) {
                //        var a = $('#tbwipdetail').DataTable();
                //a.clear().draw();
                var data = JSON.parse(res.data.d);
                var $table = $('#tbwipdetail');
                //var data = [{ 'id': 1, 'lotid': '001', 'length': '111', 'timeinterval': '123' }];
                //$table.bootstrapTable({ data: data});
                $table.bootstrapTable('destroy');
                $table.bootstrapTable({data:data} );

            }, function errorCallback(res) {
                alert("error in get wipdetail info")
            });

        
    });
    //$('#example').DataTable();

    $scope.ShowWIPDetailWO = function (wo, producttype, colno) {
        //var a = $('#tbwipdetail').DataTable();
        //a.clear().draw();
        //alert('工单：' + wo + ',产品类型：' + producttype + "列数：" + colno);

        if (wo == '') {
            colno += 1;
        }

        var worksiteid;
        switch (colno) {
            case 4:
                worksiteid = 'M35'//AG涂布
                break;
            case 5:
                worksiteid = 'M37'//AG涂布检验
                break;
            case 6:
                worksiteid = 'M40'//UV背涂
                break;
            case 7:
                worksiteid = 'M42'//UV背涂检验
                break;
            case 8:
                worksiteid = 'M45'//UV成型
                break;
            case 9:
                worksiteid = 'M47'//UV成型检验
                break;
            case 10:
                worksiteid = 'M50'//贴膜
                break;
            case 11:
                worksiteid = 'M52'//贴膜检验
                break;
            case 12:
                worksiteid = 'M55'//分条
                break;
            case 13:
                worksiteid = 'M57'//分条检验
                break;
            case 14:
                worksiteid = 'M60'//包装
                break;
        }
        $scope.wo = wo;
        $scope.producttype = producttype;
        $scope.worksiteid = worksiteid;
        

        //$scope.$on("ngRepeatFinished", function (repeatFinishedEvent, element) {
        //    var repeatId = element.parent().attr("repeat-id");
        //    switch (repeatId) {
        //        case "workcenterTasks":
        //            IniDatatable();
        //            break;
        //    }
        //})
        //$scope.tb;
        

        $('#WIPDetailModal').modal();
    }




    var tableini = function () {


        if ($('#tbwipdetail').hasClass('dataTable')) {
            //var oldTable = $('#tbwipdetail').dataTable();
            //oldTable.clear().draw(); //清空一下table
            //oldTable.Destroy(); //还原初始化了的dataTable
            //$('#tbwipdetail').empty();
        }
        else { var a = $('#tbwipdetail').dataTable({ "retrieve": true }); }


        //a.clear().draw();
        //a = $('#tbwipdetail').DataTable();


    }

    $scope.$on("ngRepeatFinished", function (repeatFinishedEvent, element) {
        var repeatId = element.parent().attr("repeat-id");
        switch (repeatId) {
            case "wipdetail":
                tableini();
                break;
        };
    })
})