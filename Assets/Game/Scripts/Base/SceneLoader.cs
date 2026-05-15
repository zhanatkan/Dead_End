using Cysharp.Threading.Tasks;
using Game.Scripts.Base.Services.AssetManagement;
using UnityEngine.SceneManagement;
using VContainer;

namespace Game.Scripts.Base
{
    public sealed class SceneLoader
    {
        private readonly IAssetProvider _assetProvider;
        public SceneName CurrentSceneName { get; private set; }

        [Inject]
        public SceneLoader(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Load(SceneName sceneName)
        {
            await LoadScene(sceneName);
        }

        private async UniTask LoadScene(SceneName sceneName)
        {
            var nextSceneName = sceneName.ToString();
            await SceneManager.LoadSceneAsync(nextSceneName).ToUniTask();
            _assetProvider.ReleaseAll();
            CurrentSceneName = sceneName;
        }
    }
}