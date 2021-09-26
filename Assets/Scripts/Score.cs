using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Score : MonoBehaviour {

    public float oldScore;
    public float score;
    public float highScore;
    public int winScore;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI highScoreUI;
    public Text testText;

    // instance of the object
    public static Score instance;

    // floating text for score popup
    public GameObject floatingText;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    void Start() {
        highScore = PlayerPrefs.GetInt("highscore");
        
        // Get previous coins amount from prefs
        oldScore = PlayerPrefs.GetFloat("Coin");
        Debug.Log("Coins:" + oldScore);
        coinText.text = PlayerPrefs.GetFloat("Coin").ToString();
        testText.text = PlayerPrefs.GetFloat("Coin").ToString();
    }

    // Update is called once per frame
    void Update () {
        highScoreUI.text = highScore.ToString();
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetFloat("highscore", highScore);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "scoreup") {

            // Trigger floating text
            ShowFloatingText(other.gameObject.transform.position, 10);
            score++;
        }
        if (other.gameObject.tag == "box")
        {
            score += 2;
        }
        if (other.gameObject.tag == "hamburger")
        {
            score += 40;
        }
        if (other.gameObject.tag == "star")
        {
            score += 10;
        }
        if (other.gameObject.tag == "apple")
        {
            score += 10;
        }
        if (other.gameObject.tag == "banana")
        {
            score += 10;
        }
        if (other.gameObject.tag == "fishy")
        {
            score += 20;
        }
        if (other.gameObject.tag == "steak")
        {
            score += 30;
        }
        if (other.gameObject.tag == "coin")
        {
            // Trigger floating text
            ShowFloatingText(other.gameObject.transform.position, 1);
            score++;
            //score += 30;
        }
        /*
        if (score > winScore)
        {
            Player.instance.WinGame();
        }
        */
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
    /// 
    /// </summary>
    public void SaveScore()
    {
        float c = oldScore + score;
        PlayerPrefs.SetFloat("Coin", c);
        Debug.Log("Level end coins:" + c);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="count"></param>
    public void AddScore(int count)
    {
        // Increment coins
        int coin = PlayerPrefs.GetInt("Coin");
        coin += count;
        // Save to prefs
        PlayerPrefs.SetInt("Coin", coin);
        // Show in text total
        coinText.text = coin.ToString();
        Debug.Log("Coins:" + coin);
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
