using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerUpsMuck
{
    class SuperCritLifesteal
    {
        [HarmonyPatch(typeof(ItemManager), "InitAllPowerups")]
        [HarmonyPostfix]
        private static void Postfix(ItemManager __instance)
        {
            Powerup[] newPowerups = { CreatePowerup(__instance) };
            __instance.powerupsOrange = __instance.powerupsOrange.Concat(newPowerups).ToArray();

            Debug.Log(__instance.powerupsOrange.Last().name);
        }

        private static Powerup CreatePowerup(ItemManager __instance)
        {
            int id = __instance.allPowerups.Count;
            Powerup powerup1 = ScriptableObject.CreateInstance<Powerup>();
            powerup1.name = "Super Lifesteal";
            powerup1.description = "Bigboi";
            powerup1.id = id;
            powerup1.mesh = __instance.allPowerups[1].mesh;
            powerup1.material = __instance.allPowerups[1].material;
            powerup1.sprite = __instance.allPowerups[1].sprite;
            powerup1.tier = Powerup.PowerTier.Orange;


            __instance.allPowerups.Add(id, powerup1);
            __instance.stringToPowerupId.Add(powerup1.name, id);

            return powerup1;
        }

        [HarmonyPatch(typeof(PowerupInventory), "GetCritChance")]
        [HarmonyPostfix]
        static void GetCritChance(ref float __result, PowerupInventory __instance)
        {
            float num = PowerupInventory.CumulativeDistribution(__instance.powerups[ItemManager.Instance.stringToPowerupId["Super Lifesteal"]], 0.1f, 0.9f);
            __result = __result + num;
        }

        [HarmonyPatch(typeof(PowerupCalculations), "GetDamageMultiplier")]
        [HarmonyPostfix]
        static void GetDamageMultiplier(ref PowerupCalculations.DamageResult __result, PowerupCalculations __instance)
        {
            if (__result.crit == true)
            {
                __result.lifesteal += PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Super Lifesteal"]];
            }
        }
    }
}
