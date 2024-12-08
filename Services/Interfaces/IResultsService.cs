using Services.Dtos;
using Services.Enums;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IResultsService
    {

        [OperationContract]
        MatchResumeDTO GetMatchResume(string matchCode);

        [OperationContract]
        int UpdateResults(int idAccount, GameResult gameResult);
    }
}
