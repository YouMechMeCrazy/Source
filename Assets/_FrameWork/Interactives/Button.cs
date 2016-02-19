using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

    [System.Serializable]
    struct Input {
        [SerializeField]
        public InputObject target;
        [SerializeField]
        public InputObject.input input;

    };


    [SerializeField]
    Input[] inputs;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hit() {
        for (int i = 0; i < inputs.Length; i++) {
            inputs[i].target.Input(inputs[i].input);
        }
    }
}
