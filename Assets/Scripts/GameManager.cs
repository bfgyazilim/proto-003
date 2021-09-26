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
}
    // end runner code

