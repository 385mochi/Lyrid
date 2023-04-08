using System.Collections;
using System.Collections.Generic;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// 移動のためのインターフェース
    /// </summary>
    public interface IMovable
    {
        bool Move(float time);
    }
}