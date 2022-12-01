using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dev_GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image trophyImage;
    [SerializeField] Sprite trophyBronze, trophySilver, trophyGold;
    [SerializeField] public Dev_TimeData tutorialLevelSO;
    [SerializeField] public Dev_TimeData level2SO;
    [SerializeField] public Dev_TimeData level3SO;
    [SerializeField] public Dev_TimeData level4SO;
    Dev_SceneSession sceneSession;
    private int displayTime;
    private Sprite displaySprite;

    void Awake()
    {
        sceneSession = FindObjectOfType<Dev_SceneSession>();
    }

    void Start()
    {
        if (sceneSession.isSuccess) {
            CheckLevel();
        }
        SetHeaderText();
        SetSubText();
        SetTrophyTime();
    }

    void CheckLevel()
    {
        if (tutorialLevelSO.id == sceneSession.m_SceneName)
        {
            displayTime = tutorialLevelSO.Value;
            displaySprite = tutorialLevelSO.trophy;

        }
        else if (level2SO.id.Contains(sceneSession.m_SceneName))
        {
            displayTime = level2SO.Value;
            displaySprite = level2SO.trophy;
        }
        else if (level3SO.id.Contains(sceneSession.m_SceneName))
        {
            displayTime = level3SO.Value;
            displaySprite = level3SO.trophy;
        }
        else if (level4SO.id.Contains(sceneSession.m_SceneName))
        {
            displayTime = level4SO.Value;
            displaySprite = level4SO.trophy;
        }
    }

    private void SetHeaderText()
    {
        if (sceneSession.isSuccess) {
            headerText.text = "Level Completed";
        } else {
            headerText.text = "Game Over";
        }
    }

    private void SetSubText()
    {
        if (sceneSession.isSuccess) {
            subText.text = "Got the goods in record time";
        } else {
            subText.text = "Bummer, you got caught by the fuzz";
        }
    }

    private void SetTrophyTime()
    {
        if (sceneSession.isSuccess) {
            trophyImage.enabled = true;
            timeText.enabled = true;

            //SetTrophy();
            timeText.text = string.Format("{0}:{1:D2}",displayTime/60, displayTime%60);
            trophyImage.sprite = displaySprite;
        } else {
            trophyImage.enabled = false;
            timeText.enabled = false;
        }
    }

    private void SetTrophy()
    {
        int percentageLeft = (int)((double)(sceneSession.startTime - displayTime) / displayTime * 100);
        Debug.Log($"startTime {sceneSession.startTime} and completed time: {displayTime} , so the percentage of completion is: {percentageLeft}");

        if (percentageLeft  >= 71 && percentageLeft  <= 100)
        {
            trophyImage.sprite = trophyBronze;
        }

        if (percentageLeft  >= 31 && percentageLeft  <= 70)
        {
            trophyImage.sprite = trophySilver;
        }

        if (percentageLeft  >= 1 && percentageLeft  <= 30)
        {
            trophyImage.sprite = trophyGold;
        }
    }
}
