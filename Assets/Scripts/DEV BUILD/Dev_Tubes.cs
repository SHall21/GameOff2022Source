using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_Tubes : MonoBehaviour
{
    [SerializeField] Transform entranceMovePoint;
    [SerializeField] Transform exitMovePoint;
    [SerializeField] bool isItBelowOrAbove;
    [SerializeField] bool isItOnTheRightToEnter;

    private Dev_CharacterController controller;

    void Awake()
    {
        controller = FindObjectOfType<Dev_CharacterController>();

        if (entranceMovePoint == null)
        {
            Debug.LogError("EntranceMovePoint is null for Tube, please set in Unity inspector");
        }

        if (exitMovePoint == null)
        {
            Debug.LogError("exitMovePoint is null for Tube, please set in Unity inspector");
        }

        if (controller == null)
        {
            Debug.LogError("controller is null for Tube, please check in Unity inspector");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller.currentHamsterState == HamsterState.Normal)
            {
                controller.SetTubeData(true, false, entranceMovePoint, exitMovePoint, isItBelowOrAbove, isItOnTheRightToEnter);
            }
            else if (controller.currentHamsterState == HamsterState.Tube)
            {
                // bools switched as you are moving from the exit to the entrance
                controller.SetTubeData(false, true, exitMovePoint, entranceMovePoint, isItBelowOrAbove, isItOnTheRightToEnter);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller.currentHamsterState == HamsterState.Normal)
            {
                controller.SetTubeData(false, false, entranceMovePoint, exitMovePoint, isItBelowOrAbove, isItOnTheRightToEnter);
            }
            else if (controller.currentHamsterState == HamsterState.Tube)
            {
                // bools switched as you are moving from the exit to the entrance
                controller.SetTubeData(false, false, exitMovePoint, entranceMovePoint, isItBelowOrAbove, isItOnTheRightToEnter);
            }
        }
    }
}
