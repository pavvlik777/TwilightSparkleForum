function openTab(event, tabName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tab-panel");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].classList.add("hidden");
    }
    tablist = document.getElementsByClassName("tab-list")[0];
    tablinks = tablist.getElementsByTagName("li");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabName).classList.remove("hidden");;
    event.currentTarget.className += " active";
}

function toggleElement(element) {
    if (element.style.display === "none") {
        element.style.display = "block";
    } else {
        element.style.display = "none";
    }
}


document.getElementById("identity-list-item").onclick = function () {
    openTab(event, 'identity-tab');
}
document.getElementById("threads-list-item").onclick = function () {
    openTab(event, 'threads-tab');
}

var imageUploadModal = document.getElementById("upload-image-modal");
var saveChangesButton = document.getElementById("save-changes-button");
var imageUploadFile = document.getElementById("image-upload-file");
var imageUploadButton = document.getElementById("image-upload-button");
imageUploadButton.onclick = function () {
    var imageData = new FormData();
    if (imageUploadFile.files.length == 0) {
        return;
    }
    imageData.append('image', imageUploadFile.files[0]);

    var xhr = new XMLHttpRequest();
    xhr.responseType = 'json';
    xhr.addEventListener("readystatechange", function () {
        if (this.readyState === this.DONE) {
            if (this.status == 200) {
                var profileImage = document.getElementById("profile-image");
                profileImage.src = this.response.url;
                var uploadImageExternalId = document.getElementById("hidden-upload-image-url");
                uploadImageExternalId.value = this.response.externalId;
                saveChangesButton.disabled = false;
                imageUploadModal.style.display = "none";
            }
            else {
                switch (this.response.errorCode) {
                    case "incorrect_input_file_path":
                        showErrorMessage("Incorrect image filepath");
                        break;
                    case "too_big_image":
                        showErrorMessage("Too big image. Max - 10 Mb");
                        break;
                    case "incorrect_image_type":
                        showErrorMessage("Incorrect image type");
                        break;
                    case "invalid_image":
                        showErrorMessage("Invalid image");
                        break;
                    default:
                        showErrorMessage("Unknown error");
                        break;
                }
            }
        }
    });
    var uploadImageUrl = imageUploadButton.getAttribute("data-request-url");
    xhr.open("POST", uploadImageUrl);

    xhr.send(imageData);
}

var uploadImageDialog = document.getElementById("upload-image-modal");
document.getElementById("image-upload-dialog-button").onclick = function () {
    toggleElement(uploadImageDialog);
}
var openImageUploadBtn = document.getElementById("image-upload-dialog-button");
var closeImageUploadBtn = document.getElementById("close-upload-dialog");
openImageUploadBtn.onclick = function () {
    imageUploadModal.style.display = "block";
}
closeImageUploadBtn.onclick = function () {
    imageUploadModal.style.display = "none";
}
window.onclick = function (event) {
    if (event.target == imageUploadModal) {
        imageUploadModal.style.display = "none";
    }
}


document.getElementById("default-tab").click();