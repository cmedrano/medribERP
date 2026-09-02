// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Navegación AJAX del menú lateral.
 *
 * En vez de recargar toda la página cada vez que se elige una opción del
 * menú, el link se intercepta, se pide la página vía fetch (con el header
 * X-Requested-With para que el servidor devuelva sólo el fragmento de
 * contenido - ver Views/Shared/_AjaxLayout.cshtml y Helpers/LayoutHelper.cs)
 * y se inyecta dentro de `.content`, sin tocar el sidebar ni el <head>.
 */
(function () {
    "use strict";

    var contentEl = null;
    var currentUrl = window.location.href;

    /**
     * Los scripts que vengan en el fragmento cargado por AJAX deben usar
     * esto en vez de "DOMContentLoaded" para inicializar sus plugins
     * (select2, listeners, etc.), porque en una navegación AJAX el evento
     * DOMContentLoaded del documento ya disparó hace rato.
     *
     * Uso: onContentReady(function () { ... });
     */
    window.onContentReady = function (fn) {
        if (document.readyState === "loading") {
            document.addEventListener("DOMContentLoaded", fn, { once: true });
        } else {
            fn();
        }
    };

    function isAjaxNavLink(link) {
        if (!link || !link.getAttribute) return false;
        if (link.hasAttribute("data-bs-toggle")) return false; // submenús colapsables
        if (link.target && link.target !== "" && link.target !== "_self") return false;
        if (link.hasAttribute("download")) return false;
        var href = link.getAttribute("href");
        if (!href || href.charAt(0) === "#") return false;
        if (link.dataset.noAjax !== undefined) return false;
        // sólo mismo origen
        if (link.origin && link.origin !== window.location.origin) return false;
        return true;
    }

    function setActiveLink(link) {
        var sidebar = document.querySelector(".sidebar");
        if (!sidebar) return;
        sidebar.querySelectorAll(".nav-link.active").forEach(function (el) {
            el.classList.remove("active");
        });
        link.classList.add("active");

        // si el link está dentro de un submenú colapsable, marcar también
        // el toggle padre como activo y mantener el submenú abierto
        var collapseParent = link.closest(".collapse");
        if (collapseParent) {
            collapseParent.classList.add("show");
            var toggle = sidebar.querySelector('[href="#' + collapseParent.id + '"]');
            if (toggle) {
                toggle.classList.add("active");
                toggle.setAttribute("aria-expanded", "true");
            }
        }
    }

    function removeInjectedScripts() {
        document.querySelectorAll("script[data-ajax-injected]").forEach(function (s) {
            s.remove();
        });
    }

    function runScripts(scripts) {
        removeInjectedScripts();
        scripts.forEach(function (oldScript) {
            var newScript = document.createElement("script");
            Array.prototype.forEach.call(oldScript.attributes, function (attr) {
                newScript.setAttribute(attr.name, attr.value);
            });
            newScript.setAttribute("data-ajax-injected", "true");
            newScript.textContent = oldScript.textContent;
            document.body.appendChild(newScript);
        });
    }

    function showLoading(show) {
        document.body.classList.toggle("ajax-loading", !!show);
    }

    function loadPage(url, push, clickedLink) {
        if (!contentEl) return Promise.resolve();

        showLoading(true);

        return fetch(url, {
            headers: { "X-Requested-With": "XMLHttpRequest" },
            credentials: "same-origin"
        })
            .then(function (response) {
                // Si el server redirigió (ej: sesión vencida -> Login), navegamos
                // normal para que el usuario vea la página completa.
                if (response.redirected) {
                    window.location.href = response.url;
                    return null;
                }
                if (!response.ok) {
                    window.location.href = url;
                    return null;
                }
                var title = response.headers.get("X-Page-Title");
                return response.text().then(function (html) {
                    return { html: html, title: title };
                });
            })
            .then(function (result) {
                if (!result) return;

                var parser = new DOMParser();
                var doc = parser.parseFromString(result.html, "text/html");
                var scripts = Array.prototype.slice.call(doc.body.querySelectorAll("script"));
                scripts.forEach(function (s) { s.remove(); });

                contentEl.innerHTML = doc.body.innerHTML;

                if (result.title) {
                    document.title = result.title + " - PresupuestoMVC";
                }

                if (clickedLink) {
                    setActiveLink(clickedLink);
                }

                currentUrl = url;
                if (push) {
                    history.pushState({ ajaxNav: true }, "", url);
                }

                runScripts(scripts);

                contentEl.scrollTop = 0;
                window.scrollTo(0, 0);
                document.dispatchEvent(new CustomEvent("ajax:content-loaded", { detail: { url: url } }));
            })
            .catch(function () {
                // ante cualquier error, no dejamos la app rota: navegación normal
                window.location.href = url;
            })
            .finally(function () {
                showLoading(false);
            });
    }

    document.addEventListener("DOMContentLoaded", function () {
        contentEl = document.querySelector(".layout > .content");
        var sidebar = document.querySelector(".sidebar");
        if (!contentEl || !sidebar) return;

        history.replaceState({ ajaxNav: true }, "", window.location.href);

        sidebar.addEventListener("click", function (e) {
            var link = e.target.closest("a");
            if (!link || !sidebar.contains(link)) return;
            if (!isAjaxNavLink(link)) return;

            if (link.href === currentUrl) {
                e.preventDefault();
                return;
            }

            e.preventDefault();
            loadPage(link.href, true, link);
        });

        window.addEventListener("popstate", function () {
            var matchingLink = null;
            sidebar.querySelectorAll("a[href]").forEach(function (a) {
                if (a.href === window.location.href) matchingLink = a;
            });
            loadPage(window.location.href, false, matchingLink);
        });
    });
})();

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
