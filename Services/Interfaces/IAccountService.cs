﻿using Data.Model;
using Services.Dtos;
using Services.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        int CreateAccount(AccountDTO accountDTO);
        [OperationContract]
        int UpdateAccount(AccountDTO accountDTO);
        [OperationContract]
        ResultsDTO GetAccountResults(int idAccount);
        [OperationContract]
        ProfileConfigurationDTO GetProfileConfiguration(int idAccount);
        [OperationContract]
        int AddFriend(int idAccount, int IdAccountFriend);
        [OperationContract]
        List<PublicAccountDTO> GetAddedFriends(int idAccount);
        [OperationContract]
        int DeleteFriend(int idFriend, int idAccount);
        [OperationContract]
        List<PublicAccountDTO> SearchByUsername(string username);
    }
}
