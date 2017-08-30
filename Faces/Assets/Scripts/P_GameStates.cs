using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates {  START, DOOR1_UNLOCKED, DOOR2_HALF_UNLOCKED, DOOR2_UNLOCKED, DOOR3_HALF_UNLOCKED_SAD, DOOR3_HALF_UNLOCKED_HAPPY, DOOR3_UNLOCKED, HAPPY_DOOR_UNLOCKED, SAD_DOOR_UNLOCKED }

public class P_GameStates : MonoBehaviour
{
    [SerializeField] GameObject door1, door2, door3, happyDoor, sadDoor;

    GameStates currentState = GameStates.START;
    int rollerCoasterPassengers;

    private void OnEnable()
    {
        Blendable.OnFaceChanged += ChangeState;
    }
    private void OnDisable()
    {
        Blendable.OnFaceChanged -= ChangeState;
    }

    void ChangeState(FaceStates state)
    {
        switch (currentState)
        {
            case GameStates.START:
                if (state == FaceStates.HAPPY)
                {
                    door1.SetActive(false);
                    currentState = GameStates.DOOR1_UNLOCKED;
                }
                break;
            case GameStates.DOOR1_UNLOCKED:
                if (state == FaceStates.HAPPY)
                {
                    currentState = GameStates.DOOR2_HALF_UNLOCKED;
                }
                break;
            case GameStates.DOOR2_HALF_UNLOCKED:
                if (state == FaceStates.HAPPY)
                {
                    door2.SetActive(false);
                    currentState = GameStates.DOOR2_UNLOCKED;
                }
                break;
            case GameStates.DOOR2_UNLOCKED:
                if (state == FaceStates.SAD)
                {
                    currentState = GameStates.DOOR3_HALF_UNLOCKED_SAD;
                }
                else if (state == FaceStates.HAPPY)
                {
                    currentState = GameStates.DOOR3_HALF_UNLOCKED_HAPPY;
                }
                break;
            case GameStates.DOOR3_HALF_UNLOCKED_SAD:
                if (state == FaceStates.HAPPY)
                {
                    door3.SetActive(false);
                    currentState = GameStates.DOOR3_UNLOCKED;
                }
                break;
            case GameStates.DOOR3_HALF_UNLOCKED_HAPPY:
                if (state == FaceStates.SAD)
                {
                    door3.SetActive(false);
                    currentState = GameStates.DOOR3_UNLOCKED;
                }
                break;
            case GameStates.DOOR3_UNLOCKED:
                if (state == FaceStates.HAPPY)
                {
                    happyDoor.SetActive(false);
                    sadDoor.SetActive(true);
                    //currentState = GameStates.HAPPY_DOOR_UNLOCKED;
                }
                else if (state == FaceStates.SAD)
                {
                    happyDoor.SetActive(true);
                    sadDoor.SetActive(false);
                    //currentState = GameStates.SAD_DOOR_UNLOCKED;
                }
                else
                {
                    happyDoor.SetActive(true);
                    sadDoor.SetActive(true);
                }
                break;
            case GameStates.HAPPY_DOOR_UNLOCKED:
                break;
            case GameStates.SAD_DOOR_UNLOCKED:
                break;
            default:
                break;
        }
    }
}
