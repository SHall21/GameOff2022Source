using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float waitForSecondsDeath = 1f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);

    public CharacterController2D controller;
    public Animator animator;
    public AudioSource quack;
    public AudioSource death;
    public float PitchVariance = 0f;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    Vector2 moveInput;
    bool jump = false;
    bool crouch = false;
    public bool isAlive;

    CircleCollider2D myCircleCollider2D;
    BoxCollider2D myBoxCollider2D;
    Rigidbody2D myRigidbody2D;

    private void Awake()
    {
        isAlive = true;
        myCircleCollider2D = GetComponent<CircleCollider2D>();
        myBoxCollider2D = GetComponent<BoxCollider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnLanding()
    {
        if (!isAlive)
            return;

        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        if (!isAlive)
            return;

        animator.SetBool("isCrouching", isCrouching);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!isAlive)
            return;

        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isAlive)
            return;

        if (context.performed && !crouch && !controller.m_wasCrouching)
        {
            jump = true;
            animator.SetBool("isJumping", true);
            JumpQuack();
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (!isAlive)
            return;

        if (context.action.WasPressedThisFrame() && context.performed)
        {
            crouch = true;
            CrouchQuack();
        }
        else if(context.action.WasReleasedThisFrame() && !context.performed)
        {
            crouch = false;
        }
    }

    // Use fixed update for genral physics 
    void FixedUpdate()
    {
        if (isAlive)
        {
            Movement();
            Die();
        }
    }

    private void Movement()
    {
        // Move our character
        horizontalMove = moveInput.x * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void Die()
    {
        if (myBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy")) || myCircleCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy")))
        {
            isAlive = false;
            animator.SetBool("isJumping", false);
            animator.SetTrigger("Death");
            death.Play();
            myRigidbody2D.velocity = deathKick;
            myCircleCollider2D.enabled = false;
            myBoxCollider2D.enabled = false;
            StartCoroutine(ProcessPlayerDeath());
        }
    }

    IEnumerator ProcessPlayerDeath()
    {
        yield return new WaitForSecondsRealtime(waitForSecondsDeath);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void JumpQuack()
    {
        if (!quack.isPlaying)
        {
            quack.pitch = PitchVariance;
            quack.Play();
            PitchVariance = PitchVariance + 0.5f;
        }
        if (PitchVariance > 2f)
        {
            PitchVariance = 1f;
        }
    }

    private void CrouchQuack()
    {
        if (!quack.isPlaying)
        {
            quack.pitch = 0.75f;
            quack.Play();
        }
        if (PitchVariance > 2f)
        {
            PitchVariance = 1f;
        }
    }
}