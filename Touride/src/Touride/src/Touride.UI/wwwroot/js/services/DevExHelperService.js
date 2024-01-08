function isNotEmpty(value) {
    return value !== undefined && value !== null && value !== "";
}

function showLoadPanel() {

    return $("#loadPanel").dxLoadPanel("instance").show();;
}


function hideLoadPanel() {

    return $("#loadPanel").dxLoadPanel("instance").hide();;
}

function showToast(messageType, isSuccess) {
    var message = '';
    if (isSuccess == 0) {
        message = `İşlem sırasında hata oluştu !`;
        messageType = 'error';
    }
    if (messageType == 'insert') {
        message = `Kaydetme işlemi başarılı !`;
        messageType = 'success';
    }
    else if (messageType == 'delete') {
        message = `Silme işlemi başarılı !`;
        messageType = 'warning';
    }
    else if (messageType == 'update') {
        message = `Güncelleme işlemi başarılı !`;
        messageType = 'success';
    }

    DevExpress.ui.notify({
        message,
        width: 450,
    },
        messageType,
        3000);
}

function createStore(storeParams) {
    const store = new DevExpress.data.CustomStore({
        key: storeParams.key,
        load(loadOptions) {
            const deferred = $.Deferred();
            const args = {};
            [
                'skip',
                'take',
                'requireTotalCount',
                'requireGroupCount',
                'sort',
                'filter',
                'totalSummary',
                'group',
                'groupSummary',
            ].forEach((i) => {
                if (i in loadOptions && isNotEmpty(loadOptions[i])) {
                    args[i] = JSON.stringify(loadOptions[i]);
                }
            });
            $.ajax({
                url: storeParams.loadUrl,
                dataType: 'json',
                data: args,
                timeout: 60000,
            }).done(function (result) {
                deferred.resolve(result.data, {
                    totalCount: result.totalCount,
                    summary: result.summary,
                    groupCount: result.groupCount,
                });
            }).fail(function (e) {
                showToast('', 0);
                deferred.reject('Data Loading Error');
            }).always(function (e) {
            });

            return deferred.promise();
        },
        insert(values) {
            var deferred = $.Deferred();
            showLoadPanel();
            $.ajax({
                url: storeParams.insertUrl,
                method: "POST",
                data: {
                    values: JSON.stringify(values),
                }
            }).done(function (response) {
                showToast('insert', 1);
                deferred.resolve(response);
            }).fail(function (e) {
                showToast('', 0);
                deferred.reject("Insert failed");
            }).always(function (e) {
                hideLoadPanel();
            });

            return deferred.promise();
        },
        update(key, values) {
            var deferred = $.Deferred();
            showLoadPanel();
            $.ajax({
                url: storeParams.updateUrl,
                method: "PUT",
                data: {
                    key,
                    values: JSON.stringify(values),
                }
            }).done(function (response) {
                showToast('update', 1);
                deferred.resolve(response);
            }).fail(function (e) {
                showToast('', 0);
                deferred.reject("Update failed");
            }).always(function (e) {
                hideLoadPanel();
            });

            return deferred.promise();
        },
        remove: function (key) {
            var deferred = $.Deferred();
            showLoadPanel();
            $.ajax({
                url: storeParams.deleteUrl + encodeURIComponent(key),
                method: "DELETE"
            }).done(function (response) {
                showToast('delete', 1);
                deferred.resolve(response);
            }).fail(function (e) {
                showToast('', 0);
                deferred.reject("Deletion failed");
            }).always(function (e) {
                hideLoadPanel();
            });

            return deferred.promise();
        },
    });

    return store;
}
