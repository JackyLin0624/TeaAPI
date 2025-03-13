using Dapper;
using System.Data;
using TeaAPI.Dtos.Products;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;

namespace TeaAPI.Repositories.Products
{
    public class VariantTypeRepository : DapperBaseRepository, IVariantTypeRepository
    {
        public VariantTypeRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<VariantTypePO>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<VariantTypePO>(
                "GetAllVariantTypes",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<VariantTypePO> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<VariantTypePO>(
                "GetVariantTypeById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CreateAsync(VariantTypePO variantType)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@TypeName", variantType.TypeName);
            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("CreateVariantType", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewId");
        }
    }

}
