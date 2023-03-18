using System.Collections;
using System.Collections.Generic;

namespace Lyrid.Charts
{
    // 譜面から読み込んだノーツのパラメータを落とし込むクラス
    public class NoteParam
    {
        public ElementType type;   // 要素の属性
        public int laneNum;        // レーン番号
        public float var_1;        // ノーツならサイズ、それ以外なら更新値
        public float var_2;        // スライドノートなら制御点の x 座標
        public float var_3;        // スライドノートなら制御点の z 座標
        public int id;             // スライドノートの識別番号
        public int connectionType; // スライドノート、レーン操作の接続タイプ

        public NoteParam()
        {
        }

        public string Info()
        {
            string info = "";
            info += type.ToString();
            info += " lane:" + laneNum;
            info += " v1:" + var_1;
            info += " v2:" + var_2;
            info += " v3:" + var_3;
            info += " id:" + id;
            info += " ct:" + connectionType;
            return info;
        }
    }
}