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

        }
        [TestMethod]
        public void IndexLoadsIndexView()
        {
            //arrange - pass the in-memory DB instance as a dependecy to the controller
            var controller = new ProductsController(_context);

            //act
            var result = (ViewResult)controller.Index().Result;

            //assert
            Assert.AreEqual("Index", result.ViewName);
        }
        [TestMethod]
        public void IndexLoadsProducts()
        {
            //arrange
            var controller = new ProductsController(_context);

            //act
            var result = (ViewResult)controller.Index().Result;
            List<Product> model = (List<Product>)result.Model; //convert data to a lit of products

            //assert
            CollectionAssert.AreEqual(_context.Products.ToList(), model);

        }
    }
}
