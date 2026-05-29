const yearSel = document.getElementById('YearId');
const monthSel = document.getElementById('MonthId');
const errorMsg = document.getElementById('error-duplicado');
const btnSubmit = document.getElementById('btnGuardar');
const form = document.getElementById('formPeriodo');
const modalTitle = document.getElementById('modalTitleText');

async function validarPeriodo() {
    if (yearSel.value && monthSel.value) {
        const response = await fetch(`/Accounting/Period/CheckPeriodExists?yearId=${yearSel.value}&monthId=${monthSel.value}`);
        const data = await response.json();

        if (data.exists) {
            errorMsg.style.display = 'block';
            monthSel.classList.add('is-invalid');
            btnSubmit.disabled = true;
        } else {
            errorMsg.style.display = 'none';
            monthSel.classList.remove('is-invalid');
            btnSubmit.disabled = false;
        }
    }
}

//yearSel.addEventListener('change', validarPeriodo);
//monthSel.addEventListener('change', validarPeriodo);

function abrirModalCrearPeriodo() {
    //form.action = "@Url.Action("Create")";
    modalTitle.innerHTML = '<i class="bi bi-plus-circle me-2"></i>Nuevo Periodo';
    btnSubmit.innerHTML = '<i class="bi bi-check-circle me-1"></i>Crear Periodo';
    document.getElementById('EditPeriodoId').value = "0";

    yearSel.value = "";
    monthSel.value = "";
    document.getElementById('ValorPresupuestado').value = "";
    errorMsg.style.display = 'none';
    monthSel.classList.remove('is-invalid');
    btnSubmit.disabled = false;

    new bootstrap.Modal(document.getElementById('modalCrear')).show();
}

function abrirModalEditar(id, anio, mes, monto) {
    //form.action = "@Url.Action("UpdatePeriod")" + "?Id=" + id;
    modalTitle.innerHTML = '<i class="bi bi-pencil-square me-2"></i>Editar Periodo';
    btnSubmit.innerHTML = '<i class="bi bi-save me-1"></i>Guardar Cambios';
    document.getElementById('EditPeriodoId').value = id;

    // Mapeo de combos
    Array.from(yearSel.options).forEach(opt => { if (opt.text == anio) yearSel.value = opt.value; });
    const nombreMesBuscado = obtenerNombreMes(parseInt(mes));
    Array.from(monthSel.options).forEach(opt => { if (opt.text.toLowerCase().includes(nombreMesBuscado)) monthSel.value = opt.value; });

    document.getElementById('ValorPresupuestado').value = monto.replace(',', '.');
    errorMsg.style.display = 'none';
    monthSel.classList.remove('is-invalid');
    btnSubmit.disabled = false;

    new bootstrap.Modal(document.getElementById('modalCrear')).show();
}

function obtenerNombreMes(numero) {
    const meses = ["enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre"];
    return meses[numero - 1];
}

document.addEventListener("DOMContentLoaded", function () {
    //if (!ViewData.ModelState.IsValid) {
    //    <text>
    //        const modal = new bootstrap.Modal(document.getElementById('modalCrear'));
    //        modal.show();
    //    </text>
    //}
});