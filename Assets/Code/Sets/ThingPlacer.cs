// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

namespace RoboRyanTron.Unite2017.Sets
{
    public class ThingPlacer : MonoBehaviour
    {
        static int nextItem = 0;
        public bool ResetThings;
        public FloatVariable LastEnabledIndex;
        public FloatVariable Cash;
        public float cost;
        public float offSet = 0.3f;
        public GameObject go;

        [SerializeField]
        Vector3 pos;

        public void Place()
        {
            if (Cash.Value > cost)
            {
                // apply the offset based on ScriptableObject Offsetter...
                pos.z += offSet;
                Instantiate(go, pos, Quaternion.identity);
                Cash.ApplyChange(-cost);
            }
        }
    }
}