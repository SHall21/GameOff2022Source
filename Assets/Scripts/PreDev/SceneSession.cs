using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSession : MonoBehaviour
{
    void Awake()
    {
        // A singleton pattern - when its loaded again, destroy the new one and keep the old one
        int numberOfScenePersists = FindObjectsOfType<SceneSession>().Length;
        if (numberOfScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetSceneSession()
    {
        Destroy(gameObject);
    }
}
