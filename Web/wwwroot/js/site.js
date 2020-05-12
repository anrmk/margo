﻿$(document).ready(() => {
    $.fn.datepicker.defaults.format = "mm/dd/yyyy";
    window.modal = $('#modalBackdrop');

    if (window.notificationHub === undefined)
        window.notificationHub = new NotificationHub();

    $('a[data-target=modal]').on('click', e => {
        e.preventDefault();
        var opt = {
            'url': $(e.currentTarget).attr('href')
        }

        $.ajax(opt).done((data, status, jqXHR) => {
            $(data).dialog('Your action is required', (action, e, content) => { });
        })
    });

    var settings = {
        validClass: "is-valid",
        errorClass: "is-invalid"
    };
    $.validator.setDefaults(settings);
    $.validator.unobtrusive.options = settings;

}).ajaxStart(() => {
    $('form fieldset').attr('disabled', 'disabled');
}).ajaxStop(() => {
    $('form fieldset').removeAttr('disabled')
});

$.fn.dialog = function (header, callback) {
    callback = callback || function () { };
    $.when(
        $('.modal .modal-title').text(header),
        $('.modal .modal-body').empty().html(this),

        window.modal.modal('show').off('shown.bs.modal').on('shown.bs.modal', (e) => {
            var form = $('.modal .modal-content form[id]');
            var submitBtn = $('.modal .modal-footer #modalSubmitBtn');
            if (form.length == 1) {
                submitBtn.attr('form', form.attr('id')).removeAttr('hidden');
            } else {
                submitBtn.attr('hidden', 'hidden');
            }
            callback("shown.bs.modal", e, this);
        }).off('hidden.bs.modal').on('hidden.bs.modal', (e) => {
            this.empty();
            callback("hidden.bs.modal", e, this);
        })
    ).done((e) => {
        callback("modal.on.load", e, this);
    });
    return window.modal;
}

$.fn.alert = function (content, callback = function () { }) {
    var $message = $(`<div class="alert alert-warning alert-dismissible fade show" role="alert">${content} <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>`);
    $message.appendTo('div.alert-container').delay(2500).fadeOut(500, function () {
        $(this).remove();
    });

    callback('alert.on.load', $message, this);
}
/**
 * Extension for bootstrapTable 
 * formatting Date
 */
$.fn.bootstrapTable.formatDate = function (value, row, index) {
    var options = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return value == null ? "" : new Date(value).toLocaleDateString("en-US", options);
    //return value == null ? "" : moment(value, 'MM-DD-YYYY').format('MM-DD-YYYY');
};

$.extend($.fn.bootstrapTable.defaults, {
    classes: 'table table-hover',
    sidePagination: 'server',
    toolbar: '#toolbar',
    showPaginationSwitch: false,
    search: true,
    idField: "id",
    pageSize: 10,
    pageList: [10, 100, 500, 'All'],
    clickToSelect: true,
    //showSearchButton: true,
    searchOnEnterKey: true,
    showRefresh: true,
    showColumns: true,
    showToggle: true,
    sortStable: true,
    pagination: true,
    maintainMetaData: true
});

//$.extend($.fn.bootstrapTable.columnDefaults, {
//    align: 'center',
//    valign: 'middle'
//})