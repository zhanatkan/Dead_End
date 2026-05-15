using System;
using System.Collections.Generic;
using Game.Scripts.Settings.Sprites;
using Game.Scripts.Base.Services.AssetManagement;
using UnityEngine;

namespace Game.Scripts.Base.Services.Bundles
{
    public class BundleInfo
    {
        public string Path;
        public HashSet<Type> WindowTypes;
        public HashSet<Type> SpriteSetupTypes;
    }

    public static class BundlesInfo
    {
        static readonly List<BundleInfo> BundleInfos = new()
        {
            new BundleInfo
            {
                Path = AssetsPath.BundlesCommonPath,
                WindowTypes = new HashSet<Type>()
                {
                    
                },
                SpriteSetupTypes = new HashSet<Type>()
                {
                    
                }
            },
            new BundleInfo
            {
                Path = AssetsPath.BundlesMainMenuPath,
                WindowTypes = new HashSet<Type>()
                {
                    
                },
                SpriteSetupTypes = new HashSet<Type>()
                {
                    
                }
            },
            new BundleInfo
            {
                Path = AssetsPath.BundlesMainGamePath,
                WindowTypes = new HashSet<Type>()
                {
                    
                },
                SpriteSetupTypes = new HashSet<Type>()
                {
                    
                }
            },
        };

        public static string GetBundleName(Type type)
        {
            foreach (var bundleInfo in BundleInfos)
            {
                if ( bundleInfo.WindowTypes.Contains(type) || bundleInfo.SpriteSetupTypes.Contains(type) )
                {
                    return bundleInfo.Path;
                }
            }

            Debug.LogError($"There are no bundle contains {nameof(type)} type");
            return null;
        }
    }
}