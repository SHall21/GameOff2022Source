using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Lee_Stealable : MonoBehaviour
{
    public bool inRange = false;
    public UnityEvent stealAction;
    Lee_Inventory inventory;
    Lee_ScoreKeeper scoreKeeper;
    Lee_AudioPlayer audioPlayer;
    [SerializeField] private InputActionReference eatInput;

    void Start()
    {
        eatInput.action.performed += Eat;
    }

    void Awake()
    {
        inventory = FindObjectOfType<Lee_Inventory>();
        scoreKeeper = FindObjectOfType<Lee_ScoreKeeper>();
        audioPlayer = FindObjectOfType<Lee_AudioPlayer>();
    }

    public void Eat(InputAction.CallbackContext context)
    {

        if (inRange && context.performed && inventory.inventory.Count < 1) 
        {  
            //scoreKeeper.TriggerAlarm();
            audioPlayer.ChompClip();
            stealAction.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
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
