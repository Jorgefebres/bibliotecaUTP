
function validar(e) { var key = window.Event ? e.which : e.keyCode; return ((key >= 48 && key <= 57) || (key == 8)); }

$(document).ready(function () { 
    $('*[data-autocomplete-url]')
        .each(function () { 
            $(this).autocomplete({ 
                source: $(this).data("autocomplete-url") 
            }); 
        }); 
});
