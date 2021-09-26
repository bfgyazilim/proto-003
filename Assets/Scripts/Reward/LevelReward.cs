using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelReward : MonoBehaviour
{
    public string url = "www.google.com";

    public string urlDate = "http://worldclockapi.com/api/json/est/now";

    public string sDate = "";

    //public Text coinText;
    public TextMeshProUGUI coinText;
    public GameObject levelRewardPanel;

    public bool delete;

    private void Start()
    {
        if(delete)
        {
            PlayerPrefs.DeleteAll();
        }

        // Check at start if how many coins we have before
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();

    }

    public void Reward(int count)
    {
        // Increment coins
        int coin = PlayerPrefs.GetInt("Coin");
        coin += count;
        // Save to prefs
        PlayerPrefs.SetInt("Coin", coin);
        // Show in text total
        coinText.text = coin.ToString();
        Debug.Log("Coins:" + coin);

        // After getting reward, set passive come another day for next button active...
        Button clickButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        clickButton.interactable = false;

        // Reward given, close the panel and continue to the game...
        ClosePanel();
    }

    public void LevelPanel()
    {
        levelRewardPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        levelRewardPanel.SetActive(false);
    }

    public void CloseButton()
    {
        levelRewardPanel.SetActive(false);
    }

}
