using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NewPlayerMovement : MonoBehaviour
{
    public NewCharacterController controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    Vector2 moveInput;
    public bool isAlive;

    [SerializeField] CapsuleCollider2D myCapsuleColliderNormal2D;
    [SerializeField] CapsuleCollider2D myCapsuleColliderThick2D;
    [SerializeField] CapsuleCollider2D myCapsuleColliderTube2D;
    [SerializeField] float hamsterThiccGravity = 5f;
    Rigidbody2D myRigidbody2D;

    private float hamsterGravity;

    private void Awake()
    {
        isAlive = true;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCapsuleColliderThick2D.enabled = false;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = true;
        hamsterGravity = myRigidbody2D.gravityScale;
    }

    public void OnLanding()
    {
        if (!isAlive)
            return;
    }

    public void TubeState()
    {
        if (!isAlive)
            return;
        
        EnableTubeState();
    }

    public void ThickState()
    {
        if (!isAlive)
            return;

        EnableThickState();
    }

    public void NormalState()
    {
        if (!isAlive)
            return;

        EnableNormalState();
    }

    public void MovingBetweenTubeState()
    {
        if (!isAlive)
            return;

        EnableMovingBetweenTubeState();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!isAlive)
            return;

        moveInput = context.ReadValue<Vector2>();
    }

    
    public void ThickDebug(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        if (context.performed && controller.currentHamsterState == HamsterState.Normal)
        {
            EnableThickState();
        } 
        else if (context.performed && controller.currentHamsterState == HamsterState.Thick)
        {
            EnableNormalState();
        }
#endif
    }

    // Use fixed update for genral physics 
    void FixedUpdate()
    {
        if (isAlive)
        {
            Movement();
        }
    }

    private void Movement()
    {
        // Move our character
        horizontalMove = moveInput.x * runSpeed;
        verticalMove = moveInput.y * runSpeed;
        //animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
    }

    private void EnableThickState()
    {
        myCapsuleColliderThick2D.enabled = true;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = false;
        animator.SetInteger("HamsterState", 2);
        myRigidbody2D.gravityScale = hamsterThiccGravity;
        myRigidbody2D.velocity = Vector3.zero;
        myRigidbody2D.freezeRotation = true;
        controller.currentHamsterState = HamsterState.Thick;
    }

    private void EnableNormalState()
    {
        myCapsuleColliderThick2D.enabled = false;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = true;
        animator.SetInteger("HamsterState", 0);
        myRigidbody2D.gravityScale = hamsterGravity;
        myRigidbody2D.velocity = Vector3.zero;
        myRigidbody2D.freezeRotation = true;
        controller.currentHamsterState = HamsterState.Normal;
        transform.rotation = new Quaternion(0,0,0,0);
    }

    private void EnableTubeState()
    {
        myCapsuleColliderThick2D.enabled = false;
        myCapsuleColliderTube2D.enabled = true;
        myCapsuleColliderNormal2D.enabled = false;
        myRigidbody2D.gravityScale = 0;
        myRigidbody2D.velocity = Vector3.zero;
        animator.SetInteger("HamsterState", 1);
        myRigidbody2D.freezeRotation = true;
        controller.currentHamsterState = HamsterState.Tube;
    }

    private void EnableMovingBetweenTubeState()
    {
        controller.currentHamsterState = HamsterState.MovingBetweenTube;
        animator.SetInteger("HamsterState", 3);
        myCapsuleColliderThick2D.enabled = false;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = false;
        myRigidbody2D.gravityScale = 0;
        myRigidbody2D.freezeRotation = true;
        myRigidbody2D.velocity = Vector3.zero;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
