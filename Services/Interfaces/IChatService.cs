using System.ServiceModel;

namespace Services.Interfaces
{
    /// <summary>
    /// Defines the operations for a chat service used within the application.
    /// This service allows to join a chat, send messages, and leave the chat.
    /// </summary>
    [ServiceContract (CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        /// <summary>
        /// Allows a user to join the chat
        /// </summary>
        /// <param name="username">The username of the user joining the chat</param>
        [OperationContract]
        void JoinToChat(string username);

        /// <summary>
        /// Sends a message from a user to a specific match chat
        /// </summary>
        /// <param name="username">The username of the user sendind the message</param>
        /// <param name="matchCode">The unique identifier for the match chat</param>
        /// <param name="message">The message content to send</param>
        [OperationContract]
        void SendMessage(string username, string matchCode, string message);

        /// <summary>
        /// Allows a user to leave the chat.
        /// </summary>
        /// <param name="username">The username of the user leaving the chat</param>
        [OperationContract]
        void LeaveChat(string username);
    }

    /// <summary>
    /// Defines the callback operations for the chat service.
    /// This allows the service to notify clients of new messages.
    /// </summary>
    [ServiceContract]
    public interface IChatServiceCallback
    {
        /// <summary>
        /// Notifies the client of a new message received in the chat
        /// </summary>
        /// <param name="username">The username of the user who sent the message</param>
        /// <param name="message">The content of the received message</param>
        [OperationContract]
        void OnReciveMessage(string username, string message);
    }
}
