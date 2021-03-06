﻿using DnDGen.Web.Helpers;
using DnDGen.Web.Models;
using EncounterGen.Generators;
using EventGen;
using System;
using System.Web.Mvc;

namespace DnDGen.Web.Controllers
{
    public class EncounterController : Controller
    {
        private readonly IEncounterGenerator encounterGenerator;
        private readonly IEncounterVerifier encounterVerifier;
        private readonly ClientIDManager clientIdManager;

        public EncounterController(IEncounterGenerator encounterGenerator, IEncounterVerifier encounterVerifier, ClientIDManager clientIdManager)
        {
            this.encounterGenerator = encounterGenerator;
            this.encounterVerifier = encounterVerifier;
            this.clientIdManager = clientIdManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new EncounterViewModel();
            return View(model);
        }

        [HttpGet]
        public JsonResult Generate(Guid clientId, EncounterSpecifications encounterSpecifications)
        {
            clientIdManager.SetClientID(clientId);

            var encounter = encounterGenerator.Generate(encounterSpecifications);

            foreach (var character in encounter.Characters)
            {
                character.Skills = CharacterHelper.SortSkills(character.Skills);
            }

            return Json(new { encounter = encounter }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Validate(Guid clientId, EncounterSpecifications encounterSpecifications)
        {
            clientIdManager.SetClientID(clientId);
            var isValid = false;

            try
            {
                isValid = encounterVerifier.ValidEncounterExistsAtLevel(encounterSpecifications);
            }
            catch
            {

            }

            return Json(new { isValid = isValid }, JsonRequestBehavior.AllowGet);
        }
    }
}