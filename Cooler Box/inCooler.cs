using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MSCLoader;

namespace Cooler_Box
{
    internal class inCooler : MonoBehaviour
    {
        private static FixedJoint joint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PART"))
            {
                other.gameObject.GetComponent<Rigidbody>().drag = 10f;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("PART"))
            {
                other.gameObject.GetComponent<Rigidbody>().drag = 1f;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Cooler_Box.lid.gameObject.activeSelf)
            {
                if (other.gameObject.layer == 19 && (other.gameObject.CompareTag("PART") || other.gameObject.CompareTag("ITEM")) && (other.name.Contains("(itemx)") || other.name.Contains("(Clone)")))
                {
                    joint = Cooler_Box.cooler.AddComponent<FixedJoint>();
                    joint.connectedBody = other.GetComponent<Rigidbody>();
                    other.transform.parent = Cooler_Box.cooler.transform;
                    other.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                }
            }
        }
    }
}
