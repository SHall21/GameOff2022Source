using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Dev_UIIntro : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skipText;
    [SerializeField] private CanvasGroup fadingCanvasGroup;
    [SerializeField] Image speechImage;
    [SerializeField] TextMeshProUGUI speechText;
    private bool line1 = true;
    private bool line2;
    private bool line3;
    private bool line4;
    private bool fadeOut;
    private string line1Message = "Heya boys, I got a job for ya that will blow the wheel right off ya cage...";
    private string line2Message = "It may look easy, but make sure ya don't get too big for yer boots see";
    private string line3Message = "This ain't yer typical loot, there's a bunch o high quality tat we need for our novelty black market store.";
    private string line4Message = "So don't half cheek it!";

    void Awake()
    {
        skipText.text = "Press any key to skip";
    }

    public void Any(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(NextLevel());
        }
    }

    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(fadingCanvasGroup.DOFade(0, 2));

        speechImage.enabled = true;
        speechText.enabled = true;
        //speechText.alpha = 0;
    }

    void Update()
    {
        if(line1) {
            StartCoroutine(SpeechSeq1());
        } else {
            StopCoroutine(SpeechSeq1());
        }

        if(line2) {
            StartCoroutine(SpeechSeq2());
        } else {
            StopCoroutine(SpeechSeq2());
        }

        if(line3) {
            StartCoroutine(SpeechSeq3());
        } else {
            StopCoroutine(SpeechSeq3());
        }

        if(line4) {
            StartCoroutine(SpeechSeq4());
        } else {
            StopCoroutine(SpeechSeq4());
        }

        if(fadeOut)
        {
            FadeOut();
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(2f);
        Scene scene = SceneManager.GetActiveScene();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FadeOut();
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator SpeechSeq1()
    {
        speechText.text = line1Message;
        yield return new WaitForSecondsRealtime(5);

        line1 = false;
        line2 = true;
    }

    private IEnumerator SpeechSeq2()
    {
        speechText.text = line2Message;
        yield return new WaitForSecondsRealtime(5);
        line2 = false;
        line3 = true;
    }

    private IEnumerator SpeechSeq3()
    {
        speechText.text = line3Message;
        yield return new WaitForSecondsRealtime(5);

        line3 = false;
        line4 = true;
    }

    private IEnumerator SpeechSeq4()
    {
        speechText.text = line4Message;
        yield return new WaitForSecondsRealtime(5);

        line4 = false;
        fadeOut = true;
    }

    private void FadeOut()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(fadingCanvasGroup.DOFade(1, 2));
    }
}
