using Data.Dao;
using Data.Model;
using Services.Dtos;
using Services.Interfaces;
using System.Collections.Generic;

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

        public int UpdateAccount(AccountDTO accountDTO)
        {
            AccountDao dao = new AccountDao();
            var account = new Account()
            {
                Id_Account = accountDTO.Id,
                Username = accountDTO.Username,
                ProfileImage = accountDTO.ProfileImage,
                AccountPassword = accountDTO.Password
            };
            return dao.UpdateAccount(account);
        }

        public ResultsDTO GetAccountResults(int idAccount)
        {
            ResultsDao dao = new ResultsDao();

            var result = dao.GetResultsByAccount(idAccount);
            var resultDTO = new ResultsDTO
            {
                Id = result.Id_Result,
                Victories = (int)result.Victories,
                Losses = (int)result.Losses,
                IdAccount = (int)result.Id_Account
            };
            return resultDTO;

        }

        public ProfileConfigurationDTO GetProfileConfiguration(int idAccount)
        {
            ProfileConfigurationDao dao = new ProfileConfigurationDao();

            var profileConfiguration = dao.GetProfileConfiguration(idAccount);
            var profileConfigurationDTO = new ProfileConfigurationDTO()
            {
                Id = profileConfiguration.Id_Configuration,
                BoardStyle = (int)profileConfiguration.BoardStyle,
                TilesStyle = (int)profileConfiguration.TilesStyle,
                IdAccount = (int)profileConfiguration.Id_Account
            };
            return profileConfigurationDTO;
        }
    }
}
