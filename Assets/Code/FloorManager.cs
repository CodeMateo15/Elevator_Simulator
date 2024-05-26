using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public int floorNumber;
    public bool activateElevator;
    public List<GameObject> numberOfPeople;
    public List<GameObject> openElevators;

    public void ActivateElevator()
    {
        int smallestDifference = int.MaxValue;
        GameObject nearestElevator = null;
        foreach (GameObject elevator in GameObject.FindGameObjectsWithTag("Elevator"))
        {
            // Calculate the difference between the elevator's onFloor and this.floorNumber
            int difference = Mathf.Abs(elevator.gameObject.GetComponent<ElevatorManager>().onFloor - floorNumber);

            // If the difference is smaller than the current smallestDifference, update nearestElevator
            if (difference < smallestDifference)
            {
                smallestDifference = difference;
                nearestElevator = elevator;
            }
        }
        if (nearestElevator != null)
        {
            nearestElevator.gameObject.GetComponent<ElevatorManager>().desiredFloors.Add(floorNumber);
            Debug.Log("Added Floor Number: " + floorNumber);
            Debug.Log(nearestElevator.gameObject.GetComponent<ElevatorManager>().desiredFloors);
        }
        Debug.Log(nearestElevator);
    }
}