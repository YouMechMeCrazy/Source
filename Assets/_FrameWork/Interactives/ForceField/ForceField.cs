using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Destructable>() == null)
        {
            return;
        }
        if (other.gameObject.GetComponent<Destructable>().forceField && other.gameObject.activeSelf)
        {
            other.gameObject.SetActive(false);
        }
    }

}
