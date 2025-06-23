document.addEventListener('DOMContentLoaded', function () {
    $("#divSubmeterPrompt").hide();

    $("#Prompts").on('change', function () {
        if ($("#Prompts").val() == 0) {
            clearPrompt();
        }
        else {
            getPrompt($(this).val(), "text")
        }
    });

    $("#PromptsPost").on('change', function () {
        if ($("#PromptsPost").val() == 0) {
            clearPromptPost();
        }
        else {
            getPrompt($(this).val(), "post")
        }
    });

    $("#LoadingContainer").hide();

    $("#spanRespostaFormLabel").show();

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

function fillScraped(scraping) {

    let inputDSOriginalText = document.getElementById('DSOriginalText');
    let inputDSOriginalTextHtml = document.getElementById('DSOriginalTextHtml');
    let inputDSPrompt = document.getElementById('DSPrompt');

    inputDSOriginalText.value = scraping.scraping.dsTextScraped;

    inputDSOriginalTextHtml.innerHTML = scraping.scraping.dsTextScraped.replace(/\n/g, '<br>');

    inputDSPrompt.value = inputDSPrompt.value.replace("[TEXTO DA FONTE AQUI]", inputDSOriginalText.value);

}

// PROMPTS

function getPrompt(idPrompt, aba) {

    $.ajax({
        url: '/Admin/GetPrompt',
        type: 'POST',
        data: {
            idp: idPrompt
        },
        success: function (data) {
            if (aba == "text") {
                fillPrompt(data);
            }
            else {
                fillPromptPost(data);
            }
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

    let promptDetails = "";

    promptDetails += '<span>' + prompt.dsObjetivo + '</span> '
    promptDetails += '<span>(# ' + prompt.idPrompt + ')</span><hr />'

    document.getElementById("divPromptDetails").innerHTML = promptDetails + '<hr />'

    document.getElementById("DSPrompt").value = prompt.dsPrompt;

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

    removeTagsUnsed();

    $('html, body').animate({ scrollTop: 0 }, 'fast');

    $("#MainContainer").hide();
    $("#LoadingContainer").show();

    $.ajax({
        url: '/Content/RunContentGeneratorOpenAI',
        type: 'POST',
        data: {
            dsp: $("#DSPrompt").val()
        },
        success: function (data) {
            $("#MainContainer").show();
            $("#LoadingContainer").hide();
            console.log(data);
            fillResponse(data.res);
        },
        failure: function (data) {
            $("#MainContainer").show();
            $("#LoadingContainer").hide();

            console.log("failure");
        },
        error: function (data) {
            $("#MainContainer").show();
            $("#LoadingContainer").hide();

            console.log("error");
            console.log(data.status);

        }
    });

    return false;
};

function fillResponse(content) {
    const textoFormatado = aplicarQuebraEmParagrafos(content);

    const container = document.getElementById('DSTextProcessResult');
    if (container) {
        container.innerHTML = textoFormatado;
    }
}

function savePost() {

    $('html, body').animate({ scrollTop: 0 }, 'fast');

    $("#MainContainer").hide();
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
            $("#MainContainer").show();
            $("#LoadingContainer").hide();
        },
        failure: function (data) {
            console.log("failure");
            $("#MainContainer").show();
            $("#LoadingContainer").hide();
        },
        error: function (data) {
            console.log("error");
            $("#MainContainer").show();
            $("#LoadingContainer").hide();
        }
    });

    return false;
};

function replaceTagByText(tag, idTextSource, idTextTarget) {

    const textoSubstituto = document.getElementById(idTextSource).innerText || document.getElementById(idTextSource).value;
    const targetElement = document.getElementById(idTextTarget);
    const textoOriginal = targetElement.innerText || targetElement.value;

    const regex = new RegExp(tag.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g');
    const textoAtualizado = textoOriginal.replace(regex, textoSubstituto);

    if (targetElement.value !== undefined) {
        targetElement.value = textoAtualizado;
    } else {
        targetElement.innerText = textoAtualizado;
    }
}

// POST

function runPromptPostOpenAI() {

    removeTagsUnsed();

    const idPromptPost = document.getElementById('PromptsPost').value;

    if (idPromptPost == 10) // Tabela PR_Prompt: IDPrompt = 10, Extrair Título, Subtítulo, Ancora e Tags
    {
        getPostMetaData();
    }
    else if (idPromptPost == 11) // Tabela PR_Prompt: IDPrompt = 11, Geração do Corpo do Post 
    {
        getPostBody();
    }
    else if (idPromptPost == 12) // Tabela PR_Prompt: IDPrompt = 12, Geração do Parágrafo de Destaque
    {
        getPostDestaque();
    }

    return false;
};

function getPostMetaData() {

    removeTagsUnsed();

    $('html, body').animate({ scrollTop: 0 }, 'fast');

    startLoading();

    $.ajax({
        url: '/Content/GetPostMetaData',
        type: 'POST',
        data: {
            dsp: $("#DSPromptPost").val()
        },
        success: function (data) {
            stopLoading();
            console.log(data);
            fillMetadataPost(data.post);
        },
        failure: function (data) {
            stopLoading();

            console.log("failure");
        },
        error: function (data) {
            stopLoading();

            console.log("error");
            console.log(data.status);

        }
    });

    return false;
};

function getPostBody() {

    removeTagsUnsed();

    $('html, body').animate({ scrollTop: 0 }, 'fast');

    startLoading();

    $.ajax({
        url: '/Content/RunContentGeneratorOpenAI',
        type: 'POST',
        data: {
            dsp: $("#DSPromptPost").val()
        },
        success: function (data) {
            stopLoading();
            console.log(data.res);
            fillPostBody(data.res);
        },
        failure: function (data) {
            stopLoading();

            console.log("failure");
        },
        error: function (data) {
            stopLoading();

            console.log("error");
            console.log(data.status);

        }
    });

    return false;
};

function fillPromptPost(prompt) {

    let promptDetails = "";


    promptDetails += '<span>' + prompt.dsObjetivo + '</span> '
    promptDetails += '<span>(# ' + prompt.idPrompt + ')</span>'

    document.getElementById("divPromptDetailsPost").innerHTML = promptDetails;

    document.getElementById("DSPromptPost").value = prompt.dsPrompt;

}

function fillMetadataPost(post) {


    document.getElementById("DSAncoraPost").value = post.dsAncoraPost;

    document.getElementById("DSTituloPost").value = post.dsTituloPost;

    document.getElementById("DSSubTituloPost").value = post.dsSubTituloPost;

    document.getElementById("DSTags").value = post.dsTags;

}

function fillPostBody(content) {

    console.log(content);

    document.getElementById("DSTextoPost").value = content;
    document.getElementById("DSTextoPostHtml").innerHTML = content;

}

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

            aplicarDiretriz('Diretriz_EstiloResposta', data.dsDiretriz, 'IDEstiloResposta');
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

            document.getElementById('Diretriz_FormatoSaida').innerHTML = data.dsDiretriz;

            aplicarDiretriz('Diretriz_FormatoSaida', data.dsDiretriz, 'IDFormatoSaida');
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
            aplicarDiretriz('Diretriz_Vies_' + categoria, data.dsDiretriz, 'IDVies' + categoria);
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
    $("#MainContainer").hide();
    $("#LoadingContainer").show();
}

function stopLoading() {
    $("#MainContainer").show();
    $("#LoadingContainer").hide();
};

function resetVieses() {
    let stResetVieses = document.getElementById("STResetVieses");

    let btnResetVieses = document.getElementById("btnResetVieses");

    if (stResetVieses.value == "0") {

        applyViesNeutro();

        stResetVieses.value = "1";

        document.getElementById("IDViesPolitico").disabled = true;
        document.getElementById("IDViesEconomico").disabled = true;
        document.getElementById("IDViesCultural_Social").disabled = true;
        document.getElementById("IDViesFilosofico_Etico").disabled = true;
        document.getElementById("IDViesCientifico_Tecnologico").disabled = true;

        btnResetVieses.classList.remove('btn-default');
        btnResetVieses.classList.add('btn-primary');

        btnResetVieses.innerHTML = "Habilitar Vieses";

        getVies(document.getElementById("IDViesNeutro").value, "Neutro");

    }
    else {

        stResetVieses.value = "0";

        document.getElementById("IDViesPolitico").disabled = false;
        document.getElementById("IDViesEconomico").disabled = false;
        document.getElementById("IDViesCultural_Social").disabled = false;
        document.getElementById("IDViesFilosofico_Etico").disabled = false;
        document.getElementById("IDViesCientifico_Tecnologico").disabled = false;

        btnResetVieses.classList.remove('btn-primary');
        btnResetVieses.classList.add('btn-default');

        btnResetVieses.innerHTML = "Inabilitar Vieses";
    }
}

function clearPrompt() {

    document.getElementById("divPromptDetails").innerHTML = "";

    document.getElementById("DSPrompt").value = "";
    document.getElementById("IDPrompt").value = "0";
    document.getElementById("Prompts").value = "0";

    document.getElementById("IDEstiloResposta").value = "0";
    document.getElementById("Diretriz_EstiloResposta").innerHTML = "";

    document.getElementById("IDFormatoSaida").value = "0";
    document.getElementById("Diretriz_FormatoSaida").innerHTML = "";

    applyViesNeutro();

}

function clearPromptPost() {

    document.getElementById("divPromptDetailsPost").innerHTML = "";
    document.getElementById("DSPromptPost").value = "";
    document.getElementById("PromptsPost").value = "0";
}

function clearFormPost() {

    document.getElementById("Pessoa").value = "0";
    document.getElementById("TipoPost").value = "0";
    document.getElementById("AcessoRestrito").value = "0";

    document.getElementById("DSAncoraPost").value = "";
    document.getElementById("DSTituloPost").value = "";
    document.getElementById("DSSubTituloPost").value = "";
    document.getElementById("DSTextoPost").value = "";
    document.getElementById("DSTags").value = "";

}

function applyViesNeutro() {

    let btnResetVieses = document.getElementById("btnResetVieses");

    document.getElementById("IDViesPolitico").value = "0";
    document.getElementById("Diretriz_Vies_Politico").innerHTML = "";
    document.getElementById("IDViesEconomico").value = "0";
    document.getElementById("Diretriz_Vies_Economico").innerHTML = "";
    document.getElementById("IDViesCultural_Social").value = "0";
    document.getElementById("Diretriz_Vies_Cultural_Social").innerHTML = "";
    document.getElementById("IDViesFilosofico_Etico").value = "0";
    document.getElementById("Diretriz_Vies_Filosofico_Etico").innerHTML = "";
    document.getElementById("IDViesCientifico_Tecnologico").value = "0";
    document.getElementById("Diretriz_Vies_Cientifico_Tecnologico").innerHTML = "";
    document.getElementById("Diretriz_Vies_Neutro").innerHTML = "";

    document.getElementById("STResetVieses").value = "0";

    document.getElementById("IDEstiloResposta").disabled = false;
    document.getElementById("IDFormatoSaida").disabled = false;

    document.getElementById("IDViesPolitico").disabled = false;
    document.getElementById("IDViesEconomico").disabled = false;
    document.getElementById("IDViesCultural_Social").disabled = false;
    document.getElementById("IDViesFilosofico_Etico").disabled = false;
    document.getElementById("IDViesCientifico_Tecnologico").disabled = false;

    btnResetVieses.classList.remove('btn-primary');
    btnResetVieses.classList.add('btn-default');

    btnResetVieses.innerHTML = "Inabilitar Vieses";
}

function aplicarDiretriz(tag, valor, idSelect) {

    console.log(valor);

    const textarea = document.getElementById("DSPrompt");

    document.getElementById(tag).innerHTML = valor + '&nbsp; <div class="btn btn-primary btn-rounded btn-xs" onclick="removerDiretriz(\'' + tag + '\',' + '\'' + valor + '\',' + '\'' + idSelect + '\')"><i class="fa-solid fa-arrow-left text-light"></i></div>'

    tag = '[' + tag + ']';

    let texto = textarea.value;

    if (!valor) {
        alert("Selecione um valor válido.");
        return;
    }

    const regex = new RegExp(tag.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g');

    texto = texto.replace(regex, valor);

    textarea.value = texto;

    // Desabilita o select
    document.getElementById(idSelect).disabled = true;
}

function removerDiretriz(tag, valor, idSelect) {

    const textarea = document.getElementById("DSPrompt");

    document.getElementById(tag).innerHTML = "";

    tag = '[' + tag + ']';

    let texto = textarea.value;

    const regex = new RegExp(valor.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g');
    texto = texto.replace(regex, tag);

    textarea.value = texto;

    // Habilita o select novamente
    document.getElementById(idSelect).disabled = false;
    document.getElementById(idSelect).value = "0";
}

function removeTagsUnsed() {

    const textarea = document.getElementById("DSPrompt");

    let texto = textarea.value;

    tags = ['[Diretriz_Vies_Neutro]', '[Diretriz_Vies_Politico]', '[Diretriz_Vies_Economico]', '[Diretriz_Vies_Cultural_Social]', '[Diretriz_Vies_Filosofico_Etico]', '[Diretriz_Vies_Cientifico_Tecnologico]', '[Diretriz_EstiloResposta]', '[Diretriz_FormatoSaida]'];

    tags.forEach(tag => {
        const regex = new RegExp(tag.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g');

        texto = texto.replace(regex, "");
    });


    textarea.value = texto;
}

function aplicarQuebraEmParagrafos(texto) {
    // Quebra por linhas
    const linhas = texto.split(/\r\n|\n|\r/);

    // Filtra linhas vazias e envolve cada uma em <p>
    const paragrafos = linhas
        .filter(linha => linha.trim() !== '')
        .map(linha => `<p>${linha.trim()}</p>`);

    return paragrafos.join('');
}

// obsoleto


function fillResponse_Obsoleto(response) {

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