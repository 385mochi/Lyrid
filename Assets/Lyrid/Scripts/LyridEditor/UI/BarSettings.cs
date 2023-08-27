using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LyridEditor.Bars;
using TMPro;

namespace LyridEditor.UI
{
    /// <summary>
    /// 小節の設定を管理するクラス
    /// </summary>
    public class BarSettings : MonoBehaviour
    {
        #region Field
        [SerializeField] private BarManager barManager;
        [SerializeField] private Text settingsLabelText;
        [SerializeField] private InputField bpmInputField;
        [SerializeField] private InputField topInputField;
        [SerializeField] private InputField bottomInputField;
        [SerializeField] private InputField divNumInputField;
        private int barId;
        #endregion

        #region Methods
        public void Init(int barId, int bpm, int top, int bottom, int divNum)
        {
            this.barId = barId;
            settingsLabelText.text = $"小節設定(#{barId})";
            bpmInputField.text = bpm.ToString();
            topInputField.text = top.ToString();
            bottomInputField.text = bottom.ToString();
            divNumInputField.text = divNum.ToString();
        }

        public void UpdateBar()
        {
            barManager.UpdateBar(
                barId-1,
                int.Parse(bpmInputField.text),
                int.Parse(topInputField.text),
                int.Parse(bottomInputField.text),
                int.Parse(divNumInputField.text)
            );
        }
        #endregion
    }
}