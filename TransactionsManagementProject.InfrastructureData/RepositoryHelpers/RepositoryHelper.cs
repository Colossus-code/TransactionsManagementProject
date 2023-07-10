using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TransactionsManagementProject.InfrastructureData.RepositoryHelpers
{
    public static class RepositoryHelper
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task<List<T>> GetList<T>(string rootPath)
        {
            HttpResponseMessage get = await httpClient.GetAsync(rootPath);

            string responseAsString = await get.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseAsString)) return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(responseAsString);
        }

        public static void PersistList<T>(List<T> genericList, string pathFile)
        {
            string serializedList = JsonSerializer.Serialize(genericList);

            File.WriteAllText(pathFile, serializedList);

        }

        public static List<T> GetListFromFile<T>(string path)
        {
            string serializedActualList = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<T>>(serializedActualList);
        }
    }
}
