using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    Material[] materials;

    [SerializeField] private int skinPrice;
    [SerializeField] private int colorPrice;
    [SerializeField] private int skinIncrement;
    [SerializeField] private int colorIncrement;

    [SerializeField] Button skinButton, colorButton;

    public TextMeshProUGUI skinButtonText, colorButtonText, coinText;


    // Start is called before the first frame update
    void Start()
    {
        CheckAvailability();
            // material initialization
        materials = GetComponent<Renderer>().materials;
        print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color1"></param>
    /// <param name="color2"></param>
    /// <param name="color3"></param>
    /// <param name="color4"></param>
    public void ChangeColor(Color color1, Color color2, Color color3, Color color4)
    {
        int i = 0;
        materials[0].color = color1;

        //materials[1].color = color2;
        //materials[2].color = color3;
        //materials[3].color = color4;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangeColorRandom()
    {
        if (colorPrice < Score.instance.oldScore)
        {         
            materials[0].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //materials[1].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //materials[2].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //materials[3].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            Score.instance.DecrementScore(colorPrice);
            coinText.text = Score.instance.oldScore.ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void ChangeSkin()
    {
        if (skinPrice < Score.instance.oldScore)
        {
            currentTexture++;
            currentTexture %= textures.Length;
            materials[0].mainTexture = textures[currentTexture];
            materials[0].color = Color.white;

            Score.instance.DecrementScore(skinPrice);
            coinText.text = Score.instance.oldScore.ToString();
        }
    }

    /// <summary>
    /// Sets the UI buttons for Skin,Color Upgrades according to the Coin Amount
    /// </summary>
    public void CheckAvailability()
    {
        // Check skin button
        if(skinPrice < Score.instance.oldScore)
        {
            //skinButton.interactable = true;
            skinButtonText.text = skinPrice.ToString();
        }
        else
        {
            //skinButton.interactable = false;
            //skinButton.enabled = false;
            skinButtonText.text = skinPrice.ToString();
        }

        // Check color button
        if (colorPrice < Score.instance.oldScore)
        {
            //colorButton.interactable = true;
            colorButtonText.text = colorPrice.ToString();
        }
        else
        {
            //colorButton.interactable = false;
            //colorButton.enabled = false;
            colorButtonText.text = colorPrice.ToString();
        }
    }
}
