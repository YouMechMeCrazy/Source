using UnityEngine;
using System.Collections;

//Starts the animator on this at a random frame
public class RandomStartingFrame : MonoBehaviour 
{

	void Awake ()
    {
        //Ignore if no animator
        if (this.GetComponent<Animator>() == null)
            return;
     
        GetComponent<Animator>().Play("StarAnim", -1, Random.Range(0f, 1f));
	}   

}
