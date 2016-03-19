using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combiner : MonoBehaviour {


    [System.Serializable]
    struct Input
    {
        [SerializeField]
        public InputObject target;
        [SerializeField]
        public InputObject.input input;

    };

    [SerializeField]
    Input[] onInputs;

    [SerializeField]
    Input[] offInputs;

    public GameObject plate1;
    public GameObject plate2;
    public GameObject button1;
    public GameObject button2;


    int activeCount = 0;

    List<GameObject> actives = new List<GameObject>();

	// Use this for initialization
	void Start () 
    {
        if (plate1 != null)
        {
            actives.Add(plate1);
        }
        if (plate2 != null)
        {
            actives.Add(plate2);
        }
        if (button1 != null)
        {
            actives.Add(button1);
        }
        if (button2 != null)
        {
            actives.Add(button2);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        activeCount = 0;
        for (int i = 0; i < actives.Count; i++)
        {
            if (actives[i].GetComponent<PressurePlate>() != null)
            {
                activeCount += actives[i].GetComponent<PressurePlate>().GetOnStatus() ? 1 : 0;
            }
            else if (actives[i].GetComponent<InteractiveButton>() != null)
            {
                activeCount += actives[i].GetComponent<InteractiveButton>().GetOnStatus() ? 1 : 0;
            }
        }
        if (activeCount == actives.Count)
        {
            for (int i = 0; i < onInputs.Length; i++)
            {
                onInputs[i].target.Input(onInputs[i].input);
            }
        }
        else 
        {
            for (int i = 0; i < offInputs.Length; i++)
            {
                offInputs[i].target.Input(offInputs[i].input);
            }
        }
	}
}
