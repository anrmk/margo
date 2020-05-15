$(document).ready(() => {
    $('body').bootstrapMaterialDesign();

    $.fn.initSidebar(navigator.platform.indexOf('Win') > -1);
    $.fn.initMinimizeSidebar();

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

$.fn.initSidebar = function (isWindows) {
    if (isWindows) {
        // if we are on windows OS we activate the perfectScrollbar function
        $('.sidebar .sidebar-wrapper, .main-panel, .main').perfectScrollbar();
        $('html').addClass('perfect-scrollbar-on');
    } else {
        $('html').addClass('perfect-scrollbar-off');
    }

    var mobile_menu_visible = 0,
        mobile_menu_initialized = false,
        toggle_initialized = false,
        bootstrap_nav_initialized = false;

    $('.navbar-toggler').on('click', function () {
        var $toggle = $(this);

        if (mobile_menu_visible == 1) {
            $('html').removeClass('nav-open');

            $('.close-layer').remove();
            setTimeout(function () { $toggle.removeClass('toggled'); }, 400);

            mobile_menu_visible = 0;
        } else {
            setTimeout(function () { $toggle.addClass('toggled'); }, 430);

            var $layer = $('<div class="close-layer"></div>');

            if ($('body').find('.main-panel').length != 0) {
                $layer.appendTo(".main-panel");
            } else if (($('body').hasClass('off-canvas-sidebar'))) {
                $layer.appendTo(".wrapper-full-page");
            }

            setTimeout(function () { $layer.addClass('visible'); }, 100);

            $layer.click(function () {
                $('html').removeClass('nav-open');
                mobile_menu_visible = 0;

                $layer.removeClass('visible');

                setTimeout(function () {
                    $layer.remove();
                    $toggle.removeClass('toggled');
                }, 400);
            });

            $('html').addClass('nav-open');
            mobile_menu_visible = 1;
        }
    });
}

$.fn.initMinimizeSidebar = function () {
    window.sidebar_mini_active = false;
    $('#minimizeSidebar').click(function () {
        var $btn = $(this);

        if (window.sidebar_mini_active == true) {
            $('body').removeClass('sidebar-mini');
            window.sidebar_mini_active = false;
        } else {
            $('body').addClass('sidebar-mini');
            window.sidebar_mini_active = true;
        }

        // we simulate the window Resize so the charts will get updated in realtime.
        var simulateWindowResize = setInterval(function () {
            window.dispatchEvent(new Event('resize'));
        }, 180);

        // we stop the simulation of Window Resize after the animations are completed
        setTimeout(function () {
            clearInterval(simulateWindowResize);
        }, 1000);
    });
},

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

$.fn.alert = function (from, align, message, type = 'warning', callback = function () { }) {
    $.notify({
        'icon': 'add_alert',
        'message': message
    }, {
        'type': type,
        'timer': 3000,
        'placement': { 'from': from, 'align': align }
    });
}

$.fn.randomDate = function (from, to) {
    const fromTime = from.getTime();
    const toTime = to.getTime();
    return new Date(fromTime + Math.random() * (toTime - fromTime));
}

/**
 * Extension for bootstrapTable 
 * formatting Date
 */
$.fn.bootstrapTable.formatDate = function (value, row, index) {
    var options = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return value == null ? "" : new Date(value).toLocaleDateString("en-US", options);
};

$.extend($.fn.bootstrapTable.defaults, {
    classes: 'table table-hover',
    theadClasses: 'text-primary',
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