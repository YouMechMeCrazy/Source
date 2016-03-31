using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        SoundController.Instance.Stop();
	}

    void Update() 
    {
        if (Input.GetButtonDown("Submit2") || Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
            SceneManager.LoadScene("LEVEL1");
            
        }
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
            SceneManager.LoadScene("LEVEL1");
        }

       

	}
}
