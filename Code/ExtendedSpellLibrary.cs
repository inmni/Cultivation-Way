using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtendedSpellLibrary : AssetLibrary<ExtendedSpellAsset>
    {

        public List<string> spellList = new List<string>();
        public override void init()
        {
            base.init();
            #region 简单法术
            add(new ExtendedSpellAsset
            {
                id = "example",
                name = "简单术法",
                rarity = 10,
                bannedCultisystem = new List<string>() { "bodying" },
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtendedWorldActions.exampleSpell)
            });
            add(new ExtendedSpellAsset
            {
                id = "lightning",
                name = "雷法",
                rarity = 9,
                coolDown = 10,
                baseCost = 50,
                type = ExtendedSpellType.ATTACK,
                bannedCultisystem = new List<string>() { "bodying" },
                might = 10f,
                chineseElement = new ChineseElement(new int[] { 0, 10, 40, 40, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.lightningSpell)
            });
            add(new ExtendedSpellAsset
            {
                id = "default_lightning",
                name = "引雷",
                rarity = 1,
                coolDown = 1,
                baseCost = 5,
                type = ExtendedSpellType.ATTACK,
                bannedCultisystem = new List<string>() { "bodying" },
                might = 5f,
                requiredLevel = 3,
                chineseElement = new ChineseElement(new int[] { 0, 10, 40, 40, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.lightning1Spell)
            });
            add(new ExtendedSpellAsset
            {
                id = "default_fire",
                name = "基础火法",
                rarity = 1,
                coolDown = 2,
                baseCost = 5,
                bannedCultisystem = new List<string>() { "bodying" },
                might = 5f,
                type = ExtendedSpellType.ATTACK,
                requiredLevel =3,
                chineseElement = new ChineseElement(new int[] { 0, 0, 0, 100, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.defaultFire)
            });
            add(new ExtendedSpellAsset
            {
                id = "swordsArray",
                name = "剑阵",
                rarity = 7,
                coolDown = 11,
                baseCost = 20,
                bannedCultisystem = new List<string>() { "bodying" },
                might = 10f,
                requiredLevel=6,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 100, 0, 0, 0, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.swordsArray)
            });
            add(new ExtendedSpellAsset
            {
                id = "base",
                name = "简单术法",
                rarity = 0,
                coolDown = 1,
                baseCost = 1,
                bannedCultisystem = new List<string>() { "bodying" },
                might = 1f,
                requiredLevel = 2,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.baseSpell)
            });
            #endregion
            #region 召唤

            #endregion
            #region 圣体显化
            add(new ExtendedSpellAsset
            {
                id = "STXH",
                name = "圣体显化",
                rarity = 1,
                coolDown = 50,
                baseCost = 0,
                might = 1f,
                requiredLevel=5,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.Shengtixianhua)
            });
            #endregion
            #region 特殊法术
            add(new ExtendedSpellAsset
            {
                id = "JiaoDragon_laser",
                name = "太虚吐息",
                rarity = 10,
                coolDown = 1,
                baseCost = 0,
                might = 10f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.laserSpell)
            });
            add(new ExtendedSpellAsset
            {
                id = "goldBar",
                name = "闷棍",
                rarity = 10,
                coolDown = 5,
                baseCost = 10,
                might = 5f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.barSpell)
            });
            add(new ExtendedSpellAsset
            {
                id = "goldBarDown",
                name = "昏棍",
                rarity = 10,
                coolDown = 1,
                baseCost = 10,
                might = 5f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.barDownSpell)
            });
            #endregion
            #region 种族法术
            add(new ExtendedSpellAsset
            {
                id = "summonTian",
                name = "折跃航标",
                rarity = 10,
                coolDown = 100,
                baseCost = 150,
                might = 1.2f,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonTianSpell)
            });
            add(new ExtendedSpellAsset
            {
                id = "summonTian1",
                name = "帝卫亲临",
                rarity = 10,
                coolDown = 1,
                baseCost = 0,
                might = 1.0f,
                requiredLevel = 101 ,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonTianSpell1)
            });
            add(new ExtendedSpellAsset
            {
                id = "summon",
                name = "亡灵复生",
                rarity = 10,
                coolDown = 6,
                baseCost = 20,
                might = 1f,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonSpell)
            });
            #endregion
            #region 十二祖巫
            add(new ExtendedSpellAsset
            {
                id = "sunkens",
                name = "地刺",
                rarity = 10,
                might = 3f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 0, 0, 0, 0, 100 }),
                spellAction = new SpellAction(ExtendedWorldActions.HouTu1)
            });
            add(new ExtendedSpellAsset
            {
                id = "HouTuSummon1",
                name = "六道轮回",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 0, 0, 0, 0, 100 }),
                spellAction = new SpellAction(ExtendedWorldActions.HouTu2)
            });
            add(new ExtendedSpellAsset
            {
                id = "HouTuSummon2",
                name = "祖巫的呼唤",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 0, 0, 0, 0, 100 }),
                spellAction = new SpellAction(ExtendedWorldActions.HouTu3)
            });
            add(new ExtendedSpellAsset
            {
                id = "timeStop",
                name = "时停",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.ZhuJiuYin)
            });
            add(new ExtendedSpellAsset
            {
                id = "shield",
                name = "守护",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.DEFEND,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.RuShou)
            });
            add(new ExtendedSpellAsset
            {
                id = "rainCloud",
                name = "雨",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 0, 0, 100, 0, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.XuanMing1)
            });
            add(new ExtendedSpellAsset
            {
                id = "wind_blade",
                name = "风刃",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.projectileLike)
            });
            add(new ExtendedSpellAsset
            {
                id = "ice_blade",
                name = "冰刃",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.projectileLike)
            });
            add(new ExtendedSpellAsset
            {
                id = "swordkee",
                name = "剑气",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.projectileLike)
            });
            add(new ExtendedSpellAsset
            {
                id = "water_orb1",
                name = "水弹",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 0, 0, 100, 0, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.projectileLike)
            });
            add(new ExtendedSpellAsset
            {
                id = "summonSnake",
                name = "唤蛇",
                rarity = 10,
                might = 1f,
                type = ExtendedSpellType.ATTACK,
                chineseElement = new ChineseElement(new int[] { 0, 0, 100, 0, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonSpell)
            });
            #endregion
        }
        public override ExtendedSpellAsset add(ExtendedSpellAsset pAsset)
        {
            if (pAsset.bannedCultisystem == null)
            {
                pAsset.bannedCultisystem = new List<string>();
            }
            if (pAsset.bannedRace == null)
            {
                pAsset.bannedRace = new List<string>();
            }
            for (int i = 0; i < 10 - pAsset.rarity; i++)
            {
                spellList.Add(pAsset.id);
            }
            for (int i = 0; i < AddAssetManager.cultisystemLibrary.list.Count; i++)
            {
                pAsset.allowedCultisystem += AddAssetManager.cultisystemLibrary.list[i].flag;
            }
            for(int i = 0; i < pAsset.bannedCultisystem.Count; i++)
            {
                pAsset.allowedCultisystem -= AddAssetManager.cultisystemLibrary.get(pAsset.bannedCultisystem[i]).flag;
            }
            return base.add(pAsset);
        }
    }
}
