using Data.Dao;
using Data.Model;
using Services.Dtos;
using Services.Interfaces;
using System;

namespace Services.Implementations
{
    public partial class ServiceImplementation : IAccountService
    {
        public int CreateAccount(AccountDTO accountDTO)
        {

            var account = new Account()
            {
                Username = accountDTO.Username,
                Email = accountDTO.Email,
                ProfileImage = 1,
                AccountPassword = accountDTO.Password
            };

            Console.WriteLine(accountDTO.Username);

            AccountDao dao = new AccountDao();
            int result = dao.CreateAccount(account);

            return result; 
        }

        public Account Login(string username, string password)
        {
            var dao = new AccountDao();
            var account = dao.Login(username, password);

            if (account != null)
            {
                return new Account()
                {
                    Username = account.Username,
                    Email = account.Email,
                    ProfileImage = 1,
                    AccountPassword = account.AccountPassword
                };
            }
            else
            {
                return null;
            }
        }
    }
}
