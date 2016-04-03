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

    private bool toggleAutoCam = false;

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
        //Frame Selector
        EditorGUILayout.LabelField("Step Selector", EditorStyles.boldLabel);
        currentFrame = EditorGUILayout.IntSlider(currentFrame, 1, numberOfSteps);
        //------
        toggleAutoCam = GUILayout.Toggle(toggleAutoCam, "Auto set the camera position on step selection.");
        EditorGUILayout.LabelField("For visualisation only. Cannot edit or add steps if ON.", EditorStyles.centeredGreyMiniLabel);

        if (toggleAutoCam)
        {
            if (typeSelected == 0 && cCin.stepsIntro.Count > 0)
            {
                cCin.transform.position = cCin.stepsIntro[currentFrame - 1].position;
                cCin.transform.rotation = cCin.stepsIntro[currentFrame - 1].rotation;
            }
            else if (typeSelected == 1 && cCin.stepsOutro.Count > 0)
            {
                cCin.transform.position = cCin.stepsOutro[currentFrame - 1].position;
                cCin.transform.rotation = cCin.stepsOutro[currentFrame - 1].rotation;
            }
        }

        //Display current frame
        EditorGUILayout.LabelField("Selected Step  : " + currentFrame);
        
        if (typeSelected == 0 && cCin.stepsIntro.Count > 0)
        {
            EditorGUILayout.LabelField("Type of step_____: " + cCin.stepsIntro[currentFrame-1].type);
            EditorGUILayout.LabelField("Position_________: " + cCin.stepsIntro[currentFrame - 1].position);
            EditorGUILayout.LabelField("Rotation_________: " + "X "+(cCin.stepsIntro[currentFrame - 1].rotation.ToEuler().x * Mathf.Rad2Deg).ToString("F2")
                                                             + " " + "Y " + (cCin.stepsIntro[currentFrame - 1].rotation.ToEuler().y * Mathf.Rad2Deg).ToString("F2")
                                                             + " " + "Z " + (cCin.stepsIntro[currentFrame - 1].rotation.ToEuler().z * Mathf.Rad2Deg).ToString("F2"));
            EditorGUILayout.LabelField("Duration_________: " + cCin.stepsIntro[currentFrame - 1].duration + " Seconds");
           
        }
        else if (typeSelected == 1 && cCin.stepsOutro.Count > 0)
        {
            EditorGUILayout.LabelField("Type of step_____: " + cCin.stepsOutro[currentFrame - 1].type);
            EditorGUILayout.LabelField("Position_________: " + cCin.stepsOutro[currentFrame - 1].position);
            EditorGUILayout.LabelField("Rotation_________: " + "X " + (cCin.stepsOutro[currentFrame - 1].rotation.ToEuler().x * Mathf.Rad2Deg).ToString("F2")
                                                             + " " + "Y " + (cCin.stepsOutro[currentFrame - 1].rotation.ToEuler().y * Mathf.Rad2Deg).ToString("F2")
                                                             + " " + "Z " + (cCin.stepsOutro[currentFrame - 1].rotation.ToEuler().z * Mathf.Rad2Deg).ToString("F2"));
            EditorGUILayout.LabelField("Duration_________: " + cCin.stepsOutro[currentFrame - 1].duration + " Seconds");
          
        }
        EditorGUILayout.LabelField("Click to move the camera to the selected step inside the inspector.");

        if (GUILayout.Button("Move Camera to " + (Cinematic_Type)typeSelected + " Step# " + currentFrame))
        {
            if (typeSelected == 0 && cCin.stepsIntro.Count > 0)
            {
                cCin.transform.position = cCin.stepsIntro[currentFrame - 1].position;
                cCin.transform.rotation = cCin.stepsIntro[currentFrame - 1].rotation;
            }
            else if (typeSelected == 1 && cCin.stepsOutro.Count > 0)
            {
                cCin.transform.position = cCin.stepsOutro[currentFrame - 1].position;
                cCin.transform.rotation = cCin.stepsOutro[currentFrame - 1].rotation;
            }

        }


        EditorGUILayout.LabelField("____________________________________________________________________________________");
        EditorGUILayout.LabelField("How to edit/add steps:", EditorStyles.helpBox);
        EditorGUILayout.LabelField("1-Select step to edit from top slider.");
        EditorGUILayout.LabelField("(If adding new step set to the step before the newly created.)");
        EditorGUILayout.LabelField("2-Select step TYPE from drop down menu below.");
        moveTypeSel = EditorGUILayout.Popup("Movement type: ", moveTypeSel, optionsCmov);
        EditorGUILayout.LabelField("3-Set step DURATION with slider below.");
        duration = EditorGUILayout.Slider(duration, 0f, 60f);
        EditorGUILayout.LabelField("4-Set the Camera POSITION in the inspector.");
        EditorGUILayout.LabelField("5-Set the Camera ROTATION in the inspector.");
        EditorGUILayout.LabelField("6-(For UPDATE) Clicking Update will override the ");
        EditorGUILayout.LabelField("selected step with your newly created one.");
        EditorGUILayout.LabelField("6-(For ADDING) Cliking the Add step will insert the new");
        EditorGUILayout.LabelField("step in the list.");

        EditorGUILayout.LabelField("____________________________________________________________________________________");

      


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
        EditorGUILayout.LabelField("This removes the Selected Step.", EditorStyles.centeredGreyMiniLabel);
       
    }

}
