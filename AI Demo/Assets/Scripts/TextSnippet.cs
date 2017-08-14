using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSnippet : MonoBehaviour
{
    [SerializeField] float fadeTimer = 1, minChildFade = 2, maxChildFade = 6;
    [SerializeField] Font[] fonts;

    TextMesh[] childTexts;
    Transform player;
    TextMesh myTextMesh;

    bool pumpIsDisassembled;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        myTextMesh = GetComponent<TextMesh>();
        myTextMesh.color = Color.clear;

        childTexts = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh t in childTexts) t.color = Color.clear;
    }

    private void Update()
    {
        transform.LookAt(2 * transform.position - player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !pumpIsDisassembled)
        {
            StartCoroutine(FadeColor(true));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FadeChildTexts(pumpIsDisassembled ? false : true));

            pumpIsDisassembled = !pumpIsDisassembled;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !pumpIsDisassembled)
        {
            StartCoroutine(FadeColor(false));
        }
    }

    public IEnumerator FadeChildTexts(bool fadeIn)
    {
        foreach (TextMesh t in childTexts)
        {
            Color originalColor = t.color, targetColor = fadeIn ? Color.white : Color.clear;

            float elapsedTime = 0, timer = Random.Range(minChildFade, maxChildFade);
            while (elapsedTime < timer)
            {
                t.color = Color.Lerp(originalColor, targetColor, elapsedTime / timer);

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            t.color = targetColor;
        }
    }

    IEnumerator FadeColor(bool fadeIn)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTimer)
        {
            myTextMesh.color = Color.Lerp(fadeIn ? Color.clear : Color.white, fadeIn ? Color.white : Color.clear, elapsedTime / fadeTimer);

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        myTextMesh.color = fadeIn ? Color.white : Color.clear;
    }
}
