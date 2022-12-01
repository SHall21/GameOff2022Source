using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lee_GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image trophyImage;
    [SerializeField] Sprite trophyBronze, trophySilver, trophyGold;
    Lee_ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<Lee_ScoreKeeper>();
    }

    void Start()
    {
        SetHeaderText();
        SetSubText();
        SetTrophyTime();
    }

    private void SetHeaderText()
    {
        if (scoreKeeper.isSuccess) {
            headerText.text = "Level Completed";
        } else {
            headerText.text = "Game Over";
        }
    }

    private void SetSubText()
    {
        if (scoreKeeper.isSuccess) {
            subText.text = "Got the goods in record time";
        } else {
            subText.text = "Bummer, you got caught by the fuzz";
        }
    }

    private void SetTrophyTime()
    {
        if (scoreKeeper.isSuccess) {
            trophyImage.enabled = true;
            timeText.enabled = true;

            SetTrophy();
            timeText.text = string.Format("{0}:{1:D2}",scoreKeeper.completedTime/60, scoreKeeper.completedTime%60);
        } else {
            trophyImage.enabled = false;
            timeText.enabled = false;
        }
    }

    private void SetTrophy()
    {
        int percentageLeft = (int)((double)(scoreKeeper.startTime - scoreKeeper.completedTime) / scoreKeeper.completedTime * 100);
        Debug.Log($"startTime {scoreKeeper.startTime} and completed time: {scoreKeeper.completedTime} , so the percentage of completion is: {percentageLeft}");

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
