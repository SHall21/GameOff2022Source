using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class Dev_UITimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] Image uiClockImage;
    [SerializeField] Image uiFillImage;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI failText;
    [SerializeField] Dev_TimeData completedTime1SO;
    [SerializeField] Dev_TimeData completedTime2SO;
    [SerializeField] Dev_TimeData completedTime3SO;
    [SerializeField] Dev_TimeData completedTime4SO;
    [SerializeField] Dev_TimeData completedTimeBest1SO;
    [SerializeField] Dev_TimeData completedTimeBest2SO;
    [SerializeField] Dev_TimeData completedTimeBest3SO;
    [SerializeField] Dev_TimeData completedTimeBest4SO;
    [SerializeField] Sprite trophyBronze, trophySilver, trophyGold;
    public int duration {get; private set;}
    public int remainingTime {get; private set;}
    public bool isFirst;
    public int startingTime = 120;
    private bool flashRoutine;
    private bool failRoutine;
    private bool settingComplete;
    Dev_SceneSession sceneSession;
    Dev_LevelManager levelManager;

    void Awake()
    {
        if (completedTime1SO == null)
        {
            Debug.Log($"Set {completedTime1SO} in inspector for {this.name}");
        }

        if (completedTime2SO == null)
        {
            Debug.Log($"Set {completedTime2SO} in inspector for {this.name}");
        }

        if (completedTime3SO == null)
        {
            Debug.Log($"Set {completedTime3SO} in inspector for {this.name}");
        }

        if (completedTime4SO == null)
        {
            Debug.Log($"Set {completedTime4SO} in inspector for {this.name}");
        }

        if (completedTimeBest1SO == null)
        {
            Debug.Log($"Set {completedTimeBest1SO} in inspector for {this.name}");
        }

        if (completedTimeBest2SO == null)
        {
            Debug.Log($"Set {completedTimeBest2SO} in inspector for {this.name}");
        }

        if (completedTimeBest3SO == null)
        {
            Debug.Log($"Set {completedTimeBest3SO} in inspector for {this.name}");
        }

        if (completedTimeBest4SO == null)
        {
            Debug.Log($"Set {completedTimeBest4SO} in inspector for {this.name}");
        }

        settingComplete = false;
        isFirst = true;
        sceneSession = FindObjectOfType<Dev_SceneSession>();
        levelManager = FindObjectOfType<Dev_LevelManager>();
        ResetTimer(); 
    }

    private void Start()
    {
        sceneSession.startTime = startingTime;
        sceneSession.SetLevelName();
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
        if (!settingComplete)
        {
            settingComplete = true;
            //sceneSession.completedTime = remainingTime;
            //sceneSession.SetCompletedTime(remainingTime);
            StopAllCoroutines();
            SetBestTrophy();
            SetTimeTrophy();
        }
    }

    private void SetBestTrophy()
    {
        var scene = sceneSession.m_SceneName;
        switch (scene)
        {
            case "Tutorial_Level":
                SetBestTroph(completedTimeBest1SO);
                break;
            case "Level_2":
                SetBestTroph(completedTimeBest2SO);
                break;
            case "Level_3":
                SetBestTroph(completedTimeBest3SO);
                break;
            case "Level_4":
                SetBestTroph(completedTimeBest4SO);
                break;
            default:
                break;
        }
    }

    private void SetTimeTrophy()
    {
        var scene = sceneSession.m_SceneName;
        switch (scene)
        {
            case "Tutorial_Level":
                SetTroph(completedTime1SO);
                break;
            case "Level_2":
                SetTroph(completedTime2SO);
                break;
            case "Level_3":
                SetTroph(completedTime3SO);
                break;
            case "Level_4":
                SetTroph(completedTime4SO);
                break;
            default:
                break;
        }
    }

    private void SetTroph(Dev_TimeData data)
    {
        data.Value = remainingTime;
        data.isLevelComplete = true;
        Debug.Log($"Setting SO trophy value");
        var sprite = SetTrophy();
        data.trophy = sprite;
        sceneSession.isSuccess = true;
    }

    private void SetBestTroph(Dev_TimeData data)
    {
        if (data.isLevelComplete)
        {
            if (data.Value < remainingTime)
            {
                data.Value = remainingTime;
                data.isLevelComplete = true;
                var sprite = SetTrophy();
                data.trophy = sprite;
            }
        }
        else
        {
            data.Value = remainingTime;
            data.isLevelComplete = true;
            var sprite = SetTrophy();
            data.trophy = sprite;
        }
    }

    private Sprite SetTrophy()
    {
        float percentageLeft = (float)((float)remainingTime / (float)startingTime) * 100;
        Debug.Log($"SetTrophy.... percentageLeft {percentageLeft}");
        if (percentageLeft  >= 60 && percentageLeft  <= 100)
        {
            return trophyGold;
        } else if (percentageLeft  >= 31 && percentageLeft  <= 59)
        {
            return trophySilver;
        } else {
            return trophyBronze;
        }
        // FIX here
    }

    public void SetTimer(int seconds)
    {
        UpdateUI(seconds);
    }

    public void ResetTimer()
    {
        sceneSession.isSuccess = false;
        timerText.text = "00:00";
        //failText.enabled = false;
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

    // Fix here - look at fail UI
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
