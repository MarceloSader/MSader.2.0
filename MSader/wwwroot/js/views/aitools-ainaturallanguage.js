$(document).ready(function () {
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
        url: '/AITools/GetPrompt',
        type: 'POST',
        data: {
            idp: idPrompt
        },
        success: function (data) {
            console.log(data);
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
    console.log(prompt);
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
        url: '/AITools/SavePrompt',
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
        url: '/AITools/SavePrompt',
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

    $("#mainContainer").hide();
    $("#LoadingContainer").show();

    $.ajax({
        url: '/AITools/RunContentGeneratorOpenAI',
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
            fillResponse(data.res);
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

function fillResponse(response) {

    var content = "";

    content += "<p style=\"font-size: 12px\">" + response.dsAncoraPost + "</p>";
    content += "<h2>" + response.dsTituloPost + "</h2>";
    content += "<p style=\"font-size: 16px\">" + response.dsSubTituloPost + "</p>";
    content += "<hr />"
    content += "<div style=\"font-size: 14px\">" + response.dsTextoPost + "</div>";
    content += "<hr />"
    content += "<div><i class=\"far fa-tags\"></i> " + response.dsTags + "</div>";

    $("#RespostaHtml").html(content);

    $("#DSTAncoraPost").val(response.dsAncoraPost);
    $("#DSTituloPost").val(response.dsTituloPost);
    $("#DSSubTituloPost").val(response.dsSubTituloPost);
    $("#DSTextoPost").val(response.dsTextoPost);
    $("#DSTags").val(response.dsTags);

    $("#spanRespostaFormLabel").hide();
    $("#spanRespostaFormFields").show();
}

function savePost() {


    $("#mainContainer").hide();
    $("#LoadingContainer").show();


    $.ajax({
        url: '/AITools/SavePost',
        type: 'POST',
        data: {
            idau: $("#Pessoa").val(),
            idtp: $("#TipoPost").val(),
            dsan: $("#DSAncoraPost").val(),
            dstp: $("#DSTituloPost").val(),
            dsst: $("#DSSubTituloPost").val(),
            dste: $("#DSTextoPost").val(),
            dsta: $("#DSTags").val(),
        },
        success: function (data) {
            fillResponse(data.res);
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


