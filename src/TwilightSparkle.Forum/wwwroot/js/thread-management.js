var createThreadForm = document.querySelector('#create-thread-form');
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


preview.addEventListener('click', () => {
    output(parse(textarea.value));

    textarea.classList.toggle('hide');
    outputArea.classList.toggle('show');
    previewMessage.classList.toggle('show');
    preview.classList.toggle('active');
});

boldButton.addEventListener('click', () =>
    insertText(textarea, '****', 'demo', 2, 6)
);

italicButton.addEventListener('click', () =>
    insertText(textarea, '**', 'demo', 1, 5)
);

heading1Button.addEventListener('click', () =>
    insertText(textarea, '#', 'heading1', 1, 9)
);

heading2Button.addEventListener('click', () =>
    insertText(textarea, '##', 'heading2', 2, 10)
);

heading3Button.addEventListener('click', () =>
    insertText(textarea, '###', 'heading3', 3, 11)
);

linkButton.addEventListener('click', () =>
    insertText(textarea, '[](https://...)', 'url text', 1, 9)
);

imageLinkButton.addEventListener('click', () =>
    insertText(textarea, '![](https://...)', 'image title', 2, 13)
);

ulButton.addEventListener('click', function () {
    insertText(textarea, '* ', 'item', 2, 6);
});

olButton.addEventListener('click', () =>
    insertText(textarea, '1. ', 'item', 3, 7)
);

createThreadForm.addEventListener("submit", function (event) {
    event.preventDefault();

    let titleTextbox = createThreadForm.querySelector("#Title");
    let sectionNameInput = createThreadForm.querySelector("#SectionName");
    let data = new FormData();
    data.append('Title', titleTextbox.value);
    data.append('Content', parse(textarea.value));
    data.append('unparsedContent', textarea.value);
    data.append('SectionName', sectionNameInput.value);

    const urlObj = new URL(createThreadForm.action);

    sendRequest(urlObj.pathname, urlObj.search, "POST", data);
}, true);