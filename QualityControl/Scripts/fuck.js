
function DelA(id) {

    var the = $("." + id);
    the.remove();
    var n = "";
    ///从数组中删除
    for (var i = 0; i < files.length; i++) {
        if (files[i].id == id) {
            n = files[i].name2;
            files.splice(i, 1);


            break;
        }
    }
    ///从服务器删除
    delname.push(n);
}
                              