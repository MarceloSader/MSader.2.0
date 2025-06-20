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

    document.getElementById('IDEstiloResposta').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Estilo Resposta:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getEstiloResposta(valorSelecionado);
    });

    document.getElementById('IDFormatoSaida').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Formato Saída:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getFormatoSaida(valorSelecionado);
    });

    document.getElementById('IDViesPolitico').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Vies:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getVies(valorSelecionado, "Politico");
    });

    document.getElementById('IDViesEconomico').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Vies:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getVies(valorSelecionado, "Economico");
    });

    document.getElementById('IDViesCultural_Social').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Vies:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getVies(valorSelecionado, "Cultural_Social");
    });

    document.getElementById('IDViesFilosofico_Etico').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Vies:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getVies(valorSelecionado, "Filosofico_Etico");
    });

    document.getElementById('IDViesCientifico_Tecnologico').addEventListener('change', function () {
        const valorSelecionado = this.value;
        console.log("Valor selecionado Vies:", valorSelecionado);

        // 🔥 Chama sua função passando o valor
        getVies(valorSelecionado, "Cientifico_Tecnologico");
    });

    getPromptResumo();

});

// SCRAPING

function runScraping() {

    startLoading();

    $.ajax({
        url: '/Content/RunScraping',
        type: 'POST',
        data: {
            url: document.getElementById('DSUrlForScraping').value
        },
        success: function (data) {

            fillScraped(data);

            stopLoading();
        },
        failure: function (data) {
            console.log("failure");
            stopLoading();
        },
        error: function (data) {
            console.log("error");
            stopLoading();
        }
    });

    return false;
};

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

function getPromptResumo(idPrompt) {

    $.ajax({
        url: '/Admin/GetPrompt',
        type: 'POST',
        data: {
            idp: 9
        },
        success: function (data) {

            fillPromptResumo(data);
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

function fillScraped(scraping) {

    document.getElementById('DSTextScraped').value = scraping.scraping.dsTextScraped;

    const formattedHtml = scraping.scraping.dsTextScraped.replace(/\n/g, '<br>');

    document.getElementById('DSTextScrapedHtml').innerHTML = formattedHtml;

    let promptResumo = document.getElementById('DSPromptResumo').value;

    promptResumo = promptResumo.replace("[TEXTO_DA_FONTE_ORIGINAL]", formattedHtml);

    console.log(promptResumo);

    document.getElementById('DSPromptResumo').value = promptResumo;
}

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

function fillPromptResumo(prompt) {

    console.log(prompt);

    $("#DSPromptResumo").val(prompt.dsPrompt);

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

// DIRETRIZES

function getEstiloResposta(idEstiloResposta) {

    $.ajax({
        url: '/Admin/GetEstiloResposta',
        type: 'GET',
        data: {
            ier: idEstiloResposta
        },
        success: function (data) {
            console.log(data);
            document.getElementById('DSDiretriz_EstiloResposta').innerHTML = data.dsDiretriz;
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

function getFormatoSaida(idFormatoSaida) {

    $.ajax({
        url: '/Admin/GetFormatoSaida',
        type: 'GET',
        data: {
            ifs: idFormatoSaida
        },
        success: function (data) {
            console.log(data);
            document.getElementById('DSDiretriz_FormatoSaida').innerHTML = data.dsDiretriz;
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

function getVies(idVies, categoria) {

    $.ajax({
        url: '/Admin/GetVies',
        type: 'GET',
        data: {
            idv: idVies
        },
        success: function (data) {
            console.log(data);
            document.getElementById('DSDiretriz_Vies_' + categoria).innerHTML = data.dsVies;
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


// HELPERS

function startLoading() {
    $("#mainContainer").hide();
    $("#LoadingContainer").show();
}

function stopLoading() {
    $("#mainContainer").show();
    $("#LoadingContainer").hide();
};

;


