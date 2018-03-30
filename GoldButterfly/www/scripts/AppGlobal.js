
var GBSetting = new Object({

    DefaultURL: '127.0.0.1:2331',
    ReadURL:'',
    GetServerURL: function () {
        return localStorage['serverurl'] || this.DefaultURL;
    }
   
});

var parmHelper = function () {
    this.KeyValueArray = new Array();
    this.Add = function (key, value) {
        var item = new Object();
        item.Key = key;
        item.Value = value;
        this.KeyValueArray[this.KeyValueArray.length] = item;
    }
}

var focusTextBox = function (id) {
    $('#' + id).textbox('textbox').focus(); 
}

var requestObject = function () {

    //var prm = { 'RequestName': 'a', 'RequestType': 0, 'Data': p, 'Msg': '' }
    this.RequestName='';
    this.RequestType = 0;
    this.Data = '';
    this.Msg = '';
}

function GetQueryString(name) {
    //var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    //var r = window.location.search.substr(1).match(reg);
    //if (r !== null) return unescape(r[2]); return null;
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
        return decodeURI(r[2]);   //对参数进行decodeURI解码
    return null;
}

function getParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null)
            return decodeURI(r[2]);   //对参数进行decodeURI解码
    return null;
 
}


var GBFile = (function () {

    var getPath = function () {
        return cordova.file.externalRootDirectory;
    }

    var SettingFile = 'setting.json';

    //function successHandler(dirEntry) {
    //    dirEntry.getFile(SettingFile, { create: true, exclusive: false }, function (fileEntry) {
    //        alert("Create the file: " + fileEntry.name + ', ' + fileEntry.fullPath);
    //        var dataObj = new Blob(['AA'], { type: 'text/plain' });
    //        writeFile(fileEntry, dataObj)
    //    }, errorGetFile);
    //}





    //将内容数据写入到文件中
    var WriteFile=function (fileEntry, dataObj,callBack) {
        //创建一个写入对象
        fileEntry.createWriter(function (fileWriter) {

            //文件写入成功
            fileWriter.onwriteend = function () {
                //alert("Successful file read...");
                callBack();
            };

            //文件写入失败
            fileWriter.onerror = function (e) {
                //alert("Failed file read: " + e.toString());
            };

            //写入文件
            fileWriter.write(dataObj);
        });
    }

    function errorGetDir() {
        GBMessager.Alert('errorGetDir');
    }
    function errorGetFile() {
        GBMessager.Alert('errorGetFile');
    }
    function errorResolve() {
        GBMessager.Alert('errorResolve');
    }


    var ReadFile =function (fileEntry,callBack) {
        fileEntry.file(function (file) {
            var reader = new FileReader();
            reader.onloadend = function () {
                callBack(this.result);
            };
            reader.readAsText(file);
        }, onErrorReadFile);
    }

    //读取文件失败响应
    function onErrorReadFile() {
        GBMessager.Alert("文件读取失败!");
    }


    //function fileExists(fileEntry) {
    //    readFile(fileEntry);
    //}

    //function fileDoesNotExist() {
    //    //readFile(fileEntry);
    //    alert('开始创建文件');
    //    window.resolveLocalFileSystemURL(getPath(), successHandler, errorResolve);
    //}

    var CreateFile=function (successHandler) {
        window.resolveLocalFileSystemURL(getPath(), successHandler, errorResolve);
    }

    var CheckIfFileExists=function (fileExists, fileDoesNotExist){

        window.resolveLocalFileSystemURL(getPath(), function (fileEntry) {
            //readFile(fileEntry);
            fileEntry.getFile(SettingFile, { create: false }, fileExists, fileDoesNotExist)

        }, function () { GBMessager.Alert('check失败') });
    }

    return{
        CheckIfFileExists: CheckIfFileExists,
        CreateFile: CreateFile,
        ReadFile: ReadFile,
        WriteFile: WriteFile,
        SettingFile:SettingFile
    };
})();

var GBMessager = (function () {

    var Alert = function (content, callBack) {
        callBack = callBack || function () { };
        $.messager.alert('提示', content, 'info', callBack);
    };
    var Confirm = function (content, callBack) { $.messager.confirm('确认', content, callBack);};
    var Prompt = function (content, callBack) { $.messager.prompt('确认', content, callBack);};
    return {
        Alert: Alert,
        Confirm: Confirm,
        Prompt: Prompt
    };
　　})();

var JQMGlobal = (function () {

    var ShowConfirmPopUp = function (content, CallBack) {
        var popupDialogId = 'ConfirmpopupDialog';
        $('<div data-role="popup" id="' + popupDialogId + '" data-confirmed="no" data-transition="pop" data-overlay-theme="a" data-theme="a" data-dismissible="false" style="min-width:216px;max-width:500px;"> \
                    \
                    <div role="main" class="ui-content">\
                    <h3 class="ui-title" style=" text-align:center;margin-bottom:15px">'+ content + '</h3>\
                    <a href="#" class="ui-btn ui-corner-all ui-shadow ui-btn-inline ui-btn-b optionConfirm" data-rel="back" style="width: 33%;border-radius: 5px;height: 30px;line-height: 30px;padding: 0;font-size: .9em;margin: 0 0 0 12%;font-weight: 100;">确定</a>\
                    <a href="#" class="ui-btn ui-corner-all ui-shadow ui-btn-inline ui-btn-b optionCancel" data-rel="back" data-transition="flow" style="width: 33%;border-radius: 5px;height: 30px;line-height: 30px;padding: 0;font-size: .9em;margin: 0 0 0 5%;font-weight: 100;color: #333;text-shadow: none;">取消</a>\
                    </div>\
                </div>')
            .appendTo($.mobile.pageContainer);
        var popupDialogObj = $('#' + popupDialogId);
        popupDialogObj.trigger('create');
        popupDialogObj.popup({
            afterclose: function (event, ui) {
                popupDialogObj.find(".optionConfirm").first().off('click');
                var isConfirmed = popupDialogObj.attr('data-confirmed') === 'yes' ? true : false;
                $(event.target).remove();
                if (isConfirmed) {
                    //这里执行确认需要执行的代码
                }
                CallBack(isConfirmed);
            }
        });
        popupDialogObj.popup('open');
        popupDialogObj.find(".optionConfirm").first().on('click', function () {
            popupDialogObj.attr('data-confirmed', 'yes');
        });
    };
    var ShowPopUp = function (content) {
        var popupDialogId = 'popupDialog';
        $('<div data-role="popup" id="' + popupDialogId + '" data-confirmed="no" data-transition="pop" data-overlay-theme="a" data-theme="a" data-dismissible="false" style="text-align:center;min-width:216px;max-width:500px;"> \
                    \
                    <div role="main" class="ui-content">\
                    <h3 class="ui-title" style="text-align:center;margin-bottom:15px">'+ content + '</h3>\
                    <a href="#" class="ui-btn ui-corner-all ui-shadow ui-btn-inline ui-btn-b optionConfirm" data-rel="back" style="width: 33%;border-radius: 5px;height: 30px;line-height: 30px;padding: 0;font-size: .9em;font-weight: 100;">确定</a>\
                    </div>\
                </div>')
            .appendTo($.mobile.pageContainer);
        var popupDialogObj = $('#' + popupDialogId);
        popupDialogObj.trigger('create');
        popupDialogObj.popup({
            afterclose: function (event, ui) {
                popupDialogObj.find(".optionConfirm").first().off('click');
                var isConfirmed = popupDialogObj.attr('data-confirmed') === 'yes' ? true : false;
                $(event.target).remove();
                if (isConfirmed) {
                    //这里执行确认需要执行的代码
                }
                //CallBack(isConfirmed);
            }
        });
        popupDialogObj.popup('open');
        popupDialogObj.find(".optionConfirm").first().on('click', function () {
            popupDialogObj.attr('data-confirmed', 'yes');
        });
    };
    var ShowTextInput = function (content, CallBack) {
        var popupDialogId = 'popupDialog';
        $('<div data-role="popup" id="' + popupDialogId + '" data-confirmed="no" data-transition="pop" data-overlay-theme="a" data-theme="a" data-dismissible="false" style="min-width:216px;max-width:500px;text-align:center;"> \
                    \
                    <div role="main" class="ui-content">\
                    <h4 class="ui-title" style="text-align:center;margin-bottom:15px">'+ content + '</h4>\
                    <input type="text" name="123asdsadasd123"  id="123asdsadasd123" value="">\
                    <a href="#" class="ui-btn ui-corner-all ui-shadow ui-btn-inline ui-btn-b optionConfirm" data-rel="back" style="width: 33%;border-radius: 5px;height: 30px;line-height: 30px;padding: 0;font-size: .9em;font-weight: 100;">确定</a>\
                    </div>\
                </div>')
            .appendTo($.mobile.pageContainer);
        var popupDialogObj = $('#' + popupDialogId);
        popupDialogObj.trigger('create');
        popupDialogObj.popup({
            afterclose: function (event, ui) {
                popupDialogObj.find(".optionConfirm").first().off('click');
                var isConfirmed = popupDialogObj.attr('data-confirmed') === 'yes' ? true : false;
                $(event.target).remove();
                if (isConfirmed) {
                    CallBack(isConfirmed, $('#123asdsadasd123').val());
                }
                //CallBack(isConfirmed);
            }
        });
        popupDialogObj.popup('open');
        popupDialogObj.find(".optionConfirm").first().on('click', function () {
            popupDialogObj.attr('data-confirmed', 'yes');
        });
    };

    return {
        ShowConfirmPopUp: ShowConfirmPopUp,
        ShowPopUp: ShowPopUp,
        ShowTextInput: ShowTextInput
    };
　　})();

/*数据跨域传输格式控制*/
var AjaxCrossDomainJson = function (url,type, prm, SuccessCallBack, FailCallBack) {
    $.ajax({
        url: url,
        type: type,
        crossDomain: true,
        data: prm,
        success: SuccessCallBack,
        fail: FailCallBack,
        error: FailCallBack
    });
};

var AjaxGB = function (type, prm, SuccessCallBack, FailCallBack) {
    AjaxCrossDomainJson(GBSetting.GetServerURL() + '/api/Data', type, prm, SuccessCallBack, FailCallBack);
};

var LazyAJax = function (prm, SuccessCallBack,ErrorCallback) {
    ErrorCallback = ErrorCallback || function () { };
    console.log(ErrorCallback);
    AjaxGB('post', prm, function (data) {
        MaskUtil.unmask();
        if (data.Status == 0) {
            GBMessager.Alert(data.Msg, ErrorCallback);
            return;
        }
        if (data.Data.length < 1) {
            GBMessager.Alert('无数据', ErrorCallback);
            return;
        }
        SuccessCallBack(data);
    }, function (data) {
        MaskUtil.unmask();
        GBMessager.Alert('发生了错误...', ErrorCallback);
        return;
    });

};

var App = (function () {
    var Exit = function () {
        localStorage.removeItem('storageNo');
        localStorage.removeItem('storageName');
        localStorage.removeItem('placeNo');
        localStorage.removeItem('placeName');
        localStorage.removeItem('username');
        localStorage.removeItem('userno');
        localStorage.removeItem('usernumber');
        navigator.app.exitApp();
    }

    return {
        Exit: Exit
    };
})();

var MaskUtil = (function () {

    var $mask, $maskMsg;

    var defMsg = '正在处理，请稍待。。。';

    function init() {
        if (!$mask) {
            $mask = $("<div class=\"datagrid-mask mymask\"></div>").appendTo("body");
        }
        if (!$maskMsg) {
            $maskMsg = $("<div class=\"datagrid-mask-msg mymask\">" + defMsg + "</div>")
                .appendTo("body").css({ 'font-size': '12px' });
        }

        $mask.css({ width: "100%", height: $(document).height() });

        $maskMsg.css({
            left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2,
        });

    }

    return {
        mask: function (msg) {
            init();
            $mask.show();
            $maskMsg.html(msg || defMsg).show();
        }
        , unmask: function () {
            $mask.hide();
            $maskMsg.hide();
        }
    }

}());    

/*检查是否为安卓系统*/
function check(){
    var u = navigator.userAgent;
    var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1; //android终端
    return isAndroid;
}


$(function () {

    document.addEventListener("deviceready", onDeviceReady, false);
    window.addEventListener('native.keyboardshow', function () {
        if ($('#txtServerUrl').is(':focus')) {
            return;
        }
        cordova.plugins.Keyboard.close();
    });

    function onDeviceReady() {
        console.log("console.log works well");
        console.log(device.cordova);

        
        //ReadFile(function (text) { alert(text); });

    }
});
