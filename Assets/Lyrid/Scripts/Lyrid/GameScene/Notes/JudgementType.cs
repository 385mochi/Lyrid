using System.Collections;
using System.Collections.Generic;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// 判定の種類を定義する列挙型
    /// </summary>
    public enum JudgementType
    {
        Perfect,    // 精度: 最高
        Great,      // 精度: 高
        Good,       // 精度: 中
        Bad,        // 精度: 低
        Miss,       // ミス
        None        // なにもしない
    }
}