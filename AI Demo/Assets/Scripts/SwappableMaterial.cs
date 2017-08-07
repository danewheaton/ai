using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappableMaterial : MonoBehaviour
{
    [SerializeField] Material geometryMaterial, vectorMaterial, winMaterial;

    Renderer myRenderer;

    private void Start()
    {
        tag = "VectorObject";
        myRenderer = GetComponent<Renderer>();
        myRenderer.material = vectorMaterial;

        P_SwapMaterials.OnWon += ChangeToWinMaterial;
    }

    public void FixMaterial()
    {
        myRenderer.material = geometryMaterial;
        tag = "Untagged";
    }

    void ChangeToWinMaterial()
    {
        myRenderer.material = winMaterial;
    }
}
