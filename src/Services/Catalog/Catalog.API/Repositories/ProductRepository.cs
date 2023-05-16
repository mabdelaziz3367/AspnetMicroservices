using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));        
        }
        public async Task CreateProduct(Product product)
        {
           await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var deleteResult = await this._context.
              Products.
              DeleteOneAsync(filter: g => g.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                .Products
                .Find<Product>(p => p.Id == id)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                .Products
                .Find<Product>( p => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _context
                  .Products
                  .Find(filter)
                  .ToListAsync();

        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context
                  .Products
                  .Find(filter)
                  .ToListAsync();
            
        }

        public async Task<bool> UpdateProduct(Product product)
        {
           var updatedResult = await this._context.
                Products.
                ReplaceOneAsync(filter: g=>g.Id == product.Id, replacement: product);
            return updatedResult.IsAcknowledged && updatedResult.ModifiedCount > 0;
        }

   
    }
}
