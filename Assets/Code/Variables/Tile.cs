using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highlight;


    public void Init(bool isOffset)
    {
        spriteRenderer.color = isOffset ? offsetColor : baseColor;
    }

    private void OnMouseEnter()
    {
#if UNITY_EDITOR

        highlight.SetActive(true);
#endif
    }

    private void OnMouseExit()
    {
#if UNITY_EDITOR

        highlight.SetActive(false);
#endif
    }
}
