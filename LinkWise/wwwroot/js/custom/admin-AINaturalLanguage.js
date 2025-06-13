document.addEventListener('DOMContentLoaded', function () {
    $("#divSubmeterPrompt").hide();

    $("#Prompts").on('change', function () {
        if ($("#Prompts").val() == 0) {
            clearPrompt();
            $("#divSubmeterPrompt").hide();
        }
        else {
            getPrompt($("#Prompts").val())
            $("#divSubmeterPrompt").show();
        }
    });
    $("#LoadingContainer").hide();

    $("#spanRespostaFormLabel").show();
    //$("#spanRespostaFormFields").hide();

});

function getPrompt(idPrompt) {

    $.ajax({
        url: '/Admin/GetPrompt',
        type: 'POST',
        data: {
            idp: idPrompt
        },
        success: function (data) {

            fillPrompt(data);
        },
        failure: function (data) {
            console.log("failure");
        },
        error: function (data) {
            console.log("error");
        }
    });

    return false;
};

function fillPrompt(prompt) {

    $("#IDPrompt").val(prompt.idPrompt);
    $("#NMTitulo").val(prompt.nmTitulo);
    $("#IDTipoPrompt").val(prompt.idTipoPrompt);
    $("#NMTipoPrompt").html(prompt.nmTipoPrompt);
    $("#DSObjetivo").val(prompt.dsObjetivo);
    $("#DSContexto").val(prompt.dsContexto);
    $("#EstiloResposta").val(prompt.idEstiloResposta);
    $("#TipoPost").val(0);
    $("#Pessoa").val(0);
    $("#Vies").val(prompt.idVies);
    $("#DSPrompt").val(prompt.dsPrompt);
    $("#NRMaxTokens").val(prompt.nrMaxTokens);
    $("#VRTemperature").val(prompt.vrTemperature.toString().replace(",", "").replace(".", ","));
}

function clearPrompt(prompt) {

    $("#IDPrompt").val(0);
    $("#NMTitulo").val("");
    $("#IDTipoPrompt").val(0);
    $("#NMTipoPrompt").html("");
    $("#DSObjetivo").val("");
    $("#DSContexto").val("");
    $("#EstiloResposta").val(0);
    $("#TipoPost").val(0);
    $("#Pessoa").val(0);
    $("#Vies").val(0);
    $("#DSPrompt").val("");
    $("#NRMaxTokens").val("");
    $("#VRTemperature").val("");
}

function savePrompt() {

    $.ajax({
        url: '/Admin/SavePrompt',
        type: 'POST',
        data: {
            idp: $("#IDPrompt").val(),
            idt: $("#IDTipoPrompt").val(),
            ide: $("#EstiloResposta").val(),
            idv: $("#Vies").val(),
            nmt: $("#NMTitulo").val(),
            dso: $("#DSObjetivo").val(),
            dsc: $("#DSContexto").val(),
            dsp: $("#DSPrompt").val(),
            nrm: $("#NRMaxTokens").val(),
            vrt: $("#VRTemperature").val(),
        },
        success: function (data) {
            console.log(data);
        },
        failure: function (data) {
            console.log("failure");
        },
        error: function (data) {
            console.log("error");
        }
    });

    return false;
};

function savePromptNew() {

    $.ajax({
        url: '/Admin/SavePrompt',
        type: 'POST',
        data: {
            idp: $("#IDPromptNew").val(),
            idt: $("#IDTipoPromptNew").val(),
            ide: $("#EstiloRespostaNew").val(),
            idv: $("#ViesNew").val(),
            nmt: $("#NMTituloNew").val(),
            dso: $("#DSObjetivoNew").val(),
            dsc: $("#DSContextoNew").val(),
            dsp: $("#DSPromptNew").val(),
            nrm: $("#NRMaxTokensNew").val(),
            vrt: $("#VRTemperatureNew").val(),
        },
        success: function (data) {
            console.log(data);
        },
        failure: function (data) {
            console.log("failure");
        },
        error: function (data) {
            console.log("error");
        }
    });

    return false;
};

function runContentGeneratorOpenAI() {

    $('html, body').animate({ scrollTop: 0 }, 'fast');

    $("#mainContainer").hide();
    $("#LoadingContainer").show();

    $.ajax({
        url: '/Admin/RunContentGeneratorOpenAI',
        type: 'POST',
        data: {
            idp: $("#IDPrompt").val(),
            idtp: $("#IDTipoPrompt").val(),
            dst: $("#DSTema").val(),
            dsu: $("#DSUrlSource").val(),
            nmer: $("#EstiloResposta").find('option:selected').text(),
            nma: $("#Pessoa").find('option:selected').text(),
            nmtp: $("#TipoPost").find('option:selected').text(),
            nmv: $("#Vies").find('option:selected').text(),
            nmt: $("#NMTitulo").val(),
            dso: $("#DSObjetivo").val(),
            dscon: $("#DSContexto").val(),
            dsp: $("#DSPrompt").val(),
            dscom: $("#DSComplemento").val(),
            nrm: $("#NRMaxTokens").val(),
            vrt: $("#VRTemperature").val().replace(".", "").replace(",", "."),
        },
        success: function (data) {
            $("#mainContainer").show();
            $("#LoadingContainer").hide();
            console.log(data);
            fillResponse(data.res);
        },
        failure: function (data) {
            $("#mainContainer").show();
            $("#LoadingContainer").hide();

            console.log("failure");
        },
        error: function (data) {
            $("#mainContainer").show();
            $("#LoadingContainer").hide();

            console.log("error");
            console.log(data.status);

        }
    });

    return false;
};

function fillResponse(response) {

    var content = "";

    content += "<p style=\"font-size: 12px\">" + response.DSAncoraPost + "</p>";
    content += "<h2>" + response.DSTituloPost + "</h2>";
    content += "<p style=\"font-size: 16px\">" + response.DSSubTituloPost + "</p>";
    content += "<hr />"
    content += "<div style=\"font-size: 14px\">" + response.DSTextoPost + "</div>";
    content += "<hr />"
    content += "<div><i class=\"fas fa-tags\"></i> " + response.DSTags + "</div>";

    $("#RespostaHtml").html(content);

    $("#DSAncoraPost").val(response.DSAncoraPost);
    $("#DSTituloPost").val(response.DSTituloPost);
    $("#DSSubTituloPost").val(response.DSSubTituloPost);
    $("#DSTextoPost").val(response.DSTextoPost);
    $("#DSTags").val(response.DSTags);

    $("#spanRespostaFormLabel").hide();
    $("#spanRespostaFormFields").show();
}

function savePost() {

    $('html, body').animate({ scrollTop: 0 }, 'fast');

    $("#mainContainer").hide();
    $("#LoadingContainer").show();


    $.ajax({
        url: '/Admin/SavePost',
        type: 'POST',
        data: {
            idau: $("#Pessoa").val(),
            idbl: $("#Blog").val(),
            idtp: $("#TipoPost").val(),
            dsan: $("#DSAncoraPost").val(),
            dstp: $("#DSTituloPost").val(),
            dsst: $("#DSSubTituloPost").val(),
            dste: $("#DSTextoPost").val(),
            dsta: $("#DSTags").val(),
            star: $("#STAcessoRestrito").val(),
        },
        success: function (data) {
            $("#mainContainer").show();
            $("#LoadingContainer").hide();
        },
        failure: function (data) {
            console.log("failure");
            $("#mainContainer").show();
            $("#LoadingContainer").hide();
        },
        error: function (data) {
            console.log("error");
            $("#mainContainer").show();
            $("#LoadingContainer").hide();
        }
    });

    return false;
};


