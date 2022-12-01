using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_UIAudio : MonoBehaviour
{
    [SerializeField] AudioClip hoverClip, clickClip;
    [SerializeField] [Range(0f, 1f)] float hoverVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float clickVolume = 1f;

    public void HoverClip()
    {
        PlayClip(hoverClip, hoverVolume);
    }

    public void ClickClip()
    {
        PlayClip(clickClip, clickVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector3 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
