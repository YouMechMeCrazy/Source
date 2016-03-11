using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Step
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
    FORWARD,
    BACKWARD,
    WAIT,
    STOP
}

public class Programable_Mover : MonoBehaviour
{

    [Tooltip("To get a smooth motion with a scale of 1 unit: speed at 1 ; steps of 0.5." +
    "If you double the speed you need to half the step duration. Increasing the scale of the object mean you need to increase the speed by the same factor as well.")]
    public ProgramablePath path;

    float startTime;
    float timer;

    int currentStep = 0;
    float yDistance;
    float xDistance;
    float zDistance;
    [SerializeField][Tooltip("Click and drag the collider of the object you wish to move here. Only need to add either a 2D or 3D collider, not both. Size of movement is based on the collider size.")]
    Collider col;
    [SerializeField]
    [Tooltip("Click and drag the collider of the object you wish to move here. Only need to add either a 2D or 3D collider, not both. Size of movement is based on the collider size.")]
    Collider2D col2d;

    bool stepInProgress = false;

    delegate void MovementDelegate();
    MovementDelegate mDelegate;


    Vector3 startMarker;
    Vector3 endMarker;


    private float journeyLength;

    // Use this for initialization
    void Start()
    {
        if (col != null)
        {
            yDistance = col.bounds.size.y;
            xDistance = col.bounds.size.x;
            zDistance = col.bounds.size.z;
        }
        else if (col2d != null)
        {
            yDistance = col2d.bounds.size.y;
            xDistance = col2d.bounds.size.x;
        }

        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if (path.steps.Count == 0)
        {
            return;
        }
        if (!stepInProgress)
        {

            stepInProgress = true;
            StartCoroutine(Move());
        }
        if (mDelegate != null)
        {
            mDelegate();
        }
    }

    IEnumerator Move()
    {

        yield return new WaitForSeconds(path.stepTime);

        if (currentStep >= path.steps.Count)
        {
            currentStep = 0;
        }

        switch (path.steps[currentStep])
        {
            case Step.UP:

                startMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                endMarker = new Vector3(transform.position.x, transform.position.y + yDistance, transform.position.z);
                journeyLength = Vector3.Distance(startMarker, endMarker);
                startTime = Time.time;
                mDelegate += LerpTo;
                StartCoroutine(NextStep());
                break;
            case Step.DOWN:

                startMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                endMarker = new Vector3(transform.position.x, transform.position.y - yDistance, transform.position.z);
                journeyLength = Vector3.Distance(startMarker, endMarker);
                startTime = Time.time;
                mDelegate += LerpTo;
                StartCoroutine(NextStep());
                break;
            case Step.RIGHT:

                startMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                endMarker = new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z);
                journeyLength = Vector3.Distance(startMarker, endMarker);
                startTime = Time.time;
                mDelegate += LerpTo;
                StartCoroutine(NextStep());
                break;
            case Step.LEFT:

                startMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                endMarker = new Vector3(transform.position.x - xDistance, transform.position.y, transform.position.z);
                journeyLength = Vector3.Distance(startMarker, endMarker);
                startTime = Time.time;
                mDelegate += LerpTo;
                StartCoroutine(NextStep());
                break;
            case Step.FORWARD:

                startMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                endMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z + zDistance);
                journeyLength = Vector3.Distance(startMarker, endMarker);
                startTime = Time.time;
                mDelegate += LerpTo;
                StartCoroutine(NextStep());
                break;
            case Step.BACKWARD:

                startMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                endMarker = new Vector3(transform.position.x, transform.position.y, transform.position.z - zDistance);
                journeyLength = Vector3.Distance(startMarker, endMarker);
                startTime = Time.time;
                mDelegate += LerpTo;
                StartCoroutine(NextStep());
                break;
            case Step.WAIT:
                StartCoroutine(Wait());
                break;
            case Step.STOP:
                path.steps.Clear();
                break;
        }

        currentStep++;
    }

    void LerpTo()
    {
        float distCovered = (Time.time - startTime) * path.moveSpeed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);
    }

    IEnumerator NextStep()
    {
        yield return new WaitForSeconds(path.stepTime);
        stepInProgress = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(path.waitDuration);
        stepInProgress = false;
    }


}

[System.Serializable]
public class ProgramablePath
{

    public List<Step> steps = new List<Step>();
    public float moveSpeed;
    public float stepTime;
    public float waitDuration;


    ProgramablePath()
    {

    }
}

