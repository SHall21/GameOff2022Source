using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_AudioPlayer : MonoBehaviour
{
    Dev_Inventory inventory;
    [SerializeField] AudioClip chatterClip, chompClip, clap_1Clip, clap_2Clip, dropOffClip, spottedClip, wellDoneClip, pillarHitClip, pillarDownClip;
    [SerializeField] [Range(0f, 1f)] float chatterVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float chompVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float clap_1Volume = 1f;
    [SerializeField] [Range(0f, 1f)] float clap_2Volume = 1f;
    [SerializeField] [Range(0f, 1f)] float dropOffVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float spottedVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float wellDoneVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float pillarHitClipVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float pillarDownVolume = 1f;

    private void Awake()
    {
        inventory = FindObjectOfType<Dev_Inventory>();
    }

    public void ChatterClip()
    {
        PlayClip(chatterClip, chatterVolume);
    }

    public void ChompClip()
    {
        PlayClip(chompClip, chompVolume);
    }

    public void Clap1Clip()
    {
        PlayClip(clap_1Clip, clap_1Volume);
    }

    public void Clap2Clip()
    {
        PlayClip(clap_2Clip, clap_2Volume);
    }

    public void DropOffClip()
    {
        if (inventory.ShoppingList.Count == 0) {
            PlayClip(wellDoneClip, wellDoneVolume);
        } else {
            PlayClip(dropOffClip, dropOffVolume);
        }
    }

    public void SpottedClip()
    {
        PlayClip(spottedClip, spottedVolume);
    }

    public void WellDoneClip()
    {
        PlayClip(wellDoneClip, wellDoneVolume);
    }

    public void PillarHitClip()
    {
        PlayClip(pillarHitClip, pillarHitClipVolume);
    }

    public void PillarDownClip()
    {
        PlayClip(pillarDownClip, pillarDownVolume);
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
