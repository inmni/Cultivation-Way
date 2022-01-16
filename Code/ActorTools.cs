using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
using CultivationWay;
using Microsoft.CSharp;
using UnityEngine;

namespace Cultivation_Way
{
    static class ActorTools
    {
        public static ActorStatus GetData(this Actor actor)
        {
            if (actor == null)
            {
                return null;
            }
            if (Main.instance.actorToData.ContainsKey(actor.GetInstanceID()))
            {
                return Main.instance.actorToData[actor.GetInstanceID()];
            }
            else
            {
                ActorStatus data = Reflection.GetField(typeof(Actor),actor,"data") as ActorStatus;
                Main.instance.actorToData.Add(actor.GetInstanceID(), data);
                return data;
            }
        }
        public static BaseStats GetCurStats(this Actor actor)
        {
            if (Main.instance.actorToCurStats.ContainsKey(actor.GetInstanceID()))
            {
                return Main.instance.actorToCurStats[actor.GetInstanceID()];
            }
            else
            {
                BaseStats curStats = Reflection.GetField(typeof(Actor), actor, "curStats") as BaseStats;
                Main.instance.actorToCurStats.Add(actor.GetInstanceID(), curStats);
                return curStats;
            }
            
        }
        public static MoreStats GetMoreStats(this Actor actor)
        {
            if (Main.instance.actorToMoreStats.ContainsKey(actor.GetData().actorID))
            {
                return Main.instance.actorToMoreStats[actor.GetData().actorID];
            }
            else
            {
                MonoBehaviour.print(actor.GetData().actorID + "("+actor.stats.race+")的MoreStats不存在");
                actor.GetData().favorite = true;
                
                return null;
            }
        }
        public static MoreActorData GetMoreData(this Actor actor)
        {
            if (Main.instance.actorToMoreData.ContainsKey(actor.GetData().actorID))
            {
                return Main.instance.actorToMoreData[actor.GetData().actorID];
            }
            else
            {
                MonoBehaviour.print(actor.GetData().actorID + "(" + actor.stats.race + ")的MoreActorData不存在");
                actor.GetData().favorite = true;
                return null;
            }
        }
        public static SpecialBody GetSpecialBody(this Actor actor)
        {
            SpecialBodyLibrary specialBodyLibrary = AddAssetManager.specialBodyLibrary;
            return specialBodyLibrary.get(actor.GetMoreData().specialBody);
        }
        public static void generateNewBody(this Actor actor)
        {
            SpecialBody newBody = new SpecialBody();
            if (actor.GetMoreData().specialBody ==null|| actor.GetMoreData().specialBody == string.Empty)
            {
                actor.GetMoreData().specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            newBody.id = actor.GetData().actorID;
            newBody.madeBy = actor.GetData().firstName;
            newBody.origin = actor.GetSpecialBody().origin;
            newBody.rank = Toolbox.randomInt(1, 4);
            if (newBody.rank > actor.GetSpecialBody().rank)
            {
                actor.GetMoreData().specialBody = newBody.id;
            }
            int index = newBody.rank;
            if(index == 3)
            {
                index = Toolbox.randomInt(3, 6);
            }
            newBody.name = NameGenerator.getName("specialBody_name")+ChineseNameAsset.rankName1[index-1]+"体";
            newBody.mod_damage = Toolbox.randomInt(0,20*newBody.rank);
            newBody.mod_health = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_attack_speed = Toolbox.randomInt(-10, 10 * newBody.rank);
            newBody.mod_speed = Toolbox.randomInt(-10, 5 * newBody.rank);
            newBody.spellRelief = Toolbox.randomInt(0, 5 * newBody.rank);
            AddAssetManager.specialBodyLibrary.add(newBody);
        }
    }
}
