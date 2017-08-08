using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_HUD : MonoBehaviour
{
    [SerializeField] Text hudText;
    [SerializeField] float fadeTimer = 1;

    P_SwapMaterials swapper;
    bool won, inTrigger;

    private void Start()
    {
        swapper = GetComponent<P_SwapMaterials>();

        hudText.text = "Press " + swapper.swapKey + " to fix texture";
        hudText.color = Color.clear;
    }

    private void Update()
    {
        if (Input.GetKeyDown(swapper.swapKey) && !won) StartCoroutine(FadeText(false));
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "VectorObject" && !won && hudText.color == Color.clear)
        {
            inTrigger = true;
            StartCoroutine(FadeText(true));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        inTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "VectorObject" && !won)
        {
            inTrigger = false;
            Invoke("CoolDownAndCallFadeOut", fadeTimer);
        }
    }

    void CallWinCoroutine()
    {
        StopAllCoroutines();
        StartCoroutine(DisplayWinText());
    }

    void CoolDownAndCallFadeOut()
    {
        if (!inTrigger) StartCoroutine(FadeText(false));
    }

    IEnumerator FadeText (bool fadeIn)
    {
        Color originalColor = hudText.color,
            newColor = fadeIn ? Color.white : Color.clear;

        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            hudText.color = Color.Lerp(originalColor, newColor, elapsedTime / fadeTimer);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        hudText.color = newColor;
    }

    IEnumerator DisplayWinText()
    {
        won = true;
        hudText.text = "You win! Yaaaay! (Press esc)";

        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            hudText.color = Color.Lerp(Color.clear, Color.magenta, elapsedTime / fadeTimer);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        hudText.color = Color.magenta;
    }
}
