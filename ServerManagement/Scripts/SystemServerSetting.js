/* File Created: June 19, 2015 */

var SYSTEMSERVERSETTING = (function ($) {
    var systemServerSettingObject = {};

    systemServerSettingObject.initialize = function () {

        $("#system-server-setting").on('click', function () {
            systemServerSettingObject.SystemServerList();
            showSettings();
        });
    };

    systemServerSettingObject.SystemServerList = function () {
        $.ajax({
            url: rootDir + "Setting/SystemServerList",
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'Server Site Management');
            },
            complete: function () {
                systemServerSettingObject.AddSystemServer();
                systemServerSettingObject.EditSystemServer();
                systemServerSettingObject.DeleteSystemServer();
            }
        });
    };

    systemServerSettingObject.AddSystemServer = function () {
        $("#btn-add").on('click', function () {
            $.ajax({
                url: rootDir + "Setting/UpdateSystemServer",
                type: 'GET',
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add Server');
                },
                complete: function () {
                    systemServerSettingObject.CancelSystemServer();
                    systemServerSettingObject.UpdateSystemServer();
                }
            });
        });
    };

    systemServerSettingObject.UpdateSystemServer = function () {
        $('#btn-submit').on('click', function () {
            var systemForm = $("form");
            $.ajax({
                url: rootDir + "Setting/UpdateSystemServer",
                type: 'POST',
                data: systemForm.serialize(),
                success: function (result) {
                    if (result.ErrorCode == -1) {
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                    else {
                        systemServerSettingObject.SystemServerList();
                    }
                },
                complete: function () {

                }
            });
        });
    };

    systemServerSettingObject.CancelSystemServer = function () {
        $('#btn-cancel').on('click', function () {
            systemServerSettingObject.SystemServerList();
        });
    };

    systemServerSettingObject.EditSystemServer = function () {
        $('.mif-pencil').on('click', function () {
            var $this = $(this);

            $.ajax({
                url: rootDir + "Setting/UpdateSystemServer",
                type: 'GET',
                data: 'Id=' + $this.attr('server-site-id'),
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add System');
                },
                complete: function () {
                    systemServerSettingObject.CancelSystemServer();
                    systemServerSettingObject.UpdateSystemServer();
                }
            });
        });
    };

    systemServerSettingObject.DeleteSystemServer = function () {
        $('.mif-cancel').on('click', function () {
            var $this = $(this);
            $.ajax({
                url: rootDir + "Setting/DeleteSystemServer",
                type: 'POST',
                data: 'Id=' + $this.attr('server-site-id'),
                success: function (result) {
                    if (result.ErrorCode == -1) {
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                    else {
                        systemServerSettingObject.SystemServerList();
                    }
                },
                complete: function () {

                }
            });
        });
    };

    return systemServerSettingObject;
} (jQuery));