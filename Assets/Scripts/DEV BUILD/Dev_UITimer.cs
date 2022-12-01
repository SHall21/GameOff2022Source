using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class Dev_UITimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] Image uiClockImage;
    [SerializeField] Image uiFillImage;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI failText;
    [SerializeField] Dev_TimeData completedTimeSO;
    public int duration {get; private set;}
    public int remainingTime {get; private set;}
    public bool isFirst;
    public int startingTime = 120;
    private bool flashRoutine;
    private bool failRoutine;
    Dev_SceneSession sceneSession;
    Dev_LevelManager levelManager;

    void Awake()
    {
        ResetTimer();
        isFirst = true;
        sceneSession = FindObjectOfType<Dev_SceneSession>();
        levelManager = FindObjectOfType<Dev_LevelManager>();
    }

    private void Start()
    {
        sceneSession.startTime = startingTime;
        SetDuration(startingTime);
        SetTimer(startingTime);
    }

    void Update()
    {
        if (!flashRoutine)
        {
            StopCoroutine(FlashUI());
            StopCoroutine(UpdateTimerTextUI());
        }

        if (!failRoutine)
        {
            StopCoroutine(UpdateFailText(10));
        }
    }

    public void TriggerAlarm()
    {
        if (isFirst) {
            isFirst = false;
            Begin();
        }
    }

    public void SetCompletedTime()
    {
        //sceneSession.completedTime = remainingTime;
        //sceneSession.SetCompletedTime(remainingTime);
        completedTimeSO.Value = remainingTime;
        sceneSession.isSuccess = true;
    }

    public void SetTimer(int seconds)
    {
        UpdateUI(seconds);
    }

    public void ResetTimer()
    {
        timerText.text = "00:00";
        failText.enabled = false;
        uiFillImage.fillAmount = 0f;

        duration = remainingTime = 0;
    }

    private Dev_UITimer SetDuration(int seconds)
    {
        duration = remainingTime = seconds;
        return this;
    }

    private void Begin ()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    public void StopTimer()
    {
        StopCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remainingTime > 0) {
            UpdateUI(remainingTime);
            remainingTime--;
            yield return new WaitForSeconds(1f);
        }
        End();
    }

    public void WrongItem()
    {
        flashRoutine = true;
        failRoutine = true;
        remainingTime -= 10;
        StartCoroutine(FlashUI());
        StartCoroutine(UpdateTimerTextUI());
        StartCoroutine(UpdateFailText(10));
    }

    public void InViewOfLight()
    {
        flashRoutine = true;
        failRoutine = true;
        remainingTime -= 10;
        //Debug.Log($"Removing time, remaining now: {remainingTime}");
        StartCoroutine(FlashUI());
        StartCoroutine(UpdateTimerTextUI());
        StartCoroutine(UpdateFailText(10));
    }

    public void OneMinute()
    {
        flashRoutine = true;
        StartCoroutine(FlashUI());
        StartCoroutine(UpdateTimerTextUI());
    }

    private IEnumerator UpdateFailText(int seconds)
    {
        while (failRoutine) {
            failText.DOFade(255,1);
            failText.DOFontSize(50,1).SetEase(Ease.InFlash);
            failText.text = string.Format("-{0}",seconds);

            yield return new WaitForSeconds(1);

            failText.DOFade(0,2);
            failRoutine = false;
        }
    }

    private IEnumerator UpdateTimerTextUI ()
    {
        while (flashRoutine) {
            timerText.DOColor(Color.red,1);

            yield return new WaitForSeconds(2);

            timerText.DOColor(Color.white,1);
            flashRoutine = false;
        }
    }

    private IEnumerator FlashUI()
    {
        while (flashRoutine) {
            uiClockImage.DOColor(Color.red,1);
            uiFillImage.DOColor(Color.red,1);

            yield return new WaitForSeconds(2);

            uiClockImage.DOColor(Color.white,1);
            uiFillImage.DOColor(Color.white,1);

            flashRoutine = false;
        }
    }

    private void UpdateUI(int seconds)
    {
        timerText.text = string.Format("{0}:{1:D2}",seconds/60, seconds%60);
        uiFillImage.fillAmount = Mathf.InverseLerp(0,duration,seconds);
    }

    public void End() {
        levelManager.LoadGameOver();
    }

    private void OnDestroy() {
        StopAllCoroutines();
        DOTween.Kill(this.gameObject);
    }
}
