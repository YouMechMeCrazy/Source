using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour {

    [System.NonSerialized]
    public bool                         hasPlayer1 = false;
    [System.NonSerialized]
    public bool                         hasPlayer2 = false;

    [Tooltip("The Visual part of the doorWay. Can leave empty if a visible gate is not required.")]
    public GameObject                   movableDoorway;
    [Tooltip("Length of the transition between areas.")]
    public float                        wallMoveDuration = 3f;
    [Tooltip("Actual camera bound of the doorway/section that needs to be disabled to pass the camera through.")]
    public GameObject                   doorWayBound;
    [Tooltip("Camera position after doorway transition. I.E. target poistion the camera lerps to during transition.")]
    public Vector3                      landingPositionOffset;

    [Tooltip("The position of the camera after the translation movement ")]
    public Vector3                      endCameraPosition;

    private Vector3                     startPOSPlayer1;
    private Vector3                     endPOSPlayer1;
    private Vector3                     startPOSPlayer2;
    private Vector3                     endPOSPlayer2;

    [Range(0f, 10f)][Tooltip("Speed of the players while moving through the gate.")]
    public float                        playerWalkSpeed = 1.0F;
    private float                       startTime;
    private float                       journeyLengthPlayer1;
    private float                       journeyLengthPlayer2;

    [Tooltip("Vector 3 of the players movement applied during transition. I.E. if you have a Vector3(10f , 0f , 1f), the players will both move 10 units on the X axis and 1 unit on the Z axis during the transition.")]
    public Vector3                      doorMovementDirection;
    [Tooltip("Movement speed of the players during the transition.")]
    public float                        doorMovementSpeed;

    GameObject                          player1 = null;
    GameObject                          player2 = null;

    //Delegate to pass in the different Update functions needed for transitions.
    delegate void doorWayDelegate();
    doorWayDelegate                     del;
    [System.NonSerialized]
    public bool                         isOn = true;

    void Update()
    {
        if (del != null)
        {
            del();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isOn)
        {
            return;
        }
        if (other.GetComponent<Player>() != null)
        {

            if (other.GetComponent<Player>().IsPlayerTwo())
            {
                hasPlayer2 = true;
                if (player2 == null)
                {
                    player2 = other.gameObject;
                } 
            }
            else
            {
                hasPlayer1 = true;
                if (player1 == null)
                {
                    player1 = other.gameObject;
                }
            }
            if (hasPlayer1 && hasPlayer2)
            {
                StartMovement();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
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
                isOn = true;
            }

        }
    }

    void StartMovement()
    {
        del += TransitionRoom;
        doorWayBound.SetActive(false);
        startTime = Time.time;

        Camera.main.GetComponent<Camera_Controller>().OffSetCamera(endCameraPosition, true);

        startPOSPlayer1 = player1.transform.position;
        endPOSPlayer1 = player1.transform.position + landingPositionOffset;

        startPOSPlayer2 = player2.transform.position;
        endPOSPlayer2 = player2.transform.position + landingPositionOffset;

        journeyLengthPlayer1 = Vector3.Distance(startPOSPlayer1, endPOSPlayer1);
        journeyLengthPlayer2 = Vector3.Distance(startPOSPlayer2, endPOSPlayer2);

        StartCoroutine(Reverse());
    }

    IEnumerator Reverse()
    {
        yield return new WaitForSeconds(wallMoveDuration);
        doorMovementSpeed *= -1f;
        
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(wallMoveDuration);
        del -= TransitionRoom;
        isOn = false;
        doorWayBound.SetActive(true);
        Camera.main.GetComponent<Camera_Controller>().OffSetCamera(new Vector3(0f,0f,0f), false);
    }

    void TransitionRoom()
    {
        if (movableDoorway != null)
        {
            movableDoorway.transform.Translate(doorMovementDirection * Time.deltaTime * doorMovementSpeed);
        }
        float distCovered = (Time.time - startTime) * playerWalkSpeed;

        float fracJourneyPlayer1 = distCovered / journeyLengthPlayer1;
        float fracJourneyPlayer2 = distCovered / journeyLengthPlayer2;

        player1.transform.position = Vector3.Lerp(startPOSPlayer1, endPOSPlayer1, fracJourneyPlayer1);
        player2.transform.position = Vector3.Lerp(startPOSPlayer2, endPOSPlayer2, fracJourneyPlayer2);

    }

  
    

}


