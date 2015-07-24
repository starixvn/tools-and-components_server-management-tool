var SITE = (function ($) {
    var siteObject = {};

    siteObject.initialize = function () {
        setInterval(function () { siteObject.autoReload(); }, 10000);
        siteObject.selectedServer();
        siteObject.Reboot();
        siteObject.RebootAll();
        siteObject.SignOut();
    };

    siteObject.autoReload = function () {
        $.ajax({
            url: rootDir + "Home/AutoReloadData",
            dataType: "json",
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {

                $.each(data, function (key, val) {
                    var serverSiteId = val.ServerSiteId;
                    var formatedDate = new Date(val.ReplyDate);
                    var formatedTime = moment(formatedDate).format('h:mm A');

                    if (val.IsResult) {
                        $('#server-' + serverSiteId).removeClass('bg-red').addClass('bg-darkCobalt');
                        $('#icon-' + serverSiteId).removeClass('mif-spinner4').addClass('mif-display');
                    }
                    else {
                        $('#server-' + serverSiteId).removeClass('bg-darkCobalt').addClass('bg-red');
                    }

                    $('#time-' + serverSiteId).html(formatedTime);
                });
            },
            complete: function () {
            }
        });
    };


    siteObject.selectedServer = function () {
        $('[name=server]').on('click', function () {
            var $this = $(this);
            if ($this.hasClass('element-selected')) {
                $this.removeClass('element-selected');
            }
            else {
                $this.addClass('element-selected');
            }
        });
    };

    siteObject.Reboot = function () {
        $('.mif-undo').on('click', function () {
            var ServerIds = [];
            var paramServerIds = [];
            $(".element-selected").each(function () {
                var $this = $(this);
                var siteId = $this.attr('site-id');
                var serverId = $this.attr('server-id');
                paramServerIds.push(siteId + ':' + serverId);
            });


            var postingData = { serverIds: paramServerIds };

            if (confirm("Are you sure you want to reboot server ????")) {

                $.ajax({
                    url: rootDir + "Home/Reboot",
                    type: 'Post',
                    data: JSON.stringify(postingData),
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        // Block UI
                    },
                    success: function (data) {
                        var errMsg = '';
                        $.each(data, function (key, val) {
                            var iconId = key.replace('server', 'icon');
                            $('#' + iconId).removeClass('mif-display').addClass('mif-spinner4');
                            if (val.ErrorCode != 0) {
                                errMsg += key + ': ' + val.ErrorMessage + '</br></br>';
                            }
                        });

                        if (errMsg != '') {
                            DIALOG.ShowDialog(errMsg, 'Error Information');
                        }
                    },
                    complete: function () {

                    }
                });
            }
        });
    };

 




    siteObject.RebootAll = function () {
        $('.mif-loop2').on('click', function () {
            if (confirm("Are you sure you want to reboot all devices ????")) {

                $.ajax({
                    url: rootDir + "Home/RebootAll",
                    type: 'Post',
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        // Block UI
                    },
                    success: function (data) {
                        $.each(data, function (key, val) {
                            var iconId = key.replace('server', 'icon');
                            $('#' + iconId).removeClass('mif-display').addClass('mif-spinner4');
                        });
                    },
                    complete: function () {

                    }
                });
            }
        });
    };

    siteObject.SignOut = function () {
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

    return siteObject;
} (jQuery));