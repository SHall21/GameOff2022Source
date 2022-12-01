using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiccMovingPlatform : MonoBehaviour
{
    [SerializeField] Transform originalPositionPoint;
    [SerializeField] Transform moveToPoint;
    [SerializeField] NewCharacterController controller;
    [SerializeField] float movingSpeed = 3f;

    private bool movePlatform = false;
    private Transform originalA;
    private Transform originalB;


    // Start is called before the first frame update
    void Awake()
    {
        if (originalPositionPoint == null)
        {
            Debug.LogError("originalPositionPoint is null for Chandelier, please set in Unity inspector");
        }

        if (moveToPoint == null)
        {
            Debug.LogError("moveToPoint is null for Chandelier, please set in Unity inspector");
        }

        if (controller == null)
        {
            Debug.LogError("controller is null for Chandelier, please set in Unity inspector");
        }

        originalA = originalPositionPoint;
        originalB = moveToPoint;
        transform.position = originalA.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var step = movingSpeed * Time.deltaTime;
        if (movePlatform && transform.position.y != originalB.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalB.position, step);
        }
        else if (!movePlatform && originalA.position != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalA.position, step);
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller.currentHamsterState == HamsterState.Thick)
            {
                movePlatform = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movePlatform = false;
        }
    }
}
