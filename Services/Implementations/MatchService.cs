using Services.Controllers;
using Services.Dtos;
using Services.Enums;
using Services.Interfaces;
using System.ServiceModel;

namespace Services.Implementations
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServiceImplementation : IMatchService
    {
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
