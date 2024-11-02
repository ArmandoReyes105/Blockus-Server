using Data.Dao;
using Services.Dtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Services.Implementations
{
    public partial class ServiceImplementation : ISessionService
    {

        private static readonly HashSet<string> connectedUsers = new HashSet<string>();

        public AccountDTO LogIn(string username, string password)
        {
            AccountDao dao = new AccountDao();
            var account = dao.Login(username, password);

            var accountDTO = new AccountDTO
            {
                Id = account.Id_Account,
                Username = account.Username,
                Password = account.AccountPassword,
                Email = account.Email,
                ProfileImage = (int)account.ProfileImage
            };

            if (connectedUsers.Contains(accountDTO.Username))
            {
                accountDTO.Id = -2; 
            }

            if (accountDTO.Id > 0)
            {
                Console.WriteLine(accountDTO.Username + " inició sesión"); 
                connectedUsers.Add(accountDTO.Username);
            }

            return accountDTO; 
        }

        public void LogOut(string username)
        {
            Console.WriteLine($"{username} cerró sesión");
            connectedUsers.Remove(username);
        }
    }
}
