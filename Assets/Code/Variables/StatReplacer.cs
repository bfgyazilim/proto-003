﻿// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace RoboRyanTron.Unite2017.Variables
{
    public class StatReplacer : MonoBehaviour
    {
        public Text Text;

        public FloatVariable Variable;

        public bool AlwaysUpdate;
        
        private void OnEnable()
        {
            Text.text = Variable.Value.ToString();
        }

        private void Update()
        {
            if (AlwaysUpdate)
            {
                Text.text = Variable.Value.ToString();
            }
        }
    }
}