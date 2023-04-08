using System.Collections;
using System.Collections.Generic;

namespace Lyrid.GameScene.Charts
{
    /// <summary>
    /// 譜面から読み込んだノーツのパラメータを保持するクラス
    /// </summary>
    public class NoteParam
    {
        #region Property
        /// <summary> 要素の属性 </summary>
        public ElementType type { get; set; }
        /// <summary> レーン番号 </summary>
        public int laneNum { get; set; }
        /// <summary> ノーツならサイズ、それ以外なら更新値 </summary>
        public float var_1 { get; set; }
        /// <summary>  スライドノートなら制御点の x 座標の割合 </summary>
        public float var_2 { get; set; }
        /// <summary> スライドノートなら制御点の z 座標の割合 </summary>
        public float var_3 { get; set; }
        /// <summary> 予備用変数 (スライドノートの先頭ノートの判定時間) </summary>
        public float var_4 { get; set; }
        /// <summary> スライドノート、レーン操作の接続タイプ </summary>
        public int connectionType { get; set; }
        #endregion

        #region Constructor
        public NoteParam(){}
        #endregion

        #region Methods
        /// <summary>
        /// パラメータの情報を log に表示するメソッド
        /// </summary>
        /// <returns> パラメータ一覧の文字列 </returns>
        public string Info()
        {
            string info = "";
            info += type.ToString();
            info += " lane:" + laneNum;
            info += " v1:" + var_1;
            info += " v2:" + var_2;
            info += " v3:" + var_3;
            info += " v4:" + var_4;
            info += " ct:" + connectionType;
            return info;
        }
        #endregion
    }
}