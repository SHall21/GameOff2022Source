using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Dev_SceneSession : MonoBehaviour
{
    public int startTime;
    static Dev_SceneSession instance;
    public bool isSuccess;
    Scene m_Scene;
    public string m_SceneName;
    public bool level1Complete;
    public bool level2Complete;
    public bool level3Complete;
    public bool level4Complete;

    void Awake()
    {
        ManageSingleton();

        m_Scene = SceneManager.GetActiveScene();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetLevelName()
    {
        m_Scene = SceneManager.GetActiveScene();
        
        if (m_Scene.name.Contains("Level")) {
            m_SceneName = m_Scene.name;
            Debug.Log($"setting scene name {m_SceneName}");
        }
    }

    public void ClearLevelName()
    {
        m_SceneName = string.Empty;
    }

    public void SetLevel1Complete()
    {
        level1Complete = true;
    }

    public void SetLevel2Complete()
    {
        level2Complete = true;
    }

    public void SetLevel3Complete()
    {
        level3Complete = true;
    }

    public void SetLevel4Complete()
    {
        level4Complete = true;
    }
}
