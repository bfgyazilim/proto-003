using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneCompletion : MonoBehaviour
{
    public GameObject panel;
    public FloatVariable EarnedStars;
    public FloatVariable LevelScore, Score;
    public FloatVariable CurrentLevel;
    public FloatReference OneStarScore;
    public FloatReference TwoStarScore;
    public FloatReference ThreeStarScore;

    public Button nextButton;

    public UnityEvent OnSceneCompleteEnabled;


    [SerializeField]
    Image starImage0, starImage1, starImage2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckLevelScore()
    {
        LevelScore.Value += 1;
        if(LevelScore.Value > 10)
        CompleteLevel(LevelScore.Value);
    }

    public void CompleteLevel(float score)
    {
        panel.SetActive(true);
        OnSceneCompleteEnabled.Invoke();

        if(score >= ThreeStarScore.Value)
        {
            // Enable All 3 Stars
            starImage2.gameObject.SetActive(true);
            starImage1.gameObject.SetActive(true);
            starImage0.gameObject.SetActive(true);
            // Increment Level
            CurrentLevel.Value += 1;
            LevelScore.Value = 0;
            // Activate Next button (we passed the level!!! wow aha!)
            nextButton.gameObject.SetActive(true);
        }
        else if (score >= TwoStarScore.Value)
        {
            // Enable StarImage0
            starImage1.gameObject.SetActive(true);
            starImage0.gameObject.SetActive(true);
        }
        if (score >= OneStarScore.Value)
        {
            // Enable StarImage0
            starImage0.gameObject.SetActive(true);
        }
        else
        {
            // kick ass! you succeed
            starImage2.gameObject.SetActive(false);
            starImage1.gameObject.SetActive(false);
            starImage0.gameObject.SetActive(false);
        }
    }

    public void LoadLevel()
    {
        Debug.Log("Load Level " + CurrentLevel.Value);
        SceneManager.LoadScene((int)CurrentLevel.Value);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }


}
