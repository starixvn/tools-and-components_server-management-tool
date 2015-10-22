var SERVICE = (function ($) {
    var serviceObject = {};

    serviceObject.initialize = function () {
        setInterval(function () { serviceObject.autoGetService(); }, 10000);
    };

    serviceObject.autoGetService = function () {
        $.ajax({
            url: rootDir + "Home/ServiceListView",
            dataType: "json",
            global: false,
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {
                $('#service-monitor-list').html(data);
            },
            complete: function () {
            }
        });
    };

    return serviceObject;
} (jQuery));