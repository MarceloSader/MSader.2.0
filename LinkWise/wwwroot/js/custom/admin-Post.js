document.addEventListener('DOMContentLoaded', function () {

    InitPagePosts();
});

function InitPagePosts() {

    var idPost = document.getElementById('IDPost').value;

    getPost(idPost, function (post) {
        if (!post) {
            alert("Post não encontrado.");
            return;
        }
        console.log(post);

        // Preenche os campos

        document.getElementById('Blog').value = post.idBlog;
        document.getElementById('IDPost').value = post.idPost;
        document.getElementById('Pessoa').value = post.idPessoa;
        document.getElementById('TipoPost').value = post.idTipoPost;
        document.getElementById('DSAncoraPost').value = post.dsAncoraPost;
        document.getElementById('DTCriacaoPost').value = post.dtCriacaoPostTwo.dsDate;
        document.getElementById('DTPublicacaoPost').value = post.dtPublicacaoPostTwo.dsDate;
        document.getElementById('DSTituloPost').value = post.dsTituloPost;
        document.getElementById('DSSubTituloPost').value = post.dsSubTituloPost;
        document.getElementById('DSTags').value = post.dsTags;
        document.getElementById('DSTextoPost').value = post.dsTextoPost;

        document.getElementById('DSTituloPostHtml').innerHTML = post.dsTituloPost;
        document.getElementById('DSSubTituloPostHtml').innerHTML = post.dsSubTituloPost;
        document.getElementById('DSTextoPostHtml').innerHTML = post.dsTextoPost;

        const acesso = document.querySelector(`input[name="acessoRestrito"][value="${post.stAcessoRestritoSql}"]`);
        const ativo = document.querySelector(`input[name="ativo"][value="${post.stPostAtivoSql}"]`);

        if (acesso) acesso.checked = true;
        if (ativo) ativo.checked = true;


    });
}

function savePost() {

    const acessoRestritoValue = document.querySelector('input[name="acessoRestrito"]:checked')?.value;
    const star = acessoRestritoValue !== undefined ? parseInt(acessoRestritoValue) : null;

    const ativoValue = document.querySelector('input[name="ativo"]:checked')?.value;
    const stpa = ativoValue !== undefined ? parseInt(ativoValue) : null

    console.log(star);
    console.log(stpa);

    $.ajax({
        url: '/Admin/SavePostTwo',
        type: 'POST',
        data: {
            idpo: document.getElementById('IDPost').value,
            idbl: document.getElementById('Blog').value,
            idau: document.getElementById('Pessoa').value,
            idtp: document.getElementById('TipoPost').value,
            dsan: document.getElementById('DSAncoraPost').value,
            dstp: document.getElementById('DSTituloPost').value,
            dsst: document.getElementById('DSSubTituloPost').value,
            dste: document.getElementById('DSTextoPost').value,
            dsta: document.getElementById('DSTags').value,
            star: star,
            stpa: stpa,
            dtcr: document.getElementById('DTCriacaoPost').value,
            dtpu: document.getElementById('DTPublicacaoPost').value,
        },
        success: function (data) {
            console.log(data.idPost);
            document.getElementById('IDPost').value = data.idPost;
            document.getElementById('DSTextoPostHtml').innerHTML = document.getElementById('DSTextoPost').value;
            document.getElementById('DSTituloPostHtml').innerHTML = document.getElementById('DSTituloPost').value;
            document.getElementById('DSSubTituloPostHtml').innerHTML = document.getElementById('DSSubTituloPost').value;
        },
        error: function () {
            console.log("Erro ao salvar post.");
        }
    });
}

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

function fillPost(post) {
   

};
