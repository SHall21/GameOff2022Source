using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewCharacterController : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private float speedMovingBetweenTube = 4f;
    [SerializeField] private float speedMovingInTubes = 15f;
    [SerializeField] private float rotationSpeedThicc = 225f;
    [SerializeField] CapsuleCollider2D myCapsuleColliderThick2D;

    const float k_GroundedRadius = .4f;                                         // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;                                                    // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    public HamsterState currentHamsterState;

    // Entering and exit tube data
    private bool canEnterTubeEntrance;
    private bool canExitTubeEntrance;
    private Transform aMovePoint;
    private Transform bMovePoint;
    private bool isItAboveOrBelow;
    private bool isItOnTheRightToEnter;
    private bool goingToPointA;
    private bool goingToPointB;
    private bool enteringTube;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent TubeStateEvent;
    public UnityEvent ThickStateEvent;
    public UnityEvent NormalStateEvent;
    public UnityEvent MovingBetweenTubeStateEvent;


    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (TubeStateEvent == null)
            TubeStateEvent = new UnityEvent();

        if (ThickStateEvent == null)
            ThickStateEvent = new UnityEvent();
        
        if (NormalStateEvent == null)
            NormalStateEvent = new UnityEvent();        
        
        if (MovingBetweenTubeStateEvent == null)
            MovingBetweenTubeStateEvent = new UnityEvent();

        // Start as Normal
        currentHamsterState = HamsterState.Normal;
        canEnterTubeEntrance = false;
        canExitTubeEntrance = false;
    }

    // Update is called once per frame (not fixed update due to a bug with the jumping)
    void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float horizontalMove, float verticalMove)
    {
        switch (currentHamsterState)
        {
            case HamsterState.Normal:
                if (canEnterTubeEntrance)
                {
                    if (isItAboveOrBelow)
                    {
                        CheckTubeEnteringAboveOrBelow(verticalMove, horizontalMove);
                    }
                    else
                    {
                        CheckTubeEnteringSide(verticalMove, horizontalMove);
                    }
                }
                MoveNormal(horizontalMove);
                break;
            case HamsterState.Tube:
                if (canExitTubeEntrance)
                {
                    if (isItAboveOrBelow)
                    {
                        CheckTubeExitingAboveOrBelow(verticalMove, horizontalMove);
                    }
                    else
                    {
                        CheckTubeExitingSide(verticalMove, horizontalMove);
                    }
                }
                MoveTube(horizontalMove, verticalMove);
                break;
            case HamsterState.Thick:
                MoveThick(verticalMove, horizontalMove);
                break;
            case HamsterState.MovingBetweenTube:
                MovingToAandB();
                break;
            default:
                Debug.LogError("Unknown Hamster State");
                break;
        }
    }

    private void MoveThick(float verticalMove, float horizontalMove)
    {
        if (horizontalMove != 0 && verticalMove == 0)
        {
            // Check you are pressing right
            if (horizontalMove > 0)
            {
                m_Rigidbody2D.freezeRotation = true;
                m_Rigidbody2D.rotation = m_Rigidbody2D.rotation - (rotationSpeedThicc * Time.deltaTime);

                Vector3 targetVelocity = new Vector2(horizontalMove * 10f, m_Rigidbody2D.velocity.y);
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            }
            else
            {
                m_Rigidbody2D.freezeRotation = true;
                m_Rigidbody2D.rotation = m_Rigidbody2D.rotation + (rotationSpeedThicc * Time.deltaTime);

                Vector3 targetVelocity = new Vector2(horizontalMove * 10f, m_Rigidbody2D.velocity.y);
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            }
        }
        else
        {
            m_Rigidbody2D.freezeRotation = false;
            m_Rigidbody2D.velocity = Vector3.zero;
        }
    }

    private void CheckTubeEnteringAboveOrBelow(float moveVertical, float moveHorizontal)
    {
        if (moveVertical != 0 && moveHorizontal == 0)
        {
            // Check you are pressing down
            if (moveVertical < 0)
            {
                MovingBetweenTubeStateEvent.Invoke();
                goingToPointA = true;
            }
        }
    }

    private void CheckTubeEnteringSide(float moveVertical, float moveHorizontal)
    {
        if (moveHorizontal != 0 && moveVertical == 0 && m_Grounded)
        {
            // Check you are pressing right
            if (moveHorizontal > 0 && isItOnTheRightToEnter)
            {
                MovingBetweenTubeStateEvent.Invoke();
                goingToPointA = true;
            }

            // Check you are pressing left
            if (moveHorizontal < 0 && !isItOnTheRightToEnter)
            {
                MovingBetweenTubeStateEvent.Invoke();
                goingToPointA = true;
            }
        }
    }

    private void CheckTubeExitingSide(float moveVertical, float moveHorizontal)
    {
        if (moveHorizontal != 0 && moveVertical == 0)
        {
            // Check you are pressing right
            if (moveHorizontal > 0 && !isItOnTheRightToEnter)
            {
                MovingBetweenTubeStateEvent.Invoke();
                goingToPointA = true;
            }

            // Check you are pressing left
            if (moveHorizontal < 0 && isItOnTheRightToEnter)
            {
                MovingBetweenTubeStateEvent.Invoke();
                goingToPointA = true;
            }
        }
    }

    private void CheckTubeExitingAboveOrBelow(float moveVertical, float moveHorizontal)
    {
        if (moveVertical != 0 && moveHorizontal == 0)
        {
            // Check you are pressing down
            if (moveVertical > 0)
            {
                MovingBetweenTubeStateEvent.Invoke();
                goingToPointA = true;
            }
        }
    }

    private void MovingToAandB()
    {
        if (goingToPointA)
        {
            if (Vector3.Distance(transform.position, aMovePoint.position) > speedMovingBetweenTube * Time.deltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, aMovePoint.position, speedMovingBetweenTube * Time.deltaTime);
            }
            else
            {
                goingToPointA = false;
                goingToPointB = true;
            }
        }
        else if (goingToPointB)
        {
            if (Vector3.Distance(transform.position, bMovePoint.position) > speedMovingBetweenTube * Time.deltaTime)
            {
                transform.position = Vector3.MoveTowards(transform.position, bMovePoint.position, speedMovingBetweenTube * Time.deltaTime);
            }
            else
            {
                goingToPointB = false;
                if (enteringTube)
                {
                    TubeStateEvent.Invoke();
                }
                else
                {
                    NormalStateEvent.Invoke();
                }
            }
        }
    }

    void MoveNormal(float move)
    {
        //only control the player if grounded
        if (m_Grounded)
        {

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }

    void MoveTube(float moveHorizontal, float moveVertical)
    {
        Vector2 moveVector = new Vector2(moveHorizontal, moveVertical);
         m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + (moveVector * speedMovingInTubes * Time.deltaTime));

         // If the input is moving the player right and the player is facing left...
         if (moveHorizontal > 0 && !m_FacingRight)
         {
             // ... flip the player.
             Flip();
         }
         // Otherwise if the input is moving the player left and the player is facing right...
         else if (moveHorizontal < 0 && m_FacingRight)
         {
             // ... flip the player.
             Flip();
         }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void SetTubeData(bool enterTube, bool exitTube, Transform entranceTransform, Transform exitTransform, bool aboveOrBelow, bool onTheRight)
    {
        canEnterTubeEntrance = enterTube;
        canExitTubeEntrance = exitTube;
        aMovePoint = entranceTransform;
        bMovePoint = exitTransform;
        isItAboveOrBelow = aboveOrBelow;
        isItOnTheRightToEnter = onTheRight;
        enteringTube = enterTube;
    }
}
