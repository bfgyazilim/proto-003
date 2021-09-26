using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    public Texture[] textures;
    public int currentTexture;
    Material[] materials1, materials2;
    Material[] matPavementL, matPavementR;
    Material[] matBg = new Material[10];

    [SerializeField] private int skinPrice;
    [SerializeField] private int colorPrice;
    [SerializeField] private int skinIncrement;
    [SerializeField] private int colorIncrement;

    [SerializeField] Button skinButton, colorButton;

    public TextMeshProUGUI skinButtonText, colorButtonText, coinText;

    public GameObject floor1, floor2, pavementL, pavementR;
    public GameObject[] bg;
    public GameObject[] sea;
    public bool overrideSettings;

    // Start is called before the first frame update
    void Start()
    {
        // Tables, or any list of elements, that you want Materials to be changed
        for (int i = 0; i < bg.Length; i++)
        {
            matBg[i] = bg[i].GetComponent<Renderer>().materials[0];
        }
        
        CheckAvailability();
        // material initialization
        materials1 = floor1.GetComponent<Renderer>().materials;
        materials2 = floor2.GetComponent<Renderer>().materials;
        //matPavementL = pavementL.GetComponent<Renderer>().materials;
        //matPavementR = pavementR.GetComponent<Renderer>().materials;
        //print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
        if (!overrideSettings)
        {
            PickRandomSea();
        }        
    }

    /// <summary>
    /// Picks a random sea object
    /// </summary>
    void PickRandomSea()
    {
        // Change Sea objects to on and off
        int rnd = Random.Range(0, 3);
        for (int i = 0; i < sea.Length; i++)
        {
            if (i == rnd)
            {
                sea[i].SetActive(true);
            }
            else
            {
                sea[i].SetActive(false);
            }
        }
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
        materials1[0].color = color1;
        materials2[0].color = color1;
        //materials[1].color = color2;
        //materials[2].color = color3;
        //materials[3].color = color4;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangeColorRandom()
    {
        if (colorPrice < UIManager.instance.GetCoins())
        {

            // screen
            materials1[0].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);            
            materials2[0].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //matPavementL[0].color = Random.ColorHSV(0f, 0.7f, 1f, 1f, 0.5f, 1f);
            //matPavementR[0].color = matPavementL[0].color;

            // muzzle
            //materials[3].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            
            //for (int i = 0; i < matBg.Length; i++)
            //{
            //  matBg[0].color = materials1[0].color;
            //}
            
            //materials[1].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //materials[2].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            //materials[3].color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            // Lion Integration
            //Analytics.Events.ContentUnlocked("Random Color" + materials[0].color.ToString(), currentTexture + 1);
            // End Lion Integration
            Debug.Log("Before DecreaseCoins called");
            UIManager.instance.DecreaseCoins(colorPrice);
            coinText.text = UIManager.instance.GetCoinsInGame().ToString();

            //Score.instance.DecrementScore(colorPrice);
            //coinText.text = Score.instance.oldScore.ToString();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void ChangeSkin()
    {
        if (skinPrice < UIManager.instance.GetCoins())
        {
            currentTexture++;
            currentTexture %= textures.Length;

            // screen
            //materials1[0].mainTexture = textures[currentTexture];
            //materials1[0].color = Color.white;
            //materials2[0].mainTexture = textures[currentTexture];
            //materials2[0].color = Color.white;

            //for (int i = 0; i < matBg.Length; i++)
            //{
            //    matBg[i].mainTexture = textures[currentTexture];
            //    matBg[i].color = Color.white;
            //}
            // bg skin
            //matBg[0].mainTexture = textures[currentTexture];
            //matBg[0].color = Color.white;

            PickRandomSea();            

            // muzzle
            //materials[4].mainTexture = textures[currentTexture];
            //materials[4].color = Color.white;
            // pavement L
            //matPavementL[0].mainTexture = textures[currentTexture];
            //matPavementL[0].color = Color.white;
            // pavement R
            //matPavementR[0].mainTexture = textures[currentTexture];
            //matPavementR[0].color = Color.white;

            // Lion Integration
            //Analytics.Events.ContentUnlocked("Skin" + textures[currentTexture + 1].name, currentTexture + 1);
            // End Lion Integration


            UIManager.instance.DecreaseCoins(skinPrice);
            coinText.text = UIManager.instance.GetCoinsInGame().ToString();

            //Score.instance.DecrementScore(skinPrice);
            //coinText.text = Score.instance.oldScore.ToString();
        }
    }

    /// <summary>
    /// Sets the UI buttons for Skin,Color Upgrades according to the Coin Amount
    /// </summary>
    public void CheckAvailability()
    {
        // Check skin button
        //if (skinPrice < Score.instance.oldScore)
        //{
            //skinButton.interactable = true;
            skinButtonText.text = skinPrice.ToString();
        //}
        //else
        //{
            //skinButton.interactable = false;
            //skinButton.enabled = false;
            skinButtonText.text = skinPrice.ToString();
        //}

        // Check color button
        //if (colorPrice < Score.instance.oldScore)
        //{
            //colorButton.interactable = true;
            colorButtonText.text = colorPrice.ToString();
        //}
        //else
        //{
            //colorButton.interactable = false;
            //colorButton.enabled = false;
            colorButtonText.text = colorPrice.ToString();
        //}
    }
}

