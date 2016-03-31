using UnityEngine;
using System.Collections;

//Starts the animator on this at a random frame
public class RandomStartingFrame : MonoBehaviour 
{
    int fDelay = 25;
    int cFrame = 0;

    bool reset = false;

    float originalSpeed = 1f;
	// Use this for initialization
	void Awake ()
    {
        //Ignore if no animator
        if (this.GetComponent<Animator>() == null)
            return;

       

        //Save original speed reference and set to something insane
        originalSpeed = this.GetComponent<Animator>().speed;
        
        this.GetComponent<Animator>().speed = Random.Range(0, 2000f);
        
       
	}

    void FixedUpdate() 
    {
        if (reset)
        {
            return;
        }
        if (cFrame == fDelay)
        {
            this.GetComponent<Animator>().speed = originalSpeed;
        }
        cFrame++;
       
    }

}
