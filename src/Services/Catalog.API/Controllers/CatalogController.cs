using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        private readonly IProductManager _productManager;
        public CatalogController(IProductManager productManager)
        {
            this._productManager = productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 30)]
        public IActionResult GetProducts()
        {
            try
            {
                var products = this._productManager.GetAll();
                return CustomResult("Data loaded successfully", products);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 30)]
        public IActionResult GetByCategory(string category)
        {
            try
            {
                var products = this._productManager.GetByCategory(category);
                return CustomResult("Data loaded successfully", products);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult GetById(string id)
        {
            try
            {
                var product = this._productManager.GetById(id);
                return CustomResult("Data loaded successfully", product);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }

        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = _productManager.Add(product);
                if(isSaved)
                {
                    return CustomResult("Product created successfully", product, HttpStatusCode.Created);
                }
                return CustomResult("Product save failed", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if(string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                var existingProduct = _productManager.GetById(product.Id);
                if (existingProduct == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                bool isUpdated = _productManager.Update(product.Id, product);
                if (isUpdated)
                {
                    return CustomResult("Product modified successfully", product, HttpStatusCode.OK);
                }
                return CustomResult("Product modified failed", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }


        [HttpDelete]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                var existingProduct = _productManager.GetById(id);
                if (existingProduct == null)
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }
                bool isDeleted = _productManager.Delete(id);
                if (isDeleted)
                {
                    return CustomResult("Product deleted successfully", HttpStatusCode.OK);
                }
                return CustomResult("Product deleted failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
