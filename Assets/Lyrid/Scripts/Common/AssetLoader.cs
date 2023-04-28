using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using CriWare;
using CriWare.Assets;

namespace Lyrid.Common
{
    /// <summary>
    /// Addressables で管理された各種 Asset のロードを管理するクラス
    /// </summary>
    public class AssetLoader: MonoBehaviour
    {
        #region Field
        private Dictionary<string, AsyncOperationHandle> handleDict;
        #endregion

        #region Methods
        private void Start()
        {
            handleDict = new Dictionary<string, AsyncOperationHandle>();
        }

        /// <summary>
        /// 指定されたアドレスに紐づくアセットが存在するかどうか判定するメソッド
        /// </summary>
        /// <param name="key"> アドレス名 </param>
        /// <returns> アセットが存在するかどうか </returns>
        private async Task<bool> Exists(string key)
        {
            var handle = Addressables.LoadResourceLocationsAsync(key);
            await handle.Task;
            return (handle.Result != null && 0 < handle.Result.Count);
        }

        /// <summary>
        /// handleDict の全ての handle を解放するメソッド
        /// </summary>
        public void ReleaseAll()
        {
            foreach (var (key, handle) in handleDict)
            {
                Addressables.Release(handle);
            }
            handleDict = new Dictionary<string, AsyncOperationHandle>();
        }

        /// <summary>
        /// 指定されたアドレスに紐づく音源 (acb ファイル) をロードするメソッド
        /// </summary>
        /// <param name="key"> アドレス名 </param>
        /// <returns> ロードした acb ファイル </returns>
        public async Task<CriAtomExAcb> LoadAudioAsync(string key)
        {
            bool exists = await Exists(key);
            if (!exists) return null;
            if (handleDict.ContainsKey(key))
            {
                Addressables.Release(handleDict[key]);
                handleDict.Remove(key);
            }
            var handle = Addressables.LoadAssetAsync<CriAtomAcbAsset>(key);
            var acbAsset = await handle.Task;
            if (!acbAsset.Loaded) acbAsset.LoadImmediate();
            handleDict.Add(key, handle);
            return acbAsset.Handle;
        }

        /// <summary>
        /// 指定されたアドレスに紐づく Sprite をロードするメソッド
        /// </summary>
        /// <param name="key"> アドレス名 </param>
        /// <returns> ロードした Sprite </returns>
        public async Task<Sprite> LoadSpriteAsync(string key)
        {
            bool exists = await Exists(key);
            if (!exists) return null;
            if (handleDict.ContainsKey(key))
            {
                Addressables.Release(handleDict[key]);
                handleDict.Remove(key);
            }
            var handle = Addressables.LoadAssetAsync<Sprite>(key);
            var sprite = await handle.Task;
            handleDict.Add(key, handle);
            return sprite;
        }

        /// <summary>
        /// 指定されたアドレスに紐づく TextAsset をロードするメソッド
        /// </summary>
        /// <param name="key"> アドレス名 </param>
        /// <returns> ロードした TextAsset </returns>
        public async Task<TextAsset> LoadTextAssetAsync(string key)
        {
            bool exists = await Exists(key);
            if (!exists) return null;
            if (handleDict.ContainsKey(key))
            {
                Addressables.Release(handleDict[key]);
                handleDict.Remove(key);
            }
            var handle = Addressables.LoadAssetAsync<TextAsset>(key);
            var textAsset = await handle.Task;
            handleDict.Add(key, handle);
            return textAsset;
        }
        #endregion
    }
}