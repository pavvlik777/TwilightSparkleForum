let getRoutes = {
    '/': {
        "apiRoute": "/api/Home/Index",
        "title": "Index",
        "successCallback": function (responseText) {
            let sidebarContent = document.getElementById('sidebar-content');
            if (sidebarContent) {
                sidebarContent.remove();
            }
            let mainContent = document.getElementById('main-content');
            if (mainContent) {
                mainContent.outerHTML = responseText;
            }

            for (let i = 0; i < this.jsFiles.length; i++) {
                getScript(this.jsFiles[i]);
            }
        },
        "jsFiles": [
            "/js/main.js"
        ]
    },
    '/Authentication/SignIn': {
        "apiRoute": "/api/Authentication/SignIn",
        "title": "Sign In",
        "successCallback": function (responseText) {
            let sidebarContent = document.getElementById('sidebar-content');
            if (sidebarContent) {
                sidebarContent.remove();
            }
            let mainContent = document.getElementById('main-content');
            if (mainContent) {
                mainContent.outerHTML = responseText;
            }

            for (let i = 0; i < this.jsFiles.length; i++) {
                getScript(this.jsFiles[i]);
            }
        },
        "jsFiles": [
            "/js/main.js"
        ]
    },
    '/Authentication/SignUp': {
        "apiRoute": "/api/Authentication/SignUp",
        "title": "Sign Up",
        "successCallback": function (responseText) {
            let sidebarContent = document.getElementById('sidebar-content');
            if (sidebarContent) {
                sidebarContent.remove();
            }
            let mainContent = document.getElementById('main-content');
            if (mainContent) {
                mainContent.outerHTML = responseText;
            }

            for (let i = 0; i < this.jsFiles.length; i++) {
                getScript(this.jsFiles[i]);
            }
        },
        "jsFiles": [
            "/js/main.js",
            "/js/authentication.js"
        ]
    },


    "/NotFound": {

    },
    '/InternalError': {

    }
}


function handleError(statusCode) {
    if (statusCode === 404) {
        sendRequest("/NotFound", "", "GET");
    }
    else if (statusCode === 401) {
        onUrlClick("/Authentication/SignIn");
    }
    else {
        sendRequest("/InternalError", "", "GET");
    }
}

function getScript(source) {
    let script = document.createElement('script');
    let mainContent = document.getElementById('main-content');
    script.async = 1;

    script.onload = script.onreadystatechange = function (_, isAbort) {
        if (isAbort || !script.readyState || /loaded|complete/.test(script.readyState)) {
            script.onload = script.onreadystatechange = null;
            script = undefined;
        }
    };

    script.src = source;
    mainContent.appendChild(script);
}

function sendRequest(pathname, search, method) {
    const targetRoute = method === "GET" ? getRoutes[pathname] : null;
    if (!targetRoute) {
        handleError(404);

        return;
    }

    if (!search) {
        search = "";
    }
    const requestFullRoute = targetRoute.apiRoute + search;

    let xhr = new XMLHttpRequest();
    xhr.open(method, requestFullRoute, true);
    xhr.onreadystatechange = function () {
        if (this.readyState !== 4) {
            return;
        }
        if (this.status !== 200) {
            handleError(this.status);

            return;
        }

        document.title = targetRoute.title + " - Twilight Sparkle Forum";
        targetRoute.successCallback(this.responseText);
    };
    xhr.send();
}


window.onpopstate = () => {
    sendRequest(window.location.pathname, window.location.search, 'GET');
}

function urlClickHandler(url) {
    const urlObj = new URL(window.location.origin + url);

    window.history.pushState(null, url, window.location.origin + url);
    sendRequest(urlObj.pathname, urlObj.search, 'GET');
}

let registerLinks = () => {
    let links = document.getElementsByClassName('forum-link');
    for (let i = 0; i < links.length; i++) {
        let element = links[i];
        element.onclick = function (event) {
            event.preventDefault();
            let url = this.getAttribute('href');
            urlClickHandler(url);
        }
    }
}

sendRequest(window.location.pathname, window.location.search, 'GET');
registerLinks();