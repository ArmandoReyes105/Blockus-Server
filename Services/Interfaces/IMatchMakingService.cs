using Services.Dtos;
using System.ServiceModel;

namespace Services.Interfaces
{
    /// <summary>
    /// Defines the matchmaking service operations for the application
    /// This service handles the creation of matches, joining existing matches.
    /// </summary>
    [ServiceContract (CallbackContract = typeof(IMatchMakingServiceCallback))]
    public interface IMatchMakingService
    {
        /// <summary>
        /// Creates a new match hosted by a specified account
        /// </summary>
        /// <param name="hostAccount">the account hosting the match</param>
        /// <returns>object representing the newly created match</returns>
        [OperationContract]
        MatchDTO CreateMatch(PublicAccountDTO hostAccount);

        /// <summary>
        /// Allows an account to join an existing match using a match code
        /// </summary>
        /// <param name="account">The account joining the match</param>
        /// <param name="matchCode">The unique code indentifying the match</param>
        /// <returns>object representing the match joined</returns>
        [OperationContract]
        MatchDTO JoinToMatch(PublicAccountDTO account, string matchCode);

        /// <summary>
        /// Allows a user to leave a match
        /// </summary>
        /// <param name="username">The username of the user leaving the match</param>
        [OperationContract(IsOneWay = true)]
        void LeaveMatch(string username);
    }

    /// <summary>
    /// Defines the callback operations for the matchmaking service
    /// These callbacks notify clients about player activities within the match
    /// </summary>
    [ServiceContract]
    public interface IMatchMakingServiceCallback
    {
        /// <summary>
        /// Notifies the client that a new player has joined the match
        /// </summary>
        /// <param name="matchDTO">Object representing the match where a player has joined</param>
        [OperationContract]
        void NotifyPlayerEntry(MatchDTO matchDTO);

        /// <summary>
        /// Notifies the client that a player has left the match
        /// </summary>
        /// <param name="matchDTO">object representing the match where a player has exited</param>
        [OperationContract]
        void NotifyPlayerExit(MatchDTO matchDTO);

        /// <summary>
        /// Notifies the client that the host has left the match
        /// </summary>
        /// <param name="matchDTO">object representing the match where a host has exited</param>
        [OperationContract]
        void NotifyHostExit(MatchDTO matchDTO);
    }
}
