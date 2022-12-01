using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_KnockoverPillar : MonoBehaviour
{
    
    [SerializeField] Transform pivotPoint;

    private bool fallingOver = false;
    public float rotateTime = 0.85f;
    public float rotateDegrees = 90.0f;
    private bool rotating = false;
    private Dev_CharacterController controller;
    private Dev_AudioPlayer audioPlayer;

    void Awake()
    {
        controller = FindObjectOfType<Dev_CharacterController>();
        audioPlayer = FindObjectOfType<Dev_AudioPlayer>();

        if (controller == null)
        {
            Debug.LogError($"{nameof(controller)} is null for {this.name}, please set in Unity inspector");
        }        
        
        if (audioPlayer == null)
        {
            Debug.LogError($"{nameof(audioPlayer)} is null for {this.name}, please set in Unity inspector");
        }

        if (pivotPoint == null)
        {
            Debug.LogError($"{nameof(pivotPoint)} is null for {this.name}, please set in Unity inspector");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (controller.currentHamsterState == HamsterState.Thick && !fallingOver)
            {
                fallingOver = true;
                audioPlayer.PillarHitClip();
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
        audioPlayer.PillarDownClip();
    }
}
