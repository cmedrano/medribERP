document.addEventListener("DOMContentLoaded", () => {

    configurarLinksAjax();

});

function configurarLinksAjax() {

    document.querySelectorAll(".nav-link").forEach(link => {

        link.addEventListener("click", function (e) {

            const href = this.getAttribute("href");

            // evitar collapse bootstrap
            if (!href || href.startsWith("#"))
                return;

            e.preventDefault();

            navegarAjax(href);

        });

    });

}

function navegarAjax(url) {

    fetch(url, {
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
        .then(response => response.text())
        .then(html => {

            const parser = new DOMParser();

            const doc = parser.parseFromString(html, 'text/html');

            const nuevoContenido =
                doc.querySelector('#main-content');

            if (nuevoContenido) {

                document.querySelector('#main-content').innerHTML =
                    nuevoContenido.innerHTML;

            }
            else {

                document.querySelector('#main-content').innerHTML =
                    html;

            }

            history.pushState({}, '', url);

            reInicializarScripts();

        })
        .catch(error => console.error(error));

}

function reInicializarScripts() {

    // diary
    if (typeof inicializarDiary === 'function') {
        inicializarDiary();
    }

    // budget
    if (typeof inicializarBudget === 'function') {
        inicializarBudget();
    }

}