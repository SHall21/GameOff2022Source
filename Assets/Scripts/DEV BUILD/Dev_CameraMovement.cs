using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dev_CameraMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float camPause = 3f;
    [SerializeField] private Vector3 aimDirection;
    [SerializeField] Transform player;
    Dev_UITimer Timer;
    Dev_UIHamBroEvents hamBroEvents;
    [SerializeField] private Transform FOVOrigin;
    [SerializeField] private Transform FOVAngle;
    [SerializeField] private Transform pfFieldOfView;
    private FieldOfView fieldOfView;
    [SerializeField] private float fov = 35f; //FOV SETTING TIGHTENS BEAM MAKING IT NARROW OR WIDE
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float camRotation = 120;  //120 is default

    Dev_AudioPlayer audioPlayer;
    private Vector3 lastMoveDir;
    private State state;
    [SerializeField] float waitTimer;
    private bool coolDown;
    private bool inLight;
    public float coolDownTime = 1.5f;
    float lastSeen;

    private bool rotateA;
    private bool rotateB;

    private enum State {
        Waiting,
        Moving,
        PlayerSeen,
        Busy,
    }

    void Start()
    {
        Timer = FindObjectOfType<Dev_UITimer>();
        lastMoveDir = aimDirection;
        state = State.Waiting;
        hamBroEvents = FindObjectOfType<Dev_UIHamBroEvents>();
        fieldOfView = Instantiate(pfFieldOfView, null).GetComponent<FieldOfView>();
        fieldOfView.SetFoV(fov);
        fieldOfView.SetViewDistance(viewDistance);
    }

    private void Awake()
    {
        audioPlayer = FindObjectOfType<Dev_AudioPlayer>();
        rotateA = true;
    }

    private void Update()
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
            break;
        }

        if (!inLight) {
            StopCoroutine(CoolDown());
        }

        if (fieldOfView != null) {
            fieldOfView.SetOrigin(FOVOrigin.position);
            fieldOfView.SetAimDirection(lastMoveDir);
        }
    }

    private void HandleMovement() {
        switch (state) {
        case State.Waiting:
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) {
                state = State.Moving;
            }
            break;
        case State.Moving:
            if (camRotation != 0)
            {
                //transform.localEulerAngles = new Vector3(0, 0, Mathf.PingPong(Time.time * moveSpeed, camRotation));
                Vector3 waypointDir = (FOVAngle.position - transform.position).normalized;
                lastMoveDir = waypointDir;

                if (rotateA) {
                    Quaternion targetRotationA = Quaternion.Euler(new Vector3(0, 0, camRotation));
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationA, moveSpeed * Time.deltaTime);

                    if (transform.rotation == targetRotationA) {
                        rotateA = false;
                        rotateB = true;
                        StartCoroutine(PauseNow());
                    }
                } else if (rotateB) {
                    Quaternion targetRotationB = Quaternion.Euler(new Vector3(0, 0, 0));
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotationB, moveSpeed * Time.deltaTime);

                    if (transform.rotation == targetRotationB) {
                        rotateB = false;
                        rotateA = true;
                        StartCoroutine(PauseNow());
                    }
                }
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

    IEnumerator PauseNow()
    {
        state = State.Busy;
        //Debug.Log($"Pausing between moves");
        yield return new WaitForSeconds(2f);
        state = State.Moving;
    }

    IEnumerator CoolDown()
    {
        state = State.Busy;
        Debug.Log($"Player in light removing time, cool down set: {coolDown}");
        audioPlayer.SpottedClip();
        hamBroEvents.StartSpotSpeech();
        Timer.InViewOfLight();
        yield return new WaitForSeconds(camPause);

        inLight = false;
        state = State.Moving;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Vector3 GetAimDir() {
        return lastMoveDir;
    }

/*     void FlipEnemyFacing()
    {
        Vector2 moveDirection =  transform.localScale;
        if (moveDirection.x != -1)
        {
            transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
        }
        else {
             transform.localScale = new Vector2(transform.localScale.x *-1, 1f);
        }
    } */
}
