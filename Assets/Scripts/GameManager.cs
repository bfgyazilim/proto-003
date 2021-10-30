using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int level;
    [SerializeField]
    private int levelCount;
    private int levelCounter = 0;
    [SerializeField]
    private int[] enemyCount;
    [SerializeField]
    private int remainingEnemy;
    private int killedEnemy;
    [SerializeField] private TextMeshPro levelNo;

    private TextMeshProUGUI swipeText;

    // instance of the object
    public static GameManager instance;

    // If checked deletes all player data
    public bool delete;

    public int nextLevelIndex;
    private int levelIndex;

    // old code

    private InGame ig;

    private GameObject[] runners;

    List<RankingSystem> sortArray = new List<RankingSystem>();

    public int pass;
    public bool failed, start;

    public string firstPlace, secondPlace, thirdPlace;
    // end old code

    // Game Level Ending variables
    [SerializeField]
    GameObject mainCharacter;
    [SerializeField]
    Transform mainPoint;
    [SerializeField]
    GameObject[] prisoners;
    [SerializeField]
    GameObject[] doors;


    // Timelines
    public GameObject flashbackTimeline, stormTimeline, aicommandTimeline;
    public GameObject dialoguePanel;

    public List<GameObject> timelines;
    [SerializeField]
    int lastActiveTimelineIndex = -1;

    // Mission stat variables (Later put on to MissionManager)
    [Header("Mission Stats")]
    [SerializeField]
    public UnityEvent[] OnMissionComplete;
    [SerializeField]
    int[] remainingTasks;
    [SerializeField]
    int[] totalTasks;
    [SerializeField]
    int[] clearedTasks;
    [SerializeField]
    int[] buildCost;
    bool resourcesSufficient;

    public enum MissionType
    {
        TILECLEAN, DESTRUCTWALLS, CARRYBOXES, BUILDHOUSE, DESTRUCTTREES, BUILDBRIDGE
    };

    //public MissionType mission;

    /// <summary>
    /// 
    /// </summary>
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
        killedEnemy = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        // delete all player data...
        if (delete)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted PlayerPrefs");
        }

        // get level data from the device If any
        if (PlayerPrefs.GetInt("Level") > 0)
        {
            level = PlayerPrefs.GetInt("Level");
            // Timeline logic, for extra camera effects and shots!
            // If came again for the scene reload
            /*
            lastActiveTimelineIndex = PlayerPrefs.GetInt("lastActiveTimelineIndex");
            if(lastActiveTimelineIndex > -1)
            {
                // So that we have a remaining level played before, activate that timeline
                ResetTimelinesToLastActive();
            }
            */
        }
        else
        {
            level = 1;
            /*
            lastActiveTimelineIndex = -1;
            */
        }

#if (!UNITY_EDITOR)
        // Set Up Voodoo Tiny Sauce at Start!
        TinySauce.OnGameStarted();
        //TinySauce.OnGameStarted(levelNumber: level.ToString());
#endif

        // Set the active level remaining enemy on less or more than levelCount levels
        // If more than levelCount, it will repeat levels, so the remaining enemies to match that
        // take the remainder to get values from (0-levelCount)
        if (level > levelCount)
        {
            int indLevel = level % levelCount;
            remainingEnemy = enemyCount[indLevel - 1];
        }
        else
        {
            // Set remaining enemy count according to the level
            remainingEnemy = enemyCount[level - 1];
        }

        // Set UI Views state to Start...
        //UIManager.instance.ResetViewsToStart();
        // Update level numbers on Progress Bar left and right
        //UIManager.instance.SetNewLevelUI(level);
        levelNo.text = "LEVEL " + level;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetEnemyCount()
    {
        // Set the active level remaining enemy on less or more than levelCount levels
        // If more than levelCount, it will repeat levels, so the remaining enemies to match that
        // take the remainder to get values from (0-levelCount)
        if (level > levelCount)
        {
            int indLevel = level % levelCount;
            return enemyCount[indLevel - 1];
        }
        else
        {
            return enemyCount[level - 1];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetRemainingEnemyCount()
    {
        return remainingEnemy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int DecreaseEnemy()
    {
        if (remainingEnemy > 0)
        {
            remainingEnemy--;
            killedEnemy++;
            // Update Progress Bar from UIManager            
            float progressRatio = (float)killedEnemy / (float)GetEnemyCount();
            UIManager.instance.IncreaseInGameProgressBar(progressRatio);

            if (remainingEnemy == 0)
            {
                PlayerController.instance.ChangePlayerStateToWin();
            }
        }

        return remainingEnemy;
    }

    /// <summary>
    /// Gets the current state of the mission
    /// </summary>
    /// <returns></returns>
    public bool GetMissionStatus()
    {
        return resourcesSufficient;
    }

    /// <summary>
    /// Specific to Mission 1, adjust the progress bar level according to the remaining floor tiles
    /// </summary>
    /// <returns></returns>
    public void HandleMissionProgress(int missionNo)
    {
        // Check Resource availability for the mission, or Do Nothing!
        int plankCount = UIManager.instance.GetPlanksInGame();
        int coinCount = UIManager.instance.GetCoinsInGame();

        // Check resources for misson type BUILDBRIDGE
        if(missionNo == (int)MissionType.BUILDBRIDGE)
        {
            if(plankCount < buildCost[(int)MissionType.BUILDBRIDGE])
            {
                resourcesSufficient = false;
                return;
            }
        }
        else if (missionNo == (int)MissionType.BUILDHOUSE)
        {
            if(plankCount < buildCost[(int)MissionType.BUILDHOUSE])
            {
                resourcesSufficient = false;
                return;
            }
        }
            // If not a bridge mission now, continue at any case...
            resourcesSufficient = true;

        // Update the progress bar, resources sufficient continue to the mission progress...
        if (remainingTasks[missionNo] > 0)
        {
            remainingTasks[missionNo]--;
            clearedTasks[missionNo]++;
            // Update Progress Bar from UIManager            
            float progressRatio = (float)clearedTasks[missionNo] / (float)totalTasks[missionNo];
            UIManager.instance.IncreaseInGameProgressBar(progressRatio);

            Debug.Log("Remaining tasks in mission: " + remainingTasks[missionNo]);

            if (remainingTasks[missionNo] == 0)
            {
                // Trigger OnMissionComplete Event
                OnMissionComplete[missionNo].Invoke();
                Debug.Log("Mission " + missionNo + "  Complete");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void WinGame()
    {
        // If you want to track if the user was able to finish the level of the game

        // ENABLE BEFORE PUBLISHING
        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), true, Score.instance.score);

        // Save coins to the device after level end
        Score.instance.SaveScore();

        // If you want to track if the user was able to finish the level of the game
        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), false, Score.instance.score);

        // Increment level, and get new settings
        GameManager.instance.SetNewLevel();

        // Reload Game
        StartCoroutine("WaitAndLoad");
    }

    /// <summary>
    /// 
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        // Reload Scene for debug purposes
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public void SetNewLevel()
    {
        // Save level to prefs
        PlayerPrefs.SetInt("Level", ++level);

        levelNo.text = "LEVEL " + level;

        UIManager.instance.SetNewLevelUI(level);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetLevelScene()
    {
        // First build index yet, add others later!!!
        return 0;

        /*
        if (level < 6)
        {
            return level - 1;
            
        }
        else
        {
            return Random.Range(0, 7);
        }
        */


        // No randomizer yet
        /*

        return level - 1;

        if(level < 6)
        {
            return 0; // City
        }
        else if(level >=6 && level < 11)
        {
            return 1; // Ancient
        }
        else if (level >= 11 && level < 16)
        {
            return 2; // Snow
        }
        else
        {
            // return random one of those three themes
            return Random.Range(0, 3);
        }
        */
    }

    public int GetLevelNo()
    {
        return level;
    }


    // keep track of game progress and send to analytics...
    bool isUserCompleteLevel;


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

    /// <summary>
    /// 
    /// </summary>
    public void GameOver()
    {
        // If you want to track if the user was able to finish the level of the game
#if (!UNITY_EDITOR)
        // Set Up Voodoo Tiny Sauce Game Progress!
        TinySauce.OnGameFinished(UIManager.instance.GetCoinsInGame());
        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), false, Score.instance.score);

#endif

        //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        // Set player state to levelFailed!
        Player.instance.levelFailed = true;

        // Set UI Views state to LOSE!
        UIManager.instance.ResetViewsToLose();

        //Debug.Log("Game Over");
    }

    /// <summary>
    /// 
    /// </summary>
    public void RestartGame()
    {
        // If WIN the game get new scene, or If FAILED get the old scene...
        if (Player.instance.levelFailed == false)
        {
            // After moving to the next timeline, make sure that you save the new level, and the last active timelines,
            // to the PlayerPrefbs (device), and reset the Remaining enemy count and such values related to THAT timeline specific.
            level++;

            // Increase level, add to PlayerPrefbs (save to device)
            PlayerPrefs.SetInt("Level", level);

            // Reset UI Progress Bar on new level
            UIManager.instance.IncreaseInGameProgressBar(0f);
            // Update level numbers on Progress Bar left and right
            UIManager.instance.SetNewLevelUI(level);
            // Add coins to the total!
            UIManager.instance.AddCoinsToInGameView(1);
            // Save collected coins
            UIManager.instance.SaveCoins();

#if (!UNITY_EDITOR)
            // TinySauce send level and score values
            TinySauce.OnGameFinished(true, UIManager.instance.GetCoinsInGame(), levelNumber: level.ToString());
#endif
            //reloads the scene scene if WIN
            SceneManager.LoadScene(GetLevelScene());

        }
        else
        {
            // Save the last active Timeline to PlayerPrefs for later loading fro where you left of...
            //PlayerPrefs.SetInt("lastActiveTimelineIndex", lastActiveTimelineIndex);

            // Set the level you are on, so that after scene reload, It knows where you left of.
            PlayerPrefs.SetInt("Level", level);

#if (!UNITY_EDITOR)
            // TinySauce send level and score values
            TinySauce.OnGameFinished(false, UIManager.instance.GetCoinsInGame(), levelNumber: level.ToString());
#endif

            // Ah, I failed, reload the scene again (Same Scene in this prototype)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartGame()
    {
        // Set UI Views state to START!
        UIManager.instance.ResetViewsToStart();
        // Allow Player to move
        Player.instance.levelStarted = true;
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

        // Set UI Views state to START!
        UIManager.instance.ResetViewsToStart();

        // Player change state to running
        //Player.instance.ChangePlayerStateToRun();

        yield return new WaitForSeconds(waitTime);
    }

    /// <summary>
    /// Wait for the given amount of seconds
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public IEnumerator WaitForGivenTime(float f)
    {
        // Move the main character to the end point to show success, and zoom in, or slow time, whatever
        //aicommandTimeline.SetActive(true);
        //ActivateTimeline();

        // Wait 3 seconds for animation to finish
        yield return new WaitForSeconds(2f);

        // Now that you have waited, turn on the WinView to go to the next level prompt.
        UIManager.instance.ResetViewsToWin();

        // Show how many coins collected in this level +XXX

        //currentScoreUI.text = "+" + Score.instance.score.ToString();
    }

    /// <summary>
    /// Timeline management temporary here, later add a Timeline Manager class!
    /// </summary>
    public void ActivateTimeline()
    {
        // If not first Timeline ?
        if (lastActiveTimelineIndex != -1)
        {
            //timelines[lastActiveTimelineIndex].SetActive(false);
        }
        // activate the next timeline
        if (++lastActiveTimelineIndex < timelines.Count)
        {
            timelines[lastActiveTimelineIndex].SetActive(true);
        }
        else
        {
            // all levels passed, turn back to the start
            lastActiveTimelineIndex = -1;
            // deactivate all passed timelines
            for (int i = 0; i < timelines.Count; i++)
            {
                timelines[i].SetActive(false);
            }
        }

        // After moving to the next timeline, make sure that you save the new level, and the last active timelines,
        // to the PlayerPrefbs (device), and reset the Remaining enemy count and such values related to THAT timeline specific.
        level++;

        // Increase level, add to PlayerPrefbs (save to device)
        PlayerPrefs.SetInt("Level", level);
        /*
        // TinySauce send level and score values
        TinySauce.OnGameFinished(true, UIManager.instance.GetCoinsFromGameView(), levelNumber: level.ToString());
        */

        if (level > levelCount)
        {
            // Our scenes in stock are finished, so delete PlayerPrefs, and start from first level again!!!
            // Increase level, add to PlayerPrefbs (save to device)            
            // Save the last active timeline, so we have increased the the level,  show/play the next Timeline stage

            PlayerPrefs.SetInt("lastActiveTimelineIndex", UnityEngine.Random.Range(-1, 7));
            Debug.Log("Reseted values. now will LOAD scene again!");
            // Reload scene, start from level 1 - also timelines and everyrhing resets to original...
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {

            // Save the last active timeline, so we have increased the the level,  show/play the next Timeline stage
            PlayerPrefs.SetInt("lastActiveTimelineIndex", lastActiveTimelineIndex);
            // Reset remaining enemy count again according to which level we have reached, read from properties.
            remainingEnemy = enemyCount[level - 1];
        }

        // Reset UI Progress Bar on new level
        UIManager.instance.IncreaseInGameProgressBar(0f);
        // Update level numbers on Progress Bar left and right
        UIManager.instance.SetNewLevelUI(level);
        // Add coins to the total!
        UIManager.instance.AddCoinsToInGameView(killedEnemy);
        // Save collected coins
        //UIManager.instance.SaveCoins(killedEnemy);
        // Reset killed enemy
        killedEnemy = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetTimelinesToLastActive()
    {
        // activate the LAST ACTIVE timeline, after scene reload, start from there
        if (lastActiveTimelineIndex < timelines.Count)
        {
            timelines[lastActiveTimelineIndex].SetActive(true);
            // deactivate all the next timelines
            for (int i = 0; i < timelines.Count; i++)
            {
                // Deactivate all timelines other than the last active timeline
                if (i != lastActiveTimelineIndex)
                {
                    timelines[i].SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Timeline management temporary here, later add a Timeline Manager class!
    /// </summary>
    private void ActivateTimeline(int tl)
    {
        timelines[tl].SetActive(true);
    }

    /// <summary>
    /// This function is for Camera movement controlling from Game central point for the level end mechanism, and zooming
    /// in the helicopter and etc...
    /// </summary>
    public void ActivateLevelEnding()
    {
        int i = 0;
        // In this case It is the helicopter, or other object facing the end point a little backwards and above...
        mainCharacter.transform.position = new Vector3(mainCharacter.transform.position.x, mainCharacter.transform.position.y - 28.22f, mainCharacter.transform.position.z + 22.2f);
        // stop moving, just focus and wait on the end point for the level end....
        mainCharacter.GetComponent<MoveUpAndDown>().enabled = false;
        mainCharacter.GetComponent<MoveUpAndDownY>().enabled = false;

        // rotate each door 90 degrees around Y axis (It means opening the doors!)
        foreach (GameObject door in doors)
        {
            //door.transform.RotateAround(Vector3.up, 90f);
            if (i == 0)
            {
                door.transform.position += new Vector3(-3f, 0, 0);
                i++;
            }
            else
            {
                door.transform.position += new Vector3(3f, 0, 0);
            }
        }

        // here we will manage the status of the movement, and take prisoners
        // to the helicopter and/or slow motion for the cut scene, to give feedback
        // and accomplisment feeling the the Player!
        foreach (GameObject prisoner in prisoners)
        {
            // move all of the prisoners to the pickup point...
            prisoner.GetComponent<PrisonerController>().ChangeDestination(mainPoint.transform);
        }
    }

    /// <summary>
    /// Handles MissionComplete Event
    /// </summary>
    /// <param name="missionNo"></param>
    public void HandleMissionComplete(MissionType missionNo)
    {
        Debug.Log("Mission " + missionNo + "Completed");
        // If It is carry boxes mission
        if ((int)missionNo == (int)MissionType.CARRYBOXES)
        {
            OnMissionComplete[(int)missionNo].Invoke();
            ActivateTimeline(0);
        }

        // If It is build house next to road mission?!
        if ((int)missionNo == (int)MissionType.BUILDHOUSE)
        {
            OnMissionComplete[(int)missionNo].Invoke();
            ActivateTimeline(1);
        }
    }
}

