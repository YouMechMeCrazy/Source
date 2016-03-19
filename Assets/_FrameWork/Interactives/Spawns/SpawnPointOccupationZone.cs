using UnityEngine;
using System.Collections;

public class SpawnPointOccupationZone : MonoBehaviour {

   bool isOccupied = false;

    public bool IsOccupied() 
    {
        return isOccupied;
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            isOccupied = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOccupied = false;
        }
    }
}
