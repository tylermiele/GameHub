using GameHub.Controllers;
using GameHub.Data;
using GameHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHubTests
{
    [TestClass]
    public  class ProductsControllerTests
    {
        private ApplicationDbContext _context;
        ProductsController controller;

        //this is a special startup method that runs before each unit test to avoid repeating arrange code
        [TestInitialize]
        public void TestInitialize()
        {
            //set up the in-memory DB that's needed in every test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);


            //create mock data in the in-memory db to unit test te ProductsController methods in the web app
            var category = new Category
            {
                CategoryId = 1000,
                Name = "Test Category"
            };
            _context.Add(category);
            
            for (var i = 1; i <=3; i++)
            {
                var product = new Product
                {
                    CategoryId = 1000,
                    Name = "Product " + i.ToString(),
                    Price = i + 10,
                    Category = category
                };
                _context.Add(product);

            }
            _context.SaveChanges();

            //create the controller insatnce in memory
            controller = new ProductsController(_context);
        }

        #region "Index"
        [TestMethod]
        public void IndexLoadsIndexView()
        {
            //arrange - pass the in-memory DB instance as a dependecy to the controller
            //var controller = new ProductsController(_context);

            //act
            var result = (ViewResult)controller.Index().Result;

            //assert
            Assert.AreEqual("Index", result.ViewName);
        }
        [TestMethod]
        public void IndexLoadsProducts()
        {
            //arrange
            //var controller = new ProductsController(_context);

            //act
            var result = (ViewResult)controller.Index().Result;
            List<Product> model = (List<Product>)result.Model; //convert data to a lit of products

            //assert
            CollectionAssert.AreEqual(_context.Products.ToList(), model);

        }
        #endregion


        #region "Details"
        [TestMethod]
        public void DetailsNoIdLoads404()
        {
            //arrange
            //var controller = new ProductsController(_context);

            //act
            var result = (ViewResult)controller.Details(null).Result;

            //assert
            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void DetailsNoProductsLoads404()
        {
            //arrange
            _context.Products = null;

            //act
            var result = (ViewResult)controller.Details(1).Result;

            //assert
            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void DetailsInvalidIdLoads404()
        {
            //act
            var result = (ViewResult)controller.Details(4).Result;

            //assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DetailsValidIdLoadsProduct()
        {
            //act - try id 1,2, or 3
            var result = (ViewResult)controller.Details(2).Result;
            var model = (Product)result.Model;

            //assert
            Assert.AreEqual(_context.Products.Find(2), model);
        }

        [TestMethod]
        public void DetailsValidIdLoadsDetailsView()
        {
            //act 
            var result = (ViewResult)controller.Details(2).Result;

            //assert
            Assert.AreEqual("Details", result.ViewName);
        }
        #endregion

        #region "Edit"
        [TestMethod]
        public void EditInvalidIdLoads404()
        {

            var result = (ViewResult)controller.Edit(4, _context.Products.Find(1)).Result;

            //assert
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void EditInvalidModelState()
        {
            controller.ModelState.AddModelError("Edit", "404");

            var result = (ViewResult)controller.Edit(1, _context.Products.Find(1)).Result;

            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod]
        public void EditSavesValidProduct()
        {
            var product = _context.Products.Find(1);
            product.Price = 10;
            var result = (RedirectToActionResult)controller.Edit(1, product).Result;
            
            
            Assert.AreEqual(product, _context.Products.Find(1));
        }
        [TestMethod]
        public void EditLoadsView()
        {

            var result = (RedirectToActionResult)controller.Edit(1, _context.Products.Find(1)).Result;

            Assert.AreEqual("Edit", result.ActionName);

        }
        #endregion

        #region "Create"
        [TestMethod]
        public void CreateValidProductSavesToDb()
        {
            var product = new Product
            {
                ProductId = 50,
                Name = "New Product",
                Price = (decimal?)59.99,
                CategoryId = 1000
            };

            var result = controller.Create(product, null);

            Assert.AreEqual(product, _context.Products.Find(50));
        }

        [TestMethod]
        public void CreateValidProductRedirectsToIndex()
        {
            var product = new Product
            {
                ProductId = 50,
                Name = "New Product",
                Price = (decimal?)59.99,
                CategoryId = 1000
            };
            var result = (RedirectToActionResult)controller.Create(product, null).Result;

            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void CreateInvalidModelReloadsCreateView()
        {
            var product = new Product { };

            controller.ModelState.AddModelError("Model Error", "Properties are incomplete");
            var result = (ViewResult)controller.Create(product, null).Result;

            Assert.AreEqual("Create", result.ViewName);
        }
        #endregion

        #region "Delete"
        [TestMethod]
        public void DeleteValidIdRemovesProductFromDb()
        {
            var result = controller.DeleteConfirmed(1);

            Assert.IsNull(_context.Products.Find(1));
        }

        #endregion
    }

}
