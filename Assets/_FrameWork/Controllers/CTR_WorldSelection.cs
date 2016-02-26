using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameWorld 
{
    JUNKYARD,
    BALLOON,
    TEMP1,
    TEMP2,
    TEMP3,
    TEMP4,
    TEMP5,
    COUNT
}

public class CTR_WorldSelection : MonoBehaviour {

    [SerializeField][Tooltip("Set to Starting world.")]
    GameWorld currentWorld;

    

    public void SetCurrentWorld(string world)
    {
        currentWorld = (GameWorld)System.Enum.Parse(typeof(GameWorld), world);
    }

    public void LoadSelectedWorld() 
    {
        string sceneName = currentWorld.ToString() + "_LevelSelect";
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            SceneManager.LoadScene(sceneName);
        }
        else 
        {
            Debug.LogError("Scene " + sceneName + " not found in the build. Try addind the scene in the build settings or ask Leo.");
        }
        
    }

   
	
}
