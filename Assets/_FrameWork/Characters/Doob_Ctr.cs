using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Doob_Ctr : MonoBehaviour {

    [SerializeField]
    List<AudioClip> clips = new List<AudioClip>();

    AudioSource aud;
    Animator anim;


    float swapAnim = 10f;
    float starTime;
  
    bool talking = false;

	// Use this for initialization
	void Awake () 
    {
        aud = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        starTime = Time.time + swapAnim;


	}
	
	// Update is called once per frame
	void Update () 
    {
        if (starTime < Time.time && !talking)
        {
            float rng = Random.Range(0f, 1f);

            if (rng > 0.5f)
            {
                anim.Play("idle");
            }
            else
            {
                anim.Play("idleAlt");
            }
            starTime = Time.time + swapAnim;
        }
	}

    public void PlayClip() 
    {
        talking = true;
        anim.SetBool("talking", true);
        AudioClip clip = clips[Random.Range(0, clips.Count)];

        aud.PlayOneShot(clip);

        StartCoroutine(EndOfSpeach(clip.length));
    }

    IEnumerator EndOfSpeach(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("talking", false);
        talking = false;
    }

}
