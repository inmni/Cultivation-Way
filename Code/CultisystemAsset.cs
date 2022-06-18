namespace Cultivation_Way
{
    internal class CultisystemAsset : Asset
    {
        public string name;//体系名

        public uint flag;//标识

        public float talentNeed;//天赋要求

        public float[] baseExperience;//基础经验

        public MoreStats[] moreStats;//各个境界属性加成

        //设全体种族集合为T, 允许种族集合为A, 禁用种族集合为B，
        //若A ≠ {φ}，则实际允许种族集合为(T∩A)\B
        //否则为T\B

        public string[] allowedRace;//允许种族

        public string[] bannedRace;//禁用种族

        public addExperienceAction addExperience;
        public CultisystemAsset()
        {

        }
    }
}
