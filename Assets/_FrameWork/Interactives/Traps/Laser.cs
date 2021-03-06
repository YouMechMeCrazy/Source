﻿using UnityEngine;
using System.Collections;

public class Laser : InputObject {


    [SerializeField]
    float length;
    Transform Beam;

    [SerializeField]
    bool starOn;

    float laserFXTimer;
    float laserDelayActivating = 0f;

    Transform RespawnLocation;

    bool laserOn = false;

    [SerializeField]
    bool canActivateButtons = false;

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
            laserOn = true;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        
        
        
        if (state == input._On) 
        {
            if (!laserOn)
            {
                laserDelayActivating = SoundController.Instance.PlayFX("Laser_Turn_On", transform.position) + Time.time;
                laserOn = true;
            }

            if (Time.time > laserDelayActivating && Time.time > laserFXTimer)
            {

                laserFXTimer = Time.time + SoundController.Instance.PlayFX("Laser_Active", transform.position);
                
            }
            

            Beam.GetComponent<Renderer>().enabled = true;
            Beam.transform.localPosition = new Vector3(length / 2, Beam.transform.localPosition.y, 0f);
            Beam.transform.localScale = new Vector3(0.1f,length/2, 0.1f);

        }
        if (state == input._Off) 
        {
            if (laserOn)
            {
                SoundController.Instance.PlayFX("Laser_Turn_Off", transform.position);
                laserOn = false;

               
                if (gameObject.transform.FindChild("countDownTimer") != null)
                {
                 
                    gameObject.transform.FindChild("countDownTimer").GetComponent<CountDownTimer>().Enable();
                }

            }
           
            Beam.GetComponent<Renderer>().enabled = false;
        }

        

	}



    void OnTriggerStay(Collider other)
    {
        if (state == input._On)
        {
            if (other.gameObject.tag == "Player" )
            {
                SoundController.Instance.PlayFX("Laser_Hitting_Mech", transform.position);
                GameController.Instance.KillPlayer(other.gameObject.GetComponent<Player>().IsPlayerTwo());
            }

            /*  JOHN COMMENTED THIS OUT SO THAT BOXES DON'T GET DESTROYED
             * 
            if (other.gameObject.GetComponent<Destructable>() && other.gameObject.activeSelf)
            {
                SoundController.Instance.PlayFX("Box_Destroyed_Laser_Metal", transform.position);
                other.gameObject.SetActive(false);
            }
            */

            if (other.gameObject.GetComponent<InteractiveButton>() && canActivateButtons)
            {
                other.gameObject.GetComponent<InteractiveButton>().Hit();
            }
        }

    }

    public bool GetStatus() 
    {
        return laserOn;
    }
}
