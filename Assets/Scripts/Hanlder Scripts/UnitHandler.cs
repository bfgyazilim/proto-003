using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitHandler : MonoBehaviour
{
    /// <summary>
    /// Give the last item to the other GameObject
    /// With DoTween Animation (DestackAnimation)
    /// </summary>
    public void HandleStackingToOther(GameObject obj)
    {

        int childCount = transform.GetChild(0).childCount;

        if (childCount != 0)
        {
            if (transform.GetChild(0).GetChild(0) != null)
            {
                Transform go = transform.GetChild(0).GetChild(childCount - 1);

                // Now that we've moved, parent to other obj now!
                go.parent = obj.transform.GetChild(0);

                // Tween here to other obj position
                DestackAnimation(go, obj);

                Debug.Log("Detached child: " + go.gameObject.name);
                //go.gameObject.SetActive(false);                    
            }
        }
    }

    /// <summary>
    /// Detach pickup object from the player to the given location in the panel transform in the inspector
    /// </summary>
    /// <param name="t"></param>
    void DestackAnimation(Transform t, GameObject obj)
    {
        Sequence spriteAnimation;

        spriteAnimation = DOTween.Sequence();

        spriteAnimation.Append(t.DOMove(obj.transform.GetChild(0).position, 0.5f)
            .SetEase(Ease.OutSine));

        /*
        spriteAnimation.Append(t.DOMove(panel.position, 0.5f)
            .SetEase(Ease.OutSine))
            .OnComplete(() => Destroy(t.gameObject));
        */
    }
}
