using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using TMPro;

// Include Facebook namespace
//using Facebook.Unity;

public class App_Initialize : MonoBehaviour {

    public GameObject inMenuUI;
    public GameObject inGameUI;
    public GameObject gameOverUI;

    public GameObject rewardMenuUI;

    public GameObject adButton;
    public GameObject restartButton;
    public GameObject player;
    private bool hasGameStarted = false;
    private bool hasSeenRewardedAd = false;
    public static App_Initialize instance;

    // keep track of game progress and send to analytics...
    bool isUserCompleteLevel;

    void Awake()
    {
        /*
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        */
        instance = this;
        Shader.SetGlobalFloat("_Curvature", 2.0f);
        Shader.SetGlobalFloat("_Trimming", 0.1f);
        Application.targetFrameRate = 60;
    }

    /*
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
    */
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    void Start () {
        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        inMenuUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
        rewardMenuUI.gameObject.SetActive(false);
    }

    public void PlayButton() {

        Debug.Log("Play Button Method Here");
        if (hasGameStarted == true) {
            StartCoroutine(StartGame(1.0f));
        } else {
            StartCoroutine(StartGame(0.0f));
        }
    }    

    /// <summary>
    /// 
    /// </summary>
    public void PauseGame() {
        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        hasGameStarted = true;
        inMenuUI.gameObject.SetActive(true);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void GameOver() {

        // If you want to track if the user was able to finish the level of the game

        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), false, Score.instance.score);

        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        hasGameStarted = true;
        inMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);

        /*
        if (hasSeenRewardedAd == true) {
            adButton.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            adButton.GetComponent<Button>().enabled = false;
            adButton.GetComponent<Animator>().enabled = false;
            restartButton.GetComponent<Animator>().enabled = true;
        }
        */
    }

    /// <summary>
    /// 
    /// </summary>
    public void RestartGame()
    {
        // failed the game
        // If win the game get new scene, or get the old scene...
        if(Player.instance.levelFailed == false)
        {
            //reloads same scene scene if failed
            SceneManager.LoadScene(GameManager.instance.GetLevelScene());
        }
        else
        {
            // win the game
            // If you want to track if the user was able to finish the level of the game
            //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString() , true, Score.instance.score);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ShowAd() {
        if (Advertisement.IsReady("rewardedVideo")) {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="result"></param>
    private void HandleShowResult(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                hasSeenRewardedAd = true;
                StartCoroutine(StartGame(1.5f));
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator StartGame(float waitTime)
    {
        // If your game has levels
        //TinySauce.OnGameStarted(levelNumber: GameManager.instance.GetLevelNo().ToString());

        // Disable hand object and text at start
        //GameManager.instance.RemoveUI();

        // Setup menus
        inMenuUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);

        // Player change state to running
        //Player.instance.ChangePlayerStateToRun();

        yield return new WaitForSeconds(waitTime);
    }


}
