using Services.Interfaces;
using Data;
using Data.Model; 

namespace Services.Implementations
{
    public class ServiceImplementation : IService1
    {
        public string GetData(int value)
        {

            Class1 class1 = new Class1();
            Account account = new Account(); 

            return string.Format("You entered: {0}", value);
        }
    }
}
