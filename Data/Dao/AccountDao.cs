using Data.Model;
using System;
using System.Data.Entity.Validation;

namespace Data.Dao
{
    public class AccountDao
    {
        public AccountDao()
        {
            
        }

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
    }
}
