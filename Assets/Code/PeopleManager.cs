using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleManager : MonoBehaviour
{
    public int onFloor = 1;
    public int desiredFloor;

    void Start()
    {
        GoToFloor();
    }

    void GoToFloor()
    {
        foreach (GameObject floorGameObject in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (onFloor.Equals(floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber))
            {
                floorGameObject.gameObject.GetComponent<FloorManager>().numberOfPeople.Add(this.gameObject);
                floorGameObject.gameObject.GetComponent<FloorManager>().ActivateElevator();
                Debug.Log("Activate done");
            }
        }
    }
}