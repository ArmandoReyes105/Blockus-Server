using Data.Model;
using Services.Dtos;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        int CreateAccount(AccountDTO accountDTO);
        [OperationContract]
        Account Login(string username, string password);
    }
}
