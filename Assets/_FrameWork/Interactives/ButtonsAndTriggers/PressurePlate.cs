using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PressurePlate : MonoBehaviour {

    [System.Serializable]
    struct Input
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
    Input[] onInputs;

    [SerializeField]
    Input[] offInputs;

    [SerializeField]
    public bool hasTimerDelay = false;
    [SerializeField]
    float timerDuration = 5f;

    bool isCountingDownTimer = false;

    bool on = false;

	
	void Start () 
    {
        TriggerList = new List<Collider>();
	}

    void Update() {

        if (isCountingDownTimer)
        {
            return;
        }

        float currweight = 0;
        
        foreach (Collider collider in TriggerList)
        {
            Weight tWeight = collider.GetComponent<Weight>();
            if (tWeight) {
                currweight += tWeight.GetWeight();
            }
        }

        if (currweight >= weight && !on)
        {
            on = true;
            for (int i = 0; i < onInputs.Length; i++)
            {
                onInputs[i].target.Input(onInputs[i].input);
            }
        }
        if (currweight < weight && on) 
        {
            if (!hasTimerDelay)
            {
                on = false;
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
        on = false;
        for (int i = 0; i < offInputs.Length; i++)
        {
            offInputs[i].target.Input(offInputs[i].input);
        }
        isCountingDownTimer = false;
    }

 //called when something enters the trigger
     void OnTriggerStay(Collider other)
     {
         //Debug.Log("hello");
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
         return on;
     }

}
