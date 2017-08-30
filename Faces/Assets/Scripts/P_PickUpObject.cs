using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HoldStates { DEFAULT, IN_SAD_TRIGGER, IN_HAPPY_TRIGGER, IN_SAD_DROP_ZONE, IN_HAPPY_DROP_ZONE, HOLDING_SAD_OBJECT, HOLDING_HAPPY_OBJECT }

public class P_PickUpObject : MonoBehaviour
{
    [SerializeField] GameObject sadRoom, happyRoom, oldSadRoom, oldHappyRoom;
    [SerializeField] Transform hand, winText;
    [SerializeField] Sprite winSprite;

    SpriteRenderer handRenderer;
    Transform objectInTrigger;
    HoldStates currentState = HoldStates.DEFAULT;

    private void Start()
    {
        handRenderer = hand.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentState == HoldStates.IN_SAD_TRIGGER) PickUpObject(false);
            if (currentState == HoldStates.IN_HAPPY_TRIGGER) PickUpObject(true);
            else if (currentState == HoldStates.IN_SAD_DROP_ZONE) DropObject(false);
            else if (currentState == HoldStates.IN_HAPPY_DROP_ZONE) DropObject(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SadPickup" && currentState != HoldStates.HOLDING_HAPPY_OBJECT && currentState != HoldStates.HOLDING_SAD_OBJECT)
        {
            currentState = HoldStates.IN_SAD_TRIGGER;
            objectInTrigger = other.transform;
            handRenderer.enabled = true;
        }
        else if (other.tag == "SadDropZone")
        {
            currentState = HoldStates.IN_SAD_DROP_ZONE;
        }
        if (other.tag == "HappyPickup" && currentState != HoldStates.HOLDING_HAPPY_OBJECT && currentState != HoldStates.HOLDING_SAD_OBJECT)
        {
            currentState = HoldStates.IN_HAPPY_TRIGGER;
            objectInTrigger = other.transform;
            handRenderer.enabled = true;
        }
        else if (other.tag == "HappyDropZone")
        {
            currentState = HoldStates.IN_HAPPY_DROP_ZONE;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentState == HoldStates.IN_SAD_TRIGGER)
        {
            currentState = HoldStates.DEFAULT;
            objectInTrigger = null;
            handRenderer.enabled = false;
        }
    }

    void PickUpObject(bool happy)
    {
        objectInTrigger.position = hand.position;
        objectInTrigger.SetParent(transform);
        objectInTrigger.GetComponent<Collider>().enabled = false;
        objectInTrigger.GetComponent<Rigidbody>().isKinematic = true;
        handRenderer.enabled = false;

        currentState = happy ? HoldStates.HOLDING_HAPPY_OBJECT : HoldStates.HOLDING_SAD_OBJECT;
    }

    void DropObject(bool happy)
    {
        objectInTrigger.SetParent(null);
        objectInTrigger.GetComponent<Rigidbody>().isKinematic = false;

        if (happy)
        {
            happyRoom.SetActive(true);
            oldHappyRoom.SetActive(false);
        }
        else
        {
            sadRoom.SetActive(true);
            oldSadRoom.SetActive(false);
        }

        if (sadRoom.activeInHierarchy && happyRoom.activeInHierarchy)
        {
            handRenderer.sprite = winSprite;
            handRenderer.enabled = true;
            winText.gameObject.SetActive(true);
        }

        currentState = HoldStates.DEFAULT;
    }
}
