using Dapper;
using System.Data;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;

namespace TeaAPI.Repositories.Products
{
    public class ProductRepository : DapperBaseRepository, IProductRepository
    {
        public ProductRepository(
            IConfiguration configuration) 
            : base(configuration)
        {
        }

        public async Task<int> CreateAsync(ProductPO product)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@Description", product.Description);
            parameters.Add("@ImageUrl", product.ImageUrl);
            parameters.Add("@IsActive", product.IsActive);
            parameters.Add("@CategoryId", product.CategoryId);
            parameters.Add("@CreateUser", product.CreateUser);
            parameters.Add("@NewProductId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "CreateProduct",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@NewProductId"); 
        }


        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DeleteProduct",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductPO>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProductPO>(
                "GetAllProducts",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductPO>> GetByCategoryIdAsync(int categoryId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProductPO>(
                "GetProductsByCategoryId",
                new { CategoryId = categoryId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProductPO> GetByIdAsync(int id, bool includeDeleted = false)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ProductPO>(
                "GetProductById",
                new { Id = id, IncludeDeleted = includeDeleted },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task ModifyAsync(ProductPO product)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "ModifyProduct",
                new
                {
                    product.Id,
                    product.ProductName,
                    product.Description,
                    product.ImageUrl,
                    product.IsActive,
                    product.CategoryId,
                    product.ModifyUser
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
