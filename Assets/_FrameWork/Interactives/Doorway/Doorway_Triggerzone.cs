using UnityEngine;
using System.Collections;

public class Doorway_Triggerzone : MonoBehaviour {

    Doorway door;
    public int zoneNumber;

    void Start() 
    {
        door = transform.parent.transform.parent.GetComponent<Doorway>();
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().HasControl())
        {
            door.OnChildrenTriggerEnter(other.gameObject, zoneNumber);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().HasControl())
        {
            door.OnChildrenTriggerExit(other.gameObject, zoneNumber);
        }
    }
}
