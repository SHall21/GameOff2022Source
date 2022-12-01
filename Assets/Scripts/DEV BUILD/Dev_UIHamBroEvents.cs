using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dev_UIHamBroEvents : MonoBehaviour
{
    [Header("Speech Images")]
    [SerializeField] Sprite hamWorriedImage;
    [SerializeField] Sprite hamAngryImage;
    [SerializeField] Sprite hamExcitedImage;
    [SerializeField] Sprite hamNeutralImage;
    [SerializeField] Sprite hamDisappointedImage;
    [SerializeField] Sprite normalBubble;
    [SerializeField] Sprite excitedBubble;

    [Header("Speech Times")]
    [SerializeField] float minLeftMessageSeconds = 4f;
    [SerializeField] float wrongItemSeconds = 4f;
    [SerializeField] float correctItemSeconds = 4f;
    [SerializeField] float someTimeSeconds = 4f;
    [SerializeField] float controlsSeconds = 4f;
    [SerializeField] float completedSeconds = 2f;
    [SerializeField] float dropOffItemSeconds = 4f;
    [SerializeField] float spottedSeconds = 4f;
    [SerializeField] float introSeconds = 4f;

    [Header("Speech Messages")]
    private string minLeftMessage = "Only a minute left, get a boogy on!";
    private string wrongItemMessage = "We don't need that";
    private string correctItemMessage = "Nice one, you got just what we needed";
    private string someTimeMessage = "Only 30 seconds left, the fuzz are coming!";
    private string controlsMessage = "You gotta press E to stuff yer cheeks";
    private string completedMessage = "Victory!  Now let's get outta here!";
    private string dropOffItemMessage = "Ca-ching!  It's safe with me";
    private string spottedMessage = "Don't get spotted!";
    //private string noMovementMessage = "Why haven't you done anything, get yer squeek on!";

    [Header("Other")]
    Dev_Inventory inventory;
    Dev_AudioPlayer audioPlayer;
    Dev_Speech speech;
    Dev_UITimer timer;
    int currentTime;
    private bool isBeginning;
    private bool minLeft;
    private bool timeLeft;
    private bool correctItem;
    private bool wrongItem;
    private bool droppedOff;
    private bool controlExplain;
    public bool completedLevel;
    public bool tutorialLevel;
    private bool spotted;

    void Awake()
    {
        inventory = FindObjectOfType<Dev_Inventory>();
        audioPlayer = FindObjectOfType<Dev_AudioPlayer>();
        speech = FindObjectOfType<Dev_Speech>();
        timer = FindObjectOfType<Dev_UITimer>();
        speech.ClearSpeech();
        isBeginning = true;
    }

    void Start()
    {
        StartCoroutine(IntroSpeech());
    }

    void Update()
    {
        currentTime = timer.remainingTime;

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

        if (currentTime > 60 && !minLeft && !correctItem && !wrongItem && !timeLeft && !completedLevel && !droppedOff) {
            speech.ChangeBrother(hamNeutralImage);
        }

        if (currentTime < 60 && !minLeft && !correctItem && !wrongItem && !timeLeft && !completedLevel && !droppedOff) {
            speech.ChangeBrother(hamWorriedImage);
        }
    }

    public void TriggerCompletedSpeech()
    {   
        completedLevel = true;
        StartCoroutine(CompletedSpeech());
    }

    private void OnEnable()
    {
        Dev_Pickup.OnPickupCollected += CheckItemSpeech;
    }

    private void OnDisable()
    {
        Dev_Pickup.OnPickupCollected -= CheckItemSpeech;
    }

    public void DropOffSpeech() {
        droppedOff = true;
        StartCoroutine(CollectedSpeech());
    }

    public void ControlsSpeech() {
        if (tutorialLevel)
        {
            controlExplain = true;
            StartCoroutine(ControlExplainSpeech());
        }
    }

    public void StartSpotSpeech() {
        spotted = true;
        StartCoroutine(SpottedSpeech());
    }

    private IEnumerator ControlExplainSpeech()
    {
        if(controlExplain) {
            speech.StartSpeech(hamExcitedImage,normalBubble,controlsMessage);
            yield return new WaitForSecondsRealtime(controlsSeconds);

            speech.ClearSpeech();
            controlExplain = false;
        }
    }

    private void CheckItemSpeech(Dev_ItemData itemData)
    {
        if (inventory.IsOnList(itemData.id))
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
        string startSpeechMessage = string.Format("You have {0} minutes on the clock, and then we have gotta cheese it",timer.startingTime/60);
        if (isBeginning) {
            audioPlayer.ChatterClip();
            speech.StartSpeech(hamNeutralImage, normalBubble, startSpeechMessage);
            yield return new WaitForSecondsRealtime(introSeconds);
        }

        speech.ClearSpeech();
        timer.TriggerAlarm();
        isBeginning = false;
    }

    private IEnumerator MinuteLeftSpeech()
    {
        if(minLeft) {
            speech.StartSpeech(hamWorriedImage,excitedBubble,minLeftMessage);
            yield return new WaitForSecondsRealtime(minLeftMessageSeconds);

            speech.ClearSpeech();
            minLeft = false;
        }
    }

    private IEnumerator TimeLeftSpeech()
    {
        if(timeLeft) {
            speech.StartSpeech(hamAngryImage,excitedBubble,someTimeMessage);
            yield return new WaitForSecondsRealtime(someTimeSeconds);

            speech.ClearSpeech();
            timeLeft = false;
        }
    }

    private IEnumerator CollectedSpeech()
    {
        if(droppedOff) {
            speech.StartSpeech(hamExcitedImage,normalBubble,dropOffItemMessage);
            yield return new WaitForSecondsRealtime(dropOffItemSeconds);

            speech.ClearSpeech();
            droppedOff = false;
        }
    }

    private IEnumerator CompletedSpeech()
    {

        if(completedLevel) {
        speech.StartSpeech(hamExcitedImage,normalBubble,completedMessage);
        yield return new WaitForSecondsRealtime(completedSeconds);

        speech.ClearSpeech();
        completedLevel = false;
        }
    }

    private IEnumerator CorrectSpeech()
    {
        if(correctItem) {
            speech.StartSpeech(hamExcitedImage,normalBubble,correctItemMessage);
            yield return new WaitForSecondsRealtime(correctItemSeconds);

            speech.ClearSpeech();
            correctItem = false;
        }
    }

    private IEnumerator WrongSpeech()
    {
        if(wrongItem) {
            timer.WrongItem();
            speech.StartSpeech(hamDisappointedImage,normalBubble,wrongItemMessage);
            yield return new WaitForSecondsRealtime(wrongItemSeconds);

            speech.ClearSpeech();
            wrongItem = false;
        }
    }

    private IEnumerator SpottedSpeech()
    {
        if(spotted) {
            speech.StartSpeech(hamDisappointedImage,normalBubble,spottedMessage);
            yield return new WaitForSecondsRealtime(spottedSeconds);

            speech.ClearSpeech();
            spotted = false;
        }
    }
}
