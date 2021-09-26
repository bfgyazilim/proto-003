using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuView : MonoBehaviour
{
    public Button playButton;
    public TextMeshProUGUI coinText;

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = PlayerPrefs.GetInt("Coin").ToString();
        Button btn = playButton.GetComponent<Button>();
        btn.onClick.AddListener(PlayButtonOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayButtonOnClick()
    {
        Debug.Log("You have clicked the button!");

        GameManager.instance.StartGame();
    }
}
