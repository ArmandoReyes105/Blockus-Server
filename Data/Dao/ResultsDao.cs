using Data.Model;
using System;
using System.Data.Entity.Core;
using System.Linq;

namespace Data.Dao
{
    public class ResultsDao
    {

        private readonly BlockusEntities _context; 

        public ResultsDao(BlockusEntities context) 
        {
            _context = context; 
        }

        public ResultsDao() : this(new BlockusEntities()) { }

        public Results GetResultsByAccount(int idAccount)
        {
            var results = new Results();
            try
            {
                results = _context.Results.Where(r => r.Id_Account == idAccount).FirstOrDefault();
            } 
            catch (EntityException e)
            {
                Console.WriteLine(e.Message);
                results = new Results
                {
                    Id_Result = -1,
                    Victories = 0,
                    Losses = 0,
                    Id_Account = 0
                };
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                results = new Results
                {
                    Id_Result = -1,
                    Victories = 0,
                    Losses = 0,
                    Id_Account = 0
                };
            }

            if (results == null)
            {
                results = new Results
                {
                    Id_Result = 0,
                    Victories = 0,
                    Losses = 0,
                    Id_Account = 0
                };
            }

            return results;
        }
    }
}
