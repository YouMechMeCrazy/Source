using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    float mainVolume = 1f;

    [SerializeField]
    GameObject fadeInScreen;
    [SerializeField]
    float fadeInDuration = 1.5f;
    private float fadeStartTime;
    private bool isFadingIn = true;

    [SerializeField]
    GameObject levelSplash;
    [SerializeField]
    float delayBeforeEndLevelSplash = 5f;
    [SerializeField]
    float endLevelSplashScaleSpeed = 0.05f;
    private bool isEndOfLevel = false;
    private float scale = 0f;

    public Speach_Bubble tempBubbleFixLevelUp;
   
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
        SoundController.Instance.PlayMusic("Level_1", true);
        fadeStartTime = Time.time;
	}

    void Update() 
    {
        if (isFadingIn)
        {
            FadeIn();
        }

        if (isEndOfLevel)
        {
            scale += endLevelSplashScaleSpeed;
            if (scale >= 1f)
            {
                scale = 1f;
                isEndOfLevel = false;
            }
            levelSplash.transform.localScale = new Vector3(scale, scale, scale);
        }
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
            //this plays the animation and sound fx.
            activeSpawnPoint.transform.parent.GetComponent<SpawnPoint>().SpawnPlayer();
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
            mainVolume = SoundController.Instance.Volume(0.25f, true);
            pauseScreen.Show();
            pauseScreen.GetComponent<PauseScreen>().isPaused = true;
            Time.timeScale = 0f;
        }
        else 
        {
            SoundController.Instance.Volume(mainVolume, true);
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

        //SoundController.Instance.PlayMusic("Level_Completed", true);

        if (Camera.main.GetComponent<Cam_Cinematic>() != null)
        {
            Camera.main.GetComponent<Camera_Controller_SmallFOV>().PlayOutro();

            StartCoroutine(EndLevelSplash(delayBeforeEndLevelSplash));

            StartCoroutine(DelayLevelOver(Camera.main.GetComponent<Cam_Cinematic>().GetDuration(Cinematic_Type.OUTRO)));
        }

    }

    IEnumerator EndLevelSplash(float delay) 
    {
        yield return new WaitForSeconds(delay);

        tempBubbleFixLevelUp.FixTheThing();
        SoundController.Instance.PlayFX("LVLComplete", new Vector3(0f,-999f,0f));
        isEndOfLevel = true;
        levelSplash.SetActive(true);
    }

    IEnumerator DelayLevelOver(float delay) 
    {
        yield return new WaitForSeconds(delay);


        SceneManager.LoadScene("MainMenuScene");
    }

    void FadeIn()
    {
        fadeInScreen.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f - ((Time.time - fadeStartTime) / fadeInDuration));

        if ((Time.time - fadeStartTime) > fadeInDuration)
        {
            isFadingIn = false;
            fadeInScreen.SetActive(false);
        }
    }

}
