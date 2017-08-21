using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;
    
    Vector3 playerLocalPosition;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
        playerLocalPosition = other.transform.localPosition;
        other.transform.SetParent(destination);
        other.transform.localPosition = playerLocalPosition;
    }
}
