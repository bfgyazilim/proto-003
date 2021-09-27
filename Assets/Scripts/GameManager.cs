using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    // Level Progress bar variables
    [SerializeField] private Text cLevelText, nLevelText;
    [SerializeField] private Image fill;

    private int level;
    private int levelCounter = 0;
    private float startDistance, distance;

    [SerializeField] private GameObject player, finish, hand;
    [SerializeField] private TextMeshPro levelNo;
     
    private TextMeshProUGUI  swipeText;


    // Theme Progress bar variables
    [SerializeField] private Text cThemeText, nThemeText;
    [SerializeField] private Image cThemeImage, nThemeImage, fillTheme;
    [SerializeField] GameObject themeProgressBar;

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


    private void Awake()
    {
        instance = this;
        /*
        cLevelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();
        nLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        fill = GameObject.Find("Fill").GetComponent<Image>();

        player = GameObject.Find("Player");
        finish = GameObject.Find("Finish");
        hand = GameObject.Find("Hand");
        levelNo = GameObject.Find("LevelNo").GetComponent<TextMesh>();
        */
    }
    

    // Start is called before the first frame update
    void Start()
    {
        // delete all player data...
        if (delete)
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Deleted PlayerPrefs");
        }

        if (PlayerPrefs.GetInt("Level") >  0)
        {
            level = PlayerPrefs.GetInt("Level");            
        }
        else
        {
            level = 1;
        }

        levelNo.text = "LEVEL " + level;

        nLevelText.text = level + 1 + "";
        cLevelText.text = level.ToString();

        startDistance = Vector3.Distance(player.transform.position, finish.transform.position);

        SetUI();        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, finish.transform.position);

        if(player.transform.position.z < finish.transform.position.z)
            fill.fillAmount = 1 - (distance / startDistance);
    }

    public void SetUI()
    {
        
        if(level < 6) //City
        {            
            float cL = level - 1;
            float mL = 5;
            fillTheme.fillAmount = cL / mL;            
            Debug.Log("Fill Amount: " + fillTheme.fillAmount);
            Debug.Log("Level: " + level);        
        }
        else if(level < 11) //Ancient
        {
            float cL = level - 1 - 5;
            float mL = 5;
            fillTheme.fillAmount = cL / mL;

            //fillTheme.fillAmount = (float)level - 1 - 5 / 5;
            Debug.Log("Fill Amount: " + fillTheme.fillAmount);
            Debug.Log("Level: " + level);

        }
        else if(level < 16) //Snow
        {
            float cL = level - 1 - 10;
            float mL = 5;
            fillTheme.fillAmount = cL / mL;

            //fillTheme.fillAmount = (float)level - 1 - 10 / 5;
            Debug.Log("Fill Amount: " + fillTheme.fillAmount);
            Debug.Log("Level: " + level);
        }
        else
        {
            themeProgressBar.SetActive(false);
        }

    }

    public void RemoveUI()
    {
        hand = GameObject.Find("Hand");

        hand.SetActive(false);
        //swipeText.gameObject.SetActive(false);
    }

    public void SetNewLevel()
    {
        // Save level to prefs
        PlayerPrefs.SetInt("Level", ++level);

        levelNo.text = "LEVEL " + level;

        nLevelText.text = level + 1 + "";
        cLevelText.text = level.ToString();
    }

    public int GetLevelScene()
    {
        if (level < 6)
        {
            return level - 1;
            
        }
        else
        {
            return Random.Range(0, 7);
        }

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

    // rr runner code
   
    void CalculatingRank()
    {
        sortArray = sortArray.OrderBy(t => t.counter).ToList();
        switch (sortArray.Count)
        {
            case 3:
                sortArray[0].rank = 3;
                sortArray[1].rank = 2;
                sortArray[2].rank = 1;

                ig.a = sortArray[2].name;
                ig.b = sortArray[1].name;
                ig.c = sortArray[0].name;
                break;

            case 2:
                sortArray[0].rank = 2;
                sortArray[1].rank = 1;

                ig.a = sortArray[1].name;
                ig.b = sortArray[0].name;
                ig.thirdPlaceImg.color = Color.red;
                break;

            case 1:
                sortArray[0].rank = 1;
                ig.a = sortArray[0].name;
                if (firstPlace == "")
                {
                    firstPlace = sortArray[0].name;
                    GameUI.instance.OpenLB();
                }
                break;
        }

        if (pass >= (float)runners.Length / 2)
        {
            pass = 0;
            sortArray = sortArray.OrderBy(t => t.counter).ToList();
            foreach (RankingSystem rs in sortArray)
            {
                if (rs.rank == sortArray.Count)
                {
                    if (rs.gameObject.name == PlayerPrefs.GetString("PlayerName"))
                    {
                        //failed = true;
                        GameUI.instance.OpenLB();
                    }

                    if (thirdPlace == "")
                        thirdPlace = rs.gameObject.name;
                    else if (secondPlace == "")
                        secondPlace = rs.gameObject.name;

                    rs.gameObject.SetActive(false);
                }
            }

            runners = GameObject.FindGameObjectsWithTag("Runner");

            sortArray.Clear();
            for (int i = 0; i < runners.Length; i++)
            {
                sortArray.Add(runners[i].GetComponent<RankingSystem>());
            }

            if (runners.Length < 2)
            {
                //finish = true;
                if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("Level"))
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            }
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
    /// 
    /// </summary>
    public void GameOver()
    {

        // If you want to track if the user was able to finish the level of the game

        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), false, Score.instance.score);

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

            // TinySauce send level and score values
            //TinySauce.OnGameFinished(true, UIManager.instance.GetCoinsInGame(), levelNumber: level.ToString());

            //reloads the scene scene if WIN
            SceneManager.LoadScene(GetLevelScene());

        }
        else
        {
            // Save the last active Timeline to PlayerPrefs for later loading fro where you left of...
            //PlayerPrefs.SetInt("lastActiveTimelineIndex", lastActiveTimelineIndex);

            // Set the level you are on, so that after scene reload, It knows where you left of.
            PlayerPrefs.SetInt("Level", level);

            // TinySauce send level and score values
            //TinySauce.OnGameFinished(false, UIManager.instance.GetCoinsInGame(), levelNumber: level.ToString());

            // Ah, I failed, reload the scene again (Same Scene in this prototype)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
    // end runner code

