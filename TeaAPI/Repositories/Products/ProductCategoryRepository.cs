using Dapper;
using System.Data;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;

namespace TeaAPI.Repositories.Products
{
    public class ProductCategoryRepository : DapperBaseRepository, IProductCategoryRepository
    {
        public ProductCategoryRepository(
            IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<ProductCategoryPO>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProductCategoryPO>(
                "GetAllProductCategories",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProductCategoryPO> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ProductCategoryPO>(
                "GetProductCategoryById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
