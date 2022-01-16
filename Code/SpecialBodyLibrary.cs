using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CultivationWay;
namespace Cultivation_Way
{
    public class SpecialBodyLibrary:AssetLibrary<SpecialBody>
    {
        public override void init()
        {
            base.init();
            this.add(new SpecialBody
            {
                id = "LXST",
                madeBy = "凌霄",
                name = "凌霄圣体",
                origin = "凌霄圣体",
                moreStats = new MoreStats(),
                mod_damage = 35,
                mod_health = 55,
                spellRelief = 0.15f,
                mod_attack_speed = -20,
                mod_speed = -20,
                rank = 3,
            });
            this.add(new SpecialBody
            {
                id = "XTDT",
                madeBy = "天",
                name = "先天道体",
                origin = "先天道体",
                moreStats = new MoreStats(),
                mod_damage = 40,
                mod_health = 40,
                spellRelief = 0.4f,
                rank = 3,
            });
            this.add(new SpecialBody
            {
                id = "HWMT",
                madeBy = "荒",
                name = "荒芜蛮体",
                origin = "荒芜蛮体",
                moreStats = new MoreStats(),
                mod_damage = 60,
                mod_attack_speed = 30,
                rank = 3,
            });
            this.add(new SpecialBody
            {
                id = "FT",
                madeBy = "凡者",
                name = "凡体",
                origin = "凡体",
                moreStats = new MoreStats(),
                rank = 1,
            });
        }
        internal void reset()
        {
            this.dict.Clear();
            Main.instance.SpecialBodyLimit = 100;
            init();
        }
        internal void clear()
        {
            this.dict.Clear();
            this.list.Clear();
            Main.instance.SpecialBodyLimit = 100;
        }
    }
}
