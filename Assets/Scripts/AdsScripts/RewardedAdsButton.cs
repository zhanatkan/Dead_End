using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button buttonShowAd;
    [SerializeField] private string androidAdID = "Rewarded_Android";
    [SerializeField] private string iosAdID = "Rewarded_iOS";
    private string adID;
    private bool adLoaded = false;  

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
        if (adLoaded)  
        {
            buttonShowAd.interactable = false;
            Advertisement.Show(adID, this);
        }
    }
    private IEnumerator WaitForAdToLoad()
    {
        while (!adLoaded)
        {
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
            buttonShowAd.interactable = true; 
            adLoaded = true;  
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
        adLoaded = false;
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error showing Ad Unit {placementId}: {error} - {message}");
        LoadAd();
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}