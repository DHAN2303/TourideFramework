var test_dataPath = "../../../assets/test_data/sub_test_data.json";
$(document).ready(function () {
    TourideDatatable.Create('ttt', true, true, test_dataPath,
        [{ data: 'first_name',
            render: function (data, type, row) {
                if (type === 'display') {
                    return '<span>' + data +'</span><div><input class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                }
                return data;
            }},
            { data: 'last_name',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }},
            { data: 'email',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }},
            { data: 'sub_kind',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var selectedBasic = (data === 'Basic') ? 'selected' : '';
                        var selectedPremium = (data === 'Premium') ? 'selected' : '';
                        var selectedPremiumPlus = (data === 'PremiumPlus') ? 'selected' : '';
                        var selectHtml = '<select style="display: none" class="custom-select" required>' +
                            '    <option class="form-control" value="1" ' + selectedBasic + '>Basic</option>' +
                            '    <option class="form-control" value="2" ' + selectedPremium + '>Premium</option>' +
                            '    <option class="form-control" value="2" ' + selectedPremiumPlus + '>PremiumPlus</option>' +
                            '</select>'+
                            '<span>'+ data +'</span>';
                        return selectHtml;
                    }
                    return data;
                }},
            { data: 'sub_period'},
            {
                data: 'sub_end',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input class="form-control" type="date" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }
            },
            {
                data: 'choose',
                defaultContent: '<div class="d-flex justify-content-center m-3">'+
                    '<input type="button" class="btn btn-primary mx-1" href="javascript:;" id="Edit" value="Edit"/>'+
                    '<input type="button" class="btn btn-danger" href="javascript:;" id="Delete" value="Delete"/>'+
                    '<input type="button" style="display: none" class="btn btn-primary mx-1" href="javascript:;" id="Cancel" value="Cancel"/>'+
                    '<input type="button" style="display: none" class="btn btn-success" href="javascript:;" id="Done" value="Done"/>'+
                    '</div>',
                targets: -1
            }
        ]);

    $('body').on('click', '#ttt #Edit', function () {
        var row = $(this).closest("tr");
        $("td",row).each(function (){
            if ($(this).find("select").length > 0){
                $(this).find("select").show();
                $(this).find("span").hide();
            }
            if ($(this).find("input").length > 0){
                $(this).find("input").show();
                $(this).find("span").hide();
            }
        });
        row.find("#Cancel").show();
        row.find("#Done").show();
        row.find("#Edit").hide();
        row.find("#Delete").hide();
    });

    $('body').on('click', '#ttt #Cancel', function () {
        var row = $(this).closest("tr");
        $("td",row).each(function (){
            if ($(this).find("select").length > 0){
                $(this).find("select").hide();
                $(this).find("span").show();
            }
            if ($(this).find("span").length > 0){
                $(this).find("input").hide();
                $(this).find("span").show();
            }
        });
        row.find("#Cancel").hide();
        row.find("#Done").hide();
        row.find("#Edit").show();
        row.find("#Delete").show();
    });
});