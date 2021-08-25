using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerUpsMuck
{
    class DrugAddict
    {
        [HarmonyPatch(typeof(ItemManager), "InitAllPowerups")]
        [HarmonyPostfix]
        private static void Postfix(ItemManager __instance)
        {
            Powerup[] newPowerups = { CreatePowerup(__instance) };
            __instance.powerupsWhite = __instance.powerupsWhite.Concat(newPowerups).ToArray();

            Debug.Log(__instance.powerupsWhite.Last().name);
        }

        private static Powerup CreatePowerup(ItemManager __instance)
        {
            int id = __instance.allPowerups.Count;
            Powerup powerup1 = ScriptableObject.CreateInstance<Powerup>();
            powerup1.name = "Drug Addict";
            powerup1.description = "Shrooms and shroom soups are more effective";
            powerup1.id = id;
            powerup1.mesh = __instance.allPowerups[1].mesh;
            powerup1.material = __instance.allPowerups[1].material;
            powerup1.sprite = __instance.allPowerups[1].sprite;
            powerup1.tier = Powerup.PowerTier.White;


            __instance.allPowerups.Add(id, powerup1);
            __instance.stringToPowerupId.Add(powerup1.name, id);

            return powerup1;
        }

        [HarmonyPatch(typeof(PlayerStatus), "Eat")]
        [HarmonyPrefix]
        static void GetCritChance(ref InventoryItem item)
        {
            float modifer = 4;
            float soupBonus = 1;
            if (item.name.Contains("Gulpon Shroom"))
            {
                Debug.Log(item.heal);
                item.heal += modifer * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
                Debug.Log(item.heal);
            }
            else if (item.name.Contains("Ligon Shroom"))
            {
                item.hunger += modifer * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
            }
            else if (item.name.Contains("Sugon Shroom"))
            {
                item.stamina += modifer * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
            }
            else if (item.name.Contains("Slurbon Shroom"))
            {
                item.heal += modifer * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
                item.hunger += modifer * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
                item.stamina += modifer * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
            }
            else if (item.name.Contains("Yellow Soup") || item.name.Contains("Purple Soup") || item.name.Contains("Red Soup") || item.name.Contains("Weird Soup"))
            {
                item.heal += (modifer + soupBonus) * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
                item.hunger += (modifer + soupBonus) * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
                item.stamina += (modifer + soupBonus) * PowerupInventory.Instance.powerups[ItemManager.Instance.stringToPowerupId["Drug Addict"]];
            }
        }
    }
}
