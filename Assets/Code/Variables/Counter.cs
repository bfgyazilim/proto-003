using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;
using UnityEngine.Events;

public class Counter : MonoBehaviour
{
    public FloatVariable RemainingBalls;
    public UnityEvent CountZeroEvent;

    public bool ResetRemainingBalls;
    public FloatReference StartingRemainingBalls;

    // Start is called before the first frame update
    void Start()
    {
        if (ResetRemainingBalls)
            RemainingBalls.SetValue(StartingRemainingBalls);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecrementCounter()
    {
        if(RemainingBalls.Value <= 0)
        {
            CountZeroEvent.Invoke();
        }
    }
}
