using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lee_ItemDeposit : MonoBehaviour
{
    Lee_Inventory inventory;
    int score;
    Lee_ScoreKeeper scoreKeeper;
    Lee_AudioPlayer audioPlayer;
    Lee_LevelManager levelManager;
    Lee_UITimer timer;

    void Awake()
    {
        inventory = FindObjectOfType<Lee_Inventory>();
        scoreKeeper = FindObjectOfType<Lee_ScoreKeeper>();
        audioPlayer = FindObjectOfType<Lee_AudioPlayer>();
        levelManager = FindObjectOfType<Lee_LevelManager>();
        timer = FindObjectOfType<Lee_UITimer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && inventory.inventory.Count > 0)
        {
            AddStolenItems(inventory.inventory[0].itemData);
            audioPlayer.DropOffClip();
            inventory.RemoveInventory();
        }

        if (scoreKeeper.CheckCompleted())
        {
            scoreKeeper.completedTime = timer.remainingTime;
            levelManager.LoadGameOver();
        }
    }

    public void AddStolenItems(Lee_ItemData itemData)
    {
        Lee_InventoryItem newItem = new Lee_InventoryItem(itemData);
        scoreKeeper.StolenItems.Add(newItem);

        if (scoreKeeper.IsOnList(itemData.id))
        {
            Debug.Log("marking item on list");
            scoreKeeper.ShoppingList.Remove(itemData.id);
        }
    }
}
