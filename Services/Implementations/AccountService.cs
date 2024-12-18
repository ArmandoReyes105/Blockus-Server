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
            ResultsDao dao = new ResultsDao(new BlockusEntities());

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

        public int AddFriend(int idAccount, int IdAccountFriend)
        {
            AccountDao accountDao = new AccountDao();
            int result = accountDao.AddFriend(idAccount, IdAccountFriend);

            return result;
        }

        public List<PublicAccountDTO> GetAddedFriends(int idAccount)
        {
            AccountDao dao = new AccountDao();
            List<Account> friends = new List<Account>();
            List<PublicAccountDTO> friendsDTO = new List<PublicAccountDTO>();

            var accountFriends = dao.GetAccountFriends(idAccount);

            foreach (var friend in accountFriends)
            {
                var friendAccount = dao.GetAccountById((int)friend.Id_Account_Friend);
                friends.Add(friendAccount);
            }

            foreach (var friendInfo in friends)
            {
                var dto = new PublicAccountDTO
                {
                    Id = friendInfo.Id_Account,
                    Username = friendInfo.Username,
                    ProfileImage = (int)friendInfo.ProfileImage
                };

                friendsDTO.Add(dto);
            }

            return friendsDTO;
        }

        public int DeleteFriend(int idFriend, int idAccount)
        {
            AccountDao dao = new AccountDao();

            int stillFriends = dao.RemoveFriendById(idFriend, idAccount);

            return stillFriends;
        }

        public List<PublicAccountDTO> SearchByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new List<PublicAccountDTO>();
            }

            AccountDao dao = new AccountDao();
            List<PublicAccountDTO> userMatches = new List<PublicAccountDTO>();

            var foundUsers = dao.GetAccountsByUsername(username);

            foreach (var user in foundUsers)
            {
                var dto = new PublicAccountDTO
                {
                    Id = user.Id_Account,
                    Username = user.Username,
                    ProfileImage = (int)user.ProfileImage
                };
                userMatches.Add(dto);
            }

            return userMatches;
        }

        public int IsUsernameUnique(string username)
        {
            var dao = new AccountDao();
            return dao.IsUniqueUsername(username);
        }
    }
}
