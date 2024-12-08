using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dao
{
    public class ResultsDao
    {
        public ResultsDao() { }

        public Results GetResultsByAccount(int idAccount)
        {
            var results = new Results();
            try
            {
                using (var context = new BlockusEntities())
                {
                    results = context.Results.Where(r => r.Id_Account == idAccount).FirstOrDefault();
                }
            }
            catch (SqlException e)
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
