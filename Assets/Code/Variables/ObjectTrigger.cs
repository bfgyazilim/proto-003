// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

namespace RoboRyanTron.Unite2017.Variables
{
    public class ObjectTrigger : MonoBehaviour
    {
        public GameObject go;

        public AudioSource AudioSource;

        public FloatVariable Variable;

        public FloatReference HighThreshold;

        private void Update()
        {
            if (Variable.Value > HighThreshold)
            {
                // Enable Object
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }
}