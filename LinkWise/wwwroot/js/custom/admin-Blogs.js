$(document).ready(function () {

    $("#divSubmeterPrompt").hide();

    InitPageBlogs();
});

function InitPageBlogs() {
    console.log("InitPageBlogs");

    getBlogs();

}

function getBlogs() {

    $.ajax({
        url: '/Admin/GetBlogs',
        type: 'GET',
        success: function (data) {
            console.log(data);
            fillBlogs(data.blogs);
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



function fillBlogs(blogs) {
    console.log(blogs);

    var dataSet = [];
    novaRow = [];
    btnRecursos = "";

    for (var i = 0; i < blogs.length; i++) {

        entry = blogs[i];
        novaRow = [];
        btnRecursos = "";

        novaRow.push(entry.idBlog);
        novaRow.push(entry.nmBlog);
        novaRow.push(entry.dsUrlBlog);
        novaRow.push(entry.dsBlog);
        novaRow.push(entry.stBlogAtivo);
        btnRecursos = '<div class="btn btn-default btn-xs text-center mx-1" title="Editar"><i class="fas fa-pencil-alt"></i></div>';
        btnRecursos += '<div class="btn btn-default btn-xs text-center" title="Ver Home do Blog"><i class="fas fa-th"></i></div>';
        novaRow.push(btnRecursos);
        
        dataSet.push(novaRow);
    }

    console.log(dataSet);

    new DataTable('#tbBlogs', {
        // Oculta o campo de pesquisa
        searching: false,

        // Oculta a paginação
        paging: false,

        // Oculta o seletor de "quantidade por página"
        lengthChange: false,

        // Oculta o texto "Mostrando de X até Y de Z registros"
        info: false,
        columns: [
            { title: 'ID', className: 'text-start', width: '10%' },
            { title: 'Nome', className: 'text-start', width: '15%' },
            { title: 'Url', className: 'text-start', width: '15%' },
            { title: 'Descrição', className: 'text-start', width: '45%' },
            { title: 'Status', className: 'text-center', width: '5%' },
            { title: '...', className: 'text-center', width: '10%' }
        ],
        data: dataSet
    });

};



