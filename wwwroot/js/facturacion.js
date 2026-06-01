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

    // Evento para confirmar factura
    $('#btnConfirmarFactura').click(function () {

        let saleDetail = [];

        $('#detalleFacturaBody tr').each(function () {
            const itemId = $(this).data('itemid');
            const codeItem = $(this).data('codeitem');
            const nameItem = $(this).data('nameitem');
            const quantity = parseInt($(this).data('cantidad'));
            const precioUnitario = parseFloat($(this).data('precio'));
            const total = parseFloat($(this).data('total'));

            saleDetail.push({
                itemId: itemId,
                codeItem: codeItem,
                nameItem: nameItem,
                quantity: quantity,
                precioUnitario: precioUnitario,
                total: total
            });
        });

        const sale = {
            clientId: $('#clientSelect').val() || null,
            nameClient: $('#clientSelect option:selected').text(),
            dni: $('#dniCliente').val(),
            priceListId: $('#listaPrecioSelect').val() || 0,

            subtotal: parseFloat($('#resumenSubtotal').text().replace('$', '').replace('.', '')),
            descuento: parseFloat($('#resumenDescuento').text().replace('$', '').replace('.', '')),
            total: parseFloat($('#resumenTotal').text().replace('$', '').replace('.', '')),

            detail: saleDetail
        };

        console.log(sale);

        $.ajax({
            url: '/Ventas/Facturacion/Crear',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(sale),
            success: function (response) {
                console.log(response);
                mostrarAlerta(
                    'success',
                    'La factura se guardó correctamente'
                );

                setTimeout(() => {
                    window.location.href =
                        `/Ventas/Facturacion/PreviewPdf/${response}`;
                }, 1500);
            },
            error: function () {
                mostrarAlerta(
                    'error',
                    'Ocurrió un error al guardar la factura'
                );
            }
        });

    });
});

$('#clientSelect').select2({
    placeholder: "Buscar Cliente...",
    width: '100%',
    allowClear: false
});

document.getElementById("btnFacturasRecientes").addEventListener("click", async function (e) {

    e.preventDefault();

    try {
        const response = await fetch('/Ventas/Facturacion/ObtenerFacturasRecientes');

        if (!response.ok)
            throw new Error("Error al obtener facturas");

        const ventas = await response.json();
        const tbody = document.getElementById("tbodyFacturas");

        tbody.innerHTML = "";
        ventas.forEach(v => {

            let detalleHtml = '';

            v.detalle.forEach(item => {

                detalleHtml += `
                        <tr>
                            <td>${item.articulo}</td>
                            <td>${item.cantidad}</td>
                            <td>$ ${item.precioUnitario}</td>
                            <td>$ ${item.total}</td>
                        </tr>
                    `;
            });

            tbody.innerHTML += `

                    <!-- FILA PRINCIPAL -->
                    <tr>
                        <td>${v.id}</td>
                        <td>${v.cliente}</td>
                        <td>${v.dni}</td>
                        <td>${v.fecha}</td>
                        <td>$ ${v.total}</td>

                        <td>
                            <button
                                class="btn btn-sm btn-primary"
                                data-bs-toggle="collapse"
                                data-bs-target="#detalle-${v.id}">
                                <i class="bi bi-eye"></i>
                                Ver Detalle
                            </button>
                        </td>

                        <td>
                            <a href="/Ventas/Facturacion/PreviewPdf/${v.id}"
                               class="btn btn-sm btn-primary"
                               target="_blank">
                                <i class="bi bi-file-pdf"></i>
                                Ver PDF
                            </a>
                        </td>
                    </tr>

                    <!-- FILA DETALLE -->
                    <tr class="collapse" id="detalle-${v.id}">
                        <td colspan="7">

                            <div class="p-3 bg-light rounded">

                                <table class="table table-sm align-middle">

                                    <thead class="table-info">
                                        <tr>
                                            <th>Artículo</th>
                                            <th>Cantidad</th>
                                            <th>Precio Unitario</th>
                                            <th>Total</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        ${detalleHtml}
                                    </tbody>

                                </table>

                            </div>

                        </td>
                    </tr>
                `;
        });

        const modal = new bootstrap.Modal(document.getElementById('modalFacturas'));
        modal.show();

    }
    catch (error) {
        console.error(error);
        alert("Ocurrió un error");
    }
});

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

function mostrarAlerta(tipo, mensaje) {
    let titulo = '';
    let icono = '';

    if (tipo === 'success') {
        titulo = 'Éxito';
        icono = '<i class="bi bi-check-circle-fill text-success"></i>';
    }
    else {
        titulo = 'Error';
        icono = '<i class="bi bi-x-circle-fill text-danger"></i>';
    }

    $('#mensajeTitulo').text(titulo);
    $('#mensajeTexto').text(mensaje);
    $('#mensajeIcono').html(icono);

    const modal = new bootstrap.Modal(
        document.getElementById('mensajeModal')
    );

    modal.show();
}

$('#clientSelect').on('change', function () {

    const clienteId = parseInt($(this).val());
    const option = this.options[this.selectedIndex];

    $('#nombreCliente').val(option.dataset.nombre || '');
    $('#dniCliente').val(option.dataset.dni || '');
    $('#resumenCliente').text(option.text || 'Sin seleccionar');

    //const clientes = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(Model.Clientes));
    const cliente = clientes.find(c => c.Id == clienteId);

    if (!cliente) {
        $('#nombreCliente').val('');
        $('#dniCliente').val('');
        $('#listaPrecioCliente').val('');
        return;
    }

    $('#nombreCliente').val(cliente.Nombre);
    $('#dniCliente').val(cliente.DNI);
    $('#listaPrecioClienteId').val(cliente.PriceListId);

});

$('#listaPrecioSelect').on('change', function () {

    const texto = $(this).find(':selected').text();

    $('#resumenLista').text(texto || 'Sin seleccionar');
});