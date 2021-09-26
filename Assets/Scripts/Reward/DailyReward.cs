using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DailyReward : MonoBehaviour
{
    public string url = "www.google.com";

    public string urlDate = "http://worldclockapi.com/api/json/est/now";

    public string sDate = "";

    //public Text coinText;
    public TextMeshProUGUI coinText;

    public List<int> rewardCoin;
    public List<Button> rewardButton;
    public Button dailyButton;
    public GameObject dailyPanel;

    public bool delete;

    private void Start()
    {
        if(delete)
        {
            PlayerPrefs.DeleteAll();
        }

        // Check at start if how many coins we have before
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();

        // If there is internet
        StartCoroutine(CheckInternet());

    }

    private IEnumerator CheckInternet()
    {
        WWW www = new WWW(url);
        yield return www;

        if(string.IsNullOrEmpty(www.text))
        {
            Debug.Log("Not connected to Internet");
        }
        else
        {
            Debug.Log("Successful Internet connection");
            StartCoroutine(CheckDate());
        }
    }

    private IEnumerator CheckDate()
    {
        WWW www = new WWW(urlDate);
        yield return www;

        string[] splitDate = www.text.Split(new string[] { "currentDateTime\":\"" }, System.StringSplitOptions.None);
        /*
        foreach (string item in splitDate)
        {
            Debug.Log(item);
        }
        */
        sDate = splitDate[1].Substring(0, 10);
        Debug.Log(sDate);
        dailyButton.interactable = true;
    }

    public void DailyCheck()
    {
        string dateOld = PlayerPrefs.GetString("PlayDateOld");

        if(string.IsNullOrEmpty(dateOld))
        {
            Debug.Log("First Game");
            Debug.Log("First reward");
            //Reward(0);
            rewardButton[0].interactable = true;
            PlayerPrefs.SetString("PlayDateOld", sDate);
            PlayerPrefs.SetInt("PlayGameCount", 1);
        }
        else
        {
            //DateTime _dateNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            // Prevents hacking of system dat-time so that we get the real time value form the internet site...
            DateTime _dateNow = Convert.ToDateTime(sDate);
            DateTime _dateOld = Convert.ToDateTime(dateOld);

            TimeSpan diff = _dateNow.Subtract(_dateOld);

            if(diff.Days >= 1 && diff.Days < 2)
            {
                Debug.Log("Other Days");
                rewardButton[1].interactable = true;
                //Reward(10);
                PlayerPrefs.SetString("PlayDateOld", _dateNow.ToString());
            }
            else if(diff.Days >= 2)
            {
                rewardButton[2].interactable = true;
                //Reward(20);
                PlayerPrefs.SetString("PlayDateOld", _dateNow.ToString());
            }
        }
    }

    public void Reward(int count)
    {
        int coin = PlayerPrefs.GetInt("Coin");
        coin += count;
        PlayerPrefs.SetInt("Coin", coin);
        coinText.text = coin.ToString();
        Debug.Log("Coins:" + coin);

        // After getting reward, set passive come another day for next button active...
        Button clickButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        clickButton.interactable = false;
        
    }

    public void DailyPanel()
    {
        DailyCheck();
        dailyPanel.SetActive(true);
    }

    public void CloseButton()
    {
        dailyPanel.SetActive(false);
    }

}
