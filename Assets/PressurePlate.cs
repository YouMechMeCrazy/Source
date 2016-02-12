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

    

    bool on = false;

	// Use this for initialization
	void Start () {
        TriggerList = new List<Collider>();
	}

    void Update() {

        float currweight = 0;
        
        foreach (Collider collider in TriggerList){
            Weight tWeight = collider.GetComponent<Weight>();
            if (tWeight) {
                currweight += tWeight.GetWeight();
            }
        }

        if (currweight >= weight && !on){
            on = true;
            for (int i = 0; i < onInputs.Length; i++)
            {
                onInputs[i].target.Input(onInputs[i].input);
            }
        }
        if (currweight < weight && on) {
            on = false;
            for (int i = 0; i < offInputs.Length; i++)
            {
                offInputs[i].target.Input(offInputs[i].input);
            }
        }

    
    }
	
 //called when something enters the trigger
 void OnTriggerStay(Collider other)
 {
     //Debug.Log("hello");
     //if the object is not already in the list
     if(!TriggerList.Contains(other))
     {
         //add the object to the list
         TriggerList.Add(other);
     }
 }
 
 //called when something exits the trigger
 void OnTriggerExit(Collider other)
 {
     //if the object is in the list
     if(TriggerList.Contains(other))
     {
         //remove it from the list
         TriggerList.Remove(other);
     }
 }

}
