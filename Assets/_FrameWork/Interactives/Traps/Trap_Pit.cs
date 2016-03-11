using UnityEngine;
using System.Collections;

public class Trap_Pit : MonoBehaviour {



    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>().HasControl())
        {
            GameController.Instance.KillPlayer(other.gameObject.GetComponent<Player>().IsPlayerTwo());
        }
    }
}
