$(() => {

    // store for bugitemstatus
    const bugItemStatusStore = DevExpress.data.AspNet.createStore({
        key: "id",
        loadUrl: `/BugItem/BugItemStatuses`,
        onBeforeSend(method, ajaxOptions) {
            ajaxOptions.xhrFields = { withCredentials: true };
        },
        success(result) {
            result.data,
            {
                totalCount: result.totalCount,
                summary: result.summary,
                groupCount: result.groupCount,
            };
        },
    });
    
    // grid instance for bugitem
    const grid = $("#bugItemGrid").dxDataGrid({
        dataSource: createStore({
            key: "id",
            loadUrl: `/BugItem/BugItems`,
            updateUrl: `/BugItem/UpdateBugItem`,
        }),
        repaintChangesOnly: true,
        editing: {
            refreshMode: "reshape",
            mode: "popup",
            allowUpdating: true,
            useIcons: true,
            popup: {
                title: "Bug Item Düzenle",
                showTitle: true,
                width: 400,
                height: 300,
            },
            form: {
                items: [
                    {
                        itemType: "group",
                        items: ["id","bugItemStatusId", "editDetails"],
                    },
                ],
            },
        },

        showBorders: true,
        showRowLines: true,
        remoteOperations: true,
        paging: {
            pageSize: 30,
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [20, 30, 40],
        },
        columns: [
            {
                dataField: "id",
                width: 50,
                fixed: true,
                editorOptions: {
                    disabled: true,
                },
            },
            {
                dataField: "erpWebSite",
                caption: "Url",
            },
            {
                dataField: "createdDate",
                caption: "Eklenme Tarihi",
                calculateCellValue(data) {
                    return moment(data.createdDate).format("DD.MM.YYYY HH:mm");
                },
            },
            {
                dataField: "bugItemStatusId",
                caption: "Durum",
                width: 100,
                fixed: true,
                lookup: {
                    dataSource: bugItemStatusStore,
                    valueExpr: "id",
                    displayExpr: "name",
                },
                cellTemplate: function (element, info) {
                    var color = "red";
                    switch (info.data.bugItemStatusId) {
                        case 2:
                            color = "orange";
                            break;
                        case 3:
                            color = "blue";
                            break;
                        case 4:
                            color = "green";
                            break;
                        case 5:
                            color = "gray";
                            break;
                    }
                    element.append("<b>" + info.text + "</b>").css("color", color);
                },
                editorOptions: {
                    width: 200,
                },
            },
            {
                dataField: "editBy",
                caption: "Düzenleyen",
                width: 300,
                height: 300
            },
            {
                dataField: "editDate",
                caption: "Düzenleme Tarihi",
                calculateCellValue(data) {
                    if (data.editDate == null)
                        return "";
                    return moment(data.editDate).format("DD.MM.YYYY HH:mm");
                },
            },
            {
                dataField: "editDetails",
                caption: "Düzenleme Detayı",
                visible: false,
                editorType: 'dxTextArea',
                editorOptions: {
                    height: 90,
                    width: 200,
                    maxLength: 200,
                },
            },
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
        grouping: {
            autoExpandAll: true,
        },
        masterDetail: {
            enabled: true,
            template: masterDetailTemplate,
        },
        onCellPrepared: function (e) {
            if (e.rowType === "data" && e.column.command === "edit") {
                var $links = e.cellElement.find(".dx-link");
                if ([4, 5].includes(e.row.data.bugItemStatusId)) {
                    $links.filter(".dx-link-edit").remove();
                }
            }
        }
    });
});

function masterDetailTemplate(_, masterDetailOptions) {
    return $("<div>").dxTabPanel({
        items: [
            {
                title: "Ekran Görüntüsü",
                template: createSSTabTemplate(masterDetailOptions.data),
            },
            {
                title: "Detay",
                template: createDetailsTabTemplate(masterDetailOptions.data),
            },
        ],
    });
}
function createSSTabTemplate(data) {
    return function () {
        return $(`
        <div class="form">
            <div class="card-body">
                <div class="form-group row">
                    <div class="col-lg-12">
                       <img src="${data.image}" style="width:80%" alt=""/>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-lg-6">
                        <label><b>${data.details}</b></label>
                    </div>
                </div>
            </div>
        </div>`);
    };
}
function createDetailsTabTemplate(data) {
    return function () {
        return $(`
        <div class="form"> 
            <div class="card-body">
                <div class="form-group row">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label><b>ID :</b>  ${data.id}</label>
                        </div>
                        <div class="form-group">
                            <label><b>ERP Web Site :</b>  ${data.erpWebSite}</label>
                        </div>
                        <div class="form-group">
                            <label><b>Oluşturan :</b>  ${data.createdBy}</label>
                        </div>
                        <div class="form-group">
                            <label><b>Oluşturulma Tarihi :</b>  ${moment(data.createdDate).format("DD.MM.YYYY HH:mm")}</label>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label><b>Durum :</b>  ${data.bugItemStatusName}</label>
                        </div>
                        <div class="form-group">
                            <label><b>Son İşlem Tarihi :</b>  ${moment(data.editDate).format("DD.MM.YYYY HH:mm")}</label>
                        </div>
                        <div class="form-group">
                            <label><b>Son İşlem Yapan :</b>  ${data.editBy}</label>
                        </div>
                        <div class="form-group">
                            <label><b>İşlem Açıklaması :</b>  ${data.editDetails}</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>`);
    };
}
