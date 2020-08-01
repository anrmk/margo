$(document).ready(() => {
    // fix menu when passed
    $('.masthead').visibility({
        once: false,
        onBottomPassed: function () {
            $('.fixed.menu').transition('fade in');
        },
        onBottomPassedReverse: function () {
            $('.fixed.menu').transition('fade out');
        }
    });

    $('.ui.dropdown').dropdown();
    $('form[data-request=ajax]').ajaxSubmit();
    $('a[data-request=ajax]').ajaxClick();
}).ajaxSend((event, xhr, options) => {
    //xhr.setRequestHeader("Authorization", "Bearer " + $('input:hidden[name="__RequestVerificationToken"]').val());
}).ajaxStart(() => {
    $('form fieldset').attr('disabled', 'disabled');
}).ajaxStop(() => {
    $('form fieldset').removeAttr('disabled')
}).ajaxError((e, jqxhr, settings, thrownError) => {
    window.console.log("Error", jqxhr.responseText);
    // $.fn.alert('top', 'center', jqxhr.responseText, 'danger');
});

$.extend(true, $.fn.dataTable.defaults, {
    'toolbar': '#toolbar',
    'dom': `<'ui stackable grid'<'row'<'eight wide column'<'dt-toolbar'>><'right aligned four wide column'l><'right aligned four wide column'f>><'row dt-table'<'sixteen wide column'tr>><'row'<'seven wide column'i><'right aligned nine wide column'p>>>`,
    'searching': true,
    'ordering': false,
    'processing': true,
    'serverSide': true,
    'searchDelay': 3500,
    'autoWidth': false,
    'mark': true,
    'rowId': function (d) {
        return 'row_' + d.id;
    },
    'language': {
        'search': '_INPUT_',
        'searchPlaceholder': 'Search...'
    },
    'fnInitComplete': function (settings, json) {
        $($.fn.dataTable.defaults.toolbar).appendTo($('div.dt-toolbar'));
    }
});

$.extend($.serializeJSON.defaultOptions, {
    'parseNumbers': true,
    'useIntKeysAsArrayIndex': false,
    'customTypes': {
        'string:nullable': function (str) { return str || null; },
        'number:nullable': function (str) { return Number(str) || null; }
    }
});