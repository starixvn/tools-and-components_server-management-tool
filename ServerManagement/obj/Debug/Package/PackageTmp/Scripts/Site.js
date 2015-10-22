var SITE = (function ($) {
    var siteObject = {};

    siteObject.initialize = function () {
        setInterval(function () { siteObject.autoReload(); }, 10000);
        siteObject.selectedServer();
        siteObject.viewSpec();
        siteObject.viewIIS();
        siteObject.viewService();
        siteObject.Reboot();
        siteObject.RebootAll();        
    };

    siteObject.autoReload = function () {
        return;
        $.ajax({
            url: rootDir + "Home/AutoReloadData",
            dataType: "json",
            global: false,
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {
                $.each(data, function (key, val) {
                    var serverId = val.ServerId;
                    var formatedReplyDate = new Date(val.ReplyDate);
                    var formatedReplyTime = moment(formatedReplyDate).format('h:mm A');

                    var formatedBootTime = new Date(val.BootUpTime);

                    if (val.IsResult) {
                        $('#server-' + serverId).removeClass('bg-red').addClass('bg-transparent');
                        $('#icon-' + serverId).removeClass('mif-spinner4').addClass('mif-display');
                    }
                    else {
                        $('#server-' + serverId).removeClass('bg-transparent').addClass('bg-red');
                    }

                    $('#time-' + serverId).html(formatedReplyTime);
                    $('#boot-time-' + serverId).html(formatedBootTime);
                });
            },
            complete: function () {
            }
        });
    };


    siteObject.selectedServer = function () {
        $('.select-server').on('click', function () {
            var $this = $(this);
            var serverId = $this.attr("server-id");
            var selectedServerObject = $('#server-' + serverId);

            if (selectedServerObject.hasClass('element-selected')) {
                selectedServerObject.removeClass('element-selected');
            }
            else {
                selectedServerObject.addClass('element-selected');
            }
        });
    };

    siteObject.viewSpec = function () {
        $('.view-spec').on('click', function () {
            var $this = $(this);
            var serverId = $this.attr("server-id");
            $.ajax({
                url: rootDir + "Home/ViewSpec",
                type: 'GET',
                data: 'serverid=' + serverId,
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Server Spec');
                },
                complete: function () {

                }
            });
        });
    };

    siteObject.viewIIS = function () {
        $('.view-iis').on('click', function () {
            var $this = $(this);
            var serverId = $this.attr("server-id");
            $.ajax({
                url: rootDir + "Home/ViewIIS",
                type: 'GET',
                data: 'serverid=' + serverId,
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'IIS Information');
                },
                complete: function () {

                }
            });
        });
    };

    siteObject.viewService = function () {
        $('.view-service').on('click', function () {
            var $this = $(this);
            var serverId = $this.attr("server-id");
            var siteId = $this.attr("site-id");
            $.ajax({
                url: rootDir + "Home/ViewService",
                type: 'GET',
                data: 'serverid=' + serverId + '&siteid=' + siteId,
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Service Information');
                },
                complete: function () {
                    siteObject.StopService();
                }
            });
        });
    };

    siteObject.StopService = function () {
        $('.action-service').on('click', function () {
            var serviceStatus = $(this).attr('data-service-status');
            var url = rootDir + "Home/StopService";
            var text = "Stopped";
            var status = 1;
            if (serviceStatus == 1) //Service is stopping
            {
                url = rootDir + "Home/StartService";
                text = "Running";
                status = 4;
            }

            var serviceName = $(this).attr('data-service-name');
            var serverId = $(this).attr('data-server-id');
            var serivceId = $(this).attr('data-service-id');
            $.ajax({
                url: url,
                type: 'POST',
                data: 'serviceName=' + serviceName + '&serverId=' + serverId,
                beforeSend: function () {

                },
                success: function (result) {
                    if (result.ErrorCode != -1) {
                        $('#service-' + serivceId).html(text);
                        $('#service-' + serivceId).attr('data-service-status', status);
                    }
                    else {
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                },
                complete: function () {

                }
            });

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

    return siteObject;
} (jQuery));