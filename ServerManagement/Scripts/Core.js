/* File Created: June 24, 2015 */

var CORE = (function ($) {
    var coreObject = {};

    coreObject.initialize = function () {
        $(document).ajaxStart(function () {
            $('#ajax_loader').show();
           // NProgress.start();

        }).ajaxStop(function () {
            $('#ajax_loader').hide();
           // NProgress.done();
        }).ajaxError(function (xhr, status) {
           // $("#ajax-debug").text(status.responseText);
           // alert(status.responseText);
        });

        coreObject.SignOut();
    };

    coreObject.isIpAddress = function (ip) {
        return /^(?=\d+\.\d+\.\d+\.\d+$)(?:(?:25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]|[0-9])\.?){4}$/.test(ip);
    };

    coreObject.isEmail = function (email) {
        var re = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
        return re.test(email);
    };

    coreObject.SignOut = function () {
        $('.mif-switch').on('click', function () {
            $.ajax({
                url: rootDir + "Authorization/SignOut",
                type: 'Post',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.ErrorCode == 0) {
                        location.href = rootDir + "Authorization/Login"
                    }
                }
            });
        });
    };

    return coreObject;
} (jQuery));
