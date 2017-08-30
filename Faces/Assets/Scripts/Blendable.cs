using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FaceStates { NEUTRAL, HAPPY, SAD }

/*
 * BLEND SHAPE KEY
 * 
 * 0 = mouth open
 * 1 = smile
 * 2 = eyebrows raised
 * 3 = eyebrows lowered (sad)
 * 4 = eyebrows furrowed
 * 5 = frown
 * 
 */

public class Blendable : MonoBehaviour
{
    public delegate void FaceChanged(FaceStates state);
    public static event FaceChanged OnFaceChanged;

    [SerializeField] Slider mouthSlider, eyebrowSlider;
    [SerializeField] FaceStates targetExpression;
    [SerializeField] bool mouthOpen;
    [SerializeField] Door door;

    SkinnedMeshRenderer myRenderer;
    Color originalColor;
    bool canCallEvent = true;

    private void Start()
    {
        myRenderer = GetComponent<SkinnedMeshRenderer>();
        originalColor = mouthSlider.GetComponentInChildren<Image>().color;

        mouthSlider.onValueChanged.AddListener(delegate { CheckIfFaceIsCorrect(); });
        eyebrowSlider.onValueChanged.AddListener(delegate { CheckIfFaceIsCorrect(); });
    }

    private void Update()
    {
        UpdateFaceToMatchSliderInput();
    }

    void UpdateFaceToMatchSliderInput()
    {
        myRenderer.SetBlendShapeWeight(1, mouthSlider.value - 50);
        myRenderer.SetBlendShapeWeight(5, -(mouthSlider.value - 50));
        if (mouthOpen) myRenderer.SetBlendShapeWeight(0, (mouthSlider.value - 50) * .75f);

        myRenderer.SetBlendShapeWeight(2, eyebrowSlider.value - 50);
        myRenderer.SetBlendShapeWeight(3, -(eyebrowSlider.value - 50));

        if (mouthSlider.value == 100 && eyebrowSlider.value == 100)
            ChangeFaceColor(Color.yellow);
        else if (mouthSlider.value == 0 && eyebrowSlider.value == 0)
            ChangeFaceColor(Color.blue);
        else ChangeFaceColor(originalColor);
    }

    void CheckIfFaceIsCorrect()
    {
        switch (targetExpression)
        {
            case FaceStates.HAPPY:
                if (mouthSlider.value == 100 && eyebrowSlider.value == 100 &&
                    OnFaceChanged != null)
                {
                    OnFaceChanged(FaceStates.HAPPY);
                }
                break;
            case FaceStates.SAD:
                if (mouthSlider.value == 0 && eyebrowSlider.value == 0 &&
                    OnFaceChanged != null)
                {
                    OnFaceChanged(FaceStates.SAD);
                }
                break;
            case FaceStates.NEUTRAL:
                if (mouthSlider.value == 100 && eyebrowSlider.value == 100 &&
                    OnFaceChanged != null)
                {
                    OnFaceChanged(FaceStates.HAPPY);
                }
                if (mouthSlider.value == 0 && eyebrowSlider.value == 0 &&
                    OnFaceChanged != null)
                {
                    OnFaceChanged(FaceStates.SAD);
                }
                break;
        }
    }

    void ChangeFaceColor(Color newColor)
    {
        foreach (Image i in mouthSlider.GetComponentsInChildren<Image>())
            i.color = newColor;
        foreach (Image i in eyebrowSlider.GetComponentsInChildren<Image>())
            i.color = newColor;
    }
}
