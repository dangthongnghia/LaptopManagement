document.addEventListener('DOMContentLoaded', function() {
    // Log when the DOM is ready
    console.log('DOM fully loaded, initializing chat...');
    
    const chatWidget = document.getElementById('chat-widget');
    const chatHeader = document.getElementById('chat-header');
    const chatBody = document.getElementById('chat-body');
    const chatInput = document.getElementById('chat-input');
    const chatToggle = document.getElementById('chat-toggle');
    const messageInput = document.getElementById('message-input');
    const sendButton = document.getElementById('send-button');
    
    // Check if all required elements exist
    if (!chatWidget || !chatBody || !chatInput || !messageInput || !sendButton) {
        console.error('Some chat elements were not found in the DOM');
        return; // Exit early if elements don't exist
    }
    
    // Initialize chat state
    let isChatOpen = false;
    
    // Initialize SignalR connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .withAutomaticReconnect()
        .build();
        
    // Start the connection
    connection.start().then(function () {
        console.log("SignalR Connected!");
        
        // Add welcome message
        addMessage('Hello! How can I help you today?', false);
    }).catch(function (err) {
        console.error(err.toString());
        addMessage('Unable to connect to chat service. Please try again later.', false);
    });
    
    // Handle receiving messages
    connection.on("ReceiveMessage", function (user, message) {
        // Only show admin messages to the user
        if (user === "Admin" || user === "System") {
            addMessage(message, false);
        }
    });

    
    
    // Toggle chat open/close
    function toggleChat() {
        isChatOpen = !isChatOpen;
        
        if (isChatOpen) {
            chatWidget.classList.remove('collapsed');
            chatBody.classList.remove('hidden');
            chatInput.classList.remove('hidden');
            if (chatToggle) chatToggle.innerHTML = '−';
            messageInput.focus();
        } else {
            chatWidget.classList.add('collapsed');
            chatBody.classList.add('hidden');
            chatInput.classList.add('hidden');
            if (chatToggle) chatToggle.innerHTML = '+';
        }
    }
    
    // Add event listeners
    if (chatHeader) {
        chatHeader.addEventListener('click', toggleChat);
    } else {
        console.warn('Chat header element not found');
    }
    
    // Send message function
    function sendMessage() {
        const message = messageInput.value.trim();
        if (!message) return;
        
        // Add user message to chat
        addMessage(message, true);
        messageInput.value = '';
        
        // Send to SignalR hub
        connection.invoke("SendMessage", "User", message).catch(function (err) {
            console.error(err.toString());
            addMessage('Error sending message. Please try again.', false);
        });
    }
    
    // Add message to chat
    function addMessage(text, isUser) {
        const messageDiv = document.createElement('div');
        messageDiv.classList.add('chat-message');
        messageDiv.classList.add(isUser ? 'user-message' : 'bot-message');
        messageDiv.textContent = text;
        chatBody.appendChild(messageDiv);
        chatBody.scrollTop = chatBody.scrollHeight;
    }
    
    // Add event listeners for sending messages
    if (sendButton) {
        sendButton.addEventListener('click', sendMessage);
    } else {
        console.error('sendButton element not found');
    }
    
    if (messageInput) {
        messageInput.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') {
                sendMessage();
            }
        });
    } else {
        console.error('messageInput element not found');
    }
});