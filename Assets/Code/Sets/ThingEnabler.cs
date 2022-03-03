// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

namespace RoboRyanTron.Unite2017.Sets
{
    public class ThingEnabler : MonoBehaviour
    {
        public ThingRuntimeSet Set;

        public void EnableAll()
        {
            // Loop backwards since the list may change when disabling
            for (int i = Set.Items.Count-1; i >= 0; i--)
            {
                Set.Items[i].gameObject.SetActive(true);
            }
        }

        public void EnableRandom()
        {
            int index = Random.Range(0, Set.Items.Count);
            Set.Items[index].gameObject.SetActive(true);
        }
    }
}