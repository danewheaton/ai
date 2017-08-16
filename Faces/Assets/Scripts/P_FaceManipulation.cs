using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FaceManipulation : MonoBehaviour
{
    [SerializeField] KeyCode smileKey = KeyCode.Alpha1, frownKey = KeyCode.Alpha2, eyesClosedKey = KeyCode.Alpha3, shrinkEarsKey = KeyCode.Alpha4, warpKey = KeyCode.Alpha5;

    SkinnedMeshRenderer face;
    bool inTrigger;

    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKey(smileKey))
            {
                face.SetBlendShapeWeight(4, face.GetBlendShapeWeight(4) + 5);
            }
            else if (Input.GetKey(frownKey))
            {
                face.SetBlendShapeWeight(3, face.GetBlendShapeWeight(3) + 5);
            }
            else if (Input.GetKey(eyesClosedKey))
            {
                face.SetBlendShapeWeight(2, face.GetBlendShapeWeight(2) + 5);
            }
            else if (Input.GetKey(shrinkEarsKey))
            {
                face.SetBlendShapeWeight(1, face.GetBlendShapeWeight(1) + 5);
            }
            else if (Input.GetKey(warpKey))
            {
                face.SetBlendShapeWeight(0, face.GetBlendShapeWeight(0) + 5);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Face")
        {
            inTrigger = true;
            face = other.GetComponent<SkinnedMeshRenderer>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }
}
