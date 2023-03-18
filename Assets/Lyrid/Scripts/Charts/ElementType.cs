using System.Collections;
using System.Collections.Generic;


namespace Lyrid.Charts
{
    // 譜面の各要素の属性を定義する列挙型
    public enum ElementType
    {
        Tap,     // タップノート
        Swipe,   // スワイプノート
        Flick,   // フリックノート
        Slide,   // スライドノート
        LanePos, // レーン位置変更
        LaneWid, // レーン幅変更
        Speed    // スピード変更
    }
}