using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Cinematic_Type { INTRO = 0, OUTRO = 1, INTERLUDE = 2, STORY = 3, COUNT = 4 }
//Add the public function Cinematic to a delegate on the camera object to add the movements.
public class Cam_Cinematic : MonoBehaviour
{

    //List of all movements for the intro.
    public List<Camera_Move> stepsIntro = new List<Camera_Move>();

    //List of all movements for the end.
    public List<Camera_Move> stepsOutro = new List<Camera_Move>();


    private List<List<Camera_Move>> sceneCinematics = new List<List<Camera_Move>>();

    delegate void mDelegate();
    mDelegate mDel;

    int currentStep = 0;
    bool hasStarted = false;



    //Variables used during camera work.
    private Vector3 stepStartPOS;
    private Vector3 stepEndPOS;
    private float startTime;
    private float movementDuration;
    private Quaternion endRotation;
    private Quaternion startRotation;

    //For editor Functions---------//
    public void SaveFrame(Cinematic_Type listType, int selected, Movement_Type type, float dur, bool isNew)
    {
        Camera_Move newStep = new Camera_Move();
        newStep.position = transform.position;
        newStep.rotation = transform.rotation;
        newStep.duration = dur;
        newStep.type = type;

        if (listType == Cinematic_Type.INTRO)
        {
            if (isNew)
            {
                stepsIntro.Add(newStep);
            }
            else
            {
                stepsIntro[selected] = newStep;
            }

        }
        else if (listType == Cinematic_Type.OUTRO)
        {
            if (isNew)
            {
                stepsOutro.Add(newStep);
            }
            else
            {
                stepsOutro[selected] = newStep;
            }
        }

    }

    public void RemoveStep(Cinematic_Type listType, int loc)
    {
        if (listType == Cinematic_Type.INTRO)
        {
            stepsIntro.RemoveAt(loc);
        }
        else if (listType == Cinematic_Type.OUTRO)
        {
            stepsOutro.RemoveAt(loc);
        }
    }


    //-----------------------------//




    void Awake()
    {
        sceneCinematics.Add(stepsIntro);
        sceneCinematics.Add(stepsOutro);
    }

    public void Cinematic(Cinematic_Type type)
    {

        if (!hasStarted)
        {
            AddNextStep(type);
            hasStarted = true;
        }

        if (mDel != null)
        {
            mDel();
        }

    }

    public void Reset()
    {
        hasStarted = false;
        currentStep = 0;
    }

    //calls the next step after previous step is over.
    IEnumerator Clock(float delay, Cinematic_Type type)
    {
        yield return new WaitForSeconds(delay);

        AddNextStep(type);
    }

    void AddNextStep(Cinematic_Type type)
    {

        //Removes the previous step.
        if (currentStep > 0)
        {
            switch (sceneCinematics[(int)type][currentStep - 1].type)
            {
                case Movement_Type.WAIT:
                    mDel -= Wait;
                    break;
                case Movement_Type.MOVELERP:
                    mDel -= MoveLerp;
                    break;
                case Movement_Type.MOVESLERP:
                    mDel -= MoveSlerp;
                    break;
                case Movement_Type.SETPOSITION:
                    mDel -= SetPosition;
                    break;
                case Movement_Type.SETROTATION:
                    mDel -= SetRotation;
                    break;
                case Movement_Type.END:
                    break;
            }
        }
        if (currentStep >= stepsIntro.Count)
        {
            return;
        }
        Camera_Move nextStep = sceneCinematics[(int)type][currentStep];

        //Select the next step to add.
        switch (nextStep.type)
        {
            case Movement_Type.WAIT:
                mDel += Wait;
                break;
            case Movement_Type.MOVELERP:
                startTime = Time.time;
                movementDuration = nextStep.duration;
                stepStartPOS = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                stepEndPOS = nextStep.position;
                endRotation = nextStep.rotation;
                startRotation = transform.rotation;
                mDel += MoveLerp;
                break;
            case Movement_Type.MOVESLERP:


                startTime = Time.time;
                movementDuration = nextStep.duration;
                stepStartPOS = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                stepEndPOS = nextStep.position;
                endRotation = nextStep.rotation;
                startRotation = transform.rotation;
                mDel += MoveSlerp;



                break;
            case Movement_Type.SETPOSITION:
                transform.position = nextStep.position;
                mDel += SetPosition;
                break;
            case Movement_Type.SETROTATION:
                transform.rotation = nextStep.rotation;
                mDel += SetRotation;
                break;
            case Movement_Type.END:
                break;
        }


        currentStep++;
        //Delay the next step by current step duration.
        StartCoroutine(Clock(nextStep.duration, type));



    }

    void Wait()
    {
        //wait function is empty.
    }

    void MoveLerp()
    {
        float fracJourney = (Time.time - startTime) / movementDuration;
        transform.position = Vector3.Lerp(stepStartPOS, stepEndPOS, fracJourney);

        RotateCam();
    }

    void MoveSlerp()
    {
        Vector3 center = (stepStartPOS + stepEndPOS) * 0.5F;
        center -= new Vector3(0, 1, 0);
        Vector3 startRelCenter = stepStartPOS - center;
        Vector3 endRelCenter = stepEndPOS - center;
        float fracComplete = (Time.time - startTime) / movementDuration;
        transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete);
        transform.position += center;

        RotateCam();
    }

    void RotateCam()
    {
        if (movementDuration > 0)
        {
            float step = (Time.time - startTime) / movementDuration;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, step);
        }
        else
        {
            transform.rotation = endRotation;
        }

    }


    void SetPosition()
    {
        //empty
    }

    void SetRotation()
    {
        //empty
    }

    void End()
    {
        //end is empty.
    }

    //returns the entire duration of the camera movement.
    public float GetDuration(Cinematic_Type type)
    {
        float dur = 0f;
        foreach (Camera_Move cMove in sceneCinematics[(int)type])
        {
            dur += cMove.duration;
        }
        return dur;
    }

}


[System.Serializable]
public class Camera_Move
{
    public Movement_Type type;
    public float duration;
    public Vector3 position;
    public Quaternion rotation;
}

public enum Movement_Type
{
    WAIT,
    MOVELERP,
    MOVESLERP,
    SETPOSITION,
    SETROTATION,
    END
}

