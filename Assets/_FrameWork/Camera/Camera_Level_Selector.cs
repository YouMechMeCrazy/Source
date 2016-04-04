using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Camera_Level_Selector : MonoBehaviour {

    [SerializeField]
    LayerMask clickable;



    float rotationY = 0f;

    [SerializeField]
    Vector3 baseCameraPosition;

    private bool isMoving = false;
    private bool inputPaused = false;
    Transform target;
    Vector3 startPOS;

    public float speedRatio = 1f;
    float speed = 1f;
    private float startTime;
    private float journeyLength;

    private Quaternion endRotation;
    private CTR_WorldSelection levelSelectionScript;

    [SerializeField]
    GameObject fadeScreen;
    [SerializeField]
    float fadingTime = 3f;
    float fadingStartTime = 0f;

    Transform targetPlanet;

    delegate void UpdateDelegate();
    UpdateDelegate updateDelegate;

    [SerializeField]
    GameObject p1;
    [SerializeField]
    string p1Name;
    [SerializeField]
    GameObject p2;
    [SerializeField]
    string p2Name;
    [SerializeField]
    GameObject p3;
    [SerializeField]
    string p3Name;
    [SerializeField]
    GameObject p4;
    [SerializeField]
    string p4Name;
    [SerializeField]
    GameObject p5;
    [SerializeField]
    string p5Name;
    [SerializeField]
    GameObject p6;
    [SerializeField]
    string p6Name;
    [SerializeField]
    GameObject p7;
    [SerializeField]
    string p7Name;


    string[] planetsName = new string[7];
    GameObject[] planets = new GameObject[7];


    [SerializeField]
    Text leftText;
    [SerializeField]
    Text rightText;
    [SerializeField]
    GameObject lockUI;

    int planetSelected = 1;

    float inputDelay = 0.5f;
    float inputDelayTimer;
    float fadeInStartTime;

    void Awake() 
    {
        levelSelectionScript = GameObject.Find("Controller_LevelSelection").GetComponent<CTR_WorldSelection>();
        targetPlanet = p1.transform;
        p1.transform.FindChild("Outline").gameObject.SetActive(true);

        planets[0] = p1;
        planets[1] = p2;
        planets[2] = p3;
        planets[3] = p4;
        planets[4] = p5;
        planets[5] = p6;
        planets[6] = p7;

        planetsName[0] = p1Name;
        planetsName[1] = p2Name;
        planetsName[2] = p3Name;
        planetsName[3] = p4Name;
        planetsName[4] = p5Name;
        planetsName[5] = p6Name;
        planetsName[6] = p7Name;
    }

    void Start() 
    {
        fadeInStartTime = Time.time;
        inputDelayTimer = Time.time;
        updateDelegate += FadeIn;
        updateDelegate += RotatePlanet;
        StartCoroutine(DelayOnLoad());
    }

	// Update is called once per frame
    void Update()
    {

        if (updateDelegate != null)
        {
            updateDelegate();
        }

    }

    void PlayerInput()
    {
        if(Time.time > inputDelayTimer + inputDelay)
        {
            if (Input.GetAxis("Horizontal") > 0.5f || Input.GetAxis("Horizontal2") > 0.5f)
            {
                inputDelayTimer = Time.time;
                int previous = planetSelected;
                planetSelected++;
          
                if (planetSelected > 7)
                {
                    planetSelected = 1;
                }
                targetPlanet = planets[planetSelected - 1].transform;

                target = targetPlanet;
                transform.SetParent(targetPlanet);

                planets[previous-1].transform.FindChild("Outline").gameObject.SetActive(false);
                targetPlanet.transform.FindChild("Outline").gameObject.SetActive(true);

               

               

                SelectNewDestination();

            }
            else if (Input.GetAxis("Horizontal") < -0.5f || Input.GetAxis("Horizontal2") < -0.5f)
            {
                inputDelayTimer = Time.time;
                int previous = planetSelected;
                planetSelected--;
                if (planetSelected < 1)
                {
                    planetSelected = 7;
                }
                targetPlanet = planets[planetSelected - 1].transform;


                target = targetPlanet;
                transform.SetParent(targetPlanet);

                planets[previous - 1].transform.FindChild("Outline").gameObject.SetActive(false);
                targetPlanet.transform.FindChild("Outline").gameObject.SetActive(true);

                SelectNewDestination();

            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (levelSelectionScript.GetCurrentWorld() == 0)
            {
                updateDelegate -= PlayerInput;
                updateDelegate += FadeToBack;

                SoundController.Instance.PlayFX("Menu_Select", new Vector3(0f, -999f, 0f));

                fadingStartTime = Time.time;
                fadeScreen.transform.FindChild("Text").GetComponent<Text>().text = "Loading " + transform.parent.name;
                StartCoroutine(DelaySceneLoad());
            }
            else 
            {
                Debug.LogError("Level does not exist yet.");
            }

        }

        
    }

   

    #region CameraMovement
    void Move()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.localPosition = Vector3.Lerp(startPOS, baseCameraPosition, fracJourney);
        //transform.LookAt(transform.parent.position);
        

        if (Vector3.Distance(transform.localPosition, baseCameraPosition) < 0.5f)
        {
            rotationY = -transform.localEulerAngles.x;
            updateDelegate -= Move;
            updateDelegate += PlayerInput;

            if (planetSelected == 1)
            {
                lockUI.SetActive(false);
            }
            else
            {
                lockUI.SetActive(true);
            }
            
        }
    }

    void RotatePlanet()
    {
        //////////////////////////
        Vector3 targetDir = targetPlanet.position - transform.position;

        float step = 15f * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
       


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDir), step);

        ///////////////////////////
    }

    void SelectNewDestination()
    {
        if (planetSelected != 1)
        {
            leftText.text = planetsName[planetSelected - 2];
        }
        else
        {
            leftText.text = planetsName[6];
        }
        if (planetSelected != 7)
        {
            rightText.text = planetsName[planetSelected];
        }
        else
        {
            rightText.text = planetsName[0];
        }

        lockUI.SetActive(false);

        //Lerping values.
        startPOS = transform.localPosition;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPOS, baseCameraPosition);
        speed = journeyLength / speedRatio;

        //updateDelegate += RotatePlanet;
        updateDelegate += Move;
        updateDelegate -= PlayerInput;
        //Changes the currently selected world in our controller.
        levelSelectionScript.SetCurrentWorld(transform.parent.name);

        SoundController.Instance.PlayFX("Menu_Move", new Vector3(0f, -999f, 0f));
    }
    #endregion


    #region Utilities

    IEnumerator DelayOnLoad() 
    {
        yield return new WaitForSeconds(fadingTime);
        updateDelegate += PlayerInput;
        updateDelegate -= FadeIn;
    }

    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(fadingTime);
        updateDelegate += PlayerInput;
        updateDelegate -= FadeToBack;
        levelSelectionScript.LoadSelectedWorld();
    }

    void FadeToBack() 
    {
        SoundController.Instance.Volume(-0.015f / fadingTime);
        fadeScreen.GetComponent<Image>().color = new Color(0f, 0f, 0f, (Time.time - fadingStartTime) / fadingTime);
        fadeScreen.transform.FindChild("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, (Time.time - fadingStartTime) / fadingTime);
    }

    void FadeIn() 
    {
        fadeScreen.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f-( ( Time.time - fadeInStartTime) / fadingTime) );
    }



    #endregion

}


