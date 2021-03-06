﻿//Submit form using jquery ajax
$.fn.ajaxSubmit = function (opt = {}) {
    this.on('submit', e => {
        e.preventDefault();
        var $form = $(e.currentTarget);

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

        $form.addClass('loading');
        $.ajax(options);
    }).on('ajaxSubmitComplete', (e, jqXHR, status) => {
        e.preventDefault();
        var func = $(e.currentTarget).attr('rel') || 'ajaxSubmitComplete';
        if (func === 'dialog') {
            if (status === 'success') {
                $(`<div>${jqXHR.responseText}</div>`).dialog();
            }
        } else if (typeof window[func] === 'function') {
            window[func](e, jqXHR, status);
        }
    });
    return this;
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
        var func = $(e.currentTarget).attr('rel') || 'ajaxClick';
        if (func === 'dialog') {
            if (status === 'success') {
                $(`<div>${jqXHR.responseText}</div>`).dialog();
            }
        } else if (typeof window[func] === 'function') {
            window[func](e, jqXHR, status);
        }
    });
    return this;
}

$.fn.dialog = function (opt) {
    var content = $((this == null || this.length == 0) ? '<p>Nothing to display</p>' : $('<div>').append(this));
    var options = $.extend({}, { 'title': 'Modal window', 'content': content }, opt);

    if (!window.dialog) {
        var $mc = $(`<div class="ui large modal"><i class="close icon"></i><div class="header">${options.title}</div>` +
            `<div class="scrolling content">${options.content.html()}</div>` +
            `<div class="actions"><div class="ui button deny">Cancel</div><button class="ui button green submit">OK</button></div>` +
            `</div>`);

        window.dialog = $mc.modal({
            'closable': false,
            'transition': 'horizontal flip',
            'onVisible': function () {
                var $modal = $(this);
                var $form = $modal.find('form');
                if ($form.length) {
                    $form.ajaxSubmit();
                    $form.find('a[data-request=ajax]').ajaxClick();
                    $modal.find('button.submit').attr('form', $form.attr('id'));
                }
            },
            'onHidden': function () {
                $(this).find('div.content').empty();
            }
        });
    } else {
        window.dialog.find('div.header').html(options.title)
        window.dialog.find('div.content').html(options.content.html())
    }
    window.dialog.modal('show');
}

$.fn.message = function (opt) {
    var options = $.extend({}, { 'title': '', 'content': '', 'status': 'info' }, opt);
    var msg = `<div class='ui floating ${options.status} message'><i class='close icon'></i> <div class='header'>${options.title}</div><p>${options.content}</p></div>`;
    $(msg).appendTo('body').delay(1500).remove();
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

$.fn.addField = function (target) {
    $.get('/api/section/getfieldtypes', { cache: true }).done(res => {
        var $section = $(`<div class="inline fields">
            <div class="eight wide field required">
                <input name="Fields[][Name]" required placeholder="Name" data-value-type="string" />
            </div>
            <div class="four wide field required" placeholder="Parent">
                <select name="Fields[][Type]" data-value-type="number" required >
                    ${res.map(d => `<option value=${d.id}>${d.title}</option>`)}
                </select>
            </div>
            <div class="three wide field">
                <div class="ui toggle checkbox">
                    <input name="Fields[][IsRequired]" type="checkbox" data-value-type="boolean" tabindex="0" >
                    <label>Is Required</label>
                </div>
            </div>
            <div class="one wide field">
                <a href="#">delete</a>
            </div>
        </div>`);

        $section.find('a').on('click', (e) => {
            e.preventDefault();
            $section.remove();
        });

        $section.appendTo(target);
    });
}