using Services.Interfaces;
using System.ServiceModel;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IChatService
    {
        public void JoinToChat(string username)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IChatServiceCallback>();
            _chatController.JoinToChat(username, callback);
        }

        public void LeaveChat(string username)
        {
            _chatController.LeaveChat(username); 
        }

        public void SendMessage(string username, string matchCode, string message)
        {
            _chatController.SendMessage(username, matchCode, message); 
        }
    }
}
