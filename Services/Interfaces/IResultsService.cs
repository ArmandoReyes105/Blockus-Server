using Services.Dtos;
using Services.Enums;
using System.ServiceModel;

namespace Services.Interfaces
{
    /// <summary>
    /// Defines the results service operations for the application. 
    /// This service handles retrieving match summaries and updating game results for accounts.
    /// </summary>
    [ServiceContract]
    public interface IResultsService
    {
        /// <summary>
        /// Retrieves a summary of the match based on the provided match code
        /// </summary>
        /// <param name="matchCode">The unique code indentifying the match</param>
        /// <returns>object representing the summary of the match</returns>
        [OperationContract]
        MatchResumeDTO GetMatchResume(string matchCode);

        /// <summary>
        /// Updates the results of a game for a specific account
        /// </summary>
        /// <param name="idAccount">The identifier of the account for which the results are being updated</param>
        /// <param name="gameResult">object representing the result of the game</param>
        /// <returns>An integer indicating the status of the update operation</returns>
        [OperationContract]
        int UpdateResults(int idAccount, GameResult gameResult);
    }
}
