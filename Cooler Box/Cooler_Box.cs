using MSCLoader;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Newtonsoft.Json;

namespace Cooler_Box
{
    public class Cooler_Box : Mod
    {
        public override string ID => "Cooler_Box"; // Your (unique) mod ID 
        public override string Name => "Cooler Box"; // Your mod name
        public override string Author => "Trimpsuz"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "A usable cooler box. Half as good as the fridge."; // Short description of your mod

        public GameObject cooler;
        public AssetBundle assets;
        public Camera camera;
        public Transform lid;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.ModSettings, Mod_Settings);
        }

        private void Mod_Settings()
        {
            // All settings should be created here. 
            // DO NOT put anything that isn't settings or keybinds in here!
        }

        private void Mod_OnLoad()
        {
            assets = LoadAssets.LoadBundle(Properties.Resources.cooler);
            cooler = assets.LoadAsset<GameObject>("cooler.prefab");
            cooler = Object.Instantiate(cooler);
            assets.Unload(false);
            cooler.name = "Cooler(Clone)";
            cooler.layer = LayerMask.NameToLayer("Parts");
            cooler.tag = "PART";
            lid = cooler.transform.Find("Lid");
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
            } else
            {
                cooler.transform.position = GameObject.Find("PLAYER").transform.position;
            }
        }
        private void Mod_OnSave()
        {
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
            if(camera == null)
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
                            lid.gameObject.SetActive(!lid.gameObject.activeSelf);
                        }
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
