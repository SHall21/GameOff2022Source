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
    Dev_SceneSession sceneSession;
    private int displayTime;

    void Awake()
    {
        sceneSession = FindObjectOfType<Dev_SceneSession>();
        displayTime = sceneSession.completedTime;
    }

    void Start()
    {
        SetHeaderText();
        SetSubText();
        SetTrophyTime();
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

            SetTrophy();
            timeText.text = string.Format("{0}:{1:D2}",displayTime/60, displayTime%60);
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
