using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dev_UITrophy : MonoBehaviour
{
    [SerializeField] Image trophy1Image;
    [SerializeField] TextMeshProUGUI trophy1Text;
    private int display1Time;
    [SerializeField] Image trophy2Image;
    [SerializeField] TextMeshProUGUI trophy2Text;
    private int display2Time;
    [SerializeField] Sprite trophyBronze, trophySilver, trophyGold;
    Dev_SceneSession sceneSession;


    void Awake()
    {
        sceneSession = FindObjectOfType<Dev_SceneSession>();
        ClearTrophy();
    }

    public void ClearTrophy()
    {
        trophy1Image.enabled = false;
        trophy1Text.enabled = false;
        trophy2Image.enabled = false;
        trophy2Text.enabled = false;
    }

    private void CheckLevel1()
    {
        if (sceneSession.level1Complete)
        {
            display1Time = sceneSession.tutorialLevelSO.Value;

            trophy1Image.enabled = true;
            trophy1Text.enabled = true;
            trophy1Text.text = string.Format("{0}:{1:D2}",display1Time/60, display1Time%60);
            SetTrophy(trophy1Image, display1Time);
        }
    }

    private void CheckLevel2()
    {
        if (sceneSession.level2Complete)
        {
            display1Time = sceneSession.level2SO.Value;

            trophy2Image.enabled = true;
            trophy2Text.enabled = true;
            trophy2Text.text = string.Format("{0}:{1:D2}",display2Time/60, display2Time%60);
            SetTrophy(trophy2Image, display2Time);
        }
    }

    private void SetTrophy(Image trophyImage, int displayTime)
    {
        int percentageLeft = (int)((double)(sceneSession.startTime - displayTime) / displayTime * 100);

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
