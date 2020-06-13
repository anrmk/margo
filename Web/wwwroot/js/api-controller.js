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


$.fn.formatCurrency = function (value) {
    return "$" + value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
};

$.fn.formatLink = function (value) {
    return value == null ? '' : `<a href='${value}'target="_blank"><i class="fa fa-external-link-alt mr-1"></i>${value}</a>`;
}

$.fn.renderDatatableAction = function (data, type, row, target) {
    return `<div class='ui fitted slider checkbox'><input type='checkbox' name='Id[]' value='${data}'> <label></label></div>`;
}