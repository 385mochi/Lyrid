using System.Collections;
using System.Collections.Generic;

namespace Lyrid.GameScene.Notes
{
    // 判定のためのインターフェース
    public interface IJudgeable
    {
        #region Methods
        JudgementType Judge();
        #endregion
    }
}