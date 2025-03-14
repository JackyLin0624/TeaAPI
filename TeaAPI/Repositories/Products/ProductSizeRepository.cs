using Dapper;
using System.Data;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;

namespace TeaAPI.Repositories.Products
{
    public class ProductSizeRepository : DapperBaseRepository, IProductSizeRepository
    {
        public ProductSizeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task DeleteByProductIdAsync(int productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DeleteProductSizesByProductId",
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductSizePO>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProductSizePO>(
                "GetAllProductSizes",
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<ProductSizePO>> GetByProductIdAsync(int productId, bool includeDeleted = false)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProductSizePO>(
                "GetProductSizesByProductId",
                new { ProductId = productId, IncludeDeleted = includeDeleted },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateSizesAsync(int productId, IEnumerable<ProductSizePO> sizes)
        {
            using var connection = CreateConnection();

            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Size", typeof(string));
            table.Columns.Add("Price", typeof(decimal));

            foreach (var size in sizes)
            {
                table.Rows.Add(size.Id == 0 ? DBNull.Value : size.Id, productId, size.Size, size.Price);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", productId);
            parameters.Add("@Sizes", table.AsTableValuedParameter("ProductSizeTableType"));

            await connection.ExecuteAsync("UpdateProductSizes", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
