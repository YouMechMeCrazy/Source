﻿using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public bool isActive = false;

    void Start() 
    {
        if (isActive)
        {
            GameController.Instance.SetActiveSpawnPoint(transform.FindChild("SpawnPoint"));
            transform.FindChild("Visual_Active").gameObject.SetActive(true);
        }
    }

    public void TriggerZone()
    {
        if (!isActive)
        { 
            isActive = true;
            transform.FindChild("Visual_Active").gameObject.SetActive(true);
            GameController.Instance.SetActiveSpawnPoint(transform.FindChild("SpawnPoint"));
        }
       
    }

    public void SetInactive() 
    {
        isActive = false;
        transform.FindChild("Visual_Active").gameObject.SetActive(false);
    }

    
}