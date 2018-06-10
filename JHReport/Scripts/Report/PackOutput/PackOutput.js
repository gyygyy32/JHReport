
$().ready(function () {
    $('.input-daterange').datepicker({
        language: 'zh-CN',//显示中文
        format: 'yyyy-mm-dd',
    });
    $('#txtEndtime').val(moment().format("YYYY-MM-DD"));
    $('#txtBegintime').val(moment().subtract(1, 'days').format("YYYY-MM-DD"));
    $('#ddlWorkshop').val('');
    $('#ddlWorkshop').select2({
        placeholder: "车间",
        allowClear: true
    });

    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

})
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

    $("#tbqc").bootstrapTable('refresh');

})
var TableInit = function () {
    var oTableInit = new Object();
    //初始化Table
    oTableInit.Init = function () {
        $('#tbqc').bootstrapTable({
            url: '../api/PackOutput/QueryInfo',         //请求后台的URL（*）
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
                field: 'pallet_nbr',
                title: '托盘号'
            }, {
                field: 'container_nbr',
                title: '柜号'
            }, {
                field: 'serial_nbr',
                title: '组件序列号'
            }, {
                field: 'workorder',
                title: '工单号'
            },
            {
                field: 'power_grade',
                title: '功率档位'
            },
            {
                field: 'current_grade',
                title: '电流档位'
            },
            {
                field: 'product_code',
                title: '装配件号'
            }, {
                field: 'el_grade',
                title: 'EL等级'
            },
            {
                field: "final_grade",
                title: "外观等级"
            },
            {
                field: "pack_date",
                title: "封箱时间"
            },
            {
                field: "descriptions",
                title: "功率组"
            },
            {
                field: "pallet_status_desc",
                title: "是否入库"
            },
            {
                field: "shift_type",
                title: "班次"
            },
            {
                field: "pmax",
                title: "Pmax"
            },
            {
                field: "voc",
                title: "VOC"
            },
            {
                field: "ISC",
                title: "ISC"
            },
            {
                field: "ff",
                title: "FF"
            },
            {
                field: "vpm",
                title: "VPM"
            },
            {
                field: "ipm",
                title: "IPM"
            },
            {
                field: "rs",
                title: "RS"
            },
            {
                field: "rsh",
                title: "RSH"
            },
            {
                field: "rsh",
                title: "RSH"
            },
            {
                field: "eff",
                title: "EFF"
            },
            {
                field: "env_temp",
                title: "ENV_TEMP"
            },
            {
                field: "surf_temp",
                title: "SURF_TEMP"
            },
            {
                field: "temp",
                title: "TEMP"
            },
            {
                field: "ivfile_path",
                title: "IVFILE_PATH"
            },
            {
                field: "pack_seq",
                title: "包装顺序"
            },
            {
                field: "check_nbr",
                title: "报检单号"
            },
            {
                field: "cell_supplier",
                title: "电池片厂商"
            },
            {
                field: "",
                title: "电池片档位"
            },
            {
                field: "cell_uop",
                title: "单片功率"
            },
            {
                field: "fff",
                title: "转换效率"
            },
            {
                field: "eva_supplier_code",
                title: "EVA厂商"

            },
            {
                field: "eva_thickness",
                title: "普通EVA规格"
            },
            {
                field: "eva_lot_nbr",
                title: "普通EVA批号"
            },
            {
                field: "bks_supplier_code",
                title: "背板厂商"
            },
            {
                field: "bks_thickness",
                title: "背板规格"
            },
            {
                field: "bks_lot_nbr",
                title: "背板批号"
            },
            {
                field: "glass_supplier_code",
                title: "玻璃供应商"
            }, {
                field: "glass_thickness_desc",
                title: "玻璃规格"
            },
            {
                field: "glass_lot_nbr",
                title: "玻璃批号"
            },
            {
                field: "jbox_supplier_code",
                title: "接线盒供应商"
            },
            {
                field: "jbox_model_desc",
                title: "接线盒规格"
            },
            {
                field: "jbox_lot_nbr",
                title: "接线盒批号"
            },
            {
                field: "frame_supplier_code",
                title: "型材供应商"
            },
            {
                field: "frame_thickness_desc",
                title: "型材规格"
            },
            {
                field: "frame_lot_nbr",
                title: "型材批号"
            },
            {
                field: "customer",
                title: "客户"
            },
            {
                field: "product_code_original",
                title: "产品组"
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
            begintime: $('#txtBegintime').val(),
            endtime: $('#txtEndtime').val(),
            workshop: $('#ddlWorkshop').val(),
            containerno: $('#txtContainerNo').val(),
            lotno: $('#txtLotID').val(),
            palletno: $('#txtPalletNo').val(),
            checkno: $('#txtCheckNo').val(),

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




