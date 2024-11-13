using Data.Dao;
using Data.Model;
using Services.Dtos;
using Services.Interfaces;

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

            AccountDao dao = new AccountDao();
            int result = dao.CreateAccount(account);

            return result; 
        }

    }
}
