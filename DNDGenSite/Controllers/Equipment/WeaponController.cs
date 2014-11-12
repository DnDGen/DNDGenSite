﻿using System;
using System.Web.Mvc;
using EquipmentGen.Common;
using EquipmentGen.Common.Items;
using EquipmentGen.Generators.Interfaces.Items.Magical;
using EquipmentGen.Generators.Interfaces.Items.Mundane;

namespace DNDGenSite.Controllers.Equipment
{
    public class WeaponController : EquipmentController
    {
        private IMagicalItemGenerator magicalWeaponGenerator;
        private IMundaneItemGenerator mundaneWeaponGenerator;

        public WeaponController(IMagicalItemGenerator magicalWeaponGenerator, IMundaneItemGenerator mundaneWeaponGenerator)
        {
            this.magicalWeaponGenerator = magicalWeaponGenerator;
            this.mundaneWeaponGenerator = mundaneWeaponGenerator;
        }

        [HttpGet]
        public JsonResult Generate(String power)
        {
            var item = GetWeapon(power);
            var treasure = new Treasure();
            treasure.Items = new[] { item };

            return BuildJsonResult(treasure);
        }

        private Item GetWeapon(String power)
        {
            if (power == PowerConstants.Mundane)
                return mundaneWeaponGenerator.Generate();

            var item = magicalWeaponGenerator.GenerateAtPower(power);

            while (item.ItemType != ItemTypeConstants.Weapon)
                item = magicalWeaponGenerator.GenerateAtPower(power);

            return item;
        }
    }
}