using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_DoorTest : MonoBehaviour
{
    [SerializeField] GameObject[] doors;

    GameObject nextDoor;
    int counter;

    private void Start()
    {
        nextDoor = doors[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && counter < doors.Length)
        {
            nextDoor.SetActive(false);
            counter++;
            nextDoor = doors[counter];
        }
    }
}
