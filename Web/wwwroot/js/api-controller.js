//Submit form using jquery ajax
$.fn.ajaxSubmit = function (opt = {}) {
    this.on('submit', e => {
        e.preventDefault();
        var $form = $(e.currentTarget);
        //var data = $form.serializeJSON();

        var options = $.extend({
            'url': $form.attr('action'),
            'type': $form.attr('method'),
            'data': JSON.stringify($form.serializeJSON()),
            'processData': false,
            'contentType': 'application/json; charset=utf-8',
            'complete': (jqXHR, status) => {
                $form.removeClass('loading').trigger('ajaxSubmitComplete', [jqXHR, status]);
            }
        }, opt);

        //if (options.type.toLowerCase() === 'post') {
        //    options.data =data);
        //}

        $form.addClass('loading');
        $.ajax(options);
    }).on('ajaxSubmitComplete', (e, jqXHR, status) => {
        e.preventDefault();
        var func = $(e.currentTarget).attr('rel');
        if (func === 'dialog') {
            if (status === 'success') {
                $(`<div>${jqXHR.responseText}</div>`).dialog();
            }
        } else if (typeof window[func] === 'function') {
            window[func](jqXHR, status);
        }
    });
}

$.fn.ajaxClick = function (opt = {}) {
    this.on('click', (e) => {
        e.preventDefault();
        var $link = $(e.currentTarget);
        var options = $.extend({
            'url': $link.attr('href'),
            'complete': (jqXHR, status) => {
                $link.trigger('ajaxClick', [jqXHR, status]);
            }
        }, opt);
        $.ajax(options);
    }).on('ajaxClick', (e, jqXHR, status) => {
        e.preventDefault();
        var func = $(e.currentTarget).attr('rel');
        if (func === 'dialog') {
            if (status === 'success') {
                $(`<div>${jqXHR.responseText}</div>`).dialog();
            }
        }
    });
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
    var options = $.extend({}, { 'title': 'Modal window', 'content': content }, opt);

    var mc = `<div class="ui modal"><i class="close icon"></i><div class="header">${options.title}</div>` +
        `<div class="content">${options.content.html()}</div>` +
        `<div class="actions"><div class="ui button deny">Cancel</div><button class="ui button green submit">OK</button></div>` +
        `</div>`;

    $(mc).modal({
        'closable': false,
        'transition': 'horizontal flip',
        'onVisible': function () {
            var $modal = $(this);
            var $form = $modal.find('form');
            if ($form.length) {
                $form.ajaxSubmit();
                $modal.find('button.submit').attr('form', $form.attr('id'));
            }
        }
    }).modal('show');
}

$.fn.message = function (opt) {
    var options = $.extend({}, { 'title': '', 'content': '', 'status': 'info' }, opt);
    var msg = `<div class='ui floating ${options.status} message'><i class='close icon'></i> <div class='header'>${options.title}</div><p>${options.content}</p></div>`;
    $(msg).appendTo('body').delay(1500).remove();
}