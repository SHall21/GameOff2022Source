using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Lee_UIChecklist : MonoBehaviour
{
    public RectTransform checkListPanel;
    public bool displayPanel;
    bool panelOpen;
    [SerializeField] private InputActionReference menuInput;

    void Awake()
    {
        displayPanel = true;
    }

    void Start()
    {
        menuInput.action.performed += Menu;
    }

    void Update()
    {
        if (displayPanel)
        {
            StartCoroutine(PanelDisplay());
        }

        if (!displayPanel)
        {
            StopCoroutine(PanelDisplay());
        }
    }

    public void Menu(InputAction.CallbackContext context)
    {

        if (context.performed && !panelOpen) 
        {   

            Debug.Log("pressing button  to open");
            StartCoroutine(OpenMenu());
        }

        if (context.performed && panelOpen) 
        {   

            Debug.Log("pressing button  to close");
            StopCoroutine(OpenMenu());
            CloseCheckListPanel();
            panelOpen = false;
        }
    }

    private IEnumerator OpenMenu()
    {
        OpenCheckListPanel();
        yield return new WaitForSeconds(.5f);

        panelOpen = true;
    }

    private IEnumerator PanelDisplay()
    {
        if (displayPanel) {
            OpenCheckListPanel();
            yield return new WaitForSeconds(3f);
        }

        CloseCheckListPanel();
        displayPanel = false;
    }

    public void OpenCheckListPanel()
    {
        checkListPanel.DOAnchorPos(Vector2.zero,0.5f);
    }

    public void CloseCheckListPanel()
    {
        checkListPanel.DOAnchorPos(new Vector2(400,0),0.5f);
    }
}
