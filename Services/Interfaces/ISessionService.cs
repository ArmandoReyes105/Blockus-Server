using Services.Dtos;
using System.ServiceModel;

namespace Services.Interfaces
{
    /// <summary>
    /// Defines the session service operations for the application. 
    /// This service handles user authentication, specifically logging in and logging out users.
    /// </summary>
    [ServiceContract]
    public interface ISessionService
    {
        /// <summary>
        /// Logs a user into the system using their username and password
        /// </summary>
        /// <param name="username">The username of the user attempting to log in</param>
        /// <param name="password">The password of the user attempting to log in</param>
        /// <returns>object representing the authenticated account</returns>
        [OperationContract]
        AccountDTO LogIn(string username, string password);

        /// <summary>
        /// Logs a user out of the system
        /// </summary>
        /// <param name="username">The username of the user attempting to log out</param>
        [OperationContract]
        void LogOut(string username); 
    }
}
