$(() => {
  
    $('#grid').dxDataGrid({
        dataSource: DevExpress.data.AspNet.createStore({
            key: 'id',
            loadUrl: `/AuditLogs/GetAuditLogs`,
            onBeforeSend(method, ajaxOptions) {
                ajaxOptions.xhrFields = { withCredentials: true };
            },
            success(result) {
                result.data, {
                    totalCount: result.totalCount,
                    summary: result.summary,
                    groupCount: result.groupCount,
                }
            },
        }),
        //dataSource: store,
        showBorders: true,
        remoteOperations: true,
        paging: {
            pageSize: 20,
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [8, 12, 20],
        },
        columns: [{
            dataField: 'id',
        }, {
            dataField: 'eventTime'
        }, {
            dataField: 'eventType'
        }, {
            dataField: 'pk1'
        }, {
            dataField: 'pk2'
        }, {
            dataField: 'data'
        }, {
            dataField: 'database'
        }, {
            dataField: 'schema'
        }, {
            dataField: 'table'
        }, {
            dataField: 'user'
        }, {
            dataField: 'correlationId'
        }, {
            dataField: 'correlationSeq'
        }
        ],
        filterRow: {
            visible: true,
        },
        headerFilter: {
            visible: true,
        },
        groupPanel: {
            visible: true,
        },
        //scrolling: {
        //    mode: 'virtual',
        //},
        height: 900,
        editing: {
            mode: 'popup',
            texts: {
                editRow: "Detay",
            },
            allowUpdating: true,
            allowDeleting: false,
            popup: {
                title: 'Detail',
                showtitle: true,
                width: 700,
                height: 525,

            },
            form: {
                items: [{
                    itemType: 'group',
                    colCount: 1,
                    colSpan: 2,
                    items: [
                        {
                            dataField: 'eventTime', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'eventType', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'pk1', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'pk2', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'database', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'schema', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'table', editorOptions: {
                                disabled: true,
                            },
                        },

                        {
                            dataField: 'user', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'correlationId', editorOptions: {
                                disabled: true,
                            },
                        },
                        {
                            dataField: 'correlationSeq', editorOptions: {
                                disabled: true,
                            },
                        },

                        {
                            dataField: 'data',
                            editorType: 'dxTextArea',
                            colSpan: 2,
                            editorOptions: {
                                height: 200,
                                disabled: true,
                            },
                        }],
                }],
            },
        },
        grouping: {
            autoExpandAll: true,
        }
    });
});