using FridgeAPI;
using System.Linq;
using UnityEngine;

namespace Cooler_Box
{
    internal class inCooler : MonoBehaviour
    {
        private static FixedJoint joint;
        public Transform lid;
        public GameObject cooler;

        private void OnTriggerStay(Collider other)
        {
            if (lid.gameObject.activeSelf)
            {
                if(other.gameObject.layer == 19 && (other.gameObject.CompareTag("PART") || other.gameObject.CompareTag("ITEM")) && (!BlacklistedItems.blacklistedItems.Any(blacklistedItem => other.gameObject.name.ToLower().Contains(blacklistedItem)) || other.gameObject.name.ToLower().Contains("grilled")))
                {
                    cooler.gameObject.GetComponent<Rigidbody>().mass += other.gameObject.GetComponent<Rigidbody>().mass;
                    joint = cooler.AddComponent<FixedJoint>();
                    joint.connectedBody = other.GetComponent<Rigidbody>();
                    other.transform.parent = cooler.transform;
                    other.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                    other.gameObject.GetComponent<Rigidbody>().drag = 0f;
                    other.gameObject.GetComponent<Rigidbody>().mass = 0f;
                }
            }
        }
    }
}
