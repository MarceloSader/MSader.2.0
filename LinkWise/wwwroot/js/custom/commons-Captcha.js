$(document).ready(function () {

    "use strict";

    $("#captchaNetVet").on("click", function () {

        var btnForm = $("#" + $(this).data("idbtnform"));

        sendingCaptcha(btnForm);

        setTimeout(function () {
            getToken(btnForm);
        }, 2000)
    });

    $("#processing_0").show();
    $("#processing_1").hide();
    $("#processing_2").hide();

    $("#btnFormSubmit").addClass("disabled");

});

function getToken(btnForm) {

    $.ajax({
        url: '/Blog/GetToken',
        type: 'POST',
        data: {
            who: "dwa"
        },
        success: function (data) {

            if (data.st == 'OK') {

                $("#tknCaptcha").val(data.tk);

                resetCaptchaYes(btnForm);

                /// public class CaptchaToken
                /// <summary>
                /// Tempo em segundos definido para expiração do token.
                /// </summary>
                /// public const int TIME_EXPIRATION = 30;
                setTimeout(function () {
                    resetCaptchaNo("exp", btnForm);
                }, 30000)
            }
            else {
                resetCaptchaNo("erro", btnForm);
            }
        },
        failure: function (data) {
            resetCaptchaNo("erro", btnForm);
        },
        error: function (data) {
            resetCaptchaNo("erro", btnForm);
        }
    });

    // O retorno é para o evento do click do form, que, caso receba true, enviará o fomulário novamente.
    // Por isso deve ser false.
    return false;
};

function sendingCaptcha(btnForm) {

    btnForm.attr("disabled", "disabled");
    btnForm.addClass("disabled");

    $("#processing_0").hide();
    $("#processing_1").show();
    $("#processing_2").hide();

    $("#captchaMsg").html("Validando...");
};

function resetCaptchaNo(st, btnForm) {

    btnForm.attr("disabled", "disabled");
    btnForm.addClass("disabled");

    $("#processing_0").show();
    $("#processing_1").hide();
    $("#processing_2").hide();

    if (st == "erro") {
        $("#captchaMsg").html("Ocorreu um erro. Por favor, tente novamente.");
    }
    else if (st == "exp") {
        $("#captchaMsg").html("Tempo limite expirou. Valide novamente.");
    }
};

function resetCaptchaYes(btnForm) {

    btnForm.removeAttr("disabled");
    btnForm.removeClass("disabled");

    $("#processing_0").hide();
    $("#processing_1").hide();
    $("#processing_2").show();

    var src2 = $("#processing_2").attr("src");

    $("#processing_2").attr('src', src2 + "?a=" + Math.random());

    $("#captchaMsg").html("Validação concluída. Envie o formulário.");



};
