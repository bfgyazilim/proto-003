using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProgressionStatus : MonoBehaviour
{
    public Image progressionImage;
    public Text timerObj;
    public float totalTime = 20;

    public UnityEvent onProgressionStatusEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalTime = totalTime - Time.deltaTime;
        if (totalTime > 0)
        {
            timerObj.text = totalTime.ToString("0");
            progressionImage.fillAmount -= 1.0f / 20 * Time.deltaTime;
        }
        else
        {
            timerObj.text = "0";
            onProgressionStatusEvent.Invoke();
        }
    }
}
