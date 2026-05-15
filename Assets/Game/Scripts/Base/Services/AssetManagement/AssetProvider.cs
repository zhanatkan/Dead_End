using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using Object = UnityEngine.Object;

namespace Game.Scripts.Base.Services.AssetManagement
{
    public sealed class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();
        
        [Inject]
        public AssetProvider()
        { }
        
        public GameObject Instantiate(string address, Transform parent, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(address);
            return Object.Instantiate(prefab, at, Quaternion.identity, parent);
        }

        public GameObject Instantiate(string address, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(address);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }

        public GameObject Instantiate(string address, Transform parent)
        {
            var prefab = Resources.Load<GameObject>(address);
            return Object.Instantiate(prefab, parent);
        }

        public GameObject Instantiate(string address)
        {
            var prefab = Resources.Load<GameObject>(address);
            return Object.Instantiate(prefab);
        }

        public void ReleaseAll()
        {
            foreach (var kvp in _handles)
            {
                foreach (var handle in kvp.Value)
                    Addressables.Release(handle);
            }

            _handles.Clear();
            _completedCache.Clear();
        }

        public async UniTask<T> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            if ( _completedCache.TryGetValue(assetReference.AssetGUID, out var completedHandle) )
            {
                return completedHandle.Result as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(assetReference);
            return await RunWithCacheOnComplete(handle, assetReference.AssetGUID);
        }

        public async UniTask<T> LoadAsync<T>(string address) where T : class
        {
            if ( _completedCache.TryGetValue(address, out var completedHandle) )
            {
                return completedHandle.Result as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            return await RunWithCacheOnComplete(handle, address);
        }

        public async UniTask<List<T>> LoadAllAsync<T>(string address) where T : class
        {
            if ( _completedCache.TryGetValue(address, out var completedHandle) )
            {
                return (completedHandle.Result as IList<T>)?.ToList();
            }

            var handle = Addressables.LoadAssetsAsync<T>(address);
            var result = await RunWithCacheOnComplete(handle, address);
            return result.ToList();
        }

        public async UniTask<List<T>> LoadAllAsyncDontCache<T>(string address) where T : class
        {
            var handle = Addressables.LoadAssetsAsync<T>(address);
            var result = await handle.Task;
            return result.ToList();
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
        {
            if ( !_handles.TryGetValue(key, out var resourceHandle) )
            {
                resourceHandle = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandle;
            }

            resourceHandle.Add(handle);
        }
        
        private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey)
            where T : class
        {
            handle.Completed += h => { _completedCache[cacheKey] = h; };

            AddHandle(cacheKey, handle);
            return await handle.Task;
        }
    }
}