let itemsSubModal = [];
const formArticuloId = document.getElementById('formArticulo');

async function abrirModalArticulo(
    crear,
    id,
    codigo,
    nombre,
    unidadMedida,
    productCategoryId,
    brendId,
    providerId,
    purchasePrice,
    salePrice,
    soloVer = false) {
    isCreating = crear;
    const btnSubmit = document.getElementById('btnSubmitForm');

    if (crear) {
        // Modo crear
        document.getElementById('modalArticuloTitle').innerHTML = `<span class="erp-modal-title-icon"><i class="bi bi-box-seam"></i></span>
                Crear nuevo articulo`;
        document.getElementById('formArticulo').action = '@Url.Action("GuardarArticulo", "Articulos")';
        document.getElementById('btnSubmitForm').innerHTML = '<i class="bi bi-check2-circle me-2"></i>Registrar articulo';
        document.getElementById('formArticulo').reset();
        btnSubmit.style.display = 'block';
        btnLista.style.display = 'block';
        document.querySelectorAll("#subModal .price-input").forEach(input => {
            input.value = "";
        });

        // Habilitar todos los campos
        document.querySelectorAll('#formArticulo input, #formArticulo select, #subModal input').forEach(el => {
            el.disabled = false;
        });
    } else {
        // Modo ver/editar
        document.getElementById('articuloId').value = id;
        document.getElementById('articuloNombre').value = nombre;
        document.getElementById('articuloCodigo').value = codigo || '';
        document.getElementById('articuloUnidadMedida').value = unidadMedida || '';
        document.getElementById('SelectRubroId').value = productCategoryId || '';
        document.getElementById('BrendId').value = brendId || '';
        document.getElementById('providerId').value = providerId || '';
        document.getElementById('PurchasePrice').value = purchasePrice || '';
        document.getElementById('salePrice').value = salePrice || '';

        let url = '@Url.Action("GetPreciosByArticulo", "Articulos", new { area = "Ventas" })';

        let response = await fetch(`${url}?articuloId=${id}`);

        let precios = await response.json();

        cargarPreciosEnSubModal(precios);

        if (soloVer) {
            document.getElementById('modalArticuloTitle').innerHTML = '<i class="bi bi-eye me-2"></i>Detalles del Articulo';
            btnSubmit.style.display = 'none';
            btnLista.style.display = 'none';
        } else {
            document.getElementById('modalArticuloTitle').innerHTML = '<i class="bi bi-pencil-square me-2 erp-modal-title-icon"></i>Editar Articulo';
            document.getElementById('formArticulo').action = '@Url.Action("EditarArticulo", "Articulos")';
            document.getElementById('btnSubmitForm').innerHTML = '<i class="bi bi-check2-circle me-2"></i>Actualizar articulo';
            btnSubmit.style.display = 'block';
            btnLista.style.display = 'block';
        }

        // Aplicar readonly/disabled según soloVer
        document.querySelectorAll('#formArticulo input, #formArticulo select, #subModal input').forEach(el => {
            el.disabled = soloVer;
        });
    }

    const modal = new bootstrap.Modal(document.getElementById('modalArticulo'));
    modal.show();
}

function cargarPreciosEnSubModal(precios) {

    let rows = document.querySelectorAll("#subModal tbody tr");

    rows.forEach(row => {

        let listIdInput = row.querySelector('input[name*="ListId"]');
        let priceInput = row.querySelector('input[name*="Price"]');

        let precioEncontrado = precios.find(p => p.listId == listIdInput.value);

        if (precioEncontrado) {
            priceInput.value = precioEncontrado.price;
        } else {
            priceInput.value = '';
        }
    });
}

function abrirModalEliminarArticulos(id, nombre) {
    document.getElementById('articuloEliminarNombre').innerText = nombre;
    document.getElementById('btnConfirmarEliminar').onclick = function () {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '@Url.Action("EliminarArticulo", "Articulos")';

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = id;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    };
}


function abrirModalLista() {
    $('#subModal').modal('show');
}

function savedSubModalArticulos() {

    let rows = document.querySelectorAll("#subModal tbody tr");
    let container = document.getElementById("priceContainer");

    container.innerHTML = "";

    rows.forEach((row, i) => {

        let listId = row.querySelector("input[type='hidden']").value;
        let price = row.querySelector("input[type='number']").value;

        if (price) {

            container.innerHTML += `
                    <input type="hidden" name="Items[${i}].ListId" value="${listId}">
                    <input type="hidden" name="Items[${i}].Price" value="${price}">
                `;
        }

    });

    $('#subModal').modal('hide');
}
document.addEventListener("DOMContentLoaded", function () {
    if (formArticuloId) {
        document.getElementById("formArticulo").addEventListener("submit", function () {

            let container = document.getElementById("hiddenItemsContainer");
            container.innerHTML = "";

            let rows = document.querySelectorAll("#subModal tbody tr");

            rows.forEach((row, index) => {
                let listId = row.querySelector('input[name*="ListId"]').value;
                let price = row.querySelector('input[name*="Price"]').value;

                container.innerHTML += `
                    <input type="hidden" name="Items[${index}].ListId" value="${listId}" />
                    <input type="hidden" name="Items[${index}].Price" value="${price}" />
                `;
            });
        });
    }
});