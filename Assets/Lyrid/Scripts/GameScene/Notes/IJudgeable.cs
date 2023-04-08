using System.Collections;
using System.Collections.Generic;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// 判定のためのインターフェース
    /// </summary>
    public interface IJudgeable
    {
        JudgementType Judge(float time, int touchType, float posX);
    }
}