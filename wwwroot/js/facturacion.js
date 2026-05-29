// Función para actualizar el precio cada vez que se cambia el artículo o la cantidad
function actualizarPrecio() {

    const articulo = document.getElementById("articuloSelect");
    const cantidad = document.getElementById("cantidadInput");
    const precio = document.getElementById("precioInput");

    const selectedOption = articulo.options[articulo.selectedIndex];
    const salePrice = parseFloat(selectedOption.dataset.price || 0);
    const cantidadValor = parseInt(cantidad.value || 1);
    const total = salePrice * cantidadValor;

    precio.value = `$ ${total.toLocaleString('es-AR')}`;
}

// Función para actualizar el resumen cada vez que se agrega o elimina un artículo
function actualizarResumen() {
    let cantidadArticulos = 0;
    let subtotal = 0;

    $('#detalleFacturaBody tr').each(function () {
        cantidadArticulos += parseInt($(this).data('cantidad'));
        subtotal += parseFloat($(this).data('total'));
    });

    // Descuento 10%
    let descuento = subtotal * 0.10;
    let total = subtotal - descuento;

    $('#resumenArticulos').text(cantidadArticulos);
    $('#resumenSubtotal').text(`$ ${subtotal.toLocaleString('es-AR')}`);
    $('#resumenDescuento').text(`$ ${descuento.toLocaleString('es-AR')}`);
    $('#resumenTotal').text(`$ ${total.toLocaleString('es-AR')}`);
}

// Inicialización de select2 y eventos
$(document).ready(function () {

    // Articulos
    $('#articuloSelect').select2({
        placeholder: "Buscar artículo...",
        width: '100%',
        allowClear: false
    });

    // Cuando cambia articulo
    $('#articuloSelect').on('change', function () {
        actualizarPrecio();
    });

    // Cuando cambia cantidad
    document
        .getElementById("cantidadInput")
        .addEventListener("input", actualizarPrecio);

    // Inicializar precio
    actualizarPrecio();

    $('#btnAgregarArticulo').on('click', function () {

        const articuloSelect = $('#articuloSelect');
        const articuloId = articuloSelect.val();

        // Validar
        if (!articuloId) {
            alert('Seleccione un artículo');
            return;
        }

        // Option seleccionada
        const selectedOption = articuloSelect.find(':selected');
        const articuloTexto = selectedOption.text();
        const precioUnitario = parseFloat(selectedOption.data('price')) || 0;
        const cantidad = parseInt($('#cantidadInput').val()) || 1;
        const total = precioUnitario * cantidad;

        // Crear fila
        const fila = `
                            <tr data-total="${total}" data-cantidad="${cantidad}">

                            <td>
                            <div class="product-cell">

                            <span class="product-icon">
                            ${articuloTexto.charAt(0)}
                            </span>

                            <div>

                            <p class="product-title">
                            ${articuloTexto}
                            </p>

                            <p class="product-meta">
                            Artículo agregado
                            </p>

                            </div>

                            </div>
                            </td>

                            <td>
                            <span class="qty-badge">
                            ${cantidad}
                            </span>
                            </td>

                            <td class="money">
                            $ ${precioUnitario.toLocaleString('es-AR')}
                            </td>

                            <td class="money">
                            $ ${total.toLocaleString('es-AR')}
                            </td>

                            <td>
                            <a href="#" class="icon-btn btnEliminar">
                            <i class="bi bi-trash"></i>
                            </a>
                            </td>

                            </tr>
                        `;

        $('#detalleFacturaBody').append(fila);// Agregar fila
        actualizarResumen(); // Actualizo resumen
        $('#cantidadInput').val(1);// Resetear formulario
        $('#precioInput').val('');
        $('#articuloSelect').val(null).trigger('change');

    });

    // Evento para eliminar artículo
    $(document).on('click', '.btnEliminar', function (e) {
        e.preventDefault();
        $(this).closest('tr').remove();
        actualizarResumen();
    });
});

$('#clientSelect').select2({
    placeholder: "Buscar Cliente...",
    width: '100%',
    allowClear: false
});

$('#clientSelect').on('change', function () {

    const clienteId = parseInt($(this).val());

    const clientes = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(Model.Clientes));
    const cliente = clientes.find(c => c.Id == clienteId);
    console.log(cliente)

    if (!cliente) {
        $('#nombreCliente').val('');
        $('#dniCliente').val('');
        $('#listaPrecioCliente').val('');
        return;
    }

    $('#nombreCliente').val(cliente.Nombre);
    $('#dniCliente').val(cliente.DNI);
    $('#listaPrecioClienteId').val(cliente.PriceListId);

    console.log(cliente.PriceListId)
});