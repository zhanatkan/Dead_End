#if UNITY_WEBGL && GAME_PUSH
using GamePush;
using UnityEngine;

namespace Game.Scripts.Base.Services.ABTest
{
    public class GPABTestService : IABTestService
    {
        public void Init()
        {
            Debug.Log(GP_Experiments.Map());
        }

        public bool CheckTestGroup(string testName, string groupName)
        {
            return GP_Experiments.Has(testName, groupName);
        }
    }
}
#endif