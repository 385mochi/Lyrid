using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LyridEditor.UI
{
    /// <summary>
    /// 小節の設定を管理するクラス
    /// </summary>
    public class BarSettings : MonoBehaviour
    {
        #region Field
        [SerializeField] private Text settingsLabelText;
        [SerializeField] private InputField bpmInputField;
        [SerializeField] private InputField topInputField;
        [SerializeField] private InputField bottomInputField;
        [SerializeField] private InputField divNumInputField;
        private int index;
        #endregion

        #region Methods
        public void Init(int index, int bpm, int top, int bottom, int divNum)
        {
            this.index = index;
            settingsLabelText.text = $"小節設定(#{index})";
            bpmInputField.text = bpm.ToString();
            topInputField.text = top.ToString();
            bottomInputField.text = bottom.ToString();
            divNumInputField.text = divNum.ToString();
        }
        #endregion
    }
}