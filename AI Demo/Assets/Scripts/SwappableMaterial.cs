using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]

public class SwappableMaterial : MonoBehaviour
{
    [SerializeField] Material geometryMaterial, vectorMaterial;

    Collider myCollider;
    Renderer myRenderer;

    private void Start()
    {
        myCollider = GetComponent<Collider>();
        myRenderer = GetComponent<Renderer>();

        if (tag == "Dummy") ChangeMaterial(false);
    }

    public void ChangeMaterial(bool changeToWireframe)
    {
        myCollider.enabled = changeToWireframe ? false : true;
        myRenderer.material = changeToWireframe ? vectorMaterial : geometryMaterial;

        if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = changeToWireframe ? false : true;
    }
}
