var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblPeliculas").DataTable({
        "ajax": {
            "url": "/Peliculas/GetTodasPeliculas",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "0%", "visible": false }, // Ajustar mejor
            { "data": "clasificacion", "width": "10%" },
            { "data": "duracion", "width": "10%" },
            {
                "data": "fechaCreacion",
                "width": "15%",
                "render": function (data) {
                    if (data)
                        return moment(data).format('YYYY/MM/DD');
                    return "";
                }
            },
            {
                "data": "id",
                "render": function (data, type, row, meta) {
                    return `<div class="text-center">
                        <a href="/Peliculas/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                            <i class="fas fa-edit"></i> Editar
                        </a>
                        &nbsp;
                        <a onclick="Delete('/Peliculas/Delete/${data}')" class="btn btn-danger text-white" style="cursor:pointer;">
                            <i class="fas fa-trash-alt"></i> Borrar
                        </a>
                        &nbsp;
                        <a onclick="VerDetalle(${meta.row})" class="btn btn-info text-white" style="cursor:pointer;">
                            <i class="fas fa-info-circle"></i> Detalle
                        </a>
                    </div>`;
                }, "width": "30%"
            }
        ],
        "autoWidth": false, // Importante para que respete tu width
        "responsive": true, // Opcional: Para mejor visualización en móviles
        "scrollX": true, // Permite scroll horizontal si es necesario
        "columnDefs": [
            { "targets": [2], "visible": false, "searchable": false } // Asegura que no ocupe espacio
        ]
    });
}

function Delete(url) {
    swal({
        title: "Esta seguro de querer borrar el registro?",
        text: "Esta acción no puede ser revertida!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function VerDetalle(index) {
    var data = dataTable.row(index).data(); // Obtener el dato actual  rutaIMagen
    
    function showMovie(index) {
        var data = dataTable.row(index).data();
        
        if (!data) {
            Swal.fire({
                title: "No hay más registros",
                icon: "info",
                confirmButtonText: "OK"
            });
            return;
        }

        var imagen = data.rutaIMagen ? data.rutaIMagen : '/images/default.png';
        
        Swal.fire({
            title: `<strong>${data.nombre}</strong>`,
            html:
                `<div class="text-start">
                    <p><strong>Descripción:</strong> ${data.descripcion}</p>
                    <p><strong>Clasificación:</strong> ${data.clasificacion}</p>
                    <p><strong>Duración:</strong> ${data.duracion} min</p>
                    <p><strong>Fecha de Creación:</strong> ${moment(data.fechaCreacion).format('YYYY/MM/DD')}</p>
                    <img src="${imagen}" alt="Imagen" class="img-fluid rounded mt-3" style="max-height:300px;">
                </div>`,
            icon: 'info',
            showCloseButton: true,
            showCancelButton: true,
            cancelButtonText: '⬅️ Anterior',
            confirmButtonText: 'Siguiente ➡️',
            focusConfirm: false,
            background: '#2d2d2d',
            color: '#fff'
        }).then((result) => {
            if (result.isConfirmed) {
                showMovie(index + 1); // Ir al siguiente
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                showMovie(index - 1); // Ir al anterior
            }
        });
    }

    showMovie(index);
}
