using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }

    Transform activeSpawnPoint;

    public Player player1Script;
    public Player player2Script;

    public GameObject p1;
    public GameObject p2;

    bool isPaused = false;

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
        DontDestroyOnLoad(gameObject);
        Time.timeScale = 1f;
    }

	// Use this for initialization
	void Start ()
    {
        SetReferences();
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
        if (!isPlayerTwo)
        {
            player1Script.Death();
        }
        else
        {
            player2Script.Death();
        }
        
    }
   
    public IEnumerator TryRespawnPlayer(bool isPlayerTwo, float waitTime = 1f) 
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

        yield return new WaitForSeconds(waitTime);

        if (!Camera.main.GetComponent<Camera_Controller_SmallFOV>().IsSpawnPointInView())
        {
            Debug.LogError("ReTrying");
            StartCoroutine(TryRespawnPlayer(isPlayerTwo, 0.1f));
        }
        else 
        {
            
            if (!isPlayerTwo)
            {
                if (!player2Script.HasControl() && activeSpawnPoint.GetComponent<SpawnPointOccupationZone>().IsOccupied())
                {
                    p1.transform.position = activeSpawnPoint.FindChild("Alternate").position;
                    p1.transform.FindChild("Legs").gameObject.SetActive(true);
                    p1.transform.FindChild("Arms").gameObject.SetActive(true);
                    player1Script.RespawnPlayer();
                }
                else
                {
                    p1.transform.position = activeSpawnPoint.position;
                    p1.transform.FindChild("Legs").gameObject.SetActive(true);
                    p1.transform.FindChild("Arms").gameObject.SetActive(true);
                    player1Script.RespawnPlayer();
                }

            }
            if (isPlayerTwo)
            {
                if (!player1Script.HasControl() && activeSpawnPoint.GetComponent<SpawnPointOccupationZone>().IsOccupied())
                {
                    p2.transform.position = activeSpawnPoint.FindChild("Alternate").position;
                    p2.transform.FindChild("Legs").gameObject.SetActive(true);
                    p2.transform.FindChild("Arms").gameObject.SetActive(true);
                    player2Script.RespawnPlayer();
                }
                else
                {
                    p2.transform.position = activeSpawnPoint.position;
                    p2.transform.FindChild("Legs").gameObject.SetActive(true);
                    p2.transform.FindChild("Arms").gameObject.SetActive(true);
                    player2Script.RespawnPlayer();
                }

            }
        }

       
    }

    public Vector3 GetactiveSpawnPoint() 
    {
        return activeSpawnPoint.position;
    }

    public void Pause() 
    {
        Debug.Log("pause");
        //playsound
        //show UI
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseScreen.Show();
            Time.timeScale = 0f;
        }
        else 
        {
            Time.timeScale = 1f;
            pauseScreen.Hide();
        }
    }
}
