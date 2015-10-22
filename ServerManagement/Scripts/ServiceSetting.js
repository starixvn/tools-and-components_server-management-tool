/* File Created: September 1, 2015 */
var SERVICESETTING = (function ($) {
    var serviceSettingObject = {};
    serviceSettingObject.initialize = function () {

        $("#service-setting").on('click', function () {
            serviceSettingObject.ServiceList();
            showSettings();
        });
    };

    serviceSettingObject.ServiceList = function () {
        $.ajax({
            url: rootDir + "Setting/ServiceList",
            type: 'GET',
            beforeSend: function () {

            },
            success: function (data) {
                DIALOG.ShowDialog(data, 'Service Management');
            },
            complete: function () {
                $('#SiteId').on('change', function () {
                    var id = $(this).val();
                    serviceSettingObject.GetServerBySite(id);
                });

                $('#ServerId').on('change', function () {
                    var id = $(this).val();
                    var siteId = $('#SiteId').val();
                    var filter = $('#filter-service').val();
                    serviceSettingObject.ServiceListView(siteId, id, filter);
                });

                $('#filter-service').on('change', function () {
                    var id = $(this).val();
                    var siteId = $('#SiteId').val();
                    var serverId = $('#ServerId').val();
                    serviceSettingObject.ServiceListView(siteId, serverId, id);
                });

                serviceSettingObject.UpdateService();
                serviceSettingObject.RemoveService();
            }
        });
    };

    serviceSettingObject.ServiceListView = function (siteId, serverId, filter) {
        $.ajax({
            url: rootDir + "Setting/ServiceListView",
            type: 'GET',
            data: 'serverId=' + serverId + '&siteid=' + siteId + '&filter=' + filter,
            beforeSend: function () {

            },
            success: function (data) {
                $('#service-content').html(data);
            },
            complete: function () {
                serviceSettingObject.UpdateService();
                serviceSettingObject.RemoveService();
            }
        });
    };

    serviceSettingObject.GetServerBySite = function (siteId) {

        $.ajax({
            url: rootDir + "Setting/GetServerBySite",
            type: 'GET',
            data: 'siteid=' + siteId,
            beforeSend: function () {

            },
            success: function (result) {
                $('#ServerId')
                    .find('option')
                    .remove()
                    .end();
                $.each(result, function (index, value) {
                    $('#ServerId')
                    .append('<option value=' + value.ServerId + '>' + value.ServerIp + '</option>')
                    .val(value.ServerIp);
                });

                $('#ServerId option:first').prop("selected", true);
            },
            complete: function () {
                serviceSettingObject.ServiceListView(siteId, $('#ServerId option:first').val(), $('#filter-service').val());
            }
        });
    };

    serviceSettingObject.RemoveService = function () {
        $('.mif-move-up').on('click', function () {
            var $this = $(this);
            var siteId = $('#SiteId').val();
            var serverId = $('#ServerId').val();
            var filter = $('#filter-service').val();

            var serviceName = $this.attr('data-service-name');
            var dispalyName = $this.attr('data-service-display-name');

            $.ajax({
                url: rootDir + "Setting/RemoveService",
                type: 'POST',
                data: 'siteid=' + siteId + '&serverId=' + serverId + '&serviceName=' + serviceName,
                beforeSend: function () {

                },
                success: function (result) {
                    if (result.ErrorCode != -1) {
                        serviceSettingObject.ServiceListView(siteId, serverId, filter);
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

    serviceSettingObject.UpdateService = function () {
        $('.mif-file-download').on('click', function () {
            var $this = $(this);
            var siteId = $('#SiteId').val();
            var serverId = $('#ServerId').val();
            var filter = $('#filter-service').val();

            var serviceName = $this.attr('data-service-name');
            var dispalyName = $this.attr('data-service-display-name');
            var status = $this.attr('data-service-status');

            $.ajax({
                url: rootDir + "Setting/InsertService",
                type: 'POST',
                data: 'siteid=' + siteId + '&serverId=' + serverId + '&serviceName=' + serviceName + '&displayName=' + dispalyName + '&status=' + status,
                beforeSend: function () {

                },
                success: function (result) {
                    if (result.ErrorCode != -1) {
                        serviceSettingObject.ServiceListView(siteId, serverId, filter);
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

    

    return serviceSettingObject;
} (jQuery));