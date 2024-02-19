using FridgeAPI;
using UnityEngine;

namespace Cooler_Box
{
    internal class inCooler1 : MonoBehaviour
    {
        private static FixedJoint joint;

        private void OnTriggerStay(Collider other)
        {
            if (Cooler_Box.lid1.gameObject.activeSelf)
            {
                if (other.gameObject.layer == 19 && (other.gameObject.GetComponent<SpoilableFood>() != null || other.gameObject.name.Contains("juice") || other.gameObject.name.Contains("yeast") || other.gameObject.name.Contains("sugar") || other.gameObject.name.Contains("ground coffee") || other.gameObject.name.Contains("potato chips")) && other.gameObject.name != "Cooler(Clone)")
                {
                    Cooler_Box.cooler1.gameObject.GetComponent<Rigidbody>().mass += other.gameObject.GetComponent<Rigidbody>().mass;
                    joint = Cooler_Box.cooler1.AddComponent<FixedJoint>();
                    joint.connectedBody = other.GetComponent<Rigidbody>();
                    other.transform.parent = Cooler_Box.cooler1.transform;
                    other.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                    other.gameObject.GetComponent<Rigidbody>().drag = 0f;
                    other.gameObject.GetComponent<Rigidbody>().mass = 0f;
                }
            }
        }
    }
}
