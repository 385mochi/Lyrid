using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LyridEditor.UI;
using TMPro;

namespace LyridEditor.Bars
{
    /// <summary>
    /// 小節を管理するクラス
    /// </summary>
    public class Bar : MonoBehaviour
    {
        #region Field
        [SerializeField] private GameObject backGround;
        [SerializeField] private GameObject linePrefab;
        [SerializeField] private GameObject barLinesObj;
        [SerializeField] private TextMeshPro barNameText;
        private List<GameObject> lineList;
        #endregion

        #region Property
        public int barId { get; private set; }
        public int bpm { get; private set; }
        public int top { get; private set; }
        public int bottom { get; private set; }
        public int divNum { get; private set; }
        public float height { get; private set; }
        public float absoluteHeight { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="barId"> 小節の id </param>
        /// <param name="bpm"> BPM </param>
        /// <param name="top"> 拍子の分子 </param>
        /// <param name="bottom"> 拍子の分母 </param>
        /// <param name="divNum"> 1拍あたりの分割数 </param> <summary>
        /// <param name="height"> 4/4拍子での小節の高さ </param>
        public void Init(int barId, int bpm, int top, int bottom, int divNum, float height)
        {
            this.barId = barId;
            barNameText.text = $"#{barId}";
            gameObject.name = $"Bar{barId}";
            this.bpm = bpm;
            this.top = top;
            this.bottom = bottom;
            this.divNum = divNum;
            this.height = height;
            absoluteHeight = height * (float)top / (float)bottom;

            UpdateBar(bpm, top, bottom, divNum);
        }

        /// <summary>
        /// 小節の設定を更新
        /// </summary>
        /// <param name="bpm"> BPM </param>
        /// <param name="top"> 拍子の分子 </param>
        /// <param name="bottom"> 拍子の分母 </param>
        /// <param name="divNum"> 1拍あたりの分割数 </param>
        public void UpdateBar(int bpm, int top, int bottom, int divNum)
        {
            this.bpm = bpm;
            this.top = top;
            this.bottom = bottom;
            this.divNum = divNum;
            absoluteHeight = height * (float)top / (float)bottom;

            // 背景と小節名の位置を調整
            backGround.transform.localScale = new Vector3(
                backGround.transform.localScale.x,
                absoluteHeight,
                backGround.transform.localScale.z
            );
            barNameText.transform.localPosition = new Vector3(
                barNameText.transform.localPosition.x,
                -absoluteHeight * 0.5f,
                barNameText.transform.localPosition.z
            );

            // ラインを再描画
            if (lineList != null)
            {
                foreach (GameObject lineObj in lineList)
                {
                    Destroy(lineObj);
                }
            }
            lineList = new List<GameObject>(top * divNum + 1);
            for (int i = 0; i < top * divNum; i++)
            {
                GameObject lineObj = Instantiate(linePrefab, barLinesObj.transform);
                lineObj.name = $"Line{i + 1}";
                lineList.Add(lineObj);
                lineObj.transform.localPosition = new Vector3(0, (-0.5f + i / (float)(top * divNum)) * absoluteHeight, 0);
                lineObj.GetComponent<BarLine>().SetId(barId, i + 1);

                if (i == 0) lineObj.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 200);
                else if (i % divNum == 0) lineObj.GetComponent<SpriteRenderer>().color = new Color32(160, 160, 160, 160);
                else lineObj.GetComponent<SpriteRenderer>().color = new Color32(80, 80, 80, 80);
            }
        }

        /// <summary>
        /// 小節設定画面を表示する
        /// </summary>
        public void DisplaySetting()
        {
            SettingsManager settingsManager = GameObject.FindWithTag("Settings").GetComponent<SettingsManager>();
            GameObject barSettingsObj = settingsManager.barSettingsObj;
            settingsManager.ClearAll();
            barSettingsObj.SetActive(true);
            barSettingsObj.GetComponent<BarSettings>().Init(barId, bpm, top, bottom, divNum);
        }

        /// <summary>
        /// Bars 基準のローカル座標から分割線の ID に変換する
        /// </summary>
        /// <param name="localPos"> Bars 基準のローカル座標 </param>
        /// <returns> 分割線の ID </returns>
        public int? LocalPosToLineId(Vector3 localPos)
        {
            // 分割線の間隔
            float dist = absoluteHeight / (float)(top * divNum);
            // 最も近い分割線のインデックスを返す
            for (int i = 0; i < lineList.Count; i++)
            {
                // 分割線の y 座標
                float y = transform.localPosition.y + lineList[i].transform.localPosition.y;
                if (y - dist * 0.5f <= localPos.y && localPos.y <= y + dist * 0.5f)
                {
                    return i;
                }
            }
            // 無ければ null を返す
            return null;
        }

        /// <summary>
        /// 分割線の ID から Bars 基準のローカル y 座標に変換する
        /// </summary>
        /// <param name="lineId"> 分割線の ID </param>
        /// <returns> Bars 基準のローカル y 座標 </returns>
        public float? LineIdToLocalPosY(int lineId)
        {
            // インデックス外であれば null を返す
            if (lineId < -1 || lineList.Count <= lineId)
            {
                return null;
            }
            // lineId が -1 であれば一番上の座標を返す
            else if (lineId == -1)
            {
                float posY = transform.localPosition.y + absoluteHeight * 0.5f;
                return posY;
            }
            else
            {
                float posY = transform.localPosition.y + lineList[lineId].transform.localPosition.y;
                return posY;
            }
        }
        #endregion
    }
}