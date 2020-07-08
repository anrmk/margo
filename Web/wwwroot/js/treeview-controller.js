﻿class TreeViewController {
    constructor(options, callback = (target) => { }) {
        this.options = $.extend(true, {
            'target': '#treeview',
            'filter': '#treeview-filter',
            'controller': { 'list': '', 'delete': '' },
            'columns': [],
            'selected': [],
            'onRowClick': (e, data) => { },
            'onDblRowClick': (e, data) => { }
        }, options);

        this.target = $(this.options.target);
        this.toolbar = $(this.target.data('toolbar'));
        this.filter = $(this.options.filter);
        this.treeview = this.target.fancytree({
            'checkbox': true,
            'source': {
                'url': this.options.controller.list,
                'contentType': 'application/json; charset=utf-8',
                'data': (d) => {
                    return { 'id': d.id };
                }
            },
            'postProcess': (e, jqXHR) => {
                jqXHR.result = this._convert(jqXHR.response.data);
            },
            'activate': (e, jqXHR) => {
                this.options.onRowClick(e, jqXHR.node.data);
            },
            'dblclick': (e, jqXHR) => {
                this.options.onDblRowClick(e, jqXHR.node.data);
            },
        });

        this.initialize();
        this.callback = callback;
        //this.callback(this);
    }

    initialize() {
        var tbody = this.target.find('tbody');
        this.toolbar.find('button[data-action=delete]').on('click', (e) => this.remove(e));

        //tbody.on('click', 'tr', (e) => {
        //    var target = $(e.currentTarget);
        //    var row = this.datatable.row(target);

        //    if (target.hasClass('active')) {
        //        target.removeClass('active');
        //    } else {
        //        this.treeview.find('tr.active').removeClass('active');
        //        target.addClass('active');
        //    }
        //    this.options.onRowClick(e, row.data());
        //});

        //tbody.on('dblclick', 'tr', (e) => {
        //    var row = this.datatable.row(e.currentTarget);
        //    this.options.onDblRowClick(e, row.data());
        //});

        //tbody.on('change', 'input', (e) => {
        //    var $input = $(e.currentTarget);
        //    var value = Number($input.val());
        //    var index = $.inArray(value, this.options.selected);

        //    if (index === -1) {
        //        this.options.selected.push(value);
        //    } else {
        //        this.options.selected.splice(index, 1);
        //    }

        //    if (this.options.selected.length > 0) {
        //        this.toolbar.find('button[data-action=delete]').enabled();
        //    } else {
        //        this.toolbar.find('button[data-action=delete]').disabled();
        //    }
        //})
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
                    this.treeview.rows(ids).remove().draw(false);
                    this.options.selected = [];
                }
            }
        }

        $.ajax(options);
    }

    _convert(data) {
        var parent, nodeMap = {};
        $.each(data, function (i, c) {
            nodeMap[c.id] = c;
        });

        // Pass 2: adjust fields and fix child structure
        data = $.map(data, function (c) {
            // Rename 'key' to 'id'
            c.key = c.id;
            // delete c.id;

            c.title = c.name;
            c.expanded = true;

            // Set checkbox for completed tasks
            //c.selected = (c.status === "completed");
            // Check if c is a child node
            if (c.parentId) {
                // add c to `children` array of parent node
                parent = nodeMap[c.parentId];
                if (parent.children) {
                    parent.children.push(c);
                } else {
                    parent.children = [c];
                }
                return null;  // Remove c from childList
            }
            return c;  // Keep top-level nodes
        });

        // Pass 3: sort children by 'position'
        $.each(data, function (i, c) {
            if (c.children && c.children.length > 1) {
                c.children.sort(function (a, b) {
                    return ((a.position < b.position) ? -1 : ((a.position > b.position) ? 1 : 0));
                });
            }
        });

        return data;
    }
}