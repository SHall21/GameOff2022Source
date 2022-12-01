using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Dev_Stealable : MonoBehaviour
{
    public bool inRange = false;
    public UnityEvent stealAction;
    Dev_Inventory inventory;
    Dev_SceneSession sceneSession;
    Dev_AudioPlayer audioPlayer;
    Dev_CharacterController controller;
    Dev_UIHamBroEvents hamBroEvents;
    [SerializeField] private InputActionReference eatInput;
    private bool isFirst;

    void Start()
    {
        eatInput.action.performed += Eat;
    }

    void Awake()
    {
        inventory = FindObjectOfType<Dev_Inventory>();
        sceneSession = FindObjectOfType<Dev_SceneSession>();
        audioPlayer = FindObjectOfType<Dev_AudioPlayer>();
        hamBroEvents = FindObjectOfType<Dev_UIHamBroEvents>();
        controller = FindObjectOfType<Dev_CharacterController>();
        isFirst = true;

        if (inventory == null)
        {
            Debug.Log($"The prop {nameof(inventory)} is set to null for {this.name}");
        }

        if (sceneSession == null)
        {
            Debug.Log($"The prop {nameof(sceneSession)} is set to null for {this.name}");
        }

        if (controller == null)
        {
            Debug.Log($"The prop {nameof(controller)} is set to null for {this.name}");
        }
    }

    public void Eat(InputAction.CallbackContext context)
    {

        if (inRange && context.performed && inventory.inventory.Count < 1)
        {
            audioPlayer.ChompClip();
            stealAction.Invoke();
            controller.ThickStateEvent.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;

            if (isFirst)
            {
                hamBroEvents.ControlsSpeech();
                isFirst = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
