using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    class MoreActors
    {
        internal static EasternDragonAsset easternDragonAsset;
        internal static Dictionary<string, SpecialActorAsset> specialActorAssets = new Dictionary<string, SpecialActorAsset>();
        private static List<int> _color_sets = new List<int>();
        internal BiDictionary<string, string> protoAndYao = new BiDictionary<string, string>();
        internal List<BiDictionary<string, string>> protoAndShengs = new List<BiDictionary<string, string>>();
        internal void init()
        {
            #region 召唤物
            ActorStats summoned = AssetManager.unitStats.clone("summoned", "skeleton");
            summoned.canLevelUp = false;
            summoned.take_items_ignore_range_weapons = true;
            summoned.maxAge = 200;
            summoned.job = "attacker";
            summoned.race = "summoned";
            summoned.procreate = false;
            summoned.useSkinColors = false;
            summoned.shadow = false;
            summoned.traits.Remove("immortal");
            ActorStats summon = AssetManager.unitStats.clone("summon", "summoned");
            ActorStats summoned1 = AssetManager.unitStats.clone("summoned1", "summoned");//仅用于填补旧版本的坑
            ActorStats summonTian = AssetManager.unitStats.clone("summonTian", "summoned");
            summonTian.race = "Tian";
            summonTian.use_items = false;
            summonTian.inspectAvatarScale = 2f;
            summonTian.ignoreTileSpeedMod = true;
            summonTian.actorSize = ActorSize.S17_Dragon;
            summonTian.shadow = true;
            summonTian.ignoreTileSpeedMod = true;
            summonTian.canHaveStatusEffect = false;
            summonTian.shadowTexture = "unitShadow_23";
            summonTian.body_separate_part_head = false;
            summonTian.canReceiveTraits = false;
            summonTian.defaultWeapons = new List<string>();
            summonTian.maxAge = 100;
            summonTian.baseStats.knockbackReduction = 100f;
            summonTian.baseStats.range = 25f;
            summonTian.baseStats.projectiles = 1;
            summonTian.baseStats.scale = 0.3f;
            summonTian.baseStats.speed = 1.5f;
            summonTian.traits.Add("giant");
            summonTian.disablePunchAttackAnimation = true;
            summonTian.texture_path = "summonTian";
            summonTian.flying = true;
            summonTian.very_high_flyer = true;
            summonTian.dieOnBlocks = false;
            summonTian.procreate = false;
            summonTian.defaultAttack = "summonTian";
            addActor(summonTian);
            ActorStats summonTian1 = AssetManager.unitStats.clone("summonTian1", "summoned");
            summonTian1.ignoreTileSpeedMod = true;
            summonTian1.actorSize = ActorSize.S17_Dragon;
            summonTian1.shadow = true;
            summonTian1.shadowTexture = "unitShadow_9";
            summonTian1.maxAge = 499;
            summonTian1.baseStats.knockbackReduction = 100f;
            summonTian1.baseStats.range = 25f;
            summonTian1.baseStats.projectiles = 1;
            summonTian1.baseStats.scale = 0.42f;
            summonTian1.baseStats.speed = 1.5f;
            summonTian1.traits.Add("giant");
            summonTian1.disablePunchAttackAnimation = true;
            summonTian1.texture_path = "summonTian1";
            summonTian1.body_separate_part_head = false;
            summonTian1.swimToIsland = false;
            summonTian1.procreate = false;
            summonTian1.defaultAttack = "summonTian1";
            summonTian1.defaultWeapons = new List<string>();
            summonTian1.use_items = false;
            addActor(summonTian1);
            Main.instance.creatureLimit.Add(summonTian1.id, 1);
            #endregion

            #region 智慧种族

            AssetManager.unitStats.get("unit_human").procreate = true;
            AssetManager.unitStats.get("unit_human").oceanCreature = false;
            AssetManager.unitStats.get("unit_elf").procreate = true;
            AssetManager.unitStats.get("unit_elf").oceanCreature = false;
            AssetManager.unitStats.get("unit_dwarf").procreate = true;
            AssetManager.unitStats.get("unit_dwarf").oceanCreature = false;
            AssetManager.unitStats.get("unit_orc").procreate = true;
            AssetManager.unitStats.get("unit_orc").oceanCreature = false;
            #region 东方人族
            ActorStats EasternHuman = AssetManager.unitStats.clone("unit_EasternHuman", "unit_human");
            EasternHuman.race = "EasternHuman";
            EasternHuman.nameLocale = "EasternHumans";
            EasternHuman.heads = 0;
            EasternHuman.useSkinColors = true;
            EasternHuman.action_death = (WorldAction)Delegate.Combine(EasternHuman.action_death, new WorldAction(ExtensionSpellActionLibrary.aTransformToGod));
            addActor(EasternHuman);
            addColor(EasternHuman, "default", "#FFE599", "#F9CB9C");
            #endregion
            #region 天族
            ActorStats TianAsset = AssetManager.unitStats.clone("Tian", "unit_human");
            TianAsset.maxAge = 300;
            TianAsset.race = "Tian";
            TianAsset.unit = true;
            TianAsset.oceanCreature = false;
            TianAsset.procreate = true;
            TianAsset.nameLocale = "Tians";
            TianAsset.nameTemplate = "Tian_name";
            TianAsset.setBaseStats(120, 17, 30, 4, 5, 100, 3);
            TianAsset.useSkinColors = false;
            TianAsset.texture_path = "t_Tian";
            TianAsset.texture_heads = "";
            TianAsset.heads = 0;
            TianAsset.icon = "Tian";
            TianAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            addActor(TianAsset);
            addActor(AssetManager.unitStats.clone("unit_Tian", "Tian"));
            #region 天族附属
            ActorStats Tian1 = AssetManager.unitStats.clone("summonTian2", "summonTian");
            Tian1.texture_path = "summonTian2";
            Tian1.nameTemplate = "default_name";
            Tian1.race = "Tian";
            Tian1.kingdom = "nomads_Tian";
            Tian1.baseStats.scale = 0.2f;
            Tian1.baseStats.speed = 10f;
            Tian1.traits = new List<string>();
            Tian1.actorSize = ActorSize.S15_Bear;
            Tian1.canLevelUp = true;
            addActor(Tian1);
            #endregion
            #endregion
            #region 冥族
            ActorStats MingAsset = AssetManager.unitStats.clone("Ming", "unit_human");
            MingAsset.maxAge = 300;
            MingAsset.race = "Ming";
            MingAsset.unit = true;
            MingAsset.needFood = true;
            MingAsset.procreate = true;
            MingAsset.oceanCreature = false;
            MingAsset.shadow = false;
            MingAsset.traits.Add("cursed_immune");
            MingAsset.nameLocale = "Mings";
            MingAsset.nameTemplate = "Ming_name";
            MingAsset.setBaseStats(150, 20, 35, 4, 0, 90, 0);
            MingAsset.useSkinColors = false;
            MingAsset.have_skin = false;
            MingAsset.texture_path = "t_Ming";
            MingAsset.texture_heads = "";
            MingAsset.heads = 0;
            MingAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            addActor(MingAsset);
            //addColor(MingAsset);
            addActor(AssetManager.unitStats.clone("unit_Ming", "Ming"));
            //addColor(AssetManager.unitStats.get("unit_Ming"));
            #endregion
            #region 妖族
            ActorStats YaoAsset = AssetManager.unitStats.clone("Yao", "unit_orc");
            YaoAsset.maxAge = 500;
            YaoAsset.race = "Yao";
            YaoAsset.unit = true;
            YaoAsset.needFood = true;
            YaoAsset.procreate = true;
            YaoAsset.damagedByOcean = false;
            YaoAsset.oceanCreature = false;
            YaoAsset.landCreature = true;
            YaoAsset.swimToIsland = true;
            YaoAsset.ignoreTileSpeedMod = true;
            YaoAsset.nameLocale = "Yaos";
            YaoAsset.icon = "iconYao";
            YaoAsset.countAsUnit = true;
            YaoAsset.setBaseStats(150, 20, 35, 4, 0, 90, 0);
            YaoAsset.color = Toolbox.makeColor("#2F5225");
            YaoAsset.useSkinColors = true;
            YaoAsset.body_separate_part_head = false;
            YaoAsset.texture_heads = "";
            YaoAsset.heads = 0;
            YaoAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            YaoAsset.animation_walk = "walk_0,walk_1,walk_2,walk_3";
            YaoAsset.animation_swim = "swim_0,swim_1,swim_2,swim_3";
            YaoAsset.canTurnIntoTumorMonster = false;
            YaoAsset.canTurnIntoMush = false;
            YaoAsset.canTurnIntoZombie = false;
            addColor(YaoAsset);
            addActor(YaoAsset);
            addActor(AssetManager.unitStats.clone("unit_Yao", "Yao"));
            addColor(AssetManager.unitStats.get("unit_Yao"));
            protoAndYao.add(YaoAsset.id, YaoAsset.id);
            #region 妖族附属
            ActorStats _Yao = AssetManager.unitStats.clone("_Yao", "Yao");
            _Yao.unit = false;
            _Yao.kingdom = "nomads_Yao";
            addYao("snake");
            addYao("chicken");
            addYao("bear");
            addYao("buffalo");
            addYao("cat");
            addYao("fox");
            addYao("monkey");
            addYao("crocodile");
            addYao("rhino");
            addYao("cow");
            ActorStats CrabYao = addYao("crab");
            CrabYao.oceanCreature = true;
            CrabYao.landCreature = false;
            addYao("frog");
            addYao("dog");
            addYao("hyena");
            ActorStats PiranhaYao = addYao("piranha");
            PiranhaYao.oceanCreature = true;
            PiranhaYao.landCreature = false;
            addYao("penguin");
            addYao("rat");
            addYao("ratKing");
            addYao("rabbit");
            addYao("turtle");
            addYao("wolf");
            addYao("sheep");
            addYao("cock");
            ActorStats LobsterYao = addYao("lobster");
            LobsterYao.oceanCreature = true;
            LobsterYao.landCreature = false;
            addYao("reindeer");
            #endregion
            #region 妖圣
            protoAndShengs.Add(new BiDictionary<string, string>());
            protoAndShengs.Add(new BiDictionary<string, string>());
            ActorStats YaoSheng = AssetManager.unitStats.clone("YaoSheng", "_Yao");
            YaoSheng.dieOnBlocks = false;
            YaoSheng.dieOnGround = false;
            YaoSheng.procreate = false;
            YaoSheng.canTurnIntoZombie = false;
            YaoSheng.ignoreTileSpeedMod = true;
            YaoSheng.baseStats.range = 15f;
            YaoSheng.use_items = false;
            YaoSheng.baseStats.health = 2000;
            YaoSheng.baseStats.damage = 300;
            YaoSheng.traits.Add("giant");
            //猴子例外
            ActorStats MonkeySheng1 = AssetManager.unitStats.clone("MonkeySheng1", "YaoSheng");
            MonkeySheng1.texture_path = "MonkeySheng1";
            MonkeySheng1.nameTemplate = "monkey_name";
            addColor(MonkeySheng1);
            addActor(MonkeySheng1);
            protoAndShengs[0].add("monkey", MonkeySheng1.id);
            Main.instance.creatureLimit.Add(MonkeySheng1.id, 1);
            ActorStats MonkeySheng2 = AssetManager.unitStats.clone("MonkeySheng2", "MonkeySheng1");
            MonkeySheng2.texture_path = "MonkeySheng2";
            addColor(MonkeySheng2);
            addActor(MonkeySheng2);
            protoAndShengs[1].add("monkey", MonkeySheng2.id);
            Main.instance.creatureLimit.Add(MonkeySheng2.id, 1);
            addYaoSheng("cat");
            addYaoSheng("cow");
            addYaoSheng("snake");
            addYaoSheng("wolf");
            addYaoSheng("chicken");
            #endregion

            #endregion

            #endregion

            #region 其他生物
            //仙狐
            ActorStats FairyFoxAsset = AssetManager.unitStats.clone("FairyFox", "fox");
            FairyFoxAsset.maxAge = 300;
            FairyFoxAsset.race = "fox";
            FairyFoxAsset.needFood = false;
            FairyFoxAsset.nameLocale = "FairyFox";
            FairyFoxAsset.shadowTexture = "unitShadow_5";
            FairyFoxAsset.useSkinColors = false;
            FairyFoxAsset.texture_path = "FairyFox";
            FairyFoxAsset.texture_heads = "";
            FairyFoxAsset.icon = "iconFairyFox";
            FairyFoxAsset.defaultAttack = "snowball";
            FairyFoxAsset.damagedByOcean = false;
            addActor(FairyFoxAsset);
            //福人
            ActorStats FuRenAsset = AssetManager.unitStats.clone("FuRen", "fox");
            FuRenAsset.maxAge = 100;
            FuRenAsset.race = "EasternHuman";
            FuRenAsset.nameLocale = "FuRen";
            FuRenAsset.useSkinColors = false;
            FuRenAsset.texture_path = "FuRen";
            FuRenAsset.texture_heads = "";
            FuRenAsset.icon = "iconFuRen";
            FuRenAsset.defaultAttack = "firework";
            FuRenAsset.use_items = false;
            addActor(FuRenAsset);
            Main.instance.creatureLimit.Add(FuRenAsset.id, 1);
            //虎
            ActorStats TigerAsset = AssetManager.unitStats.clone("Tiger", "wolf");
            TigerAsset.race = "Tiger";
            TigerAsset.nameLocale = "Tiger";
            TigerAsset.nameTemplate = "tiger_name";
            TigerAsset.texture_path = "Tiger";
            TigerAsset.icon = "iconTiger";
            addActor(TigerAsset);
            AssetManager.topTiles.get("grass_low").addUnitsToSpawn("Tiger");
            //东方龙
            ActorStats EasternDragonAsset = AssetManager.unitStats.clone("EasternDragon", "dragon");
            EasternDragonAsset.race = "EasternDragon";
            EasternDragonAsset.actorSize = ActorSize.S17_Dragon;
            EasternDragonAsset.shadowTexture = "unitShadow_23";
            EasternDragonAsset.texture_path = "EasternDragon";
            EasternDragonAsset.texture_heads = "";
            EasternDragonAsset.icon = "iconEasternDragon";
            EasternDragonAsset.skipFightLogic = true;
            EasternDragonAsset.disablePunchAttackAnimation = true;
            EasternDragonAsset.dieOnBlocks = false;
            EasternDragonAsset.dieOnGround = false;
            EasternDragonAsset.baseStats.range = 20f;
            EasternDragonAsset.baseStats.size = 10f;
            EasternDragonAsset.oceanCreature = true;
            EasternDragonAsset.landCreature = true;
            EasternDragonAsset.swampCreature = true;
            EasternDragonAsset.canTurnIntoTumorMonster = false;
            EasternDragonAsset.canTurnIntoMush = false;
            EasternDragonAsset.canTurnIntoZombie = false;
            EasternDragonAsset.traits.Remove("strong_minded");
            addActor(EasternDragonAsset);
            Main.instance.creatureLimit.Add(EasternDragonAsset.id, 1);
            //各路神明
            ActorStats Achelous = AssetManager.unitStats.clone("Achelous", "wolf");
            Achelous.maxAge = 0;
            Achelous.race = "EasternHuman";
            Achelous.needFood = false;
            Achelous.canTurnIntoMush = false;
            Achelous.canTurnIntoZombie = false;
            Achelous.canTurnIntoTumorMonster = false;
            Achelous.countAsUnit = true;
            Achelous.kingdom = "nomads_EasternHuman";
            Achelous.procreate = false;
            Achelous.ignoreJobs = true;
            Achelous.useSkinColors = false;
            Achelous.use_items = false;
            Achelous.texture_path = "Achelous";
            Achelous.icon = "Achelous";
            addActor(Achelous);
            Main.instance.creatureLimit.Add(Achelous.id, 1);
            Main.instance.godList.Add(Achelous.id, "河伯");
            ActorStats EarthGod = AssetManager.unitStats.clone("EarthGod", "Achelous");
            EarthGod.texture_path = "EarthGod";
            EarthGod.icon = "EarthGod";
            addActor(EarthGod);
            Main.instance.creatureLimit.Add(EarthGod.id, 1);
            Main.instance.godList.Add(EarthGod.id, "土地");
            ActorStats Mammon = AssetManager.unitStats.clone("Mammon", "Achelous");
            Mammon.texture_path = "Mammon";
            Mammon.icon = "Mammon";
            addActor(Mammon);
            Main.instance.creatureLimit.Add(Mammon.id, 1);
            Main.instance.godList.Add(Mammon.id, "财神");
            ActorStats Hymen = AssetManager.unitStats.clone("Hymen", "Achelous");
            Hymen.texture_path = "Hymen";
            Hymen.icon = "Hymen";
            addActor(Hymen);
            Main.instance.creatureLimit.Add(Hymen.id, 1);
            Main.instance.godList.Add(Hymen.id, "月老");
            ActorStats MountainGod = AssetManager.unitStats.clone("MountainGod", "Achelous");
            MountainGod.texture_path = "MountainGod";
            MountainGod.icon = "MountainGod";
            addActor(MountainGod);
            Main.instance.creatureLimit.Add(MountainGod.id, 1);
            Main.instance.godList.Add(MountainGod.id, "山神");
            ActorStats ZhongKui = AssetManager.unitStats.clone("ZhongKui", "Achelous");
            ZhongKui.texture_path = "ZhongKui";
            ZhongKui.icon = "ZhongKui";
            addActor(ZhongKui);
            Main.instance.creatureLimit.Add(ZhongKui.id, 1);
            Main.instance.godList.Add(ZhongKui.id, "钟馗");
            #endregion

            #region BOSS
            ActorStats JiaoDragonAsset = AssetManager.unitStats.clone("JiaoDragon", "wolf");
            JiaoDragonAsset.maxAge = 1000;
            JiaoDragonAsset.race = "JiaoDragon";
            JiaoDragonAsset.kingdom = "boss";
            JiaoDragonAsset.needFood = false;
            JiaoDragonAsset.nameLocale = "JiaoDragon";
            JiaoDragonAsset.nameTemplate = "default_name";
            JiaoDragonAsset.shadowTexture = "unitShadow_5";
            JiaoDragonAsset.useSkinColors = false;
            JiaoDragonAsset.texture_path = "JiaoDragon";
            JiaoDragonAsset.texture_heads = "";
            JiaoDragonAsset.canTurnIntoZombie = false;
            JiaoDragonAsset.canTurnIntoMush = false;
            JiaoDragonAsset.canTurnIntoTumorMonster = false;
            JiaoDragonAsset.icon = "iconJiaoDrangon";
            JiaoDragonAsset.inspectAvatarScale = 0.35f;
            JiaoDragonAsset.setBaseStats(1000000, 10000, 0, 99, 100, 100);
            JiaoDragonAsset.defaultAttack = "snowball";
            JiaoDragonAsset.damagedByOcean = false;
            JiaoDragonAsset.actorSize = ActorSize.S17_Dragon;
            JiaoDragonAsset.baseStats.areaOfEffect = 5f;
            JiaoDragonAsset.baseStats.range = 30f;
            JiaoDragonAsset.baseStats.scale = 0.4f;
            JiaoDragonAsset.baseStats.size = 0.5f;
            JiaoDragonAsset.baseStats.projectiles = 20;
            JiaoDragonAsset.baseStats.knockback = 100f;
            JiaoDragonAsset.baseStats.knockbackReduction = 100f;
            JiaoDragonAsset.procreate = false;
            JiaoDragonAsset.disablePunchAttackAnimation = true;
            addActor(JiaoDragonAsset);

            ActorStats XieDragonAsset = AssetManager.unitStats.clone("XieDragon", "JiaoDragon");
            XieDragonAsset.nameLocale = "XieDragon";
            XieDragonAsset.nameTemplate = "default_name";
            XieDragonAsset.shadowTexture = "unitShadow_5";
            XieDragonAsset.texture_path = "XieDragon";
            XieDragonAsset.icon = "iconXieDrangon";
            XieDragonAsset.setBaseStats(1000000, 10000, 0, 99, 100, 100);
            XieDragonAsset.baseStats.scale = 0.20f;
            XieDragonAsset.baseStats.size = 0.5f;
            addActor(XieDragonAsset);

            ActorStats NianAsset = AssetManager.unitStats.clone("Nian", "EasternDragon");
            NianAsset.race = "Nian";
            NianAsset.kingdom = "boss";
            NianAsset.texture_path = "Nian";
            NianAsset.icon = "iconNian";
            NianAsset.baseStats.range = 23f;
            NianAsset.oceanCreature = false;
            NianAsset.action_death = (WorldAction)Delegate.Combine(NianAsset.action_death, new WorldAction(ExtensionSpellActionLibrary.aNianDie));
            addActor(NianAsset);
            Main.instance.creatureLimit.Add(NianAsset.id, 1);
            #endregion

            #region 彩蛋
            ActorStats MengZhuAsset = AssetManager.unitStats.clone("MengZhu", "wolf");
            MengZhuAsset.maxAge = 1000;
            MengZhuAsset.race = "MengZhu";
            MengZhuAsset.kingdom = "good";
            MengZhuAsset.needFood = false;
            MengZhuAsset.nameLocale = "MengZhu";
            MengZhuAsset.nameTemplate = "MengZhu_name";
            MengZhuAsset.shadowTexture = "unitShadow_5";
            MengZhuAsset.useSkinColors = false;
            MengZhuAsset.inspect_home = false;
            MengZhuAsset.inspect_children = false;
            MengZhuAsset.inspectAvatarScale = 0.5f;
            MengZhuAsset.ignoreTileSpeedMod = true;
            MengZhuAsset.canTurnIntoZombie = false;
            MengZhuAsset.canTurnIntoMush = false;
            MengZhuAsset.canTurnIntoTumorMonster = false;
            MengZhuAsset.needFood = false;
            MengZhuAsset.texture_path = "MengZhu";
            MengZhuAsset.texture_heads = "";
            MengZhuAsset.icon = "iconMengZhu";
            MengZhuAsset.traits = new List<string> { "immortal", "cursed_immune", "fire_proof", "freeze_proof", "poison_immune", "immune", "healing_aura" };
            MengZhuAsset.attack_spells = new List<string> { "bloodRain" };
            MengZhuAsset.setBaseStats(5000000, 10000, 15, 99, 100, 100);
            MengZhuAsset.defaultAttack = "snowball";
            MengZhuAsset.damagedByOcean = false;
            MengZhuAsset.procreate = false;
            MengZhuAsset.disablePunchAttackAnimation = true;
            MengZhuAsset.baseStats.areaOfEffect = 5f;
            MengZhuAsset.baseStats.range = 30f;
            MengZhuAsset.baseStats.scale = 0.02f;
            MengZhuAsset.actorSize = ActorSize.S13_Human;
            MengZhuAsset.baseStats.projectiles = 20;
            MengZhuAsset.baseStats.knockback = 100f;
            MengZhuAsset.baseStats.knockbackReduction = 100f;
            addActor(MengZhuAsset);
            #endregion

            #region 各种幼崽
            ActorStats babyEasternHuman = AssetManager.unitStats.clone("baby_EasternHuman", "unit_EasternHuman");
            babyEasternHuman.timeToGrow = 200;
            babyEasternHuman.baby = true;
            babyEasternHuman.growIntoID = "unit_EasternHuman";
            addActor(babyEasternHuman);
            addColor(babyEasternHuman);
            ActorStats babyTian = AssetManager.unitStats.clone("baby_Tian", "unit_Tian");
            babyTian.timeToGrow = 200;
            babyTian.baby = true;
            babyTian.growIntoID = "unit_Tian";
            addActor(babyTian);
            //addColor(babyTian);
            ActorStats babyMing = AssetManager.unitStats.clone("baby_Ming", "unit_Ming");
            babyMing.timeToGrow = 200;
            babyMing.baby = true;
            babyMing.growIntoID = "unit_Ming";
            addActor(babyMing);
            //addColor(babyMing);
            ActorStats babyYao = AssetManager.unitStats.clone("baby_Yao", "unit_Yao");
            babyYao.timeToGrow = 200;
            babyYao.baby = true;
            babyYao.growIntoID = "unit_Yao";
            addActor(babyYao);
            addColor(babyYao);
            #endregion
            initEasternDragonAsset();
            initOthersSpecialActorAssets();
        }
        private ActorStats addYao(string id)
        {
            string tranformID = ActorTools.getTranformID(id);
            ActorStats pStats = AssetManager.unitStats.clone(tranformID, "_Yao");
            pStats.texture_path = tranformID;
            pStats.nameTemplate = id + "_name";
            addColor(pStats);
            addActor(pStats);
            protoAndYao.add(id, tranformID);
            return pStats;
        }
        private void addYaoSheng(string id)
        {
            string tranformID = ActorTools.getTranformID(id).Replace("Yao", "Sheng");
            ActorStats pStats = AssetManager.unitStats.clone(tranformID, "YaoSheng");
            pStats.texture_path = tranformID;
            pStats.nameTemplate = id + "_name";
            pStats.useSkinColors = false;
            addActor(pStats);
            protoAndShengs[0].add(id, tranformID);
            Main.instance.creatureLimit.Add(tranformID, 1);
        }
        private void addColor(ActorStats pStats, string pID = "default", string from = "#FFC984", string to = "#543E2C")
        {
            pStats.useSkinColors = true;
            if (pStats.color_sets == null)
            {
                pStats.color_sets = new List<ColorSet>();
            }
            ColorSet colorSet = new ColorSet();
            colorSet.id = pID;
            pStats.color_sets.Add(colorSet);
            Color pFrom = Toolbox.makeColor(from);
            Color pTo = Toolbox.makeColor(to);
            int num = 5;
            float num2 = 1f / (float)(num - 1);
            for (int i = 0; i < num; i++)
            {
                float num3 = 1f - (float)i * num2;
                if (num3 > 1f)
                {
                    num3 = 1f;
                }
                Color c = Toolbox.blendColor(pFrom, pTo, num3);
                colorSet.colors.Add(c);
            }
        }
        private void addActor(ActorStats pStats)
        {
            Main.instance.moreActors.Add(pStats.id);
            if (pStats.shadow)
            {
                Reflection.CallMethod(AssetManager.unitStats, "loadShadow", pStats);
            }
            /*
			 * 用于自动添加命名，复制人类的命名
			((ChineseNameLibrary)AssetManager.instance.dict["chineseNameGenerator"]).clone(pStats.nameTemplate, "human_name");
			*/
        }
        private void initEasternDragonAsset()
        {
            easternDragonAsset = new EasternDragonAsset();
            easternDragonAsset.list = new EasternDragonAssetContainer[11];
            Sprite[] moveDragon = Utils.ResourcesHelper.loadAllSprite("actors/EasternDragon/dragon/Move", 0.5f);
            Sprite[] moveHuman = Utils.ResourcesHelper.loadAllSprite("actors/EasternDragon/human/Move", 0.5f);
            Sprite[] stopHuman = Utils.ResourcesHelper.loadAllSprite("actors/EasternDragon/human/stop", 0.5f);
            Sprite[] upDragon = Utils.ResourcesHelper.loadAllSprite("actors/EasternDragon/human/up", 0.5f);
            Sprite[] landingHuman = Utils.ResourcesHelper.loadAllSprite("actors/EasternDragon/dragon/Landing", 0.5f);
            Sprite[] attackDragon = Utils.ResourcesHelper.loadAllSprite("actors/EasternDragon/dragon/attack", 0.5f);
            #region 龙形态
            //龙移动
            EasternDragonAssetContainer container1 = new EasternDragonAssetContainer();
            container1.frames = moveDragon;
            container1.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Move,
                shape = EasternDragonState.Shape.Dragon
            };
            easternDragonAsset.list[0] = container1;
            //龙停止
            EasternDragonAssetContainer container2 = new EasternDragonAssetContainer();
            container2.frames = moveDragon;
            container2.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Stop,
                shape = EasternDragonState.Shape.Dragon
            };
            easternDragonAsset.list[1] = container2;
            //龙攻击
            EasternDragonAssetContainer container3 = new EasternDragonAssetContainer();
            container3.frames = moveDragon;
            container3.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Attack,
                shape = EasternDragonState.Shape.Dragon
            };
            easternDragonAsset.list[2] = container3;
            //龙死亡
            EasternDragonAssetContainer container4 = new EasternDragonAssetContainer();
            container4.frames = moveDragon;
            container4.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Death,
                shape = EasternDragonState.Shape.Dragon
            };
            easternDragonAsset.list[3] = container4;
            //化龙
            EasternDragonAssetContainer container5 = new EasternDragonAssetContainer();
            container5.frames = upDragon;
            container5.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Up,
                shape = EasternDragonState.Shape.Dragon
            };
            easternDragonAsset.list[4] = container5;
            //龙施法
            EasternDragonAssetContainer container11 = new EasternDragonAssetContainer();
            container11.frames = attackDragon;
            container11.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Spell,
                shape = EasternDragonState.Shape.Dragon
            };
            easternDragonAsset.list[10] = container11;
            #endregion
            #region 人形态
            //化人
            EasternDragonAssetContainer container6 = new EasternDragonAssetContainer();
            container6.frames = landingHuman;
            container6.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Landing,
                shape = EasternDragonState.Shape.Human
            };
            easternDragonAsset.list[5] = container6;
            //人移动
            EasternDragonAssetContainer container7 = new EasternDragonAssetContainer();
            container7.frames = moveHuman;
            container7.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Move,
                shape = EasternDragonState.Shape.Human
            };
            easternDragonAsset.list[6] = container7;
            //人停止
            EasternDragonAssetContainer container8 = new EasternDragonAssetContainer();
            container8.frames = stopHuman;
            container8.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Stop,
                shape = EasternDragonState.Shape.Human
            };
            easternDragonAsset.list[7] = container8;
            //人攻击
            EasternDragonAssetContainer container9 = new EasternDragonAssetContainer();
            container9.frames = stopHuman;
            container9.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Attack,
                shape = EasternDragonState.Shape.Human
            };
            easternDragonAsset.list[8] = container9;
            //人死亡
            EasternDragonAssetContainer container10 = new EasternDragonAssetContainer();
            container10.frames = stopHuman;
            container10.id = new EasternDragonState()
            {
                actionState = EasternDragonState.ActionState.Death,
                shape = EasternDragonState.Shape.Human
            };
            easternDragonAsset.list[9] = container10;
            #endregion

        }
        private void initOthersSpecialActorAssets()
        {
            SpecialActorAsset Nian = new SpecialActorAsset();

            MoreActors.specialActorAssets.Add("Nian", Nian);
            #region 年兽
            Nian.list = new SpecialActorAssetContainer[5];
            Sprite[] stopNianSprites = Utils.ResourcesHelper.loadAllSprite("actors/Nian/stop", 0.5f, 0);
            Sprite[] moveNianSprites = Utils.ResourcesHelper.loadAllSprite("actors/Nian/move", 0.5f, 0);
            Sprite[] attackNianSprites = Utils.ResourcesHelper.loadAllSprite("actors/Nian/attack", 0.5f, 0);
            SpecialActorAssetContainer stopNian = new SpecialActorAssetContainer();
            stopNian.id = SpecialActorState.Stop;
            stopNian.frames = stopNianSprites;
            Nian.list[0] = stopNian;

            SpecialActorAssetContainer moveNian = new SpecialActorAssetContainer();
            moveNian.id = SpecialActorState.Move;
            moveNian.frames = moveNianSprites;
            Nian.list[1] = moveNian;

            SpecialActorAssetContainer attackNian = new SpecialActorAssetContainer();
            attackNian.id = SpecialActorState.Attack;
            attackNian.frames = attackNianSprites;
            Nian.list[2] = attackNian;

            SpecialActorAssetContainer deathNian = new SpecialActorAssetContainer();
            deathNian.id = SpecialActorState.Death;
            deathNian.frames = stopNianSprites;
            Nian.list[3] = deathNian;

            SpecialActorAssetContainer spellNian = new SpecialActorAssetContainer();
            spellNian.id = SpecialActorState.Spell;
            spellNian.frames = stopNianSprites;
            Nian.list[4] = spellNian;
            #endregion
        }

        #region 拦截
        //龙的攻击逻辑
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "canFight")]
        public static bool canFight_Prefix(Actor __instance, ref bool __result)
        {
            if (__instance.stats.id == "EasternDragon" && __instance.GetComponent<EasternDragon>().getState().shape == EasternDragonState.Shape.Human)
            {
                __result = true;
                return false;
            }
            return true;
        }
        //龙的生成，暂时采用此处拦截
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "addChildren")]
        public static void addChildren_Postfix(Actor __instance)
        {
            if (__instance.stats.id == "EasternDragon")
            {
                __instance.gameObject.AddComponent<EasternDragon>();
                Reflection.SetField(__instance, "children_special", new List<BaseActorComponent>() { __instance.GetComponent<EasternDragon>() });
                __instance.GetComponent<EasternDragon>().create();
            }
            else if(__instance.stats.id == "Nian")
            {
                __instance.gameObject.AddComponent<SpecialActor>();
                Reflection.SetField(__instance, "children_special", new List<BaseActorComponent>() { __instance.GetComponent<SpecialActor>() });
                __instance.GetComponent<SpecialActor>().create();
            }
        }
        //释放法术
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "applyAttack")]
        public static bool useSpell(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            if (pAttacker.objectType != MapObjectType.Actor)
            {
                return true;
            }
            ActorStatus dataA = ((Actor)pAttacker).GetData();
            ExtendedActor pa = (ExtendedActor)pAttacker;
            if (pa.extendedCurStats.spells.Count != 0)
            {
                int count = pa.extendedCurStats.spells.Count;
                int max = dataA.level / 20 + 1;
                int[] index = new int[count];
                for (int i = 0; i < count; i++)
                {
                    index[i] = i;
                }
                index.Shuffle();
                for (int i = 0, num = 0; i < count && num < max; i++)
                {
                    if (!pTarget.base_data.alive)
                    {
                        return true;
                    }
                    ExtensionSpell spell = pa.extendedCurStats.spells[index[i]];
                    //进行蓝耗和冷却检查
                    if (spell.leftCool == 0 && pa.extendedData.status.magic >= spell.cost
                        && spell.GetSpellAsset().type.attacking && spell.GetSpellAsset().type.requiredLevel <= dataA.level)
                    {
                        if (spell.castSpell(pAttacker, pTarget))
                        {
                            spell.leftCool = spell.cooldown;
                            pa.extendedData.status.magic -= spell.cost;
                            num++;
                        }
                    }
                }

            }
            return true;
        }
        //攻击距离判定修改（并入法术距离判定
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "isInAttackRange")]
        public static void isInAttackRange_Postfix(ref bool __result, Actor __instance, BaseSimObject pObject)
        {
            if (__result)
            {
                return;
            }
            MoreStatus moredata = ((ExtendedActor)__instance).extendedData.status;
            MoreStats morestats =((ExtendedActor)__instance).extendedCurStats;
            float rangeLimit = Mathf.Max(__instance.GetCurStats().range, morestats.spellRange) + ((BaseStats)Reflection.GetField(typeof(BaseSimObject), pObject, "curStats")).size;
            foreach (ExtensionSpell spell in morestats.spells)
            {
                if (((ExtendedActor)__instance).canCastSpell(spell)&& spell.GetSpellAsset().type.attacking)
                {
                    if (Toolbox.DistVec3(__instance.currentPosition, pObject.currentPosition) < rangeLimit)
                    {
                        __result = true;
                        return;
                    }
                }
            }
        }
        //经验条修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getExpToLevelup")]
        public static bool getExpToLevelUp(Actor __instance, ref int __result)
        {
            ActorStatus data = __instance.GetData();
            if (data == null)
            {
                __result = int.MaxValue;
                return false;
            }
            ChineseElement element = ((ExtendedActor)__instance).extendedCurStats.element;
            __result = (int)((100 + (data.level - 1) * (data.level - 1) * 50) *element.getImPurity()/element.GetAsset().rarity);
            return false;
        }
        //升级修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "addExperience")]
        public static bool addExperiece_Prefix(Actor __instance, int pValue)
        {
            if (__instance == null || !__instance.GetData().alive)
            {
                return true;
            }
            StackTrace st = new StackTrace();
            for (int i = 2; i < 5; i++)
            {
                if (st.GetFrame(i).GetMethod().Name == "applyAttack")
                {
                    return false;
                }
            }
            ActorStatus data = __instance.GetData();
            ExtendedActor actor = (ExtendedActor)__instance;
            if (!actor.extendedData.status.canCultivate)
            {
                return false;
            }
            int num = 110;
            data.experience += pValue;

            if (data.experience >= __instance.getExpToLevelup())
            {
                //回蓝，回冷却
                actor.extendedData.status.magic = actor.extendedCurStats.magic;//待调整与元素相关
                foreach (ExtensionSpell spell in actor.extendedCurStats.spells)
                {
                    spell.leftCool -= spell.leftCool > 0 ? 1 : 0;
                }
                __instance.setStatsDirty();
            }
            else
            {
                return false;
            }
            //如果等级达到上限，或者该生物不能升级，则优化灵根
            if (data.level >= num || !__instance.stats.canLevelUp)
            {
                while (data.experience >= __instance.getExpToLevelup())
                {
                    data.experience -= __instance.getExpToLevelup();
                    ChineseElement element1 = new ChineseElement();
                    if (element1.getImPurity() < actor.extendedCurStats.element.getImPurity())
                    {
                        actor.extendedCurStats.element = element1;
                    }
                }
            }
            while (data.experience >= __instance.getExpToLevelup() && data.level < num)
            {
                //缺少其他升级条件
                data.experience -= __instance.getExpToLevelup();
                data.level++;
                //准备雷劫
                if ((data.level - 1) % 10 == 0)
                {
                    if(!PowerActionLibrary.lightningPunishment(__instance))
                    {
                        return false;
                    }
                }
                #region 升级福利
                data.health = int.MaxValue >> 4;
                int realm = data.level;
                if (realm > 10)
                {
                    if (data.level == 50)
                    {
                        __instance.tryToUnite();
                    }
                    else if (data.level == 110)
                    {
                        __instance.generateNewBody();
                    }
                    realm = (realm + 9) / 10 + 9;
                }
                //家族升级
                Family family = Main.instance.familys[actor.extendedData.status.familyID];
                if (data.level > family.maxLevel)
                {
                    int count = 0;
                    while (count < family.maxLevel / 10)
                    {
                        ChineseElement element1 = new ChineseElement();
                        if (element1.getImPurity() < actor.extendedCurStats.element.getImPurity())
                        {
                            actor.extendedCurStats.element = element1;
                            break;
                        }
                        count++;
                    }
                    family.levelUp(data.firstName);
                }
                else if (data.level == family.maxLevel)
                {
                    ChineseElement element1 = new ChineseElement();
                    if (element1.getImPurity() < actor.extendedCurStats.element.getImPurity())
                    {
                        actor.extendedCurStats.element = element1;
                    }
                }
                //国家和城市领导人变化
                if (__instance.kingdom == null)
                {
                    continue;
                }
                if (__instance.kingdom.king != null)
                {
                    if (data.level > __instance.kingdom.king.GetData().level)
                    {
                        Actor lastKing = __instance.kingdom.king;
                        __instance.kingdom.setKing(__instance);
                        foreach (City city in __instance.kingdom.cities)
                        {
                            if (city.leader == null)
                            {
                                City.makeLeader(lastKing, city);
                                break;
                            }
                            else
                            {
                                if (city.leader.GetData().level <= lastKing.GetData().level)
                                {
                                    City.makeLeader(lastKing, city);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (data.level == 5)
                {
                    __instance.tryTransform();
                    return false;
                }
                if (data.level == 110 && __instance.stats.race == "Yao")
                {
                    __instance.tryToUnite();
                }
            }

            //法术释放
            string actorID = data.actorID;
            int level = data.level;
            foreach (ExtensionSpell spell in actor.extendedCurStats.spells)
            {
                if (__instance != null && data != null && data.alive &&
                    spell.GetSpellAsset().type.levelUp&& spell.GetSpellAsset().type.requiredLevel <= level)
                {
                    spell.castSpell(__instance, __instance);
                    break;
                }
            }
            #endregion


            return false;
        }
        //境界压制
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getHit")]
        public static bool getHit_Prefix(Actor __instance, ref float pDamage, AttackType pType = AttackType.None, BaseSimObject pAttacker = null)
        {
            if (__instance == null)
            {
                return true;
            }
            ActorStatus data1 = __instance.GetData();
            if (!data1.alive)
            {
                return true;
            }
            if (__instance.haveTrait("asylum"))
            {
                return false;
            }
            if (pType == AttackType.None)
            {
                if (__instance.GetCurStats().armor != 0)
                {
                    pDamage /= 1 - __instance.GetCurStats().armor / 100f;
                }
                return true;
            }//采用无类型伤害作为真伤判断
            if (pAttacker == null)
            {
                pDamage *= 1f - data1.level * 0.1f;
                return true;
            }
            if (__instance == pAttacker)
            {
                return false;
            }

            //人对人伤害增益
            if (pAttacker.objectType == MapObjectType.Actor)
            {
                ActorStatus data2 = ((Actor)pAttacker).GetData();
                if (data2.level <= data1.level - 10)
                {
                    return false;
                }
                if (data2.level < data1.level)
                {
                    pDamage *= 1 - (data1.level - data2.level + 1) * data1.level / 100f;
                }
                if (pAttacker.kingdom != null && pAttacker.kingdom.raceID == "EasternHuman"&&__instance.kingdom!=null)
                {
                    switch (__instance.kingdom.raceID)
                    {
                        case "Ming":
                            if (KingdomAndCityTools.checkZhongKui((Actor)pAttacker))
                                pDamage *= 1.5f;
                            break;
                        case "Yao":
                            if (KingdomAndCityTools.checkZhongKui((Actor)pAttacker))
                                pDamage *= 1.2f;
                            break;
                        case "Tian":
                            if (KingdomAndCityTools.checkZhongKui((Actor)pAttacker))
                                pDamage *= 1.1f;
                            break;
                    }
                }
            }

            //人对塔伤害减免暂时去除
            //else
            //{
            //    pDamage *= 1f - data1.level * 0.005f;
            //}
            return true;
        }
        //每年修炼经验修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "updateAge")]
        public static bool updateAge(Actor __instance)
        {
            if (__instance == null)
            {
                return true;
            }
            ActorStatus data = __instance.GetData();
            ExtendedActor actor = (ExtendedActor)__instance;
            if (!updateAge(AssetManager.raceLibrary.get(__instance.stats.race), data, __instance) && !__instance.haveTrait("immortal"))
            {
                __instance.killHimself(false, AttackType.Age, true, true);
                return false;
            }
            //回蓝，回冷却
            actor.extendedData.status.magic += data.level;//待调整与元素相关
            if (actor.extendedData.status.magic > actor.extendedCurStats.magic + 1)
            {
                actor.extendedData.status.magic = actor.extendedCurStats.magic + 1;
            }
            foreach (ExtensionSpell spell in actor.extendedCurStats.spells)
            {
                spell.leftCool -= spell.leftCool > 0 ? 1 : 0;
            }
            ChineseElement chunkElement = Main.instance.chunkToElement[__instance.currentTile.chunk.id];
            ChineseElement actorElement = actor.extendedCurStats.element;

            float exp = 5 + __instance.GetFamily().cultivationBook.rank;
            float mod = 0f;
            for (int i = 0; i < 5; i++)
            {
                int temp = chunkElement.baseElementContainer[i] % 100;
                mod += (temp * (actorElement.baseElementContainer[i] + 1)) / 1000f;
            }
            exp *= mod * __instance.GetSpecialBody().rank;
            addExperiece_Prefix(__instance, (int)exp);

            if (data.age > 100 && Toolbox.randomChance(0.03f))
            {
                __instance.addTrait("wise");
            }
            return false;
        }
        //属性实现
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ActorBase), "updateStats")]
        public static IEnumerable<CodeInstruction> updateStats_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            #region 绑定函数
            MethodInfo getCity = AccessTools.Method(typeof(ActorTools), "GetCity");
            MethodInfo getCurStats = AccessTools.Method(typeof(ActorTools), "GetCurStats");
            MethodInfo getBsFromMoreStats = AccessTools.Method(typeof(MoreStats), "GetBaseStats");
            MethodInfo part1 = AccessTools.Method(typeof(ActorTools), "dealStatsHelper1");
            MethodInfo part2 = AccessTools.Method(typeof(ActorTools), "dealStatsHelper2");
            MethodInfo addStats = AccessTools.Method(typeof(BaseStats), "addStats", new System.Type[] { typeof(BaseStats) });

            FieldInfo extendedCurStats = AccessTools.Field(typeof(ExtendedActor), "extendedCurStats");
            //MethodInfo addStats = typeof(BaseStats).GetMethod("addStats", BindingFlags.Instance | BindingFlags.NonPublic);
            #endregion
            #region 属性添加处理
            int offset = 0;
            codes.Insert(56 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(56 + offset, new CodeInstruction(OpCodes.Callvirt, part1));
            offset++;//执行part1函数(done)
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Callvirt, getCurStats));
            offset++;//获取并将CurStats压入栈
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldfld, extendedCurStats));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Callvirt, getBsFromMoreStats));
            offset++;//获取MoreStats的BaseStats并压入栈
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Callvirt, addStats));
            offset++;//两者相加
            #endregion
            #region 性格设置（为妖族
            codes[224 + offset] = new CodeInstruction(OpCodes.Callvirt, getCity);
            codes[225 + offset] = new CodeInstruction(OpCodes.Nop);
            #endregion
            #region 属性规格化处理
            codes.Insert(728 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(728 + offset, new CodeInstruction(OpCodes.Callvirt, part2));
            offset++;
            #endregion
            return codes;
        }
        //寿命实现
        public static bool updateAge(Race pRace, ActorStatus pData, Actor pActor)
        {
            pData.age++;
            ActorStats actorStats = AssetManager.unitStats.get(pData.statsID);
            pData.CallMethod("updateAttributes", actorStats, pRace, false);
            if (!MapBox.instance.worldLaws.world_law_old_age.boolVal)
            {
                return true;
            }
            int num = actorStats.maxAge;
            MoreStats morestats = ((ExtendedActor)pActor).extendedCurStats;
            if (morestats.maxAge == 0 && pData.level > 1)
            {
                pActor.CallMethod("updateStats");
            }
            if (!actorStats.id.StartsWith("summon"))
            {
                num += morestats.maxAge;
                Culture culture = MapBox.instance.cultures.get(pData.culture);
                if (culture != null)
                {
                    num += culture.getMaxAgeBonus();
                }
            }
            return actorStats.maxAge == 0 || num > pData.age || !Toolbox.randomChance(0.15f) || pActor.haveTrait("asylum");
        }
        //人物窗口
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WindowCreatureInfo), "OnEnable")]
        public static bool windowCreatureInfo(WindowCreatureInfo __instance)
        {
            if (Config.selectedUnit == null)
            {
                return false;
            }
            Actor selectedUnit = Config.selectedUnit;
            BaseStats curStats = selectedUnit.GetCurStats();
            ActorStatus data = selectedUnit.GetData();
            WindowCreatureInfoHelper helper = new WindowCreatureInfoHelper();
            helper.instance = __instance;
            __instance.nameInput.setText(data.firstName);
            __instance.health.setBar((float)data.health, (float)curStats.health, data.health.ToString() + "/" + curStats.health.ToString());
            if (selectedUnit.stats.needFood || selectedUnit.stats.unit)
            {
                __instance.hunger.gameObject.SetActive(true);
                int num = (int)((float)data.hunger / (float)selectedUnit.stats.maxHunger * 100f);
                __instance.hunger.setBar((float)num, 100f, num.ToString() + "%");
            }
            else
            {
                __instance.hunger.gameObject.SetActive(false);
            }
            __instance.damage.gameObject.SetActive(true);
            __instance.armor.gameObject.SetActive(true);
            __instance.speed.gameObject.SetActive(true);
            __instance.attackSpeed.gameObject.SetActive(true);
            __instance.crit.gameObject.SetActive(true);
            __instance.diplomacy.gameObject.SetActive(true);
            __instance.warfare.gameObject.SetActive(true);
            __instance.stewardship.gameObject.SetActive(true);
            __instance.intelligence.gameObject.SetActive(true);
            if (!selectedUnit.stats.unit)
            {
                __instance.diplomacy.gameObject.SetActive(false);
                __instance.warfare.gameObject.SetActive(false);
                __instance.stewardship.gameObject.SetActive(false);
                __instance.intelligence.gameObject.SetActive(false);
            }
            if (!selectedUnit.stats.inspect_stats)
            {
                __instance.damage.gameObject.SetActive(false);
                __instance.armor.gameObject.SetActive(false);
                __instance.speed.gameObject.SetActive(false);
                __instance.diplomacy.gameObject.SetActive(false);
                __instance.attackSpeed.gameObject.SetActive(false);
                __instance.crit.gameObject.SetActive(false);
            }
            __instance.damage.text.text = curStats.damage.ToString() ?? "";
            __instance.armor.text.text = curStats.armor.ToString() + "%";
            __instance.speed.text.text = curStats.speed.ToString() ?? "";
            __instance.crit.text.text = curStats.crit.ToString() + "%";
            __instance.attackSpeed.text.text = curStats.attackSpeed.ToString() ?? "";
            helper.showAttribute(__instance.diplomacy.text, curStats.diplomacy);
            helper.showAttribute(__instance.stewardship.text, curStats.stewardship);
            helper.showAttribute(__instance.intelligence.text, curStats.intelligence);
            helper.showAttribute(__instance.warfare.text, curStats.warfare);
            Sprite sprite = (Sprite)Resources.Load("ui/Icons/" + selectedUnit.stats.icon, typeof(Sprite));
            __instance.icon.sprite = sprite;
            __instance.avatarLoader.load(selectedUnit);
            if (selectedUnit.stats.hideFavoriteIcon)
            {
                __instance.iconFavorite.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                __instance.iconFavorite.transform.parent.gameObject.SetActive(true);
            }
            __instance.text_description.text = "";
            __instance.text_values.text = "";
            Color32 colorAge = Color.green;
            int maxAge = AssetManager.unitStats.get(data.statsID).maxAge;
            if (!data.statsID.StartsWith("summon"))
            {
                maxAge += ((ExtendedActor)selectedUnit).extendedCurStats.maxAge;
            }
            if (data.age * 5 >= maxAge << 2)
            {
                colorAge = Color.red;
            }
            helper.showStat("creature_statistics_age", data.age + "/" + maxAge, colorAge);
            if (selectedUnit.stats.inspect_kills)
            {
                helper.showStat("creature_statistics_kills", data.kills);
            }
            if (selectedUnit.stats.inspect_experience)
            {
                helper.showStat("creature_statistics_character_experience", data.experience.ToString() + "/" + Config.selectedUnit.getExpToLevelup().ToString());
            }
            if (selectedUnit.stats.inspect_experience)
            {
                helper.showStat("creature_statistics_character_level", data.level);
            }
            if (selectedUnit.stats.inspect_children)
            {
                helper.showStat("creature_statistics_children", data.children);
            }
            __instance.moodBG.gameObject.SetActive(false);
            __instance.favoriteFoodBg.gameObject.SetActive(false);
            __instance.favoriteFoodSprite.gameObject.SetActive(false);
            if (selectedUnit.stats.unit && !selectedUnit.stats.baby)
            {
                string pValue = "??";
                if (!string.IsNullOrEmpty(data.favoriteFood))
                {
                    pValue = LocalizedTextManager.getText(data.favoriteFood, null);
                    __instance.favoriteFoodBg.gameObject.SetActive(true);
                    __instance.favoriteFoodSprite.gameObject.SetActive(true);
                    __instance.favoriteFoodSprite.sprite = AssetManager.resources.get(data.favoriteFood).getSprite();
                }
                helper.showStat("creature_statistics_favorite_food", pValue);
            }
            if (selectedUnit.stats.unit)
            {
                __instance.moodBG.gameObject.SetActive(true);
                helper.showStat("creature_statistics_mood", LocalizedTextManager.getText("mood_" + data.mood, null));
                MoodAsset moodAsset = AssetManager.moods.get(data.mood);
                __instance.moodSprite.sprite = moodAsset.getSprite();
                if ((PersonalityAsset)Reflection.GetField(typeof(Actor), selectedUnit, "s_personality") != null)
                {
                    helper.showStat("creature_statistics_personality", LocalizedTextManager.getText("personality_" + ((PersonalityAsset)Reflection.GetField(typeof(Actor), selectedUnit, "s_personality")).id, null));
                }
            }
            Text text = __instance.text_description;
            text.text += "\n";
            Text text2 = __instance.text_values;
            text2.text += "\n";
            if (selectedUnit.stats.inspect_home)
            {
                string pID = "creature_statistics_homeVillage";
                object pValue2 = ((Config.selectedUnit.city != null) ? ((CityData)Reflection.GetField(typeof(City), Config.selectedUnit.city, "data")).cityName : "??");
                Kingdom kingdom = Config.selectedUnit.kingdom;
                Color? color;
                if (kingdom == null)
                {
                    color = null;
                }
                else
                {
                    KingdomColor kingdomColor = (KingdomColor)Reflection.GetField(typeof(Kingdom), kingdom, "kingdomColor");
                    color = ((kingdomColor != null) ? new Color?(kingdomColor.colorBorderOut) : null);
                }
                Color? color2 = color;
                helper.showStat(pID, pValue2, (color2 != null) ? new Color32?(color2.GetValueOrDefault()) : null);
            }
            if (Config.selectedUnit.kingdom != null && Config.selectedUnit.kingdom.isCiv())
            {
                string pID2 = "kingdom";
                object name = Config.selectedUnit.kingdom.name;
                Kingdom kingdom2 = Config.selectedUnit.kingdom;
                Color? color3;
                if (kingdom2 == null)
                {
                    color3 = null;
                }
                else
                {
                    KingdomColor kingdomColor2 = (KingdomColor)Reflection.GetField(typeof(Kingdom), kingdom2, "kingdomColor");
                    color3 = ((kingdomColor2 != null) ? new Color?(kingdomColor2.colorBorderOut) : null);
                }
                Color? color2 = color3;
                helper.showStat(pID2, name, (color2 != null) ? new Color32?(color2.GetValueOrDefault()) : null);
            }
            Culture culture = MapBox.instance.cultures.get(data.culture);
            if (culture != null)
            {
                string text3 = "";
                text3 += culture.name;
                text3 = text3 + "[" + culture.followers.ToString() + "]";
                text3 = Toolbox.coloredString(text3, new Color32?(culture.color32_text));
                helper.showStat("culture", text3);
                __instance.buttonCultures.SetActive(true);
            }
            else
            {
                __instance.buttonCultures.SetActive(false);
            }
            if (Config.selectedUnit.stats.isBoat)
            {
                Boat component = Config.selectedUnit.GetComponent<Boat>();
                helper.showStat("passengers", ((HashSet<Actor>)Reflection.GetField(typeof(Boat), component, "unitsInside")).Count);
                if ((bool)component.CallMethod("isState", BoatState.TransportDoLoading))
                {
                    helper.showStat("status", LocalizedTextManager.getText("status_waiting_for_passengers", null));
                }
            }
            __instance.text_description.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            __instance.text_values.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            __instance.text_description.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            __instance.text_values.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            if (LocalizedTextManager.isRTLLang())
            {
                __instance.text_description.alignment = TextAnchor.UpperRight;
                __instance.text_values.alignment = TextAnchor.UpperLeft;
            }
            else
            {
                __instance.text_description.alignment = TextAnchor.UpperLeft;
                __instance.text_values.alignment = TextAnchor.UpperRight;
            }
            if (selectedUnit.city == null)
            {
                __instance.buttonCity.SetActive(false);
            }
            else
            {
                __instance.buttonCity.SetActive(true);
            }
            if (selectedUnit.kingdom == null || !selectedUnit.kingdom.isCiv())
            {
                __instance.buttonKingdom.SetActive(false);
            }
            else
            {
                __instance.buttonKingdom.SetActive(true);
            }
            __instance.backgroundCiv.SetActive(__instance.buttonCity.activeSelf || __instance.buttonKingdom.activeSelf);
            helper.updateFavoriteIconFor(selectedUnit);
            helper.clearPrevButtons();
            __instance.CallMethod("loadTraits");
            helper.loadEquipment();
            return false;
        }
        //修改死亡机制
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "killHimself")]
        public static bool killHimself_Prefix(Actor __instance)
        {
            if (__instance == null || !__instance.GetData().alive)
            {
                return true;
            }
            if (__instance.haveTrait("asylum") && !__instance.stats.baby)
            {
                return false;
            }
            if (__instance.kingdom != null)
            {
                List<Actor> actors = null;
                if (Main.instance.kingdomBindActors.TryGetValue(__instance.kingdom.id, out actors))
                {
                    if(actors.Remove(__instance))
                        MonoBehaviour.print("[MoreActors.killHimself_Prefix]"+__instance.GetData().statsID + ":" + __instance.GetData().actorID);
                }
            }
            return true;
        }
        //小孩问题修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Baby), "update")]
        public static bool update_Prefix(Baby __instance, float pElapsed)
        {
            if (__instance.timerGrow > pElapsed)
            {
                return true;
            }
            Actor actorF = __instance.GetComponent<Actor>();
            ActorStatus data = actorF.GetData();
            if (!data.alive)
            {
                return false;
            }
            if ((bool)MapBox.instance.CallMethod("isPaused"))
            {
                return false;
            }

            Actor actor = MapBox.instance.createNewUnit(actorF.stats.growIntoID, actorF.currentTile, null, 0f, null);
            ActorStatus data1 = actor.GetData();
            actor.startBabymakingTimeout();
            data1.hunger = actor.stats.maxHunger / 2;
            ((GameStatsData)Reflection.GetField(typeof(GameStats), MapBox.instance.gameStats, "data")).creaturesBorn--;
            if (actorF.city != null)
            {
                actorF.city.addNewUnit(actor, true, true);
            }
            actor.CallMethod("setKingdom", actorF.kingdom);
            data1.diplomacy = data.diplomacy;
            data1.intelligence = data.intelligence;
            data1.stewardship = data.stewardship;
            data1.warfare = data.warfare;
            data1.culture = data.culture;
            data1.experience = data.experience;
            data1.level = data.level;
            data1.firstName = data.firstName;
            if (data.skin != -1)
            {
                data1.skin = data.skin;
            }
            if (data.skin_set != -1)
            {
                data1.skin_set = data.skin_set;
            }
            data1.age = data.age;
            data1.bornTime = data.bornTime;
            data1.health = data.health;
            data1.gender = data.gender;
            data1.kills = data.kills;
            foreach (string text in data.traits)
            {
                if (!(text == "peaceful"))
                {
                    actor.addTrait(text);
                }
            }
            if (data.favorite)
            {
                data1.favorite = true;
            }
            if (Config.spectatorMode && MoveCamera.focusUnit == actorF)
            {
                MoveCamera.focusUnit = actor;
            }
            ActorTools.copyMore(actorF, actor);
            actorF.killHimself(true, AttackType.GrowUp, false, false);
            return false;
        }
        //蛋问题修复
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Egg), "update", typeof(float))]
        public static IEnumerable<CodeInstruction> update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo copyMore = AccessTools.Method(typeof(ActorTools), "copyMore", new System.Type[] { typeof(Actor), typeof(Actor),typeof(bool) });
            MethodInfo getActor = AccessTools.Method(typeof(ActorTools), "getActor", new System.Type[] { typeof(Egg) });
            Label ret = new Label();

            int offset = 0;
            codes.Insert(81 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(81 + offset, new CodeInstruction(OpCodes.Callvirt, getActor));
            offset++;
            codes.Insert(81 + offset, new CodeInstruction(OpCodes.Ldloc_0));
            offset++;
            codes.Insert(81 + offset, new CodeInstruction(OpCodes.Ldc_I4_0));
            offset++;
            codes.Insert(81 + offset, new CodeInstruction(OpCodes.Call, copyMore));
            offset++;
            codes[88 + offset].labels.Add(ret);
            codes[17].operand = ret;
            return codes.AsEnumerable();
        }
        //处理新生物
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ActorBase), "generatePersonality")]
        public static void generatePersonality_Postfix(ActorBase __instance)
        {
            #region 设置出生属性
            ActorStatus status = Reflection.GetField(typeof(ActorBase), __instance, "data") as ActorStatus;
            ExtendedActor actor = (ExtendedActor)__instance;
            actor.extendedCurStats.maxAge = __instance.stats.maxAge;
            //获取修炼体系
            if (__instance.getCulture() != null)
            {
                List<string> cultisystem = new List<string>();
                Culture culture = __instance.getCulture();
                foreach (string tech in culture.list_tech_ids)
                {
                    if (tech.StartsWith("culti_"))
                    {
                        cultisystem.Add(tech);
                    }
                }
                if (cultisystem.Count > 0)
                {
                    actor.extendedData.status.cultisystem = cultisystem.GetRandom().Remove(0, 6);
                }
            }
            if (!MoreRaces.isCitizen((Actor)__instance))
            {
                if (Toolbox.randomChance(0.6f))
                {
                    actor.extendedData.status.canCultivate = false;
                }
            }
            if (Toolbox.randomChance(0.001f))
            {
                actor.extendedData.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            //设置名字
            actor.extendedData.status.familyName = __instance.stats.unit? AddAssetManager.chineseNameGenerator.get(__instance.stats.nameTemplate).addition_startList.GetRandom():AddAssetManager.chineseNameGenerator.get(__instance.stats.nameTemplate).addition_endList.GetRandom();
            actor.extendedData.status.familyID = actor.extendedData.status.familyName;
            status.firstName = ChineseNameGenerator.getCreatureName(__instance.stats.nameTemplate, actor.extendedData.status.familyName, __instance.stats.unit);
            Family family = Main.instance.familys[actor.extendedData.status.familyID];
            family.num++;
            #endregion
        }

        #endregion

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox),"createNewUnit")]
        public static bool createNewUnit_Prefix(ref Actor __result,string pStatsID, WorldTile pTile, string pJob = null, float pZHeight = 0f, ActorData pData = null)
        {
            ActorStats actorStats = AssetManager.unitStats.get(pStatsID);
            
            if (actorStats == null)
            {
                __result = null;
                return false;
            }
            ExtendedActor component;
            //Actor component;
            try
            {
                //component = UnityEngine.Object.Instantiate((GameObject)Resources.Load("actors/" + actorStats.prefab, typeof(GameObject))).gameObject.GetComponent<Actor>();
                component = UnityEngine.Object.Instantiate(Main.instance.prefabs.ExtendedActorPrefab).GetComponent<ExtendedActor>();
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("Tried to create actor: " + actorStats.id);
                UnityEngine.Debug.Log("Failed to load prefab for actor: " + actorStats.prefab);
                __result = null;
                return false;
            }
            
            component.transform.name = actorStats.id;
            component.new_creature = true;
            if (pData != null)
            {
                component.new_creature = false;
            }
            component.setWorld();
            component.loadStats(actorStats);
            if (pData != null)
            {
                Reflection.SetField(component,"data",pData.status);
                Reflection.SetField(component,"professionAsset",AssetManager.professions.get(pData.status.profession));
            }
            if (component.new_creature)
            {
                component.CallMethod("newCreature",(int)((Reflection.GetField(typeof(GameStats),MapBox.instance.gameStats,"data") as GameStatsData).gameTime + (double)MapBox.instance.units.Count));
            }
            component.transform.position = pTile.posV3;
            component.CallMethod("spawnOn",pTile, pZHeight);
            component.CallMethod("create");
            if (component.stats.kingdom != "")
            {
                component.CallMethod("setKingdom",MapBox.instance.kingdoms.dict_hidden[component.stats.kingdom]);
            }
            if (component.stats.hideOnMinimap)
            {
                component.transform.parent = Reflection.GetField(typeof(MapBox),MapBox.instance,"transformUnits") as Transform;
            }
            else
            {
                component.transform.parent = Reflection.GetField(typeof(MapBox), MapBox.instance, "transformCreatures") as Transform;
            }
            MapBox.instance.units.Add(component);
            __result = component;
            return false;
        }
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Actor), "spawnParticle")]
        public static IEnumerable<CodeInstruction> spawnParticle_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo get_sprite = AccessTools.Method(typeof(SpriteRenderer), "get_sprite");
            MethodInfo op_Inequality = AccessTools.Method(typeof(UnityEngine.Object), "op_Inequality");
            FieldInfo spriteRenderer = AccessTools.Field(typeof(Actor), "spriteRenderer");

            Label label = new Label();
            int offset = 0;
            codes.Insert(13 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(13 + offset, new CodeInstruction(OpCodes.Ldfld, spriteRenderer));
            offset++;
            codes.Insert(13 + offset, new CodeInstruction(OpCodes.Callvirt, get_sprite));
            offset++;
            codes.Insert(13 + offset, new CodeInstruction(OpCodes.Ldnull));
            offset++;
            codes.Insert(13 + offset, new CodeInstruction(OpCodes.Call, op_Inequality));
            offset++;
            codes.Insert(13 + offset, new CodeInstruction(OpCodes.Brfalse_S, label));
            offset++;
            codes[34 + offset].labels.Add(label);
            return codes.AsEnumerable();
        }
    }
}
