
function abrirModalEditarPresupuesto(id, rubroId, valorInicial, mes, anio) {
    // Llenar formulario
    document.getElementById('editRubroId').value = id;
    document.getElementById('editRubroTypeId').value = rubroId;
    document.getElementById('editValorInicial').value = valorInicial;
    document.getElementById('editMes').value = mes;
    document.getElementById('editAnio').value = anio;

    // Mostrar modal
    const modal = new bootstrap.Modal(document.getElementById('modalEditar'));
    modal.show();
}

function abrirModalCrearPresupuesto() {
    // Resetear formulario
    document.getElementById('crearRubroId').value = '';
    document.getElementById('crearValorInicial').value = '';
    document.getElementById('crearMes').value = '';
    document.getElementById('crearAnio').value = '';

    // Mostrar modal
    const modal = new bootstrap.Modal(document.getElementById('modalCrear'));
    modal.show();
}

function abrirModalEliminarPresupuesto(id, rubroId, valorInicial, mes, anio) {
    // Llenar formulario
    document.getElementById('deleteRubroId').value = id;

    const meses = [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    ];9

    // Convertir número a nombre
    const mesNombre = meses[mes - 1] || `Mes ${mes}`;

    document.getElementById('deleteRubroIdText').innerText = id;
    document.getElementById('deleteRubroType').innerText = rubroId;
    document.getElementById('deleteValorInicial').innerText = valorInicial;
    document.getElementById('deleteMes').innerText = mesNombre;
    document.getElementById('deleteAnio').innerText = anio;

    // Mostrar modal
    const modal = new bootstrap.Modal(document.getElementById('modalEliminar'));
    modal.show();
}

function limpiarFiltros() {
    cargarVista('/Accounting/Budget');
}

function aplicarFiltrosPresupuesto() {
    const rubroId = document.getElementById('rubroFilter').value;
    const mes = document.getElementById('mesFilter').value;
    const anio = document.getElementById('aniosFilter').value;
    const deficit = document.getElementById('checkboxDeficit').checked;

    // Construir URL con parámetros
    let url = '/Accounting/Budget';
    const params = new URLSearchParams();

    if (rubroId) params.append('rubroTypeId', rubroId);
    if (mes) params.append('mes', mes);
    if (anio) params.append('anio', anio);
    params.append('deficit', deficit);


    // Mantener página actual
    params.append('pagina', 1);
    //params.append('tamañoPagina', @ViewBag.TamañoPagina);

    if (params.toString()) {
        url += '?' + params.toString();
    }

    cargarVista(url);
}

//const rubrosData = @Html.Raw(System.Text.Json.JsonSerializer.Serialize(ViewBag.Rubros));

document.getElementById("crearRubroId").addEventListener("change", function () {

    const rubroId = parseInt(this.value);
    const subRubroSelect = document.getElementById("crearSubRubroId");

    // limpiar opciones
    subRubroSelect.innerHTML = '<option value="">Seleccionar sub rubro...</option>';

    if (!rubroId) return;

    const rubro = rubrosData.find(r => r.Id === rubroId);

    if (rubro && rubro.SubCategories) {
        rubro.SubCategories.forEach(sub => {
            const option = document.createElement("option");
            option.value = sub.Id;
            option.text = sub.nombreRubro;
            subRubroSelect.appendChild(option);
        });
    }
});

document.getElementById("crearRubroId").addEventListener("change", function () {

    const rubroId = parseInt(this.value);
    const subSelect = document.getElementById("crearSubRubroId");
    const container = document.getElementById("subRubroContainer");

    // reset
    subSelect.innerHTML = '<option value="">Seleccionar sub rubro...</option>';
    container.classList.add("d-none");

    if (!rubroId) return;

    const rubro = rubrosData.find(r => r.Id === rubroId);

    if (rubro && rubro.SubCategories && rubro.SubCategories.length > 0) {

        rubro.SubCategories.forEach(sub => {
            const option = document.createElement("option");
            option.value = sub.Id;
            option.text = sub.nombreRubro;
            subSelect.appendChild(option);
        });

        // mostrar solo si tiene subrubros
        container.classList.remove("d-none");
    }
});