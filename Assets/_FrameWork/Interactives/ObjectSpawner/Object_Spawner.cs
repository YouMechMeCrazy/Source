using UnityEngine;
using System.Collections;

public class Object_Spawner : MonoBehaviour {

    public GameObject[] spawningObjects;
    public Transform spawnPoint;

    int keyTick = 60;
    int currentTick = 0;
	// Update is called once per frame
	void Update ()
    {
        currentTick++;
        if (currentTick >= keyTick)
        {
            currentTick = 0;
            CheckObjectArray();
        }
	}

    void CheckObjectArray()
    {
        for (int i = 0; i < spawningObjects.Length; i++)
        {
            if (!spawningObjects[i].activeSelf)
            {

                if (spawningObjects[i].GetComponent<Pickup>().heldBy != null)
                {
                    spawningObjects[i].GetComponent<Pickup>().heldBy.RemoveHeldObject();
                }

                spawningObjects[i].transform.position = spawnPoint.position;
                spawningObjects[i].SetActive(true);
                return;
            }
        }
    }
}
