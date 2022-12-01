using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lee_LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 2f;
    Lee_AudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = FindObjectOfType<Lee_AudioPlayer>();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Lee_Prototype_THICC");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Lee_Prototype_THICC");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Lee_MainMenu");
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad("Lee_GameOver", sceneLoadDelay));
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game..");
        Application.Quit();
    }

    IEnumerator WaitAndLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
