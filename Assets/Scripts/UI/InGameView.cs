using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InGameView : MonoBehaviour
{

    // Level Progress bar variables
    [SerializeField] private Text cLevelText, nLevelText;
    [SerializeField] private Image fill;

    private int level;
    private int levelCounter = 0;
    private float startDistance, distance;

    [SerializeField] private GameObject finish, hand;
    [SerializeField] private TextMeshPro levelNo;

    private TextMeshProUGUI swipeText;
    [SerializeField]
    private TextMeshProUGUI cityText;


    // Theme Progress bar variables
    [SerializeField] private Text cThemeText, nThemeText;
    [SerializeField] private Image cThemeImage, nThemeImage, fillTheme;
    [SerializeField] GameObject themeProgressBar;

    // Score variables
    public float oldScore;
    public float score;
    public float highScore;
    public int winScore;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI feedbackText, playerText;

    public TextMeshProUGUI highScoreUI;
    public Text testText;
    float feedbacktextmaxY = 1;
    [SerializeField]
    float textwaitInterval;

    // Start position of the Feedback message texts to appear on screen
    [SerializeField]
    Vector3 feedbacktextPos, playertextPos;

    // This Speed factor is for how fast the Feedback tests move up in the ui Plane in time multiplier...
    [SerializeField]
    float textmoveSpeed = 10.2f;

    [SerializeField] string[] feedbackMessages;

    // instance of the object
    public static Score instance;

    // floating text for score popup
    private GameObject floatingText;

    // Player Dialog Menu variables
    [Header("Player Dialog Menu Stats")]
    public Text playerMenuDialogText;
    public Text playerMenuNameText;
    public TextMeshProUGUI playerEmot;
    public Image playerMenuBgImage;
    [SerializeField][Range(0,10)]
    private float fadeInterval;
    [SerializeField][Range(1,10)]
    private float delayAmount;

    // Tasks Dialog Menu variables (Array for holding multiple mission data)
    [Header("TASK DIALOG MENU STATS")]
    public Text[] missionMenuDialogText;
    public Text[] missionMenuNameText;
    public TextMeshProUGUI[] missionEmot;
    public Image[] missionMenuBgImage;
    [SerializeField]
    [Range(0, 10)]
    private float missionFadeInterval;
    [SerializeField]
    [Range(1, 10)]
    private float missionDelayAmount;


    // Start is called before the first frame update
    void Start()
    {
        InitializeMenu();
        if(this.gameObject.active)
        {
            FadePlayerMenu();
        }        
    }

    /// <summary>
    /// Player UI Above the head giving messages and fade out after time interval
    /// </summary>
    void  FadePlayerMenu()
    {
        //DG.Tweening.DOTweenModuleUI.DOFade(playerMenu.GetComponent<CanvasGroup>(), 0, 1.0f);
        //playerMenuText.DOFade(0.0f, 5.0f);
        playerMenuNameText.DOFade(0.0f, fadeInterval).SetDelay(delayAmount);
        playerMenuDialogText.DOFade(0.0f, fadeInterval).SetDelay(delayAmount);
        playerEmot.DOFade(0.0f, fadeInterval).SetDelay(delayAmount);
        playerMenuBgImage.DOFade(0.0f, fadeInterval).SetDelay(delayAmount);
    }

    /// <summary>
    /// Check progression amount, and update the bar upon mission complete percentage
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        //ShowFeedbackText();

        // Calculate the Level Progress Bar, the distance between the player and the FINISH point...
        distance = Vector3.Distance(Player.instance.transform.position, finish.transform.position);

        //if (Player.instance.transform.position.z < finish.transform.position.z)
        //    fill.fillAmount = 1 - (distance / startDistance);
        /*
        //  update Score
        highScoreUI.text = highScore.ToString();
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("highscore", highScore);
        }
        */
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    public void IncreaseProgressBarAmount(float i)
    {
        fill.fillAmount = i;
    }

    /// <summary>
    /// 
    /// </summary>
    void InitializeMenu()
    {
        //levelNo.text = "LEVEL " + level;

        // Get current level, and the next level, set the values on the progress bar on start
        nLevelText.text = GameManager.instance.GetLevelNo() + 1 + "";
        cLevelText.text = GameManager.instance.GetLevelNo().ToString();

        // Initialize Level Progress Bar
        startDistance = Vector3.Distance(Player.instance.transform.position, finish.transform.position);
        // Initialize Coins Text
        coinText.text = UIManager.instance.GetCoinsInGame().ToString();
        //testText.text = PlayerPrefs.GetFloat("Coin").ToString();

        // City texy initialize
        cityText.text = ("Which country has the capital city " + ObjectSpawner.instance.spawnedAnswer).ToUpper();

    }

    public void SetProgressBar(int level)
    {
        nLevelText.text = level + 1 + "";
        cLevelText.text = level.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetThemeProgressBarUI()
    {

        if (level < 6) //City
        {
            float cL = level - 1;
            float mL = 5;
            fillTheme.fillAmount = cL / mL;
            Debug.Log("Fill Amount: " + fillTheme.fillAmount);
            Debug.Log("Level: " + level);
        }
        else if (level < 11) //Ancient
        {
            float cL = level - 1 - 5;
            float mL = 5;
            fillTheme.fillAmount = cL / mL;

            //fillTheme.fillAmount = (float)level - 1 - 5 / 5;
            Debug.Log("Fill Amount: " + fillTheme.fillAmount);
            Debug.Log("Level: " + level);

        }
        else if (level < 16) //Snow
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    /// <param name="_score"></param>
    public void ShowFloatingText(Vector3 p, float _score)
    {
        var go = Instantiate(floatingText, p, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "+" + _score.ToString();

        //Instantiate(floatingText, p, Quaternion.identity, transform);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    public void ShowBonusText(Vector3 p)
    {
        var go = Instantiate(floatingText, p, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = "     Awesome!";
        go.GetComponent<TextMesh>().color = Color.red;
    }


    /// <summary>
    /// Show feedback upon object collection 
    /// </summary>
    /// <param name="collectible"></param>
    public void ShowFeedbackTextGeneric()
    {
        Debug.Log("ShowFeedbackTextGeneric called");
        AudioManager.instance.PlaySFX(1);
        StartCoroutine(MoveTextInTime());
    }

    /// <summary>
    /// Show feedback upon object collection 
    /// </summary>
    /// <param name="collectible"></param>
    public void ShowFeedbackTextForCollectible(Collectible collectible)
    {
        Debug.Log("ShowFeedbackTextForCollectible called");
        AudioManager.instance.PlaySFX(0);
        StartCoroutine(MoveTextInTime());
    }

    /// <summary>
    /// Show feedback message, with an additional  random Emoticon
    /// Wait for given time, and move text in intervals in Y axis up and fade out
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveTextInTime()
    {
        // Activate the text to start feedback movement accross Y axis, give
        // random messages, and fade out slowly
        int index = UnityEngine.Random.Range(0, feedbackMessages.Length);
        int alpha = 255;
        feedbackText.text = feedbackMessages[index] + " <sprite=" + index + ">";
        feedbackText.gameObject.SetActive(true);
        //Debug.Log("FeedbackText anchoredposition" + feedbackText.rectTransform.anchoredPosition);

        // Move in Y axis up until the anchoredposition of the text less than minimum feedback limit
        while(feedbackText.rectTransform.anchoredPosition.y < feedbacktextPos.y)
        {
            // fade out opacity(alpha value in time)
            if (alpha > 0)
            {
                alpha--;
            }
            feedbackText.color = new Color(255, 255, 255, alpha);
            feedbackText.rectTransform.anchoredPosition = new Vector2(0, feedbackText.rectTransform.anchoredPosition.y + (textmoveSpeed * Time.deltaTime));
            yield return new WaitForSeconds(textwaitInterval);
            //print("WaitAndPrint " + Time.time);
            //Debug.Log("FeedbackText anchoredposition" + feedbackText.rectTransform.anchoredPosition);
        }


        if(feedbackText.rectTransform.anchoredPosition.y >= feedbacktextPos.y)
        {
            // fade out and reset position
            feedbackText.color = new Color(0, 0, 0, 0);
            feedbackText.rectTransform.anchoredPosition = new Vector2(0, 0);
            //Debug.Log("FeedbackText NOT CHANGED anchoredposition" + feedbackText.rectTransform.anchoredPosition);
        }

        //yield return 0;
    }
    /// <summary>
    /// Activate the text to start feedback movement accross Y axis, give
    /// random messages, and fade out slowly
    /// </summary>
    public void ShowFeedbackText()
    {
        feedbackText.gameObject.SetActive(true);
        //Debug.Log("FeedbackText anchoredposition" + feedbackText.rectTransform.anchoredPosition);

        if (feedbackText.rectTransform.anchoredPosition.y < feedbacktextPos.y)
        {
            feedbackText.color = new Color(255, 255, 255, 255);
            feedbackText.rectTransform.anchoredPosition = new Vector2(0, feedbackText.rectTransform.anchoredPosition.y + ( textmoveSpeed * Time.deltaTime));
            //Debug.Log("FeedbackText anchoredposition" + feedbackText.rectTransform.anchoredPosition);
        }
        else
        {
            // fade out and reset position
            feedbackText.color = new Color(0, 0, 0, 0);
            feedbackText.rectTransform.anchoredPosition = new Vector2(0, 0);
            //Debug.Log("FeedbackText NOT CHANGED anchoredposition" + feedbackText.rectTransform.anchoredPosition);
        }
    }
    /// <summary>
    /// Adds the new coins to the coinText to display the new amount
    /// </summary>
    /// <param name="count"></param>
    public void AddCoins(int count)
    {
        coinText.text = count.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void DecrementScore(int amount)
    {
        oldScore -= amount;
        PlayerPrefs.SetFloat("Coin", oldScore);
        Debug.Log("Coins:" + oldScore);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="v"></param>
    public void MultiplyScore(float v)
    {
        score *= v;
    }
}
