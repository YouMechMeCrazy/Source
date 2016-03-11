using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class Sound_Music_Holder : MonoBehaviour {

    [System.Serializable]
    public struct Music 
    {
        public string name;
        public AudioClip clip;
    }
    [System.Serializable]
    public struct SoundFX
    {
        public string name;
        public AudioClip clip;
    }

    public static Sound_Music_Holder Instance { get; private set; }


    public Dictionary<string, AudioClip> soundFXDic = new Dictionary<string, AudioClip>();

    public Dictionary<string, AudioClip> musicDic = new Dictionary<string, AudioClip>();

    public Music[] musicClips;
    public SoundFX[] soundFXClips;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);



        Load_Music();
        Load_SoundFX();
    }

  

    void Load_SoundFX() 
    {
        for (int i = 0; i < soundFXClips.Length; i++)
        {
            soundFXDic.Add(soundFXClips[i].name, soundFXClips[i].clip);
        }
            
    }

    void Load_Music()
    {
        for (int i = 0; i < musicClips.Length; i++)
        {
            musicDic.Add(musicClips[i].name, musicClips[i].clip);
        }
    }

}
