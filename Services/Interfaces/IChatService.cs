using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract (CallbackContract = typeof(IChatServiceCallback))]
    public interface IChatService
    {
        [OperationContract]
        void JoinToChat(string username);

        [OperationContract]
        void SendMessage(string username, string matchCode, string message);

        [OperationContract]
        void LeaveChat(string username);
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract]
        void OnReciveMessage(string username, string message);
    }
}
