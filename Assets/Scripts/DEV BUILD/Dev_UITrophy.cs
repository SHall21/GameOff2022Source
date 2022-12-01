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
    [SerializeField] public Dev_TimeData tutorialLevelSO;
    [SerializeField] public Dev_TimeData level2SO;
    [SerializeField] public Dev_TimeData level3SO;
    [SerializeField] public Dev_TimeData level4SO;


    void Awake()
    {
        sceneSession = FindObjectOfType<Dev_SceneSession>();
    }

    private void Start() {
        ClearTrophy();
        CheckLevel1();
        CheckLevel2();
        CheckLevel3();
        CheckLevel4();
    }

    private void ClearTrophy()
    {
        trophy1Image.enabled = false;
        trophy1Text.enabled = false;
        trophy2Image.enabled = false;
        trophy2Text.enabled = false;
    }

    private void CheckLevel1()
    {
        Debug.Log("Checking...");
        if (tutorialLevelSO.isLevelComplete)
        {
            Debug.Log("Complete!!");
            display1Time = tutorialLevelSO.Value;

            trophy1Image.enabled = true;
            trophy1Text.enabled = true;
            trophy1Text.text = string.Format("{0}:{1:D2}",display1Time/60, display1Time%60);
            trophy1Image.sprite = tutorialLevelSO.trophy;
        }
    }

    private void CheckLevel2()
    {
        if (level2SO.isLevelComplete)
        {
            display2Time = level2SO.Value;

            trophy2Image.enabled = true;
            trophy2Text.enabled = true;
            trophy2Text.text = string.Format("{0}:{1:D2}",display2Time/60, display2Time%60);
            trophy2Image.sprite = level2SO.trophy;
        }
    }

    private void CheckLevel3()
    {
        if (level3SO.isLevelComplete)
        {
            display2Time = level3SO.Value;

            trophy2Image.enabled = true;
            trophy2Text.enabled = true;
            trophy2Text.text = string.Format("{0}:{1:D2}", display2Time / 60, display2Time % 60);
            trophy2Image.sprite = level3SO.trophy;
        }
    }

    private void CheckLevel4()
    {
        if (level4SO.isLevelComplete)
        {
            display2Time = level4SO.Value;

            trophy2Image.enabled = true;
            trophy2Text.enabled = true;
            trophy2Text.text = string.Format("{0}:{1:D2}", display2Time / 60, display2Time % 60);
            trophy2Image.sprite = level4SO.trophy;
        }
    }
}
