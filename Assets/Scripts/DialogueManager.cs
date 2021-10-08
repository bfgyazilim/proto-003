using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Image uiImage;
    public Animator animator;

    public Transform target;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        //uiImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 20);
        if(uiImage != null)
        {
            uiImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(UIUtils.instance.GetScreenXPositionOfObject(target.position), UIUtils.instance.GetScreenYPositionOfObject(target.position));
            //Debug.Log("======================== Dialog anchored position " + uiImage.GetComponent<RectTransform>().anchoredPosition);
        }
        else
        {
            Debug.Log("UI Image NULL");
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("isOpen", false);

    }

}
