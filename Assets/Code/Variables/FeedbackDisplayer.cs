using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackDisplayer : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
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

    /// <summary>
    /// Show feedback upon object collection 
    /// </summary>
    /// <param name="collectible"></param>
    public void ShowFeedbackTextGeneric()
    {
        Debug.Log("ShowFeedbackTextGeneric called");
        //AudioManager.instance.PlaySFX(1);
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
        while (feedbackText.rectTransform.anchoredPosition.y < feedbacktextPos.y)
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

        if (feedbackText.rectTransform.anchoredPosition.y >= feedbacktextPos.y)
        {
            // fade out and reset position
            feedbackText.color = new Color(0, 0, 0, 0);
            feedbackText.rectTransform.anchoredPosition = new Vector2(0, 0);
            //Debug.Log("FeedbackText NOT CHANGED anchoredposition" + feedbackText.rectTransform.anchoredPosition);
        }
        //yield return 0;
    }
}
