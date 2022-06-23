namespace Cultivation_Way
{
    public class SpecialBody : Asset
    {
        public string name;//名字
        public string madeBy;//创造者
        public string origin;//起源
        public int rank;//品阶
        public float inheritChance;//继承概率
        public int mod_health;//血量加成
        public int mod_damage;//攻击加成
        public int mod_attack_speed;//攻速加成
        public int mod_speed;//移速加成
        public float vampire;      //吸血
        public float antiInjury;   //反伤
        public float spellRelief; //法伤减免
        internal MoreStats moreStats;//固定属性加成
    }
}
