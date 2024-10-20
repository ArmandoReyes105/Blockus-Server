using Services.Interfaces;
using Data;
using Data.Model; 

namespace Services.Implementations
{
    public partial class ServiceImplementation : IService1
    {
        public string GetData(int value)
        {
            Account account = new Account(); 

            return string.Format("You entered: {0}", value);
        }
    }
}
