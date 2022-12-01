using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dev_PlayerMovement : MonoBehaviour
{

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    Vector2 moveInput;
    public bool isAlive;
    public bool winState;

    [SerializeField] CapsuleCollider2D myCapsuleColliderNormal2D;
    [SerializeField] CapsuleCollider2D myCapsuleColliderThick2D;
    [SerializeField] CapsuleCollider2D myCapsuleColliderTube2D;
    [SerializeField] float hamsterThiccGravity = 5f;
    Rigidbody2D myRigidbody2D;

    private float hamsterGravity;
    private Dev_CharacterController controller;
    private Animator animator;
    private bool changingState;
    Dev_UITimer timer;


    private void Awake()
    {
        controller = FindObjectOfType<Dev_CharacterController>();
        timer = FindObjectOfType<Dev_UITimer>();
        animator = GetComponent<Animator>();

        if (controller == null)
        {
            Debug.LogError($"{nameof(controller)} is null for {this.name}, please check in Unity inspector");
        }

        if (animator == null)
        {
            Debug.LogError($"{nameof(animator)} is null for {this.name}, please check in Unity inspector");
        }

        if (timer == null)
        {
            Debug.LogError($"{nameof(timer)} is null for {this.name}, please check in Unity inspector");
        }

        isAlive = true;
        winState = false;
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

        if (timer.isFirst)
            return;

        moveInput = context.ReadValue<Vector2>();
    }


    public void ThickDebug(InputAction.CallbackContext context)
    {
        // TODO: Add back in!!!
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
        if (isAlive && !winState)
        {
            Movement();
        }
    }

    private void Movement()
    {
        // Move our character
        horizontalMove = moveInput.x * runSpeed;
        verticalMove = moveInput.y * runSpeed;
        if (!changingState)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            animator.SetFloat("SpeedVer", Mathf.Abs(verticalMove));
        }

        controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime);
    }

    private void EnableThickState()
    {
        changingState = true;
        myCapsuleColliderThick2D.enabled = true;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = false;
        animator.SetFloat("Speed", 0);
        animator.SetInteger("HamsterState", 2);
        myRigidbody2D.gravityScale = hamsterThiccGravity;
        myRigidbody2D.velocity = Vector3.zero;
        myRigidbody2D.freezeRotation = true;
        controller.currentHamsterState = HamsterState.Thick;
        changingState = false;
    }

    private void EnableNormalState()
    {
        changingState = false;
        myCapsuleColliderThick2D.enabled = false;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = true;
        animator.SetInteger("HamsterState", 0);
        myRigidbody2D.gravityScale = hamsterGravity;
        myRigidbody2D.velocity = Vector3.zero;
        myRigidbody2D.freezeRotation = true;
        controller.currentHamsterState = HamsterState.Normal;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void EnableTubeState()
    {
        changingState = false;
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
        changingState = true;
        animator.SetFloat("Speed", 0);
        animator.SetFloat("SpeedVer", 0);
        controller.currentHamsterState = HamsterState.MovingBetweenTube;
        animator.SetInteger("HamsterState", 3);
        myCapsuleColliderThick2D.enabled = false;
        myCapsuleColliderTube2D.enabled = false;
        myCapsuleColliderNormal2D.enabled = false;
        myRigidbody2D.gravityScale = 0;
        myRigidbody2D.freezeRotation = true;
        myRigidbody2D.velocity = Vector3.zero;
    }

    public void EnableWin()
    {
        StartCoroutine(WinDance());
    }

    private IEnumerator WinDance()
    {
        winState = true;
        controller.winState = true;
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(0.55f);
        animator.SetBool("Win", true);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
