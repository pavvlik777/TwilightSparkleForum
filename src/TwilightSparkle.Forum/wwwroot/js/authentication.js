var signInForm = document.getElementById("sign-in-form");

signInForm.addEventListener("submit", function (event) {
    event.preventDefault();

    let usernameInput = signInForm.querySelector("#Username");
    let passwordInput = signInForm.querySelector("#Password");
    let data = new FormData();
    data.append('Username', usernameInput.value);
    data.append('Password', passwordInput.value);

    const urlObj = new URL(signInForm.action);
    sendRequest(urlObj.pathname, urlObj.search, "POST", data);
});