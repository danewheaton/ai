using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorStates { OPEN, LOCKED }

public class Door : MonoBehaviour
{
    [SerializeField] DoorStates doorState = DoorStates.LOCKED;
    [SerializeField] int unlockMessagesRequired = 1;

    int unlockMessagesReceived;

    private void Start()
    {
        switch (doorState)
        {
            case DoorStates.OPEN:
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                    r.enabled = false;
                foreach (Collider c in GetComponentsInChildren<Collider>())
                    c.enabled = false;
                break;
            case DoorStates.LOCKED:
                foreach (Renderer r in GetComponentsInChildren<Renderer>())
                    r.enabled = true;
                foreach (Collider c in GetComponentsInChildren<Collider>())
                    c.enabled = true;
                break;
            default:
                break;
        }
    }

    public void ChangeState(DoorStates state)
    {
        unlockMessagesReceived++;

        doorState = state;

        if (unlockMessagesReceived >= unlockMessagesRequired)
        {
            switch (doorState)
            {
                case DoorStates.OPEN:
                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        r.enabled = false;
                    foreach (Collider c in GetComponentsInChildren<Collider>())
                        c.enabled = false;
                    break;
                case DoorStates.LOCKED:
                    foreach (Renderer r in GetComponentsInChildren<Renderer>())
                        r.enabled = true;
                    foreach (Collider c in GetComponentsInChildren<Collider>())
                        c.enabled = true;
                    break;
                default:
                    break;
            }
        }
    }
}
