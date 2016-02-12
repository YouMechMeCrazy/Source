using UnityEngine;
using System.Collections;

//Abstract class that can have 2 states (on and off), and can recieve 2 kinds of input (on, off, toggle)


public class InputObject : MonoBehaviour {

    protected input state; //don't set state to input._Toggle, that doesn't make sense
    public enum input {_On, _Off, _Toggle}

    protected virtual void TurnOn() {
        //fill a cardboard box with kittens and dump it on a person while they sleep
    }

    protected virtual void TurnOff() {
       //same as above but cold oatmeal instead of kittens
    }

    public virtual void Input(input myInput) {
        switch (myInput) {
            case input._On:
                if (state==input._Off) { state = input._On; TurnOn(); }
                break;
            case input._Off:
                if (state == input._On) { state = input._Off;TurnOff(); }
                break;
            case input._Toggle:
                if (state == input._Off) { state = input._On; TurnOn(); }
                else                     { state = input._Off; TurnOff(); }
                break;

        }
    }
}
