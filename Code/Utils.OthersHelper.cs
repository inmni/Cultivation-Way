using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
using CultivationWay;
using System;

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
        public static float getAngle(Vector3 dotO,Vector3 dotT)//获取O到T的角度,返回值用于z
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
        public static float getDistance(Vector3 dotO,Vector3 dotT)//获取O到T的距离
        {
            return Mathf.Sqrt((dotO.x - dotT.x) * (dotO.x - dotT.x) + (dotO.y - dotT.y) * (dotO.y - dotT.y));
        }
        public static Color32 GetColor32ByElement(ChineseElement element)
        {
            Color32 color = new Color32();
            color.r = 0;
            color.g = 255;
            color.b = 255;
            color.a = (byte)(255-Mathf.Pow((element.getPurity()-1),0.2f)*193f);
            return color;
        }//通过元素获取一定透明度的颜色，可能存在一定偏差
        public static float getSpellDamage(ExtensionSpell spell,BaseSimObject pUser,BaseSimObject pTarget)
        {
            
            if (pTarget.objectType == MapObjectType.Actor)
            {
                if (((Actor)pTarget).GetData().alive)
                {
                    return 0;
                }
                float oriDamage = ((Actor)pUser).GetCurStats().damage * spell.might;
                float num = 0f;
                float num1 = 0f;
                try
                {
                    int[] elementU = ((Actor)pUser).GetMoreStats().element.baseElementContainer;
                    int[] elementT = ((Actor)pTarget).GetMoreStats().element.baseElementContainer;
                    int[] elementS = AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID).chineseElement.baseElementContainer;
                    //考虑属性克制
                    for (int i = 0; i < 5; i++)
                    {
                        num += elementU[i] * elementT[getBeOppsitedBy(i)] / 10000f;
                        num1 += elementU[i] * (elementS[i] + elementS[getBePromotedBy(i)]) / 20000f;
                    }
                    return oriDamage * num * num1;
                }catch(NullReferenceException e)
                {
                    MonoBehaviour.print("攻击方："+((Actor)pUser).GetData().firstName+":"+ ((Actor)pUser).GetMoreStats());
                    MonoBehaviour.print("存活状态：" + ((Actor)pUser).GetData().alive);
                    MonoBehaviour.print("被攻击方："+((Actor)pTarget).GetData().firstName + ":" + ((Actor)pTarget).GetMoreStats());
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
                    num1 += elementU[i] * (elementS[i] + elementS[getBePromotedBy(i)]) / 20000f;
                }
                return oriDamage * num1;
            }
        }
        public static void hitEnemiesInRange(BaseSimObject pUser,BaseSimObject pTarget,float range,float damge,ExtensionSpell spell = null)
        {
            List<Actor> enemies = getEnemyObjectInRange(pUser, pTarget.currentTile, range);
            List<WorldTile> tiles = new List<WorldTile>();

            tiles = getTilesInRange(pTarget.currentTile, pTarget.currentTile, tiles, range);
            if (spell == null)
            {
                foreach (Actor actor in enemies)
                {
                    if (actor == pUser||!actor.GetData().alive)
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
                    actor.CallMethod("getHit", getSpellDamage(spell,pUser,actor), true, AttackType.Other, pUser, true);
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
            List<WorldTile> tiles = new List<WorldTile>();
            tiles = getTilesInRange(pOrigin.currentTile, pOrigin.currentTile, tiles, r);
            
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
                    if (!list.Contains(actor) && pOrigin.kingdom==actor.kingdom)
                    {
                        list.Add(actor);
                    }
                }
            }
            return list;
        }//获取以pOrigin为中心，r为半径以内pOrigin的所有同国家的人
        public static List<Actor> getEnemyObjectInRange(BaseSimObject pUser,WorldTile pTile,float r)
        {
            List<Actor> list = new List<Actor>();
            List<WorldTile> tiles = new List<WorldTile>();
            tiles = getTilesInRange(pTile, pTile, tiles, r);
            foreach(WorldTile tile in tiles)
            {
                foreach(Actor actor in tile.units)
                {
                    if (pUser == null)
                    {
                        return list;
                    }
                    if (actor == null)
                    {
                        continue;
                    }
                    if (!list.Contains(actor)&&(pUser.kingdom == null || actor.kingdom==null||actor.kingdom.civs_allies==null||!actor.kingdom.civs_allies.ContainsKey(pUser.kingdom) || !actor.kingdom.civs_allies[pUser.kingdom]))
                    {
                        list.Add(actor);
                    }
                }
            }
            return list;
        }//获取以pTile为中心，r为半径以内pUser的所有敌人
        public static List<WorldTile> getTilesInRange(WorldTile center,WorldTile current,List<WorldTile> currentTiles,float range)
        {
            //终点条件为所有范围内的点都已经收集
            if (currentTiles.Contains(current)|| getDistance(center.posV3, current.posV3) > range)
            {
                return currentTiles;
            }
            currentTiles.Add(current);
            foreach(WorldTile tile in current.neighbours)
            {
                currentTiles = getTilesInRange(center, tile, currentTiles, range);
            }
            return currentTiles;
        }//获取以center为中心，range为半径以内的所有worldtile
    }
}
