using System.ServiceModel;

namespace SimpleWcfService
{
    [ServiceContract]
    public interface ISimpleWcfService
    {
        [OperationContract]
        string GetData();
    }
}
