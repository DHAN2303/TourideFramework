var test_dataPath = "../../../assets/test_data/user_manage_test_data.json";
$(document).ready(function () {
    TourideDatatable.Create('ttt', true, true, test_dataPath,
        [{ data: 'first_name',
            render: function (data, type, row) {
                if (type === 'display') {
                    return '<span>' + data +'</span><div><input   class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                }
                return data;
            }},
            { data: 'last_name',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input   class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }},
            { data: 'sub_kind',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span>';
                    }
                    return data;
                }  
            },
            { data: 'email',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input   class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }},
            { data: 'phone_number',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input  class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }},
            { data: 'password',
                defaultContent: '<span>*****</span>'+
                                '<div><input   type="text" class="form-control" style="display:none"  name="password"></div>'
            },
            {
                data: 'actions',
                defaultContent: '<div class="d-flex justify-content-center m-3">'+
                    '<input type="button" class="btn btn-primary  mx-1" href="javascript:;" id="Edit" value="Edit"/>'+
                    '<input type="button" class="btn btn-danger" href="javascript:;" id="Delete" value="Delete"/>'+
                    '<input type="button" style="display: none" class="btn btn-danger" href="javascript:;" id="Cancel" value="Cancel"/>'+
                    '<input type="button" style="display: none" class="btn btn-danger mx-1" id="Ban" value="Engelle">'+
                    '<input type="button" style="display: none" class="btn btn-success" href="javascript:;" id="Done" value="Done"/>'+
                    '</div>',
                targets: -1
            }
        ]);

    $('body').on('click', '#ttt #Edit', function () {
        var row = $(this).closest("tr");
        $("td",row).each(function (){
            if ($(this).find("input").length > 0){
                $(this).find("input").show();
                $(this).find("span").hide();
            }
        });
        row.find("#Cancel").show();
        row.find("#Done").show();
        row.find("#Ban").show();
        row.find("#Edit").hide();
        row.find("#Delete").hide();
    });

    $('body').on('click', '#ttt #Cancel', function () {
        var row = $(this).closest("tr");
        $("td",row).each(function (){
            if ($(this).find("span").length > 0){
                $(this).find("input").hide();
                $(this).find("span").show();
            }
        });
        row.find("#Cancel").hide();
        row.find("#Done").hide();
        row.find("#Ban").hide();
        row.find("#Edit").show();
        row.find("#Delete").show();
    });
});