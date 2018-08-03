
$().ready(function () {
    $('.input-daterange').datepicker({
        language: 'zh-CN',//显示中文
        format: 'yyyy-mm-dd',
    });
    //使用datarangepickrer控件 modify by xue lei on 2018-8-2
    //$('#txtEndtime').val(moment().format("YYYY-MM-DD"));
    //$('#txtBegintime').val(moment().subtract(1, 'days').format("YYYY-MM-DD"));
    $('#ddlWorkshop').val('');
    $('#ddlWorkshop').select2({
        placeholder: "车间",
        allowClear: true
    });
    Daterangeini();
    //QueryddlStatus();
    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

})

/*显示daterangepicker*/
function Daterangeini() {
    $('#rangetime').val(moment().subtract('hours', 24).format('YYYY-MM-DD') + ' 08:00:00' + ' - ' + moment().format('YYYY-MM-DD') + ' 08:00:00');

    $('#rangetime').daterangepicker(  
        {
            timePicker: true,
            timePicker24Hour:true,
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
                "firstDay": 1
            }
        });
}

function QueryddlStatus() { 
    var promiseweld = $.ajax({
        url: '../api/QC/QueryStatus',
        type: 'post',
        cache: true,
        async: true,
    });
    promiseweld.done(function (r) {
        $.each(r, function (index, value) {
            $("#ddlStatus").append("<option value='" + value.visit_type + "'>" + value.visit_type_desc + "</option>");
        })
        $('#ddlStatus').val('');
        $('#ddlStatus').select2({
            placeholder: "状态",
            allowClear: true
        });
    });
    promiseweld.fail(function (error) {
        console.log(error)
        alert(error);
    });
}


$('#btnQuery').click(function () {

    $("#tbtestdata").bootstrapTable('refresh');

})
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tbtestdata').bootstrapTable({
            url: '../api/TestDataDetail/QueryData',         //请求后台的URL（*）
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
            columns: [{
                checkbox: true
            }, {
                field: 'wks_visit_date',
                title: '测试时间'
            }, {
                field: 'serial_nbr',
                title: '组件序列号'
            },  {
                field: 'wks_id',
                title: '机台号'
            },
            {
                field: 'workorder',
                title: '工单号'
            },
            {
                field: 'cell_uop',
                title: '电池片功率'
            },
            {
                field: 'cell_eff',
                title: '电池片效率'
            }, {
                field: 'pmax',
                title: 'pmax'
            },
            {
                field: "voc",
                title: "voc"
            },
            {
                field: "isc",
                title: "isc"
            },
            {
                field: "ff",
                title: "ff"
            },
            {
                field: "vpm",
                title: "vpm"
            }
                ,
            {
                field: "ipm",
                title: "ipm"
            },
            {
                field: "rs",
                title: "rs"
            },
            {
                field: "rsh",
                title: "rsh"
            },
            {
                field: "eff",
                title: "eff"
            },
            {
                field: "env_temp",
                title: "env_temp"
            },
            {
                field: "surf_temp",
                title: "surf_temp"
            },
            {
                field: "temp",
                title: "temp"
            },
            {
                field: "ivfile_path",
                title: "ivfile_path"
            },
            {
                field: "product_code",
                title: "组件类型"
            },
            {
                field: "cell_part_nbr",
                title: "电池料号"
            },
            {
                field: "cell_lot_nbr",
                title: "电池批号"
            },
            {
                field: "cell_supplier_code",
                title: "电池供应商"
            },
            {
                field: "glass_part_nbr",
                title: "玻璃料号"
            },
            {
                field: "glass_supplier_code",
                title: "玻璃供应商"
            },
            {
                field: "eva_part_nbr",
                title: "EVA料号"
            },
            {
                field: "eva_supplier_code",
                title: "EVA供应商"
            },
            {
                field: "eva_lot_nbr",
                title: "EVA批号"
            },
            {
                field: "bks_part_nbr",
                title: "背板料号"
            },
            {
                field: "bks_supplier_code",
                title: "背板供应商"
            },
            {
                field: "bks_lot_nbr",
                title: "背板批号"
            },
            {
                field: "frame_part_nbr",
                title: "型材料号"
            },
            {
                field: "frame_supplier_code",
                title: "型材供应商"
            },
            {
                field: "frame_lot_nbr",
                title: "型材批号"
            },
            {
                field: "jbox_part_nbr",
                title: "接线盒料号"
            },
            {
                field: "jbox_supplier_code",
                title: "接线盒供应商"
            },
            {
                field: "jbox_lot_nbr",
                title: "接线盒批号"
            },
            {
                field: "huiliu_part_nbr",
                title: "汇流条料号"
            },
            {
                field: "huiliu_supplier_code",
                title: "汇流条供应商"
            },
            {
                field: "huiliu_lot_nbr",
                title: "汇流条批号"
            },
            {
                field: "hulian_part_nbr",
                title: "汇联条批号"
            },
            {
                field: "hulian_supplier_code",
                title: "汇联条供应商"
            }
            ]
        });
    };

    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {   //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,   //页面大小
            offset: params.offset,  //页码
            //departmentname: $("#txt_search_departmentname").val(),
            //statu: $("#txt_search_statu").val()
            begintime: $('#rangetime').val().substr(0, 19),//$('#txtBegintime').val(),
            endtime: $('#rangetime').val().split(' - ')[1],
            workshop: $('#ddlWorkshop').val(),
            workorder: $('#txtWO').val(),
            serialno: $('#LotID').val(),
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

    if ($('#txtBegintime').val() == '' || $('#txtEndtime').val() == '') {
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

    //window.open('../Report/ExportToExcel?bt=2018-10-10');$('#LotID').val()
    var a = $("<a href='../Report/TestDataDetailExcel/" + ($('#LotID').val() == '' ? 'Null' : $('#LotID').val()) + "/" + ($('#txtWO').val() == '' ? 'Null' : $('#txtWO').val()) + "/" + ($('#txtBegintime').val() == '' ? 'Null' : $('#txtBegintime').val()) + "/" + ($('#txtEndtime').val() == '' ? 'Null' : $('#txtEndtime').val()) + "/" + ( !$('#ddlWorkshop').val() ? 'Null' : $('#ddlWorkshop').val())+ "' target='_blank'></a>").get(0);
    var e = document.createEvent('MouseEvents');
    e.initEvent('click', true, true);
    a.dispatchEvent(e);

})




