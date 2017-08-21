using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates { NONE_COMPLETE, NPC1_COMPLETE, NPC2_COMPLETE, BOTH_COMPLETE, WON }

public class P_FaceManipulation : MonoBehaviour
{
    public static bool inTheater = true;

    [SerializeField] KeyCode smileKey = KeyCode.Alpha1, frownKey = KeyCode.Alpha2, eyesClosedKey = KeyCode.Alpha3, shrinkEarsKey = KeyCode.Alpha4, warpKey = KeyCode.Alpha5;
    [SerializeField] SkinnedMeshRenderer target1, target2, npc1, npc2, npc1Doppelganger, npc2Doppelganger;
    [SerializeField] GameObject screen, stairs, happy, scared, angry, sad, wall;

    SkinnedMeshRenderer face;
    GameStates currentState = GameStates.NONE_COMPLETE;
    bool inTrigger;

    private void Update()
    {
        UpdateInput();
        UpdateGameStates();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Face")
        {
            inTrigger = true;
            face = other.GetComponent<SkinnedMeshRenderer>();
            //StartCoroutine(FadeText(face.GetComponentInChildren<TextMesh>(), true));
            
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

    void UpdateInput()
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

    void UpdateGameStates()
    {
        switch (currentState)
        {
            case GameStates.NONE_COMPLETE:
                if (inTrigger)
                {
                    if (face == target1)
                    {
                        if (face.GetBlendShapeWeight(3) > 100)
                        {
                            FixFace(npc1Doppelganger, npc1, scared, happy);
                            currentState = GameStates.NPC1_COMPLETE;
                        }
                        else if (face.GetBlendShapeWeight(4) > 100)
                        {
                            FixFace(npc1Doppelganger, npc1, happy, scared);
                            currentState = GameStates.NPC1_COMPLETE;
                        }
                    }
                    else if (face == target2)
                    {
                        if (face.GetBlendShapeWeight(3) > 100)
                        {
                            FixFace(npc2Doppelganger, npc2, sad, angry);
                            currentState = GameStates.NPC2_COMPLETE;
                        }
                        else if (face.GetBlendShapeWeight(4) > 100)
                        {
                            FixFace(npc2Doppelganger, npc2, angry, sad);
                            currentState = GameStates.NPC2_COMPLETE;
                        }
                    }
                }
                break;
            case GameStates.NPC1_COMPLETE:
                if (inTrigger)
                {
                    if (face == target2)
                    {
                        if (face.GetBlendShapeWeight(3) > 100)
                        {
                            FixFace(npc2Doppelganger, npc2, sad, angry);
                            currentState = GameStates.BOTH_COMPLETE;
                        }
                        else if (face.GetBlendShapeWeight(4) > 100)
                        {
                            FixFace(npc2Doppelganger, npc2, angry, sad);
                            currentState = GameStates.BOTH_COMPLETE;
                        }
                    }
                }
                break;
            case GameStates.NPC2_COMPLETE:
                if (inTrigger)
                {
                    if (face == target1)
                    {
                        if (face.GetBlendShapeWeight(3) > 100)
                        {
                            FixFace(npc1Doppelganger, npc1, scared, happy);
                            currentState = GameStates.BOTH_COMPLETE;
                        }
                        else if (face.GetBlendShapeWeight(4) > 100)
                        {
                            FixFace(npc1Doppelganger, npc1, happy, scared);
                            currentState = GameStates.BOTH_COMPLETE;
                        }
                    }
                }
                break;
            case GameStates.BOTH_COMPLETE:
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

        doppelgangerFace.SetBlendShapeWeight(3, 100);
        npcFace.SetBlendShapeWeight(3, 100);

        correctExpression.SetActive(true);
        incorrectExpression.SetActive(false);
    }

    void Restart()
    {
        print("restarted");

        happy.SetActive(false);
        scared.SetActive(false);
        angry.SetActive(false);
        sad.SetActive(false);

        npc1Doppelganger.GetComponent<SphereCollider>().enabled = false;
        npc2Doppelganger.GetComponent<SphereCollider>().enabled = false;

        npc1.SetBlendShapeWeight(4, 0);
        npc1.SetBlendShapeWeight(3, 0);
        npc1.SetBlendShapeWeight(2, 0);
        npc1.SetBlendShapeWeight(1, 0);
        npc1.SetBlendShapeWeight(0, 0);
        npc2.SetBlendShapeWeight(4, 0);
        npc2.SetBlendShapeWeight(3, 0);
        npc2.SetBlendShapeWeight(2, 0);
        npc2.SetBlendShapeWeight(1, 0);
        npc2.SetBlendShapeWeight(0, 0);
        npc1Doppelganger.SetBlendShapeWeight(4, 0);
        npc1Doppelganger.SetBlendShapeWeight(3, 0);
        npc1Doppelganger.SetBlendShapeWeight(2, 0);
        npc1Doppelganger.SetBlendShapeWeight(1, 0);
        npc1Doppelganger.SetBlendShapeWeight(0, 0);
        npc2Doppelganger.SetBlendShapeWeight(4, 0);
        npc2Doppelganger.SetBlendShapeWeight(3, 0);
        npc2Doppelganger.SetBlendShapeWeight(2, 0);
        npc2Doppelganger.SetBlendShapeWeight(1, 0);
        npc2Doppelganger.SetBlendShapeWeight(0, 0);
        target1.SetBlendShapeWeight(4, 0);
        target1.SetBlendShapeWeight(3, 0);
        target1.SetBlendShapeWeight(2, 0);
        target1.SetBlendShapeWeight(1, 0);
        target1.SetBlendShapeWeight(0, 0);
        target2.SetBlendShapeWeight(4, 0);
        target2.SetBlendShapeWeight(3, 0);
        target2.SetBlendShapeWeight(2, 0);
        target2.SetBlendShapeWeight(1, 0);
        target2.SetBlendShapeWeight(0, 0);

        currentState = GameStates.NONE_COMPLETE;
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
