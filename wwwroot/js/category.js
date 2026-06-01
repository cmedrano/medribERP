function abrirModalCrearRubro(id, subCategory) {
    if (subCategory) {
        document.getElementById('categoryId').value = id;
        document.getElementById('createSubCategory').value = '';

        const modal = new bootstrap.Modal(document.getElementById('modalCrearSubCategory'));
        modal.show();
    } else {
        document.getElementById('createCategory').value = '';

        const modal = new bootstrap.Modal(document.getElementById('modalCrear'));
        modal.show();
    }
}

function abrirModalEliminarRubro(id, subCategory) {
    document.getElementById('deleteCategoryId').value = id;
    document.getElementById('deleteCategoryName').innerText = subCategory;
    document.getElementById('deleteCategory').innerText = id;

    const modal = new bootstrap.Modal(document.getElementById('modalEliminarRubroId'));
    modal.show();
}