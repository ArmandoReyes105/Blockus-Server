using Services.Dtos;
using Services.Enums;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract (CallbackContract = typeof(IMatchServiceCallback))]
    public interface IMatchService
    {
        [OperationContract]
        MatchDTO JoinToActiveMatch(string username);

        [OperationContract]
        BlockDTO GetCurrentBlock(string matchCode);

        [OperationContract]
        GameResult PlaceBlock(string matchCode, int points);

        [OperationContract]
        void MakeMovement(string matchCode, Movement movement);

        [OperationContract]
        GameResult SkipTurn(string matchCode);

        [OperationContract]
        void LeaveActiveMatch(string username);
    }

    [ServiceContract]
    public interface IMatchServiceCallback
    {
        [OperationContract]
        void OnBlockPlaced();

        [OperationContract]
        void OnCurrentBlockChanged(BlockDTO block);

        [OperationContract]
        void OnOpponentMovement(Movement movement);

        [OperationContract]
        void OnGameFinished(GameResult gameResult);

        [OperationContract]
        void OnPlayerLeave(string username, Color color);
    }
}
