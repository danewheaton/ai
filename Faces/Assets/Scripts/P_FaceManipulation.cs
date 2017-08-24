using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates { NO_FACES_COMPLETE, NPC1_COMPLETE, NPC2_COMPLETE, BOTH_FACES_COMPLETE, WON }

/*
 * TODO
 * 
 * - completely rewrite state machine (UpdateGameStates function)
 * - instead of UpdateSliderInput function, make an IBlendable interface that is applied to objects rather than
 * the player
 * - make it generally more portable/adaptable
 * 
 */

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

                StartCoroutine(FadeSliders(true));
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

        if (other.tag == "Face" && other.GetComponentInChildren<Slider>() != null)
        {
            StartCoroutine(FadeSliders(false));
        }
    }

    void UpdateSliderInput()
    {
        if (inTrigger && face != npc1 && face != npc2)
        {
            face.SetBlendShapeWeight(2, sliders[0].value * 100);
            face.SetBlendShapeWeight(0, sliders[1].value * 100);

            if (face == target1)
            {
                if (face.GetBlendShapeWeight(0) <= 90) scared.SetActive(false);
                else if (!happy.activeInHierarchy) scared.SetActive(true);
                if (face.GetBlendShapeWeight(2) <= 90) happy.SetActive(false);
                else if (!scared.activeInHierarchy) happy.SetActive(true);
            }
            else if (face == target2)
            {
                if (face.GetBlendShapeWeight(0) <= 90) sad.SetActive(false);
                else if (!angry.activeInHierarchy) sad.SetActive(true);
                if (face.GetBlendShapeWeight(2) <= 90) angry.SetActive(false);
                else if (!sad.activeInHierarchy) angry.SetActive(true);
            }
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
                            FixFace(npc1Doppelganger, npc1);
                            currentState = GameStates.NPC1_COMPLETE;
                        }

                        // if happy
                        if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc1Doppelganger, npc1);
                            currentState = GameStates.NPC1_COMPLETE;
                        }
                    }
                    else if (face == target2)
                    {
                        // if sad
                        if (face.GetBlendShapeWeight(0) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2);
                            currentState = GameStates.NPC2_COMPLETE;
                        }

                        // if angry
                        if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2);
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
                        // if sad
                        if (face.GetBlendShapeWeight(0) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }

                        // if angry
                        if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc2Doppelganger, npc2);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
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
                            FixFace(npc1Doppelganger, npc1);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }

                        // if happy
                        if (face.GetBlendShapeWeight(2) > 90)
                        {
                            FixFace(npc1Doppelganger, npc1);
                            currentState = GameStates.BOTH_FACES_COMPLETE;
                        }
                    }
                }
                break;
            case GameStates.BOTH_FACES_COMPLETE:
                if (happy.activeInHierarchy && sad.activeInHierarchy && inTheater) Win(true);
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

    void FixFace(SkinnedMeshRenderer doppelgangerFace, SkinnedMeshRenderer npcFace)
    {
        doppelgangerFace.GetComponent<SphereCollider>().enabled = true;
        npcFace.GetComponent<SphereCollider>().enabled = false;

        doppelgangerFace.SetBlendShapeWeight(0, 100);
        npcFace.SetBlendShapeWeight(0, 100);
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

    IEnumerator FadeSliders(bool fadeIn)
    {
        Color originalColor = fadeIn ? new Color(1, 1, 1, 0) : Color.white,
            newColor = fadeIn ? Color.white : new Color(1, 1, 1, 0);

        float elapsedTime = 0, fadeTimer = 1;
        while (elapsedTime < fadeTimer)
        {
            foreach (Slider s in sliders)
            {
                foreach (Image i in s.GetComponentsInChildren<Image>())
                    i.color = Color.Lerp(originalColor, newColor, elapsedTime / fadeTimer);
            }

            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        foreach (Slider s in sliders)
        {
            foreach (Image i in s.GetComponentsInChildren<Image>())
                i.color = newColor;
        }
    }
}
