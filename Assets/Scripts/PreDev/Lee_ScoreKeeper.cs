using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lee_ScoreKeeper : MonoBehaviour
{
    int score;
    public int startTime;
    public int completedTime;
    static Lee_ScoreKeeper instance;
    [SerializeField] public List<string> ShoppingList;
    public List<Lee_InventoryItem> StolenItems = new List<Lee_InventoryItem>();
    public bool isSuccess;

    void Awake()
    {
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

    public bool CheckCompleted()
    {
        if (ShoppingList.Count == 0) {
            isSuccess = true;
            return isSuccess;
        } else {
            return false;
        }
    }

    public bool IsOnList (string id)
    {
        if (ShoppingList.Contains(id))
        {
            return true;
        }
        else
        {
            return false;
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
