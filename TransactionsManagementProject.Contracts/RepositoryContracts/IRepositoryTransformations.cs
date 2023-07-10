using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsManagementProject.Contracts.DomainEntities;

namespace TransactionsManagementProject.Contracts.RepositoryContracts
{
    public interface IRepositoryTransformations
    {

        Task<List<Transformation>> GetTransformations();
        List<Transformation> GetTransformationsInFile();
        void PersistTransformationInFile(List<Transformation> transformations);
    }
}
