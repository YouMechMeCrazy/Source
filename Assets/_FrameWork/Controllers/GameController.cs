using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }

    Transform activeSpawnPoint;

    public Player player1Script;
    public Player player2Script;

    public GameObject p1;
    public GameObject p2;

    bool isP1Dead = false;
    bool isP2Dead = false;

    bool isPaused = false;
    float timeAtPause = 0f;

    PauseScreen pauseScreen;
     

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance.gameObject);
            Instance = this;
            Time.timeScale = 1f;
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        Time.timeScale = 1f;
    }

	// Use this for initialization
	void Start ()
    {
        SetReferences();
        SoundController.Instance.PlayMusic("BroJam");
	}


    public void SetReferences()
    {
        player1Script = GameObject.Find("John").GetComponent<Player>();
        player2Script = GameObject.Find("Brian").GetComponent<Player>();
        p1 = GameObject.Find("John");
        p2 = GameObject.Find("Brian");
        pauseScreen = GameObject.Find("PauseScreen").GetComponent<PauseScreen>();
    }

    public void SetActiveSpawnPoint(Transform sP) 
    {
        if (activeSpawnPoint != null)
        {
            activeSpawnPoint.transform.parent.GetComponent<SpawnPoint>().SetInactive();
        }
        
        activeSpawnPoint = sP;
    }

    public void KillPlayer(bool isPlayerTwo) 
    {
        if (!isPlayerTwo && !isP1Dead)
        {
            isP1Dead = true;
            player1Script.Death();
            
        }
        else if (isPlayerTwo && !isP2Dead)
        {
            isP2Dead = true;
            player2Script.Death();
        }
        
    }
   
    public void TryRespawnPlayer(bool isPlayerTwo, float waitTime = 1f) 
    {
    


        if (!isPlayerTwo && p1.activeSelf)
        {
            p1.transform.FindChild("Legs").gameObject.SetActive(false);
            p1.transform.FindChild("Arms").gameObject.SetActive(false);
        }
        else if (isPlayerTwo && p2.activeSelf)
        {
            p2.transform.FindChild("Legs").gameObject.SetActive(false);
            p2.transform.FindChild("Arms").gameObject.SetActive(false);
        }

        

        if (!Camera.main.GetComponent<Camera_Controller_SmallFOV>().IsSpawnPointInView())
        {
            Debug.LogError("not in view");
            
            StartCoroutine(DelayTryRespawn(isPlayerTwo, 1f));
        }
        else 
        {
            
            if (!isPlayerTwo)
            {
                if ( activeSpawnPoint.GetComponent<SpawnPointOccupationZone>().IsOccupied())
                {
                    p1.transform.position = activeSpawnPoint.FindChild("Alternate").position;
                    p1.transform.FindChild("Legs").gameObject.SetActive(true);
                    p1.transform.FindChild("Arms").gameObject.SetActive(true);
                    isP1Dead = false;
                    player1Script.RespawnPlayer();
                }
                else
                {
                    p1.transform.position = activeSpawnPoint.position;
                    p1.transform.FindChild("Legs").gameObject.SetActive(true);
                    p1.transform.FindChild("Arms").gameObject.SetActive(true);
                    isP1Dead = false;
                    player1Script.RespawnPlayer();
                }

            }
            if (isPlayerTwo)
            {
                if ( activeSpawnPoint.GetComponent<SpawnPointOccupationZone>().IsOccupied())
                {
                    p2.transform.position = activeSpawnPoint.FindChild("Alternate").position;
                    p2.transform.FindChild("Legs").gameObject.SetActive(true);
                    p2.transform.FindChild("Arms").gameObject.SetActive(true);
                    isP2Dead = false;
                    player2Script.RespawnPlayer();
                }
                else
                {
                    p2.transform.position = activeSpawnPoint.position;
                    p2.transform.FindChild("Legs").gameObject.SetActive(true);
                    p2.transform.FindChild("Arms").gameObject.SetActive(true);
                    isP2Dead = false;
                    player2Script.RespawnPlayer();
                }

            }
        }

       

    }

    IEnumerator DelayTryRespawn(bool isP2, float waitTime) 
    {
        yield return new WaitForSeconds(3f);     
        TryRespawnPlayer(isP2, waitTime);
    }

    public Vector3 GetactiveSpawnPoint() 
    {
        return activeSpawnPoint.position;
    }

    public void Pause() 
    {
        //playsound
        timeAtPause = Time.time;
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseScreen.Show();
            pauseScreen.GetComponent<PauseScreen>().isPaused = true;
            Time.timeScale = 0f;
        }
        else 
        {
            pauseScreen.GetComponent<PauseScreen>().isPaused = false;
            Time.timeScale = 1f;
            pauseScreen.Hide();
        }
    }

    public float TimeAtPause() 
    {
        return timeAtPause;
    }

    public void LevelOver()
    {
        player1Script.SetPlayerControl(false);
        player2Script.SetPlayerControl(false);
        player1Script.EndOfLevel();
        player2Script.EndOfLevel();
        // Play music
        if (Camera.main.GetComponent<Cam_Cinematic>() != null)
        {
            Camera.main.GetComponent<Camera_Controller_SmallFOV>().PlayOutro();
            StartCoroutine(DelayLevelOver(Camera.main.GetComponent<Cam_Cinematic>().GetDuration(Cinematic_Type.OUTRO)));
        }

    }



    IEnumerator DelayLevelOver(float delay) 
    {
        yield return new WaitForSeconds(delay);


        SceneManager.LoadScene("MainMenuScene");
    }

}
