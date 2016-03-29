using UnityEngine;
using System.Collections;

public enum CameraBounds
{
    TOP,
    BOT,
    RIGHT,
    LEFT,
    END
}

public class Camera_Controller_SmallFOV : MonoBehaviour {


    //Currently set as a click and drag. Can change to assign on start.
    public Transform player1;
    public Transform player2;


    [Tooltip("Layer mask we hit with screen side raycasts. Bounds is our default and only layer we want to hit.")]
    public LayerMask ground;//use this layer for level ground.
    [Tooltip("Layer mask we hit with screen side raycasts. Bounds is our default and only layer we want to hit.")]
    public LayerMask bounds;//use this layer for level bounds.
    [SerializeField]
    [Tooltip("The factor by which the Y position of the camera get multiplied by. Lower number means a more zoomed in environment.")]
    float zoomFactor = 5f;
    [SerializeField]
    [Tooltip("Speed of movement of the Camera.")]
    float smoothing = 2f;
    [SerializeField]
    [Tooltip("Angle of the Camera on the X axis. Adjust this value to match the camera Transform. WARNING: Low angles may cause the camera to skip or jitter!")]
    float cameraAngle = 80f;

    [SerializeField]
    float maxDistanceZoomedOut = 500f;
    [SerializeField]
    float minDistanceZoomedOut = 150f;

    [SerializeField]
    Cam_Cinematic levelCinematic;

    delegate void CinematicDelegate(Cinematic_Type type);
    CinematicDelegate dCin;


    Ray[] cameraBounds = new Ray[4];//Quad rays that delimit the camera view. (top, bot, right, left)
    float[] fieldOfViewBounds = new float[4];
    bool[] boundTouching = new bool[4] { false, false, false, false};

    Vector3 camOffset = new Vector3(0f,0f,0f);

    Transform topB;
    Transform downB;
    Transform rightB;
    Transform leftB;



     float xOffset = 0f;
     float zOffset = 0f;
     float xColliderSizeRation;
     float zColliderSizeRation;
     float prevZTopcolliderSize = 11.64441f;
     float prevZBotcolliderSize = 11.64441f;
     float prevXLeftcolliderSize = 17.92398f; 
     float prevXRightcolliderSize = 17.92398f;

     private Cinematic_Type currrentCinematicType;

    void Awake()
    {
        topB = transform.FindChild("Top");
        downB = transform.FindChild("Down");
        rightB = transform.FindChild("Right");
        leftB = transform.FindChild("Left");

      
    }

    public void PlayOutro() 
    {
        if (levelCinematic != null && levelCinematic.stepsOutro.Count > 0)
        {
            //player control handled by gamecontroller
            levelCinematic.Reset();
            dCin += levelCinematic.Cinematic;
            currrentCinematicType = Cinematic_Type.OUTRO;
        }
    }

    // Use this for initialization
    void Start () 
    {
        if (levelCinematic != null && levelCinematic.stepsIntro.Count > 0)
        {
            player1.GetComponent<Player>().SetPlayerControl(false);
            player2.GetComponent<Player>().SetPlayerControl(false);
            dCin += levelCinematic.Cinematic;
            currrentCinematicType = Cinematic_Type.INTRO;
            StartCoroutine(WaitForCinematicCamera(levelCinematic.GetDuration(Cinematic_Type.INTRO)));
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (dCin != null)
        {
            dCin(currrentCinematicType);
        }
        else
        {

            CameraRayCasting();
        }
       
        
    }

    IEnumerator WaitForCinematicCamera(float delay) 
    {
        yield return new WaitForSeconds(delay);
        dCin -= levelCinematic.Cinematic;

      
 
        player1.GetComponent<Player>().SetPlayerControl(true);
        player2.GetComponent<Player>().SetPlayerControl(true);

        player1.transform.rotation = Quaternion.identity;
        player2.transform.rotation = Quaternion.identity;
    }

    Vector3 previousPOS = new Vector3(0f,0f,0f);
   
    void CameraRayCasting()
    {
        previousPOS = new Vector3( transform.position.x,  transform.position.y,  transform.position.z);
        #region RaycastingRegion
        cameraBounds[0] = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height, 0));//top
        cameraBounds[1] = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, 0, 0));//bot
        cameraBounds[2] = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height / 2f, 0));//right
        cameraBounds[3] = Camera.main.ScreenPointToRay(new Vector3(0, Screen.height / 2f, 0));//left

        for (int i = 0; i < 4; i++)
        {
            RaycastHit hit;
            //Set collider bounds
            if (Physics.Raycast(cameraBounds[i], out hit, 10000f, ground))
            {
                if (i == 0)
                {
                    fieldOfViewBounds[0] = hit.point.z;
                    topB.position = hit.point;
                }
                if(i == 1)
                {
                    fieldOfViewBounds[1] = hit.point.z;
                    downB.position = hit.point;
                }
                if (i == 2)
                {
                    fieldOfViewBounds[2] = hit.point.x;
                    rightB.position = hit.point;
                }
                if (i == 3)
                {
                    fieldOfViewBounds[3] = hit.point.x;
                    leftB.position = hit.point;
                }
            }

            if (Physics.Raycast(cameraBounds[i], out hit, 10000f, bounds))
            {
                boundTouching[i] = true;
            }
            else
            {
                boundTouching[i] = false;
            }


        }
       
    

        topB.GetComponent<BoxCollider>().size = new Vector3((fieldOfViewBounds[2] - fieldOfViewBounds[3]), 40f, 3f);
        downB.GetComponent<BoxCollider>().size = new Vector3((fieldOfViewBounds[2] - fieldOfViewBounds[3]), 40f, 3f);
        rightB.GetComponent<BoxCollider>().size = new Vector3(3f, 40f, (fieldOfViewBounds[0] - fieldOfViewBounds[1]));
        leftB.GetComponent<BoxCollider>().size = new Vector3(3f, 40f, (fieldOfViewBounds[0] - fieldOfViewBounds[1]));
        xColliderSizeRation = topB.GetComponent<BoxCollider>().size.x;
        zColliderSizeRation = leftB.GetComponent<BoxCollider>().size.z;
       
       
        Vector3 newMean = GetMeanPosition(player1, player2);
     
        if (!boundTouching[3] && transform.position.x > newMean.x)
        {
            if (camOffset.x > 0f)
            {
                camOffset.x -= 0.5f;
            }
            if (camOffset.x <= 0.1f && camOffset.x >= -0.1f)
            {
                camOffset.x = 0f;
            }
        }
        if (!boundTouching[3] && transform.position.x > newMean.x + camOffset.x)
        {
            if(camOffset.x != 0f)
                camOffset.x -= 0.5f;
            if (camOffset.x <= 0.1f && camOffset.x >= -0.1f)
            {
                camOffset.x = 0f;
            }
        }

        if (!boundTouching[2] && transform.position.x < newMean.x)
        {
            if (camOffset.x < 0f)
            {
                camOffset.x += 0.1f;
            }
            if (camOffset.x <= 0.1f && camOffset.x >= -0.1f)
            {
                camOffset.x = 0f;
            }
        }
        if (!boundTouching[2] && transform.position.x > newMean.x + camOffset.x)
        {
            if (camOffset.x != 0f)
                camOffset.x += 0.1f;
            if (camOffset.x <= 0.1f && camOffset.x >= -0.1f)
            {
                camOffset.x = 0f;
            }
        }
    
        if (!boundTouching[1] && transform.position.z > newMean.z)
        {
            if (camOffset.z > 0f)
            {
                camOffset.z -= 1f;
            }
            if (camOffset.z <= 0.1f && camOffset.z >= -0.1f)
            {
                camOffset.z = 0f;
            }
        }
        if (!boundTouching[1] && transform.position.z > newMean.z + camOffset.z)
        {
            if (camOffset.z != 0f)
                camOffset.z -= 1f;
            if (camOffset.z <= 0.1f && camOffset.z >= -0.1f)
            {
                camOffset.z = 0f;
            }
        }
        if (!boundTouching[0] && transform.position.z < newMean.z)
        {
            if (camOffset.z < 0f)
            {
                camOffset.z += 1f;
            }
            if (camOffset.z <= 0.1f && camOffset.z >= -0.1f)
            {
                camOffset.z = 0f;
            }
        }
        if (!boundTouching[0] && transform.position.z > newMean.z + camOffset.z)
        {
            if (camOffset.z != 0f)
                camOffset.z += 1f;
            if (camOffset.z <= 0.1f && camOffset.z >= -0.1f)
            {
                camOffset.z = 0f;
            }
        }

        #endregion


        #region BoundOffset
        if (boundTouching[0] && newMean.z > transform.position.z )//top
        {
            camOffset.z = (transform.position.z - newMean.z);  
        }
        if (boundTouching[1] && newMean.z < transform.position.z )//bot
        {
            camOffset.z = (transform.position.z - newMean.z);      
        }
        if (boundTouching[2] && newMean.x > transform.position.x)//rigth
        {
            camOffset.x = (transform.position.x - newMean.x); 
        }

        if (boundTouching[3] && newMean.x < transform.position.x )//left
        {
            camOffset.x = (transform.position.x  - newMean.x);
        }
        #endregion

        #region ZoomingBoundCheck

        if (!boundTouching[3])
        {
            prevXLeftcolliderSize = xColliderSizeRation / 2f;
        }
        if (!boundTouching[2])
        {
            prevXRightcolliderSize = xColliderSizeRation / 2f;
        }

        if (boundTouching[3] && Mathf.Round(transform.position.y) < newMean.y)
        {
            xOffset = Mathf.Round((xColliderSizeRation / 2f - prevXLeftcolliderSize) * 100f) / 100f;
        }
        else if (boundTouching[2] && Mathf.Round(transform.position.y) < newMean.y)
        {
            xOffset = -Mathf.Round((xColliderSizeRation / 2f - prevXRightcolliderSize) * 100f) / 100f;
           
        }
        else
        {
            xOffset = 0f;
        }
        //---- maybe

        if (!boundTouching[0])
        {
            prevZTopcolliderSize = zColliderSizeRation / 2f;
        }
        if (!boundTouching[1])
        {
            prevZBotcolliderSize = zColliderSizeRation / 2f;
        }

        if (boundTouching[1] && Mathf.Round(transform.position.y) < newMean.y)
        {
            zOffset = Mathf.Round((zColliderSizeRation / 2f - prevZBotcolliderSize) * 100f) / 100f;
        }
        else if (boundTouching[0] && Mathf.Round(transform.position.y) < newMean.y)
        {
            zOffset = -Mathf.Round((zColliderSizeRation / 2f - prevZTopcolliderSize) * 100f) / 100f;
        }
        else
        {
            zOffset = 0f;
        }
        //-------------end of maybe
        #endregion
        Vector3 offsetRatio = new Vector3(xOffset, 0f, zOffset);


        if (Vector3.Distance(transform.position, (newMean + camOffset + offsetRatio)) >= 0.1f) 
        {
            transform.position = Vector3.Lerp(transform.position, (newMean + camOffset + offsetRatio), smoothing * Time.deltaTime);
        }
        

        
    }

    //Returns the average position between player 1 and 2 with an offset on the Z axis for the camera angle
    Vector3 GetMeanPosition(Transform play1, Transform play2)
    {
        Vector3 p1 = play1.position;
        Vector3 p2 = play2.position;

        if (!play1.FindChild("Legs").gameObject.activeSelf)
        {
            p1 = p2;
        }
        else if (!play2.FindChild("Legs").gameObject.activeSelf)
        {
            p2 = p1;
        }

        

        //Averages of our 2 player positions.
        float xMean = (p1.x + p2.x) / 2f;
        float yMean = (p1.y + p2.y) / 2f;
        float zMean = (p1.z + p2.z) / 2f;

       

        //returned values.
        float xReturned = xMean;
        float zReturned = zMean;

        //Calculate the x distance and z distance between our 2 players. Vertical is 2X because of camera angle and screen size. May need a small adjustment is we change the camera angle or resolution a lot.
        float horizontalFactor = Mathf.Abs(Vector3.Dot(p1, new Vector3(1f, 0f, 0f)) - Vector3.Dot(p2, new Vector3(1f, 0f, 0f)));
        float verticalFactor = 2f * Mathf.Abs(Vector3.Dot(p1, new Vector3(0f, 0f, 1f)) - Vector3.Dot(p2, new Vector3(0f, 0f, 1f)));

        float[] planeValues = new float[] { horizontalFactor, verticalFactor };

      
        //We set the Y position by taking our biggest distance between X and Z and adding a small bonus base on the Y distance of our players.
        float yReturned = Mathf.Clamp(((Mathf.Max(planeValues) + (yMean * 2f)) * zoomFactor), minDistanceZoomedOut, maxDistanceZoomedOut);

        //figure the correct z offset with fancy math.
        float zMeanOffset = -Mathf.Sqrt((Mathf.Pow((yMean - yReturned), 2) * Mathf.Pow((Mathf.Cos(cameraAngle * Mathf.Deg2Rad)), 2) + Mathf.Pow((xMean - xReturned), 2) * Mathf.Pow(Mathf.Cos(cameraAngle * Mathf.Deg2Rad), 2))
            / (1f - Mathf.Pow(Mathf.Cos(cameraAngle * Mathf.Deg2Rad), 2)));


        Vector3 toReturn = new Vector3(xReturned, yReturned, zReturned + zMeanOffset);

        return toReturn;
    }

    public bool IsSpawnPointInView()
    {
        Vector3 spawnPos = GameController.Instance.GetactiveSpawnPoint();

        if (spawnPos.x > leftB.position.x+2f && spawnPos.x < rightB.position.x-2f)
        {
            if (spawnPos.z > downB.position.z+2f && spawnPos.z < topB.position.z-2f)
            {
                return true;
            }
        }

        return false;
    }

}
