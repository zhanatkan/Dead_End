using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Scripts.Base.Services.ABTest
{
    public class MockABTestService : IABTestService
    {
        private readonly Dictionary<string, string> _allTests = new()
        {
            
        };

        public void Init()
        {
            Debug.Log(string.Join(", ", _allTests.Select(kv => $"{kv.Key}: {kv.Value}")));
        }

        public bool CheckTestGroup(string testName, string groupName)
        {
            if ( !_allTests.ContainsKey(testName) )
            {
                return false;
            }
            
            return _allTests[testName] == groupName;
        }
    }
}