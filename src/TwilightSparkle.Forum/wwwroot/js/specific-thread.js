var threadDeleteButton = document.getElementById("thread-delete-button");

if (threadDeleteButton) {
    threadDeleteButton.onclick = function (event) {
        event.preventDefault();

        const threadId = threadDeleteButton.getAttribute("thread-id");
        sendRequest("/Threads/DeleteThread", "?threadId=" + threadId, "POST");
    };
}

function registerButtons() {
    likeButton.onclick = function (event) {
        event.preventDefault();

        const threadId = likeButton.getAttribute("thread-id");
        sendRequest("/Threads/LikeThread", "?threadId=" + threadId + "&isLike=true", "POST");
    };
    dislikeButton.onclick = function (event) {
        event.preventDefault();

        const threadId = dislikeButton.getAttribute("thread-id");
        sendRequest("/Threads/LikeThread", "?threadId=" + threadId + "&isLike=false", "POST");
    };
}

var likeButton = document.getElementById("like-thread");
var dislikeButton = document.getElementById("dislike-thread");

if (likeButton) {
    registerButtons();
}


var commentThreadForm = document.getElementById("comment-thread-form");
var threadParsedContent = document.querySelector('#parsed-content');

var textarea = document.querySelector('#input-area');
var outputArea = document.querySelector('#output-area');
var previewMessage = document.querySelector('.preview-message');

// -------------------------------------------
// TOOLBAR
// -------------------------------------------
var preview = document.querySelector('#preview');
var boldButton = document.querySelector('#bold');
var italicButton = document.querySelector('#italic');
var heading1Button = document.querySelector('#heading1');
var heading2Button = document.querySelector('#heading2');
var heading3Button = document.querySelector('#heading3');
var linkButton = document.querySelector('#link');
var imageLinkButton = document.querySelector('#image-link');
var ulButton = document.querySelector('#list-ul');
var olButton = document.querySelector('#list-ol');

if (commentThreadForm) {
    preview.onclick = function () {
        output(parse(textarea.value));

        textarea.classList.toggle('hide');
        outputArea.classList.toggle('show');
        previewMessage.classList.toggle('show');
        preview.classList.toggle('active');
    };

    boldButton.onclick = function () {
        insertText(textarea, '****', 'demo', 2, 6)
    };

    italicButton.onclick = function () {
        insertText(textarea, '**', 'demo', 1, 5)
    };

    heading1Button.onclick = function () {
        insertText(textarea, '#', 'heading1', 1, 9)
    };

    heading2Button.onclick = function () {
        insertText(textarea, '##', 'heading2', 2, 10)
    };

    heading3Button.onclick = function () {
        insertText(textarea, '###', 'heading3', 3, 11)
    };

    linkButton.onclick = function () {
        insertText(textarea, '[](https://...)', 'url text', 1, 9)
    };

    imageLinkButton.onclick = function () {
        insertText(textarea, '![](https://...)', 'image title', 2, 13)
    };

    ulButton.onclick = function () {
        insertText(textarea, '* ', 'item', 2, 6);
    };

    olButton.onclick = function () {
        insertText(textarea, '1. ', 'item', 3, 7)
    };

    commentThreadForm.onsubmit = function (event) {
        event.preventDefault();

        const threadId = commentThreadForm.getAttribute("thread-id");
        let data = new FormData();
        data.append('threadId', threadId);
        data.append('content', parse(textarea.value));
        data.append('unparsedContent', textarea.value);

        const urlObj = new URL(commentThreadForm.action);

        sendRequest(urlObj.pathname, urlObj.search, "POST", data);
    };
}