using UnityEngine;
using System.Collections;
using Facebook.Unity;

public class Analytics : MonoBehaviour
{
    void Awake()
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }

        FB.Init(FBInitCallback);
    }

    private void FBInitCallback()
    {
        if(FB.IsInitialized)
        {
            FB.ActivateApp();
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if(!paused)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
        }
    }
}
