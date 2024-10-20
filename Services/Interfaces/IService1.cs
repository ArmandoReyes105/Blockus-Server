using System.ServiceModel;

namespace Services.Interfaces
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);
    }
}
