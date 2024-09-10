using MSCLoader;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using FridgeAPI;

namespace Cooler_Box
{
    public class Cooler_Box : Mod
    {
        public override string ID => "Cooler_Box"; // Your (unique) mod ID 
        public override string Name => "Cooler Box"; // Your mod name
        public override string Author => "Trimpsuz"; // Name of the Author (your name)
        public override string Version => "1.2.3"; // Version
        public override string Description => "Introduces two new cooler boxes to Teimo's store. Half as effective as the regular fridge while closed, and slightly over twice as effective as nothing while open."; // Short description of your mod

        public static GameObject cooler;
        public static GameObject cooler1;
        private AssetBundle assets;
        private Camera camera;
        public static Transform lid;
        public static Transform lid1;
        private BoxCollider lidCollider;
        private BoxCollider lid1Collider;
        private GameObject coolerStore;
        private GameObject cooler1Store;
        private GameObject gameObject;
        public static bool coolerPurchased;
        public static bool cooler1Purchased;
        public static bool cooler1inRegister = false;
        public static bool coolerinRegister = false;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnNewGame, Mod_NewGame);
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

        private void Mod_NewGame()
        {
            if (File.Exists(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
            {
                File.Delete(Path.Combine(Application.persistentDataPath, "CoolerBox.json"));
            }
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

            coolerinRegister = false;
            cooler1inRegister = false;

            assets = LoadAssets.LoadBundle(Properties.Resources.cooler);
            gameObject = assets.LoadAsset<GameObject>("cooler.prefab");
            cooler = Object.Instantiate(gameObject);
            cooler1 = Object.Instantiate(gameObject);
            assets.Unload(false);
            cooler.name = "Cooler(Clone)";
            cooler.layer = LayerMask.NameToLayer("Parts");
            cooler.tag = "PART";
            lid = cooler.transform.Find("Lid");
            lid.gameObject.layer = LayerMask.NameToLayer("DontCollide");
            lidCollider = cooler.GetComponents<BoxCollider>()[11];
            inCooler inCooler = cooler.AddComponent<inCooler>();
            inCooler.cooler = cooler;
            inCooler.lid = lid;
            cooler.MakePickable();

            cooler1.name = "Cooler(Clone)";
            cooler1.layer = LayerMask.NameToLayer("Parts");
            cooler1.tag = "PART";
            lid1 = cooler1.transform.Find("Lid");
            lid1.gameObject.layer = LayerMask.NameToLayer("DontCollide");
            lid1Collider = cooler1.GetComponents<BoxCollider>()[11];
            inCooler inCooler1 = cooler1.AddComponent<inCooler>();
            inCooler1.cooler = cooler1;
            inCooler1.lid = lid1;
            cooler1.MakePickable();

            if (File.Exists(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
            {
                using (FileStream stream = File.OpenRead(Path.Combine(Application.persistentDataPath, "CoolerBox.json")))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamReader sr = new StreamReader(stream))
                    using (JsonTextReader reader = new JsonTextReader(sr))
                    {
                        Save s = serializer.Deserialize<Save>(reader);
                        cooler.transform.position = new Vector3(s.coolerx, s.coolery, s.coolerz);
                        cooler.transform.rotation = Quaternion.Euler(s.coolerxRot, s.cooleryRot, s.coolerzRot);
                        coolerPurchased = s.coolerPurchased;

                        cooler1.transform.position = new Vector3(s.cooler1x, s.cooler1y, s.cooler1z);
                        cooler1.transform.rotation = Quaternion.Euler(s.cooler1xRot, s.cooler1yRot, s.cooler1zRot);
                        cooler1Purchased = s.cooler1Purchased;
                    }
                }
            } else
            {
                coolerPurchased = false;
                cooler.GetComponent<Rigidbody>().isKinematic = true;
                cooler.GetComponent<Rigidbody>().detectCollisions = false;
                cooler.transform.position = new Vector3(-1551.303f, 4.8f, 1181.904f);
                cooler.transform.eulerAngles = new Vector3(270f, 328f, 0f);

                cooler1Purchased = false;
                cooler1.GetComponent<Rigidbody>().isKinematic = true;
                cooler1.GetComponent<Rigidbody>().detectCollisions = false;
                cooler1.transform.position = new Vector3(-1551.303f, 4.8f, 1181.904f);
                cooler1.transform.eulerAngles = new Vector3(270f, 328f, 0f);
            }
        }

        private void Mod_OnLoad()
        {
            if (!ModLoader.IsModPresent("FridgeAPI"))
            {
                return;
            }

            if(!coolerPurchased)
            { 
                cooler.SetActive(false);
                this.coolerStore = Object.Instantiate(gameObject);
                Object.Destroy(gameObject);
                CoolerStore coolerStore = this.coolerStore.AddComponent<CoolerStore>();
                coolerStore.cooler = cooler;
                coolerStore.coolerPos = new Vector3(-1546.887f, 3.901165f, 1184.129f);
                coolerStore.cooler1 = false;

                cooler.GetComponent<Rigidbody>().isKinematic = true;
                cooler.GetComponent<Rigidbody>().detectCollisions = false;
                cooler.transform.position = new Vector3(-1551.303f, 4.8f, 1181.904f);
                cooler.transform.eulerAngles = new Vector3(270f, 328f, 0f);
            }
            if (!cooler1Purchased)
            {
                cooler1.SetActive(false);
                this.cooler1Store = Object.Instantiate(gameObject);
                Object.Destroy(gameObject);
                CoolerStore cooler1Store = this.cooler1Store.AddComponent<CoolerStore>();
                cooler1Store.cooler = cooler1;
                cooler1Store.coolerPos = new Vector3(-1546.887f, 4.222f, 1184.129f);
                cooler1Store.cooler1 = true;

                cooler1.GetComponent<Rigidbody>().isKinematic = true;
                cooler1.GetComponent<Rigidbody>().detectCollisions = false;
                cooler1.transform.position = new Vector3(-1551.303f, 4.8f, 1181.904f);
                cooler1.transform.eulerAngles = new Vector3(270f, 328f, 0f);
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
                Save s = new Save(cooler.transform.position, cooler.transform.rotation, coolerPurchased, cooler1.transform.position, cooler1.transform.rotation, cooler1Purchased);
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

                    if (hit.collider.gameObject == cooler1 || hit.collider.gameObject == lid1.gameObject)
                    {
                        PlayMakerGlobals.Instance.Variables.GetFsmBool("GUIuse").Value = true;
                        if (lid1.gameObject.activeSelf)
                        {
                            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = "Open Lid";
                        }
                        else
                        {
                            PlayMakerGlobals.Instance.Variables.GetFsmString("GUIinteraction").Value = "Close Lid";
                        }
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            if (lid1.gameObject.activeSelf)
                            {
                                lid1.gameObject.SetActive(false);
                                lid1Collider.isTrigger = true;
                                cooler1.gameObject.GetComponent<Fridge>().customSpoilingRate = 0.015f;
                                if (cooler1.transform.childCount > 2)
                                {
                                    foreach (Component component in cooler1.GetComponents<Component>())
                                    {
                                        if (component.GetType().Name == "FixedJoint")
                                        {
                                            Object.Destroy(component);
                                        }
                                    }

                                    foreach (Rigidbody rigidbody in cooler1.GetComponentsInChildren<Rigidbody>())
                                    {
                                        rigidbody.detectCollisions = true;
                                        rigidbody.constraints = 0;
                                        rigidbody.gameObject.transform.parent = null;
                                        rigidbody.drag = 1f;
                                        rigidbody.mass = 1f;
                                    }
                                }

                                cooler1.gameObject.GetComponent<Rigidbody>().mass = 1f;
                            }
                            else
                            {
                                lid1.gameObject.SetActive(true);
                                lid1Collider.isTrigger = false;
                                cooler1.gameObject.GetComponent<Fridge>().customSpoilingRate = 0.001f;
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
        public float coolerx, coolery, coolerz;
        public float coolerxRot, cooleryRot, coolerzRot;
        public bool coolerPurchased;
        public float cooler1x, cooler1y, cooler1z;
        public float cooler1xRot, cooler1yRot, cooler1zRot;
        public bool cooler1Purchased;

        public Save() { }
        public Save(Vector3 coolerpos, Quaternion coolerrot, bool coolerpurchased, Vector3 cooler1pos, Quaternion cooler1rot, bool cooler1purchased)
        {
            coolerx = coolerpos.x;
            coolery = coolerpos.y;
            coolerz = coolerpos.z;

            coolerxRot = coolerrot.eulerAngles.x;
            cooleryRot = coolerrot.eulerAngles.y;
            coolerzRot = coolerrot.eulerAngles.z;

            coolerPurchased = coolerpurchased;

            cooler1x = cooler1pos.x;
            cooler1y = cooler1pos.y;
            cooler1z = cooler1pos.z;

            cooler1xRot = cooler1rot.eulerAngles.x;
            cooler1yRot = cooler1rot.eulerAngles.y;
            cooler1zRot = cooler1rot.eulerAngles.z;

            cooler1Purchased = cooler1purchased;
        }
    }
}
