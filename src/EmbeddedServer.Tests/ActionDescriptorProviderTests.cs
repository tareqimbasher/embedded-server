using System;
using System.Threading.Tasks;
using EmbeddedServer;
using EmbeddedServer.Hosting;
using EmbeddedServer.Middlewares;
using EmbeddedServer.Routing;
using EmbeddedServer.Strategies;
using Moq;
using System.Collections.Generic;
using EmbeddedServer.Tests.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedServerTest.Tests
{
    [TestClass]
    public class ActionDescriptorProviderTests
    {
        [TestMethod]
        public void ActionDescriptorProvider_ReturnsActionDescriptors()
        {
            var moq = new Mock<IControllerDiscoveryStrategy>();
            moq.Setup(x => x.DiscoverControllers()).Returns(new[] { typeof(ProductsController) });

            var actionDescriptorProvider = new ActionDescriptorProvider(moq.Object).Build();
            Assert.IsTrue(actionDescriptorProvider.ActionDescriptors.Count > 0, "Could not find any action descriptors");
        }

        [TestMethod]
        public void ActionDescriptorProvider()
        {
            var actionDescriptorProvider = new ActionDescriptorProvider(new ControllerDiscoveryStrategy());
            Assert.IsTrue(actionDescriptorProvider.ActionDescriptors.Count == 0, "Action descriptor count should be 0");

            actionDescriptorProvider.Build();
            Assert.IsTrue(actionDescriptorProvider.ActionDescriptors.Count > 0, "Could not find any action descriptors");
        }
    }
}
