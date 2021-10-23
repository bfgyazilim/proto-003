﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    // UI View references from the scene
    
    public InGameView inGameView;

    [SerializeField]
    WinView winView;
    [SerializeField]
    LoseView loseView;
    [SerializeField]
    MainMenuView mainMenuView;

    public static UIManager instance;

    private int coinsTotal;

    // Mission progress menu in top of the screen that show text messages about your next activity or job...
    public RectTransform missionMenu;

    /// <summary>
    /// Sets the instance
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize coins on Start
        coinsTotal = PlayerPrefs.GetInt("Coin");
        // Set menu states on Start
        mainMenuView.gameObject.SetActive(true);
        inGameView.gameObject.SetActive(false);
        loseView.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When Game Over, this is called for returning the Views to the LoseView State....
    /// </summary>
    public void ResetViewsToLose()
    {
        mainMenuView.gameObject.SetActive(false);
        inGameView.gameObject.SetActive(false);
        loseView.gameObject.SetActive(true);
    }

    /// <summary>
    /// When Game Over, this is called for returning the Views to the LoseView State....
    /// </summary>
    public void ResetViewsToStart()
    {
        winView.gameObject.SetActive(false);
        mainMenuView.gameObject.SetActive(false);
        inGameView.gameObject.SetActive(true);
        loseView.gameObject.SetActive(false);

        // Show mission objective
        // enter game with a slide animation at top
        SlideMissionMenu(0, -300);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="level"></param>
    public void SetNewLevelUI(int level)
    {
        inGameView.SetProgressBar(level);
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetViewsToWin()
    {
        winView.gameObject.SetActive(true);
        inGameView.gameObject.SetActive(false);
        loseView.gameObject.SetActive(false);        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    public void SaveCoins()
    {
        coinsTotal += 5;
        // Save to prefs
        PlayerPrefs.SetInt("Coin", coinsTotal);
        // Show in text total
        //coinText.text = coin.ToString();
        Debug.Log("SAVED Coins:" + coinsTotal);
    }

    public void IncreaseInGameProgressBar(float i)
    {
        inGameView.IncreaseProgressBarAmount(i);
    }

    public void SetInGameProgressBarFill(float i)
    {
        inGameView.IncreaseProgressBarAmount(i);
    }

    public void AddCoinsToInGameView(int count)
    {
        // Increase Total Coins by Count
        coinsTotal += count;
        // Update inGameView UI Text to reflect the change on Game
        inGameView.AddCoins(coinsTotal);
        Debug.Log("TOTAL Coins: " + coinsTotal);
    }

    /// <summary>
    /// Get how many coins user have
    /// </summary>
    /// <returns></returns>
    public int GetCoins()
    {
        return PlayerPrefs.GetInt("Coin");
    }

    public int GetCoinsInGame()
    {
        return coinsTotal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void DecreaseCoins(int amount)
    {
        // Store last remaining coins after purchase to the coinsTotal in UIManager (access it from other canvases during the game If needed!)
        coinsTotal -= amount;
        PlayerPrefs.SetInt("Coin", coinsTotal);
        Debug.Log("Coins:" + coinsTotal + " decreased");
    }

    /// <summary>
    /// Handle the pickup logic in player
    /// </summary>
    /// <param name="collectible"></param>
    public void HandleCoinPickup(Collectible collectible)
    {
        AddCoinsToInGameView(2);  
    }

    /// <summary>
    /// Sliding animation for UI Menus, from left to right get into the screen for example
    /// </summary>
    /// <param name="rect"></param>
    public void SlideMenu(RectTransform rect)
    {
        rect.DOAnchorPos(Vector2.zero, 0.25f);
    }

    /// <summary>
    /// Sliding animation for UI Menus, from left to right get into the screen for example
    /// </summary>
    /// <param name="rect"></param>
    public void SlideMissionMenu(int x, int y)
    {
        
        missionMenu.DOAnchorPos(new Vector2(x, y), 0.25f).SetDelay(1.05f);        
    }
}
