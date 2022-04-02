﻿using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Cultivation_Way
{
    class PowerActionLibrary
    {
        public static bool inspectChunk(WorldTile pTile = null, string pPower = null)
        {
            if (pTile != null)
            {
                WindowChunkInfo.open(pTile.chunk);
            }
            return true;
        }
        public static bool spawnYao(WorldTile pTile, string pPowerID)
        {
            GodPower godPower = AssetManager.powers.get(pPowerID);
            Sfx.play("spawn", true, -1f, -1f);
            if (godPower.spawnSound != "")
            {
                Sfx.play(godPower.spawnSound, true, -1f, -1f);
            }
            if (godPower.showSpawnEffect != string.Empty)
            {
                MapBox.instance.stackEffects.CallMethod("startSpawnEffect", pTile, godPower.showSpawnEffect);
            }
            string pStatsID = "unit_Yao";
            ICollection<string> t = Main.instance.MoreActors.protoAndYao.GetSeconds();
            int index = Toolbox.randomInt(0, t.Count);
            pStatsID = t.ElementAt(index);

            Actor Yao = MapBox.instance.spawnNewUnit(pStatsID, pTile, "", godPower.actorSpawnHeight);
            Yao.GetData().level = 5;
            Yao.GetData().health = int.MaxValue >> 2;

            Yao.CallMethod("setProfession", UnitProfession.Unit);
            Yao.setStatsDirty();
            return true;
        }
        public static bool spawnSheng(WorldTile pTile, string pPowerID)
        {
            GodPower godPower = AssetManager.powers.get(pPowerID);
            if (Main.instance.creatureLimit[godPower.actorStatsId] <= 0)
            {
                return false;
            }
            Main.instance.creatureLimit[godPower.actorStatsId]--;
            Sfx.play("spawn", true, -1f, -1f);
            if (godPower.spawnSound != "")
            {
                Sfx.play(godPower.spawnSound, true, -1f, -1f);
            }
            if (godPower.showSpawnEffect != string.Empty)
            {
                MapBox.instance.stackEffects.CallMethod("startSpawnEffect", pTile, godPower.showSpawnEffect);
            }
            Actor Sheng = MapBox.instance.spawnNewUnit(godPower.actorStatsId, pTile, "", godPower.actorSpawnHeight);
            Sheng.GetData().level = 50;
            Sheng.GetMoreData().magic = int.MaxValue >> 2;
            Sheng.GetData().health = int.MaxValue >> 2;

            Sheng.CallMethod("setProfession", UnitProfession.Unit);
            Sheng.setStatsDirty();
            return true;
        }
        public static bool lightningPunishment(BaseSimObject pActor, WorldTile pTile = null)
        {
            if (pActor == null || pActor.objectType != MapObjectType.Actor)
            {
                return false;
            }
            pTile = pActor.currentTile;
            Actor actor = (Actor)pActor;
            int rank = (((Actor)pActor).GetData().level - 1) / 10;
            if (rank > 9)
            {
                rank = 9;
            }
            if (rank < 1)
            {
                rank = 1;
            }
            float size = rank / 6f;
            List<WorldTile> tiles = Utils.OthersHelper.getTilesInRange(pTile, rank*2);
            for (int i = 0; i < rank; i++)
            {
                if (Toolbox.randomChance(rank / 10f)&&tiles.Count>0)
                {
                    WorldTile tile = tiles.GetRandom();
                    Cloud cloud = MapBox.instance.cloudController.getNext();
                    
                    MethodInfo method = AccessTools.Method(typeof(Cloud), "prepare", new System.Type[] { typeof(Vector3), typeof(string) });
                    method.Invoke(cloud, new object[] { new Vector3(tile.posV3.x, tile.posV3.y + 20f), "normal" });
                    cloud.sprRenderer.color = cloud.colorRain;
                    Reflection.SetField<string>(cloud, "dropID", "rain");
                }
                BaseEffect lightning = ((BaseEffectController)MapBox.instance.stackEffects.CallMethod("get", "lightning")).spawnAtRandomScale(pTile, size, size);
            }
            float damage = Toolbox.randomFloat(actor.GetData().health>>4, actor.GetData().health>>2)*rank;
            int num = 0;
            for(int i = 0; i < 5; i++)
            {
                num += actor.GetMoreData().element.baseElementContainer[i] * actor.GetMoreData().element.baseElementContainer[i];
            }
            damage *= num / 2000f;
            actor.CallMethod("getHit",damage, true, AttackType.None, null, true);
            if (actor == null || !actor.base_data.alive)
            {
                return false;
            }
            return true;
        }
    }
}
