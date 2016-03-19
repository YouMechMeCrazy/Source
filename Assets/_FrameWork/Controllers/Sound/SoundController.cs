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


    public float PlayFX(string fx) 
    {
        if (soundDB.soundFXDic[fx].clip != null)
        {
            aud.PlayOneShot(soundDB.soundFXDic[fx].clip, soundDB.soundFXDic[fx].volume);
            return soundDB.soundFXDic[fx].clip.length;
        }
        else 
        {
            Debug.LogError("Can't load resource: Sound FX " + fx);
        }

        return 0f;
    }
    public void PlayMusic(string music)
    {
        if (soundDB.musicDic[music].clip != null)
        {
            aud.clip = soundDB.musicDic[music].clip;
            aud.volume = soundDB.musicDic[music].volume;
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
