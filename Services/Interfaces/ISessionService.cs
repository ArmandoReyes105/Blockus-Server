using Services.Dtos;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface ISessionService
    {
        [OperationContract]
        AccountDTO LogIn(string username, string password);

        [OperationContract]
        void LogOut(string username); 
    }
}
