using Services.Dtos;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract (CallbackContract = typeof(IMatchMakingServiceCallback))]
    public interface IMatchMakingService
    {
        [OperationContract]
        MatchDTO CreateMatch(PublicAccountDTO hostAccount);

        [OperationContract]
        bool JoinToMatch(PublicAccountDTO account, string matchCode); 
    }

    [ServiceContract]
    public interface IMatchMakingServiceCallback
    {

    }
}
