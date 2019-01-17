function Agregar() { $("#modal-libros").load("/Prestamo/AgregarLibro"); };

$("#btnNuevoAutor").click(function (event) {
    $("#modal-content").load("/Autor/Create");
});

$("#btnNuevaCategoria").click(function (event) {
    $("#modal-content").load("/Categoria/Create");
});

$("#btnNuevoEditorial").click(function (event) {
    $("#modal-content").load("/Editorial/Create");
});

window.onload = function () {
    var txtTitulo = document.getElementById("titulo");
    txtTitulo.onkeyup = function (e) { enviarForm(); };
    function enviarForm() {
        var frm = document.getElementById("formenviar");
        frm.submit();
    }
}

function validar(e) {
    var key = window.Event ? e.which : e.keyCode;
    return ((key >= 48 && key <= 57) || (key == 8));
}

function validarText(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // 3
    patron = /[A-Za-z\s]+/; // 4
    te = String.fromCharCode(tecla); // 5
    return patron.test(te); // 6
}

function validarTextNum(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // 3
    patron = /[A-Za-z0-9\s]+/; // 4
    te = String.fromCharCode(tecla); // 5
    return patron.test(te); // 6
}

function validarTextNumG(e) { // 1
    tecla = (document.all) ? e.keyCode : e.which; // 2
    if (tecla == 8) return true; // 3
    patron = /[\-A-Za-z0-9\s]+/; // 4
    te = String.fromCharCode(tecla); // 5
    return patron.test(te); // 6
}