using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PressurePlate : MonoBehaviour {

    [System.Serializable]
    struct InputStruct
    {
        [SerializeField]
        public InputObject target;
        [SerializeField]
        public InputObject.input input;

    };

    List<Collider> TriggerList;

    [SerializeField]
    float weight;
    
    [SerializeField]
    InputStruct[] onInputs;

    [SerializeField]
    InputStruct[] offInputs;

    [SerializeField]
    PressurePlate[] otherTriggers;

    [SerializeField]
    public bool hasTimerDelay = false;
    [SerializeField]
    float timerDuration = 5f;

    bool isCountingDownTimer = false;

    bool isOn = false;

    float currentWeight = 0;

    [SerializeField]
    Color activeColor;
    [SerializeField]
    Color deactiveColor;

    Animation animPlate;

    void Awake() 
    {
        animPlate = GetComponent<Animation>();
    }

	void Start () 
    {
        TriggerList = new List<Collider>();
        ChangeColor(false);
	}

    void Update() {

  

        if (isCountingDownTimer)
        {
            return;
        }

        currentWeight = 0;
        
        foreach (Collider collider in TriggerList)
        {
            Weight tWeight = collider.GetComponent<Weight>();
            if (tWeight) 
            {
                currentWeight += tWeight.GetWeight();
            }
        }


        //triggering and is not on.
        if (currentWeight >= weight && !isOn)
        {
            animPlate.Play();
            foreach (AnimationState state in animPlate)
            {
                state.time = 0f;
                state.speed = 2F;
            }
            SoundController.Instance.PlayFX("FloorPanel_Pressed", transform.position);
            isOn = true;
            ChangeColor(true);
            for (int i = 0; i < onInputs.Length; i++)
            {
                onInputs[i].target.Input(onInputs[i].input);
            }
        }
        //triggering but is already on.
        if (currentWeight >= weight && isOn)
        {
            
            ChangeColor(true);
            isOn = true;
            for (int i = 0; i < onInputs.Length; i++)
            {
                onInputs[i].target.Input(onInputs[i].input);
            }
        }

        //not triggering and is on.
        if (currentWeight < weight && isOn) 
        {

            animPlate.Play();
            foreach (AnimationState state in animPlate)
            {
                state.time = 0f;
                state.speed = -2F;
            }
            ChangeColor(false);
            SoundController.Instance.PlayFX("FloorPanel_Released", transform.position);


            if (otherTriggers.Length > 0)
            {
                for (int i = 0; i < otherTriggers.Length; i++)
                {
                    if (otherTriggers[i].GetWeightStatus())
                    {
                        return;
                    }
                }
            }
            if (!hasTimerDelay)
            {
                
                isOn = false;
                for (int i = 0; i < offInputs.Length; i++)
                {
                    offInputs[i].target.Input(offInputs[i].input);
                }
            }
            else 
            {

                isCountingDownTimer = true;
                StartCoroutine(DelayTimer());
            }
            
        }
       
    
    }

    IEnumerator DelayTimer() 
    {
        yield return new WaitForSeconds(timerDuration);
        isOn = false;
        ChangeColor(false);
        for (int i = 0; i < offInputs.Length; i++)
        {
            offInputs[i].target.Input(offInputs[i].input);
        }
        isCountingDownTimer = false;
    }

 //called when something enters the trigger
     void OnTriggerStay(Collider other)
     {
         
         //if the object is not already in the list
         if(!TriggerList.Contains(other))
         {
             Debug.Log(other.name + "Added");
             //add the object to the list
             TriggerList.Add(other);
         }
    
     }
 
     //called when something exits the trigger
     void OnTriggerExit(Collider other)
     {
         Debug.Log(other.name + "Removed");
         //if the object is in the list
         if(TriggerList.Contains(other))
         {
             //remove it from the list
             TriggerList.Remove(other);
         }
     }

     public bool GetOnStatus() 
     {
        
         return isOn;
     }
     public void SetOnStatus(bool status)
     {

         isOn = status;
     }
     public bool GetWeightStatus() 
     {
         return currentWeight >= weight; 
     }

     void ChangeColor(bool status) 
     {
         if(status)
            transform.FindChild("SM_assets_floor_button_01_mar30").GetComponent<MeshRenderer>().material.color = activeColor;
         else
             transform.FindChild("SM_assets_floor_button_01_mar30").GetComponent<MeshRenderer>().material.color = deactiveColor;
     }

}
