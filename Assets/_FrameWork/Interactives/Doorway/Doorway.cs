using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    [Tooltip("Length of the door animation before players start walking.")]
    public float                        doorAnimationDuration = 2f;
  
    [Tooltip("Position from the zone where the Arrow will apear.")]
    public Vector3                      arrowPositionOffset;


    private Vector3                     startPOSPlayer1;
    private Vector3                     endPOSPlayer1;
    private Vector3                     startPOSPlayer2;
    private Vector3                     endPOSPlayer2;

    [Range(0f, 100f)][Tooltip("Speed of the players while moving through the gate. Also affects transition duration.")]
    public float                        playerWalkSpeed = 1.0F;
    private float                       startTime;
    private float                       journeyLengthPlayer1;
    private float                       journeyLengthPlayer2;

    GameObject                          player1 = null;
    GameObject                          player2 = null;

    //Delegate to pass in the different Update functions needed for transitions.
    delegate void doorWayDelegate();
    doorWayDelegate                     del;

    [System.NonSerialized]
    public bool                         isActive = true;
    private bool                        hasPlayer1 = false;
    private int                         player1Loc;
    private bool                        hasPlayer2 = false;
    private int                         player2Loc;
    private bool                        isDoorUp = true;
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    [Tooltip("The Visual part of the doorWay. Can leave empty if a visible gate is not required.")]
    GameObject visual_Door;
    [SerializeField]
    Transform zone1;
    [SerializeField]
    Transform zone2;
    [SerializeField]
    Transform zone3;
    [SerializeField]
    Transform zone4;
    [SerializeField]
    Transform cameraBound;
    [SerializeField]
    [Tooltip("True if the gate appears from the trigger zones 1 and 2 first for the players.")]
    bool firstApearsFrom1and2 = true;

    void Awake() 
    {
        float dir = firstApearsFrom1and2 ? 1f : -1f;
        cameraBound.localPosition = new Vector3(0f,0f,
            (cameraBound.localScale.z/2f + 5f) * dir
            );
    }

    void Update()
    {
        if (del != null)
        {
            del();
        }

    }

    public void OnChildrenTriggerEnter(GameObject other, int zoneNumber)
    {
        if (!isActive)
        {
            return;
        }

        if (other.GetComponent<Player>().IsPlayerTwo() && !hasPlayer2)
        {
            hasPlayer2 = true;
            player2Loc = zoneNumber;
            if (player2 == null)
            {
                player2 = other.gameObject;
                
            } 
        }
        else if( !hasPlayer1)
        {
            hasPlayer1 = true;
            player1Loc = zoneNumber;
            if (player1 == null)
            {
                player1 = other.gameObject;
               
            }
        }

        if (hasPlayer1 && hasPlayer2)
        {
            arrow.SetActive(false);
            StartMovement();
        }
        else 
        {
            
            switch(zoneNumber)
            {
                case 1:
                    arrow.transform.position = zone2.position + arrowPositionOffset;
                    break;
                case 2:
                    arrow.transform.position = zone1.position + arrowPositionOffset;
                    break;
                case 3:
                    arrow.transform.position = zone4.position + arrowPositionOffset;
                    break;
                case 4:
                    arrow.transform.position = zone3.position + arrowPositionOffset;
                    break;
            }
            arrow.SetActive(true);
        }

        
    }

    public void OnChildrenTriggerExit(GameObject other, int zoneNumber)
    {
       

        if (other.GetComponent<Player>().IsPlayerTwo())
        {
            hasPlayer2 = false;
        }
        else
        {
            hasPlayer1 = false;
         
        }

        if (!hasPlayer1 && !hasPlayer2)
        {
            
            arrow.SetActive(false);
        }

        
    }

    void StartMovement()
    {
        StartCoroutine(OpenDoor());
        cameraBound.gameObject.SetActive(false);
        firstApearsFrom1and2 = !firstApearsFrom1and2;
        isActive = false;
    }

    IEnumerator Stop(float delay)
    {
        yield return new WaitForSeconds(delay);
        del -= TransitionRoom;

        hasPlayer1 = false;
        hasPlayer2 = false;
        player1Loc = 0;
        player2Loc = 0;
        del += MovingDoor;


        StartCoroutine(RaiseDoorway());
        isActive = true;

    }

    IEnumerator OpenDoor() 
    {
        //play door sound
        //play door opening anim
        player1.GetComponent<Player>().SetPlayerControl(false);//stops the players controlling.
        player2.GetComponent<Player>().SetPlayerControl(false);
        player1.GetComponent<Player>().ZeroVelocity();
        player2.GetComponent<Player>().ZeroVelocity();

        del += MovingDoor;
        yield return new WaitForSeconds(doorAnimationDuration);

        del -= MovingDoor;
        isDoorUp = false;

        startTime = Time.time;
        startPOSPlayer1 = player1.transform.position;
        Vector3 zoneOffsetP1 = player1.transform.position - (transform.FindChild("WaitingZones").transform.FindChild((player1Loc).ToString()).position);

        startPOSPlayer2 = player2.transform.position;
        Vector3 zoneOffsetP2 = player2.transform.position - (transform.FindChild("WaitingZones").transform.FindChild((player2Loc).ToString()).position);

        if (player1Loc + 2 < 5)
        {
            endPOSPlayer1 = player1Loc == 1? (zone3.position + zoneOffsetP1): (zone4.position + zoneOffsetP1);
            endPOSPlayer2 = player2Loc == 1? (zone3.position + zoneOffsetP2): (zone4.position + zoneOffsetP2);
        }
        else if (player1Loc - 2 > 0)
        {
            endPOSPlayer1 = player1Loc == 3 ? (zone1.position + zoneOffsetP1) : (zone2.position + zoneOffsetP1);
            endPOSPlayer2 = player2Loc == 3 ? (zone1.position + zoneOffsetP2) : (zone2.position + zoneOffsetP2);
        }

        journeyLengthPlayer1 = Vector3.Distance(startPOSPlayer1, endPOSPlayer1);
        journeyLengthPlayer2 = Vector3.Distance(startPOSPlayer2, endPOSPlayer2);
       

        float transitiondelay = journeyLengthPlayer1 > journeyLengthPlayer2 ? journeyLengthPlayer1 : journeyLengthPlayer2;
        transitiondelay /= playerWalkSpeed;

        del += TransitionRoom;

        StartCoroutine(Stop(transitiondelay));
    }

    IEnumerator RaiseDoorway() 
    {
        yield return new WaitForSeconds(doorAnimationDuration);
        player1.GetComponent<Player>().SetPlayerControl(true);//Start the players controlling.
        player2.GetComponent<Player>().SetPlayerControl(true);
        del -= MovingDoor;
        isDoorUp = true;
        float dir = firstApearsFrom1and2 ? 1f : -1f;
        cameraBound.localPosition = new Vector3(0f, 0f,
            (cameraBound.localScale.z / 2f + 5f) * dir
            );
        cameraBound.gameObject.SetActive(true);
    }

    void MovingDoor() 
    {
        float ySize = visual_Door.transform.localScale.y;

        

        float movementOfDoor = (ySize / doorAnimationDuration) * Time.deltaTime * (isDoorUp? 1f: -1f);

        visual_Door.transform.Translate(0f, -movementOfDoor, 0f);
    }

    void TransitionRoom()
    {
       
        float distCovered = (Time.time - startTime) * playerWalkSpeed;

        

        float fracJourneyPlayer1 = distCovered / journeyLengthPlayer1;
        float fracJourneyPlayer2 = distCovered / journeyLengthPlayer2;

        player1.transform.position = Vector3.Lerp(startPOSPlayer1, endPOSPlayer1, fracJourneyPlayer1);
        player2.transform.position = Vector3.Lerp(startPOSPlayer2, endPOSPlayer2, fracJourneyPlayer2);

    }

}


