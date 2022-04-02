using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    class OthersHelper
    {
        private static int[][] pureElementColor = new int[5][]
        {
            new int[3]{ 255,238,0},
            new int[3]{ 75,140,35},
            new int[3]{ 35,123,237},
            new int[3]{ 255,0,0},
            new int[3]{ 92,87,16}
        };
        private static Dictionary<string, int[]> idToElement = new Dictionary<string, int[]>()
        {
            { "snowball",new int[]{ 0,0,80,0,20} },
            { "firebomb",new int[]{ 0,0,0,100,0} },
            { "torch",   new int[]{ 0,50,0,50,0} },
            { "dark_orb",new int[]{ 0,30,50,0,20} },
            { "red_orb", new int[]{ 30,0,0,70,0} },
            {"freeze_orb",new int[]{ 10,0,90,0,0} },
            { "fireball",new int[]{ 0,0,20,80,0} },
            { "rock",new int[]{ 30,0,0,0,70} }
        };
        public static string UpperFirst(string text)
        {
            if (text == null)
            {
                return null;
            }
            if (text == string.Empty)
            {
                return text;
            }
            if (text[0] >= 'a' && text[0] <= 'z')
            {
                if (text.Length > 1)
                {
                    return char.ToUpper(text[0]).ToString() + text.Substring(1);
                }
                else
                {
                    return char.ToUpper(text[0]).ToString();
                }
            }
            else
            {
                return text;
            }
        }
        public static string LowerFirst(string text)
        {
            if (text == null)
            {
                return null;
            }
            if (text == string.Empty)
            {
                return text;
            }
            if (text[0] >= 'A' && text[0] <= 'Z')
            {
                if (text.Length > 1)
                {
                    return (text[0] + 'a' - 'A').ToString() + text.Substring(1);
                }
                else
                {
                    return (text[0] + 'a' - 'A').ToString();
                }
            }
            else
            {
                return text;
            }
        }
        /// <summary>
        /// 根据元素获取对应的投掷物id
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns></returns>
        public static string getRealBaseSpell(ChineseElement element)
        {
            string id = "red_orb";
            int maxMembership = 10000;
            foreach (string key in idToElement.Keys)
            {
                int membership = 1;
                for (int i = 0; i < 5; i++)
                {
                    membership *= Math.Abs(element.baseElementContainer[i]- idToElement[key][i]) + 1;
                }

                if (membership < maxMembership)
                {
                    maxMembership =  membership;
                    id = key;
                }
            }
            if (Toolbox.randomChance(0.2f))
            {
                id = idToElement.Keys.ToList().GetRandom();
            }
            return id;
        }
        public static string getOriginBodyID(SpecialBody body)
        {
            string originBodyName = body.origin;
            if (originBodyName == AddAssetManager.specialBodyLibrary.get("LXST").origin)
            {
                return "LXST";
            }
            else if (originBodyName == AddAssetManager.specialBodyLibrary.get("XTDT").origin)
            {
                return "XTDT";
            }
            else if (originBodyName == AddAssetManager.specialBodyLibrary.get("HWMT").origin)
            {
                return "HWMT";
            }
            else
            {
                return "FT";
            }
        }
        /// <summary>
        /// 获取克制type的基本元素
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int getBeOppsitedBy(int type)
        {
            switch (type)
            {
                case 0:
                    return 3;
                case 1:
                    return 1;
                case 2:
                    return 4;
                case 3:
                    return 2;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// 获取增益type的基本元素
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int getBePromotedBy(int type)
        {
            switch (type)
            {
                case 0:
                    return 4;
                case 1:
                    return 2;
                case 2:
                    return 0;
                case 3:
                    return 1;
                default:
                    return 3;
            }
        }
        public static float getAngle(Vector3 dotO, Vector3 dotT)//获取O到T的角度,返回值用于z
        {
            float angle = Mathf.Atan((dotT.y - dotO.y) / (dotT.x - dotO.x)) * 180 / Mathf.PI;
            if (dotT.x >= dotO.x)
            {
                return angle;
            }
            else
            {
                return angle + 180f;
            }
        }
        public static float getDistance(Vector3 dotO, Vector3 dotT)//获取O到T的距离
        {
            return Mathf.Sqrt((dotO.x - dotT.x) * (dotO.x - dotT.x) + (dotO.y - dotT.y) * (dotO.y - dotT.y));
        }
        public static Color32 GetColor32ByElement(ChineseElement element)
        {
            Color32 color = new Color32();
            color.r = 0;
            color.g = 255;
            color.b = 255;
            color.a = (byte)(255 - Mathf.Pow((element.getImPurity() - 1), 0.2f) * 193f);
            return color;
        }//通过元素获取一定透明度的颜色，可能存在一定偏差
        public static float getSpellDamage(ExtensionSpell spell, BaseSimObject pUser, BaseSimObject pTarget)
        {

            if (pTarget.objectType == MapObjectType.Actor)
            {
                if (!((Actor)pTarget).GetData().alive)
                {
                    return 0;
                }
                float oriDamage = ((Actor)pUser).GetCurStats().damage * spell.might;
                float num = 0f;
                float num1 = 0f;
                try
                {
                    int[] elementU = ((Actor)pUser).GetMoreData().element.baseElementContainer;
                    int[] elementT = ((Actor)pTarget).GetMoreData().element.baseElementContainer;
                    int[] elementS = AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID).chineseElement.baseElementContainer;
                    //考虑属性克制
                    for (int i = 0; i < 5; i++)
                    {
                        num += elementU[i] * elementT[getBeOppsitedBy(i)] / 2000f;
                        num1 += elementU[i] * (elementS[i] + elementS[getBePromotedBy(i)]) / 2000f;
                    }
                    return oriDamage * num * num1;
                }
                catch (NullReferenceException e)
                {
                    MonoBehaviour.print("攻击方：" + ((Actor)pUser).GetData().firstName + ":" + ((Actor)pUser).GetMoreStats());
                    MonoBehaviour.print("存活状态：" + ((Actor)pUser).GetData().alive);
                    MonoBehaviour.print("被攻击方：" + ((Actor)pTarget).GetData().firstName + ":" + ((Actor)pTarget).GetMoreStats());
                    MonoBehaviour.print("存活状态：" + ((Actor)pTarget).GetData().alive);
                    MonoBehaviour.print("*******************");
                    return 1000000f;
                }
            }
            else
            {
                float oriDamage = ((Actor)pUser).GetCurStats().damage * spell.might;
                float num1 = 0f;
                int[] elementU = ((Actor)pUser).GetMoreStats().element.baseElementContainer;
                int[] elementS = AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID).chineseElement.baseElementContainer;
                //考虑属性克制
                for (int i = 0; i < 5; i++)
                {
                    num1 += elementU[i] * (elementS[i] + elementS[getBePromotedBy(i)]) / 2000f;
                }
                return oriDamage * num1;
            }
        }
        public static void hitEnemiesInRange(BaseSimObject pUser, WorldTile pTargetTile, float range, float damge, ExtensionSpell spell = null)
        {
            List<Actor> enemies = getEnemyObjectInRange(pUser, pTargetTile, range);

            List<WorldTile>  tiles = getTilesInRange(pTargetTile, range);
            if (spell == null)
            {
                foreach (Actor actor in enemies)
                {
                    if (actor == pUser || !actor.GetData().alive)
                    {
                        continue;
                    }
                    actor.CallMethod("getHit", damge, true, AttackType.Other, pUser, true);
                }
            }
            else
            {
                foreach (Actor actor in enemies)
                {
                    if (actor == pUser || !actor.GetData().alive)
                    {
                        continue;
                    }
                    actor.CallMethod("getHit", getSpellDamage(spell, pUser, actor), true, AttackType.Other, pUser, true);
                }
            }
            foreach (WorldTile tile in tiles)
            {
                tile.setBurned();
            }
        }
        public static List<Actor> getSameKingdomObjectInRange(BaseSimObject pOrigin, float r)
        {
            List<Actor> list = new List<Actor>();
            List<WorldTile> tiles = getTilesInRange(pOrigin.currentTile, r);

            foreach (WorldTile tile in tiles)
            {
                foreach (Actor actor in tile.units)
                {
                    if (pOrigin == null)
                    {
                        return list;
                    }
                    if (actor == null)
                    {
                        continue;
                    }
                    if (!list.Contains(actor) && pOrigin.kingdom == actor.kingdom)
                    {
                        list.Add(actor);
                    }
                }
            }
            return list;
        }//获取以pOrigin为中心，r为半径以内pOrigin的所有同国家的人
        public static List<Actor> getEnemyObjectInRange(BaseSimObject pUser, WorldTile pTile, float r)
        {
            List<Actor> list = new List<Actor>();
            List<WorldTile> tiles = getTilesInRange(pTile, r);
            foreach (WorldTile tile in tiles)
            {
                foreach (Actor actor in tile.units)
                {
                    if (pUser == null)
                    {
                        return list;
                    }
                    if (actor == null)
                    {
                        continue;
                    }
                    //if (!list.Contains(actor) && (pUser.kingdom == null || actor.kingdom == null || !pUser.kingdom.isCiv()||!actor.kingdom.isCiv()||!pUser.kingdom.isNomads()||!actor.kingdom.isNomads()||pUser.kingdom.isEnemy(actor.kingdom)))
                    if(!list.Contains(actor)&&(pUser.kingdom==null||actor.kingdom==null||pUser.kingdom.isEnemy(actor.kingdom)))
                    {
                        list.Add(actor);
                    }
                }
            }
            return list;
        }//获取以pTile为中心，r为半径以内pUser的所有敌人
        /// <summary>
        /// 返回一定范围内的点
        /// </summary>
        /// <param name="center">中心点WorldTile</param>
        /// <param name="range">半径</param>
        /// <returns></returns>
        public static List<WorldTile> getTilesInRange(WorldTile center, float range)
        {
            #region 递归式，决定放弃
            ////终点条件为所有范围内的点都已经收集
            //if (currentTiles.Contains(current) || getDistance(center.posV3, current.posV3) > range)
            //{
            //    return currentTiles;
            //}
            //currentTiles.Add(current);
            //foreach (WorldTile tile in current.neighbours)
            //{
            //    currentTiles = getTilesInRange(center, tile, currentTiles, range);
            //}
            //return currentTiles;
            #endregion

            //改用寻找圆周1/4边界，进行翻转获取
            List<WorldTile> tiles = new List<WorldTile>();
            //获取边界
            List<int> right = new List<int>();
            int x = (int)range;
            int y = 0;
            float aPerTile = 1f;
            float distance = 0f;
            while (y < range)
            {
                distance = Mathf.Sqrt((x * x * aPerTile) + (y * y * aPerTile));
                while (distance >= range)
                {
                    x--;
                    distance = Mathf.Sqrt((x * x * aPerTile) + (y * y * aPerTile));
                }
                right.Add(x);
                y++;
            }
            //添加tile
            //WorldTile.neighbours中0-3对应left,right,down,up
            //确定方向
            int[][] dir = new int[4][]{ 
                new int[2]{ 1, 3}, //右上
                new int[2]{ 1, 2}, //右下
                new int[2]{ 0, 3}, //左上
                new int[2]{ 0, 2} //左下
            };
            //添加至tiles，但原点未添加，四条轴各存在一次重复，采用去重，不采用加入时判断
            for(int i = 0; i < 4; i++)
            {
                WorldTile readyToAdd = center;//水平移动用于添加
                WorldTile yLine = center;     //竖直移动，以校准x=0
                for(int yPos = 0; yPos < right.Count; yPos++)
                {
                    for(int xPos = 0; xPos < right[yPos]; xPos++)
                    {
                        tiles.Add(readyToAdd);
                        if (readyToAdd.world_edge)
                        {
                            break;
                        }
                        readyToAdd = readyToAdd.neighbours[dir[i][0]];
                    }
                    if (yLine.world_edge)
                    {
                        break;
                    }
                    yLine = yLine.neighbours[dir[i][1]];
                    readyToAdd = yLine;
                }
            }
            //去重
            int rightLim = 1;
            int leftLim = 1;
            int upLim = 1;
            int downLim = 1;
            int centerLim = 3;
            for(int i = 0; i < tiles.Count; i++)
            {
                if(tiles[i].x == center.x && tiles[i].y == center.y && centerLim > 0)
                {
                    centerLim--;
                    tiles.RemoveAt(i);
                    i--;
                }
                else if(tiles[i].x == center.x)
                {
                    if (tiles[i].y < center.y&&downLim>0)
                    {
                        downLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                    else if (tiles[i].y > center.y && upLim > 0)
                    {
                        upLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                }
                else if(tiles[i].y == center.y)
                {
                    if (tiles[i].x < center.x && leftLim > 0)
                    {
                        leftLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                    else if (tiles[i].x > center.x && rightLim > 0)
                    {
                        rightLim--;
                        tiles.RemoveAt(i);
                        i--;
                    }
                }
            }
            return tiles;
        }
        public static Projectile startProjectile(string id, BaseSimObject pUser,BaseSimObject pTarget,float xOffset=0f,float yOffset = 0f)
        {
            Vector3 target = new Vector3(pTarget.currentPosition.x, pTarget.currentPosition.y);
            float pZ = (float)pTarget.CallMethod("getZ");
            float num = Vector2.Distance(pUser.currentPosition, pTarget.currentPosition) + pZ;
            Vector3 end = Toolbox.getNewPoint(pUser.currentPosition.x, pUser.currentPosition.y, target.x, target.y, num - ((BaseStats)Reflection.GetField(typeof(BaseSimObject), pTarget, "curStats")).size, true);
            end.y += 0.1f;
            Vector3 start = Toolbox.getNewPoint(pUser.currentPosition.x, pUser.currentPosition.y, end.x, end.y - 0.1f, (float)((BaseStats)Reflection.GetField(typeof(BaseSimObject), pUser, "curStats")).size, true);
            start.x += xOffset;
            start.y += 0.5f+yOffset;

            return (Projectile)MapBox.instance.stackEffects.CallMethod("startProjectile", start, end, id, pZ);
        }
    }
    class BiDictionary<TFirst, TSecond>
    {
        IDictionary<TFirst, TSecond> firstToSecond = new Dictionary<TFirst, TSecond>();
        IDictionary<TSecond, TFirst> secondToFirst = new Dictionary<TSecond, TFirst>();
        #region 直接操作函数
        public void add(TFirst first, TSecond second)
        {
            if (firstToSecond.ContainsKey(first) || secondToFirst.ContainsKey(second))
            {
                throw new ArgumentException("repeat");
            }
            firstToSecond.Add(first, second);
            secondToFirst.Add(second, first);
        }
        public ICollection<TFirst> GetFirsts()
        {
            return firstToSecond.Keys;
        }
        public ICollection<TSecond> GetSeconds()
        {
            return secondToFirst.Keys;
        }
        public TSecond GetByFirst(TFirst first)
        {
            TSecond second;
            if (!firstToSecond.TryGetValue(first, out second))
            {
                throw new ArgumentException("first");
            }
            return second;
        }
        public TFirst GetBySecond(TSecond second)
        {
            TFirst first;
            if (!secondToFirst.TryGetValue(second, out first))
            {
                throw new ArgumentException("second");
            }
            return first;
        }
        public void RemoveByFirst(TFirst first)
        {
            TSecond second;
            if (!firstToSecond.TryGetValue(first, out second))
            {
                throw new ArgumentException("first");
            }
            firstToSecond.Remove(first);
            secondToFirst.Remove(second);
        }
        public void RemoveBySecond(TSecond second)
        {
            TFirst first;
            if (!secondToFirst.TryGetValue(second, out first))
            {
                throw new ArgumentException("second");
            }
            secondToFirst.Remove(second);
            firstToSecond.Remove(first);
        }

        #endregion

        #region 检测性函数
        public bool TryAdd(TFirst first, TSecond second)
        {
            if (firstToSecond.ContainsKey(first) || secondToFirst.ContainsKey(second))
            {
                return false;
            }
            firstToSecond.Add(first, second);
            secondToFirst.Add(second, first);
            return true;
        }
        public bool TryGetByFirst(TFirst first, out TSecond second)
        {
            return firstToSecond.TryGetValue(first, out second);
        }
        public bool TryGetBySecond(TSecond second, out TFirst first)
        {
            return secondToFirst.TryGetValue(second, out first);
        }
        public bool TryRemoveByFirst(TFirst first)
        {
            TSecond second;
            if (!firstToSecond.TryGetValue(first, out second))
            {
                return false;
            }
            firstToSecond.Remove(first);
            secondToFirst.Remove(second);
            return true;
        }
        public bool TryRemoveBySecond(TSecond second)
        {
            TFirst first;
            if (!secondToFirst.TryGetValue(second, out first))
            {
                return false;
            }
            secondToFirst.Remove(second);
            firstToSecond.Remove(first);
            return true;
        }

        #endregion
        public int Count
        {
            get { 
                return firstToSecond.Count; 
            }
        }
        public void Clear()
        {
            firstToSecond.Clear();
            secondToFirst.Clear();
        }
    }
    class Matrix
    {
        public int x
        {
            get
            {
                return this.x;
            }
        }
        public int y
        {
            get
            {
                return this.y;
            }
        }
        public Matrix()
        {

        }
    }
}
