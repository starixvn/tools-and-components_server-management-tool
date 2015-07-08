/* File Created: June 19, 2015 */

var USERSITESETTING = (function ($) {
    var userSiteSettingObject = {};

    userSiteSettingObject.initialize = function () {

        $("#user-permission-setting").on('click', function () {
            userSiteSettingObject.DataList();
            showSettings();
        });
    };

    userSiteSettingObject.DataList = function () {
        $.ajax({
            url: rootDir + "Setting/UserPermission",
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'User Permission Management');
            },
            complete: function () {
                userSiteSettingObject.ReloadData();
                userSiteSettingObject.UpdateData();
            }
        });
    };

    userSiteSettingObject.ReloadData = function () {
        $("#SiteId").on('change', function () {
            var siteId = $(this).val();
            $.ajax({
                url: rootDir + "Setting/UserPermissionView",
                type: 'GET',
                data: 'SiteId=' + siteId,
                beforeSend: function () {

                },
                success: function (data) {
                    $("tbody").html(data);
                },
                complete: function () {
                    userSiteSettingObject.UpdateData();
                }
            });
        });
    };

    userSiteSettingObject.UpdateData = function () {
        $('.radio-permission').on('click', function () {
            var permission = $(this).attr('data-permission');
            var userId = $(this).attr('data-userid');
            var param = 'SiteId=' + $('#SiteId').val() + '&Permission=' + permission + '&UserId=' + userId;

            $.ajax({
                url: rootDir + "Setting/SetUserPermission",
                type: 'POST',
                data: param,
                beforeSend: function () {

                },
                success: function () {

                },
                complete: function () {

                }
            });

        });
    };

    return userSiteSettingObject;
} (jQuery));