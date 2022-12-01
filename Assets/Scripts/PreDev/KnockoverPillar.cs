using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockoverPillar : MonoBehaviour
{
    [SerializeField] NewCharacterController controller;
    [SerializeField] Transform pivotPoint;

    private bool fallingOver = false;
    public float rotateTime = 1.0f;
    public float rotateDegrees = 90.0f;
    private bool rotating = false;

    void Awake()
    {
        if (controller == null)
        {
            Debug.LogError($"controller is null for {this.name}, please set in Unity inspector");
        }

        if (pivotPoint == null)
        {
            Debug.LogError($"pivotPoint is null for {this.name}, please set in Unity inspector");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller.currentHamsterState == HamsterState.Thick && !fallingOver)
            {
                fallingOver = true;
                StartCoroutine(Rotate(transform, pivotPoint, Vector3.back, -rotateDegrees, rotateTime));
            }
        }
    }

    // Check latest answer here: https://forum.unity.com/threads/rotating-exactly-90-degrees-specific-direction-answered.44056/
    private IEnumerator Rotate(Transform camTransform, Transform targetTransform, Vector3 rotateAxis, float degrees, float totalTime)
    {
        if (rotating)
            yield return null;
        rotating = true;

        Quaternion startRotation = camTransform.rotation;
        Vector3 startPosition = camTransform.position;
        // Get end position;
        transform.RotateAround(targetTransform.position, rotateAxis, degrees);
        Quaternion endRotation = camTransform.rotation;
        Vector3 endPosition = camTransform.position;
        camTransform.rotation = startRotation;
        camTransform.position = startPosition;

        float rate = degrees / totalTime;
        //Start Rotate
        for (float i = 0.0f; Mathf.Abs(i) < Mathf.Abs(degrees); i += Time.deltaTime * rate)
        {
            camTransform.RotateAround(targetTransform.position, rotateAxis, Time.deltaTime * rate);
            yield return null;
        }

        camTransform.rotation = endRotation;
        camTransform.position = endPosition;
        rotating = false;
    }
}
