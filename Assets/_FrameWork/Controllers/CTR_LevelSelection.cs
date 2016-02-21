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

public class CTR_LevelSelection : MonoBehaviour {

    [SerializeField][Tooltip("Set to Starting world.")]
    GameWorld currentWorld;

    

    public void SetCurrentWorld(string world)
    {
        currentWorld = (GameWorld)System.Enum.Parse(typeof(GameWorld), world);
    }

    public void LoadSelectedWorld() 
    {
        Debug.Log("World " + currentWorld + " Selected");
        SceneManager.LoadScene("w_2_lvl_1_Junkyard");
    }

   
	
}
