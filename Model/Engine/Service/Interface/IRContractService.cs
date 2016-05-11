using System.Data;
using Microsoft.Office.Interop.Word;
using Model.Engine.Repository.Interface;

namespace Model.Engine.Service.Interface
{
    public interface IRContractService : IBaseService<IRContractRepository>
    {
        int Count();
        void Create(rcontract rcontract);
        void Pay(int id);
        void RefuseContract(int id);
        void ContractToWord(int id, DocumentClass wordDocument);
        //TODO: Поменять название на спецификацию и транспортную накладную
        DataSet PDF(int id);
    }
}