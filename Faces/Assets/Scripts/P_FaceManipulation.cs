using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FaceManipulation : MonoBehaviour
{
    [SerializeField] KeyCode smileKey = KeyCode.Alpha1, frownKey = KeyCode.Alpha2, eyesClosedKey = KeyCode.Alpha3, shrinkEarsKey = KeyCode.Alpha4, warpKey = KeyCode.Alpha5;
    [SerializeField] Material winMaterial;
    [SerializeField] SkinnedMeshRenderer happyFace, sadFace, stressedFace;

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

            if ((face == happyFace && face.GetBlendShapeWeight(4) > 100) ||
                (face == sadFace && face.GetBlendShapeWeight(3) > 100) ||
                (face == stressedFace && face.GetBlendShapeWeight(2) > 100 && face.GetBlendShapeWeight(3) > 100))
            {
                inTrigger = false;
                face.material = winMaterial;
                foreach (Collider c in face.GetComponents<Collider>()) if (c.isTrigger) c.enabled = false;
                TextMesh faceMesh = face.GetComponentInChildren<TextMesh>();
                faceMesh.color = Color.yellow;
                faceMesh.text = "Correct!";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Face")
        {
            inTrigger = true;
            face = other.GetComponent<SkinnedMeshRenderer>();
            StartCoroutine(FadeText(face.GetComponentInChildren<TextMesh>(), true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;

        if (other.tag == "Face")
        {
            StartCoroutine(FadeText(face.GetComponentInChildren<TextMesh>(), false));
        }
    }

    IEnumerator FadeText(TextMesh textToFade, bool fadeIn)
    {
        Color originalColor = fadeIn ? Color.clear : Color.white, newColor = fadeIn ? Color.white : Color.clear;

        float elapsedTime = 0, fadeTimer = 1;
        while (elapsedTime < fadeTimer)
        {
            textToFade.color = Color.Lerp(originalColor, newColor, elapsedTime / fadeTimer);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        textToFade.color = newColor;
    }
}
