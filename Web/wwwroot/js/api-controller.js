//Submit form using jquery ajax
$.fn.ajaxSubmit = function (opt) {
    this.on('submit', e => {
        e.preventDefault();
        var form = $(e.currentTarget);
        var fieldset = form.find('fieldset');

        var options = $.extend({
            'url': form.attr('action'),
            'type': form.attr('method'),
            'data': JSON.stringify(form.serializeJSON()),
            'processData': false,
            'contentType': 'application/json; charset=utf-8',
            'complete': (jqXHR, status) => {
                fieldset.removeAttr('disabled');
                opt.callback(jqXHR, status);
            },
            'callback': (jqXHR, status) => {
             
            }
        }, opt);

        fieldset.attr('disabled', 'disabled');
        $.ajax(options);

        //    .done((data, status, jqXHR) => {
        //    fieldset.removeAttr('disabled');
        //    opt.callback(e, data, status, jqXHR);
        //}).fail((jqXHR, status) => {
        //    fieldset.removeAttr('disabled');
        //    opt.callback(e, null, status, jqXHR);
        //});
    })
}

$.fn.formatCurrency = function (value) {
    return "$" + value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
};

$.fn.formatLink = function (value) {
    return value == null ? '' : `<a href='${value}'target="_blank"><i class="fa fa-external-link-alt mr-1"></i>${value}</a>`;
}

$.fn.formatDate = function (data, type, row) {
    return data == null ? "" : new Date(data).toLocaleDateString();
};

$.fn.renderDatatableAction = function (data, type, row) {
    return `<div class='ui fitted slider checkbox'><input type='checkbox' name='Id[]' value='${data}'> <label></label></div>`;
}

$.fn.dialog = function (opt) {
    var content = $((this == null || this.length == 0) ? '<p>Nothing to display</p>' : this);
    var options = $.extend({}, {
        'title': 'Modal window',
        'content': content
    }, opt);

    $(options.content).find('form')

    var mc = `<div class="ui modal"><i class="close icon"></i><div class="header">${options.title}</div>` +
        `<div class="content">${options.content.html()}</div>` +
        `<div class="actions"><div class="ui button deny">Cancel</div><div class="ui button approve">OK</div></div>` +
        `</div>`;

    $(mc).modal({
        'closable': false,
        'transition': 'horizontal flip',
        'onDeny': function (e) {
            window.alert('Wait not yet!');
            return false;
        },
        'onApprove': function (e) {
            var form = $(this).find('form');
            if (form.length) {
                form.ajaxSubmit({}, (target, data, status, jqXHR) => {
                });
                //form.submit();
            }
            return false;
        }
    }).modal('show');
}