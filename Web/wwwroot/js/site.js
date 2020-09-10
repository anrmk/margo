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

    $('.ui.shape').find('button[data-target]').on('click', (e) => {
        e.preventDefault();
        $('.shape').shape('set next side', $(e.currentTarget).data('target')).shape('flip over');
    });
    $('.ui.dropdown').dropdown();
    $('form[data-request=ajax]').ajaxSubmit();
    $('a[data-request=ajax]').ajaxClick();
    $('select[data-request=ajax]').ajaxClick({'eventName': 'change'});
    $("#sidenav")
        .sidenav("settings", "content", "#main-content")
        .sidenav("settings", "btnOpen", "#sidebar-btn")
        .sidenav("attachEvent");
    // $('.ui.vertical.inverted.menu.sidebar').first()
    //     .sidebar('setting', 'dimPage', false)
    //     .sidebar('setting', 'closable', false)
    //     .sidebar('attach events', '#sidebar-btn')
    //     .sidebar('toggle');
    $('.icon.mysidebar').removeClass('disabled');

    window.Hub = new NotificationHub();
}).ajaxSend((event, xhr, options) => {
    xhr.setRequestHeader("ApiKey", "Bearer " + $('input:hidden[name="__RequestVerificationToken"]').val());
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
        'number:nullable': function (str) { return Number(str) || null; },
        'list': function (str, _, inputData) { 
            const $input = $(inputData.self);
            const $parent = $input.parent("div");
            const $inputsGroup = $parent.find("div[data-container='group'] :input");
            const inputsGroupData = $inputsGroup.serializeArray();
            
            if (inputsGroupData.length > 0) {
                const formattedInputsGroupData = inputsGroupData.chunk(2);

                str = formattedInputsGroupData
                    .map(data => `${data[0].value},${data[1].value}`)
                    .join(";");

                return str;
            }

            return str || ""; 
        },
    }
});

$.extend($.fn.dropdown.settings, {
    //'clearable': true,
    'forceSelection': false, //when you open dropdown do not focus on unselected item
    'onChange': function (value, text, $choice) {
        if (typeof window['sDropdownOnChange'] === 'function') {
            window['sDropdownOnChange']($.Event("change", { target: this, currentTarget: $choice }), value, text, $choice);
        }
    }
});