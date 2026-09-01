// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Navegación por AJAX para links marcados con la clase "ajax-nav":
// en vez de recargar toda la página, se pide el contenido por fetch
// (el server detecta el header X-Requested-With y devuelve solo el
// partial correspondiente) y se lo inyecta dentro de #mainContent.
(function () {
    var mainContent = document.getElementById('mainContent');
    if (!mainContent) return;

    function loadAjaxNav(url, pushState) {
        mainContent.classList.add('ajax-loading');

        return fetch(url, {
            headers: { 'X-Requested-With': 'XMLHttpRequest' },
            credentials: 'same-origin'
        })
            .then(function (response) {
                if (!response.ok) throw new Error('Error al cargar el contenido (' + response.status + ')');
                return response.text();
            })
            .then(function (html) {
                mainContent.innerHTML = html;
                if (pushState) {
                    window.history.pushState({ ajaxNav: true, url: url }, '', url);
                }
            })
            .catch(function (err) {
                console.error(err);
                mainContent.innerHTML = '<div class="alert alert-danger m-3">No se pudo cargar el contenido. Intentá nuevamente.</div>';
            })
            .finally(function () {
                mainContent.classList.remove('ajax-loading');
            });
    }

    document.addEventListener('click', function (e) {
        var link = e.target.closest('.ajax-nav');
        if (!link) return;

        e.preventDefault();

        document.querySelectorAll('.ajax-nav').forEach(function (a) { a.classList.remove('active'); });
        link.classList.add('active');

        loadAjaxNav(link.getAttribute('href'), true);
    });

    window.addEventListener('popstate', function (e) {
        if (e.state && e.state.ajaxNav) {
            loadAjaxNav(e.state.url, false);
        }
    });
})();
