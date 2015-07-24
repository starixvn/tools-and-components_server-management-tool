/* File Created: June 19, 2015 */

var NOTIFY = (function ($) {
    var notifyObject = {};

    notifyObject.initialize = function () {

    };

    notifyObject.Alert = function (content, title) {
        $.Notify({
            caption: title,
            content: content,
            type: 'alert'
        });
    };

    notifyObject.Success = function (content, title) {
        $.Notify({
            caption: title,
            content: content,
            type: 'success'
        });
    };

    notifyObject.Warning = function (content, title) {
        $.Notify({
            caption: title,
            content: content,
            type: 'warning'
        });
    };

    notifyObject.Information = function (content, title) {
        $.Notify({
            caption: title,
            content: content,
            type: 'info'
        });
    };

    return notifyObject;
} (jQuery));