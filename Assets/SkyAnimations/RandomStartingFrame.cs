using UnityEngine;
using System.Collections;

//Starts the animator on this at a random frame
public class RandomStartingFrame : MonoBehaviour 
{

    public string anim;

	void Awake ()
    {
        //Ignore if no animator
        if (this.GetComponent<Animator>() == null)
            return;

        GetComponent<Animator>().Play(anim, -1, Random.Range(0f, 1f));
      
	}   

}
