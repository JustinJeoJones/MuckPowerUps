using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerUpsMuck
{
    class Photosynthesis
    {
        [HarmonyPatch(typeof(ItemManager), "InitAllPowerups")]
        [HarmonyPostfix]
        private static void Postfix(ItemManager __instance)
        {
            Powerup[] newPowerups = { CreatePowerup(__instance) };
            __instance.powerupsBlue = __instance.powerupsBlue.Concat(newPowerups).ToArray();

            Debug.Log(__instance.powerupsBlue.Last().name);
        }

        private static Powerup CreatePowerup(ItemManager __instance)
        {
            int id = __instance.allPowerups.Count;
            Powerup powerup1 = ScriptableObject.CreateInstance<Powerup>();
            powerup1.name = "Photosynthesis";
            powerup1.description = "No more eating in the day";
            powerup1.id = id;
            powerup1.mesh = __instance.allPowerups[1].mesh;
            powerup1.material = __instance.allPowerups[1].material;
            powerup1.sprite = __instance.allPowerups[1].sprite;
            powerup1.tier = Powerup.PowerTier.Blue;


            __instance.allPowerups.Add(id, powerup1);
            __instance.stringToPowerupId.Add(powerup1.name, id);

            return powerup1;
        }

        [HarmonyPatch(typeof(PlayerStatus), "Hunger")]
        [HarmonyPrefix]
        static void Hunger(PlayerStatus __instance)
        {
            float time = DayCycle.time;
            int num = (12 + (int)(time * 24f)) % 24;
            //Debug.Log(num);
            if (num > 12)
            {
                //__instance.hunger += (0.015f * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Photosynthesis"]]);

                if (__instance.running != true)
                {
                    __instance.hunger += (0.015f * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Photosynthesis"]]);
                }

                if (__instance.hunger > __instance.maxHunger)
                {
                    __instance.hunger = __instance.maxHunger;
                }
            }
        }
    }
}
