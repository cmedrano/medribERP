function abrirModalLista(
    crear,
    id,
    nombre,
    descripcion,
    soloVer = false) {
    isCreating = crear;
    const btnSubmit = document.getElementById('btnSubmitForm');

    if (crear) {
        // Modo crear
        document.getElementById('modalListaTitle').innerHTML = '<i class="bi bi-plus-circle me-2"></i>Crear Lista';
        document.getElementById('formLista').action = '@Url.Action("Create", "PriceList")';
        document.getElementById('btnSubmitForm').innerText = 'Registrar Lista';
        document.getElementById('formLista').reset();
        btnSubmit.style.display = 'block';

        // Habilitar todos los campos
        document.querySelectorAll('#formLista input, #formLista select').forEach(el => {
            el.disabled = false;
        });
    } else {
        // Modo ver/editar
        document.getElementById('ListaId').value = id;
        document.getElementById('listaNombre').value = nombre;
        document.getElementById('listaDescripcion').value = descripcion || '';

        if (soloVer) {
            document.getElementById('modalListaTitle').innerHTML = '<i class="bi bi-eye me-2"></i>Detalle';
            btnSubmit.style.display = 'none';
        } else {
            document.getElementById('modalListaTitle').innerHTML = '<i class="bi bi-pencil-square me-2"></i>Editar Lista';
            document.getElementById('formLista').action = '@Url.Action("EditList", "PriceList")';
            document.getElementById('btnSubmitForm').innerText = 'Actualizar';
            btnSubmit.style.display = 'block';
        }

        // Aplicar readonly/disabled según soloVer
        document.querySelectorAll('#formLista input, #formLista select').forEach(el => {
            el.disabled = soloVer;
        });
    }

    const modal = new bootstrap.Modal(document.getElementById('modalLista'));
    modal.show();
}
function abrirModalEliminarLista(id, nombre) {
    document.getElementById('listaEliminarNombre').innerText = nombre;
    document.getElementById('btnConfirmarEliminar').onclick = function () {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '@Url.Action("DeleteList", "PriceList")';

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = id;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    };
}