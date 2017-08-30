using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldStates { DEFAULT, IN_TRIGGER, IN_DROP_ZONE, HOLDING_OBJECT }

public class P_PickUpObject : MonoBehaviour
{
    [SerializeField] Transform hand, winText;
    [SerializeField] Sprite winSprite;

    Renderer handRenderer;
    Transform objectInTrigger;
    HoldStates currentState = HoldStates.DEFAULT;

    private void Start()
    {
        handRenderer = hand.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentState == HoldStates.IN_TRIGGER) PickUpObject();
            else if (currentState == HoldStates.IN_DROP_ZONE) DropObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SadPickup" && currentState != HoldStates.HOLDING_OBJECT)
        {
            currentState = HoldStates.IN_TRIGGER;
            objectInTrigger = other.transform;
            handRenderer.enabled = true;
        }
        else if (other.tag == "SadDropZone")
        {
            currentState = HoldStates.IN_DROP_ZONE;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentState == HoldStates.IN_TRIGGER)
        {
            currentState = HoldStates.DEFAULT;
            objectInTrigger = null;
            handRenderer.enabled = false;
        }
    }

    void PickUpObject()
    {
        objectInTrigger.position = hand.position;
        objectInTrigger.SetParent(transform);
        handRenderer.enabled = false;

        currentState = HoldStates.HOLDING_OBJECT;
    }

    void DropObject()
    {
        objectInTrigger.SetParent(null);
        objectInTrigger.GetComponent<Collider>().enabled = false;

        currentState = HoldStates.DEFAULT;
    }
}
