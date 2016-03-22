using UnityEngine;
using System.Collections;

//Starts the animator on this at a random frame
public class RandomStartingFrame : MonoBehaviour 
{
    float originalSpeed = 1;
	// Use this for initialization
	void Awake ()
    {
        //Ignore if no animator
       // if (this.GetComponent<Animator>() == null)
         //   return;

        //Save original speed reference and set to something insane
        originalSpeed = this.GetComponent<Animator>().speed;
        this.GetComponent<Animator>().speed = Random.Range(0, 2000);

        Invoke("ResetSpeed", 0.1f);
	}

    void ResetSpeed()
    {
        this.GetComponent<Animator>().speed = originalSpeed;
    }
}
