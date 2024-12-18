using Data.Model;
using System;
using System.Data.Entity.Core;
using System.Linq;

namespace Data.Dao
{
    public class ProfileConfigurationDao
    {

        private readonly BlockusEntities _context; 

        public ProfileConfigurationDao() : this(new BlockusEntities()) { }

        public ProfileConfigurationDao(BlockusEntities context)
        {
            _context = context;
        }

        public ProfileConfiguration GetProfileConfiguration(int idAccount)
        {
            var profileConfiguration = new ProfileConfiguration();

            try
            {
                profileConfiguration = _context.ProfileConfiguration.Where(pc => pc.Id_Account == idAccount).FirstOrDefault();
            } 
            catch (EntityException e)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
