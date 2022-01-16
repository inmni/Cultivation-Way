using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    class ExtensionSpellType
    {
        public bool attacking = true;
        public bool attacked = false;
        public bool levelUp = false;
        public bool byChance = false;
        public int requiredLevel = 1;
        public float chance = 0.2f;
        public ExtensionSpellType()
        {
        }

    }
}
