using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Scripts.Base.Services.AssetManagement
{
    public interface IAssetProvider
    {
        GameObject Instantiate(string address, Transform parent, Vector3 at);
        GameObject Instantiate(string address, Vector3 at);
        GameObject Instantiate(string address, Transform parent);
        GameObject Instantiate(string address);
        
        
        UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class;
        UniTask<T> LoadAsync<T>(string address) where T : class;
        UniTask<List<T>> LoadAllAsync<T>(string address) where T : class;
        UniTask<List<T>> LoadAllAsyncDontCache<T>(string address) where T : class;

        void ReleaseAll();
    }
}