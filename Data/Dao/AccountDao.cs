using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace Data.Dao
{
    public class AccountDao
    {
        public AccountDao(){ }

        public int CreateAccount(Account account)
        {

            int result; 

            using (var context = new BlockusEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Account.Add(account);
                        context.SaveChanges();

                        var configuration = new ProfileConfiguration
                        {
                            Id_Account = account.Id_Account,
                            BoardStyle = 1, 
                            TilesStyle = 1
                        };

                        context.ProfileConfiguration.Add(configuration);
                        context.SaveChanges();

                        transaction.Commit();
                        result = 1; 
                    }catch (DbEntityValidationException ex)
                    {
                        transaction.Rollback();
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            }
                        }
                        result = 0; 
                    }
                }
            }

            return result;

        }

        public Account Login(string username, string password) 
        {

            Account resultAccount;  

            using (var context = new BlockusEntities())
            {
                try
                {

                    resultAccount = context.Account
                        .Where(a => (a.Username.Equals(username) || a.Email.Equals(username)) && a.AccountPassword.Equals(password))
                        .FirstOrDefault();

                } 
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);

                    resultAccount = new Account
                    {
                        Id_Account = -1,
                        ProfileImage = 0
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Detalles internos: " + e.InnerException.Message);
                    }

                    resultAccount = new Account
                    {
                        Id_Account = -1,
                        ProfileImage = 0
                    };
                }
            }

            if (resultAccount == null)
            {
                resultAccount = new Account
                {
                    Id_Account = 0,
                    ProfileImage = 0
                }; 
                
            }

            return resultAccount; 
        }

        public int IncreaseVictories(int accountId)
        {
            int operationResult = 0;

            using (var context = new BlockusEntities())
            {
                try
                {

                    var results = context.Results.Where(x => x.Id_Account == accountId).FirstOrDefault();
                    results.Victories++; 
                    operationResult = context.SaveChanges(); 

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operationResult = -1; 
                }
            }

            return operationResult;
        }

        public int IncreaseLosses(int accountId)
        {
            int operationResult = 0;

            using (var context = new BlockusEntities())
            {
                try
                {

                    var results = context.Results.Where(x => x.Id_Account == accountId).FirstOrDefault();
                    results.Losses++;
                    operationResult = context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    operationResult = -1;
                }
            }

            return operationResult;
        }

        public int UpdateAccount(Account account)
        {
            try
            {
                using (var context = new BlockusEntities())
                {
                    var acc = context.Account.FirstOrDefault(a => a.Id_Account == account.Id_Account);

                    if (acc == null)
                    {
                        Console.WriteLine("Account with Id: " + account.Id_Account + " not found");
                        return 0;
                    }

                    acc.AccountPassword = account.AccountPassword;
                    acc.Username = account.Username;
                    acc.ProfileConfiguration = account.ProfileConfiguration;
                    acc.ProfileImage = account.ProfileImage;

                    int affectedRows = context.SaveChanges();

                    return affectedRows;
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        public int AddFriend(int idAccount, int idAccountFriend)
        {
            try
            {
                if (idAccount == idAccountFriend)
                {
                    Console.WriteLine(idAccountFriend + " is you\n");
                    return -2;
                }
                using (var context = new BlockusEntities())
                {
                    var alreadyFriends = context.Friends
                        .FirstOrDefault(f => f.Id_Account == idAccount
                        && f.Id_Account_Friend == idAccountFriend);

                    if (alreadyFriends != null)
                    {
                        Console.WriteLine("Already friends...\n");
                        return 0;
                    }

                    var newFriend = new Friends
                    {
                        Id_Account = idAccount,
                        Id_Account_Friend = idAccountFriend
                    };

                    context.Friends.Add(newFriend);
                    context.SaveChanges();

                    return 1;
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public List<Friends> GetAccountFriends(int idAccount)
        {
            try
            {
                using (var context = new BlockusEntities())
                {
                    return context.Friends.Where(f => f.Id_Account == idAccount).ToList();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error al obtener la lista de amigos " + ex.Message);
                return new List<Friends>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado, intente mas tarde. " + ex.Message);
                return new List<Friends>();
            }
        }

        public int RemoveFriendById(int idFriend, int idAccount)
        {
            try
            {
                using (var context = new BlockusEntities())
                {
                    var deletedFriend = context.Friends.FirstOrDefault(f => f.Id_Account == idAccount
                        && f.Id_Account_Friend == idFriend);

                    if (deletedFriend != null)
                    {
                        context.Friends.Remove(deletedFriend);
                        context.SaveChanges();
                        return 1;
                    }
                    return 0;
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error en la base de datos, por favor intente mas tarde. \n" + ex.Message);
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inesperado. Intente mas tarde \n" + ex.Message);
                return -1;
            }
        }

        public Account GetAccountById(int idAccount)
        {
            Account resultAccount;
            try
            {
                using (var context = new BlockusEntities())
                {
                    resultAccount = context.Account.Where(a => a.Id_Account == idAccount).FirstOrDefault();
                }
            }
            catch (EntityException ex)
            {
                Console.WriteLine(ex.Message);
                resultAccount = new Account
                {
                    Id_Account = -1,
                    ProfileImage = 0
                };
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                resultAccount = new Account
                {
                    Id_Account = -1,
                    ProfileImage = 0
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                resultAccount = new Account
                {
                    Id_Account = -1,
                    ProfileImage = 0
                };
            }
            if (resultAccount == null)
            {
                resultAccount = new Account
                {
                    Id_Account = 0,
                    ProfileImage = 0
                };
            }

            return resultAccount;
        }

        public List<Account> GetAccountsByUsername(string username)
        {
            try
            {
                using (var context = new BlockusEntities())
                {
                    return context.Account.Where(a => a.Username.Contains(username)).ToList();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Account>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Account>();
            }
        }
    }
}
