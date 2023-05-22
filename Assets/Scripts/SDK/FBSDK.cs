#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE
using UnityEngine;
// using Facebook.Unity;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Facebook.Unity;

public class FBSDK : MonoBehaviour
{
    private static FBSDK instance;
    public static FBSDK Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
        FB.Init(SetInit, OnHideUnity);
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            FB.Mobile.FetchDeferredAppLinkData();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Lỗi khởi tạo của Facebook SDK");
        }
    }
    void SetInit()
    {
       
    }
    void OnHideUnity(bool isUnityShow)
    {
        if (!isUnityShow)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

}
#else
using UnityEngine;
public class FBSDK : MonoBehaviour
{ }
#endif