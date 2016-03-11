using UnityEngine;
using System.Collections;

public class Trigger_Bubble : MonoBehaviour {

    public Speach_Bubble bubble;

    private int playersInZone = 0;

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playersInZone == 0)
            { 
                bubble.AddPopUp();
            }
                
            playersInZone++;
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInZone--;
            if (playersInZone == 0)
            {
                bubble.AddPopDown();
            }
        }
    }

}
