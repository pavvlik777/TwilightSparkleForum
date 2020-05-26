var passwordInfo = document.getElementById("password-info");
var passwordInput = document.getElementById("password-input");

function validatePassword(password) {
    let length = passwordInfo.querySelector("#length");
    let letter = passwordInfo.querySelector("#letter");
    let capital = passwordInfo.querySelector("#capital");
    let number = passwordInfo.querySelector("#number");

    if (password.length < 8) {
        length.classList.remove("valid");
        length.classList.add("invalid");
    }
    else {
        length.classList.remove("invalid");
        length.classList.add("valid");
    }

    if (password.match(/[a-z]/)) {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
    }
    else {
        letter.classList.remove("valid");
        letter.classList.add("invalid");
    }

    if (password.match(/[A-Z]/)) {
        capital.classList.remove("invalid");
        capital.classList.add("valid");
    }
    else {
        capital.classList.remove("valid");
        capital.classList.add("invalid");
    }

    if (password.match(/(?=.*\d)/)) {
        number.classList.remove("invalid");
        number.classList.add("valid");
    }
    else {
        number.classList.remove("valid");
        number.classList.add("invalid");
    }
}

document.addEventListener("DOMContentLoaded", function () {
    passwordInput.addEventListener("keyup", function () {
        let password = this.value;
        validatePassword(password);
    });
    validatePassword(passwordInput.value);
});