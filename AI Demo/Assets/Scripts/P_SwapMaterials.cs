using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class P_SwapMaterials : MonoBehaviour
{
    public KeyCode swapKey = KeyCode.Space;

    [SerializeField] BoxCollider bluePump, greenPump, redPump, yellowPump;
    [SerializeField] SwappableMaterial firstBluePipes, blueSystem, greenSystem, redSystem, yellowSystem, levelGeo;
    [SerializeField] GameObject wall, cube, everything;
    [SerializeField] Material black, sky;

    bool blueIsActivated, redIsActivated, greenIsActivated, puzzleComplete, pumpIsDisassembled;

    private void Start()
    {
        ResetMaterials();
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (other == bluePump)
            {
                if (puzzleComplete)
                {
                    wall.SetActive(false);
                    yellowSystem.ChangeMaterial(false);

                    levelGeo.ChangeMaterial(true);
                    firstBluePipes.ChangeMaterial(true);
                    blueSystem.ChangeMaterial(true);
                    redSystem.ChangeMaterial(true);
                    greenSystem.ChangeMaterial(true);

                    levelGeo.GetComponent<MeshCollider>().enabled = true;
                }
                else
                {
                    if (blueIsActivated)
                    {
                        blueSystem.ChangeMaterial(true);
                        blueIsActivated = false;
                    }
                    else
                    {
                        blueSystem.ChangeMaterial(false);
                        blueIsActivated = true;
                    }
                }

                if (redIsActivated && greenIsActivated && blueIsActivated) puzzleComplete = true;
            }
            else if (other == redPump)
            {
                if (redIsActivated)
                {
                    redSystem.ChangeMaterial(true);
                    redIsActivated = false;
                }
                else
                {
                    redSystem.ChangeMaterial(false);
                    redIsActivated = true;
                }
            }
            else if (other == greenPump)
            {
                if (greenIsActivated)
                {
                    greenSystem.ChangeMaterial(true);
                    greenIsActivated = false;
                }
                else
                {
                    greenSystem.ChangeMaterial(false);
                    greenIsActivated = true;
                }
            }
            else if (other == yellowPump)
            {
                Application.Quit();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Animator pumpAnim = other.GetComponentInChildren<Animator>();
            pumpAnim.SetBool("CanDisassemble", pumpIsDisassembled ? false : true);

            everything.SetActive(pumpIsDisassembled ? true : false);
            RenderSettings.skybox = pumpIsDisassembled ? sky : black;

            if (other == bluePump)
            {
                redPump.gameObject.SetActive(pumpIsDisassembled ? true : false);
                greenPump.gameObject.SetActive(pumpIsDisassembled ? true : false);
            }
            if (other == redPump)
            {
                bluePump.gameObject.SetActive(pumpIsDisassembled ? true : false);
                greenPump.gameObject.SetActive(pumpIsDisassembled ? true : false);
            }
            if (other == greenPump)
            {
                redPump.gameObject.SetActive(pumpIsDisassembled ? true : false);
                bluePump.gameObject.SetActive(pumpIsDisassembled ? true : false);
            }

            pumpIsDisassembled = !pumpIsDisassembled;
        }
    }

    public void ResetMaterials()
    {
        levelGeo.ChangeMaterial(false);
        firstBluePipes.ChangeMaterial(false);
        redSystem.ChangeMaterial(true);
        blueSystem.ChangeMaterial(true);
        greenSystem.ChangeMaterial(true);
        yellowSystem.ChangeMaterial(true);
    }
}
