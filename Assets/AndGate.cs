using UnityEngine;
using System.Collections;

public class AndGate : MonoBehaviour {

    [System.Serializable]
    struct Input
    {
        [SerializeField]
        public InputObject target;
        [SerializeField]
        public InputObject.input input;

    };


    [SerializeField]
    Input[] inputs;

    AndGateLight leftLight;
    AndGateLight rightLight;

    bool active;

    void Start() {
        leftLight = transform.FindChild("LeftLight").GetComponent<AndGateLight>();
        rightLight = transform.FindChild("RightLight").GetComponent<AndGateLight>();
    }


	// Update is called once per frame
	void Update () {

        if (leftLight.IsOn() && rightLight.IsOn()&!active) {
            active = true;
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].target.Input(inputs[i].input);
            }
        }
        if (!leftLight.IsOn() || !rightLight.IsOn()) {
            active = false;
        }


	}


}
