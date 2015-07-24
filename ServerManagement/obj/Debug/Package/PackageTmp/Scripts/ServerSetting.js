/* File Created: June 19, 2015 */

var SERVERSETTING = (function ($) {
    var serverSettingObject = {};

    serverSettingObject.initialize = function () {

        $("#server-setting").on('click', function () {
            serverSettingObject.ServerList();
            showSettings();
        });        
    };

    serverSettingObject.ServerList = function () {
        $.ajax({
            url: rootDir + "Setting/ServerList",
            type: 'GET',
            beforeSend: function () {
                
            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'Server Management');
            },
            complete: function () {
                serverSettingObject.AddServer();
                serverSettingObject.EditServer();
            }
        });
    };

    serverSettingObject.AddServer = function () {
        $("#btn-add").on('click', function () {
            $.ajax({
                url: rootDir + "Setting/UpdateServer",
                type: 'GET',
                beforeSend: function () {
                   
                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add Server');
                },
                complete: function () {
                    serverSettingObject.CancelServer();
                    serverSettingObject.UpdateServer();
                }
            });
        });
    };

    serverSettingObject.UpdateServer = function () {
        $('#btn-submit').on('click', function () {
            if ($('#ServerIp').val() == '') {
                NOTIFY.Alert('Server IP must be required', 'Error');
                $('#ServerIp').focus();
                return;
            }

            if (!CORE.isIpAddress($('#ServerIp').val())) {
                NOTIFY.Alert('Server IP is invalid', 'Error');
                $('#ServerIp').focus();
                return;
            }

            var systemForm = $("form");
            $.ajax({
                url: rootDir + "Setting/UpdateServer",
                type: 'POST',
                data: systemForm.serialize(),
                success: function (result) {
                    if (result.ErrorCode == -1) {                        
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                    else {
                        serverSettingObject.ServerList();
                    }
                },
                complete: function () {

                }
            });
        });
    };

    serverSettingObject.CancelServer = function () {
        $('#btn-cancel').on('click', function () {
            serverSettingObject.ServerList();
        });
    };

    serverSettingObject.EditServer = function () {
        $('.icon-edit').on('click', function () {
            var $this = $(this);

            $.ajax({
                url: rootDir + "Setting/UpdateServer",
                type: 'GET',
                data: 'serverId=' + $this.attr('server-id'),
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add System');
                },
                complete: function () {
                    serverSettingObject.CancelServer();
                    serverSettingObject.UpdateServer();
                }
            });
        });
    };

    return serverSettingObject;
} (jQuery));