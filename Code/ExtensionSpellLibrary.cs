using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    internal class ExtensionSpellLibrary:AssetLibrary<ExtensionSpellAsset>
    {

        public List<string> spellList = new List<string>();
        public override void init()
        {
            base.init();
            #region 简单法术
            this.add(new ExtensionSpellAsset
            {
                id = "example",
                name = "测试",
                rarity = 10,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 1f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.exampleSpell)
            }) ;
            addSpell(t.id, t.rarity);
            this.add(new ExtensionSpellAsset
            {
                id = "lightning",
                name = "雷法",
                rarity = 9,
                coolDown = 5,
                baseCost =50,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 10f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.lightningSpell)
            });
            addSpell(t.id, t.rarity);
            this.add(new ExtensionSpellAsset
            {
                id = "default_lightning",
                name = "引雷",
                rarity = 1,
                coolDown = 1,
                baseCost = 5,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 5f,
                type = new ExtensionSpellType(),//主动，可重复，必定触发，作用于单位，远程，单体，矢量
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.lightning1Spell)
            });
            addSpell(t.id, t.rarity);
            this.add(new ExtensionSpellAsset
            {
                id = "default_fire",
                name = "基础火法",
                rarity = 1,
                coolDown = 1,
                baseCost = 5,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 5f,
                type = new ExtensionSpellType(),//主动，可重复，必定触发，作用于单位，远程，单体，矢量
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 0, 0, 0, 100, 0 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.defaultFire)
            });
            addSpell(t.id, t.rarity);
            this.add(new ExtensionSpellAsset
            {
                id = "swordsArray",
                name = "剑阵",
                rarity = 7,
                coolDown = 6,
                baseCost = 20,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying" },
                might = 10f,
                type = new ExtensionSpellType(),//主动，可重复，必定触发，作用于单位，远程，单体，矢量
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 100, 0, 0, 0, 0 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.swordsArray)
            });
            addSpell(t.id, t.rarity);
            #endregion
            #region 召唤
            
            #endregion
            #region 圣体显化
            this.add(new ExtensionSpellAsset
            {
                id = "STXH",
                name = "圣体显化",
                rarity = 10,
                coolDown = 50,
                baseCost = 0,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1f,
                type = new ExtensionSpellType(),
                direction = DirectionType.none,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.Shengtixianhua)
            });
            #endregion
            #region 特殊法术
            this.add(new ExtensionSpellAsset
            {
                id = "JiaoDragon_laser",
                name = "太虚吐息",
                rarity = 10,
                coolDown = 1,
                baseCost = 0,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1000f,
                type = new ExtensionSpellType(),
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20,20,20,20,20}),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.laserSpell)
            });
            #endregion
            #region 种族法术
            this.add(new ExtensionSpellAsset
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
                spellAction = new SpellAction(ExtensionSpellActionLibrary.summonTianSpell)
            });
            this.add(new ExtensionSpellAsset
            {
                id = "summonTian1",
                name = "帝卫亲临",
                rarity = 10,
                coolDown = 1,
                baseCost = 0,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>(),
                might = 1.0f,
                type = new ExtensionSpellType() { attacking = false,levelUp = true,requiredLevel = 101 },
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.summonTianSpell1)
            }) ;
            this.add(new ExtensionSpellAsset
            {
                id = "summon",
                name = "亡灵复生",
                rarity = 10,
                coolDown = 6,
                baseCost = 20,
                bannedRace = new List<string>(),
                bannedCultisystem = new List<string>() { "bodying", "bushido" },
                might = 1f,
                type = new ExtensionSpellType(),
                direction = DirectionType.none,
                chineseElement = new ChineseElement(new int[] { 20, 20, 20, 20, 20 }),
                spellAction = new SpellAction(ExtensionSpellActionLibrary.summonSpell)
            });
            #endregion
        }
        private void addSpell(string id,int num)
        {
            for(int i = 0; i < 10-num; i++)
            {
                spellList.Add(id);
            }
        }
    }
}
