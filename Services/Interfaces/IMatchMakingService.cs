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
        MatchDTO JoinToMatch(PublicAccountDTO account, string matchCode);

        [OperationContract(IsOneWay = true)]
        void LeaveMatch(string username);
    }

    [ServiceContract]
    public interface IMatchMakingServiceCallback
    {
        [OperationContract]
        void NotifyPlayerEntry(MatchDTO matchDTO);

        [OperationContract]
        void NotifyPlayerExit(MatchDTO matchDTO);

        [OperationContract]
        void NotifyHostExit(MatchDTO matchDTO);
    }
}
