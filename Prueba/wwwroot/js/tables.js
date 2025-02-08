// Tabla

function inicializarTable(tableId, inputID) {
    var table = $(tableId).DataTable({
      dom: "Blfrtip",
      lengthChange: false,
      bProcessing: true,
      buttons: [""],
      paginate: true,
      searching: true,
      info: false,
      row: {
        className: "selected",
      },
      language: {
        buttons: {
          copy: "Copiar",
          copyTitle: "Copiado al portapapeles",
          copyKeys:
            "Presione <i>ctrl</i> o <i>\u2318</i> + <i>C</i> para copiar los datos de la tabla a su portapapeles. <br><br>Para cancelar, haga clic en este mensaje o presione Esc.",
          copySuccess: {
            _: "%d filas copiadas",
            1: "1 fila copiada",
          },
        },
        sLoadingRecords: "Cargando...",
        sProcessing: "Procesando...",
        sEmptyTable: "No se encontraron registros",
        info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
        sInfoEmpty: "Mostrando 0 a 0 de 0 entradas",
        sInfoFiltered: "",
        paginate: {
          previous: `<i class="fa fa-chevron-left" style="color:#222222;"></i>`,
          next: `<i class="fa fa-chevron-right" style="color:#222222;"></i>`,
        },
        lengthMenu: "Mostrar _MENU_ registros",
        info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
        search: ``,
        zeroRecords: "No se encontraron registros.",
      },
      scrollX: true,
      autoWidth: false,
    });
  
    // Escuchar el evento de entrada en el campo de búsqueda personalizada
    $(inputID).on("input", function () {
      table.search(this.value).draw(); // Filtrar la tabla según el valor del input
    });
  
    $(window).on("responsive-resize", function () {
      table.columns.adjust().draw();
    });
  }
  