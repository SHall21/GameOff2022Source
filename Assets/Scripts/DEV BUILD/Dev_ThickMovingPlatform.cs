using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_ThickMovingPlatform : MonoBehaviour
{
    [SerializeField] Transform originalPositionPoint;
    [SerializeField] Transform moveToPoint;
    [SerializeField] float movingSpeed = 2.5f;
    [SerializeField] float waitUntilMoveUp = 0.2f;
    [SerializeField] float waitUntilAudioStop = 1.75f;

    private bool movePlatform = false;
    private bool touchingPlatform = false;
    private bool touchingPlatformNormal = false;
    private bool startedMovingDown = false;
    private bool startedMovingUp = false;
    private Transform originalA;
    private Transform originalB;
    private Dev_CharacterController controller;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Awake()
    {
        controller = FindObjectOfType<Dev_CharacterController>();
        audioSource = GetComponent<AudioSource>();

        if (originalPositionPoint == null)
        {
            Debug.LogError($"{nameof(originalPositionPoint)} is null for {this.name}, please set in Unity inspector");
        }

        if (moveToPoint == null)
        {
            Debug.LogError($"{nameof(moveToPoint)} is null for {this.name}, please set in Unity inspector");
        }

        if (controller == null)
        {
            Debug.LogError($"{nameof(controller)} is null for {this.name}, please set in Unity inspector");
        }        
        
        if (audioSource == null)
        {
            Debug.LogError($"{nameof(audioSource)} is null for {this.name}, please set in Unity inspector");
        }

        originalA = originalPositionPoint;
        originalB = moveToPoint;
        transform.position = originalA.position;
        touchingPlatformNormal = false;
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
        else
        {
            startedMovingUp = false;
            audioSource.Stop();
        }

        if (!startedMovingUp && !startedMovingDown)
        {
            audioSource.Stop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchingPlatformNormal = true;

            if (controller.currentHamsterState == HamsterState.Thick)
            {
                audioSource.Play();
                startedMovingDown = true;
                movePlatform = true;
                touchingPlatform = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(WaitToMoveUp());
            StartCoroutine(StopAudio());
            touchingPlatformNormal = false;
        }
    }

    private IEnumerator WaitToMoveUp()
    {
        touchingPlatform = false;
        yield return new WaitForSeconds(waitUntilMoveUp);

        if (!touchingPlatform)
        {
            audioSource.Play();
            startedMovingUp = true;
            startedMovingDown = false;
            movePlatform = false;
        }
    }

    private IEnumerator StopAudio()
    {
        yield return new WaitForSeconds(waitUntilAudioStop);
        if (!touchingPlatform && !touchingPlatformNormal)
        {
            audioSource.Stop();
        }
    }
}
