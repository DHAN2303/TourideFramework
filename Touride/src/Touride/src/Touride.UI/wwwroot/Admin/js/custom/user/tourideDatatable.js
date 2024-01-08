{
    TourideDatatable = {
        Create: function (id, ordering, paging, data_path,columns) {
            /*let table = new DataTable('#' + id);*/

            //ajax: 'https://reqres.in/api/users?page=2'
            new DataTable('#' + id, {
                ajax:{
                    dataSrc: 'data',
                    url: data_path 
                },
                info: false,
                ordering: ordering,
                paging: paging,
                columns: columns,
                searching: true,
                scrollX: true,
                responsive: true
            });
        }
    }
}
