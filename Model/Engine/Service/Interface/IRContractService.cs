using System.Data;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IRContractService : IBaseService<IRContractRepository>
    {
        int Count();
        void Create(rcontract rcontract);
        void Pay(int id);
        void RefuseContract(int id);
        string PrintDog(int id, string fileName);
        DataSet GetSpecification(int id);
    }
}