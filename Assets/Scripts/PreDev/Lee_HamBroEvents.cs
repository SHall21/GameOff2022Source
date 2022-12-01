using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lee_HamBroEvents : MonoBehaviour
{
    Lee_ScoreKeeper scoreKeeper;
    Lee_AudioPlayer audioPlayer;
    int currentTime;
    [SerializeField] List<Lee_HamBroState> SOState;
    [SerializeField] List<string> ShoppingList;
    [SerializeField] Lee_Speech speech;
    [SerializeField] Lee_UITimer timer;
    private bool isBeginning;
    private bool minLeft;
    private bool timeLeft;
    private bool correctItem;
    private bool wrongItem;

    void Awake()
    {
        scoreKeeper = FindObjectOfType<Lee_ScoreKeeper>();
        audioPlayer = FindObjectOfType<Lee_AudioPlayer>();
        speech = FindObjectOfType<Lee_Speech>();
        speech.ClearSpeech();
        isBeginning = true;
    }

    void Start()
    {
        StartCoroutine(IntroSpeech());
    }

    void Update()
    {
        currentTime = timer.GetTime();

        if  (currentTime == 60)
        {
            minLeft = true;
            StartCoroutine(MinuteLeftSpeech());
            timer.OneMinute();
        }

        if (!minLeft) {
            StopCoroutine(MinuteLeftSpeech());
        }

        if  (currentTime == 30)
        {
            timeLeft = true;
            StartCoroutine(TimeLeftSpeech());
            timer.OneMinute();
        }

        if (!timeLeft) {
            StopCoroutine(TimeLeftSpeech());
        }

        if (currentTime < 60 && !minLeft && !correctItem && !wrongItem) {
            speech.ChangeBrother(SOState[3]);
        }
    }

    private void OnEnable()
    {
        Lee_Pickup.OnPickupCollected += CheckItemSpeech;
    }

    private void OnDisable()
    {
        Lee_Pickup.OnPickupCollected -= CheckItemSpeech;
    }

    private void CheckItemSpeech(Lee_ItemData itemData)
    {
        if (scoreKeeper.IsOnList(itemData.id))
        {
            correctItem = true;
            StartCoroutine(CorrectSpeech());

            if (!correctItem) {
                StopCoroutine(CorrectSpeech());
            }
        } else {
            wrongItem = true;
            StartCoroutine(WrongSpeech());

            if (!wrongItem) {
                StopCoroutine(WrongSpeech());
            }
        }
    }

    private IEnumerator IntroSpeech()
    {
        if (isBeginning) {
            audioPlayer.ChatterClip();
            speech.StartSpeech(SOState[0]);
            yield return new WaitForSecondsRealtime(3);
        }

        speech.ClearSpeech();
        timer.TriggerAlarm();
        isBeginning = false;
    }

    private IEnumerator MinuteLeftSpeech()
    {
        if(minLeft) {
            audioPlayer.ChatterClip();
            speech.StartSpeech(SOState[3]);
            yield return new WaitForSecondsRealtime(3);

            speech.ClearSpeech();
            minLeft = false;
        }
    }

    private IEnumerator TimeLeftSpeech()
    {
        if(timeLeft) {
            audioPlayer.ChatterClip();
            speech.StartSpeech(SOState[5]);
            yield return new WaitForSecondsRealtime(3);

            speech.ClearSpeech();
            timeLeft = false;
        }
    }

    private IEnumerator CorrectSpeech()
    {
        if(correctItem) {
            audioPlayer.ChatterClip();
            speech.StartSpeech(SOState[2]);
            yield return new WaitForSecondsRealtime(3);

            speech.ClearSpeech();
            correctItem = false;
        }
    }

    private IEnumerator WrongSpeech()
    {
        if(wrongItem) {
            timer.WrongItem();
            audioPlayer.ChatterClip();
            speech.StartSpeech(SOState[1]);
            yield return new WaitForSecondsRealtime(3);

            speech.ClearSpeech();
            wrongItem = false;
        }
    }
}
