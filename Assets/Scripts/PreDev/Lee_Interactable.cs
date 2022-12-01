using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Lee_Interactable : MonoBehaviour
{
    public bool inRange = false;
    public UnityEvent interactAction;
    [SerializeField] private InputActionReference eatInput;

    void Start()
    {
        eatInput.action.performed += Eat;
    }

    public void Eat(InputAction.CallbackContext context)
    {

        if (inRange && context.performed) 
        {  
            Debug.Log("pressing button to turn disable stuff");
            interactAction.Invoke();
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
