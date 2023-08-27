using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LyridEditor.UI
{
    /// <summary>
    /// 小節のラインの設定を管理するクラス
    /// </summary>
    public class BarLineSettings : MonoBehaviour
    {
        #region Field
        [SerializeField] private Text settingsLabel;
        #endregion

        #region Methods
        public void Init(int barId, int lineId)
        {
            settingsLabel.text = $"ライン設定 (#{barId}-{lineId})";
        }
        #endregion
    }
}