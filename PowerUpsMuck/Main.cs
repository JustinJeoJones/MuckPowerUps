using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PowerUpsMuck
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "PowerUpsMuck",
            AUTHOR = "Jeo",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "1.0.0.0";

        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        #endregion

        public Main()
        {
            log = Logger;
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
            
        }

        public void Start()
        {
            harmony.PatchAll(assembly);
        }

        public void Awake()
        {
            harmony.PatchAll(typeof(SuperCritLifesteal));
            harmony.PatchAll(typeof(DrugAddict));
            harmony.PatchAll(typeof(Photosynthesis));
        }
    }
}
