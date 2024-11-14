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
        int UpdateAccount(AccountDTO accountDTO);
        [OperationContract]
        ResultsDTO GetAccountResults(int idAccount);
        [OperationContract]
        ProfileConfigurationDTO GetProfileConfiguration(int idAccount);
    }
}
