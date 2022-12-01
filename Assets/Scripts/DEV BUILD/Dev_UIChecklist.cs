using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Dev_UIChecklist : MonoBehaviour
{
    public RectTransform checkListPanel;
    public bool displayPanel;
    public bool displayChecklist;
    public bool closeChecklist;
    Vector2 moveInput;

    void Update()
    {
        ToggleChecklist();
    }

    public void CheckMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void ToggleChecklist()
    {
        if (Mathf.Abs(moveInput.x) > 0 || Mathf.Abs(moveInput.y) > 0)
        {
            CloseCheckListPanel();
        } else {
            OpenCheckListPanel();
        }
    }

    private IEnumerator OpenCheckList()
    {
        if (displayChecklist) {
            yield return new WaitForSeconds(1f);
            OpenCheckListPanel();
        }

        displayChecklist = false;
    }

    private IEnumerator CloseCheckList()
    {
        if (closeChecklist) {
            CloseCheckListPanel();
            yield return new WaitForSeconds(.5f);
        }

        closeChecklist = false;
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
        checkListPanel.DOAnchorPos(new Vector2(450,0),0.5f);
    }

    private void OnDestroy() {
        StopAllCoroutines();
        DOTween.Kill(this.gameObject);
    }
}
