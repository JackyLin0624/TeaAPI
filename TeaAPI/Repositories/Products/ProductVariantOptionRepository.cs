using Dapper;
using System.Data;
using TeaAPI.Models.Products;
using TeaAPI.Repositories.Products.Interfaces;

namespace TeaAPI.Repositories.Products
{
    public class ProductVariantOptionRepository : DapperBaseRepository, IProductVariantOptionRepository
    {
        public ProductVariantOptionRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<ProductVariantOptionPO>> GetByProductIdAsync(int productId)
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProductVariantOptionPO>(
                "GetProductVariantOptionsByProductId",
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task AssignVariantsToProductAsync(int productId, IEnumerable<int> variantValueIds)
        {
            using var connection = CreateConnection();
            var dataTable = new DataTable();
            dataTable.Columns.Add("ProductId", typeof(int));
            dataTable.Columns.Add("VariantValueId", typeof(int));

            foreach (var variantValueId in variantValueIds)
            {
                dataTable.Rows.Add(productId, variantValueId);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@VariantTable", dataTable.AsTableValuedParameter("ProductVariantTableType"));

            await connection.ExecuteAsync("AssignVariantsToProduct", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteByProductIdAsync(int productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync(
                "DeleteProductVariantsByProductId",
                new { ProductId = productId },
                commandType: CommandType.StoredProcedure
            );
        }
    }

}
