using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button buttonShowAd;
    [SerializeField] private string androidAdID = "Rewarded_Android";
    [SerializeField] private string iosAdID = "Rewarded_iOS";
    private string adID;
    private bool adLoaded = false;  // Флаг для проверки готовности рекламы

    public GameOverUI g;
    private void Awake()
    {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosAdID : androidAdID;
        buttonShowAd.interactable = true;
    }

    private void Start()
    {
        buttonShowAd.interactable = true;
        LoadAd();
    }

    public void LoadAd()
    {
        Advertisement.Load(adID, this);
    }

    public void ShowAd()
    {
        if (adLoaded)  // Проверяем, загружена ли реклама
        {
            buttonShowAd.interactable = false;
            Advertisement.Show(adID, this);
        }
        else
        {
            Debug.Log("Ad is not loaded yet.");
        }
    }
    private IEnumerator WaitForAdToLoad()
    {
        while (!adLoaded)
        {
            Debug.Log("Waiting for ad to load...");
            yield return new WaitForSeconds(1f);
        }
        ShowAd();
    }

    public void OnButtonClicked()
    {
        StartCoroutine(WaitForAdToLoad());
    }
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adID))
        {
            buttonShowAd.onClick.AddListener(ShowAd);
            buttonShowAd.interactable = true;  // Делаем кнопку активной
            adLoaded = true;  // Устанавливаем флаг готовности рекламы
            Debug.Log("Ad is loaded and ready to show.");
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            g.RevivePlayer();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error loading Ad Unit {placementId}: {error} - {message}");
        // Перезагружаем рекламу, если произошла ошибка
        adLoaded = false;
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing Ad Unit {placementId}: {error} - {message}");
        // Можем попробовать перезагрузить рекламу после неудачи
        LoadAd();
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}