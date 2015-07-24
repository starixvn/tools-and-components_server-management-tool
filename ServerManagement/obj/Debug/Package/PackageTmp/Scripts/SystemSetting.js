/* File Created: June 19, 2015 */

var SYSTEMSETTING = (function ($) {
    var systemSettingObject = {};

    systemSettingObject.initialize = function () {

        $("#system-setting").on('click', function () {
            systemSettingObject.SystemList();
            showSettings();
        });
    };

    systemSettingObject.SystemList = function () {
        $.ajax({
            url: rootDir + "Setting/SystemList",
            type: 'GET',
            beforeSend: function () {
                
            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'System Management');
            },
            complete: function () {
                systemSettingObject.AddSystem();
                systemSettingObject.EditSystem();
            }
        });
    };

    systemSettingObject.AddSystem = function () {
        $("#btn-add").on('click', function () {
            $.ajax({
                url: rootDir + "Setting/UpdateSystem",
                type: 'GET',
                beforeSend: function () {
                   
                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add System');
                },
                complete: function () {
                    systemSettingObject.CancelSystem();
                    systemSettingObject.UpdateSystem();
                }
            });
        });
    };

    systemSettingObject.UpdateSystem = function () {
        $('#btn-submit').on('click', function () {
            if ($('#SiteName').val() == '') {
                NOTIFY.Alert('Site name must be required', 'Error');
                $('#SiteName').focus();
                return;
            }

            var systemForm = $("form");
            $.ajax({
                url: rootDir + "Setting/UpdateSystem",
                type: 'POST',
                data: systemForm.serialize(),
                success: function (result) {
                    if (result.ErrorCode == -1) {                        
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                    else {
                        systemSettingObject.SystemList();
                    }
                },
                complete: function () {

                }
            });
        });
    };

    systemSettingObject.CancelSystem = function () {
        $('#btn-cancel').on('click', function () {
            systemSettingObject.SystemList();
        });
    };

    systemSettingObject.EditSystem = function () {
        $('.icon-edit').on('click', function () {
            var $this = $(this);

            $.ajax({
                url: rootDir + "Setting/UpdateSystem",
                type: 'GET',
                data: 'siteId=' + $this.attr('system-id'),
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add System');
                },
                complete: function () {
                    systemSettingObject.CancelSystem();
                    systemSettingObject.UpdateSystem();
                }
            });
        });
    };

    return systemSettingObject;
} (jQuery));