using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(Cam_Cinematic))]
public class Editor_Cinematics : Editor 
{
    private int currentFrame = 0;
    private List<List<Camera_Move>> cinLists = new List<List<Camera_Move>>();

    private int typeSelected;
    private int moveTypeSel;

    private float durationD;
    private Vector3 positionD;
    private Vector3 rotationD;
    private Movement_Type moveTypeD;

    private float duration;
    private Vector3 position;
    private Vector3 rotation;
    private Movement_Type moveType;

    private int numberOfSteps = 0;

    string[] optionsCin = new string[]
    {
            Cinematic_Type.INTRO.ToString(), Cinematic_Type.OUTRO.ToString()
    };
    string[] optionsCmov = new string[]
    {
            Movement_Type.WAIT.ToString(), Movement_Type.MOVELERP.ToString(), Movement_Type.MOVESLERP.ToString(), Movement_Type.SETPOSITION.ToString(), Movement_Type.SETROTATION.ToString()
    };
    public override void OnInspectorGUI()
    {
        Cam_Cinematic cCin = (Cam_Cinematic)target;


        if (typeSelected == 0)
        {
            numberOfSteps = cCin.stepsIntro.Count;
        }
        else if (typeSelected == 1)
        {
            numberOfSteps = cCin.stepsOutro.Count;
        }

        typeSelected = EditorGUILayout.Popup("Cinematic: " + numberOfSteps +  " steps" , typeSelected, optionsCin);



        EditorGUILayout.LabelField("Current Step: " + currentFrame);
        currentFrame = EditorGUILayout.IntSlider(currentFrame, 1, numberOfSteps);
        
        
        //Display current frame
        if (typeSelected == 0 &&  cCin.stepsIntro.Count > 0)
        {
            positionD = cCin.stepsIntro[currentFrame - 1].position;
            rotationD = cCin.stepsIntro[currentFrame - 1].rotation.eulerAngles;
            durationD = cCin.stepsIntro[currentFrame - 1].duration;
            moveTypeD = cCin.stepsIntro[currentFrame - 1].type;
        }
        else if (typeSelected == 1 && cCin.stepsOutro.Count > 0)
        {
            positionD = cCin.stepsOutro[currentFrame - 1].position;
            rotationD = cCin.stepsOutro[currentFrame - 1].rotation.eulerAngles;
            durationD = cCin.stepsOutro[currentFrame - 1].duration;
            moveTypeD = cCin.stepsOutro[currentFrame - 1].type;
        }
        



        moveTypeSel = EditorGUILayout.Popup("Movement type: ", moveTypeSel, optionsCmov); 

        EditorGUILayout.LabelField("Duration: " + durationD + " seconds");
        duration = EditorGUILayout.Slider(duration, 0f, 60f);
        positionD = EditorGUILayout.Vector3Field("Position: ", positionD);
        rotationD = EditorGUILayout.Vector3Field("Rotation: ", rotationD);

        if (GUILayout.Button("Move Camera to " + (Cinematic_Type)typeSelected + " Step# " + currentFrame))
        {
            cCin.transform.position = cCin.stepsIntro[currentFrame - 1].position;
            //cCin.transform.rotation = cCin.stepsOutro[currentFrame - 1].rotation;
        }


        if (GUILayout.Button("Update Step " + (Cinematic_Type)typeSelected + " Step# " + currentFrame))
        {
            cCin.SaveFrame((Cinematic_Type)typeSelected, currentFrame-1, (Movement_Type)moveTypeSel, duration, false);
        }


        if (GUILayout.Button("Add New Step"))
        {
            cCin.SaveFrame((Cinematic_Type)typeSelected, currentFrame - 1, (Movement_Type)moveTypeSel, duration, true);
            currentFrame++;
        }

        if (GUILayout.Button("Remove " + (Cinematic_Type)typeSelected + " Step# " + currentFrame))
        {
            if (numberOfSteps > 0)
            cCin.RemoveStep((Cinematic_Type)typeSelected, currentFrame - 1);
        }
       
    }

}
