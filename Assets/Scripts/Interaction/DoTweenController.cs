using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DoTweenController : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetLocation = Vector3.zero;

    [Range(0.5f, 10.0f), SerializeField]
    private float moveDuration = 1.0f;

    [SerializeField]
    private Ease moveEase = Ease.Linear;

    [SerializeField]
    private DoTweenType doTweenType = DoTweenType.MovementOneWay;

    [SerializeField]
    private Color targetColor;

    [Range(1.0f, 500.0f), SerializeField]
    private float scaleMultiplier = 3.0f;

    [Range(1.0f, 10.0f), SerializeField]
    private float colorChangeDuration;

    private enum DoTweenType
    {
        MovementOneWay,
        MovementTwoWay,
        MovementOneWayColorChange,
        MovementTwoWayWithSequence,
        MovementOneWayColorChangeAndScale

    }

    // Start is called before the first frame update
    void Start()
    {
        if(doTweenType == DoTweenType.MovementOneWay)
        {
            if (targetLocation == Vector3.zero)
                targetLocation = transform.position;

            transform.DOMove(targetLocation, moveDuration).SetEase(moveEase);
        }
        else if (doTweenType == DoTweenType.MovementTwoWay)
        {
            if (targetLocation == Vector3.zero)
                targetLocation = transform.position;
            StartCoroutine(MoveWithBothWays());            
        }
        else if (doTweenType == DoTweenType.MovementTwoWayWithSequence)
        {
            if (targetLocation == Vector3.zero)
                targetLocation = transform.position;

            Vector3 originalLocation = transform.position;
            DOTween.Sequence()
                .Append(transform.DOMove(targetLocation, moveDuration).SetEase(moveEase))
                .Append(transform.DOMove(originalLocation, moveDuration).SetEase(moveEase));
        }
        else if (doTweenType == DoTweenType.MovementOneWayColorChange)
        {
            if (targetLocation == Vector3.zero)
                targetLocation = transform.position;
            DOTween.Sequence()
                .Append(transform.DOMove(targetLocation, moveDuration).SetEase(moveEase))
                .Append(transform.GetComponent<Renderer>().material
                .DOColor(targetColor, colorChangeDuration).SetEase(moveEase));
        }
        else if (doTweenType == DoTweenType.MovementOneWayColorChangeAndScale)
        {
            if (targetLocation == Vector3.zero)
                targetLocation = transform.position;
            DOTween.Sequence()
                .Append(transform.DOMove(targetLocation, moveDuration).SetEase(moveEase))
                .Append(transform.DOScale(scaleMultiplier, moveDuration / 2.0f).SetEase(moveEase))
                .Append(transform.GetComponent<Renderer>().material
                .DOColor(targetColor, colorChangeDuration).SetEase(moveEase));
        }

    }

    private IEnumerator MoveWithBothWays()
    {
        Vector3 originalLocation = transform.position;
        transform.DOMove(targetLocation, moveDuration).SetEase(moveEase);
        yield return new WaitForSeconds(moveDuration);
        transform.DOMove(originalLocation, moveDuration).SetEase(moveEase);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
