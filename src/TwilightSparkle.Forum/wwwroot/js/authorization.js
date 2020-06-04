var passwordInfo = document.getElementById("password-info");
var passwordInput = document.getElementById("Password");

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

passwordInput.addEventListener("keyup", function () {
    let password = this.value;
    validatePassword(password);
});
validatePassword(passwordInput.value);

var signUpForm = document.getElementById("sign-up-form");

signUpForm.addEventListener("submit", function (event) {
    event.preventDefault();

    let usernameInput = signUpForm.querySelector("#Username");
    let passwordInput = signUpForm.querySelector("#Password");
    let passwordConfirmationInput = signUpForm.querySelector("#PasswordConfirmation");
    let emailInput = signUpForm.querySelector("#Email");
    let data = new FormData();
    data.append('Username', usernameInput.value);
    data.append('Password', passwordInput.value);
    data.append('PasswordConfirmation', passwordConfirmationInput.value);
    data.append('Email', emailInput.value);

    const urlObj = new URL(signUpForm.action);
    sendRequest(urlObj.pathname, urlObj.search, "POST", data);
});