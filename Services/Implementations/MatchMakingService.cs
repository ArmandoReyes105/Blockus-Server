using Services.Controllers;
using Services.Dtos;
using Services.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IMatchMakingService
    {
        private static readonly Dictionary<string, IMatchMakingServiceCallback> usersInMatchMaking = new Dictionary<string, IMatchMakingServiceCallback> ();
        private static readonly Dictionary<string, MatchDTO> matches = new Dictionary<string, MatchDTO>();
        private readonly MatchMakingController _matchMakingController;

        public ServiceImplementation()
        {
            _matchMakingController = new MatchMakingController(usersInMatchMaking, matches);
            _matchController = new MatchController(usersInMatchMaking, usersInActiveMatch, matches, activeMatches); 
        }

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
