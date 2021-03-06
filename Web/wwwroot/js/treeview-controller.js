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

        this.target = $(this.options.target).addClass('fancytree-connectors');
        this.toolbar = $(`<div class='ui stackable grid'>
                            <div class='row'>
                                <div class='eight wide column' data-id='toolbar'></div>
                                <div class='right aligned eight wide column'>
                                    <div class='ui form'>
                                        <input type='search' name='search' incremental placeholder='Search text' autocomplete='off' />
                                    </div>
                                </div>
                            </div>
                        </div>`).insertBefore(this.target);
        $(this.target.data('toolbar')).appendTo(this.toolbar.find('div[data-id=toolbar]'));

        this.filter = $(this.options.filter);
        this.treeview = this.target.fancytree({
            'checkbox': true,
            'icon': false,
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
            'select': (e, jqXHR) => {
                this.options.selected = jqXHR.tree.getSelectedNodes().map(x => x.key);
                if (this.options.selected.length > 0) {
                    this.toolbar.find('button[data-action=delete]').enabled();
                } else {
                    this.toolbar.find('button[data-action=delete]').disabled();
                }
            }
        });

        this.initialize();
        this.callback = callback;
    }

    initialize() {
        var tbody = this.target.find('tbody');
        this.toolbar.find('button[data-action=delete]').on('click', (e) => this.remove(e));

        this.toolbar.find('input[name=search]').on('change search', (e) => {
            if (e.type === "change" && e.target.onsearch !== undefined) {
                // We fall back to handling the change event only if the search event is not supported.
                return;
            }
            var n,
                tree = $.ui.fancytree.getTree(),
                match = $.trim($(e.target).val());

            // Pass a string to perform case insensitive matching
            n = tree.filterNodes(match, { mode: "hide" });
            // This will adjust the start value in case the filtered row set
            // is not inside the current viewport
            // tree.setViewport();

            $("span.matches").text(n ? "(" + n + " matches)" : "");
        });
    }

    reload() {
        this.treeview.fancytree('getTree').reload();
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
                    this.reload();
                    //selNodes = tree.getSelectedNodes();

                    //selNodes.forEach(function (node) {
                    //    while (node.hasChildren()) {
                    //        node.getFirstChild().moveTo(node.parent, "child");
                    //    }
                    //    node.remove();
                    //});

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