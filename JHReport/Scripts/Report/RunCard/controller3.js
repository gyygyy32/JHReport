﻿app.controller("RunCardCtrl", function (a, n) { a.QueryLot = function () { $("#tbRuncard").show(1e3); var n = $.ajax({ url: "../api/RunCard/QueryLotID", type: "post", cache: !1, async: !0, contentType: "application/json", data: JSON.stringify({ lotid: $("#LotID").val() }) }); n.done(function (n) { a.lotidinfo = void 0, a.lotidinfo = n[0], a.$apply() }), n.fail(function (a) { console.log(a), alert(a) }); var t = $.ajax({ url: "../api/RunCard/WeldStationInfo", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); t.done(function (n) { a.weldinfo = n[0], a.$apply() }), t.fail(function (a) { console.log(a), alert(a) }); var o = $.ajax({ url: "../api/RunCard/FrameBox", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); o.done(function (n) { a.FrameBoxinfo = _.find(n, function (a) { return "线盒" == a.part_type }), a.bar = _.find(n, function (a) { return "长型材" == a.part_type }), a.$apply() }), o.fail(function (a) { console.log(a), alert(a) }); var i = $.ajax({ url: "../api/RunCard/LaminationEVA", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); i.done(function (n) { a.LaminationEVAinfo = n[0], a.$apply() }), i.fail(function (a) { console.log(a), alert(a) }); var l = $.ajax({ url: "../api/RunCard/LaminationHighEVA", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); l.done(function (n) { a.LaminationHighEVAinfo = n[0], a.$apply() }), l.fail(function (a) { console.log(a), alert(a) }); var e = $.ajax({ url: "../api/RunCard/LaminationGlass", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); e.done(function (n) { a.LaminationGlassinfo = n[0], a.$apply() }), e.fail(function (a) { console.log(a), alert(a) }); var c = $.ajax({ url: "../api/RunCard/LaminationBack", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); c.done(function (n) { a.LaminationBackinfo = n[0], a.$apply() }), c.fail(function (a) { console.log(a), alert(a) }); var r = $.ajax({ url: "../api/RunCard/IV", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); r.done(function (n) { a.IVinfo = n[0], a.$apply() }), r.fail(function (a) { console.log(a), alert(a) }); var u = $.ajax({ url: "../api/RunCard/Pack", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); u.done(function (n) { a.Packinfo = n[0], a.$apply() }), u.fail(function (a) { console.log(a), alert(a) }); var f = $.ajax({ url: "../api/RunCard/ELAfterTest", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); f.done(function (n) { a.ELAfterTestinfo = n[0], a.$apply() }), f.fail(function (a) { console.log(a), alert(a) }); var p = $.ajax({ url: "../api/RunCard/ELAfterIV", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); p.done(function (n) { a.ELAfterIVinfo = n[0], a.$apply() }), p.fail(function (a) { console.log(a), alert(a) }); var d = $.ajax({ url: "../api/RunCard/ELBeforeLayup", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); d.done(function (n) { a.ELBeforeLayupinfo = n[0], a.$apply() }), d.fail(function (a) { console.log(a), alert(a) }); var y = $.ajax({ url: "../api/RunCard/Clean", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); y.done(function (n) { a.Cleaninfo = n[0], a.$apply() }), y.fail(function (a) { console.log(a), alert(a) }); var s = $.ajax({ url: "../api/RunCard/QCAfterLayup", type: "get", cache: !1, async: !0, data: { lotid: $("#LotID").val() } }); s.done(function (n) { a.QCAfterLayupinfo = n[0], a.$apply() }), s.fail(function (a) { console.log(a), alert(a) }) } }), app.filter("result", function () { return function (a) { return 0 == a ? "合格" : a > 0 ? "扣留" : void 0 } }), app.filter("line", function () { return function (a) { return "M01" == a.substring(0, 3) ? "A" : "M02" == a.substring(0, 3) ? "B" : void 0 } });