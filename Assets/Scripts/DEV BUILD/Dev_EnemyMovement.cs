using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_EnemyMovement : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypointList;
    [SerializeField] private List<float> waitTimeList;
    [SerializeField] float moveSpeed = 1f;
    private int waypointIndex;
    [SerializeField] private Vector3 aimDirection;
    [SerializeField] Transform player;
    Dev_UITimer Timer;
    Dev_UIHamBroEvents hamBroEvents;
    [SerializeField] private Transform waypoint;
    [SerializeField] private Transform pfFieldOfView;
    private FieldOfView fieldOfView;
    [SerializeField] private float fov = 10f; //FOV SETTING TIGHTENS BEAM MAKING IT NARROW OR WIDE
    [SerializeField] private float viewDistance = 60f;

    private Animator animator;

    Dev_AudioPlayer audioPlayer;
    Rigidbody2D myRigidbody;
    private Vector3 lastMoveDir;
    private State state;
    private float waitTimer;
    private bool coolDown;
    private bool inLight;
    public float coolDownTime = 1.5f;
    float lastSeen;

    private enum State {
        Waiting,
        Moving,
        PlayerSeen,
        Busy,
    }

    private void Awake()
    {
        audioPlayer = FindObjectOfType<Dev_AudioPlayer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        myRigidbody = GetComponent<Rigidbody2D>();
        Timer = FindObjectOfType<Dev_UITimer>();
        hamBroEvents = FindObjectOfType<Dev_UIHamBroEvents>();
        lastMoveDir = aimDirection;
        state = State.Waiting;
        waitTimer = waitTimeList[0];

        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
    }

    void Update()
    {
        switch (state) {
        default:
        case State.Waiting:
        case State.Moving:
            HandleMovement();
            FindTargetPlayer();
            break;
        case State.PlayerSeen:
            //PlayerSpotted();
            //break;
        case State.Busy:
            animator.SetBool("isMoving", false);
            break;
        }

        if (!inLight) {
            StopCoroutine(CoolDown());
        }

        if (fieldOfView != null) {
            fieldOfView.SetOrigin(waypoint.position);
            fieldOfView.SetAimDirection(lastMoveDir);
        }
    }

    private void HandleMovement() {
        switch (state) {
        case State.Waiting:
            waitTimer -= Time.deltaTime;
            //animator.SetBool("isMoving", true);
            if (waitTimer <= 0f) {
                state = State.Moving;
            }
            break;
        case State.Moving:
            Vector3 waypoint = waypointList[waypointIndex];

            Vector3 waypointDir = (waypoint - transform.position).normalized;
            animator.SetBool("isMoving", true);
            lastMoveDir = waypointDir;

            float distanceBefore = Vector3.Distance(transform.position, waypoint);
            //animatedWalker.SetMoveVector(waypointDir);
            transform.position = transform.position + waypointDir * moveSpeed * Time.deltaTime;
            float distanceAfter = Vector3.Distance(transform.position, waypoint);

            float arriveDistance = .1f;
            if (distanceAfter < arriveDistance || distanceBefore <= distanceAfter) {
                // Go to next waypoint
                FlipEnemyFacing();
                waitTimer = waitTimeList[waypointIndex];
                waypointIndex = (waypointIndex + 1) % waypointList.Count;
                state = State.Waiting;
            }
            break;
        }
    }

    private void FindTargetPlayer() {
        if (Vector3.Distance(GetPosition(), player.position) < viewDistance) {
            // Player inside viewDistance
            Vector3 dirToPlayer = (player.position - GetPosition()).normalized;
            if (Vector3.Angle(GetAimDir(), dirToPlayer) < fov / 2f) {
                // Player inside Field of View
                for (int i = 0; i <= 2; i++) {
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(GetPosition(), dirToPlayer, viewDistance);
                    if (raycastHit2D.collider != null) {
                        // Hit something
                        if (raycastHit2D.collider.gameObject.CompareTag("Player")) {

                            if (inLight == false) {
                                // Hit Player
                                inLight = true;
                                //PlayerSpotted();
                                StartCoroutine(CoolDown());
                            }
                        } else {
                            // Hit something else
                        }
                    }
                }
            }
        }
    }

    IEnumerator CoolDown()
    {
        state = State.Busy;
        Debug.Log($"Player in light removing time, cool down set: {coolDown}");
        audioPlayer.SpottedClip();
        hamBroEvents.StartSpotSpeech();
        Timer.InViewOfLight();
        yield return new WaitForSeconds(5f);

        inLight = false;
        state = State.Moving;
    }

/*     private void PlayerSpotted() {
        state = State.Busy;
        if (inLight) {
            inLight = false;
            //Debug.Log("Found And Hitting Player");
            StartCoroutine(CoolDown());
        }

        if (!coolDown) {
            Debug.Log($"Player not in light, cool down set: {coolDown}");
            state = State.Moving;
            StopCoroutine(CoolDown());
        } else {
            state = State.PlayerSeen;
        }
    } */

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Vector3 GetAimDir() {
        return lastMoveDir;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        Vector2 moveDirection =  transform.localScale;
        if (moveDirection.x != -1)
        {
            transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
        }
        else {
             transform.localScale = new Vector2(transform.localScale.x *-1, 1f);
        }
    }
}
