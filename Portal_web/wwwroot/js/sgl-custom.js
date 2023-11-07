//Request to create or update a record.
function GetMainPathDirectory() {

    var pathName = window.location.pathname.slice(1)

    res = pathName.split("/");
    if (res.length > 2)
        return window.location.origin + "/" + res[0];
    else
        return "";
}


function validarNit(nit) {
    var nd, add = 0;
    if (nd = /^(\d+)\-?([\dk])$/i.exec(nit)) {
        nd[2] = (nd[2].toLowerCase() == 'k') ? 10 : parseInt(nd[2]);
        for (var i = 0; i < nd[1].length; i++) {
            add += ((((i - nd[1].length) * -1) + 1) * nd[1][i]);
        }
        return ((11 - (add % 11)) % 11) == nd[2];
    } else {
        return false;
    }
}

function toastNotificacion(objeto, callback) {

    // si la funcion no tiene callback, solo se muestra la alerta
    if (typeof (callback) === "undefined") {
        swal({
            title: objeto.title,
            text: objeto.msg,
            type: objeto.event
        });

    } else {

        if (!objeto.hasOwnProperty('cancel'))
            objeto.cancel = false;

        swal({
            title: objeto.title,
            text: objeto.msg,
            type: objeto.event,
            showCancelButton: objeto.cancel,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, continuar",
            closeOnConfirm: true
        }, function (isConfirm) {
            if (isConfirm) {
                callback();
            }
        });
    }



    //var msg = objeto.msg;
    //var event = objeto.event;
    //var title = objeto.title;
    //toastr.options = {
    //    closeButton: true,
    //    progressBar: true,
    //    preventDuplicates: true,
    //    positionClass: 'toast-top-right',
    //    onclick: null
    //};
    //if (event === "success") {
    //    toastr.success(msg, title);
    //}
    //if (event === "error") {
    //    toastr.error(msg, title);
    //}
    //if (event === "info") {
    //    toastr.info(msg, title);
    //}
    //if (event === "warning") {
    //    toastr.info(msg, title);
    //}
}

function send_submit(arreglo) {
    var form = document.createElement("form");
    form.setAttribute("method", arreglo.method);
    form.setAttribute("action", arreglo.url);
    for (var key in arreglo.params) {
        if (arreglo.params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", arreglo.params[key]);
            form.appendChild(hiddenField);
        }
    }
    document.body.appendChild(form);
    form.submit();
}

function reload_popupDiv(arreglo) {

    var row = document.createElement("div")
    row.classList.add("row");

    var col = document.createElement("div")
    col.classList.add("col-md-12");
    col.classList.add("center")



    var btn = document.createElement("button");
    btn.classList.add('btn')
    btn.classList.add('btn-primary');
    btn.innerHTML = "Recargar"

    var br = document.createElement("br");
    var br2 = document.createElement("br");

    col.appendChild(btn);

    col.appendChild(br);
    col.appendChild(br2);

    row.append(col);

    btn.addEventListener("click", () => {
        reloadRequest(arreglo)
    })

    $("#" + arreglo.div).append(row);
}

function reloadRequestDiv(arreglo) {
    ajax_on_div(arreglo);
}

function ajax_on_div(arreglo) {
    if (arreglo.hasOwnProperty("ibox"))
        OpenLoading(arreglo.ibox);

    $.ajax({
        type: arreglo.method,
        data: arreglo.params,
        url: arreglo.url,
        async: true,
        success: function (data) {
            var res = JSON.parse(JSON.stringify(data));

            if (res.hasOwnProperty('timeOut')) {
                reload_popupDiv(arreglo);
            }

            if (arreglo.hasOwnProperty("ibox"))
                CloseLoading(arreglo.ibox);


            $("#" + arreglo.div).html(data);
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                alert("page not found");
            },
            500: function () {
                alert("Error en la pagina");
            }
        }
    });
}

function ajax_on_div_status(arreglo) {
    startLoading();
    $.ajax({
        type: "POST",
        data: arreglo.params,
        url: arreglo.url,
        async: true,
        success: function (data) {
            $("#loading").hide();
            $("#" + arreglo.div).html(data);
        },
        complete: function () {
            if (localStorage.getItem("misObservaciones") != "") {
                $("#inputObservacionesGenerales").val(localStorage.getItem("misObservaciones"));
                localStorage.removeItem("misObservaciones");
                localStorage.clear();
            }
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                alert("page not found");
            },
            500: function () {
                alert("Error en la pagina");
            }
        }
    });
}

function ajax_on_popup(arreglo) {
    $("#modalIni").modal("show");
    $("#modalIni-title").html(arreglo.title);

    $("#modalIni-body").html("");
    $("#modalIni-body").append(`<div class="text-center"><h3> Cargando  Informacion.... </h3> </div>`);
    $("#modalIni-body").append(`<div class="progress">
                        <div class="progress-bar progress-bar-striped progress-bar-animated progress-bar-danger" style="width: 100%" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>`);
    startLoading();
    $.ajax({
        type: arreglo.method,
        data: arreglo.params,
        url: arreglo.url,
        async: true,
        success: function (data) {
            var res = JSON.parse(JSON.stringify(data));

            if (res.hasOwnProperty('timeOut')) {
                window.location.href = res.url;
            }

            $("#modalIni-body").html(data);
            stopLoading();
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                alert("page not found");
                stopLoading();
            },
            500: function () {
                alert("Error en la pagina");
                stopLoading();
            },
            203: function () {
                alert("page not found");
                stopLoading();
            },
        }
    });
}

function ajax_on_minipopup(arreglo) {
    $("#modal-mini-form").modal("show");
    $("#modal-mini-title").html(arreglo.title);

    $("#modal-mini-body").html("");
    $("#modal-mini-body").append(`<div class="text-center"><h3> Cargando  Informacion.... </h3> </div>`);
    $("#modal-mini-body").append(`<div class="progress">
                        <div class="progress-bar progress-bar-striped progress-bar-animated progress-bar-danger" style="width: 100%" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100"></div>
                    </div>`);

    startLoading();
    $.ajax({
        type: arreglo.method,
        data: arreglo.params,
        url: arreglo.url,
        async: true,
        success: function (data) {
            var res = JSON.parse(JSON.stringify(data));
            if (res.hasOwnProperty('timeOut'))
                window.location.href = res.url;
            $("#modal-mini-body").html(data);
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                alert("page not found");
            },
            500: function () {
                alert("Error en la pagina");
            }
        }
    });
}

function ajax_on_select(arreglo, callback) {
    var $dropdown = $("#" + arreglo.select);

    if (arreglo.hasOwnProperty("ibox"))
        OpenLoading(arreglo.ibox);

    $.ajax({
        type: arreglo.method,
        data: arreglo.params,
        url: arreglo.url,
        async: true,
        success: function (data) {
            try {

                if (arreglo.hasOwnProperty("ibox"))
                    CloseLoading(arreglo.ibox);

                arrayJ = JSON.parse(JSON.stringify(data));

                if (arrayJ.hasOwnProperty('timeOut'))
                    window.location.href = arrayJ.url;

                $dropdown.find('option').remove().end();
                $dropdown.append("<option value=''>No Definido</option>");

                var arrayT = (arrayJ.hasOwnProperty('data')) ? arrayJ.data : arrayJ;
                $.each(arrayT, function (index, value) {
                    $dropdown.append($("<option />").val(value.id).text(value.text));
                });

                if (typeof callback !== 'undefined')
                    callback(JSON.stringify(data));


            } catch (ex) {
                console.log(ex);
                $dropdown.find('option').remove().end();
                $dropdown.append("<option value=''>No Definido</option>");
            }
        },
        statusCode: {
            404: function () {
                alert("page not found");
            },
            500: function () {
                alert("Error en la pagina");
            }
        }
    });
}

function form_to_json(objeto) {
    var myform = $("#" + objeto);
    var disabled = myform.find(':input:disabled').removeAttr('disabled');
    var formdata = $("#" + objeto).serializeArray();
    var data = {};
    $(formdata).each(function (index, obj) {
        data[obj.name] = obj.value;
    });
    disabled.attr('disabled', 'disabled');
    return data;
}

function ajax_send(arreglo, callback) {
    var response = {};
    if (arreglo.hasOwnProperty("ibox"))
        OpenLoading(arreglo.ibox);


    if (arreglo.hasOwnProperty("button"))
        $("#" + arreglo.button).prop("disabled", "disabled");

    rp = {};
    $.ajax({
        type: arreglo.method,
        data: arreglo.params,
        url: arreglo.url,
        async: true,
        success: function (data) {
            try {
                var res = JSON.parse(JSON.stringify(data));

                if (res.hasOwnProperty('timeOut'))
                    window.location.href = res.url;

                if (arreglo.hasOwnProperty("ibox"))
                    CloseLoading(arreglo.ibox);

                if (arreglo.hasOwnProperty("button"))
                    $("#" + arreglo.button).removeAttr("disabled");

                if (typeof callback !== 'undefined')
                    callback(JSON.stringify(data));

            } catch (ex) {
                rp.msg = "Ha ocurrido un error, comuniquese con el departamento de sistemas";
                rp.title = "Aviso";
                rp.event = "error";
                toastNotificacion(rp);
            }
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                response.Code = 404;
                response.Msg = "Fail";
                callback(response);
            },
            500: function () {
                response.Code = 500;
                response.Msg = "Server Error";
                callback(response);
            }
        }
    });
}

function tabladinamica(objeto) {
    $('#' + objeto).dataTable({
        "language": {
            "decimal": "",
            "emptyTable": "No Hay registros Disponibles",
            "info": "Mostrando _START_ to _END_ de _TOTAL_ registros",
            "infoEmpty": "Mostrando 0 to 0 de 0 Registros",
            "infoFiltered": "(Fildrado de _MAX_ total registros)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ ",
            "loadingRecords": "Cargando..",
            "processing": "Procesando",
            "search": "Buscar:",
            "zeroRecords": "No Hay registros encontrados",
            "paginate": {
                "first": "Inicio",
                "last": "Final",
                "next": "Siguiente",
                "previous": "Anterior"
            },
        },
        "aaSorting": [],
    });
}

function roundThis(mount) {
    let myRound = !isNaN(mount) ? Math.round((mount * 100) / 100) : 0.00;
    return myRound.toFixed(2);
}

function startLoading() {
    $("#myloading").css({
        "display": "block",
        "position": "absolute",
        "top": "0",
        "left": "0",
        "z-index": "999999999",
        "width": "100vw",
        "height": $("#wrapper").height(),
        "background-color": "rgba(192, 192, 192, 0.5)",
        "background-image": "url(\"public/assets/js/custom/loader_main.gif\")",
        "background-repeat": "no-repeat",
        "background-position": "center",
    })
    $("#myloading").show();
}

function stopLoading() {
    $("#myloading").css({});
    $("#myloading").hide();
}

function confirm(title, text, callback, cancellCallBack) {
    swal({
        title: title,
        text: text,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, continuar",
        closeOnConfirm: true
    }, function () {
        callback();
    }, function () {
        cancellCallBack();
    }
    );
}

function success_confirm(arreglo, callback) {
    swal({
        title: arreglo.title,
        text: arreglo.msg,
        type: arreglo.event,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, continuar",
        closeOnConfirm: true
    }, function (isConfirm) {
        if (isConfirm) {
            callback();
        }
    }
    );
}

function valid_form(form, callback) {
    var rp = { msg: "", event: "", title: "" }
    var form = $("#" + form);
    form.validate().settings.ignore = ":disabled,:hidden";
    if (form.valid()) {
        callback();
    } else {
        rp.msg = "Debe llenar todos los campos";
        rp.title = "Aviso";
        rp.event = "error";
        toastNotificacion(rp);
    }
}

function form_reset(form) {
    document.getElementById(form).reset();
}

var acc = document.getElementsByClassName("accordion");
for (var i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function () {
        this.classList.toggle("accordion--active");
        var panel = this.nextElementSibling;
        if (panel.style.maxHeight) {
            panel.style.maxHeight = null;
        } else {
            panel.style.maxHeight = panel.scrollHeight + "px";
        }
    });
}

function showAlert(alert) {
    $(`#${alert}`).fadeIn(1000);
    setTimeout(function () {
        $(`#${alert}`).fadeOut(1000);
    }, 5000);
}

function showAlertWhenFinishedOk(message) {
    swal("Excelente, buen trabajo!", message, "success");
}

function validateOnlyNumbers(string) {
    var out = '';
    var filtro = '1234567890';
    for (var i = 0; i < string.length; i++)
        if (filtro.indexOf(string.charAt(i)) != -1)
            out += string.charAt(i);
    return out;
};

$(".toggle-password").click(function () {
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});

function validateOnlyPrices(string) {
    var out = '';
    var filtro = '1234567890.';
    for (var i = 0; i < string.length; i++)
        if (filtro.indexOf(string.charAt(i)) != -1)
            out += string.charAt(i);
    return out;
};

function validateQuantityWithParameter(value, restriction) {
    var out = '';
    var filtro = '1234567890.';
    for (var i = 0; i < value.length; i++)
        if (filtro.indexOf(value.charAt(i)) != -1)
            out += value.charAt(i);

    const restrictionValue = parseFloat(restriction) > parseFloat(value);
    if (restrictionValue) {
        return restriction;
    }
    return out;
};

function showAlertWhenOccurredError(message) {
    swal("Error", message, "error")
}

function showErrorAlertOfAnInactivationRequest(message) {
    swal("Error", `Ha ocurrido un error al deshabilitar ${message}`, "error")
}

function validateSummationOfChildren() {
    const children_boys = $("#input_children_boys").val();
    const children_girls = $("#input_children_girls").val();
    return parseInt((children_boys == '' ? '0' : children_boys)) + parseInt((children_girls == '' ? '0' : children_girls))
}


function ShowIngredients(_cod, count) {
    for (var i = 1; i <= count; i++) {
        var ingredientsBox = document.getElementById("dish_" + i);
        if (parseInt(_cod) === i) {
            ingredientsBox.style.display = 'block'
        }
        else {
            ingredientsBox.style.display = 'none'
        }
    }
}




function GetIngredientsFromNewDishModal() {
    var myData = document.getElementById("Ingredients").rows
    getIngredients = []
    for (var i = 0; i < myData.length; i++) {
        if (i > 1) {
            el = myData[i].children
            my_el = []
            for (var j = 0; j < el.length; j++) {
                switch (j) {
                    case 5:
                        {
                            break;
                        }
                    case 0:
                        {
                            my_el.push(el[j].id);
                            break;
                        }
                    case 1:
                        {
                            my_el.push(el[j].id);
                            break;
                        }
                    case 2:
                        {
                            my_el.push(el[j].id);
                            my_el.push(el[j].innerText);
                            break;
                        }
                    case 3:
                        {
                            my_el.push(el[j].innerText);
                            break;
                        }
                    default:
                        break;
                }
            }
            getIngredients.push(my_el)
        }
    }

    var ingredients = []
    for (var i = 0; i < getIngredients.length; i++) {
        var newIngredient = new Object();
        newIngredient.IngredientId = 0;
        newIngredient.DishId = 1;
        newIngredient.Measure = parseInt(getIngredients[i][1]);
        newIngredient.Price = parseInt(getIngredients[i][5]);
        newIngredient.ProductId = parseInt(getIngredients[i][2]);
        newIngredient.ProductName = getIngredients[i][3];
        newIngredient.Product_Type = parseInt(getIngredients[i][0]);
        newIngredient.Quantity = parseInt(getIngredients[i][4]);
        newIngredient.UserId = 0;
        ingredients.push(newIngredient)
    }
    return ingredients
}

function AddNewRowInIngredientTable() {
    var product = $('#products').val();

    var $dropdownProductos = $('#products');
    var $dropdownMeasure = $('#Measure');
    if (parseInt(product) > 0) {
        var Product_Type_Select = document.getElementById("Product_Type");
        var productType_Id = Product_Type_Select.options[Product_Type_Select.selectedIndex].value;
        var productType_Name = Product_Type_Select.options[Product_Type_Select.selectedIndex].text;

        var Measure_Select = document.getElementById("Measure");
        var Measure_Id = Measure_Select.options[Measure_Select.selectedIndex].value;
        var Measure_Name = Measure_Select.options[Measure_Select.selectedIndex].text;

        var Product_Select = document.getElementById("products");
        var Product_Name = Product_Select.options[Product_Select.selectedIndex].text;

        var quantity = $('#quantity').val();
        var table = document.getElementById("Ingredients");
        $(table).find('tbody').append(`<tr><td class="text-center" id="${productType_Id}">${productType_Name}</td><td class="text-center" id
        ="${Measure_Id}">${Measure_Name}</td><td class="text-center" id="${product}">${Product_Name}</td><td class="text-center">${quantity}</td>
        <button type="button" class="btn btn-outline btn-danger" onclick="DeleteRowFromTable(this)">
        <i class="fa fa-trash"></i></button></td></tr>`);

        $dropdownProductos.find('option').remove().end();
        $dropdownProductos.append("<option value=''>No Definido</option>");
        $('#products')[0].selectedIndex = 0;
        $('#Measure')[0].selectedIndex = 0;
    }
}


function ajax_send_file(arreglo, callback) {
    var response = {};

    if (arreglo.hasOwnProperty("ibox"))
        OpenLoading(arreglo.ibox);

    rp = {};
    var fd = new FormData();
    var files = $('#' + arreglo.image)[0].files[0];
    files = (typeof (files) === "undefined") ? "" : files;
    fd.append('Images', files);

    jQuery.each(arreglo.params, function (i, val) {
        fd.append(i, val);
    });

    $.ajax({
        url: arreglo.url,
        type: arreglo.method,
        data: fd,
        contentType: false,
        processData: false,
        async: true,
        success: function (data) {
            try {


                var res = JSON.parse(JSON.stringify(data));
                if (res.hasOwnProperty('timeOut'))
                    window.location.href = res.url;

                if (arreglo.hasOwnProperty("ibox"))
                    CloseLoading(arreglo.ibox);

                if (typeof callback !== 'undefined')
                    callback(JSON.stringify(data));


            } catch (ex) {
                console.log(ex);
                rp.msg = ex;
                rp.title = "Aviso";
                rp.event = "error";
                toastNotificacion(rp);
            }
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                response.Code = 404;
                response.Msg = "Fail";
                callback(response);
            },
            500: function () {
                response.Code = 500;
                response.Msg = "Server Error";
                callback(response);
            }
        }
    });
}

function ajax_send_file_document(arreglo, callback) {
    var response = {};

    if (arreglo.hasOwnProperty("ibox"))
        OpenLoading(arreglo.ibox);

    rp = {};
    var fd = new FormData();
    var files = document.getElementById(arreglo.document);
    var filename = files.files[0].name;
    var extension = filename.split('.').pop().toUpperCase();

    if (extension != "XLS" && extension != "XLSX") {
        fileErrorMessage.innerHTML = 'Porfavor seleccione archivos con extension .xls or .xlsx ';
        return false;
    }

    files = (typeof (files) === "undefined") ? "" : files;
    fd.append(filename, files.files[0]);

    jQuery.each(arreglo.params, function (i, val) {
        fd.append(i, val);
    });

    $.ajax({
        url: arreglo.url,
        type: arreglo.method,
        data: fd,
        contentType: false,
        cache: false,
        processData: false,
        async: true,
        success: function (data) {
            try {
                var res = JSON.parse(JSON.stringify(data));
                if (res.hasOwnProperty('timeOut'))
                    window.location.href = res.url;

                if (arreglo.hasOwnProperty("ibox"))
                    CloseLoading(arreglo.ibox);

                if (typeof callback !== 'undefined')
                    callback(JSON.stringify(data));
            } catch (ex) {
                console.log(ex);
                rp.msg = "Ha ocurrido un error, comuniquese con el departamento de sistemas";
                rp.title = "Aviso";
                rp.event = "error";
                toastNotificacion(rp);
            }
        },
        statusCode: {
            200: function () {
                stopLoading();
            },
            404: function () {
                response.Code = 404;
                response.Msg = "Fail";
                callback(response);
            },
            500: function () {
                response.Code = 500;
                response.Msg = "Server Error";
                callback(response);
            }
        }
    });
}

function DeleteRowFromTable(link) {
    var row = link.parentNode.parentNode;
    var table = row.parentNode;
    table.removeChild(row);
}

function OpenLoading(container) {
    $(`#${container}`).children('.ibox-content').toggleClass('sk-loading');
}

function LoadingModal(container) {
    $(`#${container}`).toggleClass('sk-loading');
}

function CloseLoading(container) {
    $(`#${container}`).children('.ibox-content').toggleClass('sk-loading');
}

function SelectAllItems(items) {
    const value = document.getElementById("select_all_details").checked;
    for (var i = 0; i < items; i++) {
        const itemHtml = document.getElementById(`checked_${i + 1}`);
        if (itemHtml != null) {
            if (!itemHtml.disabled) {
                itemHtml.checked = value;
            }
        }
    }
}

//"~/Menu/ShowMenu"
function ShowTheMenu(id, school, url) {
    ajax_on_popup({
        url: url,
        method: "GET",
        params: { school: school, menu: id }
    });
}

function ShowBankBalance(school, url) {
    ajax_on_minipopup({
        url: url,
        method: "GET",
        params: { id: school },
        title: "Saldo de Alimentación Escolar"
    });
}

function showPassword(id) {
    var object = $("#" + id);
    object.attr("type", (object.attr("type") === "password") ? "text" : "password");
}

function DownloadPDF(table) {
    const columns = $(`#${table} thead tr td`).length;
    var css = '@@page { size: landscape; }',
        head = document.head || document.getElementsByTagName('head')[0],
        style = document.createElement('style');

    style.type = 'text/css';
    style.media = 'print';
    if (style.styleSheet) {
        style.styleSheet.cssText = css;
    } else {
        style.appendChild(document.createTextNode(css));
    }

    document.getElementById("report_brand_image").style.display = "flex";
    head.appendChild(style);
    $(`#${table} tr > *:nth-child(${columns})`).hide();
    const tableHtml = document.getElementById("div_ibox_pdf").innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = tableHtml;
    window.print();
    document.body.innerHTML = originalContents;
    document.getElementById("report_brand_image").style.display = "none";
    $(`#${table} tr > *:nth-child(${columns})`).show();
}

var DownloadExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = `<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel"
            xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet>
            <x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets>
            </x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}
            </table></body></html>`
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name) {
        if (!table.nodeType) table = document.getElementById(table)
        var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
        window.location.href = uri + base64(format(template, ctx))
    }
})()

var getPersonalIP = function () {
    return new Promise((resolve, rejected) => {

        resolve("0.0.0.0");

        //$.getJSON("https://api.ipify.org?format=json", function (data) {
        //    resolve(data.ip);
        //}).fail(function () {
        //    rejected("127.0.0.1");
        //})
    })
}

function GetErrorMessage(response, errorDefault) {
    var message = '';

    console.log(response);

    switch (response.StatusCode) {
        case 403: {
            message = response.Message;
            break;
        }
        case 400: {
            message = response.Message;
            break;
        }
        default:
            message = errorDefault;
            break;
    }
    return message;
}

function Return(url) {
    send_submit({ url: url, method: "GET" })
}

function parseDateFromDatatable(str) {
    return moment.utc(str).utcOffset('-06:00').format('DD/MM/YYYY HH:mm A')
}

function parseDateFromDatatableSimpleFormat(str) {
    return moment.utc(str).utcOffset('-06:00').format('DD/MM/YYYY')
}



function startEvent(message, event, interval) {
    return new Promise((resolve, reject) => {

        var id = setInterval(event, interval + 2000);
        resolve({ id: id, interval: interval });
    })
}

function startMessage(message, event, interval) {
    return new Promise((resolve, reject) => {
        var messageId = setInterval(() => {
            toastNotificacion({ msg: message, title: "Aviso!", event: "info" });
        }, interval);
        resolve({ id: messageId, interval: interval });
    })
}


function setEvent(message, event, interval = 2000) {

    startMessage(message, event, interval).then((respMessage) => {

        setTimeout(() => {
            clearInterval(respMessage.id);
        }, (respMessage.interval + 1000));

        startEvent(message, event, interval + 2000).then((respEvent) => {
            setTimeout(() => {
                clearInterval(respEvent.id);
            }, (respEvent.interval + 3000));
        })

    })
}

function convertJsonToQueryString(json, prefix) {
    //convertJsonToQueryString({ Name: 1, Children: [{ Age: 1 }, { Age: 2, Hoobbie: "eat" }], Info: { Age: 1, Height: 80 } })
    if (!json) return null;
    var str = "";
    for (var key in json) {
        var val = json[key];
        if (isJson(val)) {
            str += convertJsonToQueryString(val, ((prefix || key) + "."));
        } else if (typeof (val) == "object" && ("length" in val)) {
            for (var i = 0; i < val.length; i++) {
                //debugger
                str += convertJsonToQueryString(val[i], ((prefix || key) + "[" + i + "]."));
            }
        }
        else {
            str += "&" + ((prefix || "") + key) + "=" + val;
        }
    }
    return str ? str.substring(1) : str;
}


isJson = function (obj) {
    return typeof (obj) == "object" && Object.prototype.toString.call(obj).toLowerCase() == "[object object]" && !obj.length;
};

function SpanishJson() {

    return {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_ registros",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        }
    }
}

function readURL(input, image_input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + image_input).attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]); // convert to base64 string
    }
}