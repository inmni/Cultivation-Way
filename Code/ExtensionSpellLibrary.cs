using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtensionSpellLibrary : AssetLibrary<ExtensionSpellAsset>
    {

        public List<string> spellList = new List<string>();
        public override void init()
        {
            base.init();
            #region 简单法术
            add(new ExtensionSpellAsset
            {
                id = "example",
                name = "简单术法",
                rarity = 10,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 1f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtendedWorldActions.exampleSpell)
            });
            addSpell(t.id, t.rarity);
            add(new ExtensionSpellAsset
            {
                id = "lightning",
                name = "雷法",
                rarity = 9,
                coolDown = 10,
                baseCost = 50,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 10f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtendedWorldActions.lightningSpell)
            });
            addSpell(t.id, t.rarity);
            add(new ExtensionSpellAsset
            {
                id = "default_lightning",
                name = "引雷",
                rarity = 1,
                coolDown = 1,
                baseCost = 5,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 5f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtendedWorldActions.lightning1Spell)
            });
            addSpell(t.id, t.rarity);
            add(new ExtensionSpellAsset
            {
                id = "default_fire",
                name = "基础火法",
                rarity = 1,
                coolDown = 2,
                baseCost = 5,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 5f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 0, 0, 0, 100, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.defaultFire)
            });
            addSpell(t.id, t.rarity);
            add(new ExtensionSpellAsset
            {
                id = "swordsArray",
                name = "剑阵",
                rarity = 7,
                coolDown = 11,
                baseCost = 20,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 10f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 100, 0, 0, 0, 0 }),
                spellAction = new SpellAction(ExtendedWorldActions.swordsArray)
            });
            addSpell(t.id, t.rarity);
            add(new ExtensionSpellAsset
            {
                id = "base",
                name = "简单术法",
                rarity = 0,
                coolDown = 1,
                baseCost = 1,
                bannedCultisystem = new List<string>() { "bodying" },
                bannedRace = new List<string>(),
                might = 1f,
                type = new ExtensionSpellType() { requiredLevel = 5 },
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.baseSpell)
            });
            addSpell(t.id, t.rarity);
            #endregion
            #region 召唤

            #endregion
            #region 圣体显化
            add(new ExtensionSpellAsset
            {
                id = "STXH",
                name = "圣体显化",
                rarity = 1,
                coolDown = 50,
                baseCost = 0,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1f,
                type = new ExtensionSpellType(),
                direction = DirectionType.none,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.Shengtixianhua)
            });
            addSpell(t.id, t.rarity);
            #endregion
            #region 特殊法术
            add(new ExtensionSpellAsset
            {
                id = "JiaoDragon_laser",
                name = "太虚吐息",
                rarity = 10,
                coolDown = 1,
                baseCost = 0,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 10f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.laserSpell)
            });
            add(new ExtensionSpellAsset
            {
                id = "goldBar",
                name = "闷棍",
                rarity = 10,
                coolDown = 5,
                baseCost = 10,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 5f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.barSpell)
            });
            add(new ExtensionSpellAsset
            {
                id = "goldBarDown",
                name = "昏棍",
                rarity = 10,
                coolDown = 1,
                baseCost = 10,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 5f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.barDownSpell)
            });
            #endregion
            #region 种族法术
            add(new ExtensionSpellAsset
            {
                id = "summonTian",
                name = "折跃航标",
                rarity = 10,
                coolDown = 100,
                baseCost = 150,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1.2f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonTianSpell)
            });
            add(new ExtensionSpellAsset
            {
                id = "summonTian1",
                name = "帝卫亲临",
                rarity = 10,
                coolDown = 1,
                baseCost = 0,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1.0f,
                type = new ExtensionSpellType() { attacking = false, levelUp = true, requiredLevel = 101 },
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonTianSpell1)
            });
            add(new ExtensionSpellAsset
            {
                id = "summon",
                name = "亡灵复生",
                rarity = 10,
                coolDown = 6,
                baseCost = 20,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1f,
                type = new ExtensionSpellType(),
                direction = DirectionType.none,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtendedWorldActions.summonSpell)
            });
            #endregion
        }
        private void addSpell(string id, int num)
        {
            for (int i = 0; i < 10 - num; i++)
            {
                spellList.Add(id);
            }
        }
    }
}
