
$().ready(function () {
    $('.input-daterange').datepicker({
        language: 'zh-CN',//显示中文
        format: 'yyyy-mm-dd',
    });
    //使用datarangepickrer控件 modify by xue lei on 2018-8-2
    //$('#txtEndtime').val(moment().format("YYYY-MM-DD"));
    //$('#txtBegintime').val(moment().subtract(1, 'days').format("YYYY-MM-DD"));
    //$('#ddlWorkshop').val('');
    //$('#ddlWorkshop').select2({
    //    placeholder: "车间",
    //    allowClear: true
    //});
    Daterangeini();
    //QueryddlStatus();
    //1.初始化Table
    $('#wodaterange').tooltip();

})

/*显示daterangepicker*/
function Daterangeini() {
    //$('#rangetime').val(moment().subtract('hours', 24).format('YYYY-MM-DD') + ' 08:00:00' + ' - ' + moment().format('YYYY-MM-DD') + ' 08:00:00');


    $('#rangetime').daterangepicker(
        {
            //autoUpdateInput: false,
            timePicker: true,
            timePicker24Hour: true,
            'locale': {
                "format": 'YYYY-MM-DD hh:mm:ss',
                "separator": " - ",
                "applyLabel": "确定",
                "cancelLabel": "取消",
                "fromLabel": "起始时间",
                "toLabel": "结束时间'",
                "customRangeLabel": "自定义",
                "weekLabel": "W",
                "daysOfWeek": ["日", "一", "二", "三", "四", "五", "六"],
                "monthNames": ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
                "firstDay": 1,
                //"cancelLabel": '清除'
            }
        });
    $('#rangetime').val('');
    $('#rangetime').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
    });
}

//function QueryddlStatus() {
//    var promiseweld = $.ajax({
//        url: '../api/QC/QueryStatus',
//        type: 'post',
//        cache: true,
//        async: true,
//    });
//    promiseweld.done(function (r) {
//        $.each(r, function (index, value) {
//            $("#ddlStatus").append("<option value='" + value.visit_type + "'>" + value.visit_type_desc + "</option>");
//        })
//        $('#ddlStatus').val('');
//        $('#ddlStatus').select2({
//            placeholder: "状态",
//            allowClear: true
//        });
//    });
//    promiseweld.fail(function (error) {
//        console.log(error)
//        alert(error);
//    });
//}


$('#btnQuery').click(function () {
    //检查查询参数
    if ($('#rangetime').val() == ''
        && $('#txtWO').val() == ''
        && $('#txtSales').val() == ''
        && $('#txtCustomer').val() == ''
    ) {
        //alert('请输入查询参数');
        alertinfo($('#boxbody'), '请输入查询参数');
        return;
    }

    //时间范围卡关 不能超过一周
    var bt = moment($('#rangetime').val().substr(0, 19));
    var et = moment($('#rangetime').val().split(' - ')[1]);
    if (et.diff(bt, 'days') > 7) {
        alertinfo($('#boxbody'), '时间范围不能超过7天');
        return;
    }


    //$("#tbtestdata").bootstrapTable('refresh');
    var oTable = new TableInit();
    oTable.Init();
})
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tbWOFinishStatus').bootstrapTable('destroy').bootstrapTable({
            url: '../api/WOFinishStatus/QueryInfo',         //请求后台的URL（*）
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
            exportHiddenCells: true,
            //onLoadSuccess: function (data) {
            //    var data = $('#tbqc').bootstrapTable('getData', true);
            //    //合并单元格
            //    mergeCells(data, "serial_nbr", 1, $('#tbqc'));
            //    mergeCells(data, "schedule_nbr", 1, $('#tbqc'));

            //},
            columns: [[{
                checkbox: true,
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                field: 'create_date',
                title: '工单日期',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                field: 'firstlottime',
                title: '生码日期',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                field: 'customer',
                title: '客户',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: 'sale_order',
                title: '订单号',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: 'workorder',
                title: '工单号',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: 'firstlot',
                title: '生产批次',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: 'product_code',
                title: '组件功率',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                field: 'plan_qty',
                title: '工单数量',
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: "laminationqty",
                title: "叠层数量",
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: "cell_supplier_desc",
                title: "电池片厂家",
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                field: "cellgrade",
                title: "电池片挡位",
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            },
            {
                title: "组件等级分布",
                colspan: 4,
                align: 'center',
                valign: 'middle'

            }
                ,
            {
                title: "测试功率",
                colspan: 3,
                align: 'center',
                valign: 'middle'

            }
            ],
            [
                {
                    field: "A",
                    title: "A",
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: "B",
                    title: "B",
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: "C",
                    title: "C",
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: "scrapqty",
                    title: "报废",
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: "max",
                    title: "最大值",
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: "min",
                    title: "最小值",
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: "avg",
                    title: "平均值",
                    align: 'center',
                    valign: 'middle'
                }
            ]]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            //departmentname: $("#txt_search_departmentname").val(),
            //statu: $("#txt_search_statu").val()
            bt: $('#rangetime').val().substr(0, 19),//$('#txtBegintime').val(),
            et: $('#rangetime').val().split(' - ')[1],
            customer: $('#txtCustomer').val(),
            wo: $('#txtWO').val(),
            sales: $('#txtSales').val(),
        };
        return temp;
    };
    return oTableInit;
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
    if ($('#rangetime').val() == '') {
        alert('请输入查询参数');
        return;
    }

    var bt = $('#rangetime').val().substr(0, 19);
    var et = $('#rangetime').val().split(' - ')[1];
    //if ($('#txtBegintime').val() == '' || $('#txtEndtime').val() == '') {
    //    alert('请输入查询参数');
    //    return;
    //}
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

    //window.open('../Report/ExportToExcel?bt=2018-10-10');$('#LotID').val()
    var a = $("<a href='../Report/WOFinishStatusExcel/" + ($('#LotID').val() == '' ? 'Null' : $('#LotID').val()) + "/" + ($('#txtWO').val() == '' ? 'Null' : $('#txtWO').val()) + "/" + (bt == '' ? 'Null' : encodeURI(bt)) + "/" + (et == '' ? 'Null' : encodeURI(et)) + "/" + (!$('#ddlWorkshop').val() ? 'Null' : $('#ddlWorkshop').val()) + "' target='_blank'></a>").get(0);
    var e = document.createEvent('MouseEvents');
    e.initEvent('click', true, true);
    a.dispatchEvent(e);

})




