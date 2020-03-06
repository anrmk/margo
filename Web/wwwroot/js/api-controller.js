//Get company summary range
$.fn.getCompanySummaryRange = function (id) {
    return $.ajax(`/api/company/${id}/summaryrange`);
};

//Get bulk customers
$.fn.getBulkCustomers = function (id, from, to) {
    return $.ajax({
        url: '/api/customer/bulk',
        data: { 'id': id, 'from': from, 'to': to }
    });
}

//Get bulk customers
$.fn.getBulkInvoices = function (id, from, to) {
    return $.ajax({
        url: '/api/invoice/unpaid',
        data: { 'id': id, 'from': from, 'to': to }
    });
}

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

$.fn.randomDate = function (from, to) {
    const fromTime = from.getTime();
    const toTime = to.getTime();
    return new Date(fromTime + Math.random() * (toTime - fromTime));
}