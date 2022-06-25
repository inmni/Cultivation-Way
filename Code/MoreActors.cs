using Cultivation_Way.Utils;
using CultivationWay;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class MoreActors
    {
        internal static EasternDragonAsset easternDragonAsset;
        internal static Dictionary<string, SpecialActorAsset> specialActorAssets = new Dictionary<string, SpecialActorAsset>();
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
            summonTian.texture_path = "t_summonTian";
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
            summonTian1.texture_path = "t_summonTian1";
            summonTian1.body_separate_part_head = false;
            summonTian1.swimToIsland = false;
            summonTian1.procreate = false;
            summonTian1.defaultAttack = "summonTian1";
            summonTian1.defaultWeapons = new List<string>();
            summonTian1.use_items = false;
            addActor(summonTian1);
            ExtendedWorldData.instance.creatureLimit.Add(summonTian1.id, 1);
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
            EasternHuman.texture_heads = string.Empty;
            EasternHuman.body_separate_part_head = false;
            EasternHuman.useSkinColors = true;
            EasternHuman.action_death = (WorldAction)Delegate.Combine(EasternHuman.action_death, new WorldAction(ExtendedWorldActions.aTransformToGod));
            addActor(EasternHuman);
            addColor(EasternHuman, "default", "#FFE599", "#F9CB9C");
            #endregion
            #region 天族
            ActorStats TianAsset = AssetManager.unitStats.clone("Tian", "unit_human");
            TianAsset.maxAge = 300;
            TianAsset.race = "Tian";
            TianAsset.unit = true;
            TianAsset.body_separate_part_head = false;
            TianAsset.oceanCreature = false;
            TianAsset.procreate = true;
            TianAsset.nameLocale = "Tians";
            TianAsset.nameTemplate = "Tian_name";
            TianAsset.setBaseStats(120, 17, 30, 4, 5, 100, 3);
            TianAsset.useSkinColors = false;
            TianAsset.texture_path = "t_Tian";
            TianAsset.texture_heads = string.Empty;
            TianAsset.heads = 0;
            TianAsset.icon = "Tian";
            TianAsset.defaultWeapons = new List<string> { "knife1" };
            TianAsset.defaultWeaponsMaterial = new List<string> { "base" };
            TianAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            addActor(TianAsset);
            addActor(AssetManager.unitStats.clone("unit_Tian", "Tian"));
            #region 天族附属
            ActorStats Tian1 = AssetManager.unitStats.clone("summonTian2", "summonTian");
            Tian1.texture_path = "t_summonTian2";
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
            MingAsset.texture_heads = string.Empty;
            MingAsset.body_separate_part_head = false;
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
            YaoAsset.texture_heads = string.Empty;
            YaoAsset.body_separate_part_head = false;
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
            ExtendedWorldData.instance.creatureLimit.Add(MonkeySheng1.id, 1);
            ActorStats MonkeySheng2 = AssetManager.unitStats.clone("MonkeySheng2", "MonkeySheng1");
            MonkeySheng2.texture_path = "MonkeySheng2";
            addColor(MonkeySheng2);
            addActor(MonkeySheng2);
            protoAndShengs[1].add("monkey", MonkeySheng2.id);
            ExtendedWorldData.instance.creatureLimit.Add(MonkeySheng2.id, 1);
            addYaoSheng("cat");
            addYaoSheng("cow");
            addYaoSheng("snake");
            addYaoSheng("wolf");
            addYaoSheng("chicken");
            #endregion

            #endregion
            #region 巫族
            ActorStats Wu = AssetManager.unitStats.clone("unit_Wu", "unit_human");
            Wu.race = "Wu";
            Wu.nameLocale = "Wus";
            Wu.nameTemplate = "Wu_name";
            Wu.body_separate_part_head = true;
            Wu.traits = new List<string> { "strong","giant"};
            Wu.heads = 6;
            Wu.useSkinColors = true;
            Wu.action_death = ExtendedWorldActions.aTransformToXingTian;
            addActor(Wu);
            addColor(Wu, "default", "#FFE599", "#F9CB9C");
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
            FairyFoxAsset.texture_path = "t_FairyFox";
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
            FuRenAsset.texture_path = "t_FuRen";
            FuRenAsset.texture_heads = "";
            FuRenAsset.icon = "iconFuRen";
            FuRenAsset.defaultAttack = "firework";
            FuRenAsset.use_items = false;
            addActor(FuRenAsset);
            ExtendedWorldData.instance.creatureLimit.Add(FuRenAsset.id, 1);
            //虎
            ActorStats TigerAsset = AssetManager.unitStats.clone("Tiger", "wolf");
            TigerAsset.race = "Tiger";
            TigerAsset.nameLocale = "Tiger";
            TigerAsset.nameTemplate = "tiger_name";
            TigerAsset.texture_path = "t_Tiger";
            TigerAsset.icon = "iconTiger";
            addActor(TigerAsset);
            AssetManager.topTiles.get("grass_low").addUnitsToSpawn("Tiger");
            //东方龙
            ActorStats EasternDragonAsset = AssetManager.unitStats.clone("EasternDragon", "dragon");
            EasternDragonAsset.race = "EasternDragon";
            EasternDragonAsset.prefab = "p_easternDragon";
            EasternDragonAsset.actorSize = ActorSize.S17_Dragon;
            EasternDragonAsset.shadowTexture = "unitShadow_23";
            EasternDragonAsset.texture_path = "t_EasternDragon";
            EasternDragonAsset.texture_heads = "";
            //EasternDragonAsset.animation_swim = "";
            //EasternDragonAsset.animation_idle = "";
            //EasternDragonAsset.animation_walk = "";
            EasternDragonAsset.icon = "iconEasternDragon";
            EasternDragonAsset.skipFightLogic = true;
            EasternDragonAsset.disablePunchAttackAnimation = true;
            EasternDragonAsset.specialAnimation = true;
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
            ExtendedWorldData.instance.creatureLimit.Add(EasternDragonAsset.id, 1);
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
            Achelous.texture_path = "t_Achelous";
            Achelous.icon = "Achelous";
            addActor(Achelous);
            ExtendedWorldData.instance.creatureLimit.Add(Achelous.id, 1);
            ExtendedWorldData.instance.godList.Add(Achelous.id, "河伯");
            ActorStats EarthGod = AssetManager.unitStats.clone("EarthGod", "Achelous");
            EarthGod.texture_path = "t_EarthGod";
            EarthGod.icon = "EarthGod";
            addActor(EarthGod);
            ExtendedWorldData.instance.creatureLimit.Add(EarthGod.id, 1);
            ExtendedWorldData.instance.godList.Add(EarthGod.id, "土地");
            ActorStats Mammon = AssetManager.unitStats.clone("Mammon", "Achelous");
            Mammon.texture_path = "t_Mammon";
            Mammon.icon = "Mammon";
            addActor(Mammon);
            ExtendedWorldData.instance.creatureLimit.Add(Mammon.id, 1);
            ExtendedWorldData.instance.godList.Add(Mammon.id, "财神");
            ActorStats Hymen = AssetManager.unitStats.clone("Hymen", "Achelous");
            Hymen.texture_path = "t_Hymen";
            Hymen.icon = "Hymen";
            addActor(Hymen);
            ExtendedWorldData.instance.creatureLimit.Add(Hymen.id, 1);
            ExtendedWorldData.instance.godList.Add(Hymen.id, "月老");
            ActorStats MountainGod = AssetManager.unitStats.clone("MountainGod", "Achelous");
            MountainGod.texture_path = "t_MountainGod";
            MountainGod.icon = "MountainGod";
            addActor(MountainGod);
            ExtendedWorldData.instance.creatureLimit.Add(MountainGod.id, 1);
            ExtendedWorldData.instance.godList.Add(MountainGod.id, "山神");
            ActorStats ZhongKui = AssetManager.unitStats.clone("ZhongKui", "Achelous");
            ZhongKui.texture_path = "t_ZhongKui";
            ZhongKui.icon = "ZhongKui";
            addActor(ZhongKui);
            ExtendedWorldData.instance.creatureLimit.Add(ZhongKui.id, 1);
            ExtendedWorldData.instance.godList.Add(ZhongKui.id, "钟馗");
            //祖巫
            ActorStats _ZuWu = AssetManager.unitStats.clone("_ZuWu", "unit_Wu");
            _ZuWu.baseStats.health = 3000;
            _ZuWu.baseStats.damage = 200;
            _ZuWu.procreate = false;
            _ZuWu.unit = false;
            ActorStats DiJiang = addZuWu("DiJiang");
            DiJiang.baseStats.speed = 200;
            addZuWu("GongGong");
            addZuWu("HouTu");
            ActorStats GouMang = addZuWu("GouMang");
            GouMang.traits.Add("healing_aura");
            addZuWu("QiangLiang");
            addZuWu("RuShou");
            ActorStats SheBiShi =addZuWu("SheBiShi");
            SheBiShi.traits.Add("poisonous");
            addZuWu("TianWu");
            addZuWu("XingTian");
            addZuWu("XiZi");
            addZuWu("XuanMing");
            addZuWu("ZhuJiuYin");
            ActorStats ZhuRong =addZuWu("ZhuRong");
            ZhuRong.traits.Add("extendedBurning_feet");
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
            JiaoDragonAsset.texture_path = "t_JiaoDragon";
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
            XieDragonAsset.texture_path = "t_XieDragon";
            XieDragonAsset.icon = "iconXieDrangon";
            XieDragonAsset.setBaseStats(1000000, 10000, 0, 99, 100, 100);
            XieDragonAsset.baseStats.scale = 0.20f;
            XieDragonAsset.baseStats.size = 0.5f;
            addActor(XieDragonAsset);

            ActorStats NianAsset = AssetManager.unitStats.clone("Nian", "EasternDragon");
            NianAsset.race = "Nian";
            NianAsset.kingdom = "boss";
            NianAsset.prefab = "p_specialActor";
            NianAsset.texture_path = "t_Nian";
            NianAsset.icon = "iconNian";
            NianAsset.baseStats.range = 23f;
            NianAsset.oceanCreature = false;
            NianAsset.action_death = (WorldAction)Delegate.Combine(NianAsset.action_death, new WorldAction(ExtendedWorldActions.aNianDie));
            addActor(NianAsset);
            ExtendedWorldData.instance.creatureLimit.Add(NianAsset.id, 1);
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
            MengZhuAsset.texture_path = "t_MengZhu";
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
            ActorStats babyWu = AssetManager.unitStats.clone("baby_Wu", "unit_Wu");
            babyWu.timeToGrow = 200;
            babyWu.baby = true;
            babyWu.growIntoID = "unit_Wu";
            addActor(babyWu);
            addColor(babyWu);
            #endregion
            initEasternDragonAsset();
            initOthersSpecialActorAssets();
        }
        private ActorStats addYao(string id)
        {
            string tranformID = ActorTools.getTranformID(id);
            ActorStats pStats = AssetManager.unitStats.clone(tranformID, "_Yao");
            pStats.texture_path = "t_" + tranformID;
            pStats.nameTemplate = id + "_name";
            addColor(pStats);
            addActor(pStats);
            protoAndYao.add(id, tranformID);
            return pStats;
        }
        private ActorStats addZuWu(string id)
        {
            ActorStats stats = AssetManager.unitStats.clone(id, "_ZuWu");
            stats.texture_path = "t_" + id;
            ExtendedWorldData.instance.creatureLimit.Add(id, 1);
            return stats;
        }
        private void addYaoSheng(string id)
        {
            string tranformID = ActorTools.getTranformID(id).Replace("Yao", "Sheng");
            ActorStats pStats = AssetManager.unitStats.clone(tranformID, "YaoSheng");
            pStats.texture_path = "t_" + tranformID;
            pStats.nameTemplate = id + "_name";
            pStats.useSkinColors = false;
            addActor(pStats);
            protoAndShengs[0].add(id, tranformID);
            ExtendedWorldData.instance.creatureLimit.Add(tranformID, 1);
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
            Main.instance.addActors.Add(pStats.id);
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

       
    }
}
