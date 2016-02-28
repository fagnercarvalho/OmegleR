namespace OmegleR.Hubs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;

    using OmegleR.Models;

    /// <summary>
    ///     SignalR Hub responsible for handling chat events.
    /// </summary>
    public class ChatHub : Hub
    {
        private static readonly object MyLock = new object();
        private static readonly List<OneOnOne> Chats;

        /// <summary>
        ///     Initializes static members of the <see cref="ChatHub"/> class.
        /// </summary>
        static ChatHub()
        {
            Chats = new List<OneOnOne>();
        }

        /// <summary>
        ///     Send a message to the user you are talking to.
        /// </summary>
        /// <param name="message">Message to the user.</param>
        public void Send(string message)
        {
            var user = this.Context.ConnectionId;
            var chat = GetChatByUser(user);
            if (chat == null)
            {
                return;
            }

            if (chat.User1 == user)
            {
                Clients.Client(chat.User2).receiveMessage(message);
            }
            else
            {
                Clients.Client(chat.User1).receiveMessage(message);
            }
        }

        /// <summary>
        ///     Stop the current chat.
        /// </summary>
        public void Stop()
        {
            this.Disconnect();
        }

        /// <summary>
        ///     Handle new user connection.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/>.</returns>
        public override Task OnConnected()
        {
            lock (MyLock)
            {
                var openSlotChat = Chats.FirstOrDefault(c => string.IsNullOrEmpty(c.User2));
                if (openSlotChat == null)
                {
                    Chats.Add(new OneOnOne { User1 = this.Context.ConnectionId });
                }
                else
                {
                    openSlotChat.User2 = this.Context.ConnectionId;

                    const string WelcomeMessage = "You are now chatting with a stranger. Say hello!";
                    Clients.Client(openSlotChat.User1).receiveWelcomeMessage(WelcomeMessage);
                    Clients.Client(openSlotChat.User2).receiveWelcomeMessage(WelcomeMessage);
                }
            }

            return base.OnConnected();
        }

        /// <summary>
        ///     Handle user disconnection.
        /// </summary>
        /// <param name="stopCalled">true if this was called by the client.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/>.</returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            this.Disconnect();

            return base.OnDisconnected(stopCalled);
        }

        private static OneOnOne GetChatByUser(string user)
        {
            return Chats.FirstOrDefault(c => c.User1 == user || c.User2 == user);
        }

        private void Disconnect()
        {
            var user = this.Context.ConnectionId;
            var chat = GetChatByUser(user);

            // this condition is true when the second (and last)
            // user decides to disconnect from the chat
            // (the chat item was deleted after the first user disconnected)
            if (chat == null)
            {
                return;
            }

            Chats.Remove(chat);

            // this condition is true when an user
            // disconnected before chatting with someone
            if (chat.User2 == null)
            {
                return;
            }

            const string DisconnectedMessage = "Stranger has disconnected";
            if (chat.User1 == user)
            {
                Clients.Client(chat.User2).userDisconnected(DisconnectedMessage);
            }
            else
            {
                Clients.Client(chat.User1).userDisconnected(DisconnectedMessage);
            }
        }
    }
}