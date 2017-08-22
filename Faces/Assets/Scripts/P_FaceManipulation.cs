using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates { NO_FACES_COMPLETE, NPC1_COMPLETE, NPC2_COMPLETE, BOTH_FACES_COMPLETE, WON }

public class P_FaceManipulation : MonoBehaviour
{
    public static bool inTheater = true;
    [SerializeField] SkinnedMeshRenderer target1, target2, npc1, npc2, npc1Doppelganger, npc2Doppelganger;
    [SerializeField] GameObject screen, stairs, happy, scared, angry, sad, wall;

    Slider[] sliders = new Slider[2];
    SkinnedMeshRenderer face;
    GameStates currentState = GameStates.NO_FACES_COMPLETE;
    bool inTrigger;

    private void Update()
    {
        UpdateSliderInput();
        UpdateGameStates();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Face")
        {
            inTrigger = true;
            face = other.GetComponent<SkinnedMeshRenderer>();
            //StartCoroutine(FadeText(face.GetComponentInChildren<TextMesh>(), true));

            if (other.GetComponentInChildren<Slider>() != null)
            {
                Slider[] childSliders = other.GetComponentsInChildren<Slider>();

                sliders[0] = childSliders[0];
                sliders[1] = childSliders[1];
            }
            
            if (!inTheater)
            {
                npc1.GetComponent<SphereCollider>().enabled = true;
                npc2.GetComponent<SphereCollider>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;

        if (other.tag == "Face")
        {
            //StartCoroutine(FadeText(face.GetComponentInChildren<TextMesh>(), false));
        }
    }

    void UpdateSliderInput()
    {
        if (inTrigger && (face == target1 || face == target2))
        {
            face.SetBlendShapeWeight(2, sliders[0].value * 100);
            face.SetBlendShapeWeight(0, sliders[1].value * 100);
        }
    }

    // TODO refactor - this whole machine is VERY LAZY CODE, needs complete rewrite
    void UpdateGameStates()
    {
        switch (currentState)
        {
            case GameStates.NO_FACES_COMPLETE:
                if (inTrigger)
                {
                    if (face == target1)
                    {
                        // if scared
                        if (face.GetBlendShapeWeight(0) > 90)
                        {
                            FixFace(npc1Doppelganger, npc1, scared, happy);
                            currentState = GameStates.NPC1_COMPLETE;
                        }

                        // if happy
                        else if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc1Doppelganger, npc1, happy, scared);
                            currentState = GameStates.NPC1_COMPLETE;
                        }
                        else
                        {
                            happy.SetActive(false);
                            scared.SetActive(false);
                        }
                    }
                    else if (face == target2)
                    {
                        // if sad
                        if (face.GetBlendShapeWeight(0) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2, sad, angry);
                            currentState = GameStates.NPC2_COMPLETE;
                        }

                        // if angry
                        else if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2, angry, sad);
                            currentState = GameStates.NPC2_COMPLETE;
                        }
                        else
                        {
                            angry.SetActive(false);
                            sad.SetActive(false);
                        }
                    }
                }
                break;
            case GameStates.NPC1_COMPLETE:
                if (inTrigger)
                {
                    if (face == target2)
                    {
                        // if sad
                        if (face.GetBlendShapeWeight(0) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2, sad, angry);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }

                        // if angry
                        else if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2, angry, sad);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }
                        else
                        {
                            angry.SetActive(false);
                            sad.SetActive(false);
                        }
                    }
                }
                break;
            case GameStates.NPC2_COMPLETE:
                if (inTrigger)
                {
                    if (face == target1)
                    {
                        // if scared
                        if (face.GetBlendShapeWeight(0) > 90)
                        {
                            FixFace(npc1Doppelganger, npc1, scared, happy);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }

                        // if happy
                        else if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc1Doppelganger, npc1, happy, scared);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }
                        else
                        {
                            happy.SetActive(false);
                            scared.SetActive(false);
                        }
                    }
                }
                break;
            case GameStates.BOTH_FACES_COMPLETE:
                if (happy.activeInHierarchy && sad.activeInHierarchy && inTheater)
                {
                    Win(true);
                }
                else if (happy.activeInHierarchy && angry.activeInHierarchy)
                {
                    if (face == npc1Doppelganger)
                    {
                        npc2.GetComponent<SphereCollider>().enabled = true;
                        Restart();
                    }
                    else if (face == npc2Doppelganger)
                    {
                        npc1.GetComponent<SphereCollider>().enabled = true;
                        Restart();
                    }
                }
                else if (scared.activeInHierarchy && angry.activeInHierarchy && inTheater)
                {
                    Win(false);
                }
                else if (scared.activeInHierarchy && sad.activeInHierarchy)
                {
                    if (face == npc1Doppelganger)
                    {
                        npc2.GetComponent<SphereCollider>().enabled = true;
                        Restart();
                    }
                    else if (face == npc2Doppelganger)
                    {
                        npc1.GetComponent<SphereCollider>().enabled = true;
                        Restart();
                    }
                }
                break;
            default:
                break;
        }
    }

    void FixFace(SkinnedMeshRenderer doppelgangerFace, SkinnedMeshRenderer npcFace,
        GameObject correctExpression, GameObject incorrectExpression)
    {
        doppelgangerFace.GetComponent<SphereCollider>().enabled = true;
        npcFace.GetComponent<SphereCollider>().enabled = false;

        doppelgangerFace.SetBlendShapeWeight(0, 100);
        npcFace.SetBlendShapeWeight(0, 100);

        correctExpression.SetActive(true);
        incorrectExpression.SetActive(false);
    }

    void Restart()
    {
        happy.SetActive(false);
        scared.SetActive(false);
        angry.SetActive(false);
        sad.SetActive(false);

        sliders[0].value = 0;
        sliders[1].value = 0;

        npc1Doppelganger.GetComponent<SphereCollider>().enabled = false;
        npc2Doppelganger.GetComponent<SphereCollider>().enabled = false;

        npc1.SetBlendShapeWeight(2, 0);
        npc1.SetBlendShapeWeight(0, 0);
        npc2.SetBlendShapeWeight(2, 0);
        npc2.SetBlendShapeWeight(0, 0);
        npc1Doppelganger.SetBlendShapeWeight(2, 0);
        npc1Doppelganger.SetBlendShapeWeight(0, 0);
        npc2Doppelganger.SetBlendShapeWeight(2, 0);
        npc2Doppelganger.SetBlendShapeWeight(0, 0);
        target1.SetBlendShapeWeight(2, 0);
        target1.SetBlendShapeWeight(0, 0);
        target2.SetBlendShapeWeight(2, 0);
        target2.SetBlendShapeWeight(0, 0);

        currentState = GameStates.NO_FACES_COMPLETE;
    }

    void Win(bool npc1WasTheKiller)
    {
        if (npc1WasTheKiller)
        {
            happy.GetComponent<TextMesh>().text = "killer";
            sad.GetComponent<TextMesh>().text = "victim";
        }
        else
        {
            scared.GetComponent<TextMesh>().text = "witness";
            angry.GetComponent<TextMesh>().text = "killer";
        }

        stairs.SetActive(true);
        screen.SetActive(false);
        wall.SetActive(false);

        npc1.GetComponent<SphereCollider>().enabled = false;
        npc2.GetComponent<SphereCollider>().enabled = false;
        npc1Doppelganger.GetComponent<SphereCollider>().enabled = false;
        npc2Doppelganger.GetComponent<SphereCollider>().enabled = false;
        target1.GetComponent<SphereCollider>().enabled = false;
        target2.GetComponent<SphereCollider>().enabled = false;

        currentState = GameStates.WON;
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
