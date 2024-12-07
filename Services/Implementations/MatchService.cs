using Services.Controllers;
using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using Services.MatchState;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services.Implementations
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServiceImplementation : IMatchService
    {
        private static readonly Dictionary<string, IMatchServiceCallback> usersInActiveMatch = new Dictionary<string, IMatchServiceCallback>();
        private static readonly Dictionary<string, ActiveMatch> activeMatches = new Dictionary<string, ActiveMatch>();
        private readonly MatchController _matchController;

        public MatchDTO JoinToActiveMatch(string username)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IMatchServiceCallback>();
            return _matchController.JoinToActiveMatch(username, callback); 
        }

        public BlockDTO GetCurrentBlock(string matchCode)
        {
            return _matchController.GetCurrentBlock(matchCode);
        }

        public GameResult PlaceBlock(string matchCode, int points)
        {
            return _matchController.PlaceBlock(matchCode, points);
        }

        public void MakeMovement(string matchCode, Movement movement)
        {
            _matchController.MakeMovement(matchCode, movement);
        }

        public GameResult SkipTurn(string matchCode)
        {
            return _matchController.SkipTurn(matchCode); 
        }

        public void LeaveActiveMatch(string username)
        {
            _matchController.LeaveActiveMatch(username);
        }
    }
}
