using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine.UI;

public class SceneCompletion : MonoBehaviour
{
    public GameObject panel;
    public FloatVariable EarnedStars;
    public FloatVariable Cash;

    public FloatReference OneStarScore;
    public FloatReference TwoStarScore;
    public FloatReference ThreeStarScore;

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
    public void CheckCash()
    {
        if(Cash.Value > 10)
        {
            CompleteLevel(Cash.Value);
        }
    }

    public void CompleteLevel(float score)
    {
        panel.SetActive(true);

        if(score >= ThreeStarScore.Value)
        {
            // Enable StarImage0
            starImage2.gameObject.SetActive(true);
        }
        else if (score >= TwoStarScore.Value)
        {
            // Enable StarImage0
            starImage1.gameObject.SetActive(true);
        }
        if (score >= OneStarScore.Value)
        {
            // Enable StarImage0
            starImage0.gameObject.SetActive(true);
        }
    }
}
