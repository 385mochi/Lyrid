using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LyridEditor.UI;
using LyridEditor.Lanes;

namespace LyridEditor.Bars
{
    /// <summary>
    /// 全ての小節を管理するクラス
    /// </summary>
    public class BarManager : MonoBehaviour
    {
        #region Field
        private List<GameObject> barList;
        [SerializeField] private GameObject barPrefab;
        private GameObject addBarButton;
        #endregion

        #region Property
        /// <summary> 小節の幅 </summary>
        public float barWidth { get; private set; }
        /// <summary> 小節の高さ(4/4拍子でのデフォルト値) </summary>
        public float barHeight { get; private set; }

        public int barNum { get { return barList.Count; } }
        #endregion

        #region Methods
        private void Start()
        {
            barList = new List<GameObject>();
            barWidth = 10.0f;
            barHeight = 8.0f;
            addBarButton = GameObject.Find("AddBarButton");
        }

        /// <summary>
        /// 小節を追加するメソッド
        /// </summary>
        public void AddBar()
        {
            // 新たに小節を生成
            GameObject barObj = Instantiate(barPrefab, this.transform);
            barList.Add(barObj);

           // 生成した小節を最後尾の小節の後ろに移動
            if (barList.Count > 1)
            {
                barObj.transform.localPosition = new Vector3(
                    0,
                    barList[barList.Count-2].transform.localPosition.y + barList[barList.Count-2].GetComponent<Bar>().absoluteHeight,
                    0
                );
            }

            // 前の小節がない時はデフォルト値で初期化
            if (barList.Count <= 1)
            {
                barObj.GetComponent<Bar>().Init(barList.Count, 120, 4, 4, 4, barHeight);
            }
            // 前の小節があるときはその小節の設定を反映
            else
            {
                Bar latestBar = barList[barList.Count-2].GetComponent<Bar>();
                barObj.GetComponent<Bar>().Init(barList.Count, latestBar.bpm, latestBar.top, latestBar.bottom, latestBar.divNum, barHeight);
            }

            // レーンを更新する
            GameObject.Find("Lanes").GetComponent<LaneManager>().AddBar();

            // 小節追加ボタンを調整
            AdjustAddBarButton();
        }

        /// <summary>
        /// 小節の設定を更新する
        /// </summary>
        /// <param name="barId"> 小節の id </param>
        /// <param name="bpm"> BPM </param>
        /// <param name="top"> 拍子の分子 </param>
        /// <param name="bottom"> 拍子の分母 </param>
        /// <param name="divNum"> 1拍あたりの分割数 </param>
        public void UpdateBar(int barId, int bpm, int top, int bottom, int divNum)
        {
            //  id がリスト外のときは何もしない
            if (barId < 0 || barList.Count <= barId)
            {
                return;
            }

            // 設定を反映し、高さの差分を取得して指定 id 以降の小節の位置を調整する
            Bar bar = barList[barId].GetComponent<Bar>();
            float prevHeight = bar.absoluteHeight;
            bar.UpdateBar(bpm, top, bottom, divNum);
            float currHeight = bar.absoluteHeight;
            bar.transform.position += new Vector3(0, (currHeight - prevHeight) * 0.5f, 0);
            for (int i = barId + 1; i < barList.Count; i++)
            {
                barList[i].transform.position += new Vector3(0, (currHeight - prevHeight), 0);
            }

            // 小節追加ボタンを調整
            AdjustAddBarButton();
        }

        /// <summary>
        /// Bars 基準のローカル座標から小節と分割線の ID に変換する
        /// </summary>
        /// <param name="localPos"> Bars 基準のローカル座標 </param>
        /// <returns> 小節と分割線の ID </returns>
        public (int, int)? LocalPosToBarLineId(Vector3 localPos)
        {
            // 小節外の場合は null を返す
            if (localPos.x < -barWidth * 0.5 || barWidth * 0.5 < localPos.x)
            {
                return null;
            }

            // 各小節について、最も近い分割線を探す
            for (int i = 0; i < barList.Count; i++)
            {
                Bar bar = barList[i].GetComponent<Bar>();
                int? lineId = bar.LocalPosToLineId(localPos);

                // 見つかった場合は、小節と分割線の ID を返す
                if (lineId != null)
                {
                    return (i, lineId!.Value);
                }
            }

            return null;
        }

        /// <summary>
        /// 小節と分割線の ID から Bars 基準のローカル y 座標に変換する
        /// </summary>
        /// <param name="barId"> 小節の ID </param>
        /// <param name="lineId"> 分割線の ID </param>
        /// <returns> Bars 基準のローカル y 座標 </returns>
        public float? BarLineIdToLocalPosY(int barId, int lineId)
        {
            // barId が -1 の場合は一番最後の小節の末端の y 座標を返す
            if (barId == -1)
            {
                Bar bar = barList[barList.Count-1].GetComponent<Bar>();
                float? posY = bar.LineIdToLocalPosY(-1);
                return posY;
            }
            else
            {
                Bar bar = barList[barId].GetComponent<Bar>();
                float? posY = bar.LineIdToLocalPosY(lineId);
                return posY;
            }
        }

        /// <summary>
        /// 小節追加ボタンを調整するメソッド
        /// /// </summary>
        private void AdjustAddBarButton()
        {
            Bar lastBar = barList[barList.Count-1].GetComponent<Bar>();
            addBarButton.transform.position = new Vector3(
                addBarButton.transform.position.x,
                barList[barList.Count-1].transform.position.y + lastBar.height * (float)lastBar.top / (float)lastBar.bottom *  0.5f,
                addBarButton.transform.position.z
            );
        }
        #endregion
    }
}