using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimate : MonoBehaviour
{
    Transform panel;
    Sequence spriteAnimation;

    // Start is called before the first frame update
    void Start()
    {
        AnimateSprite();
    }


    void AnimateSprite()
    {
        panel = GameObject.FindGameObjectWithTag("banknoteP").transform;
        spriteAnimation = DOTween.Sequence();

        spriteAnimation.Append(transform.DOMove(panel.position, 1.5f)
            .SetEase(Ease.OutSine))
            .OnComplete(() => Destroy(gameObject));
    }
}
