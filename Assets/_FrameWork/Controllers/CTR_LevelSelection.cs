using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CTR_LevelSelection : MonoBehaviour {

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

    public float fadingTime = 3f;
    public LayerMask clickable;
    public GameObject fadeScreen;
    float fadingStartTime;

    //gets the info from the gameController/manager
    GameWorld currentWorld = GameWorld.LEVEL1;

    public int numberOfLevels = 1;

    int selectedLevel = 1;
    GameObject world;

    List<Transform> levels = new List<Transform>();

    bool isLoadingNewLevel = false; 

    void Awake() 
    {
        //Todo link the info from the gameController. Currently defaultewd to JUNKYARD
        world = GameObject.Find(currentWorld.ToString()).gameObject;
    }
	// Use this for initialization
	void Start () 
    {
        int numberOfLevels = world.transform.childCount;

        for (int i = 1; i <= numberOfLevels; i++)
        {
            levels.Add(world.transform.FindChild(i.ToString()));
        }

	}

    void Update() 
    {
        if (!isLoadingNewLevel)
        {
            PlayerInput();
        }
        else 
        {
            FadeToBack();
        }
        
    }

    void PlayerInput() 
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 185f, clickable))
            {
                fadingStartTime = Time.time;
                StartCoroutine(DelaySceneLoad());
                isLoadingNewLevel = true;
                selectedLevel = int.Parse(hit.collider.gameObject.name);
                
            }
        }
        ViewInput();
    }

    void ViewInput()
    {

        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = world.transform.localEulerAngles.y + Input.GetAxis("Horizontal") * sensitivityX;

            rotationY += Input.GetAxis("Vertical") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            world.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            world.transform.Rotate(0, Input.GetAxis("Horizontal") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Vertical") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            world.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    public void LoadSelectedLevel()
    {
        string sceneName = currentWorld.ToString() + "_" + selectedLevel;
        if (selectedLevel <= numberOfLevels)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " not found in the build. Try addind the scene in the build settings or ask Leo.");
        }

    }

    #region Utilities
    IEnumerator DelaySceneLoad()
    {
        yield return new WaitForSeconds(fadingTime);
        LoadSelectedLevel();
    }

    void FadeToBack()
    {
        fadeScreen.GetComponent<Image>().color = new Color(0f, 0f, 0f, (Time.time - fadingStartTime) / fadingTime);
        fadeScreen.transform.FindChild("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, (Time.time - fadingStartTime) / fadingTime);
    }

    #endregion

}
