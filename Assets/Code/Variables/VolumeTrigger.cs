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
    public class VolumeTrigger : MonoBehaviour
    {
        public UnityEvent TriggerEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                TriggerEvent.Invoke();
                Debug.Log("Volume Triggered");
            }
        }
    }
}