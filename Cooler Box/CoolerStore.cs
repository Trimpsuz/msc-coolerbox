using FridgeAPI;
using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Cooler_Box
{
    internal class CoolerStore : MonoBehaviour
    {
        private PlayMakerFSM cashRegister;
        private FsmBool guiBuy;
        private FsmString guiInteraction;
        private CoolerStoreFSM coolerStoreFSM;
        private bool orderToPay;
        public GameObject cooler;
        private bool mouseOver;
        private Camera camera;
        private Transform lid;
        private Transform handle;

        private void Start()
        {
            Destroy(GetComponent<Fridge>());
            transform.SetParent(GameObject.Find("STORE").transform.Find("LOD/ActivateStore/FoodProducts"));
            gameObject.layer = LayerMask.NameToLayer("DontCollide");
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.position = new Vector3(-1546.887f, 3.901165f, 1184.129f);
            transform.eulerAngles = new Vector3(270f, 147f, 180f);
            lid = transform.Find("Lid");
            handle = transform.Find("Handle");
            cashRegister = GameObject.Find("STORE/StoreCashRegister/Register").GetComponent<PlayMakerFSM>();
            guiBuy = PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIbuy");
            guiInteraction = PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction");
            FsmState fsmState = cashRegister.FsmStates.FirstOrDefault((FsmState state) => state.Name == "Purchase");
            coolerStoreFSM = new CoolerStoreFSM
            {
                action = new Action(OrderPurhcase)
            };
            List<FsmStateAction> list = fsmState.Actions.ToList();
            list.Insert(0, coolerStoreFSM);
            fsmState.Actions = list.ToArray();
        }

        private void Update()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            else
            {
                foreach (RaycastHit hit in Physics.RaycastAll(camera.transform.position, camera.transform.forward, 1f))
                {
                    if (hit.collider.gameObject == gameObject || hit.collider.gameObject == lid.gameObject || hit.collider.gameObject == handle.gameObject)
                    {
                        if (orderToPay)
                        {
                            mouseOver = true;
                            guiBuy.Value = true;
                            guiInteraction.Value = "Cooler Box 50 mk";
                            if (Input.GetMouseButtonDown(1))
                            {
                                mouseOver = true;
                                orderToPay = false;
                                GetComponent<MeshRenderer>().enabled = true;
                                lid.GetComponent<MeshRenderer>().enabled = true;
                                handle.GetComponent<MeshRenderer>().enabled = true;
                                cashRegister.FsmVariables.GetFsmFloat("PriceTotal").Value -= 50f;
                                cashRegister.SendEvent("PURCHASE");
                                return;
                            }
                        }
                        else
                        {
                            mouseOver = true;
                            guiBuy.Value = true;
                            guiInteraction.Value = "Cooler Box 50 mk";
                            if (Input.GetMouseButtonDown(0))
                            {
                                mouseOver = true;
                                orderToPay = true;
                                GetComponent<MeshRenderer>().enabled = false;
                                lid.GetComponent<MeshRenderer>().enabled = false;
                                handle.GetComponent<MeshRenderer>().enabled = false;
                                cashRegister.FsmVariables.GetFsmFloat("PriceTotal").Value += 50f;
                                cashRegister.SendEvent("PURCHASE");
                                return;
                            }
                        }
                        break;
                    }
                    else if (mouseOver)
                    {
                        mouseOver = false;
                        guiBuy.Value = false;
                        guiInteraction.Value = "";
                    }
                }
            }
        }

        private void OrderPurhcase()
        {
            if(orderToPay)
            {
                orderToPay = false;
                cooler.SetActive(true);
                cooler.GetComponent<Rigidbody>().isKinematic = false;
                FsmState fsmState = cashRegister.FsmStates.FirstOrDefault((FsmState state) => state.Name == "Purchase");
                List<FsmStateAction> list = fsmState.Actions.ToList<FsmStateAction>();
                list.Remove(coolerStoreFSM);
                fsmState.Actions = list.ToArray();
                Destroy(gameObject);
                Cooler_Box.coolerPurchased = true;
            }
        }

        private class CoolerStoreFSM : FsmStateAction
        {
            public override void OnEnter()
            {
                action();
                base.Finish();
            }

            public Action action;
        }
    }


}
