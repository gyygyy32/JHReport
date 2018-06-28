
$().ready(function () {
    
    //1.初始化Table
    //var oTable = new TableInit();
    //oTable.Init();

})

$('#btnQuery').click(function () {
    if ($('#txtLot').val() == '' && $('#txtSalesorder').val() == '') {
        alert('请输入查询参数');
        return;
    }
    $("#tbqc").bootstrapTable('destory');
    //$("#tbqc").bootstrapTable('refresh');
    var oTable = new TableInit();
    oTable.Init();
})
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tbjk').bootstrapTable({
            url: '../api/JK/QueryInfo',         //请求后台的URL（*）
            method: 'post',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
            contentType: 'application/json',
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 20,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: true,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            //height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            //uniqueId: "aiid",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            showExport: true,                     //是否显示导出
            exportDataType: "all",              //basic', 'all', 'selected'.
            exportTypes: ['json', 'xml', 'csv', 'txt', 'sql', 'excel'],


            onLoadSuccess: function (data) {
                //var data = $('#tbqc').bootstrapTable('getData', true);
                ////合并单元格
                //mergeCells(data, "serial_nbr", 1, $('#tbqc'));
                //mergeCells(data, "schedule_nbr", 1, $('#tbqc'));

            },
            columns: [{
                checkbox: true
            }, {
                    field: 'TTIME',
                    title: 'TTIME'
            }, {
                    field: 'LOT_NUM',
                    title: 'LOT_NUM'
            }, {
                    field: 'WORK_ORDER_NO',
                    title: 'WORK_ORDER_NO'
            }, {
                    field: 'DEVICENUM',
                    title: 'DEVICENUM'
            },
            {
                field: 'AMBIENTTEMP',
                title: 'AMBIENTTEMP'
            },
            {
                field: 'INTENSITY',
                title: 'INTENSITY'
            },
            {
                field: 'FF',
                title: 'FF'
            }, {
                field: 'EFF',
                title: 'EFF'
            },
            {
                field: "PM",
                title: "PM"
            },
            {
                field: "ISC",
                title: "ISC"
                },
                {
                    field: "IPM",
                    title: "IPM"
                },
                {
                    field: "VOC",
                    title: "VOC"
                },
                {
                    field: "VPM",
                    title: "VPM"
                },
                {
                    field: "SENSORTEMP",
                    title: "SENSORTEMP"
                },
                {
                    field: "VC_CELLEFF",
                    title: "VC_CELLEFF"
                },
                {
                    field: "DEC_CTM",
                    title: "DEC_CTM"
                },
                {
                    field: "RS",
                    title: "RS"
                },
                {
                    field: "RSH",
                    title: "RSH"
                },
                {
                    field: "SUPPLIER_NAME",
                    title: "SUPPLIER_NAME"
                },
                {
                    field: "RESULT",
                    title: "RESULT"
                },
                {
                    field: "TYPE",
                    title: "TYPE"
                },
                {
                    field: "IV_TEST_KEY",
                    title: "IV_TEST_KEY"
                },
                {
                    field: "PALLET_NO",
                    title: "PALLET_NO"
                },
                {
                    field: "PRO_LEVEL",
                    title: "PRO_LEVEL"
                },
                {
                    field: "CIR_NO",
                    title: "CIR_NO"
                },
                {
                    field: "SALE_ORDER_NO",
                    title: "SALE_ORDER_NO"
                }]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            //departmentname: $("#txt_search_departmentname").val(),
            //statu: $("#txt_search_statu").val()
            salesorder: $('#txtSalesorder').val(),
            lot: $('#txtLot').val(),
        };
        return temp;
    };
    return oTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        //初始化页面上面的按钮事件
    };

    return oInit;
};

/**
 * 合并单元格
 * @param data  原始数据（在服务端完成排序）
 * @param fieldName 合并属性名称
 * @param colspan   合并列
 * @param target    目标表格对象
 */
function mergeCells(data, fieldName, colspan, target) {
    //声明一个map计算相同属性值在data对象出现的次数和
    var sortMap = {};
    for (var i = 0; i < data.length; i++) {
        for (var prop in data[i]) {
            if (prop == fieldName) {
                var key = data[i][prop]
                if (sortMap.hasOwnProperty(key)) {
                    sortMap[key] = sortMap[key] * 1 + 1;
                } else {
                    sortMap[key] = 1;
                }
                break;
            }
        }
    }
    for (var prop in sortMap) {
        console.log(prop, sortMap[prop])
    }
    var index = 0;
    for (var prop in sortMap) {
        var count = sortMap[prop] * 1;
        $(target).bootstrapTable('mergeCells', { index: index, field: fieldName, colspan: colspan, rowspan: count });
        index += count;
    }
}

//导出excel
$('#btnExportExcel').click(function () {
    console.log("clicked");

    if ($('#txtLot').val() == '' && $('#txtSalesorder').val() == '') {
        alert('请输入查询参数');
        return;
    }
    //var promiseweld = $.ajax({
    //    url: '../Report/ExportToExcel',
    //    type: 'post',
    //    cache: true,
    //    async: true,
    //});

    //promiseweld.fail(function (error) {
    //    console.log(error)
    //    alert(error);
    //});

    //window.open('../Report/ExportToExcel?bt=2018-10-10');
    var a = $("<a href='../Report/JKExcel/" + $('#txtSalesorder').val() + "/" + $('#txtLot').val()+"' target='_blank'></a>").get(0);
    var e = document.createEvent('MouseEvents');
    e.initEvent('click', true, true);
    a.dispatchEvent(e);

})



