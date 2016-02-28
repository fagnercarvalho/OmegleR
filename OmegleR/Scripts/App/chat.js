$(function () {
    var chat = $.connection.chatHub;

    var youLabel = '<span style="color:#0000FF"><b>You: </b></span>';
    var strangerLabel = '<span style="color:#FF0000"><b>Stranger: </b></span>';

    // SignalR Hub methods handlers
    chat.client.receiveMessage = function (message) {
        appendMessage(strangerLabel + $('<div/>').text(message).html());
        scrollToBottom();
    }

    chat.client.receiveWelcomeMessage = function (message) {
        appendMessage(message);
        enableElements();
    }

    chat.client.userDisconnected = function (message) {
        disconnect(message);
    }

    // Connecting to SignalR Hub 
    $.connection.hub.start().done(function () {
        // Buttons event handlers
        $('#btn-send-message').on('click', function () {
            var message = $('#text-message').val();
            sendMessage(message);
        });

        $(document).keyup(function (e) {
            if (e.keyCode == 13) {
                // Enter handler
                e.preventDefault();
                var message = $('#text-message').val();
                sendMessage(message);
            } else if (e.keyCode == 27) {
                // Esc handler
                stopChat();
            }
        });

        $('#btn-stop-chat').on('click', function () {
            stopChat();
        });
    });

    // Append message to chat panel
    function appendMessage(message) {
        var chatDiv = $('.chat');
        chatDiv.append(message);
        chatDiv.append('<br/>');
    }

    // Send message through the SignalR Hub to the user you talking to
    function sendMessage(message) {
        if (!message) {
            return;
        }

        chat.server.send(message);
        $('#text-message').val('').focus();

        appendMessage(youLabel + $('<div/>').text(message).html());
        scrollToBottom();
    }

    // Send request to stop chat through the SignalR Hub
    function stopChat() {
        chat.server.stop();
        disconnect('You have disconnected');
    }

    // Event handler for when the other party leaves the chat
    function disconnect(message) {
        appendMessage("<b>" + message + "</b>");
        appendMessage('<a class="btn btn-primary" onclick="location.reload();">New chat</a>');
        disableElements();
        removeCloseConfirmation();
    }

    // Scroll to the bottom of the chat panel
    function scrollToBottom() {
        var element = document.getElementsByClassName('chat')[0];
        element.scrollTop = element.scrollHeight;
    }

    // Disable buttons and chat input area
    function disableElements() {
        $('#text-message').attr('disabled', 'disabled');
        $('#btn-send-message').attr('disabled', 'disabled');
        $('#btn-stop-chat').attr('disabled', 'disabled');
    }

    // Enable buttons and chat input area
    function enableElements() {
        $('#text-message').removeAttr('disabled');
        $('#btn-send-message').removeAttr('disabled');
        $('#btn-stop-chat').removeAttr('disabled');
    }

    // Add confirmation dialog to appear 
    // in case the user tries to close the window
    function addCloseConfirmation() {
        removeCloseConfirmation();

        $(window).on("beforeunload", function (e) {
            var confirmationMessage = 'You are in the middle of something.';

            (e || window.event).returnValue = confirmationMessage;
            return confirmationMessage;
        });
    }

    // Remove confirmation dialog that was going to appear
    // in case the user tries to close the window
    function removeCloseConfirmation() {
        $(window).off("beforeunload");
    }

    appendMessage("Waiting for user...");
    disableElements();
    addCloseConfirmation();
});