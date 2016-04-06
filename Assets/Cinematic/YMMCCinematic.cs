using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class YMMCCinematic : MonoBehaviour {
    public MovieTexture ymmcMovie;
    float timePassed;
    bool startCounting;

    [SerializeField]
    float delaySound;
    [SerializeField]
    float delayVid;

    float duration;
	// Use this for initialization
	void Start () 
    {
        SoundController.Instance.PlayMusic("Cinematic_Intro", true, false);
        StartCoroutine(PlayDelay());
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

    IEnumerator PlayDelay() 
    {
        yield return new WaitForSeconds(delayVid);
        GetComponent<Renderer>().material.mainTexture = ymmcMovie;
        ymmcMovie.Play();
        startCounting = true;
    }
   
}
