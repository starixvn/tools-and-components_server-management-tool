/* File Created: June 19, 2015 */

var USERSETTING = (function ($) {
    var userSettingObject = {};

    userSettingObject.initialize = function () {

        $("#user-setting").on('click', function () {
            userSettingObject.DataList();
            showSettings();
        });
    };

    userSettingObject.DataList = function () {
        $.ajax({
            url: rootDir + "Setting/UsersList",
            type: 'GET',
            beforeSend: function () {
                
            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'User Management');
            },
            complete: function () {
                userSettingObject.AddData();
                userSettingObject.EditData();
            }
        });
    };

    userSettingObject.AddData = function () {
        $("#btn-add").on('click', function () {
            $.ajax({
                url: rootDir + "Setting/UpdateUser",
                type: 'GET',
                beforeSend: function () {
                   
                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add User');
                },
                complete: function () {
                    userSettingObject.Cancel();
                    userSettingObject.UpdateData();
                }
            });
        });
    };

    userSettingObject.UpdateData = function () {
        $('#btn-submit').on('click', function () {
            if ($('#Username').val() == '') {
                NOTIFY.Alert('Username must be required', 'Error');
                $('#Username').focus();
                return;
            }

            if ($('#AliasName').val() == '') {
                NOTIFY.Alert('AliasName must be required', 'Error');
                $('#AliasName').focus();
                return;
            }

            if ($('#Email').val() == '') {
                NOTIFY.Alert('Email must be required', 'Error');
                $('#Email').focus();
                return;
            }

            if (!CORE.isEmail($('#Email').val())) {
                NOTIFY.Alert('Email is invalid', 'Error');
                $('#Email').focus();
                return;
            }

            var userForm = $("form");
            $.ajax({
                url: rootDir + "Setting/UpdateUser",
                type: 'POST',
                data: userForm.serialize(),
                success: function (result) {
                    if (result.ErrorCode == -1) {                        
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                    else {
                        userSettingObject.DataList();
                    }
                },
                complete: function () {

                }
            });
        });
    };

    userSettingObject.Cancel = function () {
        $('#btn-cancel').on('click', function () {
            userSettingObject.DataList();
        });
    };

    userSettingObject.EditData = function () {
        $('.icon-edit').on('click', function () {
            var $this = $(this);

            $.ajax({
                url: rootDir + "Setting/UpdateUser",
                type: 'GET',
                data: 'userId=' + $this.attr('user-id'),
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Edit User');
                },
                complete: function () {
                    userSettingObject.Cancel();
                    userSettingObject.UpdateData();
                }
            });
        });
    };

    return userSettingObject;
} (jQuery));