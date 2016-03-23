using UnityEngine;
using System.Collections;

public class EndLevel_Zone : MonoBehaviour {

    [SerializeField]
    int numberOfRequiredPlayers = 1;

    private int playersInZone = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInZone++;
            if (playersInZone >= numberOfRequiredPlayers)
            { 
                GameController.Instance.LevelOver();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInZone--;
        }
    }
}
