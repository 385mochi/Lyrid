using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LyridEditor.UI;

namespace LyridEditor
{
    /// <summary>
    /// 小節を分割する線のクラス
    /// </summary>
    public class BarLine : MonoBehaviour
    {
        [SerializeField] SpriteRenderer lineSprite;
        public int barId { get; private set; }
        public int lineId  { get; private set; }

        /// <summary>
        /// 分割線の ID を指定する
        /// </summary>
        /// <param name="barId"> 小節の識別番号 </param>
        /// <param name="lineId"> 分割線の識別番号 </param>
        public void SetId(int barId, int lineId)
        {
            this.barId = barId;
            this.lineId = lineId;
        }

        public void DisplaySetting()
        {
            SettingsManager settingsManager = GameObject.FindWithTag("Settings").GetComponent<SettingsManager>();
            settingsManager.ClearAll();
            GameObject barLineSettingsObj = settingsManager.barLineSettingsObj;
            barLineSettingsObj.SetActive(true);
            barLineSettingsObj.GetComponent<BarLineSettings>().Init(barId, lineId);
        }
    }
}