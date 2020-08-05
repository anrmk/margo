class DocumentController {
    constructor(options, callback = (target) => { }) {
        this.options = $.extend(true, {
            'table': '#table',
            'filter': '#table-filter',
            'controller': { 'list': '', 'delete': '' },
            'columns': [],
            'selected': [],
            'onRowClick': (e, data) => { },
            'onDblRowClick': (e, data) => { },
            'onRowChange': (e, data) => { }
        }, options);

        this.table = $(this.options.table);
        this.toolbar = $(this.table.data('toolbar'));
        this.filter = $(this.options.filter);
        this.datatable = this.table.DataTable({
            'ajax': {
                'url': this.options.controller.list,
                'contentType': 'application/json; charset=utf-8',
                'data': (d) => {
                    var options = {
                        'draw': d.draw,
                        'start': d.start,
                        'length': d.length,
                        'search': {
                            'value': d.search.value
                        },
                    }

                    return options;
                    // return d; $.extend({}, d, this.filter.serializeJSON());
                }
            },
            'columns': this.options.columns,
            'rowCallback': (row, data) => {
                if ($.inArray(data.id, this.options.selected) !== -1) {
                    $(row).find('input[type=checkbox]').attr('checked', true)
                    //$(row).addClass('active');
                }
            }
        }).on('draw', (e, settings) => {
            $(e.currentTarget).find('a[data-request=ajax]').ajaxClick();
        });

        this.initialize();
        this.callback = callback;
    }

    reload(paging) {
        this.datatable.draw(paging);
    }

    initialize() {
        var tbody = this.table.find('tbody');
        this.toolbar.find('button[data-action=delete]').on('click', (e) => this.remove(e));

        tbody.on('click', 'tr', (e) => {
            var target = $(e.currentTarget);
            var row = this.datatable.row(target);

            if (target.hasClass('active')) {
                target.removeClass('active');
            } else {
                this.table.find('tr.active').removeClass('active');
                target.addClass('active');
            }
            this.options.onRowClick(e, row.data());
        });

        tbody.on('dblclick', 'tr', (e) => {
            var row = this.datatable.row(e.currentTarget);
            this.options.onDblRowClick(e, row.data());
        });

        tbody.on('change', 'input', (e) => {
            var $input = $(e.currentTarget);
            var row = this.datatable.row($($input).parents('tr'));

            //var value = Number($input.val());
            var value = $.fn.isGuid($input.val()) ? $input.val() : Number($input.val());
            var index = $.inArray(value, this.options.selected);

            if (index === -1) {
                this.options.selected.push(value);
            } else {
                this.options.selected.splice(index, 1);
            }

            if (this.options.selected.length > 0) {
                this.toolbar.find('button[data-action=delete]').enabled();
            } else {
                this.toolbar.find('button[data-action=delete]').disabled();
            }

            this.options.onRowChange(e, row.data());
        })
    }

    remove(e) {
        if (!confirm('Are you sure want to delete this?')) { return false; }

        var options = {
            'url': this.options.controller.delete,
            'data': { 'id': this.options.selected },
            'traditional': true,
            'contentType': 'application/json; charset=utf-8',
            'complete': (jqXHR, status) => {
                if (status === 'success') {
                    var ids = this.options.selected.map(x => '#row_' + x);
                    this.datatable.rows(ids).remove().draw(false);
                    this.options.selected = [];
                }
            }
        }

        $.ajax(options);
    }
}