function alertinfo(e,info) {
    var content = "<div class='alert alert-warning alert-dismissible'><button type ='button' class='close' data-dismiss='alert' aria-hidden='true' >×</button><h4><i class='icon fa fa-warning'></i> Alert!</h4>"+info+"</div >";
    e.prepend(content);
}