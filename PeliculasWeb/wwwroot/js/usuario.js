var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblUsuarios").DataTable({
        "ajax": {
            "url": "/Usuarios/GetTodosUsuarios",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "50%"},
            { "data": "nombre", "width": "50%" },
            { "data": "userName", "width": "50%" }
        ],
        "autoWidth": false, // Importante para que respete tu width
        "responsive": true, // Opcional: Para mejor visualización en móviles
        "scrollX": true,
    });
}