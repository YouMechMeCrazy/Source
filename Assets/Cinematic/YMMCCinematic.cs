using UnityEngine;
using System.Collections;

public class YMMCCinematic : MonoBehaviour {
    public MovieTexture ymmcMovie;
    float timePassed;
    bool startCounting;
	// Use this for initialization
	void Start () 
    {
        GetComponent<Renderer>().material.mainTexture = ymmcMovie;
        ymmcMovie.Play();
        startCounting = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if(startCounting == true)
        {
            timePassed += Time.fixedDeltaTime;
        }
        if(timePassed >= ymmcMovie.duration)
        {
            Application.LoadLevel(1);
        }
	}
}
