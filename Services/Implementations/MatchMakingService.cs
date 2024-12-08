using Services.Dtos;
using Services.Interfaces;
using System.ServiceModel;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IMatchMakingService
    {
        public MatchDTO CreateMatch(PublicAccountDTO hostAccount)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMatchMakingServiceCallback>();
            return _matchMakingController.CreateMatch(hostAccount, callback);
        }

        public MatchDTO JoinToMatch(PublicAccountDTO account, string matchCode)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMatchMakingServiceCallback>();
            return _matchMakingController.JoinToMatch(account, matchCode, callback);
        }

        public void LeaveMatch(string username)
        {
            _matchMakingController.LeaveMatch(username);
        }
    }
}
