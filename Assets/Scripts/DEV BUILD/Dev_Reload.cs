using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_Reload : MonoBehaviour
{
        Dev_LevelManager levelManager;
        Dev_SceneSession sceneSession;

    void Awake() {
        levelManager = FindObjectOfType<Dev_LevelManager>();
        sceneSession = FindObjectOfType<Dev_SceneSession>();

    }

    public void LoadPreviousGame()
    {
        levelManager.LoadScene(sceneSession.m_SceneName);
    }
}
