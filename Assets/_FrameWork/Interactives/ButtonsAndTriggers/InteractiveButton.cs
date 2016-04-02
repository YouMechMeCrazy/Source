using UnityEngine;
using System.Collections;

public class InteractiveButton : MonoBehaviour {

    [System.Serializable]
    struct Input {
        [SerializeField]
        public InputObject target;
        [SerializeField]
        public InputObject.input input;


    };


    [SerializeField]
    Input[] inputsOn;
    [SerializeField]
    Input[] inputsOff;

    public bool isOn = false;

    public bool hasTimerDelay = false;
    bool isOnTimeDelay = false;

    [SerializeField]
    float timerDuration = 5f;
  



    public void Hit() 
    {
        transform.FindChild("Button").GetComponent<Animator>().SetTrigger("buttonPress");
        SoundController.Instance.PlayFX("Button_Pressed", transform.position);
        if (!hasTimerDelay)
        {
            if (!isOn)
            {
                for (int i = 0; i < inputsOn.Length; i++)
                {
                    inputsOn[i].target.Input(inputsOn[i].input);
                }
                isOn = true;
            }
            else 
            {
                for (int i = 0; i < inputsOff.Length; i++)
                {
                    inputsOff[i].target.Input(inputsOff[i].input);
                }
                isOn = false;
            }
           
        }
        else //If this button has a timer.
        {
            if (!isOnTimeDelay)
            {
                if (!isOn)
                {
                    for (int i = 0; i < inputsOn.Length; i++)
                    {
                        inputsOn[i].target.Input(inputsOn[i].input);
                    }
                    isOn = true;
                }
                else
                {
                    for (int i = 0; i < inputsOff.Length; i++)
                    {
                        inputsOff[i].target.Input(inputsOff[i].input);
                    }
                    isOn = false;
                }
                isOnTimeDelay = true;
                StartCoroutine(DelayTimer());
            }
           
        }
       

    }

    IEnumerator DelayTimer() 
    {
        
        yield return new WaitForSeconds(timerDuration);
        if (!isOn)
        {
            for (int i = 0; i < inputsOn.Length; i++)
            {
                inputsOn[i].target.Input(inputsOn[i].input);
            }
            isOn = true;
        }
        else
        {
            for (int i = 0; i < inputsOff.Length; i++)
            {
                inputsOff[i].target.Input(inputsOff[i].input);
            }
            isOn = false;
        }
        isOnTimeDelay = false;
        
    }

    public bool GetOnStatus()
    {
        return isOn;
    }
}
