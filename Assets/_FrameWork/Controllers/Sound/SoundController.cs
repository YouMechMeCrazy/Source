using UnityEngine;
using System.Collections;


public class SoundController : MonoBehaviour {


    public static SoundController Instance { get; private set; }

    AudioSource aud;
    Sound_Music_Holder soundDB;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        aud = GetComponent<AudioSource>();
        soundDB = GetComponent<Sound_Music_Holder>();
    }


    public void PlayFX(string fx) 
    {
        if (soundDB.soundFXDic[fx] != null)
        {
            aud.PlayOneShot(soundDB.soundFXDic[fx]);
        }
        else 
        {
            Debug.LogError("Can't load resource: Sound FX " + fx);
        }
        
    }
    public void PlayMusic(string music)
    {
        if (soundDB.musicDic[music] != null)
        {
            aud.clip = soundDB.musicDic[music];
            aud.Play();
        }
        else 
        {
            Debug.LogError("Can't load resource: Music " + music);
        }
        
    }

    public void Loop(bool setLooping) 
    {
        aud.loop = setLooping;
    }

    public void Stop() 
    {
        aud.Stop();
        aud.clip = null;
    }

    public SoundController() 
    {
        
    }
	
}
