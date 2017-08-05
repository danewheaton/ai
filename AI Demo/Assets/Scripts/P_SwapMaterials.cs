using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SwapMaterials : MonoBehaviour
{
    public delegate void Won();
    public static event Won OnWon;
    public KeyCode swapKey = KeyCode.Space;

    [SerializeField] GameObject finalObject;

    SwappableMaterial swappable;
    List<GameObject> swappableObjects = new List<GameObject>();
    bool inSwappableTrigger, won;

    private void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("VectorObject"))
            swappableObjects.Add(g);

        finalObject.SetActive(false);
    }

    private void Update()
    {
        if (inSwappableTrigger && Input.GetKeyDown(swapKey))
        {
            swappable.FixMaterial();
            swappableObjects.Remove(swappable.gameObject);
        }

        if (swappableObjects.Count == 1) finalObject.SetActive(true);

        else if (swappableObjects.Count <= 0 && !won && OnWon != null)
        {
            won = true;
            OnWon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "VectorObject")
        {
            swappable = other.GetComponent<SwappableMaterial>();
            inSwappableTrigger = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        inSwappableTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "VectorObject") inSwappableTrigger = false;
    }
}
