﻿using CharacterGen.Randomizers.Alignments;
using CharacterGen.Randomizers.CharacterClasses;
using CharacterGen.Randomizers.Races;
using CharacterGen.Verifiers;
using DnDGen.Web.Controllers.Characters;
using DnDGen.Web.Repositories;
using EventGen;
using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace DnDGen.Web.Tests.Unit.Controllers.Characters
{
    [TestFixture]
    public class RandomizersControllerTests
    {
        private RandomizersController controller;
        private Mock<IRandomizerVerifier> mockRandomizerVerifier;
        private Mock<IRandomizerRepository> mockRandomizerRepository;
        private Mock<ClientIDManager> mockClientIdManager;
        private Guid clientId;

        [SetUp]
        public void Setup()
        {
            mockRandomizerRepository = new Mock<IRandomizerRepository>();
            mockRandomizerVerifier = new Mock<IRandomizerVerifier>();
            mockClientIdManager = new Mock<ClientIDManager>();
            controller = new RandomizersController(mockRandomizerRepository.Object, mockRandomizerVerifier.Object, mockClientIdManager.Object);

            clientId = Guid.NewGuid();
        }

        [Test]
        public void VerifyHandlesGetVerb()
        {
            var attributes = AttributeProvider.GetAttributesFor(controller, "Verify");
            Assert.That(attributes, Contains.Item(typeof(HttpGetAttribute)));
        }

        [Test]
        public void VerifyReturnsJsonResult()
        {
            var result = controller.Verify(clientId, "alignment randomizer type", "class name randomizer type", "level randomizer type", "base race randomizer type", "metarace randomizer type");
            Assert.That(result, Is.InstanceOf<JsonResult>());
        }

        [Test]
        public void VerifyJsonResultAllowsGet()
        {
            var result = controller.Verify(clientId, "alignment randomizer type", "class name randomizer type", "level randomizer type", "base race randomizer type", "metarace randomizer type") as JsonResult;
            Assert.That(result.JsonRequestBehavior, Is.EqualTo(JsonRequestBehavior.AllowGet));
        }

        [Test]
        public void VerifySetsClientId()
        {
            var result = controller.Verify(clientId, "alignment randomizer type", "class name randomizer type", "level randomizer type", "base race randomizer type", "metarace randomizer type") as JsonResult;
            Assert.That(result, Is.InstanceOf<JsonResult>());

            mockClientIdManager.Verify(m => m.SetClientID(It.IsAny<Guid>()), Times.Once);
            mockClientIdManager.Verify(m => m.SetClientID(clientId), Times.Once);
        }

        [Test]
        public void VerifyReturnsPositiveVerification()
        {
            var mockAlignmentRandomizer = new Mock<IAlignmentRandomizer>();
            var mockClassNameRandomizer = new Mock<IClassNameRandomizer>();
            var mockLevelRandomizer = new Mock<ILevelRandomizer>();
            var mockBaseRaceRandomizer = new Mock<RaceRandomizer>();
            var mockMetaraceRandomizer = new Mock<IForcableMetaraceRandomizer>();

            mockRandomizerRepository.Setup(r => r.GetAlignmentRandomizer("alignment randomizer type", "set alignment")).Returns(mockAlignmentRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetClassNameRandomizer("class name randomizer type", "set class name")).Returns(mockClassNameRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetLevelRandomizer("level randomizer type", 9266, false)).Returns(mockLevelRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetBaseRaceRandomizer("base race randomizer type", "set base race")).Returns(mockBaseRaceRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetMetaraceRandomizer("metarace randomizer type", true, "set metarace")).Returns(mockMetaraceRandomizer.Object);

            mockRandomizerVerifier.Setup(g => g.VerifyCompatibility(mockAlignmentRandomizer.Object, mockClassNameRandomizer.Object, mockLevelRandomizer.Object, mockBaseRaceRandomizer.Object, mockMetaraceRandomizer.Object))
                .Returns(true);

            var result = controller.Verify(clientId, "alignment randomizer type", "class name randomizer type", "level randomizer type", "base race randomizer type", "metarace randomizer type", "set alignment", "set class name", 9266, false, "set base race", true, "set metarace") as JsonResult;
            dynamic data = result.Data;
            Assert.That(data.compatible, Is.True);
        }

        [Test]
        public void VerifyReturnsNegativeVerification()
        {
            var mockAlignmentRandomizer = new Mock<IAlignmentRandomizer>();
            var mockClassNameRandomizer = new Mock<IClassNameRandomizer>();
            var mockLevelRandomizer = new Mock<ILevelRandomizer>();
            var mockBaseRaceRandomizer = new Mock<RaceRandomizer>();
            var mockMetaraceRandomizer = new Mock<IForcableMetaraceRandomizer>();

            mockRandomizerRepository.Setup(r => r.GetAlignmentRandomizer("alignment randomizer type", "set alignment")).Returns(mockAlignmentRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetClassNameRandomizer("class name randomizer type", "set class name")).Returns(mockClassNameRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetLevelRandomizer("level randomizer type", 9266, false)).Returns(mockLevelRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetBaseRaceRandomizer("base race randomizer type", "set base race")).Returns(mockBaseRaceRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetMetaraceRandomizer("metarace randomizer type", true, "set metarace")).Returns(mockMetaraceRandomizer.Object);

            mockRandomizerVerifier.Setup(g => g.VerifyCompatibility(mockAlignmentRandomizer.Object, mockClassNameRandomizer.Object, mockLevelRandomizer.Object, mockBaseRaceRandomizer.Object, mockMetaraceRandomizer.Object))
                .Returns(false);

            var result = controller.Verify(clientId, "alignment randomizer type", "class name randomizer type", "level randomizer type", "base race randomizer type", "metarace randomizer type", "set alignment", "set class name", 9266, false, "set base race", true, "set metarace") as JsonResult;
            dynamic data = result.Data;
            Assert.That(data.compatible, Is.False);
        }

        [Test]
        public void DoNotHaveToPassInOptionalParameters()
        {
            var mockAlignmentRandomizer = new Mock<IAlignmentRandomizer>();
            var mockClassNameRandomizer = new Mock<IClassNameRandomizer>();
            var mockLevelRandomizer = new Mock<ILevelRandomizer>();
            var mockBaseRaceRandomizer = new Mock<RaceRandomizer>();
            var mockMetaraceRandomizer = new Mock<IForcableMetaraceRandomizer>();

            mockRandomizerRepository.Setup(r => r.GetAlignmentRandomizer("alignment randomizer type", string.Empty)).Returns(mockAlignmentRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetClassNameRandomizer("class name randomizer type", string.Empty)).Returns(mockClassNameRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetLevelRandomizer("level randomizer type", 0, true)).Returns(mockLevelRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetBaseRaceRandomizer("base race randomizer type", string.Empty)).Returns(mockBaseRaceRandomizer.Object);
            mockRandomizerRepository.Setup(r => r.GetMetaraceRandomizer("metarace randomizer type", false, string.Empty)).Returns(mockMetaraceRandomizer.Object);

            mockRandomizerVerifier.Setup(g => g.VerifyCompatibility(mockAlignmentRandomizer.Object, mockClassNameRandomizer.Object, mockLevelRandomizer.Object, mockBaseRaceRandomizer.Object, mockMetaraceRandomizer.Object))
                .Returns(true);

            var result = controller.Verify(clientId, "alignment randomizer type", "class name randomizer type", "level randomizer type", "base race randomizer type", "metarace randomizer type") as JsonResult;
            dynamic data = result.Data;
            Assert.That(data.compatible, Is.True);
        }
    }
}