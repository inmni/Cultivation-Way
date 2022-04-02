namespace Cultivation_Way
{
    public class EasternDragonState
    {//动作状态
        public enum ActionState
        {
            Stop = 0,//停止
            Move = 1,//移动
            Attack = 2,//正在攻击
            Spell = 3,//施法
            Up = 4,//化龙
            Landing = 5,//化人
            Death = 6
        }
        public enum Shape
        {
            Human = 0,
            Dragon = 1
        }
        internal ActionState actionState = ActionState.Stop;
        internal Shape shape = Shape.Dragon;
    }
}
