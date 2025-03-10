using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsInitializer : MonoBehaviour,IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameID = "5722339";
    [SerializeField] private string iosGameID = "5722338";
    [SerializeField] private bool testMod = true;
    private string gameID;

    void Awake()
    {
        InitializeAds();
    }   
    public void InitializeAds()
    {
        gameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameID : androidGameID;
        Advertisement.Initialize(gameID, testMod, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        // ѕосле успешной инициализации загружаем рекламу
        FindObjectOfType<RewardedAdsButton>().LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Yes {error.ToString()} - {message}");
    }
}