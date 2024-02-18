using MSCLoader;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using FridgeAPI;

namespace Cooler_Box
{
    public class Cooler_Box : Mod
    {
        public override string ID => "Cooler_Box"; // Your (unique) mod ID 
        public override string Name => "Cooler Box"; // Your mod name
        public override string Author => "Trimpsuz"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "A usable cooler box. Half as good as the fridge."; // Short description of your mod

        public static GameObject cooler;
        private AssetBundle assets;
        private Camera camera;
        public static Transform lid;
        private BoxCollider lidCollider;

        public override void ModSetup()
        {
            SetupFunction(Setup.PreLoad, Mod_PreLoad);
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.ModSettings, Mod_Settings);
            SetupFunction(Setup.PostLoad, Mod_PostLoad);
        }

        private void Mod_Settings()
        {
            // All settings should be created here. 
            // DO NOT put anything that isn't settings or keybinds in here!
        }

        private void Mod_PostLoad()
        {
            if (!ModLoader.IsModPresent("FridgeAPI"))
            {
                ModConsole.Log("<color=red><b>Cooler Box: No Fridge API present! Mod won't work!</b></color>");
                return;
            }
        }

        private void Mod_PreLoad()
        {
            if (!ModLoader.IsModPresent("FridgeAPI"))
            {
                return;
            }

            assets = LoadAssets.LoadBundle(Properties.Resources.cooler);
            cooler = assets.LoadAsset<GameObject>("cooler.prefab");
            cooler = Object.Instantiate(cooler);
            assets.Unload(false);
            cooler.name = "Cooler(Clone)";
            cooler.layer = LayerMask.NameToLayer("Parts");
            cooler.tag = "PART";
            lid = cooler.transform.Find("Lid");
            lid.gameObject.layer = LayerMask.NameToLayer("DontCollide");
            lidCollider = cooler.GetComponents<BoxCollider>()[11];
            cooler.AddComponent<inCooler>();
            cooler.MakePickable();

            if(File.Exists(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
            {
                using (FileStream stream = File.OpenRead(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamReader sr = new StreamReader(stream))
                    using (JsonTextReader reader = new JsonTextReader(sr))
                    {
                        Save s = serializer.Deserialize<Save>(reader);
                        cooler.transform.position = new Vector3(s.x, s.y, s.z);
                        cooler.transform.rotation = Quaternion.Euler(s.xRot, s.yRot, s.zRot);
                    }
                }
            }
        }

        private void Mod_OnLoad()
        {
            if (!ModLoader.IsModPresent("FridgeAPI"))
            {
                return;
            }

            if (!File.Exists(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
            {
                cooler.transform.position = GameObject.Find("PLAYER").transform.position;
            }
        }

        private void Mod_OnSave()
        {
            if (!ModLoader.IsModPresent("FridgeAPI"))
            {
                return;
            }

            using (FileStream stream = File.OpenWrite(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
            {
                Save s = new Save(cooler.transform.position, cooler.transform.rotation);
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(stream))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, s);
                }
            }

        }
        private void Mod_Update()
        {
            if (!ModLoader.IsModPresent("FridgeAPI"))
            {
                return;
            }

            if (camera == null)
            {
                camera = Camera.main;
            } else
            {
                foreach (RaycastHit hit in Physics.RaycastAll(camera.transform.position, camera.transform.forward, 1f))
                {
                    if(hit.collider.gameObject == cooler || hit.collider.gameObject == lid.gameObject)
                    {
                        PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = true;
                        if(lid.gameObject.activeSelf)
                        {
                            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = "Open Lid";
                        } else
                        {
                            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = "Close Lid";
                        }
                        if(Input.GetKeyDown(KeyCode.F))
                        {
                            if(lid.gameObject.activeSelf)
                            {
                                lid.gameObject.SetActive(false);
                                lidCollider.isTrigger = true;
                                cooler.gameObject.GetComponent<Fridge>().customSpoilingRate = 0.015f;
                                if (cooler.transform.childCount > 2)
                                {
                                    foreach (Component component in cooler.GetComponents<Component>())
                                    {
                                        if (component.GetType().Name == "FixedJoint")
                                        {
                                            Object.Destroy(component);
                                        }
                                    }

                                    foreach (Rigidbody rigidbody in cooler.GetComponentsInChildren<Rigidbody>())
                                    {
                                        rigidbody.detectCollisions = true;
                                        rigidbody.constraints = 0;
                                        rigidbody.gameObject.transform.parent = null;
                                        rigidbody.drag = 1f;
                                        rigidbody.mass = 1f;
                                    }
                                }

                                cooler.gameObject.GetComponent<Rigidbody>().mass = 1f;
                            } else
                            {
                                lid.gameObject.SetActive(true);
                                lidCollider.isTrigger = false;
                                cooler.gameObject.GetComponent<Fridge>().customSpoilingRate = 0.001f;
                            }
                        }
                        break;
                    }
                }
            }
            
        }
    }

    public class Save
    {
        public float x, y, z;
        public float xRot, yRot, zRot;

        public Save() { }
        public Save(Vector3 pos, Quaternion rot)
        {
            x = pos.x;
            y = pos.y;
            z = pos.z;

            xRot = rot.eulerAngles.x;
            yRot = rot.eulerAngles.y;
            zRot = rot.eulerAngles.z;
        }
    }
}
