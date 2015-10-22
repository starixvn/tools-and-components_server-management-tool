/* File Created: June 19, 2015 */

var DIALOG = (function ($) {
    var dialogObject = {};

    dialogObject.initialize = function () {

    };

    dialogObject.ShowDialog = function (content, title) {
        $('.dialog-content').html(content);
        $('.dialog-title span').html(title);
        var dialog = $('#dialog').data('dialog');

        dialog.open();
    };

    dialogObject.CloseDialog = function () {
        var dialog = $('#dialog').data('dialog');
        dialog.close();
    };

    return dialogObject;
} (jQuery));