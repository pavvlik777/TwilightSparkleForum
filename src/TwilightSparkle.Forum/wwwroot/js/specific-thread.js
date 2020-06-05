var threadDeleteButton = document.getElementById("thread-delete-button");

threadDeleteButton.addEventListener("click", function (event) {
    event.preventDefault();

    const threadId = threadDeleteButton.getAttribute("thread-id");
    sendRequest("/Threads/DeleteThread", "?threadId=" + threadId, "POST");
})