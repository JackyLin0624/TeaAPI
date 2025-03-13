using Dapper;
using System.Data;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;

namespace TeaAPI.Repositories.Products
{
    public class VariantValueRepository : DapperBaseRepository, IVariantValueRepository
    {
        public VariantValueRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<VariantValuePO>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<VariantValuePO>(
                "GetAllVariantValues",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<VariantValuePO>> GetByTypeIdAsync(int variantTypeId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<VariantValuePO>(
                "GetVariantValuesByTypeId",
                new { VariantTypeId = variantTypeId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<VariantValuePO>> GetByIdsAsync(IEnumerable<int> variantValueIds)
        {
            using var connection = CreateConnection();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            foreach (var id in variantValueIds)
            {
                dataTable.Rows.Add(id);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@VariantValueIds", dataTable.AsTableValuedParameter("IntList"));

            return await connection.QueryAsync<VariantValuePO>(
                "GetVariantValuesByIds",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
