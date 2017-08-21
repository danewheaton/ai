using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        Transform player = other.transform;
        player.SetParent(transform);
        Vector3 playerLocalPosition = player.localPosition;

        player.SetParent(destination);
        player.localPosition = playerLocalPosition;
        player.SetParent(null);
        P_FaceManipulation.inTheater = !P_FaceManipulation.inTheater;
    }
}
