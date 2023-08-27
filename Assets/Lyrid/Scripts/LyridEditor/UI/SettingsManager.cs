using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LyridEditor.UI
{
    /// <summary>
    /// 各設定を管理するクラス
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        public GameObject barSettingsObj;
        public GameObject barLineSettingsObj;

        public void ClearAll()
        {
            if (barSettingsObj.activeSelf) barSettingsObj.SetActive(false);
            if (barLineSettingsObj.activeSelf) barLineSettingsObj.SetActive(false);
        }
    }
}