using UnityEngine;
using System.Collections;

public class Laser : InputObject {


    [SerializeField]
    float length;
    Transform Beam;

    [SerializeField]
    bool starOn;

    
    Transform RespawnLocation;

	// Use this for initialization
	void Start () {

        Beam = transform.FindChild("LaserBeam");
        if (starOn) 
        { 
            state = input._Off; 
        } 
        else 
        { 
            state = input._On; 
        }
	}
	
	// Update is called once per frame
	void Update () 
    {

        if (state == input._On) 
        {
            Beam.GetComponent<Renderer>().enabled = true;
            Beam.transform.localPosition = new Vector3(length / 2, Beam.transform.localPosition.y, 0f);
            Beam.transform.localScale = new Vector3(0.1f,length/2, 0.1f);

        }
        if (state == input._Off) 
        {
            Beam.GetComponent<Renderer>().enabled = false;
        }

	}



    void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Player" && state == input._On)
        {
            GameController.Instance.KillPlayer(other.gameObject.GetComponent<Player>().IsPlayerTwo());
        }

        if (other.gameObject.GetComponent<Destructable>() && other.gameObject.activeSelf)
        {
            other.gameObject.SetActive(false);
        }
    }
}
