using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

    public GameObject boxDestroy;

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.GetComponent<Destructable>().forceField && other.gameObject.activeSelf)
        {
            other.gameObject.SetActive(false);

            Instantiate(boxDestroy, transform.localPosition, Quaternion.identity);
        }
        if (other.gameObject.GetComponent<Destructable>() == null)
        {
            return;
        }
    }

}
