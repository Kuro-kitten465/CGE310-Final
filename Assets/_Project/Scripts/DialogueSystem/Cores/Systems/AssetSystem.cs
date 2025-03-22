using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Debug;

namespace KuroNovel.Core
{
    public class AssetSystem
    {
        private readonly Dictionary<string, AsyncOperationHandle> _loadedAssets = new();

        public async Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            string key = assetReference.AssetGUID;
            
            if (_loadedAssets.TryGetValue(key, out var existingHandle) && existingHandle.IsValid())
            {
                return (T)existingHandle.Result;
            }
            
            try
            {
                AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedAssets[key] = handle;
                    return handle.Result;
                }
                else
                {
                    LogError($"[KuroNovel] Failed to load asset: {key}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        public void ReleaseAsset(string key)
        {
            if (_loadedAssets.TryGetValue(key, out var handle) && handle.IsValid())
            {
                Addressables.Release(handle);
                _loadedAssets.Remove(key);
            }
        }

        public void ReleaseAllAssets()
        {
            foreach (var handle in _loadedAssets.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            
            _loadedAssets.Clear();
        }
    }
}
