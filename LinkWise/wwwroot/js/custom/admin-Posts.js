document.addEventListener('DOMContentLoaded', function () {

    InitPagePosts();
});

function InitPagePosts() {
    console.log("InitPagePosts");

    getPosts();

}

function getPosts() {

    $.ajax({
        url: '/Admin/GetPosts',
        type: 'GET',
        success: function (data) {
            console.log(data);
            fillPosts(data.posts);
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

function getPost(idPost, callback) {
    $.ajax({
        url: '/Admin/GetPost',
        type: 'GET',
        data: {
            idp: idPost
        },
        success: function (data) {
            callback(data.post);
        },
        error: function () {
            console.log("Erro ao buscar post.");
            callback(null);
        }
    });
}

function fillPosts(posts) {
    console.log(posts);

    var dataSet = [];
    novaRow = [];
    btnRecursos = "";

    for (var i = 0; i < posts.length; i++) {

        entry = posts[i];
        novaRow = [];
        btnRecursos = "";

        novaRow.push(entry.idPost);
        novaRow.push(entry.dsTituloPost);
        novaRow.push(entry.dtCriacaoPostTwo.dsDate);
        novaRow.push(entry.dtPublicacaoPostTwo.dsDate);
        novaRow.push(entry.stPostAtivoTwo.dsStatusAtivo);
        novaRow.push(entry.stAcessoRestritoTwo.dsStatusAtivo);
        btnRecursos = '<div class="btn btn-default btn-xs text-center mx-1" title="Editar dados do post" onClick="postModal(' + entry.idPost + ')"><i class="fas fa-pencil-alt"></i></div>';
        btnRecursos += '<div class="btn btn-default btn-xs text-center mx-1" title="Editar texto do post" onClick="postTextoModal(' + entry.idPost + ')"><i class="fas fa-align-left"></i></div>';
        btnRecursos += '<div class="btn btn-default btn-xs text-center mx-1" title="Editar imagens" onClick="postImagesModal(' + entry.idPost + ')"><i class="far fa-images"></i></div>';
        btnRecursos += '<div class="btn btn-default btn-xs text-center" onClick="openPostAdmin(' + entry.idPost + ')" title="Editar Post"><i class="fa-regular fa-pen-to-square"></i></div>';
        novaRow.push(btnRecursos);

        dataSet.push(novaRow);
    }

    console.log(dataSet);

    new DataTable('#tbPosts', {
        // Oculta o campo de pesquisa
        searching: false,

        // Oculta a paginação
        paging: false,

        // Oculta o seletor de "quantidade por página"
        lengthChange: false,

        // Oculta o texto "Mostrando de X até Y de Z registros"
        info: false,
        columns: [
            { title: 'ID', className: 'text-start', width: '5%' },
            { title: 'Título', className: 'text-start', width: '40%' },
            { title: 'Criado Em', className: 'text-start', width: '15%' },
            { title: 'Publicado Em', className: 'text-start', width: '15%' },
            { title: 'Status', className: 'text-center', width: '5%' },
            { title: 'Privado', className: 'text-center', width: '5%' },
            { title: 'Recursos', className: 'text-center', width: '15%' }
        ],
        data: dataSet
    });

};

function postModal(idPost) {

    // Exibe o modal
    const myModal = new bootstrap.Modal(document.getElementById('postModal'), {
        keyboard: false
    });
    myModal.show();

    getPost(idPost, function (post) {
        if (!post) {
            alert("Post não encontrado.");
            return;
        }
        console.log(post);

        // Preenche os campos
        document.getElementById('IDPost').innerHTML = post.idPost;
        document.getElementById('DSAncoraPost').value = post.dsAncoraPost;
        document.getElementById('DTCriacaoPost').value = post.dtCriacaoPostTwo.dsDate;
        document.getElementById('DTPublicacaoPost').value = post.dtPublicacaoPostTwo.dsDate;
        document.getElementById('DSTituloPost').value = post.dsTituloPost;
        document.getElementById('DSSubTituloPost').value = post.dsSubTituloPost;
        document.getElementById('DSTags').value = post.dsTags;

        document.getElementById('postModal').addEventListener('shown.bs.modal', function () {
            const postData = { acessoRestrito: 0, ativo: 1 };

            const acesso = document.querySelector(`input[name="acessoRestrito"][value="${postData.acessoRestrito}"]`);
            const ativo = document.querySelector(`input[name="ativo"][value="${postData.ativo}"]`);

            if (acesso) acesso.checked = true;
            if (ativo) ativo.checked = true;
        });

        //$('input[name="acessoRestrito"][value="' + postData.acessoRestrito + '"]').prop('checked', true);
        //$('input[name="ativo"][value="' + postData.ativo + '"]').prop('checked', true);

        // Ao fechar o modal, você limpa os campos
        document.getElementById('postModal').addEventListener('hidden.bs.modal', function () {
            // Desmarca todos os radio buttons
            document.querySelectorAll('#postModal input[type="radio"]').forEach(el => el.checked = false);

            // Se quiser limpar outros campos também:
            document.querySelectorAll('#postModal input[type="text"], #postModal textarea').forEach(el => el.value = '');

            // ... limpe outras coisas aqui se necessário
        });
    });
}

function postTextoModal(idPost) {

    // Exibe o modal
    const myModal = new bootstrap.Modal(document.getElementById('postTextoModal'), {
        keyboard: false
    });
    myModal.show();

    getPost(idPost, function (post) {
        if (!post) {
            alert("Post não encontrado.");
            return;
        }
        console.log(post);

        // Preenche os campos
        document.getElementById('lblIDPostTexto').innerHTML = post.idPost;
        document.getElementById('lblDSTituloPostTexto').innerHTML = post.dsTituloPost;
        document.getElementById('DSTextoPost').value = post.dsTextoPost;

        // Ao fechar o modal, você limpa os campos
        document.getElementById('postTextoModal').addEventListener('hidden.bs.modal', function () {

            // Se quiser limpar outros campos também:
            document.querySelectorAll('#postModal input[type="text"], #postModal textarea').forEach(el => el.value = '');

            // ... limpe outras coisas aqui se necessário
        });
    });
}

function postImagesModal(idPost) {

    var countImages = 0;

    // Exibe o modal
    const myModal = new bootstrap.Modal(document.getElementById('postImagesModal'), {
        keyboard: false
    });

    myModal.show();

    clearFormImage();

    getPost(idPost, function (post) {
        if (!post) {
            alert("Post não encontrado.");
            return;
        }
        console.log(post);

        var divImages = document.getElementById('divImages');
        var contentDivImages = "";

        if (post.midias.length == 0) {
            contentDivImages = "<span>Nenhuma imagem encontrada para este post</span>"
        }
        else {
            post.midias.forEach(function (midia) {

                countImages++;

                contentDivImages += '<div class="row">';
                contentDivImages += '   <div class="col-lg-2"><span class="img-thumbnail d-block"><img class="img-fluid" src="' + midia.dsUrlMidia + '" alt="Project Image"></span></div>';
                contentDivImages += '   <div class="col-lg-5">';
                contentDivImages += '       <p><strong>' + midia.nmTitulo + '</strong>';
                contentDivImages += '       <br />' + midia.dsLegenda + '</p>';
                contentDivImages += '   </div>';
                contentDivImages += '   <div class="col-lg-3">';
                // Alternativa para o if a seguir   
                // contentDivImages += '<i class="fas fa-toggle-' + (midia.stMidiaMain ? 'on' : 'off') + '" style="cursor: pointer;' + (midia.stMidiaMain ? ' color:#000000' : '') + '" title="' + (midia.stMidiaMain ? 'Remover principal' : 'Tornar principal') + '" onclick="setMidiaPrincipal(' + midia.idMidia + ', ' + post.idPost + ')"></i>';
                if (midia.stMidiaMain) {
                    contentDivImages += '<div class="btn btn-sm btn-light mx-1" title="Mídia Principal"><i class="fas fa-toggle-on" style="color:#000000"></i></div>';
                }
                else {
                    contentDivImages += '<div class="btn btn-sm btn-light mx-1" title="Tornar principal" onclick="setMidiaMain(' + midia.idPostMidia + ', ' + midia.idMidia + ', ' + idPost + ')"><i class="fas fa-toggle-off" style="cursor: pointer"></i></div>';
                }
                contentDivImages += '       <div class="btn btn-sm btn-light" title="Excluir Imagem" onclick="delMidia(' + midia.idPostMidia + ', ' + midia.idMidia + ', ' + idPost + ')"><i class="fas fa-times" style="cursor: pointer"></i></div>';
                contentDivImages += '       <div class="btn btn-sm btn-light mx-1 my-1" title="Editar Imagem" onclick="editMidia(' + midia.idMidia + ', \'' + midia.nmTitulo + '\', \'' + midia.dsLegenda + '\', \'' + midia.cdEmbedded + '\')"><i class="fas fa-pencil" style="cursor: pointer"></i></div>';
                contentDivImages += '   </div>';
                contentDivImages += '   <div class="col-lg-2">';
                contentDivImages += '       <div class="input-group mb-3"><input type="number" id="NROrdem' + midia.idPostMidia + '" value="' + midia.nrOrdem + '" class="form-control form-control-sm" style="width: 50px"><span class="input-group-text text-3" style="cursor: pointer" title="Salvar" onclick="setPostMidiaOrdem(' + midia.idPostMidia + ')"><i class="fas fa-cloud-upload-alt"></i></span></div>';
                contentDivImages += '   </div>';
                contentDivImages += '</div>';

            });
        }


        document.getElementById('lblCountImages').innerHTML = countImages;

        divImages.innerHTML = contentDivImages;

        // Preenche os campos
        document.getElementById('lblIDPostImages').innerHTML = post.idPost;
        document.getElementById('lblDSTituloPostImages').innerHTML = post.dsTituloPost;

        // Ao fechar o modal, você limpa os campos
        document.getElementById('postTextoModal').addEventListener('hidden.bs.modal', function () {

            // Se quiser limpar outros campos também:
            document.querySelectorAll('#postModal input[type="text"], #postModal textarea').forEach(el => el.value = '');

            // ... limpe outras coisas aqui se necessário
        });
    });
};

function saveMidia() {

    const idMidia = document.getElementById('IDMidia').innerHTML;
    const idPost = document.getElementById('lblIDPostImages').innerHTML;

    if (idMidia == 0) {
        addMidia(idPost);
    }
    else {
        updMidia(idMidia, idPost);
    }

};

function addMidia(idPost) {

    const inputFile = document.getElementById('inputFileUpload'); // <- aqui você pega o input
    const files = inputFile.files; // <- e aqui os arquivos

    //if (!files || files.length === 0) {
    //    alert("Selecione pelo menos um arquivo.");
    //    return;
    //}

    const formData = new FormData();

    formData.append("idp", idPost);
    formData.append("nmt", document.getElementById('NMTitulo').value);
    formData.append("dsl", document.getElementById('DSLegenda').value);
    formData.append("cde", document.getElementById('CDEmbedded').value);

    for (const file of files) {
        formData.append("files", file);
    }

    fetch("/Admin/SaveImagemPost", {
        method: "POST",
        body: formData
    }).then(data => {
        // Fecha o modal
        const modalEl = document.getElementById('postImagesModal');
        const modalInstance = bootstrap.Modal.getInstance(modalEl);
        modalInstance.hide();

        // Aguarda o fechamento antes de reabrir
        setTimeout(() => {
            postImagesModal(idPost);
        }, 350);
    });
};

function updMidia(idMidia, idPost) {

    const formData = new FormData();

    formData.append("idm", idMidia);
    formData.append("nmt", document.getElementById('NMTitulo').value);
    formData.append("dsl", document.getElementById('DSLegenda').value);
    formData.append("cde", document.getElementById('CDEmbedded').value);

    fetch("/Admin/UpdMidia", {
        method: "POST",
        body: formData
    }).then(data => {
        // Fecha o modal
        const modalEl = document.getElementById('postImagesModal');
        const modalInstance = bootstrap.Modal.getInstance(modalEl);
        modalInstance.hide();

        // Aguarda o fechamento antes de reabrir
        setTimeout(() => {
            postImagesModal(idPost);
        }, 350);
    });
};

function setMidiaMain(idPostMidia, idMidia, idPost) {

    const params = new URLSearchParams();
    params.append("idpm", idPostMidia);
    params.append("idm", idMidia);
    params.append("idp", idPost);

    fetch('/Admin/SetMidiaMain', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao definir mídia como principal.');
            }
            return response.json(); // ou .text() se não retornar JSON
        })
        .then(data => {
            // Oculta o modal
            const modalElement = document.getElementById('postImagesModal');
            const modalInstance = bootstrap.Modal.getInstance(modalElement);
            modalInstance.hide();

            // Aguarda a animação de fechamento do modal (usualmente 300ms)
            setTimeout(() => {
                postImagesModal(idPost);
            }, 350);
        })
        .catch(error => {
            console.error(error);
            alert('Não foi possível atualizar a mídia principal.');
        });
}

function setPostMidiaOrdem(idPostMidia) {
    const input = document.getElementById('NROrdem' + idPostMidia);
    const novaOrdem = parseInt(input.value);

    const params = new URLSearchParams();
    params.append("idpm", idPostMidia);
    params.append("nro", novaOrdem);

    if (isNaN(novaOrdem) || novaOrdem < 0) {
        alert("Informe uma ordem válida.");
        return;
    }

    fetch('/Admin/SetPostMidiaOrdem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Erro ao atualizar a ordem.");
            }

            // Feedback visual simples no botão
            const iconSpan = input.nextElementSibling;
            if (iconSpan) {
                const original = iconSpan.innerHTML;
                iconSpan.innerHTML = '<i class="fas fa-check-circle text-success"></i>';

                setTimeout(() => {
                    iconSpan.innerHTML = original;
                }, 1500);
            }
        })
        .catch(error => {
            console.error(error);
            alert("Erro ao atualizar a ordem.");
        });
}

function delMidia(idPostMidia, idMidia, idPost) {
    if (!confirm("Tem certeza que deseja excluir esta mídia?")) return;


    const params = new URLSearchParams();
    params.append("idpm", idPostMidia);
    params.append("idm", idMidia);

    fetch('/Admin/DelMidia', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params
    })
    .then(response => {
        if (!response.ok) {
            throw new Error("Erro ao excluir a mídia.");
        }
        return response.json(); // ou .text(), dependendo da resposta
    })
    .then(data => {
        // Fecha o modal
        const modalEl = document.getElementById('postImagesModal');
        const modalInstance = bootstrap.Modal.getInstance(modalEl);
        modalInstance.hide();

        // Aguarda o fechamento antes de reabrir
        setTimeout(() => {
            postImagesModal(idPost);
        }, 350);
    })
    .catch(error => {
        console.error(error);
        alert("Não foi possível excluir a mídia.");
    });
}

function editMidia(idMidia, nmTitulo, dsLegenda, cdEmbedded) {

    document.getElementById('lblAccordionPostImages').innerHTML = "Editar imagem";
    document.getElementById('IDMidia').innerHTML = idMidia;
    document.getElementById('NMTitulo').value = nmTitulo;
    document.getElementById('DSLegenda').value = dsLegenda;
    document.getElementById('CDEmbedded').value = cdEmbedded;
}

function clearFormImage() {

    document.getElementById('lblAccordionPostImages').innerHTML = "Adicione uma imagem";
    document.getElementById('IDMidia').innerHTML = "0";
    document.getElementById('NMTitulo').value = "";
    document.getElementById('DSLegenda').value = "";
    document.getElementById('CDEmbedded').value = "";
}

function openPostAdmin(idPost) {
    window.location = "/Admin/Post/?idp=" + idPost;
}






