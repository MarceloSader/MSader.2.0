$(document).ready(function () {


});

function submitLogon() {

    $.ajax({
        url: '/Account/Logon',
        type: 'POST',
        data: {
            dse: $("#DSEmail").val(),
            cdc: $("#CDChave").val()
        },
        success: function (data) {
            console.log(data);
            window.location = "/Admin/HomeAdmin";
        },
        failure: function (data) {
            console.log("failure");
        },
        error: function (data) {
            console.log(data);
            if (data.status === 401) {
                alert("Usuário ou senha inválidos.");
            } else {
                alert("Erro inesperado: " + data.status);
            }
        }
    });

    return false;
};





