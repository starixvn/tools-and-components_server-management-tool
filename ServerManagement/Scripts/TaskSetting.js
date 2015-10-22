/* File Created: June 19, 2015 */

var TASKSETTING = (function ($) {
    var taskSettingObject = {};

    taskSettingObject.initialize = function () {

        $("#task-setting").on('click', function () {
            taskSettingObject.TaskList();
            showSettings();
        });
    };

    taskSettingObject.TaskList = function () {
        $.ajax({
            url: rootDir + "Setting/TaskList",
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'Task Management');
            },
            complete: function () {
                taskSettingObject.AddTask();
                taskSettingObject.EditTask();                
            }
        });
    };

    taskSettingObject.AddTask= function () {
        $("#btn-add").on('click', function () {
            $.ajax({
                url: rootDir + "Setting/UpdateTask",
                type: 'GET',
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Add Task');
                },
                complete: function () {
                    taskSettingObject.CancelTask();
                    taskSettingObject.UpdateTask();
                }
            });
        });
    };

    taskSettingObject.UpdateTask = function () {
        $('#btn-submit').on('click', function () {
            if ($('#TaskName').val() == '') {
                NOTIFY.Alert('Task Name must be required', 'Error');
                $('#TaskName').focus();
                return;
            }

            if ($('#Action').val() == '') {
                NOTIFY.Alert('Task Action must be required', 'Error');
                $('#Action').focus();
                return;
            }
                      
            var form = $("form");
            $.ajax({
                url: rootDir + "Setting/UpdateTask",
                type: 'POST',
                data: form.serialize(),
                success: function (result) {
                    if (result.ErrorCode == -1) {
                        NOTIFY.Alert(result.ErrorMessage, 'Error');
                    }
                    else {
                        taskSettingObject.TaskList();
                    }
                },
                complete: function () {

                }
            });
        });
    };

    taskSettingObject.CancelTask = function () {
        $('#btn-cancel').on('click', function () {
            taskSettingObject.TaskList();
        });
    };

    taskSettingObject.EditTask = function () {
        $('.icon-edit').on('click', function () {
            var $this = $(this);

            $.ajax({
                url: rootDir + "Setting/UpdateTask",
                type: 'GET',
                data: 'taskId=' + $this.attr('task-id'),
                beforeSend: function () {

                },
                success: function (data) {
                    DIALOG.ShowDialog(data, 'Edit Task');
                },
                complete: function () {
                    taskSettingObject.CancelTask();
                    taskSettingObject.UpdateTask();
                }
            });
        });
    };      

    return taskSettingObject;
} (jQuery));