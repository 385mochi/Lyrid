using System.Collections;
using System.Collections.Generic;

namespace Lyrid.GameScene.Notes
{
    // 移動のためのインターフェース
    public interface IMovable
    {
        #region Methods
        void Move(float rate);
        #endregion
    }
}