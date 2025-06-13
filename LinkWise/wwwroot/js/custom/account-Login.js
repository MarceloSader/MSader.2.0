document.addEventListener('DOMContentLoaded', function () {

    initPageLogin();

});

function initPageLogin() {
    let dsEmail = localStorage.getItem('linkwise_dsemail');
    let cdChave = localStorage.getItem('linkwise_cdchave');

    if (dsEmail != null && dsEmail.length > 0) {
        document.getElementById('DSEmail').value = dsEmail;
        document.getElementById('CDChave').value = cdChave;
    } else {
        console.log('Credenciais não localizadas. ' + dsEmail + ', ' + cdChave + '.');
    }
}

function submitLogon() {

    let local_dse = document.getElementById('DSEmail').value;
    let local_cdc = document.getElementById('CDChave').value;

    $.ajax({
        url: '/Account/Logon',
        type: 'POST',
        data: {
            dse: local_dse,
            cdc: local_cdc
            //dse: $("#DSEmail").val(),
            //cdc: $("#CDChave").val()
        },
        success: function (data) {
            console.log(data);
            getOrCreateLocalCredentials(local_dse, local_cdc);
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


// Verifica se o identificador já está no localStorage
function getOrCreateLocalCredentials(dsEmail, cdChave) {

    let localDSEmail = localStorage.getItem('linkwise_dsemail');
    let localCDChave = localStorage.getItem('linkwise_cdchave');

    console.log('data: ' + localDSEmail + '; ' + localCDChave + '.');

    if (!localDSEmail) {
        localStorage.setItem('linkwise_dsemail', dsEmail);
        localStorage.setItem('linkwise_cdchave', cdChave);
        console.log('Local data created');
    } else {
        console.log('Usuário já identificado');
    }
}



