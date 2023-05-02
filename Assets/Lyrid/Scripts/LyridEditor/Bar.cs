using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LyridEditor.UI;

namespace LyridEditor
{
    /// <summary>
    /// 小節を管理するクラス
    /// </summary>
    public class Bar : MonoBehaviour
    {
        #region Field
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject barLinesObj;
        [SerializeField] private TextMeshPro barNameText;
        private List<GameObject> lineList;
        private float widthRate = 0.142f;
        #endregion

        public int index { get; private set; }
        public int bpm { get; private set; }
        public int top { get; private set; }
        public int bottom { get; private set; }
        public int divNum { get; private set; }

        #region Methods
        public void Init(int index, int bpm, int top, int bottom, int divNum)
        {
            this.index = index;
            barNameText.text = $"#{index}";

            this.bpm = bpm;
            this.top = top;
            this.bottom = bottom;
            this.divNum = divNum;

            // ラインを描画
            lineList = new List<GameObject>(top * divNum + 1);
            for (int i = 0; i < top * divNum + 1; i++)
            {
                GameObject lineObj = Instantiate(linePrefab, barLinesObj.transform);
                lineList.Add(lineObj);
                lineObj.transform.localPosition = new Vector3(0, -0.5f + i / (float)(top * divNum), 0);
                if (i == 0 || i == top * divNum) lineObj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
                else if (i % divNum == 0) lineObj.GetComponent<SpriteRenderer>().color = new Color32(160, 160, 160, 160);
                else lineObj.GetComponent<SpriteRenderer>().color = new Color32(80, 80, 80, 80);
            }
        }

        public void DisplaySetting()
        {
            SettingsManager settingsManager = GameObject.FindWithTag("Settings").GetComponent<SettingsManager>();
            GameObject barSettingsObj = settingsManager.barSettingsObj;
            barSettingsObj.SetActive(true);
            barSettingsObj.GetComponent<BarSettings>().Init(index, bpm, top, bottom, divNum);
        }
        #endregion
    }
}