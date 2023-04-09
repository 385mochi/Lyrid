using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using CriWare;
using CriWare.Assets;

namespace Lyrid.Common
{
    /// <summary>
    /// 各種 Asset をロード管理するクラス
    /// </summary>
    public class AssetLoader
    {
        #region Constructor
        public AssetLoader(){}
        #endregion

        #region Methods
        /// <summary>
        /// 音源 (acb ファイル) をロードするメソッド
        /// </summary>
        /// <param name="key"> アドレス名 </param>
        /// <returns> acb インスタンス </returns>
        public async Task<CriAtomExAcb> LoadAudioAsync(string key)
        {
            // ファイルが存在するかチェック
            var location = Addressables.LoadResourceLocationsAsync(key);
            var locationResult = await location.Task;

            // 存在すればロード
            if (locationResult.Count != 0)
            {
                var acbAsset = await Addressables.LoadAssetAsync<CriAtomAcbAsset>(key).Task;
                acbAsset.LoadImmediate();
                return acbAsset.Handle;
            }
            return null;
        }
        #endregion
    }
}