using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dev_Speech : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speechText;
    [SerializeField] Image speechImage;
    [SerializeField] Image hamBro;

    private void Awake() {
        if (hamBro == null) {
            Debug.Log($"{nameof(hamBro)} is null for this script: {this.name}");
        }
    }

    public void ClearSpeech()
    {
        speechImage.enabled = false;
        speechText.enabled = false;
    }

    public void StartSpeech(Sprite hamImage, Sprite bubble, string message)
    {
        if (hamImage == null)
        {
            Debug.Log("Ham image is missing");
            ClearSpeech();
            return;
        }

        speechImage.enabled = true;
        speechText.enabled = true;

        hamBro.sprite = hamImage;
        speechImage.sprite = bubble;
        speechText.text = message;
    }

    public void ChangeBrother(Sprite image)
    {
        if (image == null)
        {
            return;
        }

        hamBro.sprite = image;
    }
}
