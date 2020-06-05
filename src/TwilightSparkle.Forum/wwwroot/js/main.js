function showErrorMessage(message) {
    showMessage(message, "error");
}

function showSuccessMessage(message) {
    showMessage(message, "success");
}

function showMessage(message, type) {
    let siteMessages = document.getElementById("site-messages");
    let errorMessage = document.createElement("div");
    errorMessage.classList.add("alert", type);

    let closeBtn = document.createElement("span");
    closeBtn.id = "close-btn";
    closeBtn.innerHTML = "&times;";
    closeBtn.onclick = function () {
        let div = this.parentElement;
        setTimeout(function () {
            div.parentElement.removeChild(div);
        }, 600);
    }

    let errorText = document.createElement("p");
    errorText.innerHTML = message;

    errorMessage.appendChild(closeBtn);
    errorMessage.appendChild(errorText);

    siteMessages.appendChild(errorMessage);

    window.setTimeout(function () {
        errorMessage.parentElement.removeChild(errorMessage);
    }, 6000);
}

function toggleLoadingGif() {
    let loading = document.getElementById("loading");
    if (!loading) {
        return;
    }
    if (loading.classList.contains("hide")) {
        loading.classList.remove("hide");
    } else {
        loading.classList.add("hide");
    }
}

var signOutForm = document.getElementById("sign-out-form");

if (signOutForm) {
    signOutForm.addEventListener("submit", function (event) {
        event.preventDefault();

        const urlObj = new URL(signOutForm.action);
        sendRequest(urlObj.pathname, urlObj.search, "POST");
    });
}