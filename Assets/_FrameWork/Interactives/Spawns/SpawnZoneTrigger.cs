using UnityEngine;
using System.Collections;

public class SpawnZoneTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player" )
        {
            transform.parent.GetComponent<SpawnPoint>().TriggerZone();
        }
    }
}
