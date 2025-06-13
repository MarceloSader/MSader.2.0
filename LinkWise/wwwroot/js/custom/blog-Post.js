
document.addEventListener('DOMContentLoaded', function () {

    InitPagePost();
});

function InitPagePost() {
    console.log("InitPagePost");

    const userId = getOrCreateUserId();

    const idPost = document.getElementById('IDPost').value;

    getPostComments(idPost);

    document.getElementById('divComentarBottom').style.display = 'none';
    document.getElementById('notificationPostComment').style.display = 'none';

}

function getPostComments(idPost) {

    document.getElementById('divPostCommentsLoading').style.display = 'block';
    document.getElementById('divPostComments').innerHTML = '';

    setTimeout(function () {

        $.ajax({
            url: '/Blog/GetPostComments',
            type: 'GET',
            data: {
                p: idPost
            },
            success: function (data) {
                fillPostComments(data.postComments);

                document.getElementById('divPostCommentsLoading').style.display = 'none';
            },
            failure: function (data) {
                console.log("failure");
            },
            error: function (data) {
                console.log("error");
            }
        });
    }, 2000);

    return false;
};

function fillPostComments(postComments) {
    console.log(postComments);

    let divPostComments = document.getElementById('divPostComments');
    
    let comments = "";

    let nrTotalPostComments = 0;

    let lineBtnReply = '';

    if (postComments.length > 0) {
        for (var i = 0; i < postComments.length; i++) {

            nrTotalPostComments++;

            lineBtnReply = '';

            if (postComments[i].idPostCommentParent == 0) {
                lineBtnReply = '<span  onclick="openPpostComment(' + postComments[i].idPostComment + ')"> <a href="javascript:;"><i class="fas fa-reply"></i> Reply</a></span>';
            }

            comments += '<li>';
            comments += '<div class="comment">';
            comments += '	<div class="img-thumbnail img-thumbnail-no-borders d-none d-sm-block">';
            comments += '		<img class="avatar" alt="" src="' + postComments[i].dsUrlAvatar + '" style="width:48px; heigth:48px" >';
            comments += '	</div>';
            comments += '	<div class="comment-block">';
            comments += '		<div class="comment-arrow"></div>';
            comments += '		<span class="comment-by">';
            comments += '			<strong>' + postComments[i].nmPessoa + ' (' + postComments[i].idPostComment + ')</strong>';
            comments += '			<span class="float-end">';
            comments += lineBtnReply;
            comments += '			</span>';
            comments += '		</span>';
            comments += '		<p>' + postComments[i].dsComment + '</p>';
            comments += '		<span class="date float-end">' + postComments[i].dtCommentTwo.dsDate + ' ' + postComments[i].dtCommentTwo.dsTime + '</span>';
            comments += '	</div>';
            comments += '</div>';

            console.log(postComments[i].postCommentsChildren.length);

            if (postComments[i].postCommentsChildren.length > 0) {

                console.log(postComments[i].postCommentsChildren.length);
                comments += '<ul class="comments reply">';

                for (var j = 0; j < postComments[i].postCommentsChildren.length; j++) {

                    nrTotalPostComments++;

                    console.log(postComments[i].postCommentsChildren[j].nmPessoa);
                    comments += '	<li>';
                    comments += '		<div class="comment">';
                    comments += '			<div class="img-thumbnail img-thumbnail-no-borders d-none d-sm-block">';
                    comments += '				<img class="avatar" alt="" src="' + postComments[i].dsUrlAvatar + '" style="width:48px; heigth:48px" >';
                    comments += '			</div>';
                    comments += '			<div class="comment-block">';
                    comments += '				<div class="comment-arrow"></div>';
                    comments += '				<span class="comment-by">';
                    comments += '			        <strong>' + postComments[i].postCommentsChildren[j].nmPessoa + ' (' + postComments[i].postCommentsChildren[j].idPostComment + ')</strong>';
                    comments += '				</span>';
                    comments += '				<p>' + postComments[i].postCommentsChildren[j].dsComment + '</p>';
                    comments += '				<span class="date float-end">' + postComments[i].postCommentsChildren[j].dtCommentTwo.dsDate + ' ' + postComments[i].postCommentsChildren[j].dtCommentTwo.dsTime + '</span>';
                    comments += '			</div>';
                    comments += '		</div>';
                    comments += '	</li>';
                }

                comments += '</ul>';

            }

            comments += '</li>';
        }

        if (postComments.length > 2) {
            document.getElementById('divComentarBottom').style.display = 'block';
        }
    }
    else {
        comments = "Nenhum comentário localizado. Seja o primeiro a comentar.";
    }

    document.getElementById('NRPostComments').innerHTML = nrTotalPostComments;

    divPostComments.innerHTML = comments;

};

function addPostComment() {

    if (!validatePostComment()) {
        return false;
    }

    $.ajax({
        url: '/Blog/AddPostComment',
        type: 'POST',
        data: {
            ipos: document.getElementById('IDPost').value,
            ipcp: document.getElementById('IDPostComment').value,
            cvis: getOrCreateUserId(),
            nvis: document.getElementById('NMPessoa').value,
            dema: document.getElementById('DSEmail').value,
            dcom: document.getElementById('DSComment').value,
            tken: document.getElementById('tknCaptcha').value,
        },
        success: function (data) {
            closePostComment();
            getPostComments(document.getElementById('IDPost').value);
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

function validatePostComment() {
    var bSuccess = true;

    let fieldNMPessoa = document.getElementById('NMPessoa');
    let fieldDSEmail = document.getElementById('DSEmail');
    let fieldDSComment = document.getElementById('DSComment');

    if (fieldNMPessoa.value.length == 0) {
        fieldNMPessoa.focus();
        showNotificationPostComment(fieldNMPessoa.getAttribute("data-msg-required"));
        bSuccess = false;
    }
    else if (fieldDSComment.value.length == 0) {
        fieldDSComment.focus();
        showNotificationPostComment(fieldDSComment.getAttribute("data-msg-required"));
        bSuccess = false;
    }

    return bSuccess;
}

function addPostCommentByAI(ipcp, idPessoaAI) {

    $.ajax({
        url: '/Blog/AddPostComment',
        type: 'GET',
        data: {
            ipos: document.getElementById('IDPost').value,
            ipcp: ipcp,
            cvis: getOrCreateUserId(),
            idai: idPessoaAI,
        },
        success: function (data) {
            fillPostComments(data.postComments);
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

function generateUUID() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

// Modal Comment

function openPpostComment(idPostComment) {

    document.getElementById('IDPostComment').value = idPostComment;

    // Exibe o modal
    const myModal = new bootstrap.Modal(document.getElementById('modalPostComment'), {
        keyboard: false
    });
    myModal.show();
}

function closePostComment() {

    document.getElementById('NMPessoa').value = "";
    document.getElementById('DSEmail').value = "";
    document.getElementById('DSComment').value = "";
    document.getElementById('IDPostComment').value = "0";

    const modalElement = document.getElementById('modalPostComment');
    const modalInstance = bootstrap.Modal.getInstance(modalElement); // Recupera a instância já ativa
    if (modalInstance) {
        modalInstance.hide();
    }
}


// Verifica se o identificador já está no localStorage
function getOrCreateUserId() {
    let userId = localStorage.getItem('user_id');

    if (!userId) {
        userId = generateUUID();
        localStorage.setItem('user_id', userId);
        console.log('Novo user_id gerado:', userId);
    } else {
        console.log('Usuário já identificado com user_id:', userId);
    }

    return userId;
}

function showNotificationPostComment(message) {
    const notification = document.getElementById('notificationPostComment');
    const messageContainer = document.getElementById('messageNotificationPostComment');

    messageContainer.innerHTML = message;
    notification.classList.add('show');

    // Garante que a transição ocorra (reexibe o elemento)
    setTimeout(() => {
        notification.style.display = 'block';
        notification.style.opacity = '1';
    }, 10);

    // Remove após 5 segundos com fade-out
    setTimeout(() => {
        notification.style.opacity = '0';

        // Após a transição de fade-out, oculta o elemento
        setTimeout(() => {
            notification.classList.remove('show');
            notification.style.display = 'none';
        }, 500); // Tempo deve ser igual ao do `transition` no CSS
    }, 5000);
}







