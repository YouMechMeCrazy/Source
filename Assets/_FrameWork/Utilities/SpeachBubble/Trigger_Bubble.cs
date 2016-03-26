using UnityEngine;
using System.Collections.Generic;

public class Trigger_Bubble : MonoBehaviour {

    public Speach_Bubble bubble;

    private int playersInZone = 0;

    List<GameObject> players = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < players.Count; i++)
            {
                Debug.Log(players[i].name);
            }
        }
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playersInZone == 0)
            { 
                bubble.AddPopUp();
            }
            players.Add(other.gameObject);
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
            players.Remove(other.gameObject);
        }
    }

}
