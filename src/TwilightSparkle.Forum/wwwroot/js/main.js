var siteMessages = document.getElementById("site-messages");

function showErrorMessage(message) {
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