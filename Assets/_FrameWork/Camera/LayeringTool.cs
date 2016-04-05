using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LayeringTool : MonoBehaviour {

    [SerializeField]
    List<GameObject> spawnedElements = new List<GameObject>();

    [SerializeField]
    int amountOfElements = 10;

    int currentElement = 0;

    float timer;

    float clipDuration = 4.083333f;

    List<List<GameObject>> runTimeLists = new List<List<GameObject>>();

	// Use this for initialization
	void Awake () 
    {
        for (int i = 0; i < spawnedElements.Count; i++)
        {
            runTimeLists.Add(new List<GameObject>());

            for (int j = 0; j < amountOfElements; j++)
            {
                GameObject newSpawned = Instantiate(spawnedElements[i], transform.position, Quaternion.identity) as GameObject;
                newSpawned.transform.Rotate(Vector3.right, 30f);
                newSpawned.SetActive(false);
                runTimeLists[i].Add(newSpawned);
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        if (timer > clipDuration / amountOfElements)
        {

            timer = 0f;
            Vector3 newPosition = new Vector3(Random.Range(transform.position.x-20f, transform.position.x+20f), Random.Range(6f, transform.position.y - 2f), Random.Range(transform.position.z, transform.position.z+50f) );

            for (int i = 0; i < spawnedElements.Count; i++)
            {
                runTimeLists[i][currentElement].transform.position = newPosition;
               
                runTimeLists[i][currentElement].SetActive(true);
            }

            currentElement++;
            if (currentElement == amountOfElements)
            {
                currentElement = 0;
            }
           
            
        }
	}

    void SetFrame(GameObject obj) 
    {
        if (obj.GetComponent<Animator>() == null)
            return;

        obj.GetComponent<Animator>().Play(0, -1, 0f);
    }
}
