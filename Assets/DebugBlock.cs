using UnityEngine;
using System.Collections;


//Howdy! I'm Debuggy the Debugger! You're new here aren't you?

public class DebugBlock : InputObject {


    protected override void TurnOn()
    {
        base.TurnOn();
        Debug.Log("I am now ON! =)");
    }
    protected override void TurnOff()
    {
        base.TurnOff();
        Debug.Log("I am now OFF! =)");
    }

}
