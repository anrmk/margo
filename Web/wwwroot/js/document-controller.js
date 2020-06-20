class DocumentController {
    constructor(options, callback = (target) => { }) {
        this.options = $.extend(true, {
            'table': '#table',
            'filter': '#table-filter',
            'controller': { 'api': '', 'mvc': '' },
            'columns': [],
            'selected': window.selected
        }, options);

        this.table = $(this.options.table);
        this.toolbar = $(this.table.data('toolbar'));
        this.filter = $(this.options.filter);
        this.datatable = this.table.DataTable({
            'ajax': {
                'url': this.options.controller.api,
                'data': (d) => {
                    var data = $.extend({}, d, this.filter?.serializeJSON());
                    return d;
                }
            },
            'columns': this.options.columns,
            'rowCallback': (row, data) => {
                if ($.inArray(data.id, this.options.selected) !== -1) {
                    $(row).addClass('active');
                }
            },
        });

        this.initialize();
        this.callback = callback;
        //this.callback(this);
    }

    initialize() {
        var tbody = this.table.find('tbody');

        tbody.on('click', 'tr', (e) => {
            var target = $(e.currentTarget);

            if (target.hasClass('active')) {
                target.removeClass('active');
            } else {
                this.table.find('tr.active').removeClass('active');
                target.addClass('active');
            }
        });

        tbody.on('dblclick', 'tr', (e) => {
            var row = this.datatable.row(e.currentTarget);
            var data = row.data();
            location.href = `${this.options.controller.mvc}/Edit/${data.id}`;
        });

        tbody.on('change', 'input', (e) => {
            var inputs = this.table.find('input:checked');
            if (inputs.length > 0) {
                this.toolbar.find('button[data-action=delete]').enabled();
            } else {
                this.toolbar.find('button[data-action=delete]').disabled();
            }
        })
    }
}