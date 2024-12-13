using Services.Dtos;
using Services.Enums;
using System.ServiceModel;

namespace Services.Interfaces
{
    /// <summary>
    /// Defines the match service operations for the application. 
    /// This service handles joining active matches, retrieving the current block, placing blocks, making movements, skipping turns, and leaving active matches.
    /// </summary>
    [ServiceContract (CallbackContract = typeof(IMatchServiceCallback))]
    public interface IMatchService
    {
        /// <summary>
        /// Allows a user to join an active match
        /// </summary>
        /// <param name="username">The username of the user joining the match</param>
        /// <returns>object representing the match the user has joined</returns>
        [OperationContract]
        MatchDTO JoinToActiveMatch(string username);

        /// <summary>
        /// Retrieves the current block for a given match
        /// </summary>
        /// <param name="matchCode">The unique code identifying the match</param>
        /// <returns>/// <returns>A <see cref="BlockDTO"/> object representing the current block</returns>
        [OperationContract]
        BlockDTO GetCurrentBlock(string matchCode);

        /// <summary>
        /// Places a block in the match and awards points
        /// </summary>
        /// <param name="matchCode">The unique code identifying the match</param>
        /// <param name="points">The points awarded for placing the block</param>
        /// <returns>object representing the result of the action</returns>
        [OperationContract]
        GameResult PlaceBlock(string matchCode, int points);

        /// <summary>
        /// Executes a movement within the match
        /// </summary>
        /// <param name="matchCode">The unique code identifying the match</param>
        /// <param name="movement">object representing the movement to be made</param>
        [OperationContract]
        void MakeMovement(string matchCode, Movement movement);

        /// <summary>
        /// Allows a user to skip their turn in the match
        /// </summary>
        /// <param name="matchCode">he unique code identifying the match</param>
        /// <returns>object representing the result of skipping the turn</returns>
        [OperationContract]
        GameResult SkipTurn(string matchCode);

        /// <summary>
        /// Allows a user to leave an active match
        /// </summary>
        /// <param name="username">The username of the user leaving the match</param>
        [OperationContract]
        void LeaveActiveMatch(string username);
    }

    /// <summary>
    /// Defines the callback operations for the match service. 
    /// These callbacks notify clients about match activities such as block placements, block changes, opponent movements, game completion, and player exits
    /// </summary>
    [ServiceContract]
    public interface IMatchServiceCallback
    {
        /// <summary>
        /// Notifies the client that a block has been placed in the match
        /// </summary>
        [OperationContract]
        void OnBlockPlaced();

        /// <summary>
        /// Notifies the client that the current block has changed
        /// </summary>
        /// <param name="block">object representing the new current block</param>
        [OperationContract]
        void OnCurrentBlockChanged(BlockDTO block);

        /// <summary>
        /// Notifies the client of an opponent's movement in the match
        /// </summary>
        /// <param name="movement">object representing the opponent's movement</param>
        [OperationContract]
        void OnOpponentMovement(Movement movement);

        /// <summary>
        /// Notifies the client that the game has finished
        /// </summary>
        /// <param name="gameResult">object representing the result of the game</param>
        [OperationContract]
        void OnGameFinished(GameResult gameResult);

        /// <summary>
        /// Notifies the client that a player has left the match
        /// </summary>
        /// <param name="username">The username of the player who left</param>
        /// <param name="color">The color associated with the player who left</param>
        [OperationContract]
        void OnPlayerLeave(string username, Color color);
    }
}
