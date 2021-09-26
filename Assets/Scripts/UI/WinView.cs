using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WinView : MonoBehaviour
{
    // button logic
    public Button nextButton;

    // Array of coins to be animated on click next, to the score Up Above one by one
    public Image[] coins;

    public TextMeshProUGUI coinText, rescueText;

    public static WinView instance;

    void NextButtonOnClick()
    {
        Debug.Log("You have clicked the button!");

        // Animate all Images to up for giving collected to Global coins
        foreach (Image cImage in coins)
        {
            cImage.gameObject.GetComponent<Animator>().SetBool("isActive", true);
            AudioManager.instance.PlayLevelWin();
            // Call coin collect here
            //coin.gameObject.GetComponent<Animator>().SetBool("isActive", true);
        }

        // Won the game, so that we can continue to the next Timeline (Which will bring us to the)
        // next platform for fight!!!

        // Disable WinView
        //gameObject.SetActive(false);
        UIManager.instance.ResetViewsToStart();
        // Start journey to the next Platform...
        //GameManager.instance.ActivateTimeline();
        GameManager.instance.RestartGame();
    }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Button btn = nextButton.GetComponent<Button>();
        btn.onClick.AddListener(NextButtonOnClick);

        // Display total coins
        coinText.text = UIManager.instance.GetCoinsInGame().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
