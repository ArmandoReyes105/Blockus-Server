using Data.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dao
{
    public class ProfileConfigurationDao
    {
        public ProfileConfigurationDao() { }

        public ProfileConfiguration GetProfileConfiguration(int idAccount)
        {
            var profileConfiguration = new ProfileConfiguration();

            try
            {
                using (var context = new BlockusEntities())
                {
                    profileConfiguration = context.ProfileConfiguration.Where(pc => pc.Id_Account == idAccount).FirstOrDefault();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                profileConfiguration = new ProfileConfiguration
                {
                    Id_Configuration = -1,
                    BoardStyle = 0,
                    TilesStyle = 0,
                    Id_Account = 0
                };
            }

            if (profileConfiguration == null)
            {
                profileConfiguration = new ProfileConfiguration
                {
                    Id_Configuration = 0,
                    BoardStyle = 0,
                    TilesStyle = 0,
                    Id_Account = 0
                };
            }

            return profileConfiguration;
        }
    }
}
