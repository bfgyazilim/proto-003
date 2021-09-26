using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseView : MonoBehaviour
{
    // button logic
    public Button loseButton;

    void NextButtonOnClick()
    {
        Debug.Log("You have clicked the button!");

        GameManager.instance.RestartGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        Button btn = loseButton.GetComponent<Button>();
        btn.onClick.AddListener(NextButtonOnClick);
    }
}
