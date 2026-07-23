let idMarcaEliminar = 0;
let idMarcaEditar = 0;

/* Seccion Crear y Editar */
document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formMarca");

    form.addEventListener("submit", async function (e) {

        e.preventDefault();

        const codigo = document.getElementById("marcaCodigo").value.trim();
        const nombre = document.getElementById("marcaNombre").value.trim();

        if (codigo === "" || nombre === "") {
            alert("Complete todos los campos.");
            return;
        }

        const marca = {
            codigo: codigo,
            nombre: nombre
        };

        try {
            let url = "";
            let metodo = "";

            if (idMarcaEditar == 0) {
                url = "/Ventas/Marca/Crear";
                metodo = "POST";
            }
            else {
                url = "/Ventas/Marca/Editar";
                metodo = "PUT";
            }

            const marca = {

                id: idMarcaEditar,
                codigo: codigo,
                nombre: nombre

            };

            const response = await fetch(url, {
                method: metodo,
                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify(marca)
            });

            if (response.ok) {
                const modal = bootstrap.Modal.getInstance(
                    document.getElementById("modalMarca")
                );

                modal.hide();
                location.reload();

            }
            else {
                const error = await response.text();
                alert(error);
            }

        }
        catch (error) {
            console.error(error);
            alert("Ocurrió un error.");
        }

    });

});

function abrirModalMarca(modo, id = 0, code = "", name = "") {
    document.getElementById("formMarca").reset();

    const txtCodigo = document.getElementById("marcaCodigo");
    const txtNombre = document.getElementById("marcaNombre");

    const btnGuardar = document.getElementById("btnSubmitForm");

    txtCodigo.value = code;
    txtNombre.value = name;

    idMarcaEditar = id;

    switch (modo) {
        case "crear":
            document.getElementById("modalMarcaTitle").innerText = "Nueva Marca";

            btnGuardar.innerText = "Guardar";
            btnGuardar.style.display = "block";

            txtCodigo.disabled = false;
            txtNombre.disabled = false;

            idMarcaEditar = 0;

            break;

        case "editar":
            document.getElementById("modalMarcaTitle").innerText = "Editar Marca";

            btnGuardar.innerText = "Actualizar";
            btnGuardar.style.display = "block";

            txtCodigo.disabled = false;
            txtNombre.disabled = false;

            break;

        case "ver":
            document.getElementById("modalMarcaTitle").innerText = "Detalle de Marca";

            btnGuardar.style.display = "none";

            txtCodigo.disabled = true;
            txtNombre.disabled = true;

            break;
    }
    const modal = new bootstrap.Modal(document.getElementById("modalMarca"));
    modal.show();
}

/* Seccion Eliminar */
document.getElementById("btnEliminarMarca")
    .addEventListener("click", eliminarMarca);

async function eliminarMarca() {
    try {
        const response = await fetch(`/Ventas/Marca/Eliminar?id=${idMarcaEliminar}`, {
            method: "DELETE"
        });

        if (response.ok) {
            location.reload();
        }
        else {
            const error = await response.text();
            alert(error);
        }

    }
    catch (error) {
        console.error(error);
        alert("Ocurrió un error.");
    }

}

function abrirModalEliminar(id, nombre) {
    idMarcaEliminar = id;

    document.getElementById("nombreMarcaEliminar").innerText = nombre;

    const modal = new bootstrap.Modal(document.getElementById("modalEliminarMarca"));

    modal.show();
}




