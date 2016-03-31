using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameWorld 
{
    LEVEL1,
    LEVEL2,
    LEVEL3,
    LEVEL4,
    LEVEL5,
    LEVEL6,
    LEVEL7,
    COUNT
}

public class CTR_WorldSelection : MonoBehaviour {

    [SerializeField][Tooltip("Set to Starting world.")]
    GameWorld currentWorld;

    void Start() 
    {
        SoundController.Instance.PlayMusic("SpaceAdventure");
    }

    public void SetCurrentWorld(string world)
    {
        currentWorld = (GameWorld)System.Enum.Parse(typeof(GameWorld), world);
        
    }

    public void LoadSelectedWorld() 
    {
        string sceneName = currentWorld.ToString();

        sceneName = "LoadScreen";
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else 
        {
            Debug.LogError("Scene " + sceneName + " not found in the build. Try addind the scene in the build settings or ask Leo.");
        }
        
    }

    public int GetCurrentWorld() 
    {
        return (int)currentWorld;
    }

   
	
}
