using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Dev_SceneSession : MonoBehaviour
{
    public int startTime;
    public int completedTime;
    static Dev_SceneSession instance;
    public bool isSuccess;
    Scene m_Scene;
    public string m_SceneName;
    [SerializeField] public Dev_TimeData tutorialLevelSO;
    [SerializeField] public Dev_TimeData level2SO;
    public bool level1Complete;
    public bool level2Complete;

    void Awake()
    {
        ManageSingleton();

        m_Scene = SceneManager.GetActiveScene();
    }

    void Start()
    {
        if (m_Scene.name.Contains("Level")) {
            m_SceneName = m_Scene.name;
        }

        if (tutorialLevelSO.name.Contains(m_SceneName))
        {
            Debug.Log("SO matching level");
            completedTime = tutorialLevelSO.Value;
        } else if (level2SO.name.Contains(m_SceneName))
        {
            Debug.Log("SO matching level");
            completedTime = level2SO.Value;
        }
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

    public void SetLevel1Complete()
    {
        level1Complete = true;
    }

    public void SetLevel2Complete()
    {
        level2Complete = true;
    }
}
