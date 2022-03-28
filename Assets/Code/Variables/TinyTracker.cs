using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

public class TinyTracker : MonoBehaviour
{
    public FloatVariable CurrentLevel;
    public FloatVariable Score;

    // Start is called before the first frame update
    void Start()
    {
        TinySauce.OnGameStarted(CurrentLevel.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrackLevelCompleted()
    {
        TinySauce.OnGameFinished(true, Score.Value, CurrentLevel.ToString());
    }
}
