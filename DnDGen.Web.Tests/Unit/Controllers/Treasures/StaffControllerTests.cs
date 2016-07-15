﻿using DnDGen.Web.Controllers.Treasures;
using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using TreasureGen.Items;
using TreasureGen.Items.Magical;

namespace DnDGen.Web.Tests.Unit.Controllers.Treasures
{
    [TestFixture]
    public class StaffControllerTests
    {
        private StaffController controller;
        private Mock<MagicalItemGenerator> mockStaffGenerator;
        private Item staff;

        [SetUp]
        public void Setup()
        {
            mockStaffGenerator = new Mock<MagicalItemGenerator>();
            controller = new StaffController(mockStaffGenerator.Object);

            staff = new Item { ItemType = ItemTypeConstants.Staff };
            mockStaffGenerator.Setup(g => g.GenerateAtPower("power")).Returns(staff);
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
        public void GenerateReturnsStaffFromGenerator()
        {
            var result = controller.Generate("power") as JsonResult;
            dynamic data = result.Data;

            Assert.That(data.treasure.Items[0], Is.EqualTo(staff));
            Assert.That(data.treasure.Items.Length, Is.EqualTo(1));
        }

        [Test]
        public void ItemTypeMustBeStaff()
        {
            var otherItem = new Item { ItemType = "other" };
            mockStaffGenerator.SetupSequence(g => g.GenerateAtPower("power"))
                .Returns(otherItem)
                .Returns(staff);

            var result = controller.Generate("power") as JsonResult;
            dynamic data = result.Data;

            Assert.That(data.treasure.Items[0], Is.EqualTo(staff));
            Assert.That(data.treasure.Items.Length, Is.EqualTo(1));
        }
    }
}