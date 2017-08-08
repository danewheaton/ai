using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Checkpoints : MonoBehaviour
{
    Vector3 spawnPosition, spawnEulers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            spawnPosition = transform.position;
            spawnEulers = transform.eulerAngles;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Respawn")
        {
            transform.position = spawnPosition;
            transform.eulerAngles = spawnEulers;

            GetComponent<P_SwapMaterials>().ResetMaterials();
        }
    }
}
