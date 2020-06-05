var createThreadButton = document.getElementById("create-thread-button");

if (createThreadButton) {
    createThreadButton.onclick = function () {
        const sectionName = createThreadButton.getAttribute("data-id");
        urlClickHandler("/Threads/CreateThread?sectionName=" + sectionName);
    }
}