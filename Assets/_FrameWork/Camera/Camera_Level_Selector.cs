using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Camera_Level_Selector : MonoBehaviour {

    [SerializeField]
    LayerMask clickable;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    [SerializeField]
    float sensitivityX = 15F;
    [SerializeField]
    float sensitivityY = 15F;


    float minimumX = -360F;
    float maximumX = 360F;

    [SerializeField]
    float minimumY = -60F;
    [SerializeField]
    float maximumY = 60F;

    float rotationY = 0F;

    [SerializeField]
    Vector3 baseCameraPosition;

    private bool isMoving = false;
    private bool inputPaused = false;
    Transform target;
    Vector3 startPOS;

    public float speedRatio = 2f;
    float speed = 1f;
    private float startTime;
    private float journeyLength;

    private Quaternion endRotation;
    private CTR_LevelSelection levelSelectionScript;

    [SerializeField]
    GameObject fadeScreen;
    [SerializeField]
    float fadingTime = 3f;
    float fadingStartTime = 0f;

    delegate void UpdateDelegate();
    UpdateDelegate updateDelegate;

    void Awake() 
    {
        levelSelectionScript = GameObject.Find("Controller_LevelSelection").GetComponent<CTR_LevelSelection>();
       
    }

    void Start() 
    {
        updateDelegate += PlayerInput;
    }

	// Update is called once per frame
    void Update()
    {

        if (updateDelegate != null)
        {
            updateDelegate();
        }
    }

    
    #region PlayerInputs
    void PlayerInput() 
    {
        ViewInput();
        if (Input.GetAxis("Fire1") != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f, clickable))
            {
                if (hit.collider.gameObject.name != transform.parent.name)
                {
                    target = hit.collider.transform;
                    transform.SetParent(hit.collider.transform);
                    SelectNewDestination();
                }
            }
        }

        if (Input.GetAxis("Submit") == 1)
        {
            updateDelegate -= PlayerInput;
            updateDelegate += FadeToBack;

            fadingStartTime = Time.time;
            fadeScreen.transform.FindChild("Text").GetComponent<Text>().text = "Loading " + transform.parent.name;
            StartCoroutine(DelaySceneLoad());
            
        }
    }

    void ViewInput() 
    {
        
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Horizontal") * sensitivityX;

            rotationY += Input.GetAxis("Vertical") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Vertical") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }
    #endregion
    #region CameraMovement
    void Move()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.localPosition = Vector3.Lerp(startPOS, baseCameraPosition, fracJourney);
        transform.LookAt(transform.parent.position);

        if (Vector3.Distance(transform.localPosition, baseCameraPosition) < 0.5f)
        {
            rotationY = -transform.localEulerAngles.x;
            updateDelegate -= Move;
            updateDelegate += PlayerInput;
        }
    }

    void SelectNewDestination()
    {
        //Lerping values.
        startPOS = transform.localPosition;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPOS, baseCameraPosition);
        speed = journeyLength / speedRatio;

        updateDelegate += Move;
        updateDelegate -= PlayerInput;
        //Changes the currently selected world in our controller.
        levelSelectionScript.SetCurrentWorld(transform.parent.name);
    }
    #endregion
    #region Utilities
    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(fadingTime);
        updateDelegate += PlayerInput;
        updateDelegate -= FadeToBack;
        levelSelectionScript.LoadSelectedWorld();
    }

    void FadeToBack() 
    {
        fadeScreen.GetComponent<Image>().color = new Color(0f, 0f, 0f, (Time.time - fadingStartTime) / fadingTime);
        fadeScreen.transform.FindChild("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, (Time.time - fadingStartTime) / fadingTime);
    }

    #endregion

}


