using GameHub.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GameHubTests
{
    //this file contains unit tests for the methods in the GameHub web app SimpleController file
    [TestClass]
    public class SimpleControllerTests
    {
        [TestMethod]
        public void IndexReturnsSomething()
        {
            //arrange - set up conditions to try the Index method
            var controller = new SimpleController();

            //act - execute the Index method
            var result = controller.Index();

            //assert - did th method return something and not a null response
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexLoadsIndexView()
        {
            //arrange - set up conditions to try the Index method
            var controller = new SimpleController();

            //act - execute the Index method
            var result = (ViewResult)controller.Index();

            //assert - did th method return something and not a null response
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}