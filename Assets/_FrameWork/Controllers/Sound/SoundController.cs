using UnityEngine;
using System.Collections;


public class SoundController : MonoBehaviour {


    public static SoundController Instance { get; private set; }

    AudioSource aud;
    Sound_Music_Holder soundDB;

    [SerializeField]
    float minAudioRange = 5f;
    [SerializeField]
    float maxAudioRange = 20f;

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

        aud.pitch = 1f;
    }


    public float PlayFX(string fx, Vector3 location) 
    {
       
        if (soundDB.soundFXDic[fx].clip != null)
        {
            if (location.y == -999f)
            {
                aud.PlayOneShot(soundDB.soundFXDic[fx].clip, soundDB.soundFXDic[fx].volume);
                return soundDB.soundFXDic[fx].clip.length;
            }
            else
            {
                PlayClipAt(soundDB.soundFXDic[fx].clip, location, soundDB.soundFXDic[fx].volume);
                return soundDB.soundFXDic[fx].clip.length;
            }
           
        }
        else
        {
            Debug.LogError("Can't load resource: Sound FX " + fx);
        }
        
    

        return 0f;
    }

    public void PlayMusic(string music, bool overrideMusic = false)
    {
        if (soundDB.musicDic[music].clip != null)
        {
            //Check if we are Playing the same music.
            if (!overrideMusic)
            {
                if (aud.clip == soundDB.musicDic[music].clip)
                {
                    return;
                }
            }
            aud.clip = soundDB.musicDic[music].clip;
            aud.volume = soundDB.musicDic[music].volume;
            aud.Play();
        }
        else 
        {
            Debug.LogError("Can't load resource: Music " + music);
        }
        
    }

    public float PlayVO(string VO)
    {
        if (soundDB.voDIC[VO].clip != null)
        {
            aud.PlayOneShot(soundDB.voDIC[VO].clip, soundDB.voDIC[VO].volume);
            return soundDB.voDIC[VO].clip.length;
        }
        else
        {
            Debug.LogError("Can't load resource: voice over " + VO);
        }

        return 0f;
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

    public float Volume(float amount, bool setAt = false) 
    {
        if (setAt)
        {
            float vol = aud.volume;
            aud.volume = amount;
            return vol;
        }
        aud.volume += amount;
        return aud.volume;
    }

    public SoundController() 
    {
        
    }

    AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float aVolume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        aSource.minDistance = minAudioRange;
        aSource.maxDistance = maxAudioRange;
        aSource.volume = aVolume;
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

	
}
