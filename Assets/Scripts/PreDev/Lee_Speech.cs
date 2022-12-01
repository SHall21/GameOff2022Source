using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Lee_Speech : MonoBehaviour
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

    public void StartSpeech(Lee_HamBroState state)
    {
        if (state == null)
        {
            ClearSpeech();
            return;
        }

        speechImage.enabled = true;
        speechText.enabled = true;

        hamBro.sprite = state.image;
        speechImage.sprite = state.bubble;
        speechText.text = state.message;
    }

    public void ChangeBrother(Lee_HamBroState state)
    {
        if (state == null)
        {
            return;
        }

        hamBro.sprite = state.image;
    }
}
