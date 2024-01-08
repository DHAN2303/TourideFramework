var test_dataPath = "../../../assets/test_data/content_complaint_test_data.json";
$(document).ready(function () {
    TourideDatatable.Create('ttt', true, true, test_dataPath,
        [{
            data: 'complaining_user',
            render: function (data, type, row) {
                if (type === 'display') {
                    return '<span>' + data + '</span><div><input   class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                }
                return data;
            }
        },
            {
                data: 'reported_user',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data + '</span><div><input   class="form-control" type="text" style="display:none"  name="sub_end_input" value="' + data + '"></div>';
                    }
                    return data;
                }
            },
            {
                data: 'content',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<button class="btn btn-primary" type="button" data-toggle="modal" data-target="#exampleModalCenter"> İçeriği Gör</button>';
                    }
                    return data;
                }
            },
            {
                data: 'content_shared_date',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data + '</span>';
                    }
                    return data;
                }
            },
            {
                data: 'content_complaints_number',
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<span>' + data + '</span>';
                    }
                    return data;
                }
            },
            {
                data: 'actions',
                defaultContent: '<div class="d-flex justify-content-center m-3">' +
                    '<input type="button" class="btn btn-primary mx-1" href="javascript:;" id="removeContentButton" value="Remove Content"/>' +
                    '<input type="button" class="btn btn-danger" href="javascript:;" id="removeReportButton" value="Remove Report"/>' +
                    '</div>',
                targets: -1
            }


        ]);
});   
    

// start get user ıdentity kind
let complaintFilterDateSelection = document.getElementById('complaintFilterDateSelection');
complaintFilterDateSelection.onchange = (_) =>{
    let selecetedIndex = complaintFilterDateSelection.selectedIndex;
    document.getElementById("complaintFilterFirstDate").disabled=selecetedIndex !== 3;
    document.getElementById("complaintFilterSecondDate").disabled=selecetedIndex !== 3;
}
// end get user ıdentity kind

// start get user ıdentity kind
let complaintFilterComplainingUserSelection = document.getElementById('complaintFilterComplainingUserSelection');
complaintFilterComplainingUserSelection.onchange = (_) =>{
    let selecetedIndex = complaintFilterComplainingUserSelection.selectedIndex;
    document.getElementById("complaintFilterComplainingUserIdentity").disabled=selecetedIndex === 0;
}
// end get user ıdentity kind

// start get user ıdentity kind
let complaintFilterReportedUserSelection = document.getElementById('complaintFilterReportedUserSelection');
complaintFilterReportedUserSelection.onchange = (_) =>{
    let selecetedIndex = complaintFilterReportedUserSelection.selectedIndex;
    document.getElementById("complaintFilterReportedUserIdentity").disabled=selecetedIndex === 0;
}
// end get user ıdentity kind

$('#complaintContentModal').on('shown.bs.modal', function () {
    $('#myInput').trigger('focus')
})