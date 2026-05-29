function abrirModalCrearCuenta() {
    document.getElementById('createAccountId');
    document.getElementById('InitialBalanceId');

    const modal = new bootstrap.Modal(document.getElementById('modalCrear'));
    modal.show();
}

function abrirModalEliminarCuenta(id, user, rol) {
    // Llenar formulario
    document.getElementById('deleteUserId').value = id;
    document.getElementById('deleteUsernameId').innerText = user;
    document.getElementById('deleteRol').innerText = rol;

    // Mostrar modal
    const modal = new bootstrap.Modal(document.getElementById('modalEliminar'));
    modal.show();
}

function abrirModalIngresar() {
    document.getElementById("accountId").value = "";
    document.getElementById("IngresarMonto").value = "";
    document.getElementById("crearNota").value = "";

    const modal = new bootstrap.Modal(document.getElementById('modalIngresar'));
    modal.show();
}
function abrirModalTransferir() {
    document.getElementById("accountOriginId").value = "";
    document.getElementById("accountDestinationId").value = "";
    document.getElementById("IncomeAmountTransfer").value = "";

    const modal = new bootstrap.Modal(document.getElementById('modalTransferir'));
    modal.show();
}