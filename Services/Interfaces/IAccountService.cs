using Data.Model;
using Services.Dtos;
using Services.Enums;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        int CreateAccount(AccountDTO accountDTO);
    }
}
