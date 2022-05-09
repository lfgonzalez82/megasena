// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function abrirDezenas(id) {
    if ($("#seta" + id).attr('class') === 'fas fa-angle-right') {
        $("#seta" + id).removeClass('fas fa-angle-right');
        $("#seta" + id).addClass('fas fa-angle-down');
        $("#"+id).show()
    }
    else{
        $("#seta" + id).removeClass('fas fa-angle-down');
        $("#seta" + id).addClass('fas fa-angle-right');
        $("#"+id).hide();
    }
 }

 function gerarExcel() {

    $.ajax({
        type: "POST",
        url: "/Resultados/GerarResultadoExcel",
        success: function (retorno) {




        },
        error: function (xhr, status, err) {
            // There has to be a better way to do this!!
            var title = xhr.responseText.split("<title>")[1].split("</title>")[0];
            alert(title);
        }
    })
}