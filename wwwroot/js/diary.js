//function inicializarDiary() {

//    document.getElementById("crearFecha")
//        ?.addEventListener("change", () => {

//            actualizarRubrosPorFecha(
//                "crearFecha",
//                "crearRubroId"
//            );

//        });
//    const crearFecha = document.getElementById("crearFecha");
//    if (crearFecha) {
//        crearFecha.addEventListener("change", () => {
//            actualizarRubrosPorFecha("crearFecha", "crearRubroId");
//        });
//    }

//    const editFecha = document.getElementById("editFecha");
//    if (editFecha) {
//        editFecha.addEventListener("change", () => {
//            actualizarRubrosPorFecha("editFecha", "editRubroId");
//        });
//    }

//    const modalCrear = document.getElementById("modalCrearGastoId");
//    if (modalCrear) {
//        modalCrear.addEventListener("shown.bs.modal", function () {
//            const fechaInput = document.getElementById("crearFecha");
//            if (!fechaInput) return;

//            //fechaInput.focus();

//            //if (fechaInput.showPicker) {
//            //    fechaInput.showPicker();
//            //}

//            //setTimeout(() => {
//            //    fechaInput.showPicker();
//            //}, 100);
//        });
//    }
//}

function actualizarRubrosPorFecha(fechaInputId, rubroSelectId, rubroSeleccionadoId = null) {
    const date = document.getElementById(fechaInputId).value;
    if (!date) return;

    fetch(window.urls.getRubros + date)
        .then(res => res.json())
        .then(data => {
            const select = document.getElementById(rubroSelectId);
            select.innerHTML = '<option value="">Seleccionar rubro...</option>';

            data.forEach(r => {
                // Verificamos si este es el rubro que debe estar seleccionado
                const isSelected = (rubroSeleccionadoId && r.id == rubroSeleccionadoId) ? 'selected' : '';
                select.innerHTML += `<option value="${r.id}" ${isSelected}>${r.nombreRubro}</option>`;
            });
        })
        .catch(err => console.error("Error cargando rubros:", err));
}

//function actualizarRubrosPorFecha(fechaInputId, rubroSelectId, rubroSeleccionadoId = null) {
//    const fechaInput = document.getElementById(fechaInputId);
//    const select = document.getElementById(rubroSelectId);

//    if (!fechaInput || !select) return;

//    const date = fechaInput.value;
//    if (!date) return;

//    if (!window.urls || !window.urls.getRubros) {
//        console.error("No existe window.urls.getRubros");
//        return;
//    }

//    fetch(window.urls.getRubros + date)
//        .then(res => res.json())
//        .then(data => {
//            select.innerHTML = '<option value="">Seleccionar rubro...</option>';

//            data.forEach(r => {
//                const isSelected = rubroSeleccionadoId && r.id == rubroSeleccionadoId ? 'selected' : '';
//                select.innerHTML += `<option value="${r.id}" ${isSelected}>${r.nombreRubro}</option>`;
//            });
//        })
//        .catch(err => console.error("Error cargando rubros:", err));
//}

// --- MODAL CREACIÓN ---

function AbrirModalCrearGasto() {
    document.getElementById('crearFecha').value = new Date().toISOString().split('T')[0];
    document.getElementById('crearMonto').value = "";
    document.getElementById('crearNota').value = "";
    document.getElementById('crearRubroId').value = "";
    document.getElementById('crearPeriodoId').value = "";
    document.getElementById('crearCuentaId').value = "";

    // Al abrir, cargamos los rubros para la fecha por defecto (hoy)
    //actualizarRubrosPorFecha("crearFecha", "crearRubroId");

    const modal = new bootstrap.Modal(document.getElementById('modalCrearGastoId'));
    modal.show();
}

// Listener para cambios de fecha en creación
//document.getElementById("crearFecha").addEventListener("change", () => {
//    actualizarRubrosPorFecha("crearFecha", "crearRubroId");
//});

// --- MODAL EDICIÓN ---

function AbrirModalEditarGasto(id, fecha, monto, nota, rubroId, cuentaId, periodoId) {
    document.getElementById('editGastoId').value = id;
    document.getElementById('editFecha').value = fecha;
    document.getElementById('editMonto').value = monto;
    document.getElementById('editNota').value = nota;
    document.getElementById('editCuentaId').value = cuentaId;
    document.getElementById('editPeriodoId').value = periodoId;

    // Cargamos rubros específicos de esa fecha y seleccionamos el actual
    actualizarRubrosPorFecha("editFecha", "editRubroId", rubroId);

    const modal = new bootstrap.Modal(document.getElementById('modalEditarGastoId'));
    modal.show();
}

// Listener para cambios de fecha en edición
//document.getElementById("editFecha").addEventListener("change", () => {
//    actualizarRubrosPorFecha("editFecha", "editRubroId");
//});

// --- ELIMINACIÓN Y FILTROS ---

function AbrirModalEliminarGasto(id, fecha, monto, rubro, cuenta) {
    document.getElementById('deleteGastoId').value = id;
    document.getElementById('deleteGastoFecha').textContent = fecha;
    document.getElementById('deleteGastoMonto').textContent = '$' + parseFloat(monto).toFixed(2);
    document.getElementById('deleteGastoRubro').textContent = rubro;
    document.getElementById('deleteCuenta').textContent = cuenta;

    const modal = new bootstrap.Modal(document.getElementById('modalEliminarGastoId'));
    modal.show();
}

function limpiarFiltrosGasto() {
    window.location.href = '@Url.Action("Index", "Diary")';
}

function aplicarFiltrosGastos() {
    const rubroId = document.getElementById('rubroFilter').value;
    const cuentaId = document.getElementById('cuentaFilter').value;
    const fechaDesde = document.getElementById('fechaDesde')?.value;
    const fechaHasta = document.getElementById('fechaHasta')?.value;

    let url = '@Url.Action("Index", "Diary")';
    const params = new URLSearchParams();

    if (rubroId) params.append('rubroTypeId', rubroId);
    if (cuentaId) params.append('cuentaId', cuentaId);
    if (fechaDesde) params.append('fechaDesde', fechaDesde);
    if (fechaHasta) params.append('fechaHasta', fechaHasta);

    params.append('pagina', 1);
    params.append('tamañoPagina', diaryConfig.tamañoPagina);

    if (params.toString()) {
        url += '?' + params.toString();
    }

    window.location.href = url;
}

// --- LÓGICA DE PERSISTENCIA (AJAX CREAR) ---

function crearGasto() {
    const form = document.querySelector('#modalCrearGastoId form');

    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }

    const formData = new FormData(form);
    fetch(urls.createGasto, {
        method: 'POST',
        body: formData,
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(res => res.json())
        .then(data => {
            if (data.confirmationRequired) {
                mostrarModalSaldo(data.mensaje);
                return;
            }
            if (data.redirectUrl) {
                window.location.href = data.redirectUrl;
            }
        })
        .catch(err => {
            console.error(err);
            alert('Error al crear gasto');
        });
}

function mostrarModalSaldo(mensaje) {
    document.getElementById('mensajeSaldo').innerText = mensaje;
    const modal = new bootstrap.Modal(document.getElementById('modalSaldoInsuficiente'));
    modal.show();
}

function confirmarGastoConSaldoNegativo() {
    document.getElementById('forzarSaldoNegativo').value = "true";
    const modal = bootstrap.Modal.getInstance(document.getElementById('modalSaldoInsuficiente'));
    modal.hide();
    crearGasto();
}

// --- INICIALIZACIÓN ---

//document.addEventListener("DOMContentLoaded", () => {
//    // Carga inicial para el campo de creación si hay una fecha por defecto
//    actualizarRubrosPorFecha("crearFecha", "crearRubroId");
//});

//const modalCrear = document.getElementById('modalCrearGastoId');
//modalCrear.addEventListener('shown.bs.modal', function () {
//    const fechaInput = document.getElementById('crearFecha');
//    fechaInput.focus();
//    if (fechaInput.showPicker) {
//        fechaInput.showPicker();
//    }
//});