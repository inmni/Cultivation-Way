using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    class Family
    {
        public string id;//家族姓氏

        public int maxLevel;//家族最大等级

        public string honorary;//家族历史最强者名字

        public CultivationBook cultivationBook;//家族功法

        public void levelUp(string name)
        {
            honorary = name;
            maxLevel++;
            if (Toolbox.randomChance(maxLevel / 110f))
            {
                cultivationBook.levelUp();
            }
        }
        public Family(string id)
        {
            this.id = id;
            maxLevel = 0;
            honorary = "";
            cultivationBook = new CultivationBook();
        }
        public Family() {
            id = "";
            maxLevel = 0;
            honorary = "";
            cultivationBook = new CultivationBook();
        }
    }
}
