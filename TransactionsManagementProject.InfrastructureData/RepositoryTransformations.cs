using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsManagementProject.Contracts.DomainEntities;
using TransactionsManagementProject.Contracts.RepositoryContracts;
using TransactionsManagementProject.InfrastructureData.DTOs;
using TransactionsManagementProject.InfrastructureData.RepositoryHelpers;

namespace TransactionsManagementProject.InfrastructureData
{
    public class RepositoryTransformations : IRepositoryTransformations
    {
        private readonly string _pathRoot;

        private readonly string _pathFile;

        public RepositoryTransformations(IConfiguration configuration)
        {
            _pathRoot = configuration.GetSection("ApiCalls:Transformations").Value;

            _pathFile = configuration.GetSection("PathFiles:Transformations").Value;
        }

        public async Task<List<Transformation>> GetTransformations()
        {
            List<TransformationsDto> transformationsDto = await RepositoryHelper.GetList<TransformationsDto>(_pathRoot);

            if (transformationsDto == null || transformationsDto.Count == 0) return new List<Transformation>();

            return TransformDtoToDomain(transformationsDto);

        }

        public List<Transformation> GetTransformationsInFile()
        {
            return RepositoryHelper.GetListFromFile<Transformation>(_pathFile);
        }

        public void PersistTransformationInFile(List<Transformation> transformations)
        {
            RepositoryHelper.PersistList<Transformation>(transformations, _pathFile);
        }

        private List<Transformation> TransformDtoToDomain(List<TransformationsDto> transformationsDto)
        {
            List<Transformation> transformations = new List<Transformation>();

            foreach (var transformationDto in transformationsDto)
            {
                transformations.Add(new Transformation
                {
                    EntryType = transformationDto.EntryType,
                    ExitType = transformationDto.ExitType,
                    Ratio = decimal.Parse(transformationDto.Ratio, CultureInfo.InvariantCulture)

                });
            }

            return transformations;

        }
    }
}
