using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_Scorekeeper : MonoBehaviour
{
    public bool isFirst;
    int score;
    float startingTime = 300.0f;
    static Dev_Scorekeeper instance;

    void Awake()
    {
        isFirst = true;
        ManageSingleton();
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

    public float GetTime()
    {
        if (!isFirst)
        {
            if (startingTime <= 0.0f)
            {
                startingTime = 0f;
            }
            else
            {
                startingTime -= Time.deltaTime;
            }
        }

        return startingTime;
    }

    public void TriggerAlarm()
    {
        if (isFirst)
        {
            //Debug.Log($"isFirst is currently at: {isFirst}");
            isFirst = false;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void ModifyScore(int value)
    {
        score += value;
        Mathf.Clamp(score, 0, int.MaxValue);
    }

    public void ResetScore()
    {
        score = 0;
    }
}
