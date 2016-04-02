using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ClipData
{
    public AudioClip clip;
    public float volume;
    public string name;

    public ClipData(AudioClip c, float v, string n) 
    {
        clip = c;
        volume = v;
        name = n;
    }
}

public class Sound_Music_Holder : MonoBehaviour {

    [System.Serializable]
    public struct Music 
    {
        public string name;
        public AudioClip clip;
        public float volume;
    }
    [System.Serializable]
    public struct SoundFX
    {
        public string name;
        public AudioClip clip;
        public float volume;
    }



    public static Sound_Music_Holder Instance { get; private set; }


    public Dictionary<string, ClipData> soundFXDic = new Dictionary<string, ClipData>();

    public Dictionary<string, ClipData> voDIC = new Dictionary<string, ClipData>();

    public Dictionary<string, ClipData> musicDic = new Dictionary<string, ClipData>();

    public Music[] musicClips;
    public SoundFX[] soundFXClips;
    public SoundFX[] voiceOverClips;

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
        Load_VoiceOver();

        
    }

  

    void Load_SoundFX() 
    {
        for (int i = 0; i < soundFXClips.Length; i++)
        {
            ClipData newClipData = new ClipData(soundFXClips[i].clip,soundFXClips[i].volume , soundFXClips[i].name);
            soundFXDic.Add(soundFXClips[i].name, newClipData);
        }
            
    }

    void Load_VoiceOver()
    {
        for (int i = 0; i < voiceOverClips.Length; i++)
        {
            ClipData newClipData = new ClipData(voiceOverClips[i].clip, voiceOverClips[i].volume, voiceOverClips[i].name);
            voDIC.Add(voiceOverClips[i].name, newClipData);
        }

    }

    void Load_Music()
    {
        for (int i = 0; i < musicClips.Length; i++)
        {
            ClipData newClipData = new ClipData(musicClips[i].clip, musicClips[i].volume, musicClips[i].name);
            musicDic.Add(musicClips[i].name, newClipData);
        }
    }

    public float GetClipDuration(string clip, bool isSoundFX, bool isVoiceOver = false) 
    {
        if (isVoiceOver)
        {
            return voDIC[clip].clip.length;
        }
        if (isSoundFX)
        {

            return soundFXDic[clip].clip.length;
        }
        else
        {
            return musicDic[clip].clip.length;
        }
    }

}
