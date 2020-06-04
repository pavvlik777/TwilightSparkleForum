function showErrorMessage(message) {
    let siteMessages = document.getElementById("site-messages");
    let errorMessage = document.createElement("div");
    errorMessage.classList.add("alert", "error");

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

function showSuccessMessage(message) {
    let siteMessages = document.getElementById("site-messages");
    let errorMessage = document.createElement("div");
    errorMessage.classList.add("alert", "success");

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

var signOutForm = document.getElementById("sign-out-form");

if (signOutForm) {
    signOutForm.addEventListener("submit", function (event) {
        event.preventDefault();

        const urlObj = new URL(signOutForm.action);
        sendRequest(urlObj.pathname, urlObj.search, "POST");
    });
}