//Submit form using jquery ajax
$.fn.ajaxSubmit = function (opt, callback) {
    var fieldset = this.find('fieldset');

    var options = $.extend({
        'url': this.attr('action'),
        'type': this.attr('method'),
        'data': JSON.stringify(this.serializeJSON({ parseNumbers: true })),
        'processData': false,
        'contentType': 'application/json; charset=utf-8'
    }, opt);

    fieldset.attr('disabled', 'disabled');
    $.ajax(options).done((data, status, jqXHR) => {
        fieldset.removeAttr('disabled');
        callback(this, data, status, jqXHR);
    }).fail((jqXHR, status) => {
        fieldset.removeAttr('disabled');
        callback(this, null, status, jqXHR);
    });
}

$.fn.close = function () {
    $(this).closest('.card').fadeOut();
}

