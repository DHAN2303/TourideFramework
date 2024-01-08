var test_dataPath = "../../../assets/test_data/coupon_test_data.json";
$(document).ready(function () {
    TourideDatatable.Create('ttt',true,true,test_dataPath,
        [
            {
                data: 'coupon_name',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input   class="form-control" type="text" style="display:none"  name="coupon_name" value="' + data + '"></div>';
                    }
                    return data;
                }
            },
            {
                data: 'coupon_created_date',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input   class="form-control" type="date" style="display:none"  name="coupon_created_date" value="' + data + '"></div>';
                    }
                    return data;
                }
            },
            {
                data: 'coupon_end_date',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input   class="form-control" type="date" style="display:none"  name="coupon_end_date" value="' + data + '"></div>';
                    }
                    return data;
                }
            },
            {
                data: 'coupon_sub_kind',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var selectedBasic = (data === 'Basic') ? 'selected' : '';
                        var selectedPremium = (data === 'Premium') ? 'selected' : '';
                        var selectedPremiumPlus = (data === 'PremiumPlus') ? 'selected' : '';
                        return '<select style="display: none" class="custom-select" required>' +
                            '    <option class="form-control" value="1" ' + selectedBasic + '>Basic</option>' +
                            '    <option class="form-control" value="2" ' + selectedPremium + '>Premium</option>' +
                            '    <option class="form-control" value="2" ' + selectedPremiumPlus + '>PremiumPlus</option>' +
                            '</select>' +
                            '<span>' + data + '</span>';
                    }
                    return data;
                }
            },
            {
                data: 'coupon_belonging_to',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var selectedAll = (data === 'All') ? 'selected' : '';
                        var selectedClub = (data === 'Club') ? 'selected' : '';
                        var selectedGroup = (data === 'Group') ? 'selected' : '';
                        var selectedPersonal = (data === 'Personal') ? 'selected' : '';
                        return '<select style="display: none" class="custom-select" required>' +
                            '<option class="form-control" value="1" ' + selectedAll + '>Club</option>' +
                            '<option class="form-control" value="1" ' + selectedClub + '>Club</option>' +
                            '<option class="form-control" value="2" ' + selectedGroup + '>Group</option>' +
                            '<option class="form-control" value="2" ' + selectedPersonal + '>Personal</option>' +
                            '</select>' +
                            '<span>' + data + '</span>';
                    }
                    return data;
                }
            },
            {
                data: 'coupon_belonging_name',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data +'</span><div><input   class="form-control" type="text" style="display:none"  name="coupon_belonging_name" value="' + data + '"></div>';
                    }
                    return data;
                }
            },
            {
                data: 'actions',
                defaultContent: '<input type="button" class="btn btn-primary mx-1" id="Edit" value="Edit"/>'+
                                '<input type="button" class="btn btn-danger" id="Delete" value="Delete"/>'+
                                '<input type="button" style="display: none" class="btn btn-danger mx-1" id="Cancel" value="Cancel"/>'+
                                '<input type="button" style="display: none" class="btn btn-success" id="Done" value="Done"/>'
            },
        ]);
});

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


// start coupon create belonging action
let couponCreateBelongingIdentitySelection = document.getElementById('couponCreateBelongingIdentitySelection');
couponCreateBelongingIdentitySelection.onchange = (_) =>{
    let couponCreateBelongingIdentitySelectionselecetedIndex = couponCreateBelongingIdentitySelection.selectedIndex;
    document.getElementById("couponCreateCustomEndDateInput").disabled=couponCreateBelongingIdentitySelectionselecetedIndex !== 4;
}
// end coupon create start date action

// start coupon create start date action
let couponCreateEndDateSelection = document.getElementById('couponCreateEndDateSelection');
couponCreateEndDateSelection.onchange = (_) =>{
    let couponCreateEndDateSelectionselecetedIndex = couponCreateEndDateSelection.selectedIndex;
    document.getElementById("couponCreateCustomEndDateInput").disabled=couponCreateEndDateSelectionselecetedIndex !== 4;
}
// end coupon create start date action


// start coupon filter start date action
let couponFilterStartDateSelection = document.getElementById('couponFilterStartDateSelection');
couponFilterStartDateSelection.onchange = (_) =>{
    let couponFilterStartDateSelectionselecetedIndex = couponFilterStartDateSelection.selectedIndex;
    document.getElementById("couponFilterStartDateInput").disabled=couponFilterStartDateSelectionselecetedIndex !== 4;
    document.getElementById("couponFilterEndDateSelection").disabled=couponFilterStartDateSelectionselecetedIndex !== 4;
}
// end coupon filter start date action

// start coupon filter end date action
let couponFilterEndDateSelection = document.getElementById('couponFilterEndDateSelection');
couponFilterEndDateSelection.onchange = (_) =>{
    let couponFilterEndDateSelectionselecetedIndex = couponFilterEndDateSelection.selectedIndex;
    document.getElementById("couponFilterEndDateInput").disabled=couponFilterEndDateSelectionselecetedIndex !== 4;
}
// start coupon filter end date action