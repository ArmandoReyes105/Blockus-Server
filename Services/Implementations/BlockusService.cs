using Services.Interfaces;
using System;

namespace Services.Implementations
{
    public partial class BlockusService : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

    }
}
