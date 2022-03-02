// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace RoboRyanTron.Unite2017.Variables
{
    public class VariableObjectEnabler : MonoBehaviour
    {
        public AudioSource AudioSource;

        public FloatVariable Variable;

        public FloatReference VariableThreshold;

        public UnityEvent PlaceholderEvent;


        private void Update()
        {
            if (Variable.Value >= VariableThreshold)
            {
                if (!AudioSource.isPlaying)
                    AudioSource.Play();
                PlaceholderEvent.Invoke();
            }
            else
            {
                // do nothing
            }
        }
    }
}