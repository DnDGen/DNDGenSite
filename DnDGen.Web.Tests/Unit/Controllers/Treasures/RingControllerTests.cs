﻿using DnDGen.Web.Controllers.Treasures;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using TreasureGen.Common.Items;
using TreasureGen.Generators.Items.Magical;

namespace DnDGen.Web.Tests.Unit.Controllers.Treasures
{
    [TestFixture]
    public class RingControllerTests
    {
        private RingController controller;
        private Mock<MagicalItemGenerator> mockRingGenerator;
        private Item ring;

        [SetUp]
        public void Setup()
        {
            mockRingGenerator = new Mock<MagicalItemGenerator>();
            controller = new RingController(mockRingGenerator.Object);

            ring = new Item { ItemType = ItemTypeConstants.Ring };
            mockRingGenerator.Setup(g => g.GenerateAtPower("power")).Returns(ring);
        }

        [Test]
        public void GenerateHandlesGetVerb()
        {
            var attributes = AttributeProvider.GetAttributesFor(controller, "Generate");
            Assert.That(attributes, Contains.Item(typeof(HttpGetAttribute)));
        }

        [Test]
        public void GenerateReturnsJsonResult()
        {
            var result = controller.Generate("power");
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void GenerateJsonResultAllowsGet()
        {
            var result = controller.Generate("power") as JsonResult;
            Assert.That(result.JsonRequestBehavior, Is.EqualTo(JsonRequestBehavior.AllowGet));
        }

        [Test]
        public void GenerateReturnsRingFromGenerator()
        {
            var result = controller.Generate("power") as JsonResult;
            dynamic data = result.Data;

            Assert.That(data.treasure.Items[0], Is.EqualTo(ring));
            Assert.That(data.treasure.Items.Length, Is.EqualTo(1));
        }

        [Test]
        public void ItemTypeMustBeRing()
        {
            var otherItem = new Item { ItemType = "other" };
            mockRingGenerator.SetupSequence(g => g.GenerateAtPower("power"))
                .Returns(otherItem)
                .Returns(ring);

            var result = controller.Generate("power") as JsonResult;
            dynamic data = result.Data;

            Assert.That(data.treasure.Items[0], Is.EqualTo(ring));
            Assert.That(data.treasure.Items.Length, Is.EqualTo(1));
        }
    }
}