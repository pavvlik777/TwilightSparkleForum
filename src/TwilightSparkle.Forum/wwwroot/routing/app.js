function defaultSuccessCallback(responseText) {
    document.title = this.title + " - Twilight Sparkle Forum";
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
    registerLinks();
}

let getRoutes = {
    '/': {
        "apiRoute": "/api/Home/Index",
        "title": "Index",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/main.js"
        ]
    },
    '/Home/Index': {
        "apiRoute": "/api/Home/Index",
        "title": "Index",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/main.js"
        ]
    },
    '/Home/Profile': {
        "apiRoute": "/api/Home/Profile",
        "title": "Profile",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/profile.js",
            "/js/main.js"
        ]
    },
    '/Authentication/SignIn': {
        "apiRoute": "/api/Authentication/SignIn",
        "title": "Sign In",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/authentication.js",
            "/js/main.js"
        ]
    },
    '/Authentication/SignUp': {
        "apiRoute": "/api/Authentication/SignUp",
        "title": "Sign Up",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/authorization.js",
            "/js/main.js"
        ]
    },
    '/Threads/SectionThreads': {
        "apiRoute": "/api/Threads/SectionThreads",
        "title": "Section threads",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/section-threads.js",
            "/js/main.js"
        ]
    },
    '/Threads/ThreadsDetails': {
        "apiRoute": "/api/Threads/ThreadsDetails",
        "title": "Thread details",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/main.js"
        ]
    },
    '/Threads/CreateThread': {
        "apiRoute": "/api/Threads/CreateThread",
        "title": "Create thread",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/thread-management.js",
            "/markdown/markdown.js",
            "/js/main.js"
        ]
    },


    "/NotFoundError": {
        "apiRoute": "/api/Error/NotFound",
        "title": "Not found",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/main.js"
        ]
    },
    '/InternalError': {
        "apiRoute": "/api/Error/InternalError",
        "title": "Internal error",
        "successCallback": defaultSuccessCallback,
        "errorCallback": handleError,
        "jsFiles": [
            "/js/main.js"
        ]
    }
}

let postRoutes = {
    '/Authentication/SignIn': {
        "apiRoute": "/api/Authentication/SignIn",
        "title": "Sign In",
        "successCallback": function (responseText) {
            reloadBody("/");
        },
        "errorCallback": function (statusCode, responseText) {
            if (statusCode === 401) {
                showErrorMessage(responseText);
            }
            else {
                showErrorMessage("Unknown error");
            }
        },
        "jsFiles": [
        ]
    },
    '/Authentication/SignUp': {
        "apiRoute": "/api/Authentication/SignUp",
        "title": "Sign In",
        "successCallback": function (responseText) {
            reloadBody("/Authentication/SignIn");
        },
        "errorCallback": function (statusCode, responseText) {
            if (statusCode === 401) {
                showErrorMessage(responseText);
            }
            else {
                showErrorMessage("Unknown error");
            }
        },
        "jsFiles": [
        ]
    },
    '/Home/SaveChanges': {
        "apiRoute": "/api/Home/SaveChanges",
        "title": "Save Changes",
        "successCallback": function (responseText) {
            reloadBody("/Home/Profile", function () { showSuccessMessage("Success"); });
        },
        "errorCallback": function (statusCode, responseText) {
            if (statusCode === 400) {
                showErrorMessage(responseText);
            }
            else {
                showErrorMessage("Unknown error");
            }
        },
        "jsFiles": [
        ]
    },
    "/Authentication/SignOut": {
        "apiRoute": "/api/Authentication/SignOut",
        "title": "Sign Out",
        "successCallback": function (responseText) {
            reloadBody("/");
        },
        "errorCallback": handleError,
        "jsFiles": [
        ]
    },
    "/Threads/CreateThread": {
        "apiRoute": "/api/Threads/CreateThread",
        "title": "Create thread",
        "successCallback": function (responseText) {
            reloadBody("/");
        },
        "errorCallback": function (statusCode, responseText) {
            if (statusCode === 400) {
                showErrorMessage(responseText);
            }
            else {
                showErrorMessage("Unknown error");
            }
        },
        "jsFiles": [
        ]
    }
}


function reloadBody(url, callback = null) {
    let xhr = new XMLHttpRequest();
    xhr.open("GET", "/api/Home/App", true);
    xhr.onreadystatechange = function () {
        if (this.readyState !== 4) {
            return;
        }
        if (this.status !== 200) {
            handleError(this.status);

            return;
        }

        let body = document.getElementsByTagName("body")[0];
        if (body) {
            body.outerHTML = this.responseText;
        }

        urlClickHandler(url, callback);
    };
    xhr.send();
}

function handleError(statusCode) {
    if (statusCode === 404) {
        sendRequest("/NotFoundError", "", "GET");
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

    script.onload = script.onreadystatechange = function (_, isAbort) {
        if (isAbort || !script.readyState || /loaded|complete/.test(script.readyState)) {
            script.onload = script.onreadystatechange = null;
            script = undefined;
        }
    };

    script.src = source;
    mainContent.appendChild(script);
}

function sendRequest(pathname, search, method, formData = null, callback = null) {
    const targetRoute = method === "GET" ? getRoutes[pathname] : postRoutes[pathname];
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
            targetRoute.errorCallback(this.status, this.responseText);

            return;
        }

        targetRoute.successCallback(this.responseText);
        if (callback) {
            callback();
        }
    };
    if (!formData) {
        xhr.send();
    }
    else {
        xhr.send(formData);
    }
}


window.onpopstate = () => {
    sendRequest(window.location.pathname, window.location.search, 'GET');
}

function urlClickHandler(url, callback) {
    const urlObj = new URL(window.location.origin + url);

    window.history.pushState(null, url, window.location.origin + url);
    toggleLoadingGif();
    sendRequest(urlObj.pathname, urlObj.search, 'GET', null, callback);
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