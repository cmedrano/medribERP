let debounceTimer;
let isCreating = true;
const DEBOUNCE_DELAY = 300;
const MIN_SEARCH_LENGTH = 3;
function inicializarCliente() {
    document.getElementById('modalClienteTitle');
}
// Inicializar event listeners en los inputs
document.addEventListener('DOMContentLoaded', function () {
    const inputNombre = document.getElementById('inputNombre');
    const inputNombreFantasia = document.getElementById('inputNombreFantasia');
    const formCliente = document.getElementById('formCliente');
    const emailInput = document.getElementById('clienteEmail');
    const codigoPostalInput = document.getElementById('clienteCodigoPostal');


    if (inputNombre && inputNombreFantasia && formCliente && emailInput && codigoPostalInput) {
        inputNombre.addEventListener('input', realizarBusqueda);

        inputNombreFantasia.addEventListener('input', realizarBusqueda);

        // Agregar validación al enviar el formulario
        formCliente.addEventListener('submit', validarEmailAntesDeSomete);

        // Limpiar errores al escribir en el email
        emailInput.addEventListener('input', limpiarErrorEmail);

        // Agregar evento onBlur para cargar localidades y provincia
        codigoPostalInput.addEventListener('blur', cargarLocalidadesYProvincia);
    }
});

// Función para limpiar errores del email
function limpiarErrorEmail() {
    const emailInput = document.getElementById('clienteEmail');
    const errorContainer = document.getElementById('emailErrorContainer');
    const errorMessage = document.getElementById('emailErrorMessage');

    emailInput.classList.remove('is-invalid');
    errorMessage.textContent = ''; // Vaciamos el texto
    errorContainer.classList.add('d-none'); // Escondemos el contenedor
    errorContainer.classList.remove('d-block');
}

// Función para validar email antes de enviar el formulario
async function validarEmailAntesDeSomete(event) {
    const emailInput = document.getElementById('clienteEmail');
    const email = emailInput.value.trim();
    const clienteId = document.getElementById('clienteId').value;

    // Si no hay email, dejar que se envíe (es opcional)
    if (!email) {
        return true;
    }

    // Detener el envío del formulario mientras se valida
    event.preventDefault();

    try {
        const params = new URLSearchParams();
        params.append('email', email);
        if (clienteId) {
            params.append('clienteId', clienteId);
        }

        const response = await fetch(`@Url.Action("ValidarEmail", "Clientes")?${params.toString()}`, {
            method: 'GET',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        const data = await response.json();

        if (!data.valido) {
            // Mostrar error
            mostrarErrorEmail(data.mensaje);
            return false;
        } else {
            // Email válido, enviar formulario
            limpiarErrorEmail();
            event.target.submit();
        }
    } catch (error) {
        console.error('Error validando email:', error);
        mostrarErrorEmail('Error al validar el email. Por favor, intente nuevamente.');
    }
}

// Función para mostrar error de email
function mostrarErrorEmail(mensaje) {
    const emailInput = document.getElementById('clienteEmail');
    const errorContainer = document.getElementById('emailErrorContainer');
    const errorMessage = document.getElementById('emailErrorMessage');

    emailInput.classList.add('is-invalid');
    errorMessage.textContent = mensaje;
    errorContainer.classList.remove('d-none'); // Mostramos
    errorContainer.classList.add('d-block');
}
// Función de debounce
function realizarBusqueda() {
    clearTimeout(debounceTimer);

    const searchNombre = document.getElementById('inputNombre').value.trim();
    const searchFantasia = document.getElementById('inputNombreFantasia').value.trim();

    // Verificar si al menos uno cumple con el mínimo de caracteres
    const nombreValido = searchNombre.length >= MIN_SEARCH_LENGTH || searchNombre.length === 0;
    const fantasiaValida = searchFantasia.length >= MIN_SEARCH_LENGTH || searchFantasia.length === 0;

    if (!nombreValido || !fantasiaValida) {
        return;
    }

    debounceTimer = setTimeout(() => {
        buscarClientes(1, searchNombre, searchFantasia);
    }, DEBOUNCE_DELAY);
}

// Función para buscar clientes (AJAX)
function buscarClientes(page = 1, searchNombre = '', searchFantasia = '') {
    const loading = document.getElementById('loadingIndicator');
    loading.classList.remove('d-none');

    const params = new URLSearchParams();
    if (searchNombre) params.append('searchNombre', searchNombre);
    if (searchFantasia) params.append('searchFantasia', searchFantasia);
    params.append('page', page);

    fetch(`@Url.Action("Index", "Clientes")?${params.toString()}`, {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(response => {
            if (!response.ok) throw new Error('Error en la respuesta del servidor');
            return response.text();
        })
        .then(html => {
            document.getElementById('tablaContainer').innerHTML = html;
            loading.classList.add('d-none');
        })
        .catch(error => {
            console.error('Error:', error);
            loading.classList.add('d-none');
            alert('Error al buscar clientes');
        });
}

// Función para cambiar página
function cambiarPagina(page) {
    const searchNombre = document.getElementById('inputNombre').value.trim();
    const searchFantasia = document.getElementById('inputNombreFantasia').value.trim();
    buscarClientes(page, searchNombre, searchFantasia);
}

// Función para limpiar filtros
function limpiarFiltros() {
    document.getElementById('inputNombre').value = '';
    document.getElementById('inputNombreFantasia').value = '';
    buscarClientes(1, '', '');
}

function abrirModalCliente(crear, id, nombre, telefono, domicilio, localidad, provincia, codigoPostal, email, celular, dni, cuit, fantasia, condicionDeVenta, categoria, operacionesContado, inhabitadoFacturar, soloVer = false) {
    isCreating = crear;
    const btnSubmit = document.getElementById('btnSubmitForm');

    if (crear) {
        // Modo crear
        document.getElementById('modalClienteTitle').innerHTML = `<span class="erp-modal-title-icon"><i class="bi bi-person-plus"></i></span>
            Crear nuevo cliente`;
        document.getElementById('formCliente').action = '@Url.Action("GuardarCliente", "Clientes")';
        document.getElementById('btnSubmitForm').innerHTML = '<i class="bi bi-check2-circle me-2"></i>Registrar cliente';
        document.getElementById('formCliente').reset();
        btnSubmit.style.display = 'block';

        // Habilitar todos los campos
        document.querySelectorAll('#formCliente input, #formCliente select').forEach(el => {
            el.disabled = false;
        });

        // Limpiar errores
        limpiarErrorEmail();
    } else {
        // Modo ver/editar
        document.getElementById('clienteId').value = id;
        document.getElementById('clienteNombre').value = nombre;
        document.getElementById('clienteFantasia').value = fantasia || '';
        document.getElementById('clienteTelefono').value = telefono || '';
        document.getElementById('clienteCelular').value = celular || '';
        document.getElementById('clienteEmail').value = email || '';
        document.getElementById('clienteDomicilio').value = domicilio || '';
        document.getElementById('clienteCodigoPostal').value = codigoPostal || '';
        document.getElementById('clienteProvincia').value = provincia || '';
        document.getElementById('clienteDNI').value = dni || '';
        document.getElementById('clienteCUIT').value = cuit || '';
        document.getElementById('clienteCategoria').value = categoria || '';
        document.getElementById('clienteCondicionDeVenta').value = condicionDeVenta || '';
        document.getElementById('clienteOperacionesContado').checked = String(operacionesContado) === 'true';
        document.getElementById('clienteInhabilitadoFacturar').checked = String(inhabitadoFacturar) === 'true';

        // Cargar localidades si hay código postal
        if (codigoPostal) {
            cargarLocalidadesYProvinciaAlEditar(codigoPostal, localidad);
        } else {
            document.getElementById('clienteLocalidad').innerHTML = '<option value="">Seleccionar localidad...</option>';
        }

        if (soloVer) {
            document.getElementById('modalClienteTitle').innerHTML = '<i class="bi bi-eye me-2"></i>Detalles del Cliente';
            btnSubmit.style.display = 'none';
        } else {
            document.getElementById('modalClienteTitle').innerHTML = '<i class="bi bi-pencil-square erp-modal-title-icon"></i>Editar Cliente';
            document.getElementById('formCliente').action = '@Url.Action("EditarCliente", "Clientes")';
            document.getElementById('btnSubmitForm').innerHTML = '<i class="bi bi-check2-circle me-2"></i>Actualizar cliente';
            btnSubmit.style.display = 'block';
        }

        // Aplicar readonly/disabled según soloVer
        document.querySelectorAll('#formCliente input, #formCliente select').forEach(el => {
            el.disabled = soloVer;
        });

        // Limpiar errores
        limpiarErrorEmail();
    }

    const modal = new bootstrap.Modal(document.getElementById('modalCliente'));
    modal.show();
}

// Función auxiliar para cargar localidades al editar
async function cargarLocalidadesYProvinciaAlEditar(codigoPostal, localidadActual) {
    const localidadSelect = document.getElementById('clienteLocalidad');

    if (!codigoPostal) {
        localidadSelect.innerHTML = '<option value="">Seleccionar localidad...</option>';
        return;
    }

    try {
        const response = await fetch(`@Url.Action("ObtenerLocalidadPorCodigoPostal", "Clientes")?codigoPostal=${encodeURIComponent(codigoPostal)}`, {
            method: 'GET',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        const data = await response.json();

        if (data.success) {
            // Llenar el select de localidades
            localidadSelect.innerHTML = '';
            data.localidades.forEach((localidad) => {
                const option = document.createElement('option');
                option.value = localidad;
                option.textContent = localidad;
                // Seleccionar la localidad actual si existe
                if (localidad === localidadActual) {
                    option.selected = true;
                }
                localidadSelect.appendChild(option);
            });

            // Si no se encontró la localidad actual, seleccionar la primera
            if (!localidadSelect.value && data.localidades.length > 0) {
                localidadSelect.value = data.localidades[0];
            }
        } else {
            localidadSelect.innerHTML = '<option value="">Seleccionar localidad...</option>';
        }
    } catch (error) {
        console.error('Error al obtener localidades:', error);
        localidadSelect.innerHTML = '<option value="">Seleccionar localidad...</option>';
    }
}

function abrirModalEliminar(id, nombre) {
    document.getElementById('clienteEliminarNombre').innerText = nombre;
    document.getElementById('btnConfirmarEliminar').onclick = function () {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '@Url.Action("EliminarCliente", "Clientes")';

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'id';
        input.value = id;

        form.appendChild(input);
        document.body.appendChild(form);
        form.submit();
    };
}

// Función para cargar localidades y provincia cuando sale del campo código postal
async function cargarLocalidadesYProvincia() {
    const codigoPostalInput = document.getElementById('clienteCodigoPostal');
    const localidadSelect = document.getElementById('clienteLocalidad');
    const provinciaInput = document.getElementById('clienteProvincia');
    const codigoPostal = codigoPostalInput.value.trim();

    // Limpiar si el código postal está vacío
    if (!codigoPostal) {
        localidadSelect.innerHTML = '<option value="">Seleccionar localidad...</option>';
        provinciaInput.value = '';
        return;
    }

    try {
        const response = await fetch(`@Url.Action("ObtenerLocalidadPorCodigoPostal", "Clientes")?codigoPostal=${encodeURIComponent(codigoPostal)}`, {
            method: 'GET',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        const data = await response.json();

        if (data.success) {
            // Llenar el select de localidades
            localidadSelect.innerHTML = '';
            data.localidades.forEach((localidad, index) => {
                const option = document.createElement('option');
                option.value = localidad;
                option.textContent = localidad;
                if (index === 0) {
                    option.selected = true; // Seleccionar la primera localidad por defecto
                }
                localidadSelect.appendChild(option);
            });

            // Llenar el campo de provincia
            provinciaInput.value = data.provincia;
            // provinciaInput.readOnly = true; // Hacerlo de solo lectura
        } else {
            // Mostrar error
            console.warn(data.mensaje);
            localidadSelect.innerHTML = '<option value="">Seleccionar localidad...</option>';
            provinciaInput.value = '';
            provinciaInput.readOnly = false;
        }
    } catch (error) {
        console.error('Error al obtener localidades:', error);
        localidadSelect.innerHTML = '<option value="">Seleccionar localidad...</option>';
        provinciaInput.value = '';
        provinciaInput.readOnly = false;
    }
}