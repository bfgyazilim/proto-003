using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using TMPro;

public class ScoreDisplayer : MonoBehaviour
{
    public FloatVariable Score;
    public TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        scoreText.text = Score.Value.ToString();
    }

    public void UpdateScore()
    {
        scoreText.text = Score.Value.ToString();
    }
}
