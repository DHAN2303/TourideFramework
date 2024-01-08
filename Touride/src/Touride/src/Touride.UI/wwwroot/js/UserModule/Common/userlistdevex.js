$(() => {

    const store = createStore({
        key: 'id',
        loadUrl: '/User/Get',
        insertUrl: '/User/insert'
    });

    $('#userListGrid').dxDataGrid({
        dataSource: store,
        showBorders: true,
        showRowLines: true,
        remoteOperations: true,
        paging: {
            pageSize: 10,
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [10, 20, 30],
        },
        columns: [
            {
                dataField: 'id',
                width: 200,
                fixed: true,
                editorOptions: {
                    disabled: true,
                },
            },
            {
                dataField: 'name'
            },
            {
                dataField: 'surname',
                caption: "Surname",
                editorType: 'dxTextArea',
                editorOptions: {
                    height: 90,
                    width: 200,
                    maxLength: 200,
                },
            },
            {
                dataField: 'phone',
                caption: "Phone",
                editorType: 'dxTextArea',
                editorOptions: {
                    height: 90,
                    width: 200,
                    maxLength: 200,
                },
            },
            {
                dataField: 'gender',
                caption: "Gender",
                editorType: 'dxTextArea',
                editorOptions: {
                    height: 90,
                    width: 200,
                    maxLength: 200,
                },
            },
            {
                dataField: 'isDeleted',
                dataType: 'boolean'
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
        editing: {
            mode: 'popup', /* row */
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            useIcons: true,
            popup: {
                title: 'User List',
                showTitle: true,
                maxWidth: 700,
                maxHeight: 525,
            },
            form: {
                items: [
                    {
                        itemType: "group",
                        items: ["id", "name", "description", "isActive"],
                    },
                ],
            },
        },
        grouping: {
            autoExpandAll: true,
        }
    });
});
