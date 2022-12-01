using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lee_AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip chatterClip, chompClip, clap_1Clip, clap_2Clip, dropOffClip, spottedClip;
    [SerializeField] [Range(0f, 1f)] float chatterVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float chompVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float clap_1Volume = 1f;
    [SerializeField] [Range(0f, 1f)] float clap_2Volume = 1f;
    [SerializeField] [Range(0f, 1f)] float dropOffVolume = 1f;
    [SerializeField] [Range(0f, 1f)] float spottedVolume = 1f;
    AudioSource m_MyAudioSource;

    private void Awake()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
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
        PlayClip(dropOffClip, dropOffVolume);
    }

    public void SpottedClip()
    {
        PlayClip(spottedClip, spottedVolume);
    }

    public void StopAudio()
    {
        m_MyAudioSource.Stop();
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
