using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ElevatorManager : MonoBehaviour
{
    private SpriteRenderer sprite;
    Color red = new Color(0.8773585f, 0.4345407f, 0.4345407f, 1);
    Color green = new Color(0.5046639f, 0.8584906f, 0.4737896f, 1);

    int nearestFloor = -1;
    public int onFloor = 1;
    private int limitOfPeople;
    public List<GameObject> peopleInElevator;

    public bool movingToDesire;
    public List<int> desiredFloors;

    // Sets up sprite when game starts
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    IEnumerator MoveUp()
    {
        for (int i = 1; i <= 6 && (peopleInElevator.Count > 0 || desiredFloors.Count != 0 ) && onFloor <= 6; i++)
        {
            yield return new WaitForSeconds(2f);
            onFloor += 1;
            transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            foreach (GameObject personObject in peopleInElevator.ToList())
            {
                personObject.transform.position = new Vector3(personObject.transform.position.x, personObject.transform.position.y + 5, this.transform.position.z);
            }
            foreach (GameObject floorGameObject in GameObject.FindGameObjectsWithTag("Floor"))
            {
                if (onFloor.Equals(floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber))
                {
                    foreach (GameObject personObject in peopleInElevator.ToList())
                    {
                        if (personObject.gameObject.GetComponent<PeopleManager>().desiredFloor == floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber)
                        {
                            personObject.transform.position = new Vector3(-9, personObject.transform.position.y, this.transform.position.z);
                            floorGameObject.gameObject.GetComponent<FloorManager>().numberOfPeople.Add(personObject);
                            peopleInElevator.Remove(personObject);
                        }
                    }
                    if (floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber == nearestFloor)
                    {
                        movingToDesire = false;
                        desiredFloors.Remove(nearestFloor);
                    }
                }
            }
            //add animation
        }
        foreach (GameObject floorGameObject in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (onFloor.Equals(floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber))
            {
                if (floorGameObject.GetComponent<FloorManager>().numberOfPeople.Count > 0)
                {
                    AddPeopleToElevator();
                }
            }
        }
    }

    IEnumerator MoveDown()
    {
        for (int i = 1; i <= 6 && (peopleInElevator.Count > 0 || desiredFloors.Count != 0) && onFloor <= 1; i++)
        {
            yield return new WaitForSeconds(2f);
            onFloor -= 1;
            transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
            foreach (GameObject personObject in peopleInElevator.ToList())
            {
                personObject.transform.position = new Vector3(personObject.transform.position.x, personObject.transform.position.y - 5, this.transform.position.z);
            }
            foreach (GameObject floorGameObject in GameObject.FindGameObjectsWithTag("Floor"))
            {
                if (onFloor.Equals(floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber))
                {
                    foreach (GameObject personObject in peopleInElevator.ToList())
                    {
                        if (personObject.gameObject.GetComponent<PeopleManager>().desiredFloor == floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber)
                        {
                            personObject.gameObject.GetComponent<PeopleManager>().onFloor = onFloor;
                            personObject.transform.position = new Vector3(-9, personObject.transform.position.y, this.transform.position.z);
                            floorGameObject.gameObject.GetComponent<FloorManager>().numberOfPeople.Add(personObject);
                            peopleInElevator.Remove(personObject);
                        }
                    }
                    if (floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber == nearestFloor)
                    {
                        movingToDesire = false;
                        desiredFloors.Remove(nearestFloor);
                    }
                }
            }
            //add animation
        }
        foreach (GameObject floorGameObject in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (onFloor.Equals(floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber))
            {
                if (floorGameObject.GetComponent<FloorManager>().numberOfPeople.Count > 0)
                {
                    AddPeopleToElevator();
                }
            }
        }
    }

    void AddPeopleToElevator()
    {
        foreach (GameObject floorGameObject in GameObject.FindGameObjectsWithTag("Floor"))
        {
            if (onFloor.Equals(floorGameObject.gameObject.GetComponent<FloorManager>().floorNumber))
            {
                foreach (GameObject personObject in floorGameObject.gameObject.GetComponent<FloorManager>().numberOfPeople.ToList())
                {
                    if (limitOfPeople >= peopleInElevator.Count)
                    {
                        personObject.gameObject.GetComponent<PeopleManager>().onFloor = onFloor;
                        personObject.transform.position = new Vector3(this.transform.position.x, personObject.transform.position.y, this.transform.position.z);
                        peopleInElevator.Add(personObject);
                        floorGameObject.gameObject.GetComponent<FloorManager>().numberOfPeople.Remove(personObject);
                        desiredFloors.Add(personObject.gameObject.GetComponent<PeopleManager>().desiredFloor);
                    }
                }
            }
        }
    }

    // Updates each frame with new color depending on if elevator is open or not
    void Update()
    {
        if (peopleInElevator.Count > 0)
        {
            sprite.color = this.green;
        }
        else
        {
            sprite.color = this.red;
        }

        if (desiredFloors.Count != 0 && movingToDesire == false)
        {
            Debug.Log("start method");
            int smallestDifference = int.MaxValue;

            // Iterate through all desired floors
            foreach (int floor in desiredFloors)
            {
                // Calculate the difference between the current floor and the desired floor
                int difference = Mathf.Abs(floor - onFloor);
                Debug.Log(difference);

                // If the difference is smaller than the current smallestDifference, update nearestFloor
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    nearestFloor = floor;
                }
            }

            if (smallestDifference > 0)
            {
                StartCoroutine(MoveUp());
            }
            if (smallestDifference < 0)
            {
                StartCoroutine(MoveDown());
            }
            if (smallestDifference == 0)
            {
                AddPeopleToElevator();
                Debug.Log("added people");
                StartCoroutine(MoveUp());
            }
            movingToDesire = true;
        }
    }
}