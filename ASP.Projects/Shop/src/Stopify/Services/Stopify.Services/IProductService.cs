

using Stopify.Services.Models;
namespace Stopify.Services
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IProductService
    {

        Task<bool> Create(ProductServiceModel inputmodel);

        Task<bool> CreateProductType(ProductTypeServiceModel productTypeServiceModel);
        IQueryable<ProductTypeServiceModel> GetAllProductTypes();

        IQueryable<ProductServiceModel> GetAllProducts();
    }
}
