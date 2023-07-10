using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionsManagementProject.Contracts.RepositoryContracts
{
    public interface IRepositoryCache
    {
        List<T> GetCacheList<T>(string key);
        T GetCache<T>(string key, T defaultValue);

        void SetCache<T>(string key, T generic);

        void SetCacheList<T>(string key, List<T> generic);

    }
}
