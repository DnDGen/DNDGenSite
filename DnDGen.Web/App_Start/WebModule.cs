﻿using DnDGen.Web.App_Start.Factories;
using DnDGen.Web.Controllers.Treasures;
using DnDGen.Web.Repositories;
using DnDGen.Web.Repositories.Domain;
using Ninject;
using Ninject.Modules;
using Octokit;
using TreasureGen.Common.Items;
using TreasureGen.Generators.Items.Magical;
using TreasureGen.Generators.Items.Mundane;

namespace DnDGen.Web.App_Start
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ArmorController>().ToMethod(c => new ArmorController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Armor),
                c.Kernel.Get<MundaneItemGenerator>(ItemTypeConstants.Armor)));
            Bind<PotionController>().ToMethod(c => new PotionController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Potion)));
            Bind<RingController>().ToMethod(c => new RingController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Ring)));
            Bind<RodController>().ToMethod(c => new RodController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Rod)));
            Bind<ScrollController>().ToMethod(c => new ScrollController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Scroll)));
            Bind<StaffController>().ToMethod(c => new StaffController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Staff)));
            Bind<ToolController>().ToMethod(c => new ToolController(c.Kernel.Get<MundaneItemGenerator>(ItemTypeConstants.Tool)));
            Bind<AlchemicalItemController>().ToMethod(c => new AlchemicalItemController(c.Kernel.Get<MundaneItemGenerator>(ItemTypeConstants.AlchemicalItem)));
            Bind<WandController>().ToMethod(c => new WandController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Wand)));
            Bind<WeaponController>().ToMethod(c => new WeaponController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.Weapon),
                c.Kernel.Get<MundaneItemGenerator>(ItemTypeConstants.Weapon)));
            Bind<WondrousItemController>().ToMethod(c => new WondrousItemController(c.Kernel.Get<MagicalItemGenerator>(ItemTypeConstants.WondrousItem)));
            Bind<IGitHubClient>().ToMethod(c => GitHubClientFactory.Create());
            Bind<ErrorRepository>().To<GitHubErrorRepository>();
            Bind<RuntimeFactory>().ToMethod(c => new NinjectRuntimeFactory(c.Kernel));
            Bind<IRandomizerRepository>().To<RandomizerRepository>();
        }
    }
}