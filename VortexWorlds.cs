using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace VortexWorlds
{
    public static class ModInfo
    {
        public const string Name = "Vortex Worlds";
        public const string Version = "1.0.0";
    }

    [BepInPlugin("vortex-worlds", ModInfo.Name, ModInfo.Version)]
    public class VortexWorlds : BaseUnityPlugin
    {
        private const string VortexWorldsDir = "vortex-worlds";

        private static void Log(string msg) => Debug.Log($"[vortex-worlds] {msg}");
        private static void Error(string msg) => Debug.LogError($"[vortex-worlds] {msg}");

        private void Awake()
        {
            // Sync files from the vortex-worlds folder to the worlds folder
            var vortexWorldsDir = Path.Combine(Utils.GetSaveDataPath(), VortexWorldsDir);
            var valheimWorldsDir = Path.Combine(Utils.GetSaveDataPath(), "worlds");

            if (!Directory.Exists(vortexWorldsDir))
            {
                // Definitely nothing for us to do if the directory doesn't exist...
                return;
            }   
            
            // Make sure the Valheim worlds directory is created, always
            if (!Directory.Exists(valheimWorldsDir))
            {
                Directory.CreateDirectory(valheimWorldsDir);
            }

            IEnumerable<string> GetWorldsFromDir(string dir) => Directory
                .GetFiles(dir, "*.fwl")
                .Select(p => Path.GetFileNameWithoutExtension(p));

            // Determine if there are any new worlds that need deploying
            var valheimWorlds = GetWorldsFromDir(valheimWorldsDir);
            var vortexWorlds = GetWorldsFromDir(vortexWorldsDir);
            var worldsToDeploy = vortexWorlds.Except(valheimWorlds, StringComparer.InvariantCultureIgnoreCase).ToList();
            
            if (worldsToDeploy.Count > 0)
            {
                Log($"Found {worldsToDeploy.Count} new worlds to deploy");
                foreach (var world in worldsToDeploy)
                {
                    // Generate a manifest of files to copy
                    var sourceFiles = Directory.GetFiles(vortexWorldsDir, $"{world}.*");
                    var sourceAndTarget = sourceFiles
                        .Select(path => new
                            {src = path, tgt = Path.Combine(valheimWorldsDir, Path.GetFileName(path))})
                        .ToList();
                    // If any of the source files already exist then we don't copy anything ... 
                    if (sourceAndTarget.Any(st => File.Exists(st.tgt)))
                    {
                        Error($"Deploying {world} failed as one of more files already existed in the target directory ...");
                    }
                    else
                    {
                        try
                        {
                            foreach (var st in sourceAndTarget)
                            {
                                File.Copy(st.src, st.tgt, overwrite: false);
                            }
                            Log($"Deploying {world} completed");
                        }
                        catch (Exception ex)
                        {
                            // We think we checked all conditions above, but its always possible something changed in the 
                            // meantime. 
                            Error($"Deploying {world} failed due to an exception occurring: {ex.Message}");
                            // We will NOT try and "clean up" here, because we don't know what happened, and might make things
                            // worse. 
                        }
                    }
                }
            }
        }
    }
}
