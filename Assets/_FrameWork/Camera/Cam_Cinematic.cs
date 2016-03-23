using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Add the public function Cinematic to a delegate on the camera object to add the movements.
public class Cam_Cinematic : MonoBehaviour {

    //List of all movements.
    public List<Camera_Move> steps = new List<Camera_Move>();

    

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


    public void Cinematic() 
    {
        
        if (!hasStarted)
        {
            AddNextStep();
            hasStarted = true;
        }

        if (mDel != null)
        {
            mDel();
        }

    }
    //calls the next step after previous step is over.
    IEnumerator Clock(float delay) 
    {
        yield return new WaitForSeconds(delay);

        AddNextStep();
    }

    void AddNextStep() 
    {

        //Removes the previous step.
        if (currentStep > 0)
        {
            switch (steps[currentStep - 1].type)
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
        if (currentStep >= steps.Count)
        {
            return;
        }
        Camera_Move nextStep = steps[currentStep];

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
                mDel += MoveLerp;
                break;
            case Movement_Type.MOVESLERP:
                startTime = Time.time;
                movementDuration = nextStep.duration;
                stepStartPOS = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                stepEndPOS = nextStep.position;
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
        StartCoroutine(Clock(nextStep.duration));

        

    }

    void Wait() 
    {
        //wait function is empty.
    }

    void MoveLerp() 
    {
        float fracJourney = (Time.time - startTime) / movementDuration;
        transform.position = Vector3.Lerp(stepStartPOS, stepEndPOS, fracJourney);
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
    public float GetDuration()
    {
        float dur = 0f;
        foreach (Camera_Move cMove in steps)
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

